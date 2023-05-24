using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class JobOrder
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public short ProjectManagerEmployeeId { get; set; }
        public byte JobOrderChannelId { get; set; }
        public string JobName { get; set; }
        public string ClientNotes { get; set; }
        public string InternalNotes { get; set; }
        public string ClientPonumber { get; set; }
        public string CustomerSpecificField1Value { get; set; }
        public string CustomerSpecificField2Value { get; set; }
        public DateTime SubmittedDateTime { get; set; }
        public DateTime OverallDeliveryDeadline { get; set; }
        public DateTime? OverallCompletedDateTime { get; set; }
        public short ClientCurrencyId { get; set; }
        public int? ClientInvoiceId { get; set; }
        public decimal? OverallChargeToClient { get; set; }
        public int? LinkedJobOrderId { get; set; }
        public byte? ClientApprovalRequirementStatus { get; set; }
        public DateTime? ClientApprovedByDateTime { get; set; }
        public int? ClientApprovedByContactId { get; set; }
        public bool ExtranetNotifyClientReviewersOfDeliveries { get; set; }
        public bool IsAtrialProject { get; set; }
        public bool IsAcmsproject { get; set; }
        public bool IsHighlyConfidential { get; set; }
        public short? SetupByEmployeeId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public string DeletedFreeTextReason { get; set; }
        public bool IsActuallyOnlyAquote { get; set; }
        public int? OriginatedFromEnquiryId { get; set; }
        public int? DiscountId { get; set; }
        public int? SurchargeId { get; set; }
        public decimal? SubTotalOverallChargeToClient { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? SurchargeAmount { get; set; }
        public DateTime? InternalHighLowMarginApprovedDateTime { get; set; }
        public short? InternalHighLowMarginApprovedByEmployeeId { get; set; }
        public string InternalHighLowMarginApprovedNotes { get; set; }
        public decimal? OverallChargeToClientForMarginCalucation { get; set; }
        public byte? ExtranetJobOrderStatus { get; set; }
        public int? DesignPlusFileId { get; set; }
        public byte? Priority { get; set; }
        public decimal? AnticipatedFinalValueAmount { get; set; }
        public decimal? FinalValueAdjustment { get; set; }
        public string InvoicingNotes { get; set; }
        public decimal? OverallAnticipatedChargeToClientForMarginCalucation { get; set; }
        public string CustomerSpecificField3Value { get; set; }
        public bool? OnHold { get; set; }
        public int? OnHoldByEmployee { get; set; }
        public DateTime? OnHoldDateTime { get; set; }
        public int? OffHoldByEmployee { get; set; }
        public DateTime? OffHoldDateTime { get; set; }
        public bool ToSendRefFilesToReviewer { get; set; }
        public DateTime? EonfilesDeletedDateTime { get; set; }
        public string CustomerSpecificField4Value { get; set; }
        public DateTime? EarlyInvoiceDateTime { get; set; }
        public short? EarlyInvoiceByEmpId { get; set; }
        public DateTime? CommissionProcessedDateTime { get; set; }
        public short? CommissionProcessedByEmployeeId { get; set; }
        public byte? OverdueReasonId { get; set; }
        public string OverdueComment { get; set; }
        public string OrgHfmcodeIs { get; set; }
        public string OrgHfmcodeBs { get; set; }
        public DateTime? OriginalOverallDeliveryDeadline { get; set; }
        public bool? PrintingProject { get; set; }
        public bool? ArchivedToLionBox { get; set; }
        public DateTime? ArchivedToLionBoxDateTime { get; set; }
        public DateTime? OverdueCommentUpdate { get; set; }
        public string DeadlineChangeReason { get; set; }
        public decimal? GrossMarginPercentage { get; set; }
        public decimal? OverallSterlingPaymentToSuppliers { get; set; }
        public decimal? OverallSterlingPaymentToSuppliersForMarginCalc { get; set; }
        public decimal? AnticipatedGrossMarginPercentage { get; set; }
        public byte? LionBoxArchivingStatus { get; set; }
        public DateTime? ArchivedToAmazonS3dateTime { get; set; }
        public DateTime? ArchivedDateTime { get; set; }
        public int? PreTranslateFromTemplateId { get; set; }
        public int? SaveTranslationsToTemplateId { get; set; }
        public bool? IsMachineTranslationJobFromiPlus { get; set; }
        public byte? MachineTranslationEngineSelected { get; set; }
    }
}
