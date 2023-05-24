using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class Task
    {
        public int Id { get; set; }
        public byte DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public byte TaskTypeId { get; set; }
        public DateTime DueDateTime { get; set; }
        public bool IsTimeSpecific { get; set; }
        public bool IsHot { get; set; }
        public short TaskForEmployeeId { get; set; }
        public string InstructionNotes { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public short? CompletedByEmployeeId { get; set; }
        public string ProgressNotes { get; set; }
        public DateTime CreatedDate { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
