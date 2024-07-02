using System.Data;
using Microsoft.Data.SqlClient;  // Updated namespace
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ManagementDatabase.Models;

namespace ManagementDatabase.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IConfiguration _config;

        public EmployeeRepository(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection Connection => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "SELECT * FROM Employees";
                dbConnection.Open();
                return await dbConnection.QueryAsync<Employee>(query);
            }
        }

        public async Task<Employee?> GetEmployee(int id)  // Marked as nullable
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "SELECT * FROM Employees WHERE EmployeeID = @Id";
                dbConnection.Open();
                var employee = await dbConnection.QueryFirstOrDefaultAsync<Employee>(query, new { Id = id });
                return employee;
            }
        }

        public async Task AddEmployee(Employee employee)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "INSERT INTO Employees (EmployeeName, EmployeeDescription, EmployeeAge, EmployeeGender) VALUES (@EmployeeName, @EmployeeDescription, @EmployeeAge, @EmployeeGender)";
                dbConnection.Open();
                await dbConnection.ExecuteAsync(query, employee);
            }
        }

        public async Task LogEmployeeLogin(int employeeId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "INSERT INTO Logins (EmployeeID, LoginDate) VALUES (@EmployeeID, @LoginDate)";
                dbConnection.Open();
                await dbConnection.ExecuteAsync(query, new { EmployeeID = employeeId, LoginDate = DateTime.Now });
            }
        }
    }
}
