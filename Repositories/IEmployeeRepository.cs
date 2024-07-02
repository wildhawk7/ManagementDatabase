using ManagementDatabase.Models;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployees();
    Task<Employee> GetEmployee(int id);
    Task AddEmployee(Employee employee);
    Task LogEmployeeLogin(int employeeId);
}
