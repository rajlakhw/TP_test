using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ChatLog
    {
        public int Id { get; set; }
        public Guid ChatSession { get; set; }
        public string ChatPerson { get; set; }
        public string ChatMesssage { get; set; }
        public DateTime? SentTime { get; set; }
    }
}
