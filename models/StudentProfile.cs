using System;

namespace KpitApi.models;

public class StudentProfile
{
    public int Id { get; set; }

    public string Bio { get; set; } = string.Empty;

    // foreign key
    public int StudentId { get; set; }
    public Student? Student { get; set; }
}
