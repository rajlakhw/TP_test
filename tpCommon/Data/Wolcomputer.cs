using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class Wolcomputer
    {
        public int ComputerId { get; set; }
        public string DisplayName { get; set; }
        public string Hostname { get; set; }
        public string MacAddress { get; set; }
        public string EmployeeId { get; set; }
        public string NetworkUserName { get; set; }
    }
}
