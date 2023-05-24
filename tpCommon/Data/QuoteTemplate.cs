using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class QuoteTemplate
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string LanguageIanacode { get; set; }
        public string OpeningSectionText { get; set; }
        public string ClosingSectionText { get; set; }
        public int DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
