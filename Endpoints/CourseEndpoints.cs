using System;
using KpitApi.data;
using KpitApi.models;
using KpitApi.records;
using Microsoft.EntityFrameworkCore;

namespace KpitApi.Endpoints;

public static class CourseEndpoints
{
    public static void AddAllCourseEndpoints(this WebApplication app)
    {
        var courseGroup = app.MapGroup("/course");

        courseGroup.MapPost("", async (AddCourseDto courseDto, KpitStoreContext context) =>
        {
            context.Courses.Add(new Course
            {
                Name = courseDto.Name
            });

            await context.SaveChangesAsync();
        });

        courseGroup.MapGet("", (KpitStoreContext context) =>
        {
            return context.Courses.Select(c => new DisplayCourseDto
            (
                c.Id,
                c.Name
            ));
        });

        courseGroup.MapPut("/{id}", async (int id, AddCourseDto courseDto, KpitStoreContext context) =>
        {
            var course = await context.Courses.FindAsync(id);
            if (course == null) return Results.NotFound("course doesn't exist");
            course.Name = courseDto.Name;

            await context.SaveChangesAsync();
            return Results.Accepted();
        });

        courseGroup.MapDelete("/{id}", async (int id, KpitStoreContext context) =>
        {
            await context.Courses
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();

            await context.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
