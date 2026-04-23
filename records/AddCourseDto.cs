using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KpitApi.records;

public record class AddCourseDto(
    [Required][StringLength(15)] string Name
)
{

}
