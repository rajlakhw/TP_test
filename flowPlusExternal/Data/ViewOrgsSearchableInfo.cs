using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ViewOrgsSearchableInfo
    {
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public string PostcodeOrZip { get; set; }
        public short CountryId { get; set; }
        public string GroupName { get; set; }
        public int? GroupId { get; set; }
        public string EmailAddress { get; set; }
        public string OrgTechnology { get; set; }
        public string HfmcodeIs { get; set; }
        public string HfmcodeBs { get; set; }
    }
}
