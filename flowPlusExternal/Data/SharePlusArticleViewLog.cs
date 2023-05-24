using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class SharePlusArticleViewLog
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public DateTime ViewedDateTime { get; set; }
        public short ViewedByEmployeeId { get; set; }
        public bool? ArtilcleMarkedAsHelpful { get; set; }
        public bool? ArtilcleMarkedAsNotHelpful { get; set; }
    }
}
