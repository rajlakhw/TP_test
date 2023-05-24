using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class OrgIndustryRelationship
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public short OrgIndustryId { get; set; }
    }
}
