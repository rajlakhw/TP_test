using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class CmspublicationFamily
    {
        public int Id { get; set; }
        public int LinguisticDatabaseId { get; set; }
        public string Name { get; set; }
        public string ClientIdstring { get; set; }
        public string CustomTextField1 { get; set; }
        public string CustomTextField2 { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedByExtranetUserName { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public string LastModifiedByExtranetUserName { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string DeletedByExtranetUserName { get; set; }
    }
}
