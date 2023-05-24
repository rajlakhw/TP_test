namespace ViewModels.Common
{
    public class EmployeeDetailsViewModel
    {
        public short Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string JobTitle { get; set; }
        public bool DisplayJobTitle { get; set; }
    }
}
