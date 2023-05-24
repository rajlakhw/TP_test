using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class DesignPlusLinksPath
    {
        public int Id { get; set; }
        public string LinkName { get; set; }
        public int DesignPlusFileId { get; set; }
        public string OriginalLinkPath { get; set; }
        public string PreviewLinkPath { get; set; }
        public string LinkType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedBy { get; set; }
    }
}
