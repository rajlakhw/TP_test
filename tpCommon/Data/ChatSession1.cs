using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ChatSession1
    {
        public int ChatSessionId { get; set; }
        public bool? IsActiveChat { get; set; }
        public int? DataObjectId { get; set; }
        public byte? DataObjectTypeId { get; set; }
    }
}
