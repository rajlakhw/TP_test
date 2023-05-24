using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public partial class LinguisticSupplierPoliceCheckDetail
    {
        public int Id { get; set; }
        public int LinguistId { get; set; }
        public short PoliceCheckId { get; set; }
        public DateTime? PoliceCheckIssueDate { get; set; }
        public DateTime? PoliceCheckExpiryDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
