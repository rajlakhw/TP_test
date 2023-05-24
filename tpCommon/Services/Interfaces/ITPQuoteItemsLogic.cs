using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;
using ViewModels.Common;

namespace Services.Interfaces
{
    public interface ITPQuoteItemsLogic : IService
    {
        Task<bool> QuoteItemExists(int QuoteItemId);
        Task<QuoteItem> GetQuoteItemById(int quoteitemId);
        Task<ViewModels.QuoteItems.QuoteItemsViewModel> GetViewModelById(int quoteitemId);
        Task<QuoteItem> UpdateQuoteItem(int quoteitemID, byte LanguageServiceId, string SourceLanguageIanacode, string TargetLanguageIanacode, int WordCountNew, int WordCountExact, int WordCountRepetitions, int WordCountPerfectMatches,
                                     int WordCountFuzzyBand1, int WordCountFuzzyBand2, int WordCountFuzzyBand3, int WordCountFuzzyBand4, decimal ChargeToClient, int Pages, int Characters, int Documents, int WorkMinutes, int AudioMinutes, int InterpretingExpectedDurationMinutes,
                                     string InterpretingLocationOrgName, string InterpretingLocationAddress1, string InterpretingLocationAddress2, string InterpretingLocationAddress3, string InterpretingLocationAddress4, string InterpretingLocationCountyOrState,
                                     string InterpretingLocationPostcodeOrZip, short InterpretingLocationCountryId, string ExternalNotes, short lastModifiedByEmployeeID, byte languageServiceCategoryId = 0);
        Task<QuoteItem> RemoveQuoteItem(int quoteitemid, short deletedByEmployeeID);
        Task<QuoteItem> ApplyToQuoteItem(QuoteItem item, TPWordCountBreakdownBatchModel model);
        Task<QuoteItem> UpdateQuoteItem(QuoteItem model);

    }
}
