using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using System.Collections;

namespace Services.Interfaces
{
    public interface ITPTimeZonesService : IService
    {
        IReadOnlyCollection<TimeZoneInfo> GetAllTimeZonesForDisplay();
        DateTime GetCurrentGMT();
        DateTime ConvertGMTToRequestedTimeZone(DateTime dateToConvert, string timeZoneToConvert);
    }
}
