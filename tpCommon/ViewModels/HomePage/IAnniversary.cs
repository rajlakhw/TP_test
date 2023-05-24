using System;

namespace ViewModels.HomePage
{
    public interface IAnniversary
    {
        public int EmployeeID { get; set; }
        public int AnniversaryID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageBase64 { get; set; }
        public DateTime AnniversaryDate { get; set; }
        public bool IsBirthday { get; set; }
        public bool ShowLikesAndComments { get; set; }
        public int DaysLeft { get; set; }
    }
}
