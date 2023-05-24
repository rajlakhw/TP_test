using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.JobItem
{
    public class JobItemDetailsModel
    {
        public int JobItemId { get; set; }
        public string LanguageService { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string Translator { get; set; }
        public string Reviewer { get; set; }
        public string FileName { get; set; }
        public string Cost  { get; set; }
        public string Status { get; set; }
        public string ProofReadingCompDate { get; set; }
        public bool IsDownloaded { get; set; }
        public string Progress { get; set; }
        public bool IsSubmitForReview { get; set; }
        public List<ReviewerContact> Editor { get; set; }
        public string SupplierCompletionDeadline { get; set; }
    }

    public class ReviewerContact
    {
        public int ContactId { get; set; }
        public string ReviewerName { get; set; }
    }
}
