using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class LinguisticDatabaseTermKey
    {
        public int Id { get; set; }
        public int LinguisticDatabaseId { get; set; }
        public string ClientTermKeyId { get; set; }
        public int? CmspublicationReleaseId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public string CreatedByExtranetUserName { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public string DeletedByExtranetUserName { get; set; }
    }
}
