﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class LanguageService
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string? Definition { get; set; }
        public bool? IncludeInMarginCalculations { get; set; }
        public bool? MostFrequentlyUsed { get; set; }
        public bool? Hide { get; set; }
        public string? LanguageServiceCategoryID { get; set; }
    }
}
