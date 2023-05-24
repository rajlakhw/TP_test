using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class BetssonTranslationFlowRequest
    {
        public int Id { get; set; }
        public int TranslationFlowRequestId { get; set; }
        public string BetssonUniquePublicId { get; set; }
        public string RequestName { get; set; }
        public DateTime? DeadlineByClient { get; set; }
        public string TargetLanguage { get; set; }
        public string Urlstring { get; set; }
        public int TpjobOrderId { get; set; }
    }
}
