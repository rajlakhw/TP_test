using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class JobOrderAutomationSetting
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public bool? AnalyseSourceEnabled { get; set; }
        public bool? AddClientCostsEnabled { get; set; }
        public bool? AddSupplierCostsEnabled { get; set; }
        public bool? SendToSupplierEnabled { get; set; }
        public bool? SendForClientReviewEnabled { get; set; }
        public bool? UpdateMemoriesEnabled { get; set; }
        public bool? AutoDeliveryEnabled { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
    }
}
