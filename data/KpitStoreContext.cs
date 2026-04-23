using System;
using KpitApi.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace KpitApi.data;

public class KpitStoreContext(DbContextOptions<KpitStoreContext> options) : DbContext(options)
{
    public DbSet<Student> Students => Set<Student>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();

}
