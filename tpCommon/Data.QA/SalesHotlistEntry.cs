using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class SalesHotlistEntry
    {
        public int Id { get; set; }
        public short OwnerEmployeeId { get; set; }
        public byte AppliesToDataObjectTypeId { get; set; }
        public int AppliesToDataObjectId { get; set; }
        public decimal BestGuessOfAnnualGbpspend { get; set; }
        public decimal BestGuessOfGbpspendWithUsThisFy { get; set; }
        public string ProgressNotes { get; set; }
        public string NextStepsNotes { get; set; }
        public byte WinChancePercentage { get; set; }
        public decimal? AdjustedAnnualGbpspend { get; set; }
        public decimal? AdjustedGbpspendWithUsThisFy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? RemovedDateTime { get; set; }
        public short? RemovedByEmployeeId { get; set; }
        public byte? RemovedReason { get; set; }
    }
}
