using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ExtraWorkingDay
    {
        public short Id { get; set; }
        public DateTime WorkingDate { get; set; }
        public bool IsBulgarianBankHoliday { get; set; }
        public bool IsHalfDay { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
