using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class Cmslanguage
    {
        public int Id { get; set; }
        public int? DatabaseId { get; set; }
        public string LanguageIanacode { get; set; }
        public bool? IsDefaultForTranslation { get; set; }
        public bool? IsDefaultForPublishing { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
