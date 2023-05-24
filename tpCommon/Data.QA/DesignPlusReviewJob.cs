using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class DesignPlusReviewJob
    {
        public int Id { get; set; }
        public int DesignPlusFileId { get; set; }
        public int ReviewerId { get; set; }
        public DateTime SentForReviewDateTime { get; set; }
        public int SentForReviewByContact { get; set; }
        public byte? ReviewStatus { get; set; }
        public DateTime? ReviewCompletedDateTime { get; set; }
        public int? ReviewCompletedByContact { get; set; }
        public DateTime? ReviewDeadline { get; set; }
        public string NotesToReviewer { get; set; }
        public string ApprovalNotes { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedBy { get; set; }
        public int? JobItemId { get; set; }
    }
}
