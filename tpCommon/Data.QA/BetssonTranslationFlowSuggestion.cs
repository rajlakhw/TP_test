using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class BetssonTranslationFlowSuggestion
    {
        public int Id { get; set; }
        public int TptextId { get; set; }
        public string SuggestionMatchedText { get; set; }
        public string SuggestionText { get; set; }
        public string SuggestionTranslation { get; set; }
    }
}
