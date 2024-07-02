namespace ManagementDatabase.Models
{
    public class Login
    {
        public int LoginID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime LoginDate { get; set; } = DateTime.Now; // Default to current date/time
        public DateTime? LogoutDate { get; set; }

        // Optionally, you can also define a parameterless constructor
        public Login()
        {
            // Initialize other properties if needed
        }
    }
}
