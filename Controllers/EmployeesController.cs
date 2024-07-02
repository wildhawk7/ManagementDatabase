using ManagementDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ManagementDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IConfiguration _config;

        public EmployeesController(IEmployeeRepository employeeRepository, IConfiguration config)
        {
            _employeeRepository = employeeRepository;
            _config = config;
        }

        private IDbConnection Connection => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployees();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployee(id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _employeeRepository.AddEmployee(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }

        [HttpPost("{id}/login")]
        public async Task<IActionResult> LogEmployeeLogin(int id)
        {
            await _employeeRepository.LogEmployeeLogin(id);
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Employee employee)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = "INSERT INTO Employees (EmployeeName, EmployeeDescription, EmployeeAge, EmployeeGender, Username, Password, Role) VALUES (@EmployeeName, @EmployeeDescription, @EmployeeAge, @EmployeeGender, @Username, @Password, @Role)";
                    dbConnection.Open();
                    var result = await dbConnection.ExecuteAsync(query, employee);
                    if (result > 0)
                    {
                        // Registration successful
                        return Ok(new { Message = "User registered successfully" });
                    }
                    else
                    {
                        // Registration failed
                        return StatusCode(500, "Internal server error");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = "SELECT * FROM Employees WHERE Username = @Username AND Password = @Password";
                    dbConnection.Open();
                    var user = await dbConnection.QueryFirstOrDefaultAsync<Employee>(query, new { request.Username, request.Password });
                    if (user != null)
                    {
                        // Login successful
                        return Ok(user);
                    }
                    else
                    {
                        // Login failed
                        return Unauthorized();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
