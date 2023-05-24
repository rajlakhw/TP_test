using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ViewLanguagesMultilingualInfo
    {
        public string Name { get; set; }
        public string AssociatedIanascript { get; set; }
        public byte? TpserviceRestrictId { get; set; }
        public byte TpregionGroupingId { get; set; }
        public bool IsCommon { get; set; }
        public string LanguageIanacodeBeingDescribed { get; set; }
        public string LanguageIanacodeOfName { get; set; }
        public bool? TranslatableViaGoogleApi { get; set; }
        public bool? TranslatableSourceViaDeepL { get; set; }
        public bool? TranslatableTargetViaDeepL { get; set; }
    }
}
