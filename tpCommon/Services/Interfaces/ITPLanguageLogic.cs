using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;

namespace Services.Interfaces
{
    public interface ITPLanguageLogic : IService
    {
        LocalLanguageInfo GetLanguageInfo(string languageCodeBeingDescribed, string IANACode);
        string MapLangIanaCodeWithRegion(string IanaCode);
    }
}
