using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITPLinguistsAltairDetails : IService
    {
        Task<Data.LinguistsAltairDetail> GetTINNumber(int LinguistID);
        Task<Data.LinguistsAltairDetail> UpdateTINNumber(int LinguistID,string TINNumber);
    }
}
