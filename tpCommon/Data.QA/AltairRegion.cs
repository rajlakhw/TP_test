using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class AltairRegion
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
    }
}
