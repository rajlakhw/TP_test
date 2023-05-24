using System;

namespace ViewModels.HomePage
{
    public class EmployeesHighFives
    {
        public int EmployeeID { get; set; }
        public string Comment { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageBase64 { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
