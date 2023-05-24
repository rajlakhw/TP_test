using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class Enquiry
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public byte OrderChannelId { get; set; }
        public byte Status { get; set; }
        public byte? DecisionReasonId { get; set; }
        public int? DecisionMadeByContactId { get; set; }
        public DateTime? DecisionMadeDateTime { get; set; }
        public string ExternalNotes { get; set; }
        public string InternalNotes { get; set; }
        public DateTime? DeadlineRequestedByClient { get; set; }
        public int? WentAheadAsJobOrderId { get; set; }
        public string JobName { get; set; }
        public string AdditionalDetails { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public DateTime? EnqFilesDeletedDateTime { get; set; }
        public bool? PrintingProject { get; set; }
        public DateTime? ArchivedToLionBoxDateTime { get; set; }
        public DateTime? ArchivedToAmazonS3dateTime { get; set; }
        public DateTime? ArchivedDateTime { get; set; }
        public int? AssignedToEmployeeID { get; set; }
        public int? PriorityID { get; set; }
    }
}
