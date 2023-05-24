using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Services.Common;

namespace Services.Interfaces
{
    public interface ITPLocalCurrencyInfo : IService
    {
        Task<LocalCurrencyInfo> GetById(int CurrencyID);
        Task<IEnumerable<LocalCurrencyInfo>> GetAll();
    }
}
