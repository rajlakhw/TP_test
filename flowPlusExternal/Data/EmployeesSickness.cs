using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeesSickness
    {
        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public DateTime SicknessStartDateTime { get; set; }
        public byte StartDateAmorPmorFullDay { get; set; }
        public DateTime SicknessEndDateTime { get; set; }
        public byte EndDateAmorPmorFullDay { get; set; }
        public decimal TotalDays { get; set; }
        public DateTime? ConfirmedDateTime { get; set; }
        public short? ConfirmedByEmployeeId { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public short? ApprovedByEmployeeId { get; set; }
        public DateTime? RejectedDateTime { get; set; }
        public short? RejectedByEmployeeId { get; set; }
        public string EmployeeSicknessReason { get; set; }
        public string ApprovalNotes { get; set; }
        public string DeclinedNotes { get; set; }
        public string SickNotes { get; set; }
        public string DoctorsCertificate { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
