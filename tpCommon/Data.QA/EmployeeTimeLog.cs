using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class EmployeeTimeLog
    {
        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public DateTime LogDateTime { get; set; }
        public bool LoggedIn { get; set; }
    }
}
