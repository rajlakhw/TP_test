using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class OrgIndustry
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public short? MainIndustryId { get; set; }
        public short? AltairIndustryId { get; set; }
    }
}
