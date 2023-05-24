using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ReviewPlusSignOffJobItem
    {
        public int Id { get; set; }
        public int? JobItemId { get; set; }
        public int? JobOrderId { get; set; }
        public bool? IsJobItemDesignPlusReview { get; set; }
        public int? ClientReviewerId { get; set; }
        public int? LanguageServiceId { get; set; }
        public string SourceLanguageIanacode { get; set; }
        public string TargetLanguageIanacode { get; set; }
        public int? QuestionId { get; set; }
        public string ReviewGrade { get; set; }
        public int? CreatedByEmployeeId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? DeletedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
