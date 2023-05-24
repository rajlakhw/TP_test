using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;

namespace Services.Interfaces
{
    public interface ITPQuotesLogic : IService
    {
		Task<Quote> CreateQuote(int EnquiryID, bool IsCurrentVersion, short QuoteCurrencyID, string LangIANACode, string Title,
								string QuoteFileName, string InternalNotes, DateTime QuoteDate, string QuoteOrgName,
								string QuoteAddress1, string QuoteAddress2, string QuoteAddress3, string QuoteAddress4,
								string QuoteCountyOrState, string QuotePostcodeOrZip, short QuoteCountryID, string AddresseeSalutationName,
								string OpeningSectionText, string ClosingSectionText, byte TimelineUnit, double TimelineValue,
								byte WordCountPresentationOption, bool ShowInterpretingDurationInBreakdown, bool ShowWorkDurationInBreakdown,
								bool ShowPagesOrSlidesInBreakdown, bool ShowNumberOfCharactersInBreakdown, bool ShowNumberOfDocumentsInBreakdown,
								short SalesContactEmployeeID, string CustomerSpecificField1Value, string CustomerSpecificField2Value,
								string CustomerSpecificField3Value, string CustomerSpecificField4Value, bool ShowCustomerSpecificField1Value,
								bool ShowCustomerSpecificField2Value, bool ShowCustomerSpecificField3Value, bool ShowCustomerSpecificField4Value,
								string ClientPONumber, short CreatedByEmployeeID, DateTime CreatedDateTime, bool PrintingProject = false);

        Task<bool> QuoteExists(int QuoteId);

		Task<QuoteItem> CreateQuoteItem(int QuoteID, byte LanguageServiceID, string SourceLanguageIANAcode, string TargetLanguageIANAcode,
										int WordCountNew, int WordCountFuzzyBand1, int WordCountFuzzyBand2, int WordCountFuzzyBand3, int WordCountFuzzyBand4,
										int WordCountExact, int WordCountRepetitions, int WordCountPerfectMatches, int Pages, int Characters,
										int Documents, int InterpretingExpectedDurationMinutes, string InterpretingLocationOrgName,
										string InterpretingLocationAddress1, string InterpretingLocationAddress2,
										string InterpretingLocationAddress3, string InterpretingLocationAddress4,
										string InterpretingLocationCountyOrState, string InterpretingLocationPostcodeOrZip,
										short? InterpretingLocationCountryID, int AudioMinutes, int WorkMinutes, string ExternalNotes,
										decimal? ChargeToClient, short CreatedByEmployeeID, int SupplierWordCountNew, int SupplierWordCountFuzzyBand1,
										int SupplierWordCountFuzzyBand2, int SupplierWordCountFuzzyBand3, int SupplierWordCountFuzzyBand4,
										int SupplierWordCountExact, int SupplierWordCountRepetitions, int SupplierWordCountPerfectMatches,
										int WordCountClientSpecific = 0, int SupplierWordCountClientSpecific = 0,int languageServiceCategoryId = 0);

		Task<Quote> GetCurrentQuote(int enquiryId);

		Task<List<QuoteItem>> GetAllQuoteItems(int quoteId);

		Task<Quote> GetQuoteById(int quoteId);
		List<int> GetAllQuotesForApprovalPage(int contactID, int orgID, int orgGroupID , int start, int pageSize);
		string ExtranetAndWSDirectoryPathForApp(int quoteId);
		Task<int> UpdateQuote(int QuoteID, int EnquiryID, bool IsCurrentVersion, Int16 QuoteCurrencyID, string LangIANACode, string Title, string QuoteFileName, string InternalNotes, DateTime QuoteDate, string QuoteOrgName, string QuoteAddress1, string QuoteAddress2, string QuoteAddress3, string QuoteAddress4, string QuoteCountyOrState, string QuotePostcodeOrZip, Int16 QuoteCountryID, string AddresseeSalutationName, string OpeningSectionText, string ClosingSectionText, Int16 TimelineUnit, decimal TimelineValue, Int16 WordCountPresentationOption, bool ShowInterpretingDurationInBreakdown, bool ShowWorkDurationInBreakdown, bool ShowPagesOrSlidesInBreakdown, bool ShowNumberOfCharactersInBreakdown, bool ShowNumberOfDocumentsInBreakdown, Int16 SalesContactEmployeeID, string CustomerSpecificField1Value, string CustomerSpecificField2Value, string CustomerSpecificField3Value, string CustomerSpecificField4Value, bool ShowCustomerSpecificField1Value, bool ShowCustomerSpecificField2Value, bool ShowCustomerSpecificField3Value, bool ShowCustomerSpecificField4Value, string ClientPONumber, DateTime LastModifiedDateTime, Int16 LastModifiedByEmployee, bool PrintingProject = false, Int16 AssignedToEmployee = 0);
		Task<int> MarkQuoteCurrentVersion(int QuoteID, int EnquiryID);
		Task<ViewModels.Quotes.QuotesViewModel> GetViewModelById(int quoteId);
		Task<Quote> UpdateQuoteInformation(int quoteID, short QuoteCurrencyId, string LangIanacode,
									string Title, string QuoteFileName, string InternalNotes, string OpeningSectionText, string ClosingSectionText, short lastModifiedByEmployeeID);
		Task<Quote> UpdateQuoteOptions(int quoteID, double TimelineValue, byte TimelineUnit, byte WordCountPresentationOption, bool ShowInterpretingDurationInBreakdown, bool ShowWorkDurationInBreakdown, bool ShowPagesOrSlidesInBreakdown, bool ShowNumberOfCharactersInBreakdown, bool ShowNumberOfDocumentsInBreakdown,
									 string ClientPonumber, string CustomerSpecificField1Value, string CustomerSpecificField2Value, string CustomerSpecificField3Value, string CustomerSpecificField4Value, bool ShowCustomerSpecificField1Value,
									 bool ShowCustomerSpecificField2Value, bool ShowCustomerSpecificField3Value, bool ShowCustomerSpecificField4Value, string AddresseeSalutationName, short lastModifiedByEmployeeID);
		Task<Quote> UpdateQuotePDFDetails(int quoteID, DateTime QuoteDate, string QuoteOrgName, string QuoteAddress1, string QuoteAddress2, string QuoteAddress3, string QuoteAddress4, string QuoteCountyOrState, string QuotePostcodeOrZip,
									 short QuoteCountryId, string Title, string OpeningSectionText, string ClosingSectionText, short SalesContactEmployeeId, short lastModifiedByEmployeeID);
		Task<Quote> UpdateSurchargeID(int Id, int surchargeId, short? lastModifiedByUserId);
		Task<Quote> UpdateDiscountID(int Id, int discountId, short? lastModifiedByUserId);

	}
}
