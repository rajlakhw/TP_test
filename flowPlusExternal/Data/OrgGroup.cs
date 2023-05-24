using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class OrgGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? HqorgId { get; set; }
        public DateTime CreatedDate { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public string OriginalgroupCodeForXref { get; set; }
        public DateTime? FirstPaidJobDate { get; set; }
        public decimal? ClientSpendLastFinancialYear { get; set; }
        public decimal? ClientSpendCurrentFinancialYear { get; set; }
        public decimal? ClientSpendOverLast12Months { get; set; }
        public decimal? ClientSpendOverLast3Months { get; set; }
        public decimal? InvoicedMarginOverLast3Months { get; set; }
        public byte BrandId { get; set; }
        public bool OnlyAllowEncryptedSuppliers { get; set; }
        public int? LinguisticDatabaseId { get; set; }
        public byte JobFilesToBeSavedWithinRegion { get; set; }
    }
}
