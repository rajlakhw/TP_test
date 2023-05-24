using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using ViewModels.Common;

namespace Services.Interfaces
{
    public interface ITPEnquiriesService : IService
    {
        Task<List<ViewModels.Enquiries.SentEnquiries>> GetSentEnquiries(string startDate = null, string endDate = null, int EmployeeID = 0);
        Task<int> GetNumberOfPendingEnquiriesForClient(string extranetUserName);
        Task<decimal> GetValueOfPendingEnquiriesForClient(string extranetUserName);
        Task<List<ViewModels.Enquiries.PendingOrNotStartedEnquiries>> GetPendingEnquiriesNotPGD(int EmployeeID = 0);
        Task<List<ViewModels.Enquiries.PendingOrNotStartedEnquiries>> GetPendingEnquiries(int EmployeeID = 0);
        Task<List<ViewModels.Enquiries.GoneOrRejectedEnquiries>> GetApprovedRejectedEnquiries(string startDate, string endDate, int EmployeeID = 0);
        string GetFileExtentions(int orgId, int enquiryID);

        Task<ViewModels.Enquiries.EnquiriesViewModel> Update(string enqToUpdate, string enqPriorityToUpdate, string enqAssignedToUpdate, string enqNotesToUpdate);

        Task<Enquiry> CreateEnquiry(int ContactID, byte OrderChannelID, string ExternalNotes, string InternalNotes, string JobName,
                                    short CreatedByEmployeeID, DateTime? DeadlineRequestedByClient = null, bool PrintingProject = false,
                                    bool FromExternalServer = false, DateTime? EnquiryDeadline = null, int? assignedToEmployeeID = null);

        Task<Enquiry> GetEnquiryById(int enquiryId);

        Task<bool> EnquiryExists(int enquiryId);

        string ExtranetAndWSDirectoryPathForApp(int jobOrderId);

        void AnnounceThisEnquiryCreation(int quoteId, string SendingEmailAddress, string SubjectLine, bool ExternalNotification, bool AcknowledgeAsQuote,
                                             string UILangIANACode = "", string CustomerSpecificMessage = "", bool IsFileInDTPFormat = true,
                                             bool IsPrintingProject = false);

        Task<string> EnquiryDirectoryPathForUser(int enquiryId);

        Task<string> EnquiryDirectoryPathForApp(int enquiryId);

        string GetTotalSourceLangsCountString(int quoteId, string IANACode);
        Task<List<Enquiry>> GetEnquiryByContactId(int contactId);
        Task<Enquiry> ApproveOrRejectEnquiry(Enquiry model);
        string CopyReferenceFilesFromExternalServer(string QuoteRefPath, string ExternalRefPath);
        Task<EnquiryQuoteItem> CreateEnquiryQuoteItems(int enquiryId, int languageServiceID, string sourceLangaugeIanaCode, string targetLangaugeIanaCode, short createdByEmployeeID,int languageServiceCategoryId = 0);

        Task<EnquiryQuoteItem> CreateMultipleEnquiryQuoteItems(int enquiryId, string quoteitems, short createdByEmployeeID);
        Task<EnquiryQuoteItem> RemoveQuoteItem(int enquiryquoteitemid, short deletedByEmployeeID);
        Task<Enquiry> UpdateKeyInformation(int enquiryID, byte orderChannelID, string jobName,
                                     short lastModifiedByEmployeeID, DateTime? deadlineRequestedByClient = null,
                                    DateTime? EnquiryDeadline = null, int? assignedToEmployeeID = null);
        Task<Enquiry> UpdateOptionalInformation(int enquiryID, string clientNotes, string internalNotes, string additionalDetails,
                                     short lastModifiedByEmployeeID, bool printingProject = false);

        Task<ViewModels.Enquiries.EnquiriesViewModel> GetViewModelById(int enquiryId);

        Task<List<ViewModels.Enquiries.QuotesResults>> GetQuotes(int EnquiryID);
        Task<Enquiry> UpdateStatus(int enqToUpdate, byte status, int contactID, short lastModifiedByEmployeeID);
        Task<Enquiry> UpdateJobOrder(int enqToUpdate, int joborderID, short lastModifiedByEmployeeID);
        Task<Enquiry> UpdateLogicalInformation(int enquiryID, int? winChance, DateTime? clientDecisionDate, short lastModifiedByEmployeeID);
    }
}
