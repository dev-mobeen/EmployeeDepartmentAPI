using EmployeeDepartmentAPI.Data;
using EmployeeDepartmentAPI.Dtos;
using EmployeeDepartmentAPI.Models;
using EmployeeDepartmentAPI.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDepartmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            // Project into your DTOs in one go – no need for Include/ThenInclude
            var list = await _context.Employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Email = e.Email,
                    DepartmentId = e.DepartmentId,

                    // map the Department navigation into your DepartmentDto
                    //Department = new DepartmentDto
                    //{
                    //    Id = e.Department!.Id,
                    //    Name = e.Department.Name,

                    //    // if you don’t need the full employee list here, you can leave this empty
                    //    Employees = new List<EmployeeDto>()
                    //}
                })
                .ToListAsync();

            return Ok(new ApiResponse<List<EmployeeDto>>(
                list,
                "Employee list",
                200
            ));
        }


        // GET: api/Employee/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees.Include(e => e.Department)
                                                   .FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return NotFound(new ApiResponse<string>(
                    null,
                    "Employee not found",
                    404,
                    false
                ));
            }

            return Ok(new ApiResponse<Employee>(
                employee,
                "Employee details",
                200
            ));
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.FullName) || string.IsNullOrWhiteSpace(employee.Email))
            {
                return BadRequest(new ApiResponse<string>(
                    null,
                    "Name and Email are required",
                    400,
                    false
                ));
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return StatusCode(201, new ApiResponse<Employee>(
                employee,
                "Employee created",
                201
            ));
        }

        // PUT: api/Employee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee updated)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new ApiResponse<string>(
                    null,
                    "Employee not found",
                    404,
                    false
                ));
            }

            employee.FullName = updated.FullName;
            employee.Email = updated.Email;
            employee.DepartmentId = updated.DepartmentId;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Employee>(
                employee,
                "Employee updated",
                200
            ));
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new ApiResponse<string>(
                    null,
                    "Employee not found",
                    404,
                    false
                ));
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string>(
                "Employee deleted",
                "Deleted successfully",
                200
            ));
        }
    }
}
