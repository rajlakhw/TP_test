using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class SharePlusArticle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Htmlbody { get; set; }
        public string Contents { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmpId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmpId { get; set; }
        public DateTime? LastViewedDateTime { get; set; }
        public short? LastViewedByEmpId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public int HistoricalNumberOfViews { get; set; }
        public int? NumberOfTimesViewed { get; set; }
        public bool IsPinnedArticle { get; set; }
    }
}
