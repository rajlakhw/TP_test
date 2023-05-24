using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class AccountsOverview
    {
        public int Id { get; set; }
        public int? NewClientInvoices { get; set; }
        public int? SuccessfullyCreatedAndSentInvoices { get; set; }
        public int? AutoChasingOfOverdueClientInvoices { get; set; }
        public int? ChasedOverdueInvoicesWithPenaltyCharges { get; set; }
        public int? UpdatedOverdueInvoicesWithPenaltyCharges { get; set; }
        public int? ClientInvoicesExportForSage { get; set; }
        public int? ClientRecordExportsForSage { get; set; }
        public int? SupplierInvoicesExportsForSage { get; set; }
        public int? SupplierRecordExportsForSage { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
