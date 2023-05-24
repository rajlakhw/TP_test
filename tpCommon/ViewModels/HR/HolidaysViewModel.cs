using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.HR
{
    public class HolidaysViewModel
    {
        public decimal? HolidaysRemaining { get; set; }
        public int Year { get; set; }
        public decimal? TotalAnnualHolidays { get; set; }
        public double TotalHolidaysForCurrentRequest { get; set; }
        public string NextAvailableHolidayDate { get; set; }
        public string NextWorkingDayAfterHolidayString { get; set; }
    }
}
