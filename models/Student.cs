using System;

namespace KpitApi.models;

public class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = string.Empty;

    //foreign key
    public int DepartmentId { get; set; }

    public Department? Department { get; set; }

    public StudentProfile? StudentProfile { get; set; }

    public List<Course> Courses { get; set; } = new();

}
