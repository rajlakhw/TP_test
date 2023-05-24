namespace ViewModels.JobItem
{
    public class JobItemUpdateModel
    {
        public JobItemViewModel Item { get; set; }
        public int LoggedInEmployeeId { get; set; }
        public string SectionToUpdate { get; set; }
        public string keyInfoSection { get => JobItemViewModel.keyInfoSectionString; }
        public string supplierInfoSection { get => JobItemViewModel.supplierInfoSectionString; }
        public string profitabilitySection { get => JobItemViewModel.profitabilitySectionString; }
        //public string CLSSection { get => CLSSection; }
    }
}
