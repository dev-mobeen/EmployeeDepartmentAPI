namespace EmployeeDepartmentAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        // Foreign key
        public int DepartmentId { get; set; }

        // Navigation property
        public Department? Department { get; set; }
    }
}
