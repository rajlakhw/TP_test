using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Repositories;
using ViewModels.Common;

namespace Services
{
    public class TPQuoteItemsLogic : ITPQuoteItemsLogic
    {
        private readonly IRepository<QuoteItem> quoteItemRepository;
		private readonly ITPQuotesLogic quoteService;
		private readonly ITPTimeZonesService timeZonesService;

		public TPQuoteItemsLogic(IRepository<QuoteItem> tpQuoteItemRepo, ITPQuotesLogic quoteService,
			ITPTimeZonesService tPTimeZonesService)
        {
            this.quoteItemRepository = tpQuoteItemRepo;
			this.quoteService = quoteService;
			this.timeZonesService = tPTimeZonesService;
		}

        public async Task<bool> QuoteItemExists(int QuoteItemId)
        {
            var result = await quoteItemRepository.All().Where(q => q.Id == QuoteItemId).FirstOrDefaultAsync();
            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<QuoteItem> GetQuoteItemById(int quoteitemId)
        {
            var result = await quoteItemRepository.All().Where(q => q.Id == quoteitemId && q.DeletedDateTime == null).FirstOrDefaultAsync();

            return result;
        }

		public async Task<ViewModels.QuoteItems.QuoteItemsViewModel> GetViewModelById(int quoteitemId)
		{
			var result = await quoteItemRepository.All().Where(e => e.Id == quoteitemId)
				.Select(x => new ViewModels.QuoteItems.QuoteItemsViewModel()
				{
					Id = x.Id,
					QuoteId = x.QuoteId,
					LanguageServiceId = x.LanguageServiceId,
					SourceLanguageIanacode = x.SourceLanguageIanacode,
					TargetLanguageIanacode = x.TargetLanguageIanacode,
					WordCountNew = x.WordCountNew,
					WordCountFuzzyBand1 = x.WordCountFuzzyBand1,
					WordCountFuzzyBand2 = x.WordCountFuzzyBand2,
					WordCountFuzzyBand3 = x.WordCountFuzzyBand3,
					WordCountFuzzyBand4 = x.WordCountFuzzyBand4,
					WordCountExact = x.WordCountExact,
					WordCountRepetitions = x.WordCountRepetitions,
					WordCountPerfectMatches = x.WordCountPerfectMatches,
					WordCountClientSpecific = x.WordCountClientSpecific,
					Pages = x.Pages,
					Characters = x.Characters,
					Documents = x.Documents,
					InterpretingExpectedDurationMinutes = x.InterpretingExpectedDurationMinutes,
					InterpretingLocationOrgName = x.InterpretingLocationOrgName,
					InterpretingLocationAddress1 = x.InterpretingLocationAddress1,
					InterpretingLocationAddress2 = x.InterpretingLocationAddress2,
					InterpretingLocationAddress3 = x.InterpretingLocationAddress3,
					InterpretingLocationAddress4 = x.InterpretingLocationAddress4,
					InterpretingLocationCountyOrState = x.InterpretingLocationCountyOrState,
					InterpretingLocationPostcodeOrZip = x.InterpretingLocationPostcodeOrZip,
					InterpretingLocationCountryId = x.InterpretingLocationCountryId,
					AudioMinutes = x.AudioMinutes,
					WorkMinutes = x.WorkMinutes,
					ExternalNotes = x.ExternalNotes,
					ChargeToClient = x.ChargeToClient,
					CreatedByEmployeeId = x.CreatedByEmployeeId,
					CreatedDateTime = x.CreatedDateTime,
					LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
					LastModifiedDateTime = x.LastModifiedDateTime,
					DeletedByEmployeeId = x.DeletedByEmployeeId,
					DeletedDateTime = x.DeletedDateTime,
					SupplierWordCountNew = x.SupplierWordCountNew,
					SupplierWordCountFuzzyBand1 = x.SupplierWordCountFuzzyBand1,
					SupplierWordCountFuzzyBand2 = x.SupplierWordCountFuzzyBand2,
					SupplierWordCountFuzzyBand3 = x.SupplierWordCountFuzzyBand3,
					SupplierWordCountFuzzyBand4 = x.SupplierWordCountFuzzyBand4,
					SupplierWordCountExact = x.SupplierWordCountExact,
					SupplierWordCountRepetitions = x.SupplierWordCountRepetitions,
					SupplierWordCountPerfectMatches = x.SupplierWordCountPerfectMatches,
					SupplierWordCountClientSpecific = x.SupplierWordCountClientSpecific
				})
				.FirstOrDefaultAsync();


			return result;
		}

		public async Task<QuoteItem> UpdateQuoteItem(int quoteitemID, byte LanguageServiceId, string SourceLanguageIanacode, string TargetLanguageIanacode, int WordCountNew, int WordCountExact, int WordCountRepetitions, int WordCountPerfectMatches,
									 int WordCountFuzzyBand1, int WordCountFuzzyBand2, int WordCountFuzzyBand3, int WordCountFuzzyBand4, decimal ChargeToClient, int Pages, int Characters, int Documents, int WorkMinutes, int AudioMinutes, int InterpretingExpectedDurationMinutes,
									 string InterpretingLocationOrgName, string InterpretingLocationAddress1, string InterpretingLocationAddress2, string InterpretingLocationAddress3, string InterpretingLocationAddress4, string InterpretingLocationCountyOrState,
									 string InterpretingLocationPostcodeOrZip, short InterpretingLocationCountryId, string ExternalNotes, short lastModifiedByEmployeeID,byte languageServiceCategoryId=0)
		{
			var quoteitem = await GetQuoteItemById(quoteitemID);

			// maybe add security check later here and autoMapper
			quoteitem.LanguageServiceId = LanguageServiceId;
			quoteitem.SourceLanguageIanacode = SourceLanguageIanacode;
			quoteitem.TargetLanguageIanacode = TargetLanguageIanacode;
			quoteitem.WordCountNew = WordCountNew;
			quoteitem.WordCountExact = WordCountExact;
			quoteitem.WordCountRepetitions = WordCountRepetitions;
			quoteitem.WordCountPerfectMatches = WordCountPerfectMatches;
			quoteitem.WordCountFuzzyBand1 = WordCountFuzzyBand1;
			quoteitem.WordCountFuzzyBand2 = WordCountFuzzyBand2;
			quoteitem.WordCountFuzzyBand3 = WordCountFuzzyBand3;
			quoteitem.WordCountFuzzyBand4 = WordCountFuzzyBand4;
			quoteitem.ChargeToClient = ChargeToClient;
			quoteitem.Pages = Pages;
			quoteitem.Characters = Characters;
			quoteitem.Documents = Documents;
			quoteitem.WorkMinutes = WorkMinutes;
			quoteitem.AudioMinutes = AudioMinutes;
			quoteitem.InterpretingExpectedDurationMinutes = InterpretingExpectedDurationMinutes;
			quoteitem.InterpretingLocationOrgName = InterpretingLocationOrgName;
			quoteitem.InterpretingLocationAddress1 = InterpretingLocationAddress1;
			quoteitem.InterpretingLocationAddress2 = InterpretingLocationAddress2;
			quoteitem.InterpretingLocationAddress3 = InterpretingLocationAddress3;
			quoteitem.InterpretingLocationAddress4 = InterpretingLocationAddress4;
			quoteitem.InterpretingLocationCountyOrState = InterpretingLocationCountyOrState;
			quoteitem.InterpretingLocationPostcodeOrZip = InterpretingLocationPostcodeOrZip;

			if (InterpretingLocationCountryId > 0)
            {
				quoteitem.InterpretingLocationCountryId = InterpretingLocationCountryId;
			}
			
			quoteitem.ExternalNotes = ExternalNotes;
			quoteitem.LastModifiedByEmployeeId = lastModifiedByEmployeeID;
			quoteitem.LanguageServiceCategoryId = languageServiceCategoryId;

			quoteItemRepository.Update(quoteitem);
			await quoteItemRepository.SaveChangesAsync();

			return quoteitem;
		}

		public async Task<QuoteItem> RemoveQuoteItem(int quoteitemid, short deletedByEmployeeID)
		{
			var QuoteItem = await GetQuoteItemById(quoteitemid);

			QuoteItem.DeletedByEmployeeId = deletedByEmployeeID;
			QuoteItem.DeletedDateTime = (DateTime)timeZonesService.GetCurrentGMT();

			quoteItemRepository.Update(QuoteItem);
			await quoteItemRepository.SaveChangesAsync();

			return QuoteItem;
		}
		public async Task<QuoteItem> ApplyToQuoteItem(QuoteItem item, TPWordCountBreakdownBatchModel model)
		{
			var quoteitem = await GetQuoteItemById(item.Id);
			short? InterpretingLocationCountryID = null;
			if (quoteitem.InterpretingLocationCountryId != null) { InterpretingLocationCountryID = quoteitem.InterpretingLocationCountryId; }
			// maybe add security check later here and autoMapper

			quoteitem.WordCountNew = model.pNewWords;
			quoteitem.WordCountExact = model.pExactMatchWords;
			quoteitem.WordCountRepetitions = model.pRepetitionsWords;
			quoteitem.WordCountPerfectMatches = model.pMatchPlusOrPerfectMatchWords;
			quoteitem.WordCountFuzzyBand1 = model.pFuzzyBand1Words;
			quoteitem.WordCountFuzzyBand2 = model.pFuzzyBand2Words;
			quoteitem.WordCountFuzzyBand3 = model.pFuzzyBand3Words;
			quoteitem.WordCountFuzzyBand4 = model.pFuzzyBand4Words;
			quoteitem.Characters = model.pTotalCharacterCount;
			quoteitem.InterpretingLocationCountryId = InterpretingLocationCountryID;
			quoteitem.LastModifiedByEmployeeId = item.LastModifiedByEmployeeId;
			quoteitem.LastModifiedDateTime = GeneralUtils.GetCurrentGMT();
			quoteitem.SupplierWordCountNew = model.pLinguisticNewWords;
			quoteitem.SupplierWordCountFuzzyBand1 = model.pLinguisticFuzzyBand1Words;
			quoteitem.SupplierWordCountFuzzyBand2 = model.pLinguisticFuzzyBand2Words;
			quoteitem.SupplierWordCountFuzzyBand3 = model.pLinguisticFuzzyBand3Words;
			quoteitem.SupplierWordCountFuzzyBand4 = model.pLinguisticFuzzyBand4Words;
			quoteitem.SupplierWordCountExact = model.pExactMatchWords;
			quoteitem.SupplierWordCountRepetitions = model.pRepetitionsWords;
			quoteitem.SupplierWordCountPerfectMatches = model.pMatchPlusOrPerfectMatchWords;

			


			quoteItemRepository.Update(quoteitem);
			await quoteItemRepository.SaveChangesAsync();

			return quoteitem;
		}
		public async Task<QuoteItem> UpdateQuoteItem(QuoteItem model)
		{
			var quoteitem = await GetQuoteItemById(model.Id);

			// maybe add security check later here and autoMapper
			quoteitem.LanguageServiceId = model.LanguageServiceId;
			
			quoteitem.WordCountNew = model.WordCountNew ?? quoteitem.WordCountNew;
			quoteitem.WordCountExact = model.WordCountExact ?? quoteitem.WordCountExact;
			quoteitem.WordCountRepetitions = model.WordCountRepetitions ?? quoteitem.WordCountRepetitions;
			quoteitem.WordCountPerfectMatches = model.WordCountPerfectMatches ?? quoteitem.WordCountPerfectMatches;
			quoteitem.WordCountFuzzyBand1 = model.WordCountFuzzyBand1 ?? quoteitem.WordCountFuzzyBand1;
			quoteitem.WordCountFuzzyBand2 = model.WordCountFuzzyBand2 ?? quoteitem.WordCountFuzzyBand2;
			quoteitem.WordCountFuzzyBand3 = model.WordCountFuzzyBand3 ?? quoteitem.WordCountFuzzyBand3;
			quoteitem.WordCountFuzzyBand4 = model.WordCountFuzzyBand4 ?? quoteitem.WordCountFuzzyBand4;
			quoteitem.ChargeToClient = model.ChargeToClient ?? quoteitem.ChargeToClient;
			quoteitem.Pages = model.Pages ?? quoteitem.Pages;
			quoteitem.Characters = model.Characters ?? quoteitem.Characters;
			quoteitem.Documents = model.Documents ?? quoteitem.Documents;
			quoteitem.WorkMinutes = model.WorkMinutes ?? quoteitem.WorkMinutes;
			quoteitem.InterpretingExpectedDurationMinutes = model.InterpretingExpectedDurationMinutes ?? quoteitem.InterpretingExpectedDurationMinutes;
			quoteitem.LastModifiedByEmployeeId = model.LastModifiedByEmployeeId;
			quoteitem.LastModifiedDateTime =GeneralUtils.GetCurrentUKTime();
			quoteItemRepository.Update(quoteitem);
			await quoteItemRepository.SaveChangesAsync();

			return quoteitem;
		}
	}
}
