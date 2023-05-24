using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class PerformancePlusManualFigure
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int? QualifiedCalls { get; set; }
        public int? ToNewCompaniesCalls { get; set; }
        public int? SentEmails { get; set; }
        public decimal? ValueEnquiriesReceived { get; set; }
        public string Notes { get; set; }
        public int? DayOfTheWeek { get; set; }
        public DateTime? Week { get; set; }
    }
}
