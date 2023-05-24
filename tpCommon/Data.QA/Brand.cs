using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class Brand
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string CompanyNameToShow { get; set; }
        public string DomainName { get; set; }
        public string ApplicationName { get; set; }
        public string EmailAddress { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
