using System;
using KpitApi.data;
using KpitApi.models;
using KpitApi.records;
using Microsoft.EntityFrameworkCore;

namespace KpitApi.Endpoints;

public static class DepartmentEndpoints
{
    public static void AddDepartmentEndpoints(this WebApplication app)
    {
        var departmentGroup = app.MapGroup("/department");

        departmentGroup.MapGet("", (KpitStoreContext context) =>
        {
            return context.Departments
            .Select(d => new DisplayDepartmentDto(
                d.Id,
                d.Name
            ));
        });

        departmentGroup.MapPost("", async (AddDepartmentDto departmentDto, KpitStoreContext context) =>
        {
            await context.Departments.AddAsync(
                new Department
                {
                    Name = departmentDto.Name
                }
            );
            await context.SaveChangesAsync();
            return Results.Accepted("Added Department");
        });

        departmentGroup.MapPut("/{id}", async (int id, AddDepartmentDto departmentDto, KpitStoreContext context) =>
        {
            var department = await context.Departments.FindAsync(id);
            if (department == null) return Results.NotFound("Department doesn't exist");
            department.Name = departmentDto.Name;

            await context.SaveChangesAsync();
            return Results.Accepted("Department changed");
        });

        departmentGroup.MapDelete("/{id}", async (int id, KpitStoreContext context) =>
        {
            await context.Departments.Where(d => d.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }
}
