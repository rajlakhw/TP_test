using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Services.Common;
using ViewModels.JobItem;
using Global_Settings;
using System;
using ViewModels.JobOrder;
using ViewModels.Common;

namespace Services.Interfaces
{
    public interface ITPJobItemService : IService
    {
        Task<JobItem> GetById(int JobItemID);
        Task<JobItemViewModel> GetViewModelById(int id);
        Task<JobItemUpdateModel> Update(JobItemUpdateModel item);
        Task<List<JobItem>> GetByJobOrderId(int JobOrderID);
        Task<List<JobItem>> GetBySourceLangForJobOrderId(int JobOrderID, string SourceLanguageIANACode);
        Task<List<JobItem>> GetByTargetLangForJobOrderId(int JobOrderID, string TargetLanguageIANACode);
        System.Threading.Tasks.Task GenerateJobBrief(int jobItemId);
        Task<JobItem> UpdateJobItemKeyInformation(int jobitemid, System.DateTime Deadline, decimal ChargeToClient, decimal PaymentToSupplier, bool MinSupplierChargeApplied, int WorkMinutes, int NewClientWordCount, int NewSupplierWordCount, bool Visible, short suppliercurrencyID);
        Task<JobItem> UpdateJobItemClientDeadline(int jobitemid, System.DateTime Deadline);

        Task<JobItem> CreateItem(int OrderID, bool IsVisibleToClient, byte LanguageServiceID, string SourceLangIANACode,
                                        string TargetLangIANACode, int WordCountNew, int WordCountFuzzyBand1,
                                        int WordCountFuzzyBand2, int WordCountFuzzyBand3, int WordCountFuzzyBand4,
                                        int WordCountExact, int WordCountRepetitions, int WordCountPerfectMatches,
                                        Enumerations.TranslationMemoryRequiredValues TranslationMemoryRequiredValues,
                                        int Pages, int Characters, int Documents, int InterpretingExpectedDurationMinutes,
                                        string InterpretingLocationOrgName, string InterpretingLocationAddress1,
                                        string InterpretingLocationAddress2, string InterpretingLocationAddress3,
                                        string InterpretingLocationAddress4, string InterpretingLocationCountyOrState,
                                        string InterpretingLocationPostcodeOrZip, byte? InterpretingLocationCountryID,
                                        int AudioMinutes, int WorkMinutes, DateTime SupplierSentWorkDateTime,
                                        DateTime SupplierCompletionDeadline, DateTime OurCompletionDeadline,
                                        string DescriptionForSupplierOnly, string FileName, bool SupplierIsClientReviewer,
                                        int? LinguisticSupplierOrClientReviewerID, decimal ChargeToClient,
                                        decimal PaymentToSupplier, byte? PaymentToSupplierCurrencyID, DateTime SupplierInvoicePaidDate,
                                        byte SupplierInvoicePaidMethodID, short CreatedByEmployeeID, int SupplierWordCountNew,
                                        int SupplierWordCountFuzzyBand1, int SupplierWordCountFuzzyBand2,
                                        int SupplierWordCountFuzzyBand3, int SupplierWordCountFuzzyBand4, int SupplierWordCountExact,
                                        int SupplierWordCountRepetitions, int SupplierWordCountPerfectMatches,
                                        bool SupplierWordCountsTakenFromClient, string CustomerSpecificField = "",
                                        string ContextFieldsList = "", int WordCountClientSpecific = 0,
                                        int SupplierWordCountClientSpecific = 0, bool FromExternalServer = false,
                                        int ClientCostCalculatedByID = 0, DateTime? ClientCostCalculatedByDateTime = null,
                                        bool ClientCostCalculatedByPriceList = false, int SupplierCostCalculatedByID = 0,
                                        DateTime? SupplierCostCalculatedByDateTime = null,
                                        bool MinimumSupplierChargeApplied = false,
                                        bool SupplierCostCalculatedByPriceList = false, byte? LanguageServiceCategoryId = 0);
        Task<JobItem> CreateJobItem(JobItem model);
        Task<List<JobItem>> CheckAvailableTargetLanguages(string sourceLanguageSelected, int jobOrderID);
        System.Threading.Tasks.Task configurenetworkfolders(int jobitemid, bool fromexternalserver = false);
        System.Threading.Tasks.Task DeliveryFolderCheck(string OldFolderNumber, string NewFolderNumber,
                                    string InternalJobDrivePath, int jobitemid);
        System.Threading.Tasks.Task CreateClientReviewSubfolders(string ClientReviewFolderNumber, string InternalDriveJobPath, int jobitemid);
        System.Threading.Tasks.Task CreateTranslationSubfolders(string InternalDriveJobPath, int jobitemid);
        Task<int> UpdateReviewerOfJobItem(int jobItemId, int reviewerId);
        string ExtranetTranslateOnlineFromTranslationDirectoryPathForApp(int jobitemId);
        string ExtranetTranslateOnlineOverallDirectoryPathForApp(int jobitemId);
        string ExtranetReviewPlusFromReviewDirectoryPathForApp(int jobItemID);
        string ExtranetFolderForSupplierPath(int jobItemId);
        Task<bool> UpdateClientReviewStatus(int jobItemId, Enumerations.ReviewStatus NewStatus, string NotesFromClientReviewer = "");
        Task<JobItem> ApplyToJobItem(int jobItemID, int orgGroupID, int orgID, ViewModels.EmployeeModels.EmployeeViewModel EmployeeCurrentlyLoggedOn, TPWordCountBreakdownBatchModel model, bool applyToClientWordCounts = true, bool applyToClientWordCountsOnly = false);
        Task<bool> ItemExists(int itemId);
        Task<JobItemViewModel> RemoveItem(JobItemViewModel item);
        Task<bool> SetupFlowPlusLicencingJobOrder(bool SetUpOneJobOrderForAllLicences, int LicenceMappingId = 0, int LicenceId = 0);
    }
}
