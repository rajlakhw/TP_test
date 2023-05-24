using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class LanguageService
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public bool? IncludeInMarginCalculations { get; set; }
        public bool? MostFrequentlyUsed { get; set; }
        public bool? IsForPgdonly { get; set; }
    }
}
