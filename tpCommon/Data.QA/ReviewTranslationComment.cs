using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ReviewTranslationComment
    {
        public int Id { get; set; }
        public int JobItemId { get; set; }
        public string FileName { get; set; }
        public int Segment { get; set; }
        public string Comment { get; set; }
        public string TranslateOnlineComment { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string LastModifiedUserName { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public int? DeletedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
