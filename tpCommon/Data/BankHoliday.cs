using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class BankHoliday
    {
        public int Id { get; set; }
        public DateTime BankHolidayDate { get; set; }
        public bool IsAukbankHoliday { get; set; }
        public bool IsDenmarkBankHoliday { get; set; }
        public bool IsGermanyBankHoliday { get; set; }
        public bool IsSwedenBankHoliday { get; set; }
        public bool IsItalianBankHoliday { get; set; }
        public bool IsJapanBankHoliday { get; set; }
        public bool IsBulgarianBankHoliday { get; set; }
        public bool IsColombianBankHoliday { get; set; }
        public bool IsRomanianBankHoliday { get; set; }
        public bool IsCostaRicaBankHoliday { get; set; }
        public bool IsHalfDay { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
