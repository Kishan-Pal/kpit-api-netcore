using System.ComponentModel.DataAnnotations;

namespace KpitApi.records;

public record class AddStudentDto(
    [Required][StringLength(15)] string Name,
    [Required][EmailAddress] string Email,
    [Required] int DepartmentId,
    [Required][StringLength(100)] string Bio
)
{

}
