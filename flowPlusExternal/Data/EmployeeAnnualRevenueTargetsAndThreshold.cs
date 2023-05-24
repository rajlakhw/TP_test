using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeeAnnualRevenueTargetsAndThreshold
    {
        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public DateTime FinancialYearStarting { get; set; }
        public decimal Gbptarget { get; set; }
        public decimal GbpthresholdQ1 { get; set; }
        public decimal GbpthresholdQ2 { get; set; }
        public decimal GbpthresholdQ3 { get; set; }
        public decimal GbpthresholdQ4 { get; set; }
    }
}
