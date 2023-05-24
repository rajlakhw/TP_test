using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using Data;

namespace Services.Interfaces
{
    public interface IQuoteAndOrderDiscountsAndSurchargesCategories : IService
    {
        Task<QuoteAndOrderDiscountsAndSurchargesCategory> GetById(int ID);
        Task<IEnumerable<QuoteAndOrderDiscountsAndSurchargesCategory>> GetAll();
        Task<IEnumerable<QuoteAndOrderDiscountsAndSurchargesCategory>> GetAllDiscountCategories();
        Task<IEnumerable<QuoteAndOrderDiscountsAndSurchargesCategory>> GetAllSurchargeCategories();

    }
}
