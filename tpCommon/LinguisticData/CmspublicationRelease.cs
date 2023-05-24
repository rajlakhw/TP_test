using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class CmspublicationRelease
    {
        public int Id { get; set; }
        public int LinguisticDatabaseId { get; set; }
        public int ClientMajorVersionNumber { get; set; }
        public int? ClientMinorVersionNumber { get; set; }
        public int? ClientRevisionVersionNumber { get; set; }
        public string ClientDescription { get; set; }
        public DateTime ReleaseDateTime { get; set; }
        public byte ReleaseType { get; set; }
        public string ClientBugFixOrEnhancementIdstring { get; set; }
        public int? ClonedFromReleaseId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedByExtranetUserName { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public string LastModifiedByExtranetUserName { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string DeletedByExtranetUserName { get; set; }
        public DateTime? LockedDateTime { get; set; }
        public string LockedByExtranetUserName { get; set; }
    }
}
