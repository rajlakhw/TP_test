using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public partial class flowPlusLicences
    {
        public int Id { get; set; }
        public Byte ApplicationId { get; set; }
        public decimal AppCost { get; set; }
        public bool IsEnabled { get; set; }
        public bool DemoEnabled { get; set; }
        public DateTime LastEnabledDateTime { get; set; }
        public short LastEnabledByEmpID { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmpID { get; set; }
        public DateTime? LastDisabledDateTime { get; set; }
        public short? LastDisabledByEmpID { get; set; }
        public int? OrderContactID { get; set; }
        public DateTime? PreviousOrderSetDate { get; set; }
        public DateTime? NextOrderSetDate { get; set; }
    }
}
