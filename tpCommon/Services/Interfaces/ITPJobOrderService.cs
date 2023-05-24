using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using Data;
using Data;
using Microsoft.AspNetCore.Mvc;
using Services.Common;
using ViewModels.JobItem;
using ViewModels.JobOrder;
using ViewModels.flowPlusExternal;
using ViewModels.flowPlusExternal.ReviewPlus;
using Global_Settings;

namespace Services.Interfaces
{
    public interface ITPJobOrderService : IService
    {
        Task<JobOrder> GetById(int JobOrderID);
        Task<JobOrderViewModel> GetViewModelById(int JobOrderID);
        String NetworkDirectoryPathForApp(int JobOrderID, bool ForUser = false, DateTime? SubmittedDateTime = null);
        String NetworkDirectoryPathForApp(int JobOrderID, int OrgID, byte JobServerLocation, bool ForUser, DateTime? SubmittedDateTime = null);
        Task<JobOrder> UpdateKeyInformation<JobOrderViewteModel>(int Id, string JobName, DateTime deadline, short? lastModifiedByUserId, bool markcompleted, byte? OverdueReasonID, string OverdueComment, bool? IsCls, int? TypeOfOrder, bool? EscalatedToAccountManager, string? DeadlineChangeReason,int? LinkedJobOrder);
        Task<JobOrder> UpdateProjectInformation<JobOrderViewteModel>(int Id, short ProjectManagerId, byte JobOrderChannelId, string ClientNotes, string InternalNotes, bool IsATrialProject, bool? IsAPrintingProject, bool IsHighlyConfidential, byte? Priority, short? lastModifiedByUserId);
        Task<JobOrder> UpdateFinancialInformation<JobOrderViewteModel>(int Id, string InvoicingNotes, string ClientPonumber, DateTime? EarlyInvoiceDateTime, short? EarlyInvoiceByEmpId, string CustomerSpecificField1Value, string CustomerSpecificField2Value, string CustomerSpecificField3Value, string CustomerSpecificField4Value, short? lastModifiedByUserId,string OrgHfmcodeIs, string OrgHfmcodeBs,short ClientCurrency);
        Dictionary<string, string> AllTemplatePaths(int JobOrderID, int OrgID, byte JobServerLocation, bool ForUser, DateTime? SubmittedDateTime = null);
        Task<IEnumerable<JobItemsDataTableViewModel>> GetJobItems(int JobOrderId);
        Task<IEnumerable<JobOrderDataTableViewModel>> GetAllJobOrdersForDataObjectAndType(int dataObjectId, int dataTypeId, int pageNumber, int pageSize, string searchTerm, int columnToOrderBy, string orderDirection);
        Task<(int, int)> GetAllJobOrdersCountForDataObjectAndType(int dataObjectId, int dataTypeId, string searchTerm);
        Task<int> GetNumberOfOpenJobOrdersForClient(string extranetUserName);
        Task<decimal> GetValueOfOpenJobOrdersForClient(string extranetUserName);
        Task<int> GetNumberOfJobOrdersServiceInProgressForClient(string extranetUserName);
        Task<decimal> GetValueOfJobOrdersServiceInProgressForClient(string extranetUserName);
        Task<int> GetNumberOfJobOrdersInReviewForClient(string extranetUserName);
        Task<decimal> GetValueOfJobOrdersInReviewForClient(string extranetUserName);
        Task<int> GetNumberOfJobOrdersInFinalChecksForClient(string extranetUserName);
        Task<decimal> GetValueOfJobOrdersInFinalChecksForClient(string extranetUserName);
        Task<int> GetNumberOfJobOrdersReadyToCollectForClient(string extranetUserName);
        Task<decimal> GetValueOfJobOrdersReadyToCollectForClient(string extranetUserName);
        Task<List<int>> GetAllJobOrdersWithServiceInProgressForClient(string extranetUserName);
        Task<List<int>> GetAllOpenJobOrdersForClient(string extranetUserName);
        Task<int> GetNumberOfJobItemsInOpenOrdersForClient(string extranetUserName);
        Task<int> GetNumberOfCompleteJobItemsInOpenOrdersForClient(string extranetUserName);
        Task<int> GetWordCountOfAllJobItemsInOpenOrdersForClient(string extranetUserName);
        Task<int> GetWordCountOfCompleteJobItemsInOpenOrdersForClient(string extranetUserName);
        Task<decimal> GetJobOrderProgress(int JobOrderID);
        Task<OrderDetailModel> GetOrderDetails(int JobOrderId);
        Task<List<JobItemDetailsModel>> GetJobItemDetails(int JobOrderId);
        Task<int> UpdatePriority(int jobOrderId, string priority, string extranetUserName);
        Task<int> CancelJobOrder(int jobOrderId, string extranetUserName, string cancelComments);
        Task<int> SubmitReview(int jobItemId, int reviewerId, DateTime? deadlineDate, string extranetUserName);
        Task<(int, int)> GetAllJobOrdersCountForDataObjectAndTypeWithDateRange(int dataObjectId, int dataTypeId, string searchTerm, string startDate, string endDate);
        Task<IEnumerable<JobOrderDataTableViewModel>> GetAllJobOrdersForDataObjectAndTypeWithDateRange(int dataObjectId, int dataTypeId, int pageNumber, int pageSize, string searchTerm, int columnToOrderBy, string orderDirection, string startDate, string endDate);
        FileContentResult DownloadOrderSourceFile(int jobOrderId, string extranetUserName, string fileDownloadType, string fileIndex);
        Task<FileContentResult> DownloadJobOrder(int jobOrderId, string extranetUserName);
        Task<FileContentResult> DownloadJobItem(int jobItemId, string extranetUserName);
        Task<JobOrder> UpdateEndclientInformation<JobOrderViewteModel>(int Id, int endclientid, int campaignid, int brandid, int categoryid, short? lastModifiedByUserId);
        Task<JobOrder> UpdateSurchargeID<JobOrderViewteModel>(int Id, int surchargeId, short? lastModifiedByUserId);
        Task<JobOrder> UpdateDiscountID<JobOrderViewteModel>(int Id, int discountId, short? lastModifiedByUserId);

        Task<JobOrder> CreateNewOrder<Order>(int contactId, short projectManagerId, byte jobOrderChannelId, string jobName,
                                                    string clientNotes, string internalNotes, string clientPONumber,
                                                    string customerSpecificField1Value, string customerSpecificField2Value,
                                                    string customerSpecificField3Value, string customerSpecificField4Value,
                                                    DateTime overallDeliveryDeadlineDate, int overallDeliveryDeadlineHour,
                                                    int overallDeliveryDeadlineMinute, short clientCurrencyID,
                                                    int linkedJobOrderID, bool isATrialProject, bool isACMSProject,
                                                    bool isHighlyConfidential, short setUpByEmployeeID, bool isActuallyOnlyAQuote,
                                                    bool createNetworkJobFolder, string folderTemplateName, string invoicingNotes,
                                                    string orgHFMCodeIS = "", string orgHFMCodeBS = "",
                                                    bool extranetNotifyClientReviewersOfDeliveries = false,
                                                    byte priority = 0, bool printingProject = false,
                                                    int endClientID = 0, int brandID = 0,
                                                    int categoryID = 0, int campaignID = 0, bool isCLS = false,int? DiscountId = null, int? SurchargeId = null, decimal? DiscountAmount = 0, decimal? SurchargeAmount = 0, decimal? OverallChargeToClient=0, decimal? SubTotalOverallChargeToClient=0);

        Task<bool> configureNetworkFolders(string FolderTemplateName, int jobOrderId);

        string ExtranetAndWSDirectoryPathForApp(int jobOrderId);

        int CopyReferenceFiles(List<FileModel> refFiles, string ExternalRefPath, bool isDesignPlusJob = false, bool OkToDeleteTempRefFile = true);

        Task<bool> OrderExists(int orderId);

        void AnnounceThisOrderCreation(int orderId, string SendingEmailAddress, string SubjectLine, bool ExternalNotification, bool AcknowledgeAsQuote,
                                             string UILangIANACode = "", string CustomerSpecificMessage = "", bool IsFileInDTPFormat = true,
                                             bool QuoteApprovalNotification = false, bool IsPrintingProject = false,
                                             bool SendTranslateOnlineJobAutomatically = false, ExtranetUsersTemp extranetUser = null);

        string GetTotalSourceLangsCountString(int orderId, string IANACode);

        //Task<List<JobItem>> GetBySourceLangForJobOrderId(int JobOrderID, string SourceLanguageIANACode);
        //Task<List<JobItem>> GetByTargetLangForJobOrderId(int JobOrderID, string TargetLanguageIANACode);

        Task<List<JobItem>> GetAllJobItems(int JobOrderID);
        Task<ReviewSignOffModel> GetSignOffData(int jobItemId);

        Task<string> AutomateFinancialExpressExcel(int EmployeeID, int OrderID);
        Task<string> AutomateFinancialExpressMergeExcel(int EmployeeID, int OrderID);
        Task<int> ApproveReview(string extranetUserName, int jobItemId, string comments);
        Task<IEnumerable<JobOrderDataTableViewModel>> GetAllJobOrdersForOrderStatusPage(int dataObjectId, int dataTypeId, int pageNumber, int pageSize, string searchTerm, int columnToOrderBy, string orderDirection, string startDate, string endDate);
        Task<IEnumerable<JobOrderDataTableViewModel>> GetAllJobOrdersForQuickProjOverview(int dataObjectId, int dataTypeId, int pageNumber, int pageSize, string searchTerm, int columnToOrderBy, string orderDirection);
        Task<String> GetCompletionStatusStringForJobOrder(int jobOrderId, string languageIANACode);
        Task<(int, int)> GetAllJobOrdersCountForOrderStatusPage(int dataObjectId, int dataTypeId, string searchTerm, string startDate, string endDate);
        Task<(int, int)> GetAllJobOrdersCountForQuickProjOverview(int dataObjectId, int dataTypeId, string searchTerm);
        Task<JobOrder> CreateJobOrder(JobOrder model);
        Task<JobOrder> UpdateOrder(int ID, Int16 ProjectManagerEmployeeID, Int16 OrderChannelID, string JobName, string ClientNotes, string InternalNotes, string ClientPONumber, string CustomerSpecificField1Value, string CustomerSpecificField2Value, string CustomerSpecificField3Value, string CustomerSpecificField4Value, DateTime OverallDeliveryDeadlineDate, int OverallDeliveryDeadlineHour, int OverallDeliveryDeadlineMinute, bool OverallIsComplete, Int16 ClientCurrencyID, int LinkedJobOrderID, int ClientInvoiceID, bool IsATrialProject, bool IsACMSProject, bool IsHighlyConfidential, string InvoicingNotes, DateTime EarlyInvoiceDateTime, int EarlyInvoiceByEmpID, int OverdueReasonID, string OverdueReasonComment, string OrgHFMCodeIS = "", string OrgHFMCodeBS = "", bool PrintingProject = false, DateTime OriginalOverallDeliveryDeadline = default(DateTime), string DeadlineChangeReason = "", Int16 Priority = 0, bool EscalatedToAccountManager = false, int EndClientID = 0, int BrandID = 0, int CategoryID = 0, int CampaignID = 0, bool IsCLS = false);
        List<int> GetJobOrderIDsByExactNamePOOrNotes(string JobNameSearchStr, string PONumSearchStr, string NotesSearchStr, DateTime SubmittedStartDate, DateTime SubmittedEndDate, int OrgID = 0, bool AlsoCheckForDeletedJobs = false, bool GetJobsForArchiving = false);
        Task<JobOrder> UpdateMachineTranslationTemplateDetails(int jobOrderId, int preTranslateFromTemplateId, int saveTranslationsToTemplateId, byte MTEngine);
        Task<List<DeadlineChangeReason>> GetAllDeadlineChangeReason();
    }
}
