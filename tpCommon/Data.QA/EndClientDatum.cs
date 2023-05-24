using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class EndClientDatum
    {
        public int Id { get; set; }
        public int EndClientId { get; set; }
        public int DataObjectTypeId { get; set; }
        public string DataObjectName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
