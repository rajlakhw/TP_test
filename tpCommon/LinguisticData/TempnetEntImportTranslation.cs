using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class TempnetEntImportTranslation
    {
        public string ClientTermKeyId { get; set; }
        public string English { get; set; }
        public string TranslationLanguage { get; set; }
        public int? TptranslationTermId { get; set; }
        public string Translation { get; set; }
    }
}
