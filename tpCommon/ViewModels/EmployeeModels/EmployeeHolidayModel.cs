using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.EmployeeModels
{
    public class EmployeeHolidayModel 
    {
        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public byte? OfficeId { get; set; }
        public int Year { get; set; }
        public byte TeamId { get; set; }
        public DateTime? TerminateDate { get; set; }
        public int TotalBaseAnnualHolidays { get; set; }
        public int? LoyaltyDays { get; set; }
        public int? MiscDays { get; set; }
        public int? PreviouslyWorkedDays { get; set; }
        public decimal? HolidaysRequested { get; set; }
        public decimal? HolidaysRemaining { get; set; }
        public string HolidayNotes { get; set; }
    }
}
