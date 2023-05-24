using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class Conference
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public DateTime? LastDateWeAttended { get; set; }
        public short NumberOfOccasionsWeAttended { get; set; }
        public DateTime? DateOfNextConference { get; set; }
    }
}
