using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ViewContactsSearchableInfo
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public short CountryId { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public string EmailAddress { get; set; }
    }
}
