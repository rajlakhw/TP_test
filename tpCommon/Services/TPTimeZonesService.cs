using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Services.Interfaces;

namespace Services
{
    public class TPTimeZonesService : ITPTimeZonesService
    {
        public IReadOnlyCollection<TimeZoneInfo> GetAllTimeZonesForDisplay()
        {
            return System.TimeZoneInfo.GetSystemTimeZones();
        }

        public DateTime GetCurrentGMT()
        {
            return System.TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
        }

        public DateTime ConvertGMTToRequestedTimeZone(DateTime dateToConvert, string timeZoneToConvert)
        {
            return System.TimeZoneInfo.ConvertTime(dateToConvert, TimeZoneInfo.FindSystemTimeZoneById(timeZoneToConvert));
        }
    }
}
