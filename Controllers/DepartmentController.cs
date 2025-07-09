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
    [Authorize] // 🔐 Secure with JWT
    public class DepartmentController : ControllerBase
        {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
            {
            _context = context;
            }

        // GET: api/Department
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
            {
            var departmentDtos = await _context.Departments
                .Select(d => new DepartmentDto
                    {
                    Id = d.Id,
                    Name = d.Name,
                    Employees = d!.Employees
                        .Select(e => new EmployeeDto
                            {
                            Id = e.Id,
                            FullName = e.FullName,
                            Email = e.Email
                            })
                        .ToList()
                    })
                .ToListAsync();

            return Ok(new ApiResponse<List<DepartmentDto>>(
                departmentDtos,
                "Department list",
                200
            ));
            }

        // GET: api/Department/{id}
        //[HttpGet("{id}")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<DepartmentDto>>> GetDepartment(int id)
        {
            // 1) Look the record up (include Employees if you need them)
            var entity = await _context.Departments
                                       .Include(d => d.Employees)
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(d => d.Id == id);

            // 2) 404 when it isn’t there
            if (entity is null)
            {
                return NotFound(new
                {
                    status = false,
                    message = $"Department with id {id} not found",
                    statusCode = 200
                });
            }

            // 3) Map entity ➜ DTO  (AutoMapper, Mapster, or manual)
            var dto = new DepartmentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Employees = entity.Employees
                                  .Select(e => new EmployeeDto
                                  {
                                      Id = e.Id,
                                      FullName = e.FullName,
                                      Email = e.Email,
                                      DepartmentId = e.DepartmentId
                                  })
                                  .ToList()
            };

            // 4) 200 OK with a typed envelope
            return Ok(
                new ApiResponse<DepartmentDto>(
                    data: dto,
                    message: "Success",      // optional
                    statusCode: 200,
                    status: true
                )
            );
        }


        // POST: api/Department
        [HttpPost]
        public async Task<IActionResult> CreateDepartment(
                [FromBody] DepartmentCreateDto dto)
        {
            // Map DTO ➜ entity
            var entity = new Department { Name = dto.Name };

            _context.Departments.Add(entity);
            await _context.SaveChangesAsync();

            // Map entity ➜ output DTO (Id is now filled in)
            var outDto = new DepartmentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                //Employees = new List<EmployeeDto>()   // empty on create
            };

            return CreatedAtAction(nameof(GetDepartment), new { id = entity.Id }, outDto);
        }

        // PUT: api/Department/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<DepartmentCreateDto>>> UpdateDepartment(int id,[FromBody] DepartmentCreateDto dto)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department is null)
            {
                return NotFound(new
                {
                    status = false,
                    message = $"Department with id {id} not found",
                    statusCode = 200
                });
            }

            department.Name = dto.Name;
            await _context.SaveChangesAsync();

            // Manually create a DTO
            var Newdto = new DepartmentCreateDto
            {
                //Id = department.Id,
                Name = department.Name
                // if your DTO has Employees, you’d map them here too
            };

            return Ok(new ApiResponse<DepartmentCreateDto>(
                data: Newdto,
                message: "Success",
                statusCode: 200,
                status: true
            ));
        }


        // DELETE: api/Department/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
            {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound(new
                {
                    status = false,
                    message = $"Department with id {id} not found",
                    statusCode = 200
                });
            }
                //return NotFound();

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            //return NoContent();
            return Ok(new
            {
                //data = department,
                status = true,
                message = $"Department with id {id} Deleted",
                statusCode = 200
            });
        }
        }
    }
