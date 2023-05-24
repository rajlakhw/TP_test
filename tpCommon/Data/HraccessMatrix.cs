using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class HraccessMatrix
    {
        public short EmployeeAccessLevelId { get; set; }
        public short? EmployeeFromDataObjectId { get; set; }
        public byte? EmployeeFromDataObjectTypeId { get; set; }
        public short? AccessToApproveAccessLevelId { get; set; }
        public short? AccessToApproveDataObjectId { get; set; }
        public byte? AccessToApproveDataObjectTypeId { get; set; }
        public bool EnableAccessToHolidays { get; set; }
        public bool EnableAccessToSickness { get; set; }
    }
}
