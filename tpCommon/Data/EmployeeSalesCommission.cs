using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeeSalesCommission
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CommissionDate { get; set; }
        public DateTime CommissionProcessedDateTime { get; set; }
        public int CommissionProcessedByEmployeeId { get; set; }
        public decimal CommissionAmount { get; set; }
    }
}
