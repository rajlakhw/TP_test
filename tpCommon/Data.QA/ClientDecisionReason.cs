using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ClientDecisionReason
    {
        public byte Id { get; set; }
        public byte DecisionReasonId { get; set; }
        public byte Type { get; set; }
        public string Reason { get; set; }
        public string Ianacode { get; set; }
    }
}
