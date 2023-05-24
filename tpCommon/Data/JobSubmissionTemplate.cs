using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class JobSubmissionTemplate
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public bool IsQuoteTemplate { get; set; }
        public int DataObjectId { get; set; }
        public byte DataObjectTypeId { get; set; }
        public string SourceLangIanacode { get; set; }
        public bool? SourceLangShowMostRequestedLangsOnly { get; set; }
        public string TargetLangsIanacodes { get; set; }
        public bool? TargetLangShowMostRequestedLangsOnly { get; set; }
        public string ProjectName { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Notes { get; set; }
        public bool? CreateDifferentOrdersForEachLang { get; set; }
        public bool? RequiresReviewPlus { get; set; }
        public bool? RequiresDesignPlus { get; set; }
        public bool? NotifyReviewersOfDelivery { get; set; }
        public bool? RequiresDtp { get; set; }
        public short? DeadlineInDays { get; set; }
        public short? DeadlineHour { get; set; }
        public short? DeadlineMinute { get; set; }
        public string DeadlineTimeZone { get; set; }
        public bool? IsConfidential { get; set; }
        public short? CurrencyId { get; set; }
        public bool? RequiresProofreading { get; set; }
        public string InvoiceApprovingEntity { get; set; }
        public string InvoiceApproverInitials { get; set; }
        public string ProjectNumber { get; set; }
        public string CostCenter { get; set; }
        public bool? IsPrintingOrPackagingProject { get; set; }
        public string CustomField1Value { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByContactId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public int? LastModifiedByContactId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedByContactId { get; set; }
    }
}
