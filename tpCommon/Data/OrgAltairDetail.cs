using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class OrgAltairDetail
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string OrgLegalName { get; set; }
        public int? AltairRegionId { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public short? CorporateGroupeId { get; set; }
        public string Siretnumber { get; set; }
        public string Ssnnumber { get; set; }
        public string Einnumber { get; set; }
        public string Gstnumber { get; set; }
        public string Hstnumber { get; set; }
        public string Sirennumber { get; set; }
        public string Tinnumber { get; set; }
        public string Sinnumber { get; set; }
        public string Qstnumber { get; set; }
    }
}
