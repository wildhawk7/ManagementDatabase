namespace ManagementDatabase.Models
{
    public class Admin
    {
        public int AdminID { get; set; }
        public string? AdminName { get; set; }
        public string? AdminPassword { get; set; }

        // Optionally, you can also define a parameterless constructor
        public Admin()
        {
            // Initialize other properties if needed
        }
    }
}
