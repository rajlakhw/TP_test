using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class TPCommonGeneralUtils
    {
        public static DateTime GetDateFromGivenWeek(this DateTime dt, DayOfWeek weekDay)
        {
            int diff = (dt.DayOfWeek - weekDay) % 7;
            if (diff != 0 && weekDay == DayOfWeek.Sunday)
            {
                diff = -(diff) + 7;
                diff = -(diff);
            }
            else if(diff != 0 && dt.DayOfWeek == DayOfWeek.Sunday)
            {
                diff = (diff + 7);
            }
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
