using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class HighFife
    {
        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public string HighFiveComment { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public byte? DeletedByEmployeeId { get; set; }
    }
}
