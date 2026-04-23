namespace KpitApi.records;

public record DisplayStudentDto(
    int Id,
    string Name,
    string Email,
    string Bio,
    string DepartmentName,
    string[] CourseNames
)
{

}
