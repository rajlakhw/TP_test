using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EnquiryQuoteItem
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int LanguageServiceId { get; set; }
        public string SourceLanguageIanaCode { get; set; }
        public string TargetLanguageIanaCode { get; set; }
        public int CreatedByEmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? DeletedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? LanguageServiceCategoryId { get; set; }
    }
}
