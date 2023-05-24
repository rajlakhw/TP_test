using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data;
using ViewModels.Organisation;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Contact
{
    public class JobContactResults
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

    public class ContactSource
    {
        public string SourceName { get; set; }
        public int Id { get; set; }

    }

    public class ContactCountry
    {
        public string CountryName { get; set; }
        public int Id { get; set; }
        public string IsoCode { get; set; }
        public string Prefix { get; set; }
    }
    public class EnquiriesContactResults
    {
        public int Id { get; set; }
        public string EnqName { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public DateTime SubmittedDateTime { get; set; }
        public string SubmittedBy { get; set; }
        public short SubmittedByID { get; set; }
        public string Deadline { get; set; }
        public string EnqStatus { get; set; }
        public string EnqReason { get; set; }
        public string EnqCurrencyName { get; set; }
        public decimal? EnqValue { get; set; }
        public DateTime? EnqModified { get; set; }
        public string EnqModifiedBy { get; set; }
        public short? EnqModifiedByID { get; set; }
        public string EnqSales { get; set; }
    }

    public class iPlusUser
    {
        public string username { get; set; }

        public ExtranetUserAccessLevel UserAccessLevel { get; set; }

        public bool translateOnlineEnabled { get; set; }

        public bool designPlusEnabled { get; set; }

    }

    public class ExtranetUserAccessLevel
    {
        public int AccessLevelId { get; set; }

        public string AccessLevelName { get; set; }
    }

    public class ContactViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public DateTime? contactDeletedDate { get; set; }
        public string orgName { get; set; }
        public int orgId { get; set; }
        public string orgDesignPlusStatus { get; set; }
        public string orgTranslateOnlineStatus { get; set; }
        public int orgDesignPlusRemainingUsers { get; set; }
        public int orgTranslateOnlineRemainingUsers { get; set; }
        public DateTime? orgDeletedDate { get; set; }
        public string orgGroupName { get; set; }
        public int orgGroupId { get; set; }
        public DateTime? orgGroupDeletedDate { get; set; }
        public string JobTitle { get; set; }
        public IEnumerable<QuoteTemplates.QuoteTemplateTableViewModel> AllQuoteTemplates { get; set; }
        public IEnumerable<PriceLists.PriceListTableViewModel> AllPriceLists { get; set; }

        public IEnumerable<ApprovedOrBlockedLinguistTableViewModel> ApprovedOrBlockedLinguists { get; set; }
        public byte[] CustomerLogoImageBinary { get; set; }
        public string LogoImageBase64 { get; set; }
        public SelectList TPIntroductionSourceList { get; set; }
        public List<ContactSource> TPIntroductionSourceListIdAndName { get; set; }
        public int? TPIntroductionSource { get; set; }
        public string Department { get; set; }
        public string LandlineNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string iplusName { get; set; }
        public iPlusUser extranetUserObj { get; set; }
        public int? GroupID { get; set; }
        public short? LandlineCountryID { get; set; }
        public short? MobileCountryID { get; set; }
        public short? FaxCountryID { get; set; }
        public SelectList CountryList { get; set; }
        public List<ContactCountry> CountryNamesAndPrefix { get; set; }
        public string notes { get; set; }
        public string contactCreatedOn { get; set; }
        public short contactCreatedBy { get; set; }
        public string contactCreatedByName { get; set; }
        public string createByImageBase64 { get; set; }
        public string contactModified { get; set; }
        public short? contactModifiedBy { get; set; }
        public string contactModifiedByName { get; set; }
        public string modifiedByImageBase64 { get; set; }
        public string GDPRStatus { get; set; }

        public byte? GDPRStatusId { get; set; }
        public bool OptedInForMarketingCampaign { get; set; }
        public bool ApprovedHighLowMargin { get; set; }
        public bool ExcludeZeroMargin { get; set; }
        public bool NotifyOnlyWhenLastJob { get; set; }
        public bool Notifications { get; set; }
        public string NotificationEmails { get; set; }
        public int ContacRelatedAlerts { get; set; }
        public string SectionToUpdate { get; set; }
        public short EmployeCurrentlyLoggedInID { get; set; }

        public string keyInfoSection { get => "KeyInfo"; private set { } }
        public string notesFormSection { get => "NotesForm"; private set { } }
        public string contactSettingsSection { get => "ContactSettingsForm"; private set { } }


        public List<EnquiriesContactResults> EnqResults { get; set; }
        public List<JobContactResults> JobResutls { get; set; }

        public List<ExtranetAccessLevels> AllAvailableAccess { get; set; }

        public IReadOnlyCollection<TimeZoneInfo> AllTimeZones { get; set; }
        public bool AccessToEditContact { get; set; }
        public bool AccessToEditHighLowMargin { get; set; }
        public bool AccessToEditExcludeZeroMargin { get; set; }
        public bool AccessToAddJobOrder { get; set; }
        public bool AccessToEditPriceList { get; set; }


    }
}
