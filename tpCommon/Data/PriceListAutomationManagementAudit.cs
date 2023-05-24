using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class PriceListAutomationManagementAudit
    {
        public int Id { get; set; }
        public int DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public string Action { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public int? OrgGroupId { get; set; }
    }
}
