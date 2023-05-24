using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using Data;

namespace Services.Interfaces
{
    public interface IQuoteAndOrderDiscountsAndSurcharges : IService
    {
        Task<QuoteAndOrderDiscountsAndSurcharge> GetById(int ID);
        Task<QuoteAndOrderDiscountsAndSurcharge> RemoveSurchargeOrDiscount(int ID, short? deletedByUserId);
        Task<QuoteAndOrderDiscountsAndSurcharge> UpdateSurchargeOrDiscount(int ID, byte category, string description, bool PercOrValue, decimal amount, short? lastmodifiedByUserId);
        Task<QuoteAndOrderDiscountsAndSurcharge> CreateSurchargeOrDiscount(bool surchargeordiscounttype, byte category, string description, bool PercOrValue, decimal amount, short createdByByUserId);
    }
}
