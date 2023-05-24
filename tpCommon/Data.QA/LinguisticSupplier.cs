using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class LinguisticSupplier
    {
        public int Id { get; set; }
        public byte SupplierTypeId { get; set; }
        public string AgencyOrTeamName { get; set; }
        public string MainContactSurname { get; set; }
        public string MainContactFirstName { get; set; }
        public bool? MainContactGender { get; set; }
        public DateTime? MainContactDob { get; set; }
        public short? MainContactNationalityCountryId { get; set; }
        public string AgencyCompanyRegistrationNumber { get; set; }
        public short? AgencyNumberOfLinguists { get; set; }
        public short? AgencyNumberOfDtporMultimediaOps { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string CountyOrState { get; set; }
        public string PostcodeOrZip { get; set; }
        public short CountryId { get; set; }
        public short? MainLandlineCountryId { get; set; }
        public string MainLandlineNumber { get; set; }
        public short? SecondaryLandlineCountryId { get; set; }
        public string SecondaryLandlineNumber { get; set; }
        public bool? SecondaryLandlineIsWorkNumber { get; set; }
        public short? MobileCountryId { get; set; }
        public string MobileNumber { get; set; }
        public short? FaxCountryId { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string SecondaryEmailAddress { get; set; }
        public bool? SecondaryEmailIsWorkAddress { get; set; }
        public string SkypeId { get; set; }
        public string WebAddress { get; set; }
        public string Notes { get; set; }
        public byte SupplierStatusId { get; set; }
        public short SupplierSourceId { get; set; }
        public DateTime? ApplicationFormSentToSupplierDate { get; set; }
        public DateTime? ApplicationFormReceivedFromSupplierDate { get; set; }
        public bool? HasAccessToCar { get; set; }
        public bool? WouldSignWitnessStatement { get; set; }
        public byte? PrimaryOperatingSystemId { get; set; }
        public byte? PrimaryOperatingSystemVersionId { get; set; }
        public string SubjectAreaSpecialismsAsDescribedBySupplier { get; set; }
        public string MotherTongueLanguageIanacode { get; set; }
        public string IfBilingualOtherMotherTongueIanacode { get; set; }
        public short? CurrencyId { get; set; }
        public short? AgreedPaymentMethodId { get; set; }
        public string Vatnumber { get; set; }
        public string RatesNotes { get; set; }
        public decimal? MemoryRateForExactMatches { get; set; }
        public decimal? MemoryRateForRepetitions { get; set; }
        public decimal? MemoryRateFor95To99Percent { get; set; }
        public decimal? MemoryRateFor85To94Percent { get; set; }
        public decimal? MemoryRateFor75To84Percent { get; set; }
        public decimal? MemoryRateFor50To74Percent { get; set; }
        public decimal? MemoryRateForPerfectMatches { get; set; }
        public string Referee1Name { get; set; }
        public string Referee1FullAddress { get; set; }
        public string Referee1Phone { get; set; }
        public string Referee1EmailAddress { get; set; }
        public string Referee2Name { get; set; }
        public string Referee2FullAddress { get; set; }
        public string Referee2Phone { get; set; }
        public string Referee2EmailAddress { get; set; }
        public bool IsAclientInternalLinguist { get; set; }
        public DateTime CreatedDate { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public DateTime? NdaunlockedDateTime { get; set; }
        public short? NdaunlockedByEmployeeId { get; set; }
        public DateTime? NdauploadedDateTime { get; set; }
        public short? NdauploadedByEmployeeId { get; set; }
        public DateTime? LastExportedToSageDateTime { get; set; }
        public short? CurrencyOfFirstInvoiceExportedToSage { get; set; }
        public bool? HasEncryptedComputer { get; set; }
        public int? SupplierResponsivenessRating { get; set; }
        public int? SupplierFollowingTheInstructionsRating { get; set; }
        public int? SupplierAttitudeRating { get; set; }
        public int? SupplierQualityOfWorkRating { get; set; }
        public int? SupplierOnTimeDeliveryRating { get; set; }
        public int? OverallRating { get; set; }
        public decimal? QualityRatingAvgForFirst2Jobs { get; set; }
        public int? IsQualityRatingPendingForFirst2Jobs { get; set; }
        public int? IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded { get; set; }
        public bool Ftpenabled { get; set; }
        public DateTime? GdpracceptedDateTime { get; set; }
        public string GdpracceptanceText { get; set; }
        public DateTime? GdprrejectedDateTime { get; set; }
        public byte? Gdprstatus { get; set; }
        public bool? GdpraccetedViaIplus { get; set; }
        public bool NeedApprovalToBeAddedToDb { get; set; }
        public DateTime? ApprovedToAbeAddedtoDbdatetime { get; set; }
        public short? ApprovedToAbeAddedtoDbbyEmpId { get; set; }
        public DateTime? NonEeaclauseAcceptedDateTime { get; set; }
        public DateTime? NonEeaclauseDeclinedDateTime { get; set; }
        public int? SapmasterDataReferenceNumber { get; set; }
        public DateTime? ContractUnlockedDateTime { get; set; }
        public short? ContractUnlockedByEmployeeId { get; set; }
        public DateTime? ContractUploadedDateTime { get; set; }
        public short? ContractUploadedByEmployeeId { get; set; }
    }
}
