using System.ComponentModel.DataAnnotations;

namespace KpitApi.records;

public record AddDepartmentDto(
    [Required][StringLength(15)] string Name
)
{

}
