using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class MarketingCampaign
    {
        public int Id { get; set; }
        public byte TypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EmailSubjectLine { get; set; }
        public string EmailHtmlbody { get; set; }
        public byte[] EmailImageBinary { get; set; }
        public string EmailHtmlsignature { get; set; }
        public string EmailFromName { get; set; }
        public string EmailFromEmailAddress { get; set; }
        public byte? InternalFollowUpTasksTypeId { get; set; }
        public short? InternalFollowUpTasksEmployeeId { get; set; }
        public string InternalFollowUpTasksInstructionNotes { get; set; }
        public DateTime? InternalFollowUpTasksDueDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public DateTime? LaunchedDateTime { get; set; }
        public short? LaunchedByEmployeeId { get; set; }
        public DateTime? ProcessingCompletedBySystemDateTime { get; set; }
        public byte? BrandId { get; set; }
    }
}
