using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ReviewTranslation
    {
        public int Id { get; set; }
        public int JobItemId { get; set; }
        public string FileName { get; set; }
        public int Segment { get; set; }
        public string SourceText { get; set; }
        public string TranslationBeforeReview { get; set; }
        public string TranslationDuringReview { get; set; }
        public string TranslationBeforeReviewCollapsedTags { get; set; }
        public string TranslationDuringReviewCollapsedTags { get; set; }
        public bool ReviewStatus { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public int CreatedByEmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedUserName { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public int? OriginalMatchPercentage { get; set; }
        public string OriginalMatchPercentCssclass { get; set; }
        public string ContextFieldIds { get; set; }
        public bool? Locked { get; set; }
        public string ContentType { get; set; }
        public string ContextInfo { get; set; }
        public string KeyContextInfo { get; set; }
        public bool? IsMttranslated { get; set; }
        public string MttranslationReview { get; set; }
        public string MttranslationReviewCollapsedTags { get; set; }
    }
}
