namespace ManagementDatabase.Models
{
    public class Employer
    {
        public int EmployerID { get; set; }
        public string EmployerName { get; set; } = string.Empty;
        public string EmployerDescription { get; set; } = string.Empty;

        // Optionally, you can also define a parameterless constructor
        public Employer()
        {
            // Initialize other properties if needed
        }
    }
}
