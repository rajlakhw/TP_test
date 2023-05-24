using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class DesignPlusDocumentLayer
    {
        public int Id { get; set; }
        public int DesignPlusFileId { get; set; }
        public int IndesignId { get; set; }
        public string IndesignName { get; set; }
        public bool ActiveLayer { get; set; }
        public bool IsLocked { get; set; }
        public bool IsVisibleInSourceFile { get; set; }
        public bool? IsVisibleOnDesignPlus { get; set; }
        public bool? IsVisibleInReviewSourceFile { get; set; }
        public bool? IsVisibleInReviewDesignPlus { get; set; }
        public byte VisiblityStatus { get; set; }
        public byte? ReviewVisibilityStatus { get; set; }
    }
}
