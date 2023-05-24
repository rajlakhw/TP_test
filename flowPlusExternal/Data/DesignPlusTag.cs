using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class DesignPlusTag
    {
        public int Id { get; set; }
        public int ClientDesignPlusFileId { get; set; }
        public int TextFrameId { get; set; }
        public string TagString { get; set; }
    }
}
