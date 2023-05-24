using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class LinguisticSupplierInvoiceJobItem
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public int? OriginatedFromInvoiceId { get; set; }
        public int? JobItemId { get; set; }
    }
}
