using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class TrainingCourse
    {
        public int Id { get; set; }
        public int TrainingTypeId { get; set; }
        public string TrainingName { get; set; }
        public short? GivenOnWeek { get; set; }
        public short? DurationInMinutes { get; set; }
        public short? TrainingOfficerEmployeeId { get; set; }
        public string TrainersNotes { get; set; }
        public string Notes { get; set; }
        public bool? IsVisible { get; set; }
        public byte? TypeOfCourse { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedBy { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedBy { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedBy { get; set; }
    }
}
