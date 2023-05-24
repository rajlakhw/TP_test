using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class TimesheetsApproval
    {
        public int Id { get; set; }
        public int TimesheetsEmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public int? ApprovedByEmpId { get; set; }
        public DateTime? SubmissionDateTime { get; set; }
        public DateTime? UnlockDateTime { get; set; }
        public int? UnlockedByEmpId { get; set; }
    }
}
