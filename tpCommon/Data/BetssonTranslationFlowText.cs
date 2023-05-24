using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class BetssonTranslationFlowText
    {
        public int Id { get; set; }
        public int TprequestId { get; set; }
        public int TextId { get; set; }
        public string TextName { get; set; }
        public string TextDescription { get; set; }
        public string TextContent { get; set; }
        public bool IncludeForTranslation { get; set; }
        public DateTime? CommentCheckedDateTime { get; set; }
        public int? CommentCheckedByEmployeeId { get; set; }
    }
}
