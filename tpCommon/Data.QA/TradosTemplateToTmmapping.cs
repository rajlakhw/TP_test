using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class TradosTemplateToTmmapping
    {
        public int Id { get; set; }
        public int TradosTemplateId { get; set; }
        public string SourceIanacode { get; set; }
        public string TargetIanacode { get; set; }
        public string TranslationMemoryPath { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DatetedByEmployeeId { get; set; }
    }
}
