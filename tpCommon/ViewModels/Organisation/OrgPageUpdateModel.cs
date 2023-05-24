using ViewModels.EmployeeModels;

namespace ViewModels.Organisation
{
    public class OrgPageUpdateModel
    {
        public EmployeeViewModel LoggedInEmployee { get; set; }
        public OrganisationViewModel Organisation { get; set; }
        public string SectionToUpdate { get; set; }

        public string keyInfoSection { get => OrgPageViewModel.keyInfoSectionString; }
        public string generalInfoSection { get => OrgPageViewModel.generalInfoSectionString; }
        public string quotingAndInvoicingSection { get => OrgPageViewModel.quotingAndInvoicingSectionString; }
        public string salesAndMarketingSection { get => OrgPageViewModel.salesAndMarketingSectionString; }
        public string iplusSettingsSection { get => OrgPageViewModel.iplusSettingsSectionString; }
        public string chargeableSoftwareSection { get => OrgPageViewModel.chargeableSoftwareSectionString; }
        public string jobSettingsSection { get => OrgPageViewModel.jobSettingsSectionString; }
        public string lionBoxSection { get => OrgPageViewModel.lionBoxSectionString; }

        // additional props
        public string AutoInvoiceUncheckedExplanationText { get; set; }
    }
}
