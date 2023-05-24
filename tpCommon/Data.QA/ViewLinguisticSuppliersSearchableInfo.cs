using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ViewLinguisticSuppliersSearchableInfo
    {
        public int SupplierId { get; set; }
        public string AgencyOrTeamName { get; set; }
        public string MainContactSurname { get; set; }
        public string MainContactFirstName { get; set; }
        public byte SupplierTypeId { get; set; }
        public string PostcodeOrZip { get; set; }
        public short CountryId { get; set; }
        public byte SupplierStatusId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string EmailAddress { get; set; }
        public string SubjectAreaSpecialismsAsDescribedBySupplier { get; set; }
        public DateTime? NdauploadedDateTime { get; set; }
        public decimal? MemoryRateForPerfectMatches { get; set; }
        public decimal? MemoryRateForExactMatches { get; set; }
        public decimal? MemoryRateForRepetitions { get; set; }
        public decimal? MemoryRateFor95To99Percent { get; set; }
        public decimal? MemoryRateFor85To94Percent { get; set; }
        public decimal? MemoryRateFor75To84Percent { get; set; }
        public decimal? MemoryRateFor50To74Percent { get; set; }
        public bool? HasEncryptedComputer { get; set; }
        public int? OverallRating { get; set; }
        public byte? Gdprstatus { get; set; }
        public bool NeedApprovalToBeAddedToDb { get; set; }
        public DateTime? NonEeaclauseDeclinedDateTime { get; set; }
    }
}
