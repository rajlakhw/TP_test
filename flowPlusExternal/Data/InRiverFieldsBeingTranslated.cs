using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class InRiverFieldsBeingTranslated
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public int JobOrderId { get; set; }
        public string FieldName { get; set; }
        public string SourceLanguageIanacode { get; set; }
        public string TargetLanguageIanacode { get; set; }
    }
}
