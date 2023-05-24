using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class DesignPlusTextChange
    {
        public int Id { get; set; }
        public int DesignPlusFileId { get; set; }
        public int? OriginalTextFrameId { get; set; }
        public int TextFrameId { get; set; }
        public int PageNumber { get; set; }
        public string OldText { get; set; }
        public string NewText { get; set; }
        public string OldTextWithTags { get; set; }
        public string NewTextWithTags { get; set; }
        public byte Status { get; set; }
        public int Version { get; set; }
        public DateTime ChangeDateTime { get; set; }
        public int ChangedBy { get; set; }
        public bool IsClientReviewChange { get; set; }
        public string LabelName { get; set; }
        public string LabelName1 { get; set; }
    }
}
