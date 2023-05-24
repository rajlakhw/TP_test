using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class EndClientData
    {
        public int Id { get; set; }
        public int EndClientID { get; set; }
        public int DataObjectTypeID { get; set; }
        public string DataObjectName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
