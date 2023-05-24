using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class JobOrderAutomationDatum
    {
        public int Id { get; set; }
        public int? JobId { get; set; }
        public DateTime? JobSetUpDate { get; set; }
        public DateTime? AnalyseSourceDate { get; set; }
        public DateTime? AddClientCostsDate { get; set; }
        public DateTime? AddSupplierCostsDate { get; set; }
        public DateTime? SendToSupplierDate { get; set; }
        public DateTime? SendForClientReviewDate { get; set; }
        public DateTime? UpdateMemoriesDate { get; set; }
        public DateTime? AutoDeliveryDate { get; set; }
        public DateTime? AssignedSupplierDate { get; set; }
    }
}
