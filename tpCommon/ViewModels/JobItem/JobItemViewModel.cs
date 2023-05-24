using System;
using System.Collections.Generic;
using ViewModels.Common;
using Global_Settings;
using ViewModels.Contact;

namespace ViewModels.JobItem
{
    public class JobItemViewModel
    {
        public int Id { get; set; }
        public int JobOrderId { get; set; }
        public bool IsVisibleToClient { get; set; }
        public byte LanguageServiceId { get; set; }
        public byte? LanguageServiceCategoryId { get; set; }
        public string SourceLanguageIanacode { get; set; }
        public string TargetLanguageIanacode { get; set; }
        public string CustomerSpecificField { get; set; }
        public int? WordCountNew { get; set; }
        public int? WordCountFuzzyBand1 { get; set; }
        public int? WordCountFuzzyBand2 { get; set; }
        public int? WordCountFuzzyBand3 { get; set; }
        public int? WordCountFuzzyBand4 { get; set; }
        public int? WordCountExact { get; set; }
        public int? WordCountRepetitions { get; set; }
        public int? WordCountPerfectMatches { get; set; }
        public int? WordCountClientSpecific { get; set; }
        public byte TranslationMemoryRequired { get; set; }
        public int? Pages { get; set; }
        public int? Characters { get; set; }
        public int? Documents { get; set; }
        public int? InterpretingExpectedDurationMinutes { get; set; }
        public int? InterpretingActualDurationMinutes { get; set; }
        public string InterpretingLocationOrgName { get; set; }
        public string InterpretingLocationAddress1 { get; set; }
        public string InterpretingLocationAddress2 { get; set; }
        public string InterpretingLocationAddress3 { get; set; }
        public string InterpretingLocationAddress4 { get; set; }
        public string InterpretingLocationCountyOrState { get; set; }
        public string InterpretingLocationPostcodeOrZip { get; set; }
        public short? InterpretingLocationCountryId { get; set; }
        public int? AudioMinutes { get; set; }
        public int? WorkMinutes { get; set; }
        public DateTime? SupplierSentWorkDateTime { get; set; }
        public DateTime? SupplierAcceptedWorkDateTime { get; set; }
        public DateTime? SupplierCompletionDeadline { get; set; }
        public DateTime? SupplierCompletedItemDateTime { get; set; }
        public DateTime? OurCompletionDeadline { get; set; }
        public DateTime? WeCompletedItemDateTime { get; set; }
        public string DescriptionForSupplierOnly { get; set; }
        public string FileName { get; set; }
        public bool? SupplierIsClientReviewer { get; set; }
        public int? LinguisticSupplierOrClientReviewerId { get; set; }
        public string ExtranetSignoffComment { get; set; }
        public decimal? ChargeToClient { get; set; }
        public decimal? PaymentToSupplier { get; set; }
        public short? PaymentToSupplierCurrencyId { get; set; }
        public DateTime? SupplierInvoicePaidDate { get; set; }
        public byte? SupplierInvoicePaidMethodId { get; set; }
        public short? ExtranetClientStatusId { get; set; }
        public short? WebServiceClientStatusId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short? CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public string DeletedFreeTextReason { get; set; }
        public decimal? ChargeToClientAfterDiscountSurcharges { get; set; }
        public int? TotalNumberOfReviewSegments { get; set; }
        public int? TotalNumberOfChangedReviewSegments { get; set; }
        public decimal? PercentageOfChangedReviewSegments { get; set; }
        public int? DownloadedByContactId { get; set; }
        public decimal? AnticipatedFinalValueAmount { get; set; }
        public int? SupplierWordCountNew { get; set; }
        public int? SupplierWordCountFuzzyBand1 { get; set; }
        public int? SupplierWordCountFuzzyBand2 { get; set; }
        public int? SupplierWordCountFuzzyBand3 { get; set; }
        public int? SupplierWordCountFuzzyBand4 { get; set; }
        public int? SupplierWordCountExact { get; set; }
        public int? SupplierWordCountRepetitions { get; set; }
        public int? SupplierWordCountPerfectMatches { get; set; }
        public bool? SupplierWordCountsTakenFromClient { get; set; }
        public string ContextFieldsList { get; set; }
        public int? SupplierWordCountClientSpecific { get; set; }
        public byte? SupplierResponsivenessRating { get; set; }
        public byte? SupplierFollowingTheInstructionsRating { get; set; }
        public byte? SupplierAttitudeRating { get; set; }
        public byte? SupplierQualityOfWorkRating { get; set; }
        public string QualityRatingNotGivenReason { get; set; }
        public DateTime? SupplierRatingsGivenDateTime { get; set; }
        public int? SupplierRatingsGivenBy { get; set; }
        public bool? QualityRatingReminderDisabled { get; set; }
        public bool? NotifiedLinguistForApproachingDeadline { get; set; }
        public decimal? AnticipatedGrossMarginPercentage { get; set; }
        public decimal? MarginAfterDiscountSurcharges { get; set; }
        public int? ClientCostCalculatedById { get; set; }
        public DateTime? ClientCostCalculatedByDateTime { get; set; }
        public bool? ClientCostCalculatedByPriceList { get; set; }
        public int? SupplierCostCalculatedById { get; set; }
        public DateTime? SupplierCostCalculatedByDateTime { get; set; }
        public bool? SupplierCostCalculatedByPriceList { get; set; }
        public bool? MinimumSupplierChargeApplied { get; set; }

        // Additional props
        public JobItemMarginCalculations MarginCalculations { get; set; }
        public JobItemAdditionalDetails AdditionalDetails { get; set; }
        public PgdDetails PGDDetails { get; set; }
        public decimal ChargeToClientInGbp { get; set; }
        public decimal ChargeToClientInGbpAfterDiscountSurcharges { get; set; }
        public decimal AnticipatedChargeToClientInGbpAfterDiscountSurcharges { get; set; }
        public decimal PaymentToSupplierInGbp { get; set; }
        public decimal MarginPercentage
        {
            get
            {
                if (this.ChargeToClient == 0 || this.ChargeToClientInGbp == 0)
                {
                    if (this.PaymentToSupplier == 0)
                        return 0;
                    else
                        return -100;
                }
                else if (this.ChargeToClient < 0)
                    return -((this.ChargeToClientInGbp - this.PaymentToSupplierInGbp) / this.ChargeToClientInGbp) * 100;
                else
                    return (this.ChargeToClientInGbp - this.PaymentToSupplierInGbp) / this.ChargeToClientInGbp * 100;
            }
            set { }
        }
        public bool IsCompleted { get; set; }
        /// <summary>
        /// The broader category of the service type (e.g. both consecutive and simultaneous interpreting belong to the single category of "face-to-face interpreting")
        /// </summary>
        public Enumerations.ServiceCategory Category
        {
            get
            {
                // NB hard-coded to database IDs
                switch (this.LanguageServiceId)
                {
                    case 0:
                        return Enumerations.ServiceCategory.NoSpecificCategory;
                    case 1:
                        return Enumerations.ServiceCategory.TranslationAndOtherWrittenServices; // translation
                    case 2:
                        return Enumerations.ServiceCategory.FaceToFaceInterpreting; // consecutive
                    case 3:
                        return Enumerations.ServiceCategory.TelephoneInterpreting;
                    case 4:
                        return Enumerations.ServiceCategory.DesktopPublishing;
                    case 5:
                        return Enumerations.ServiceCategory.TranslationAndOtherWrittenServices; // transcription
                    case 6:
                        return Enumerations.ServiceCategory.AudioServices; // voiceover
                    case 7:
                        return Enumerations.ServiceCategory.TranslationAndOtherWrittenServices; // proofreading
                    case 8:
                        return Enumerations.ServiceCategory.TranslationAndOtherWrittenServices; // linguistic QA
                    case 9:
                        return Enumerations.ServiceCategory.TranslationAndOtherWrittenServices; // software or web testing
                    case 10:
                        return Enumerations.ServiceCategory.FaceToFaceInterpreting; // simultaneous
                    case 11:
                        return Enumerations.ServiceCategory.OtherServices; // linguistic consultancy
                    case 12:
                        return Enumerations.ServiceCategory.OtherServices; // travel time
                    case 13:
                        return Enumerations.ServiceCategory.OtherServices; // travel expenses
                    case 14:
                        return Enumerations.ServiceCategory.TranslationAndOtherWrittenServices; // copywriting
                    case 15:
                        return Enumerations.ServiceCategory.TranslationAndOtherWrittenServices; // memory alignment
                    case 16:
                        return Enumerations.ServiceCategory.ClientCommissionOrReferralPayments;
                    case 17:
                        return Enumerations.ServiceCategory.TranslationAndOtherWrittenServices; // sworn translation
                    case 18:
                        return Enumerations.ServiceCategory.TranslationAndOtherWrittenServices; // Braille
                    case 19:
                        return Enumerations.ServiceCategory.OtherServices; // distribution/postage/packaging
                    case 20:
                        return Enumerations.ServiceCategory.OtherServices; // technology licensing
                    case 21:
                        return Enumerations.ServiceCategory.ClientReview;
                    case 22:
                        return Enumerations.ServiceCategory.Certification;
                    case 23:
                        return Enumerations.ServiceCategory.OtherServices; // file preparation
                    case 24:
                        return Enumerations.ServiceCategory.OtherServices; // Equipment
                    case 34:
                        return Enumerations.ServiceCategory.OtherServices; // Printing
                    case 35:
                        return Enumerations.ServiceCategory.OtherServices; // Subtitling
                    default:
                        return Enumerations.ServiceCategory.OtherServices;
                }
            }
            set { }
        }
        public bool ShowSignOffComments { get; set; }
        public int LoggedInEmployeeId { get; set; }
        public int InterpretingExpectedHours { get; set; }
        public int InterpretingExpectedMinutes { get; set; }
        public int InterpretingActualHours { get; set; }
        public int InterpretingActualMinutes { get; set; }
        public int AudioTimeHours { get; set; }
        public int AudioTimeMinutes { get; set; }
        public int WorkTimeHours { get; set; }
        public int WorkTimeMinutes { get; set; }
        public IEnumerable<CountryViewModel> Countries { get; set; }
        public IEnumerable<DropdownOptionViewModel> Languages { get; set; }
        public IEnumerable<DropdownOptionViewModel> LanguageServices { get; set; }
        public IEnumerable<DropdownOptionViewModel> LanguageServiceCategory { get; set; }
        public IEnumerable<DropdownOptionViewModel> Currencies { get; set; }
        public IEnumerable<DropdownOptionViewModel> PaymentMethods { get; set; }
        public IEnumerable<DropdownOptionViewModel> PGDServices { get; set; }
        public IEnumerable<DropdownOptionViewModel> PGDDropdownItems { get; set; }
        public IEnumerable<PGD.DropdownListItemViewModel> AllPGDDropdownItems { get; set; }
        public List<ContactModel> ClientReviewers { get; set; }
        public bool SupplierInvoiceFieldsEnabled { get; set; }
        public bool SupplierInvoiceCurrencyEnabled { get; set; }
        public bool DeleteButtonEnabled { get; set; }
        public bool CompleteCheckboxEnabled { get; set; }
        public bool EditPageEnabled { get; set; }
        public string  SupplierOrContactName { get; set; }

        // dont change these
        public static readonly string keyInfoSectionString = "key-info";
        public static readonly string supplierInfoSectionString = "supplier-info";
        public static readonly string profitabilitySectionString = "profitability";
        public static readonly string CLSString = "CLS";
        public string SectionToUpdate { get; set; }
        public string keyInfoSection { get => keyInfoSectionString; }
        public string supplierInfoSection { get => supplierInfoSectionString; }
        public string profitabilitySection { get => profitabilitySectionString; }
        //public string CLSSection { get => CLSSection; }
    }

    public class JobItemMarginCalculations
    {
        public int ClientInvoiceId { get; set; }
        public bool ClientInvoiceFinalised { get; set; }
        public DateTime DateToGetRateFrom { get; set; }
        public int OrderCurrencyId { get; set; }
    }

    public class JobItemAdditionalDetails
    {
        public int JobOrderId { get; set; }
        public string JobOrderName { get; set; }
        public int JobOrderChannelId { get; set; }
        public int? JobOrderDesignPlusFileId { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public byte? OrgJobServerLocation { get; set; }
        public string OrgLogoImageBase64 { get; set; }
        public byte[] OrgCustomerLogoImageBinary { get; set; }
        public int OrgGroupId { get; set; }
        public string OrgGroupName { get; set; }
        public bool OrgGroupOnlyAllowEncryptedSuppliers { get; set; }
        public bool IsOrgGroupDeleted { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactEmailAddress { get; set; }
        public byte? FuzzyBand1BottomPercentage { get; set; }
        public byte? FuzzyBand1TopPercentage { get; set; }
        public byte? FuzzyBand2BottomPercentage { get; set; }
        public byte? FuzzyBand2TopPercentage { get; set; }
        public byte? FuzzyBand3BottomPercentage { get; set; }
        public byte? FuzzyBand3TopPercentage { get; set; }
        public byte? FuzzyBand4BottomPercentage { get; set; }
        public byte? FuzzyBand4TopPercentage { get; set; }
        public bool IsCLSJob { get; set; }
        public int OrderCurrencyId { get; set; }
        public string OrderCurrencyName { get; set; }
        public DateTime? EarlyInvoiceDateTime { get; set; }
        public string JobOrderNetworkPath { get; set; }
        public bool IsInterpreting { get; set; }
        public bool OrderOnHold { get; set; }
        public int ClientInvoiceId { get; set; }
        public DateTime? OrderOverallCompletedDateTime { get; set; }
        public DateTime? ArchivedToAmazonS3dateTime { get; set; }
        public DateTime? ArchivedDateTime { get; set; }
        public byte? LionBoxArchivingStatus { get; set; }
        public int? TypeOfOrder { get; set; }
        public decimal? EarlyPaymentDiscount { get; set; }
    }

    public class PgdDetails
    {
        public int Id { get; set; }
        public int JobItemId { get; set; }
        public string Markets { get; set; }
        public string Service { get; set; }
        public string AssetsOverview { get; set; }
        public DateTime? AirDate { get; set; }
        public bool CopydeckStored { get; set; }
        public string Votalent { get; set; }
        public bool BuyoutAgreementSigned { get; set; }
        public string UsageType { get; set; }
        public int? UsageDuration { get; set; }
        public DateTime? UsageStartDate { get; set; }
        public DateTime? UsageEndDate { get; set; }
    }
}
