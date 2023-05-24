using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ChatSession
    {
        public Guid ChatSessionId { get; set; }
        public bool? IsActiveChat { get; set; }
        public string User1 { get; set; }
        public string User2 { get; set; }
    }
}
