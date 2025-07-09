using EmployeeDepartmentAPI.Models;

namespace EmployeeDepartmentAPI.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int DepartmentId { get; set; }

        // ← use the DTO type here
        //public DepartmentDto Department { get; set; } = default!;
    }
}
