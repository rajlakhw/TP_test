using System;
using Global_Settings;

namespace ViewModels.JobItem
{
    public class BriefModel
    {
        public int ItemId { get; set; }
        public int? LinguistOrReviewerId { get; set; }
        public string LinguistName { get; set; }
        public string OrderNetworkDirectoryPathForApp { get; set; }
        public int TargetLangId { get; set; }
        public string TargetLangName { get; set; }
        public string JobBriefFilePathForApp { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public DateTime? SupplierSentWorkDateTime { get; set; }
        public int SourceLangId { get; set; }
        public string SourceLangName { get; set; }
        public DateTime SupplierCompletionDeadlineForALoggedInExtranetUser { get; set; }
        public int? SupplierWordCountNew { get; set; }
        public int? SupplierWordCountFuzzyBand1 { get; set; }
        public int? SupplierWordCountFuzzyBand2 { get; set; }
        public int? SupplierWordCountFuzzyBand3 { get; set; }
        public int? SupplierWordCountFuzzyBand4 { get; set; }
        public int? SupplierWordCountExact { get; set; }
        public int? SupplierWordCountRepetitions { get; set; }
        public int? SupplierWordCountPerfectMatches { get; set; }
        public bool? SupplierWordCountsTakenFromClient { get; set; }
        public decimal? PaymentToSupplier { get; set; }
        public string PaymentToSupplierCurrencyPrefix { get; set; }
        public string DescriptionForSupplierOnly { get; set; }
        public string TranslationMemoryRequiredDisplayString
        {
            get
            {
                switch (this.TranslationMemoryRequired)
                {
                    case (byte)Enumerations.TranslationMemoryRequiredValues.No:
                        return "No";
                    case (byte)Enumerations.TranslationMemoryRequiredValues.NotApplicable:
                        return "Not applicable";
                    case (byte)Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons:
                        return "Not known";
                    case (byte)Enumerations.TranslationMemoryRequiredValues.Yes:
                        return "Yes";
                    case (byte)Enumerations.TranslationMemoryRequiredValues.YesAndTagEditorRequired:
                        return "Yes; Trados TagEditor is required";
                    case (byte)Enumerations.TranslationMemoryRequiredValues.YesAndTrados2009or2011Required:
                        return "Yes; Trados 2009 or 2011 is required";
                    case (byte)Enumerations.TranslationMemoryRequiredValues.YesAndAcrossRequired:
                        return "Yes; Across is required";
                    default:
                        return "Not known";
                }
            }
            set { }
        }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public byte? OrgServerLocation { get; set; }
        public string ContactName { get; set; }
        public string ContactLandlineNumber { get; set; }
        public byte TranslationMemoryRequired { get; set; }
        public string InterpretingLocationOrgName { get; set; }
        public string InterpretingLocationAddress1 { get; set; }
        public string InterpretingLocationAddress2 { get; set; }
        public string InterpretingLocationAddress3 { get; set; }
        public string InterpretingLocationAddress4 { get; set; }
        public string InterpretingLocationCountyOrState { get; set; }
        public string InterpretingLocationPostcodeOrZip { get; set; }
        public short? InterpretingLocationCountryId { get; set; }
        public string InterpretingLocationCountryName { get; set; }
        public string InterpretingLocationCombinedOrgAddressAndCountry
        {
            get => String.Join(", ", this.InterpretingLocationOrgName.Trim(), this.InterpretingLocationAddress1.Trim(), this.InterpretingLocationAddress2.Trim(), this.InterpretingLocationAddress3.Trim(), this.InterpretingLocationAddress4.Trim(), this.InterpretingLocationCountyOrState.Trim(), this.InterpretingLocationPostcodeOrZip.Trim(), this.InterpretingLocationCountryName.Trim());

            set { }
        }
        public string ProjectManagerFullName { get; set; }
        public int? InterpretingExpectedDurationMinutes { get; set; }
        public byte LanguageServiceId { get; set; }
        public string LanguageServiceName { get; set; }
        public string ExtranetUserName { get; set; }
        public DateTime? SupplierCompletionDeadline { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string ExtranetUserDefaultTmieZone { get; set; }
    }
}
