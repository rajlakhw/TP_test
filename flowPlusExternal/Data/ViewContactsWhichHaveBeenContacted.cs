using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ViewContactsWhichHaveBeenContacted
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
    }
}
