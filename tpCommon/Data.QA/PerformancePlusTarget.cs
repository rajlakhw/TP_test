using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class PerformancePlusTarget
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int? BookedMeetingsTarget { get; set; }
        public int? AttendedMeetingsTarget { get; set; }
        public int? ProposalsTendersTarget { get; set; }
        public int? JobsGoneAheadTarget { get; set; }
        public decimal? ChargeToClientTarget { get; set; }
        public int? EnquiriesTarget { get; set; }
        public int? CallDataRecordsTarget { get; set; }
        public DateTime? Week { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public int CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public int? LastModifiedByEmployeeId { get; set; }
    }
}
