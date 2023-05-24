using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeeInterimTarget
    {
        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public DateTime FinancialYearStarting { get; set; }
        public decimal InterimTargetQ1 { get; set; }
        public decimal InterimTargetQ2 { get; set; }
        public decimal InterimTargetQ3 { get; set; }
        public decimal InterimTargetQ4 { get; set; }
    }
}
