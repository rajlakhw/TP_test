using System;
using System.Collections.Generic;

namespace Data
{
    public class EnquiryStatus
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedBy { get; set; }
    }
}
