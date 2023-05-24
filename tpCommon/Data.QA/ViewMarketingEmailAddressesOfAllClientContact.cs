using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ViewMarketingEmailAddressesOfAllClientContact
    {
        public string DataObjectType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactEmailAddress { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public short CountryId { get; set; }
        public byte LegalStatusId { get; set; }
        public short? OrgIndustryId { get; set; }
        public string CreatedByEmployee { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
