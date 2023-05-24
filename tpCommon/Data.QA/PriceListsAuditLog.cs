using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class PriceListsAuditLog
    {
        public int Id { get; set; }
        public int DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public string ChangeInItem { get; set; }
        public string Action { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
