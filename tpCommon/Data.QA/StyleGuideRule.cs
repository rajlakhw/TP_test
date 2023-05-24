using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class StyleGuideRule
    {
        public int Id { get; set; }
        public byte AppliesToDataObjectTypeId { get; set; }
        public int AppliesToDataObjectId { get; set; }
        public string RulesXml { get; set; }
    }
}
