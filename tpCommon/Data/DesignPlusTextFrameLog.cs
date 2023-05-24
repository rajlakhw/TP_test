using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class DesignPlusTextFrameLog
    {
        public int Id { get; set; }
        public int DesignPlusFileId { get; set; }
        public int? OriginalTextFrameId { get; set; }
        public int TextFrameId { get; set; }
        public int? PreviewVersionTextFrameId { get; set; }
        public short? PreviewVersion { get; set; }
        public bool IsInReviewMode { get; set; }
    }
}
