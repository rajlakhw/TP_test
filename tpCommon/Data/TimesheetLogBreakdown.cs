using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class TimesheetLogBreakdown
    {
        public int Id { get; set; }
        public int TimesheetId { get; set; }
        public int CategoryId { get; set; }
        public short TaskHours { get; set; }
        public short TaskMinutes { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public short? CreatedByEmpId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmpId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmpId { get; set; }
    }
}
