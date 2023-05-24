using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Services.Common;
using ViewModels.Common;

namespace Services.Interfaces
{
    public interface ITPCurrenciesLogic : IService
    {
        Task<Currency> GetById(int CurrencyID);
        Task<IEnumerable<Currency>> GetAll();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllENCurrencies();
        Task<LocalCurrencyInfo> GetCurrencyInfo(int currencyId, string IANACode);
    }
}
