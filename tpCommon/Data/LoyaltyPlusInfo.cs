using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class LoyaltyPlusInfo
    {
        public int Id { get; set; }
        public decimal? Year { get; set; }
        public string Perk { get; set; }
        public int OfficeId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
