using System;
using System.Collections.Generic;
using Data;

namespace ViewModels.HomePage
{
    public class EmployeesAnnivarsaries : IAnniversary, IEquatable<EmployeesAnnivarsaries>
    {
        public int EmployeeID { get; set; }
        public int AnniversaryID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageBase64 { get; set; }
        public DateTime AnniversaryDate { get; set; }
        public List<WorkAnnivarsariesComment> Comments { get; set; }
        public List<WorkAnniversariesLike> Likes { get; set; }
        public bool IsBirthday { get => false; set { } }
        public bool ShowLikesAndComments { get; set; }
        public int DaysLeft {
            get
            {
                int daysLeft = (new DateTime(DateTime.Today.Year, this.AnniversaryDate.Month, this.AnniversaryDate.Day) - DateTime.Today).Days;
                if (this.AnniversaryDate.Month == 1 && DateTime.Today.Month == 12)
                    daysLeft = (new DateTime(DateTime.Today.Year + 1, this.AnniversaryDate.Month, this.AnniversaryDate.Day) - DateTime.Today).Days;
                return daysLeft;
            }
            set { } }

        public bool Equals(EmployeesAnnivarsaries other)
        {
            if (other is null)
                return false;

            return this.EmployeeID == other.EmployeeID && this.AnniversaryDate == other.AnniversaryDate;
        }
        public override bool Equals(object obj) => Equals(obj as EmployeesAnnivarsaries);
        public override int GetHashCode() => (EmployeeID, AnniversaryDate).GetHashCode();
    }
}
