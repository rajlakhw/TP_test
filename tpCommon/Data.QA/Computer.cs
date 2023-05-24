using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class Computer
    {
        public int ComputerId { get; set; }
        public string DisplayName { get; set; }
        public string Hostname { get; set; }
        public string MacAddress { get; set; }
        public int? EmployeeId { get; set; }
        public string NetworkUserName { get; set; }
    }
}
