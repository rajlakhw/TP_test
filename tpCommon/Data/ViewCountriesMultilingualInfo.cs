using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ViewCountriesMultilingualInfo
    {
        public short CountryId { get; set; }
        public string Isocode { get; set; }
        public string CountryName { get; set; }
        public string LanguageIanacode { get; set; }
    }
}
