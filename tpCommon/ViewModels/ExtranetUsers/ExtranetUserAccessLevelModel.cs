namespace ViewModels.ExtranetUsers
{
    public class ExtranetUserAccessLevelModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanViewDetailsOfOtherOrgUsers { get; set; }
        public bool CanViewDetailsOfOtherGroupUsers { get; set; }
        public bool CanViewDetailsOfOtherOrgOrders { get; set; }
        public bool CanViewDetailsOfOtherGroupOrders { get; set; }
        public bool CanDownloadOtherOrgCompletedOrders { get; set; }
        public bool CanDownloadOtherGroupCompletedOrders { get; set; }
        public bool CanRequestWrittenServicesWork { get; set; }
        public bool CanRequestInterpretingServicesWork { get; set; }
        public bool CanRequestWorkAboveOrgValueThreshold { get; set; }
        public bool CanApproveOrgWorkRequestsAboveThreshold { get; set; }
        public bool CanApproveGroupWorkRequestsAboveThreshold { get; set; }
        public bool CanViewPricingAndCosts { get; set; }
        public bool CanAddAndManageOtherOrgExtranetUsers { get; set; }
        public bool CanAddAndManageOtherGroupExtranetUsers { get; set; }
        public bool CanReviewOwnJobsInOwnLanguageCombos { get; set; }
        public bool CanReviewOtherOrgJobsInOwnLanguageCombos { get; set; }
        public bool CanReviewOtherGroupJobsInOwnLanguageCombos { get; set; }
        public bool CanReviewOtherOrgJobsInAnyLanguageCombo { get; set; }
        public bool CanReviewOtherGroupJobsInAnyLanguageCombo { get; set; }
        public bool CanRerouteReviewJobsToOtherOrgExtranetUsers { get; set; }
        public bool CanRerouteReviewJobsToOtherGroupExtranetUsers { get; set; }
        public bool CanSignOffReviewRequestsWithoutViewingFirst { get; set; }
        public bool CanAddAndEditGlossaryEntries { get; set; }
        public bool CanAccessCmsfunctionality { get; set; }
        public bool CanRequestTranslationFromWithinCms { get; set; }
        public bool CanAddEditAndLockCmsreleases { get; set; }
        public bool CanAddAndEditCmspublications { get; set; }
        public bool CanViewLinguisticSuppliers { get; set; }
    }
}
