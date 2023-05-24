using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using ViewModels.Common;

namespace ViewModels.Enquiries
{
    public class PendingOrNotStartedEnquiries
    {
        public int EnquiryID { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime? clientDeadline { get; set; }
        public string orgName { get; set; }
        public string orgStatus { get; set; }
        public int enqPriorityID { get; set; }
        public int orgID { get; set; }
        public string assignedToEmployeeName { get; set; }
        public int assignedToEmployeeID { get; set; }

        public string SalesOwner { get; set; }
        public int SalesOwnerID { get; set; }
        public string enquiryStatus { get; set; }
        public string enquiryNotes { get; set; }
        public string orgSLA { get; set; }
        public string OrgID { get; set; }
        public string enqfileExtentions { get; set; }
        public string sourceLanguages { get; set; }
        public string targetLanguages { get; set; }
        public string sourceLanguagesCount { get; set; }
        public string targetLanguagesCount { get; set; }
        public DateTime? EnquiryDeadline { get; set; }
        public int? WinChance { get; set; }
        public DateTime? ClientDecisionDate { get; set; }



    }

    public class GoneOrRejectedEnquiries
    {
        public int EnquiryID { get; set; }
        public DateTime createdDate { get; set; }
        public string orgName { get; set; }
        public string enquiryNotes { get; set; }

        public int orgID { get; set; }
        public string enquiryStatus { get; set; }
        public string enqReason { get; set; }
        public string assignedTo { get; set; }
        public string SalesOwner { get; set; }
        public int SalesOwnerID { get; set; }
        public decimal chargetToClient { get; set; }
        public decimal chargetToClientGBP { get; set; }
        public string enqPrefix { get; set; }
        public int approvedJobOrderID { get; set; }
    }
    public class SentEnquiries
    {
        public int EnquiryID { get; set; }
        public DateTime createdDate { get; set; }
        public string orgName { get; set; }
        public int orgID { get; set; }
        public string enquiryNotes { get; set; }
        public string assignedToEmployeeName { get; set; }
        public string SalesOwner { get; set; }
        public int SalesOwnerID { get; set; }
        public string enquiryStatus { get; set; }
        public int enqPriorityID { get; set; }
        public int assignedToID { get; set; }
        public decimal chargetToClient { get; set; }
        public decimal chargetToClientGBP { get; set; }
        public string enqPrefix { get; set; }

    }


    public class EnquiriesViewModel
    {
        public string enqToUpdate { get; set; }

        public string enqPriorityToUpdate { get; set; }

        public string enqAssignedToUpdate { get; set; }

        public string enqNotesToUpdate { get; set; }
        public IEnumerable<PendingOrNotStartedEnquiries> PendingOrNotStartedEnquiriesList { get; set; }

        public int? currentEmployeeForEnq { get; set; }
        public List<Employee> EmployeesList { get; set; }
        public IEnumerable<GoneOrRejectedEnquiries> GoneOrRejectedEnquiriesList { get; set; }

        public IEnumerable<SentEnquiries> SentEnquiriesList { get; set; }

        public string startDateSent { get; set; }
        public string endDateSent { get; set; }
        public string startDateApproved { get; set; }
        public string endDateApproved { get; set; }

        public int Id { get; set; }
        public int ContactId { get; set; }
        public byte OrderChannelId { get; set; }
        public byte Status { get; set; }
        public byte? DecisionReasonId { get; set; }
        public int? DecisionMadeByContactId { get; set; }
        public DateTime? DecisionMadeDateTime { get; set; }
        public string ExternalNotes { get; set; }
        public string InternalNotes { get; set; }
        public DateTime? DeadlineRequestedByClient { get; set; }
        public int? WentAheadAsJobOrderId { get; set; }
        public string JobName { get; set; }
        public string AdditionalDetails { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public DateTime? EnqFilesDeletedDateTime { get; set; }
        public bool? PrintingProject { get; set; }
        public DateTime? ArchivedToLionBoxDateTime { get; set; }
        public DateTime? ArchivedToAmazonS3dateTime { get; set; }
        public DateTime? ArchivedDateTime { get; set; }
        public int? AssignedToEmployeeID { get; set; }
        public int? PriorityID { get; set; }
        public DateTime? EnquiryDeadline { get; set; }

        public string ContactName { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public int? OrgSLA { get; set; }
        public DateTime? SLAdeadline { get; set; }
        public int OrgGroupId { get; set; }
        public string OrgGroupName { get; set; }
        public IEnumerable<JobOrderChannel> AllJobOrderChannels { get; set; }
        public List<Employee> ListOfEmployees { get; set; }
        public short? Ownership { get; set; }
        public List<LanguageService> LanguageServices { get; set; }
        public List<DropdownOptionViewModel> Languages { get; set; }
        public List<EnquiryQuoteItem> EnquiryQuoteItems { get; set; }
        public List<QuotesResults> QuotesResults { get; set; }
        public string EnquiryFolder { get; set; }
        public List<QuoteItem> QuoteItems { get; set; }
        public bool IsAllowedToEdit { get; set; }
        public bool IsFinalised { get; set; }
        public bool MarkForSales { get; set; }
        public bool MarkForOps { get; set; }
        public bool FinaliseEnquiry { get; set; }
        public string FinaliseEnquiryButtonText { get; set; }
        public bool LinkJobOrder { get; set; }
        public bool EditPageEnabled { get; set; }
        public string DeleteLink { get; set; }
        public string FinaliseEnquiryLink { get; set; }
        public string LinkJobOrderLink { get; set; }
        public string CreateAutoQuoteLink { get; set; }
        public bool MarkAsPending { get; set; }
        public string MarkAsPendingText { get; set; }
        public IQueryable<ClientDecisionReason> ClientDecisionReason { get; set; }
        public IEnumerable<DropdownOptionViewModel> LanguageServiceCategory { get; set; }
        public int? WinChance { get; set; }
        public DateTime? ClientDecisionDate { get; set; }
        public bool WebServiceDelivery { get; set; }
        public string WebServiceDeliveryLink { get; set; }

    }

    public class QuotesResults
    {
        public bool CreatedAutomatically { get; set; }
        public bool CurrentVersion { get; set; }
        public int QuoteID { get; set; }
        public string QuoteFileName { get; set; }
        public string Language { get; set; }
        public string InternalNotes { get; set; }
        public DateTime QuoteDate { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public string Prefix { get; set; }
        public decimal OverallChargeToClient { get; set; }
        public string SalesContact { get; set; }
        public short SalesContactID { get; set; }
        public string CreatedBy { get; set; }
        public int CreatedByID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LanguageIANACode { get; set; }
        public int LastModifiedEmployeeID { get; set; }
    }

    public class EnquiryQuoteItemsViewModel
    {
        public int Id { get; set; }
        public int LanguageServiceID { get; set; }
        public string SourceLanguageIANACode { get; set; }
        public string TargetLanguageIANACode { get; set; }
    }
}
