using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class WorkFlow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedByEmployeeId { get; set; }
        public DateTime CreatedDatetime { get; set; }
    }
}
