using ViewModels.Common;
using System.Collections.Generic;
using ViewModels.EmployeeModels;
using System.Linq;
using ViewModels.OrgGroup;
using ViewModels.flowplusLicences;

namespace ViewModels.Organisation
{
    public class OrgPageViewModel
    {
        // dont change these
        public static readonly string keyInfoSectionString = "key-info";
        public static readonly string generalInfoSectionString = "general-info";
        public static readonly string quotingAndInvoicingSectionString = "quoting-and-invoicing";
        public static readonly string salesAndMarketingSectionString = "sales-and-marketing";
        public static readonly string iplusSettingsSectionString = "iplus-settings";
        public static readonly string chargeableSoftwareSectionString = "chargeable-software";
        public static readonly string jobSettingsSectionString = "job-settings";
        public static readonly string lionBoxSectionString = "lion-box";
        public static readonly string flowpluslicencingSectionString = "flow-plus-licencing";
        public static readonly string priceListSectionString = "price-list";

        public EmployeeViewModel LoggedInEmployee { get; set; }
        public OrganisationViewModel Organisation { get; set; }

        public IEnumerable<CountryViewModel> Countries { get; set; }
        public IEnumerable<AltairRegionViewModel> Regions { get; set; }
        public IEnumerable<Data.Currency> Currencies { get; set; }
        public IEnumerable<OrgIndustrySectorViewModel> OrgIndustrySectors { get; set; }
        public IEnumerable<DropdownOptionViewModel> OrgSalesCategories { get; set; }
        public IEnumerable<DropdownOptionViewModel> ClientTechnologies { get; set; }
        public IEnumerable<DropdownOptionViewModel> OrgMainIndustries { get; set; }
        public IEnumerable<DropdownOptionViewModel> OrgIntroSources { get; set; }
        public IEnumerable<DropdownOptionViewModel> OrgLegalStatusCategories { get; set; }
        public IEnumerable<DropdownOptionViewModel> AltairCorporateGroups { get; set; }
        public IEnumerable<DropdownOptionViewModel> DecisionReasons { get; set; }
        public IEnumerable<OrgIndustryRelationshipViewModel> OrgIndustryRelationships { get; set; }
        public IEnumerable<OrgTechnologyRelationshipViewModel> OrgTechnologyRelationships { get; set; }
        public IEnumerable<JobOrder.JobOrderDataTableViewModel> AllJobOrders { get; set; }
        public IEnumerable<Contact.ContactTableViewModel> AllContacts { get; set; }
        public IEnumerable<PriceLists.PriceListTableViewModel> AllPriceLists { get; set; }
        public IEnumerable<QuoteTemplates.QuoteTemplateTableViewModel> AllQuoteTemplates { get; set; }
        public IEnumerable<FileSystem.DownloadbleFile> SharedIplusFiles { get; set; }
        public IEnumerable<ApprovedOrBlockedLinguistTableViewModel> ApprovedOrBlockedLinguists { get; set; }
        public IEnumerable<EnquiriesGroupResults> AllEnquiries { get; set; }
        public IEnumerable<Data.flowPlusChargeFrequency> AllflowPlusChargeFrequencies { get; set; }
        public string SectionToUpdate { get; set; }
        public bool AllowedToEdit { get; set; }
        public bool AllowedToAddContact { get; set; }
        public bool IsEnabledAllowPriorityCheckbox { get; set; }
        public string KeyClientInfoFolder { get; set; }
        public string AutoInvoiceUncheckedExplanationText { get; set; }
#nullable enable
        public string? IsEnabledLinguistRatingCheckbox { get; set; }
        public string? IsEnabledForClientAutomationCheckbox { get; set; }

        public string? OrgSLA { get; set; }

        public string keyInfoSection { get => keyInfoSectionString; }
        public string redAlertSection { get => "red-alert-section"; }
        public string generalInfoSection { get => generalInfoSectionString; }
        public string quotingAndInvoicingSection { get => quotingAndInvoicingSectionString; }
        public string salesAndMarketingSection { get => salesAndMarketingSectionString; }
        public string iplusSettingsSection { get => iplusSettingsSectionString; }
        public string chargeableSoftwareSection { get => chargeableSoftwareSectionString; }
        public string jobSettingsSection { get => jobSettingsSectionString; }
        public string lionBoxSection { get => lionBoxSectionString; }
        public string flowpluslicencingSection { get => flowpluslicencingSectionString; }
        public string priceListSection { get => priceListSectionString; }

#nullable enable
        public bool keyInfoButton { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(keyInfoSection) && x.Page == "Organisation"); }
        public bool redAlertControl { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(redAlertSection) && x.Page == "Organisation"); }
        public bool generalInfoButton { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(generalInfoSection) && x.Page == "Organisation"); }
        public bool quotingAndInvoicingButton { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(quotingAndInvoicingSection) && x.Page == "Organisation"); }
        public bool salesAndMarketingButton { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(salesAndMarketingSection) && x.Page == "Organisation"); }
        public bool iplusSettingsButton { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(iplusSettingsSection) && x.Page == "Organisation"); }
        public bool chargeableSoftwareButton { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(chargeableSoftwareSection) && x.Page == "Organisation"); }
        public bool jobSettingsButton { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(jobSettingsSection) && x.Page == "Organisation"); }
        public bool lionBoxButton { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(lionBoxSection) && x.Page == "Organisation"); }
        public bool flowpluslicencingButton { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(flowpluslicencingSection) && x.Page == "Organisation"); }
        public bool isAllowedToAddPriceList { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(priceListSection) && x.Page == "Organisation"); }
        public flowplusLicenceMappingModel? flowplusLicencing { get; set; }
    }
}
