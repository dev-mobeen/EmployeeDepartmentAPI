using System.Text.Json.Serialization;

namespace EmployeeDepartmentAPI.Models
{
    public class Department
    {
        //[key]
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property
        //[JsonIgnore]
        public ICollection<Employee>? Employees { get; set; }
    }
}
