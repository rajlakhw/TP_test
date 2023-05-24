using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ClientDesignFile
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string DocumentFileType { get; set; }
        public int? IndesignDocumentId { get; set; }
        public int? IndesignReviewDocumentId { get; set; }
        public int? WorkingCopyDocumentId { get; set; }
        public int? WorkingCopyReviewDocumentId { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int TotalNumberOfSpreads { get; set; }
        public string AdditionalComment { get; set; }
        public int? JobOrderId { get; set; }
        public int? JobItemId { get; set; }
        public bool? IsInternalTranslatedCopy { get; set; }
        public DateTime UploadedDateTime { get; set; }
        public int UploadedByClientId { get; set; }
        public int? ReviewId { get; set; }
        public bool FontsUpdatePending { get; set; }
        public DateTime? SentForReviewDateTime { get; set; }
        public int? SentForReviewBy { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public int? LastModifiedByClientId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedByClientId { get; set; }
        public DateTime? CommentUpdatedDateTime { get; set; }
        public int? CommentUpdatedBy { get; set; }
        public int FolderId { get; set; }
        public bool? AutosaveOn { get; set; }
        public short? AutosaveTime { get; set; }
        public bool? ReviewAutoSaveOn { get; set; }
        public short? ReviewAutoSaveTime { get; set; }
        public string SourceLangCode { get; set; }
        public string TargetLangCode { get; set; }
        public int? ParentDesignPlusId { get; set; }
        public short? IdsportNumber { get; set; }
        public short? ReviewDocIdsportNumber { get; set; }
        public short? IdsdelPortNumber { get; set; }
        public short? RvIdspdelPortNumber { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public int? CompletedByClientId { get; set; }
        public int? SegmentsAutomaicallyAligned { get; set; }
        public int? SourceSegmentsForManualAlignment { get; set; }
        public int? TargetSegmentsForManualAlignment { get; set; }
        public DateTime? TmalignedDateTime { get; set; }
    }
}
