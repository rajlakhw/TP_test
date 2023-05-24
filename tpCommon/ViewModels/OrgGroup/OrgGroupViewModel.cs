using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data;
using ViewModels.Organisation;
using ViewModels.EmployeeModels;

namespace ViewModels.OrgGroup
{
    public class OrgGroupViewModel
    {
        public static readonly string editableSectionString = "editable-info";
        public static readonly string flowpluslicencingSectionString = "flow-plus-licencing";
        public static readonly string priceListSectionString = "price-list";
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public bool EncryptedSuppliers { get; set; }
        public bool ShowProofreadingOptionToClient { get; set; }
        public IEnumerable<ApprovedOrBlockedLinguistTableViewModel> ApprovedOrBlockedLinguists { get; set; }
        public IEnumerable<QuoteTemplates.QuoteTemplateTableViewModel> AllQuoteTemplates { get; set; }
        public IEnumerable<PriceLists.PriceListTableViewModel> AllPriceLists { get; set; }
        public SelectList BrandList { get; set; }
        public int EmployeCurrentlyLoggedInID { get; set; }
        public string groupCreatedOn { get; set; }
        public short groupCreatedBy { get; set; }
        public string groupCreatedByName { get; set; }
        public string groupCreateByImageBase64 { get; set; }
        public string groupModified { get; set; }
        public DateTime? groupDeletedDate { get; set; }
        public short? groupModifiedBy { get; set; }
        public string groupModifiedByName { get; set; }
        public int? HQOrgID { get; set; }
        public string groupModifiedByImageBase64 { get; set; }
        public string FirstJobDate { get; set; }
        public short? JobFilesCountryID { get; set; }
        public string JobFilesCountryName { get; set; }
        public string SpendLastFinancialYear { get; set; }
        public string SpendThisFinancialYear { get; set; }
        public string SpendLast12Months { get; set; }
        public string SpendLast3Months { get; set; }
        public string InvoicedMarginOverLast3Months { get; set; }
        public List<Brands> BrandNames { get; set; }
        public List<EnquiriesGroupResults> EnqResults { get; set; }
        public List<JobGroupResults> JobResutls { get; set; }
        public List<OrgResults> OrgResutls { get; set; }
        public List<OrgPriceListsResults> PriceListsResutls { get; set; }
        public EmployeeViewModel LoggedInEmployee { get; set; }
        public bool AllowedToEdit { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName == editableSectionString && x.Page == "OrgGroup"); }
        public bool flowpluslicencingButton { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName == flowpluslicencingSectionString && x.Page == "OrgGroup"); }
        public bool isAllowedToAddPriceList { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName.StartsWith(priceListSectionString) && x.Page == "OrgGroup"); }
        public DateTime? ContractExpiryDate { get; set; }
    }

    public class Brands
    {
        public string BrandName { get; set; }
        public int Id { get; set; }

    }


    public class JobGroupResults
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public DateTime SubmittedDateTime { get; set; }
        public DateTime Deadline { get; set; }
        public string JobStatus { get; set; }
        public string JobCurrencyName { get; set; }
        public decimal? JobValue { get; set; }
        public decimal? JobMargin { get; set; }
    }

    public class OrgResults
    {
        public bool OrgHQ { get; set; }
        public int OrgID { get; set; }
        public string OrgName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

    }

    public class OrgPriceListsResults
    {
        public int PriceListID { get; set; }
        public string PriceListName { get; set; }
        public string Currency { get; set; }
        public DateTime InForceSince { get; set; }
        public string AppliesTo { get; set; }


    }

    public class EnquiriesGroupResults
    {
        public int Id { get; set; }
        public string EnqName { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public DateTime SubmittedDateTime { get; set; }
        public string SubmittedBy { get; set; }
        public short SubmittedByID { get; set; }
        public DateTime? Deadline { get; set; }
        public string EnqStatus { get; set; }
        public string EnqReason { get; set; }
        public string EnqCurrencyName { get; set; }
        public decimal? EnqValue { get; set; }
        public DateTime? EnqModified { get; set; }
        public string EnqModifiedBy { get; set; }
        public short? EnqModifiedByID { get; set; }
        public string EnqSales { get; set; }
        public short EnqSalesID { get; set; }
        public string SourceLanguagesCombined { get; set; }
        public string TargetLanguagesCombined { get; set; }

    }

}
