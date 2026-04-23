using System;

namespace KpitApi.models;

public class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<Student> Students { get; set; } = [];

}
