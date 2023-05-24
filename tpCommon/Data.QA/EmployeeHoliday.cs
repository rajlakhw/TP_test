using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class EmployeeHoliday
    {
        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public int Year { get; set; }
        public decimal? TotalBaseAnnualHolidays { get; set; }
        public decimal? MiscDays { get; set; }
        public int? PreviouslyWorkedDays { get; set; }
        public int? LoyaltyDays { get; set; }
        public int? LoyaltyHolidaysTemp { get; set; }
        public decimal? TotalAnnualHolidays { get; set; }
        public decimal? HolidaysRemaing { get; set; }
        public decimal? HolidaysRequested { get; set; }
        public decimal? HolidaysApproved { get; set; }
        public decimal? HolidaysTaken { get; set; }
        public decimal? HolidaysOnGoingOrTaken { get; set; }
        public string HolidayNotes { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployee { get; set; }
    }
}
