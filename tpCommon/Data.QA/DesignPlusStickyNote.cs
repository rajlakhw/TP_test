using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class DesignPlusStickyNote
    {
        public int Id { get; set; }
        public int? DesignPlusFileId { get; set; }
        public int? PageNumber { get; set; }
        public string StickyNoteText { get; set; }
        public int? Xpos { get; set; }
        public int? Ypos { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedByEmployeeId { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedByEmployeeId { get; set; }
    }
}
