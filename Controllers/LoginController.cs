using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection Connection => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
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
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Employee
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeDescription { get; set; }
        public int EmployeeAge { get; set; }
        public string EmployeeGender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
