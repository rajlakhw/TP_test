using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ClientPriceListRate
    {
        public int Id { get; set; }
        public int ClientPriceListId { get; set; }
        public byte LanguageServiceId { get; set; }
        public string SourceLangIanacode { get; set; }
        public string TargetLangIanacode { get; set; }
        public short LanguageRateUnitId { get; set; }
        public decimal Rate { get; set; }
        public decimal MinimumCharge { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public bool? ApprovedByClient { get; set; }
        public string ApprovedByClientType { get; set; }
    }
}
