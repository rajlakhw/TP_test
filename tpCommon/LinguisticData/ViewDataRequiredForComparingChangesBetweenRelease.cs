using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class ViewDataRequiredForComparingChangesBetweenRelease
    {
        public int LinguisticDatabaseId { get; set; }
        public string ClientTermKeyId { get; set; }
        public int TptermKeyId { get; set; }
        public int? CmspublicationReleaseId { get; set; }
        public int EntryId { get; set; }
        public string LanguageIanacode { get; set; }
        public string EntryTermText { get; set; }
        public byte EntryStatus { get; set; }
        public int CmspublicationId { get; set; }
        public string CmspublicationClientIdstring { get; set; }
        public string CmspublicationName { get; set; }
    }
}
