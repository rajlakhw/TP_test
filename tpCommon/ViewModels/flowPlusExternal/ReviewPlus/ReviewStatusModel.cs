using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.flowPlusExternal.ReviewPlus
{
    public class ReviewStatusModel
    {
        public string RequestedBy { get; set; }
        public string Reviewer { get; set; }
        public int JobItemId { get; set; }
        public string JobName { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public bool AllowPriority { get; set; }
        public byte? Priority { get; set; }
        public DateTime DueDate { get; set; }
        public int SupplierWordCountTotal { get; set; }
        public string ReviewProgress { get; set; }
        public DateTime? SupplierCompletionDeadline { get; set; }
        public DateTime? SupplierSentWorkDateTime { get; set; }
        public DateTime? SupplierAcceptedWorkDateTime { get; set; }
    }

    public class PendingAndInProgressReviewModel
    {
        public List<ReviewStatusModel> PendingReviewDocuments { get; set; }
        public List<ReviewStatusModel> InProgressReviewDocuments { get; set; }
    }

    public class ReviewSignOffModel
    {
        public int JobItemId { get; set; }
        public int TotalNumberOfChangedReviewSegments { get; set; }
        public int TotalNumberOfReviewSegments { get; set; }
        public decimal PercentageOfChangedReviewSegments { get; set; }
        public string TranslationsChanged { get; set; }
        public bool SupplierAcceptedJobItem { get; set; }
    }
}
