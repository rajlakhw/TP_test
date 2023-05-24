using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class NewsItemsLanguage
    {
        public string Name { get; set; }
        public byte? TpserviceRestrictId { get; set; }
        public bool IsCommon { get; set; }
        public string LanguageIanacodeBeingDescribed { get; set; }
        public string LanguageIanacodeOfName { get; set; }
    }
}
