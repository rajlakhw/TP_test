using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class OrgReminderSetting
    {
        public int Id { get; set; }
        public int DataTypeId { get; set; }
        public int DataObjectId { get; set; }
        public int TimeSpan { get; set; }
        public byte ReminderType { get; set; }
        public bool? AutoApprove { get; set; }
        public bool? BeforeDeadline { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
