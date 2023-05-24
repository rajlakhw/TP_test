using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class DeadlineChangeReason
    {
        public int Id { get; set; }
        public string ReasonText { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
