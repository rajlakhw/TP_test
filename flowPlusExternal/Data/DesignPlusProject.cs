using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class DesignPlusProject
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public int? CompletedBy { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedBy { get; set; }
    }
}
