using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class LanguageCombinationsForTop5Supplier
    {
        public int Id { get; set; }
        public string SourceIanacode { get; set; }
        public string TargetIanacode { get; set; }
        public bool IncludeLanguageVariants { get; set; }
    }
}
