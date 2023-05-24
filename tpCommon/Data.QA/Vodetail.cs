using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class Vodetail
    {
        public int Id { get; set; }
        public bool QuoteOrJob { get; set; }
        public int DataObjectId { get; set; }
        public int? ProjectType { get; set; }
        public string Service { get; set; }
        public string Category { get; set; }
        public string EndClient { get; set; }
        public string Brand { get; set; }
        public string Campaign { get; set; }
        public string Channel { get; set; }
        public int? NumberOfTranscreationAssets { get; set; }
        public int? NumberOfAssets { get; set; }
        public string UsageRights { get; set; }
        public int? UsageDuration { get; set; }
        public DateTime? LiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? VoprojectType { get; set; }
        public int? Vocomplexity { get; set; }
    }
}
