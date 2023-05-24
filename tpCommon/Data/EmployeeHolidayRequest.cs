using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeeHolidayRequest
    {
        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public DateTime HolidayStartDateTime { get; set; }
        public byte StartDateAmorPmorFullDay { get; set; }
        public DateTime HolidayEndDateTime { get; set; }
        public byte EndDateAmorPmorFullDay { get; set; }
        public decimal TotalDays { get; set; }
        public DateTime RequestedDateTime { get; set; }
        public short RequestedByEmployeeId { get; set; }
        public bool? IsStartDateYearSameAsEndDateYear { get; set; }
        public decimal HolidaysInStartDateYear { get; set; }
        public decimal HolidaysInEndDateYear { get; set; }
        public byte? Status { get; set; }
        public short? ApprovedByEmployeeId { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public short? RejectedByEmployeeId { get; set; }
        public DateTime? RejectedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
