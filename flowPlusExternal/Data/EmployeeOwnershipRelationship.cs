using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeeOwnershipRelationship
    {
        public int Id { get; set; }
        public byte DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public short EmployeeId { get; set; }
        public short EmployeeOwnershipTypeId { get; set; }
        public DateTime InForceStartDateTime { get; set; }
        public DateTime? InForceEndDateTime { get; set; }
        public decimal? CommissionPercentage { get; set; }
        public bool ReceiveNotifications { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public short? CreatedByEmployee { get; set; }
        public DateTime? ConfirmToEndDateTime { get; set; }
        public short? ConfirmToEndEmpId { get; set; }
    }
}
