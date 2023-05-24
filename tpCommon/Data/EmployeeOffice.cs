using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeeOffice
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedByEmployeeId { get; set; }
    }
}
