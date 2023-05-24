using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class AutoClientInvoicingSetting
    {
        public int Id { get; set; }
        public byte DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public string InvoiceTitle { get; set; }
        public string InvoiceDescription { get; set; }
        public string InvoiceLangIanacode { get; set; }
        public bool ShowOrderPonumbersInBreakdown { get; set; }
        public bool ShowNotesInBreakdown { get; set; }
        public bool ShowSourceLangsInBreakdown { get; set; }
        public bool ShowTargetLangsInBreakdown { get; set; }
        public bool ShowJobItemsInBreakdown { get; set; }
        public bool ShowContactNamesInBreakdown { get; set; }
        public byte WordCountPresentationOption { get; set; }
        public bool ShowInterpretingDurationInBreakdown { get; set; }
        public bool ShowWorkDurationInBreakdown { get; set; }
        public bool ShowPagesOrSlidesInBreakdown { get; set; }
        public bool ShowCustomerSpecificField1ValueInBreakdown { get; set; }
        public bool ShowDeliveryDate { get; set; }
        public byte? InvoiceScheduleOption { get; set; }
        public byte? InvoiceScheduleDay { get; set; }
        public DateTime CreatedDate { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public bool? InvoiceByPonumber { get; set; }
    }
}
