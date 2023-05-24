using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class CallDataRecord
    {
        public int Id { get; set; }
        public byte Source { get; set; }
        public string CalledNumber { get; set; }
        public string CallingNumber { get; set; }
        public DateTime StartDateTimeOfCall { get; set; }
        public DateTime EndDateTimeOfCall { get; set; }
        public short? EmployeeIdatTimeOfCall { get; set; }
        public int? CalledNumberContactIdatTimeOfCall { get; set; }
        public bool? CallDirection { get; set; }
        public long? RecId { get; set; }
        public DateTime? AnswerDateTimeCall { get; set; }
        public string CallType { get; set; }
    }
}
