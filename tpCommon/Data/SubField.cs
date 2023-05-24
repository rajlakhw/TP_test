using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class SubField
    {
        public int Id { get; set; }
        public int MasterFieldId { get; set; }
        public string Name { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
