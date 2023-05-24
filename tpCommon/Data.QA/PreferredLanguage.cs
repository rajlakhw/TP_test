using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class PreferredLanguage
    {
        public byte DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public string LanguageIanacode { get; set; }
        public bool IsSource { get; set; }
        public byte? CustomerSpecificCode { get; set; }
    }
}
