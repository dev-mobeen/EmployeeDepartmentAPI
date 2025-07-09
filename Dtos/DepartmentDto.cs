using System.ComponentModel.DataAnnotations;

namespace EmployeeDepartmentAPI.Dtos
{


    public class DepartmentCreateDto       // request body
    {
        [Required]                         // validation
        public string Name { get; set; } = string.Empty;
    }
    public class DepartmentDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        // Initialize to an empty list so you never get a null-reference
        public ICollection<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    }
}
