using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class AuditTrailEvent
    {
        public int Id { get; set; }
        public int LinguisticDatabaseId { get; set; }
        public byte DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public string LanguageIanacode { get; set; }
        public DateTime EventLoggedDateTime { get; set; }
        public byte EventType { get; set; }
        public string ClientIdstringBefore { get; set; }
        public string ClientIdstringAfter { get; set; }
        public string MainTextContentBefore { get; set; }
        public string MainTextContentAfter { get; set; }
        public string ExtranetUserName { get; set; }
        public byte? StatusBefore { get; set; }
        public byte? StatusAfter { get; set; }
    }
}
