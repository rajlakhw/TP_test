using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class LinguisticSupplierInvoiceEarlyPaymentOption
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal? Discount { get; set; }
        public short? PaymentDays { get; set; }
        public string IanalanguageCode { get; set; }
    }
}
