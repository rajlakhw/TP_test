using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class LanguageCodesToUseInStudio
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string LanguageIanacodeBeingDescribed { get; set; }
        public string LanguageIanacodeInStudio { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
