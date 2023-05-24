using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class OrgTechnologyRelationship
    {
        public int OrgId { get; set; }
        public short OrgTechnologyId { get; set; }
        public bool? MainTechnology { get; set; }
    }
}
