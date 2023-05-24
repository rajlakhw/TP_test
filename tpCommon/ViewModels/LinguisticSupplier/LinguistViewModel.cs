﻿using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Contact;

namespace ViewModels.LinguisticSupplier
{
    public class LinguistViewModel
    {
        public string UIMessage { get; set; }
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
        public string RateNotes { get; set; }
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
        public string WouldSignWitnessStatementString { get; set; }
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
        public string SectionToUpdate { get; set; }
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
        public decimal? SupplierResponsivenessRating { get; set; }
        public decimal? SupplierFollowingTheInstructionsRating { get; set; }
        public decimal? SupplierAttitudeRating { get; set; }
        public decimal? SupplierQualityOfWorkRating { get; set; }
        public decimal? SupplierOnTimeDeliveryRating { get; set; }
        public decimal? OverallRating { get; set; }
        public decimal? QualityRatingAvgForFirst2Jobs { get; set; }
        public int? IsQualityRatingPendingForFirst2Jobs { get; set; }
        public int? IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded { get; set; }
        public bool Ftpenabled { get; set; }
        public DateTime? GdpracceptedDateTime { get; set; }
        public byte? Gdprstatus { get; set; }
        public bool? GdpraccetedViaIplus { get; set; }
        public DateTime? GdprrejectedDateTime { get; set; }
        public bool? NeedApprovalToBeAddedToDb { get; set; }
        public DateTime? ApprovedToAbeAddedtoDbdatetime { get; set; }
        public short? ApprovedToAbeAddedtoDbbyEmpId { get; set; }
        public DateTime? NonEeaclauseAcceptedDateTime { get; set; }
        public DateTime? NonEeaclauseDeclinedDateTime { get; set; }
        public int? SapmasterDataReferenceNumber { get; set; }
        public DateTime? ContractUnlockedDateTime { get; set; }
        public short? ContractUnlockedByEmployeeId { get; set; }
        public DateTime? ContractUploadedDateTime { get; set; }
        public short? ContractUploadedByEmployeeId { get; set; }
        public bool Disabled { get; set; }
        public IEnumerable<LinguistItems> AllItems { get; set; }
        public IEnumerable<LinguistTop5Items> AllLinguistTop5Items { get; set; }
        public IEnumerable<LinguistTop5Words> AllLinguistTop5Words { get; set; }
        public IEnumerable<LinguistRateResults> AllRates { get; set; }
        public IEnumerable<ViewModels.Common.CountryViewModel> AllCountries { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> supplierStatus { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> linguistTypes { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> linguistVendorTypes { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> PaymentMethods { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> OperationSystems { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> PaymentCurrencies { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> OperationSystemVersion { get; set; }
        public IEnumerable<LinguistApprovedOrgs> AllLinguistApprovedOrgs { get; set; }
        public IEnumerable<LinguistBlockedOrgs> AllLinguistBlockedOrgs { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> SoftwareApplications { get; set; }
        public int NonEEAClauseStatus { get; set; }
        public int GeoPoliticalGroup { get; set; }
        public string NDAFilePath { get; set; }
        public string NDAFileName{ get; set; }
        public string ContractFilePath { get; set; }
        public string ContractFileName{ get; set; }
        public string GDPRDocPath { get; set; }
        public string GDPRDocFileName { get; set; }
        public string pGDPRStatus{ get; set; }
        public string LinguistGDPRStatus { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> Languages { get; set; }
        public string TINNumber { get; set; }
        public List<LinguisticSupplierInvoice> Invoices { get; set; }
        public List<Currency> Currency { get; set; }
        public List<LocalCurrencyInfo> CurrencyInfo { get; set; }
        public string CVFileName { get; set; }
        public string CVName { get; set; }
        public string CVExist { get; set; }
        public string PortfolioFileName { get; set; }
        public string PortfolioName { get; set; }
        public string PortfolioExist { get; set; }
        public IEnumerable<LinguisticSupplierSoftwareApplication> LinguisticSupplierSoftwareApplication { get; set; }
        public string RegistrationFormFileName { get; set; }
        public string RegistrationFormName { get; set; }
        public string RegistrationFormExist { get; set; }
        public string ProofIDFileName { get; set; }
        public string ProofIDName { get; set; }
        public string ProofIDExist { get; set; }
        public Employee LoggedInEmployee { get; set; }
        public byte[] LinguistProfileImage { get; set; }
        public string SubFieldsIds { get; set; }
        public string MasterFieldsIds { get; set; }
        public string MediaTypeIds { get; set; }
        public IEnumerable<DropdownOptionViewModel> MasterFields { get; set; }
        public IEnumerable<SubField> SubFields { get; set; }
        public IEnumerable<DropdownOptionViewModel> MediaTypes { get; set; }
        public string MasterFieldID { get; set; }
        public string SubFieldID { get; set; }
        public string MediaTypeID { get; set; }
        public int Complaints { get; set; }
        public int Compliments { get; set; }
        public string DefaultTimeZone { get; set; }
        public DateTime CurrentInfoTimeLocal { get; set; }
        public string CategoryName { get; set; }
        public string SubjectNotes { get; set; }
        public string CategoryRGBColor { get; set; }
        public string ProofBankFileName { get; set; }
        public string ProofBankName { get; set; }
        public string ProofCompanyFileName { get; set; }
        public string ProofCompanyName { get; set; }
        public LinguisticSupplierInvoiceTemplate LinguisticSupplierInvoiceTemplate { get; set; }
        public string VendorTypeID { get; set; }
        public bool? Referee1Received { get; set; }
        public bool? Referee2Received { get; set; }
        public string Referee1ReceivedString { get; set; }
        public string Referee2ReceivedString { get; set; }
        public LinguistViewModel FindLinguistByNameEmailAddOrSkype { get; set; }
        public string VendorContractFileName { get; set; }
        public string VendorContractName { get; set; }
        public string VendorContractExist { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> LinguistPoliceChecks { get; set; }
        public List<ContactCountry> CountryNamesAndPrefix { get; set; }
        public IEnumerable<ViewModels.Common.DropdownOptionViewModel> LinguisticProfessionalBodies { get; set; }
        public string ProfessionalBodyID { get; set; }
        public List<LinguisticSupplierPoliceCheckDetail> linguisticSupplierPoliceCheckDetails { get; set; }
    }


    public class LinguistRateResults
    {
        public int linguistRateID { get; set; }
        public string serviceName { get; set; }
        public string lingSourceLang { get; set; }
        public string lingTargetLang { get; set; }
        public string lingSubjectArea { get; set; }
        public string lingCurrency { get; set; }
        public string standardRate { get; set; }
        public string linguistRateRateUnit { get; set; }
        public string minimumCharge { get; set; }
        public string appliesTo { get; set; }
        public string rateNotes { get; set; }

        public string lastModified {get; set; }
        public byte appliesToTypeID { get; set; }
        public int appliesToId { get; set; }
        public decimal MemoryRateForPerfectMatches { get; set; }
        public decimal MemoryRateForExactMatches { get; set; }
        public decimal MemoryRateForRepetitions { get; set; }
        public decimal MemoryRateFor95To99Percent { get; set; }
        public decimal MemoryRateFor85To94Percent { get; set; }
        public decimal MemoryRateFor75To84Percent { get; set; }
        public decimal MemoryRateFor50To74Percent { get; set; }
        public decimal MemoryRateForClientSpecificPercent { get; set; }
    }

    public class LinguistItems
    {
        public int itemID { get; set; }
        public string orgName { get; set; }
        public int orgID { get; set; }
        public string contactName { get; set; }
        public int contactID { get; set; }
        public string orderName { get; set; }
        public int orderID { get; set; }
        public string itemService { get; set; }
        public string itemSource { get; set; }
        public string itemTarget { get; set; }
        public string itemStatus { get; set; }
        public decimal itemPaymentToSupplier { get; set; }
        public decimal itemMargin { get; set; }
        public string itemStatusColor { get; set; }
    }

    public class LinguistTop5Words
    {
        public string orgName { get; set; }
        public int orgID { get; set; }
        public int numberOfOrgWords { get; set; }
        public int TotalNumberOfWords { get; set; }
    }

    public class LinguistTop5Items
    {
        public string orgName { get; set; }
        public int orgID { get; set; }
        public int numberOfOrgItems { get; set; }
        public int TotalNumberOfItems { get; set; }
    }
    public class LinguistApprovedOrgs
    {
        public string orgName { get; set; }
        public int orgID { get; set; }
        
    }
    public class LinguistBlockedOrgs
    {
        public string orgName { get; set; }
        public int orgID { get; set; }
       
    }
    public class SoftwareApplications
    {
        public string NameAndVersion { get; set; }
        public int ID { get; set; }

    }
    public partial class PlanningCalendarAppointmentViewModel
    {
        public int Id { get; set; }
        public string ExtranetUserName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string CategoryName { get; set; }
        public short? WorkingTimeStartHours { get; set; }
        public short? WorkingTimeStartMinutes { get; set; }
        public short? WorkingTimeEndHours { get; set; }
        public short? WorkingTimeEndMinutes { get; set; }
        public string SubjectLine { get; set; }
        public string Notes { get; set; }

    }


}
