using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class TrainingSession
    {
        public int Id { get; set; }
        public int TrainingCourseId { get; set; }
        public DateTime TrainingSessionDate { get; set; }
        public byte TrainingStartTimeInHours { get; set; }
        public byte TrainingStartTimeInMinutes { get; set; }
        public byte TrainingEndTimeInHours { get; set; }
        public byte TrainingEndTimeInMinutes { get; set; }
        public short? TrainingOfficerEmployeeId { get; set; }
        public short? SignOffInstructor { get; set; }
        public DateTime? SignOffDateTime { get; set; }
        public short? SignedOffByEmployeeId { get; set; }
        public string ExternalTrainer { get; set; }
        public bool TrainerIsExternal { get; set; }
        public string TrainingLocation { get; set; }
        public bool TrainingLocationIsAmeetingRoom { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
