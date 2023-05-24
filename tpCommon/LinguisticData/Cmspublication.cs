using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class Cmspublication
    {
        public int Id { get; set; }
        public int LinguisticDatabaseId { get; set; }
        public string ClientIdstring { get; set; }
        public string Name { get; set; }
        public int? CmspublicationFamilyId { get; set; }
        public string CustomTextField1Data { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedByExtranetUserName { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public string LastModifiedByExtranetUserName { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string DeletedByExtranetUserName { get; set; }
    }
}
