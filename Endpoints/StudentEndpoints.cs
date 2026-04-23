using System;
using KpitApi.data;
using KpitApi.models;
using KpitApi.records;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication;

namespace KpitApi.Endpoints;

public static class StudentEndpoints
{
    public static void AddStudentEndpoints(this WebApplication app)
    {
        var studentGroup = app.MapGroup("/student");

        studentGroup.MapGet("/", async (KpitStoreContext context) =>
        {
            var students = await context.Students.Select(s => new DisplayStudentDto(
                s.Id, s.Name, s.Email, s.StudentProfile!.Bio, s.Department!.Name, s.Courses.Select(c => c.Name).ToArray()
            )).ToListAsync();

            return Results.Ok(students);
        });

        studentGroup.MapGet("/{id}", async (int id, KpitStoreContext context) =>
        {
            var studentDetails = await context.Students
                .Where(s => s.Id == id)
                .Select(s => new DisplayStudentDto(
                    s.Id,
                    s.Name,
                    s.Email,
                    s.StudentProfile != null ? s.StudentProfile.Bio : "not found",
                    s.Department != null ? s.Department.Name : "not found",
                    s.Courses.Select(c => c.Name).ToArray()
                ))
                .FirstOrDefaultAsync();

            if (studentDetails == null) return Results.NotFound("Student doesn't exist.");

            return Results.Ok(studentDetails);
        });

        studentGroup.MapPost("", async (AddStudentDto studentDto, KpitStoreContext context) =>
        {
            var department = await context.Departments.FindAsync(studentDto.DepartmentId);
            if (department == null) return Results.NotFound("Department not found");
            var student = new Student();
            var studentProfile = new StudentProfile();

            student.Name = studentDto.Name;
            student.Email = studentDto.Email;
            student.DepartmentId = department.Id;
            student.Department = department;

            studentProfile.Bio = studentDto.Bio;
            student.StudentProfile = studentProfile;


            await context.AddAsync(student);
            await context.SaveChangesAsync();

            return Results.Ok("Student added successfully");
        });

        studentGroup.MapPut("/{id}", async (int id, AddStudentDto studentDto, KpitStoreContext context) =>
        {
            var student = await context.Students.FindAsync(id);

            if (student == null) return Results.NotFound("Student not found");

            var department = await context.Departments.FindAsync(studentDto.DepartmentId);

            if (department == null) return Results.NotFound("Department doesn't exist.");

            student.Name = studentDto.Name;
            student.Email = studentDto.Email;
            student.DepartmentId = studentDto.DepartmentId;
            student.Department = department;
            student.StudentProfile?.Bio = studentDto.Bio;

            await context.SaveChangesAsync();
            return Results.Ok("Modified student");
        });

        studentGroup.MapDelete("/{id}", async (int id, KpitStoreContext context) =>
        {
            await context.Students
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();

            return Results.Ok("Student deleted sucessfully.");

        });

        studentGroup.MapGet("/{id}/course/", async (int id, KpitStoreContext context) =>
        {
            var courses = await context.Students
                .Where(s => s.Id == id)
                .SelectMany(s => s.Courses)
                .Select(c => new DisplayCourseDto(
                    c.Id, c.Name
                ))
                .ToArrayAsync();

            return Results.Ok(courses);
        });

        studentGroup.MapPost("/{studentId}/course/{courseId}",
            async (int studentId, int courseId, KpitStoreContext context) =>
        {
            var student = await context.Students
                .Include(s => s.Courses)
                .Where(s => s.Id == studentId)
                .FirstOrDefaultAsync();
            if (student == null) return Results.NotFound("Student doesn't exist");
            var course = await context.Courses.FindAsync(courseId);
            if (course == null) return Results.NotFound("Course doesn't exist");

            if (student.Courses.Any(c => c.Id == courseId))
                return Results.BadRequest("Course already enrolled.");

            student.Courses.Add(course);


            await context.SaveChangesAsync();
            return Results.Ok("Course added.");

        });

        studentGroup.MapDelete("/{studentId}/course/{courseId}",
        async (int studentId, int courseId, KpitStoreContext context) =>
        {
            var student = await context.Students
                .Include(s => s.Courses)
                .Where(s => s.Id == studentId)
                .FirstOrDefaultAsync();

            if (student == null) return Results.NotFound("Student doesn't exist");

            if (!student.Courses.Any(c => c.Id == courseId))
                return Results.BadRequest("Course is not enrolled.");

            var course = await context.Courses.FindAsync(courseId);
            if (course == null)
                return Results.NotFound("Course not found. There is error in database mapping!");

            student.Courses.Remove(course);

            await context.SaveChangesAsync();

            return Results.Ok("Course removed.");
        });
    }
}
