using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ViewCurrenciesMultilingualInfo
    {
        public short CurrencyId { get; set; }
        public string Symbol { get; set; }
        public string Prefix { get; set; }
        public string LanguageIanacode { get; set; }
        public string CurrencyName { get; set; }
    }
}
