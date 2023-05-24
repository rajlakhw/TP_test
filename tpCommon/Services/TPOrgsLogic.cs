using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.Organisation;
using ViewModels.JobOrder;
using System.IO;
using Microsoft.Extensions.Configuration;
using Global_Settings;
using Microsoft.Data.SqlClient;
using System.Data;
using ViewModels.Contact;
using ViewModels.ExtranetUsers;
using ViewModels.flowplusLicences;

namespace Services
{
    public class TPOrgsLogic : ITPOrgsLogic
    {
        private readonly IRepository<Org> orgRepository;
        private readonly IRepository<Timesheet> timesheetRepository;
        private readonly IRepository<OrgAltairDetail> orgAltairDetailRepository;
        private readonly IRepository<Contact> contactRepository;
        private readonly IRepository<ExtranetUser> extranetUserRepository;
        private readonly IRepository<OrgIndustryRelationship> orgIndustryRelationshipRepository;
        private readonly IRepository<OrgTechnologyRelationship> orgTechnologyRelationshipRepository;
        private readonly IRepository<ClientTechnology> technologyRepository;
        private readonly IRepository<OrgIndustry> industryRepository;
        private readonly IConfiguration configuration;
        private readonly IRepository<OrgGroup> orgGroupRepository;
        private readonly ITPClientInvoicesLogic clientInvoicesService;
        private readonly IRepository<JobOrder> orderRepository;
        private readonly IRepository<ExtranetAccessLevels> extraneAccessLevelRepo;
        private readonly ITPEmployeeOwnershipsLogic ownershipsService;
        private readonly ITPflowplusLicencingLogic flowplusLicencingLogic;


        public TPOrgsLogic(IRepository<Org> repository, IRepository<Timesheet> repository1, IRepository<OrgAltairDetail> orgAltairDetailRepository,
            IRepository<Contact> contactRepository,
            IRepository<OrgIndustryRelationship> orgIndustryRelationshipRepository,
            IRepository<OrgTechnologyRelationship> orgTechnologyRelationshipRepository,
            IRepository<ClientTechnology> TechnologyRepository,
            IRepository<OrgGroup> orgGroupRepository,
            IRepository<OrgIndustry> industryRepository,
            IConfiguration configuration,
            ITPClientInvoicesLogic clientInvoicesService,
            ITPEmployeeOwnershipsLogic ownershipsService,
            IRepository<ExtranetUser> extranetUserRepository,
            IRepository<JobOrder> OrderRepository,
            IRepository<ExtranetAccessLevels> extraneAccessLevelRepo,
            ITPflowplusLicencingLogic _flowplusLicencingLogic)
        {
            this.orgRepository = repository;
            this.timesheetRepository = repository1;
            this.orgAltairDetailRepository = orgAltairDetailRepository;
            this.contactRepository = contactRepository;
            this.orgIndustryRelationshipRepository = orgIndustryRelationshipRepository;
            this.orgTechnologyRelationshipRepository = orgTechnologyRelationshipRepository;
            technologyRepository = TechnologyRepository;
            this.orgGroupRepository = orgGroupRepository;
            this.industryRepository = industryRepository;
            this.configuration = configuration;
            this.clientInvoicesService = clientInvoicesService;
            this.ownershipsService = ownershipsService;
            this.extranetUserRepository = extranetUserRepository;
            this.orderRepository = OrderRepository;
            this.extraneAccessLevelRepo = extraneAccessLevelRepo;
            this.flowplusLicencingLogic = _flowplusLicencingLogic;
        }

        public async Task<Org> GetOrgDetails(int OrgID)
        {
            var result = await orgRepository.All().Where(o => o.Id == OrgID).SingleOrDefaultAsync();
            return result;
        }

        public async Task<List<String>> GetAllOrgSuggestionsResults<Org>(string orgIDOrNameToSearch)
        {

            if (orgIDOrNameToSearch.All(char.IsNumber) == true)
            {
                //var result = await orgRepository.All().Where(o => o.Id.ToString().StartsWith(orgIDOrNameToSearch) ||
                //                    o.OrgName.StartsWith(orgIDOrNameToSearch)).Select(o => new { o.Id, o.OrgName }).ToListAsync();
                var result = await orgRepository.All().Where(o => o.Id.ToString().StartsWith(orgIDOrNameToSearch) && o.DeletedDate == null)
                                                      .Select(o => new { o.Id, o.OrgName }).ToListAsync();
                var newList = new List<String>();
                for (var i = 0; i < result.Count; i++)
                {
                    newList.Add(result.ElementAt(i).Id.ToString() + " - " + result.ElementAt(i).OrgName);
                }
                return newList;
            }
            else
            {
                var result = await orgRepository.All()
                                    .Where(o => o.OrgName.StartsWith(orgIDOrNameToSearch) && o.DeletedDate == null)
                                    .Select(o => new { o.Id, o.OrgName }).ToListAsync();
                var newList = new List<String>();
                for (var i = 0; i < result.Count; i++)
                {
                    newList.Add(result.ElementAt(i).Id.ToString() + " - " + result.ElementAt(i).OrgName);
                }
                return newList;

            }


        }


        public async Task<Dictionary<int, string>> GetAllOrgsIdAndName(bool onlyGetOrgsWithJobs = true)
        {
            Dictionary<int, string> result = null;
            if (onlyGetOrgsWithJobs == true)
            {
                result = await orderRepository.All().Where(o => o.DeletedDate == null).Select(o => o.ContactId)
                                                  .Join(contactRepository.All().Where(c => c.DeletedDate == null).Select(c => new { Id = c.Id, OrgId = c.OrgId }),
                                                  o => o,
                                                  c => c.Id,
                                                  (o, c) => new { orgId = c.OrgId }).Distinct()
                                                  .Join(orgRepository.All().Where(o => o.DeletedDate == null).Select(o => new { Id = o.Id, OrgName = o.OrgName }),
                                                  c => c.orgId,
                                                  o => o.Id,
                                                  (c, o) => new { Id = o.Id, orgName = o.OrgName })
                                                  .Select(o => new { o.Id, o.orgName })
                                                  .Distinct().OrderBy(o => o.orgName).ToDictionaryAsync(i => i.Id, j => j.orgName);
            }
            else
            {
                result = await orgRepository.All().Where(o => o.DeletedDate == null).Select(o => new { o.Id, o.OrgName }).Distinct().OrderBy(o => o.OrgName).ToDictionaryAsync(i => i.Id, j => j.OrgName);
            }


            return result;
        }

        /// <summary>
        /// Gets orgs that have jobs against them
        /// </summary>
        /// <param name="AllGroupIDString"></param>
        /// <returns></returns>
        public async Task<List<string>> GetAllOrgsForOrgGroupString(string AllGroupIDString, bool onlyGetOrgsWithJobs = true)
        {
            if (onlyGetOrgsWithJobs == true)
            {
                if (AllGroupIDString != "")
                {
                    var result = await orgRepository.All().Where(o => o.DeletedDate == null && AllGroupIDString.Contains(o.OrgGroupId.ToString()))
                                        .Join(contactRepository.All(),
                                         o => o.Id,
                                         c => c.OrgId,
                                         (o, c) => new { contact = c, org = o }).Where(c => c.contact.DeletedDate == null)
                                        .Join(orderRepository.All(),
                                         c => c.contact.Id,
                                         o => o.ContactId,
                                         (c, o) => new { org = c.org, order = o })
                                        .Where(o => o.order.DeletedDate == null).Select(o => o.org).Distinct()
                                        .Select(og => new { customName = og.Id.ToString() + " - " + og.OrgName, OrgName = og.OrgName })
                                        .OrderBy(og => og.OrgName).Select(o => o.customName)
                                        .ToListAsync();

                    return result;
                }
                else
                {
                    //only get all orgs that have jobs against them
                    var result = await orderRepository.All().Where(o => o.DeletedDate == null).Select(o => o.ContactId)
                                                      .Join(contactRepository.All(),
                                                      o => o,
                                                      c => c.Id,
                                                      (o, c) => new { contact = c })
                                                      .Where(c => c.contact.DeletedDate == null)
                                                      .Select(c => c.contact.OrgId).Distinct()
                                                      .Join(orgRepository.All(),
                                                      c => c,
                                                      o => o.Id,
                                                      (c, o) => new { org = o })
                                                      .Where(o => o.org.DeletedDate == null)
                                                      .Select(og => new { customName = og.org.Id + " - " + og.org.OrgName, OrgName = og.org.OrgName }).Distinct()
                                                      .OrderBy(og => og.OrgName).Select(o => o.customName)
                                                      .ToListAsync();

                    return result;
                }
            }
            else
            {
                var result = await orgRepository.All().Where(o => o.DeletedDate == null && (AllGroupIDString).Equals(o.OrgGroupId.ToString()))
                                        .Select(og => new { customName = og.Id.ToString() + " - " + og.OrgName, OrgName = og.OrgName })
                                        .OrderBy(og => og.OrgName).Select(o => o.customName)
                                        .ToListAsync();

                return result;
            }



        }

        public async Task<List<string>> GetAllOrgsForInternalExternalFilters(string InternalExternalOrAll)
        {
            string[] externalGroupIDs = { "72112", "72113", "72114", "72115" };
            if (InternalExternalOrAll == "internal")
            {
                var result = await orgRepository.All().Where(o => o.DeletedDate == null && !externalGroupIDs.Contains(o.OrgGroupId.ToString()))
                                    .Join(contactRepository.All().Where(c => c.DeletedDate == null),
                                     o => o.Id,
                                     c => c.OrgId,
                                     (o, c) => new { contact = c, org = o })
                                    .Join(orderRepository.All().Where(o => o.DeletedDate == null),
                                     c => c.contact.Id,
                                     o => o.ContactId,
                                     (c, o) => new { org = c.org })
                                    .Select(og => new { customName = og.org.Id.ToString() + " - " + og.org.OrgName, OrgName = og.org.OrgName }).Distinct()
                                    .OrderBy(og => og.OrgName).Select(o => o.customName)
                                    .ToListAsync();

                return result;
            }
            else if (InternalExternalOrAll == "external")
            {
                var result = await orgRepository.All().Where(o => o.DeletedDate == null && externalGroupIDs.Contains(o.OrgGroupId.ToString()))
                                     .Join(contactRepository.All().Where(c => c.DeletedDate == null),
                                     o => o.Id,
                                     c => c.OrgId,
                                     (o, c) => new { contact = c, org = o })
                                    .Join(orderRepository.All().Where(o => o.DeletedDate == null),
                                     c => c.contact.Id,
                                     o => o.ContactId,
                                     (c, o) => new { org = c.org })
                                    .Select(og => new { customName = og.org.Id.ToString() + " - " + og.org.OrgName, OrgName = og.org.OrgName }).Distinct()
                                    .OrderBy(og => og.OrgName).Select(o => o.customName)
                                    .ToListAsync();

                return result;
            }
            else
            {
                //only get all orgs that have jobs against them
                var result = await orderRepository.All().Where(o => o.DeletedDate == null).Select(o => o.ContactId)
                                                  .Join(contactRepository.All(),
                                                  o => o,
                                                  c => c.Id,
                                                  (o, c) => new { contact = c })
                                                  .Where(c => c.contact.DeletedDate == null)
                                                  .Select(c => c.contact.OrgId).Distinct()
                                                  .Join(orgRepository.All(),
                                                  c => c,
                                                  o => o.Id,
                                                  (c, o) => new { org = o })
                                                  .Where(o => o.org.DeletedDate == null)
                                                  .Select(og => new { customName = og.org.Id + " - " + og.org.OrgName, OrgName = og.org.OrgName }).Distinct()
                                                  .OrderBy(og => og.OrgName).Select(o => o.customName)
                                                  .ToListAsync();

                return result;
            }



        }

        public async Task<List<string>> GetAllTimesheetOrgsForOrgGroupString(string AllGroupIDString)
        {
            if (AllGroupIDString != "")
            {
                var result = await orgRepository.All().Where(o => o.DeletedDate == null && AllGroupIDString.Contains(o.OrgGroupId.ToString()))
                                                        .Join(timesheetRepository.All(),
                                                          og => og.Id,
                                                          t => t.OrgId,
                                                          (og, t) => new { org = og, timesheet = t })
                                    .Select(og => new { customName = og.org.Id + " - " + og.org.OrgName, OrgName = og.org.OrgName }).Distinct()
                                    .OrderBy(og => og.OrgName).Select(o => o.customName)
                                    .ToListAsync();

                return result;
            }
            else
            {
                var result = await orgRepository.All().Where(o => o.DeletedDate == null)
                                                    .Join(timesheetRepository.All(),
                                                          og => og.Id,
                                                          t => t.OrgId,
                                                          (og, t) => new { org = og, timesheet = t })
                                                     .Select(og => new { customName = og.org.Id + " - " + og.org.OrgName, OrgName = og.org.OrgName }).Distinct()
                                                     .OrderBy(og => og.OrgName).Select(o => o.customName)
                                                    .ToListAsync();

                return result;
            }

        }

        public async Task<OrganisationViewModel> GetOrg(int Id)
        {
            var res = new OrganisationViewModel();
            //try
            //{
            //    var org = await orgRepository.All()
            //        .Where(o => o.Id == Id)
            //        .Join(orgAltairDetailRepository.All(),
            //        org => org.Id, alt => alt.OrgId, (org, alt) => new { org, alt })
            //        .Select(x => x.org.CopyProperties(new OrganisationViewModel()
            //        {
            //            HouseNumber = Convert.ToInt32(x.alt.HouseNumber),
            //            AltairRegionId = x.alt.AltairRegionId,
            //            CompanyRegistrationNumber = x.alt.CompanyRegistrationNumber,
            //            Tinnumber = x.alt.Tinnumber,
            //            Siretnumber = x.alt.Siretnumber,
            //            Sirennumber = x.alt.Sirennumber,
            //            Ssnnumber = x.alt.Ssnnumber,
            //            Einnumber = x.alt.Einnumber,
            //            Gstnumber = x.alt.Gstnumber,
            //            Hstnumber = x.alt.Hstnumber,
            //            Sinnumber = x.alt.Sinnumber
            //        }))
            //        .FirstOrDefaultAsync();


            //    res = org as OrganisationViewModel;
            //    if (res == null)
            //    {
            //        return new OrganisationViewModel();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var a = 0;
            //    throw;
            //}

            // var flowplusLicencingDetails = await flowplusLicencingLogic.GetflowPlusLicencingDetailsForDataObject(Id, 2);


            try
            {
                res = await orgRepository.All().Where(x => x.Id == Id)
                    .Join(orgGroupRepository.All(), org => org.OrgGroupId, orgGroup => orgGroup.Id, (org, orgGroup) => new { org, orgGroup })
                    .Select(x => new OrganisationViewModel()
                    {
                        Id = x.org.Id,
                        OrgName = x.org.OrgName,
                        Address1 = x.org.Address1,
                        Address2 = x.org.Address2,
                        Address3 = x.org.Address3,
                        Address4 = x.org.Address4,
                        CountyOrState = x.org.CountyOrState,
                        PostcodeOrZip = x.org.PostcodeOrZip,
                        CountryId = x.org.CountryId,
                        PhoneCountryId = x.org.PhoneCountryId,
                        PhoneNumber = x.org.PhoneNumber,
                        FaxCountryId = x.org.FaxCountryId,
                        FaxNumber = x.org.FaxNumber,
                        EmailAddress = x.org.EmailAddress,
                        WebAddress = x.org.WebAddress,
                        InvoiceDefaultContactId = x.org.InvoiceDefaultContactId,
                        InvoiceDefaultSecondContactId = x.org.InvoiceDefaultSecondContactId,
                        InvoiceOrgName = x.org.InvoiceOrgName,
                        InvoiceAddress1 = x.org.InvoiceAddress1,
                        InvoiceAddress2 = x.org.InvoiceAddress2,
                        InvoiceAddress3 = x.org.InvoiceAddress3,
                        InvoiceAddress4 = x.org.InvoiceAddress4,
                        InvoiceCountyOrState = x.org.InvoiceCountyOrState,
                        InvoicePostcodeOrZip = x.org.InvoicePostcodeOrZip,
                        InvoiceCountryId = x.org.InvoiceCountryId,
                        InvoiceCurrencyId = x.org.InvoiceCurrencyId,
                        InvoiceLangIanacode = x.org.InvoiceLangIanacode,
                        InvoiceBlanketPonumber = x.org.InvoiceBlanketPonumber,
                        InvoicePaymentTermDays = x.org.InvoicePaymentTermDays,
                        PonumbersRequiredForInvoicing = x.org.PonumbersRequiredForInvoicing,
                        InvoicingAutoChaseOverdueContacts = x.org.InvoicingAutoChaseOverdueContacts,
                        InvoicingAutoCreateAndSendInvoices = x.org.InvoicingAutoCreateAndSendInvoices,
                        PonumbersRequiredForGoAheads = x.org.PonumbersRequiredForGoAheads,
                        Vatnumber = x.org.Vatnumber,
                        OrgGroupId = x.org.OrgGroupId,
                        SalesCategoryId = x.org.SalesCategoryId,
                        LegalStatusId = x.org.LegalStatusId,
                        Notes = x.org.Notes,
                        ExcludeFromDirectMarketing = x.org.ExcludeFromDirectMarketing,
                        CustomerLogoImageBinary = x.org.CustomerLogoImageBinary,
                        CustomerSpecificField1Name = x.org.CustomerSpecificField1Name,
                        CustomerSpecificField2Name = x.org.CustomerSpecificField2Name,
                        CustomerSpecificField3Name = x.org.CustomerSpecificField3Name,
                        CustomerSpecificField4Name = x.org.CustomerSpecificField4Name,
                        ExtranetShowClientReviewOptions = x.org.ExtranetShowClientReviewOptions,
                        ExtranetIncludeClientReviewByDefault = x.org.ExtranetIncludeClientReviewByDefault,
                        ExtranetNotifyClientReviewersOfDeliveriesByDefault = x.org.ExtranetNotifyClientReviewersOfDeliveriesByDefault,
                        ExtranetCanSubmitFtpdetailsNotFiles = x.org.ExtranetCanSubmitFtpdetailsNotFiles,
                        ExtranetShowPonumberBox = x.org.ExtranetShowPonumberBox,
                        ExtranetShowDtpoptions = x.org.ExtranetShowDtpoptions,
                        ExtranetDtpoptionSelectedByDefault = x.org.ExtranetDtpoptionSelectedByDefault,
                        CreatedDate = x.org.CreatedDate,
                        CreatedByEmployeeId = x.org.CreatedByEmployeeId,
                        LastModifiedDate = x.org.LastModifiedDate,
                        LastModifiedByEmployeeId = x.org.LastModifiedByEmployeeId,
                        DeletedDate = x.org.DeletedDate,
                        DeletedByEmployeeId = x.org.DeletedByEmployeeId,
                        OriginalorgCodeForXref = x.org.OriginalorgCodeForXref,
                        FuzzyBand1BottomPercentage = x.org.FuzzyBand1BottomPercentage,
                        FuzzyBand1TopPercentage = x.org.FuzzyBand1TopPercentage,
                        FuzzyBand2BottomPercentage = x.org.FuzzyBand2BottomPercentage,
                        FuzzyBand2TopPercentage = x.org.FuzzyBand2TopPercentage,
                        FuzzyBand3BottomPercentage = x.org.FuzzyBand3BottomPercentage,
                        FuzzyBand3TopPercentage = x.org.FuzzyBand3TopPercentage,
                        FuzzyBand4BottomPercentage = x.org.FuzzyBand4BottomPercentage,
                        FuzzyBand4TopPercentage = x.org.FuzzyBand4TopPercentage,
                        PaymentDaysAreFromEndOfMonth = x.org.PaymentDaysAreFromEndOfMonth,
                        ExtranetSendClientReviewDeadlineReminders = x.org.ExtranetSendClientReviewDeadlineReminders.Value,
                        DefaultQuoteApprovalReason = x.org.DefaultQuoteApprovalReason,
                        SpendFrequencyAlertDays = x.org.SpendFrequencyAlertDays,
                        SpendFrequencyAlertLastIssued = x.org.SpendFrequencyAlertLastIssued,
                        IplusTimeOut = x.org.IplusTimeOut,
                        IncludeInNotificationsOn = x.org.IncludeInNotificationsOn,
                        IncludeInNotifications = x.org.IncludeInNotifications,
                        AllowInHouseTranslation = x.org.AllowInHouseTranslation,
                        FirstPaidJobDate = x.org.FirstPaidJobDate,
                        ClientSpendLastFinancialYear = x.org.ClientSpendLastFinancialYear,
                        ClientSpendCurrentFinancialYear = x.org.ClientSpendCurrentFinancialYear,
                        ClientSpendOverLast12Months = x.org.ClientSpendOverLast12Months,
                        ClientSpendOverLast3Months = x.org.ClientSpendOverLast3Months,
                        InvoicedMarginOverLast3Months = x.org.InvoicedMarginOverLast3Months,
                        LastExportedToSageDateTime = x.org.LastExportedToSageDateTime,
                        DesignPlusAccessEnabled = x.org.DesignPlusAccessEnabled,
                        AllowPriority = x.org.AllowPriority,
                        CanCancelJobsViaExtranet = x.org.CanCancelJobsViaExtranet,
                        EarlyPaymentDiscount = x.org.EarlyPaymentDiscount,
                        TranslateonlineStatus = x.org.TranslateonlineStatus,
                        DesignplusStatus = x.org.DesignplusStatus,
                        TranslateonlineTrialStarted = x.org.TranslateonlineTrialStarted,
                        DesignplusTrialStarted = x.org.DesignplusTrialStarted,
                        IsOnRedAlert = x.org.IsOnRedAlert,
                        ShowCustomField1 = x.org.ShowCustomField1,
                        ShowCustomField2 = x.org.ShowCustomField2,
                        ShowCustomField3 = x.org.ShowCustomField3,
                        ShowCustomField4 = x.org.ShowCustomField4,
                        TpintroductionSource = x.org.TpintroductionSource,
                        OtherIntroductionSource = x.org.OtherIntroductionSource,
                        AllowAssigningClientReviewer = x.org.AllowAssigningClientReviewer,
                        ManageReviews = x.org.ManageReviews,
                        AlertClientOfNonApprovedLinguists = x.org.AlertClientOfNonApprovedLinguists,
                        ShowFourthFuzzyBand = x.org.ShowFourthFuzzyBand,
                        CustSpecificPaymentTermDaysEnabled = x.org.CustSpecificPaymentTermDaysEnabled,
                        AllowLateFees = x.org.AllowLateFees,
                        LastAutoInvoicedDate = x.org.LastAutoInvoicedDate,
                        EmailUnsubscribedDateTime = x.org.EmailUnsubscribedDateTime,
                        EmailUnsubscribedReason = x.org.EmailUnsubscribedReason,
                        MainIndustryId = x.org.MainIndustryId,
                        LinguistRatingEnabled = x.org.LinguistRatingEnabled,
                        Ftpenabled = x.org.Ftpenabled,
                        ExtranetAllowRequestOfJobsAndQuotesWithNoDeadline = x.org.ExtranetAllowRequestOfJobsAndQuotesWithNoDeadline,
                        IsOnFinancialRedAlert = x.org.IsOnFinancialRedAlert,
                        JobServerLocation = x.org.JobServerLocation,
                        LinguisticDatabaseId = x.org.LinguisticDatabaseId,
                        EnabledForAutoQuoteGeneration = x.org.EnabledForAutoQuoteGeneration,
                        TradosProjectTemplatePath = x.org.TradosProjectTemplatePath,
                        EnabledForPerSegmentCommenting = x.org.EnabledForPerSegmentCommenting,
                        EnabledForQualityScoringReportOnExtranet = x.org.EnabledForQualityScoringReportOnExtranet,
                        AllowEarlyClientInvoicing = x.org.AllowEarlyClientInvoicing,
                        EnabledForClientAutomation = x.org.EnabledForClientAutomation,
                        OrgTechnology = x.org.OrgTechnology,
                        HfmcodeIs = x.org.HfmcodeIs,
                        HfmcodeBs = x.org.HfmcodeBs,
                        JobFilesToBeSavedWithinRegion = x.org.JobFilesToBeSavedWithinRegion,
                        SapmasterDataReferenceNumber = x.org.SapmasterDataReferenceNumber,
                        IsEnabledForDeadlineChangeReason = x.org.IsEnabledForDeadlineChangeReason,
                        EnabledForOrgLevelMarginsThreshold = x.org.EnabledForOrgLevelMarginsThreshold,
                        MarginsApprovalLowerThreshold = x.org.MarginsApprovalLowerThreshold,
                        MarginsApprovalUpperThreshold = x.org.MarginsApprovalUpperThreshold,
                        ClientLionBoxLink = x.org.ClientLionBoxLink,
                        SupplierLionBoxLink = x.org.SupplierLionBoxLink,
                        OnRedAlertSince = x.org.OnRedAlertSince,
                        OnRedAlertFromEmployeeId = x.org.OnRedAlertFromEmployeeId,
                        LogoImageBase64 = x.org.LogoImageBase64,
                        OrgSLA = x.org.SLA.ToString(),
                        MachineTranslationEnabled = x.org.MachineTranslationEnabled,
                        EnabledForSpecialAutomation = x.org.EnabledForSpecialAutomation,
                        OrgGroupName = x.orgGroup.Name,
                        IsOrgGroupDeleted = x.orgGroup.DeletedDate == null ? false : true
                    }).FirstOrDefaultAsync();
                if (res == null)
                    return null;

                var altairDetails = await orgAltairDetailRepository.All().FirstOrDefaultAsync(x => x.OrgId == Id);
                if (altairDetails != null)
                {
                    res.HouseNumber = altairDetails.HouseNumber;
                    res.AltairRegionId = altairDetails.AltairRegionId;
                    res.City = altairDetails.City;
                    res.CompanyRegistrationNumber = altairDetails.CompanyRegistrationNumber;
                    res.CorporateGroupeId = altairDetails.CorporateGroupeId;
                    res.Tinnumber = altairDetails.Tinnumber;
                    res.Siretnumber = altairDetails.Siretnumber;
                    res.Sirennumber = altairDetails.Sirennumber;
                    res.Ssnnumber = altairDetails.Ssnnumber;
                    res.Einnumber = altairDetails.Einnumber;
                    res.Gstnumber = altairDetails.Gstnumber;
                    res.Hstnumber = altairDetails.Hstnumber;
                    res.Sinnumber = altairDetails.Sinnumber;
                }

                res.AutoClientInvoicingSettingsId = await clientInvoicesService.GetAutoClientInvoicingSettingsId(Id, (int)Enumerations.DataObjectTypes.Org);

                res.AllOwnerships = await ownershipsService.GetEmployeeOwnershipForDataObjectAndOwnershipType(Id, Enumerations.DataObjectTypes.Org);
            }
            catch (Exception ex)
            {
                var a = 0;
            }

            return res;
        }

        public async Task<OrgPageUpdateModel> Update(OrgPageUpdateModel org)
        {
            var dbOrgEntity = await orgRepository.All().Where(x => x.Id == org.Organisation.Id).FirstOrDefaultAsync();

            if (dbOrgEntity == null)
                return org;

            dbOrgEntity.LastModifiedByEmployeeId = org.LoggedInEmployee.Id;
            dbOrgEntity.LastModifiedDate = GeneralUtils.GetCurrentUKTime();

            if (org.SectionToUpdate == org.keyInfoSection)
            {
                var orgAltairDetails = await orgAltairDetailRepository.All().Where(x => x.OrgId == org.Organisation.Id).FirstOrDefaultAsync();

                dbOrgEntity.OrgName = org.Organisation.OrgName;
                if (orgAltairDetails != null) { orgAltairDetails.HouseNumber = org.Organisation.HouseNumber; }
                dbOrgEntity.Address1 = org.Organisation.Address1;
                dbOrgEntity.Address2 = org.Organisation.Address2;
                dbOrgEntity.Address3 = org.Organisation.Address3;
                dbOrgEntity.Address4 = org.Organisation.Address4;
                dbOrgEntity.CountyOrState = org.Organisation.CountyOrState;
                if (orgAltairDetails != null) { orgAltairDetails.City = org.Organisation.City; }
                dbOrgEntity.CountryId = org.Organisation.CountryId;
                if (orgAltairDetails != null) { orgAltairDetails.AltairRegionId = org.Organisation.AltairRegionId; }
                dbOrgEntity.PostcodeOrZip = org.Organisation.PostcodeOrZip;
                dbOrgEntity.EmailAddress = org.Organisation.EmailAddress;
                dbOrgEntity.WebAddress = org.Organisation.WebAddress;
                dbOrgEntity.PhoneNumber = org.Organisation.PhoneNumber;
                dbOrgEntity.PhoneCountryId = org.Organisation.PhoneCountryId;
                dbOrgEntity.FaxNumber = org.Organisation.FaxNumber;
                dbOrgEntity.FaxCountryId = org.Organisation.FaxCountryId;
                dbOrgEntity.IsOnRedAlert = org.Organisation.IsOnRedAlert;
                dbOrgEntity.IsOnFinancialRedAlert = org.Organisation.IsOnFinancialRedAlert;

                await orgAltairDetailRepository.SaveChangesAsync();
            }
            else if (org.SectionToUpdate == org.generalInfoSection)
            {
                dbOrgEntity.Notes = org.Organisation.Notes;
            }
            else if (org.SectionToUpdate == org.quotingAndInvoicingSection)
            {
                var orgAltairDetails = await orgAltairDetailRepository.All().Where(x => x.OrgId == org.Organisation.Id).FirstOrDefaultAsync();
                dbOrgEntity.SLA = Convert.ToInt32(org.Organisation.OrgSLA);
                dbOrgEntity.InvoiceOrgName = org.Organisation.InvoiceOrgName;
                dbOrgEntity.InvoiceAddress1 = org.Organisation.InvoiceAddress1;
                dbOrgEntity.InvoiceAddress2 = org.Organisation.InvoiceAddress2;
                dbOrgEntity.InvoiceAddress3 = org.Organisation.InvoiceAddress3;
                dbOrgEntity.InvoiceAddress4 = org.Organisation.InvoiceAddress4;
                dbOrgEntity.InvoiceCountyOrState = org.Organisation.InvoiceCountyOrState;
                dbOrgEntity.InvoiceCountryId = org.Organisation.InvoiceCountryId;
                dbOrgEntity.InvoicePostcodeOrZip = org.Organisation.InvoicePostcodeOrZip;
                dbOrgEntity.InvoiceDefaultContactId = org.Organisation.InvoiceDefaultContactId;
                dbOrgEntity.InvoiceDefaultSecondContactId = org.Organisation.InvoiceDefaultSecondContactId;
                dbOrgEntity.InvoiceCurrencyId = org.Organisation.InvoiceCurrencyId;
                dbOrgEntity.InvoiceLangIanacode = org.Organisation.InvoiceLangIanacode;
                dbOrgEntity.InvoiceBlanketPonumber = org.Organisation.InvoiceBlanketPonumber;
                dbOrgEntity.PaymentDaysAreFromEndOfMonth = org.Organisation.PaymentDaysAreFromEndOfMonth;
                dbOrgEntity.AllowEarlyClientInvoicing = org.Organisation.AllowEarlyClientInvoicing;
                dbOrgEntity.EarlyPaymentDiscount = org.Organisation.EarlyPaymentDiscount;
                dbOrgEntity.PonumbersRequiredForGoAheads = org.Organisation.PonumbersRequiredForGoAheads;
                dbOrgEntity.PonumbersRequiredForInvoicing = org.Organisation.PonumbersRequiredForInvoicing;
                dbOrgEntity.AllowLateFees = org.Organisation.AllowLateFees;
                dbOrgEntity.InvoicingAutoCreateAndSendInvoices = org.Organisation.InvoicingAutoCreateAndSendInvoices;
                dbOrgEntity.InvoicingAutoChaseOverdueContacts = org.Organisation.InvoicingAutoChaseOverdueContacts;
                dbOrgEntity.Vatnumber = org.Organisation.Vatnumber;
                if (orgAltairDetails != null)
                {
                    orgAltairDetails.CompanyRegistrationNumber = org.Organisation.CompanyRegistrationNumber;
                    orgAltairDetails.Tinnumber = org.Organisation.Tinnumber;
                    await orgAltairDetailRepository.SaveChangesAsync();
                }
                dbOrgEntity.HfmcodeBs = org.Organisation.HfmcodeBs;
                dbOrgEntity.HfmcodeIs = org.Organisation.HfmcodeIs;
                dbOrgEntity.DefaultQuoteApprovalReason = org.Organisation.DefaultQuoteApprovalReason;
                dbOrgEntity.EnabledForAutoQuoteGeneration = org.Organisation.EnabledForAutoQuoteGeneration;
                dbOrgEntity.SapmasterDataReferenceNumber = org.Organisation.SapmasterDataReferenceNumber;
                dbOrgEntity.ShowCustomField1 = org.Organisation.ShowCustomField1;
                dbOrgEntity.ShowCustomField2 = org.Organisation.ShowCustomField2;
                dbOrgEntity.ShowCustomField3 = org.Organisation.ShowCustomField3;
                dbOrgEntity.ShowCustomField4 = org.Organisation.ShowCustomField4;
            }
            else if (org.SectionToUpdate == org.salesAndMarketingSection)
            {
                var orgAltairDetails = await orgAltairDetailRepository.All().Where(x => x.OrgId == org.Organisation.Id).FirstOrDefaultAsync();

                dbOrgEntity.SalesCategoryId = org.Organisation.SalesCategoryId;

                dbOrgEntity.ExcludeFromDirectMarketing = org.Organisation.ExcludeFromDirectMarketing;
                if (org.Organisation.ClientTechnologies != null && org.Organisation.ClientTechnologies.Count() > 0)
                {
                    var techOrgRelationships = await orgTechnologyRelationshipRepository.All().Where(x => x.OrgId == org.Organisation.Id).ToListAsync();
                    if (techOrgRelationships.Count > 0)
                    {
                        var oldRelations = techOrgRelationships.Where(x => org.Organisation.ClientTechnologies.Contains(x.OrgTechnologyId) == false).ToList();
                        oldRelations.ForEach(x => orgTechnologyRelationshipRepository.Delete(x));

                        var newRelations = org.Organisation.ClientTechnologies.Where(x => techOrgRelationships.Select(x => (int)x.OrgTechnologyId).ToList().Contains(x) == false).ToList();
                        if (newRelations != null && newRelations.Count > 0)
                        {
                            var technologiesToAdd = await technologyRepository.All().Where(x => newRelations.Contains(x.Id)).ToListAsync();
                            foreach (var tech in technologiesToAdd)
                            {
                                await orgTechnologyRelationshipRepository.AddAsync(new OrgTechnologyRelationship()
                                {
                                    OrgId = org.Organisation.Id,
                                    OrgTechnologyId = (short)tech.Id
                                });
                            }
                            dbOrgEntity.OrgTechnology = string.Join(",", technologiesToAdd.Select(x => x.TechnologyName));
                        }
                    }
                    else
                    {
                        var technologiesToAdd = await technologyRepository.All().Where(x => org.Organisation.ClientTechnologies.Contains(x.Id)).ToListAsync();
                        foreach (var tech in technologiesToAdd)
                        {
                            await orgTechnologyRelationshipRepository.AddAsync(new OrgTechnologyRelationship()
                            {
                                OrgId = org.Organisation.Id,
                                OrgTechnologyId = (short)tech.Id
                            });
                        }
                    }
                    await orgTechnologyRelationshipRepository.SaveChangesAsync();
                }
                dbOrgEntity.LegalStatusId = org.Organisation.LegalStatusId;
                if (orgAltairDetails != null)
                {
                    orgAltairDetails.CorporateGroupeId = org.Organisation.CorporateGroupeId;
                    await orgAltairDetailRepository.SaveChangesAsync();
                }
                if (org.Organisation.OrgIndustrySectors != null && org.Organisation.OrgIndustrySectors.Count() > 0)
                {
                    var industryOrgRelationships = await orgIndustryRelationshipRepository.All().Where(x => x.OrgId == org.Organisation.Id).ToListAsync();
                    if (industryOrgRelationships.Count > 0)
                    {
                        var oldRelations = industryOrgRelationships.Where(x => org.Organisation.OrgIndustrySectors.Contains(x.OrgIndustryId) == false).ToList();
                        oldRelations.ForEach(x => orgIndustryRelationshipRepository.Delete(x));

                        var newRelations = org.Organisation.OrgIndustrySectors.Where(x => industryOrgRelationships.Select(x => (int)x.OrgIndustryId).ToList().Contains(x) == false).ToList();
                        if (newRelations != null && newRelations.Count > 0)
                        {
                            var industriesToAdd = await industryRepository.All().Where(x => newRelations.Contains(x.Id)).ToListAsync();
                            foreach (var industry in industriesToAdd)
                            {
                                await orgIndustryRelationshipRepository.AddAsync(new OrgIndustryRelationship()
                                {
                                    OrgId = org.Organisation.Id,
                                    OrgIndustryId = industry.Id
                                });
                            }
                        }
                    }
                    else
                    {
                        var industriesToAdd = await industryRepository.All().Where(x => org.Organisation.OrgIndustrySectors.Contains(x.Id)).ToListAsync();
                        foreach (var industry in industriesToAdd)
                        {
                            await orgIndustryRelationshipRepository.AddAsync(new OrgIndustryRelationship()
                            {
                                OrgId = org.Organisation.Id,
                                OrgIndustryId = industry.Id
                            });
                        }
                    }
                    //await orgIndustryRelationshipRepository.SaveChangesAsync();
                }
                dbOrgEntity.MainIndustryId = org.Organisation.MainIndustryId;
                dbOrgEntity.TpintroductionSource = org.Organisation.TpintroductionSource;
                dbOrgEntity.SpendFrequencyAlertDays = org.Organisation.SpendFrequencyAlertDays;
                dbOrgEntity.AlertClientOfNonApprovedLinguists = org.Organisation.AlertClientOfNonApprovedLinguists;
            }
            else if (org.SectionToUpdate == org.iplusSettingsSection)
            {
                dbOrgEntity.ExtranetShowClientReviewOptions = org.Organisation.ExtranetShowClientReviewOptions;
                dbOrgEntity.ExtranetIncludeClientReviewByDefault = org.Organisation.ExtranetIncludeClientReviewByDefault;
                dbOrgEntity.ExtranetSendClientReviewDeadlineReminders = org.Organisation.ExtranetSendClientReviewDeadlineReminders;
                dbOrgEntity.AllowAssigningClientReviewer = org.Organisation.AllowAssigningClientReviewer;
                dbOrgEntity.ManageReviews = org.Organisation.ManageReviews;
                dbOrgEntity.EnabledForPerSegmentCommenting = org.Organisation.EnabledForPerSegmentCommenting;
                dbOrgEntity.ExtranetShowDtpoptions = org.Organisation.ExtranetShowDtpoptions;
                dbOrgEntity.ExtranetDtpoptionSelectedByDefault = org.Organisation.ExtranetDtpoptionSelectedByDefault;
                dbOrgEntity.IplusTimeOut = org.Organisation.IplusTimeOut;
                dbOrgEntity.CanCancelJobsViaExtranet = org.Organisation.CanCancelJobsViaExtranet;
                dbOrgEntity.ExtranetAllowRequestOfJobsAndQuotesWithNoDeadline = org.Organisation.ExtranetAllowRequestOfJobsAndQuotesWithNoDeadline;
                dbOrgEntity.IncludeInNotificationsOn = org.Organisation.IncludeInNotificationsOn;
                dbOrgEntity.IncludeInNotifications = org.Organisation.IncludeInNotifications;
            }
            else if (org.SectionToUpdate == org.jobSettingsSection)
            {
                dbOrgEntity.JobServerLocation = org.Organisation.JobServerLocation;
                dbOrgEntity.JobFilesToBeSavedWithinRegion = org.Organisation.JobFilesToBeSavedWithinRegion;
                dbOrgEntity.AllowPriority = org.Organisation.AllowPriority;
                dbOrgEntity.LinguistRatingEnabled = org.Organisation.LinguistRatingEnabled;
                dbOrgEntity.EnabledForClientAutomation = org.Organisation.EnabledForClientAutomation;
                dbOrgEntity.TradosProjectTemplatePath = org.Organisation.TradosProjectTemplatePath;
                dbOrgEntity.EnabledForOrgLevelMarginsThreshold = org.Organisation.EnabledForOrgLevelMarginsThreshold;

                dbOrgEntity.FuzzyBand1BottomPercentage = org.Organisation.FuzzyBand1BottomPercentage;
                dbOrgEntity.FuzzyBand1TopPercentage = org.Organisation.FuzzyBand1TopPercentage;
                dbOrgEntity.FuzzyBand2BottomPercentage = org.Organisation.FuzzyBand2BottomPercentage;
                dbOrgEntity.FuzzyBand2TopPercentage = org.Organisation.FuzzyBand2TopPercentage;
                dbOrgEntity.FuzzyBand3BottomPercentage = org.Organisation.FuzzyBand3BottomPercentage;
                dbOrgEntity.FuzzyBand3TopPercentage = org.Organisation.FuzzyBand3TopPercentage;
                dbOrgEntity.FuzzyBand4BottomPercentage = org.Organisation.FuzzyBand4BottomPercentage;
                dbOrgEntity.FuzzyBand4TopPercentage = org.Organisation.FuzzyBand4TopPercentage;
            }
            else if (org.SectionToUpdate == org.lionBoxSection)
            {
                dbOrgEntity.ClientLionBoxLink = org.Organisation.ClientLionBoxLink;
                dbOrgEntity.SupplierLionBoxLink = org.Organisation.SupplierLionBoxLink;
            }
            else if (org.SectionToUpdate == org.chargeableSoftwareSection)
            {
                if (dbOrgEntity.TranslateonlineStatus != org.Organisation.TranslateonlineStatus && org.Organisation.TranslateonlineStatus != "Trial")
                {
                    if (org.Organisation.TranslateonlineStatus.EndsWith("+") == false)
                    {
                        int CurrentMaxUsers = 999;
                        if (dbOrgEntity.TranslateonlineStatus.EndsWith("+") == false && dbOrgEntity.TranslateonlineStatus != "Trial")
                        {
                            string dbMaxUsers = dbOrgEntity.TranslateonlineStatus.Split("-")[1];
                            CurrentMaxUsers = int.Parse(dbMaxUsers);
                        }

                        int NewMaxUsers = int.Parse(org.Organisation.TranslateonlineStatus.Split("-")[1]);
                        if (NewMaxUsers < CurrentMaxUsers)
                        {
                            int remainingUsers = await this.GetTranslateonlineRemainingUsers(dbOrgEntity.Id);
                            if ((CurrentMaxUsers - remainingUsers) > NewMaxUsers)
                                this.DisableTranslateOnlineForAllExtranetUsersForOrg(dbOrgEntity.Id);
                        }
                    }
                }

                dbOrgEntity.AllowInHouseTranslation = org.Organisation.AllowInHouseTranslation;
                dbOrgEntity.DesignPlusAccessEnabled = org.Organisation.DesignPlusAccessEnabled;
                dbOrgEntity.TranslateonlineStatus = org.Organisation.TranslateonlineStatus;
                dbOrgEntity.DesignplusStatus = org.Organisation.DesignplusStatus;
            }
            else
            {

            }

            await orgRepository.SaveChangesAsync();

            return org;
        }

        public async Task<IEnumerable<DefaultInvoiceContactViewModel>> GetDefaultInvoiceContanctsForOrg(int orgId, int? orgGroupId)
        {
            var contacts = new List<DefaultInvoiceContactViewModel>();
            if (orgGroupId == 0)
            {
                contacts = await contactRepository.All().Where(x => x.OrgId == orgId && x.DeletedDate == null)
                   .Select(c => new DefaultInvoiceContactViewModel()
                   {
                       Id = c.Id,
                       Name = c.Name,
                       EmailAddress = c.EmailAddress,
                       OrgId = c.OrgId,
                       OrgName = c.InvoiceOrgName
                   })
                   .ToListAsync();
            }
            else
            {
                contacts = await contactRepository.All().Where(x => x.DeletedDate == null)
                    .Join(orgRepository.All(), c => c.OrgId, o => o.Id,
                    (c, o) => new { c, o })
                    .Join(orgGroupRepository.All().Where(x => x.Id == orgGroupId), o => o.o.OrgGroupId, og => og.Id,
                    (o, og) => new { o, og })
                    .Select(x => new DefaultInvoiceContactViewModel()
                    {
                        Id = x.o.c.Id,
                        Name = x.o.c.Name,
                        EmailAddress = x.o.c.EmailAddress,
                        OrgId = x.o.o.Id,
                        OrgName = x.o.o.OrgName
                    })
                    .Distinct()
                    .ToListAsync();
            }
            return contacts;
        }

        public async Task<IEnumerable<OrgIndustryRelationshipViewModel>> GetOrgIdustryRelationship(int orgId) => await orgIndustryRelationshipRepository.All().Where(x => x.OrgId == orgId).Select(x => new OrgIndustryRelationshipViewModel() { Id = x.Id, OrgId = x.OrgId, OrgIndustryId = x.OrgIndustryId }).ToListAsync();

        public async Task<IEnumerable<OrgTechnologyRelationshipViewModel>> GetOrgTechnologyRelationship(int orgId) => await orgTechnologyRelationshipRepository.All().Where(x => x.OrgId == orgId).Select(x => new OrgTechnologyRelationshipViewModel() { OrgTechnologyId = x.OrgTechnologyId, OrgId = x.OrgId, MainTechnologyId = x.MainTechnology }).ToListAsync();

        public string NetworkPriceListInfoDirectoryPathForApp(int orgId, byte? jobServerLocationForApp)
        {
            try
            {
                var config = new GlobalVariables();
                configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                string jobServerPath;
                if (jobServerLocationForApp.GetValueOrDefault() == ((byte)Enumerations.JobDriveBaseDirectoryPathForApp.London))
                    jobServerPath = config.LondonJobDriveBaseDirectoryPathForApp;
                else
                    jobServerPath = config.SofiaJobDriveBaseDirectoryPathForApp;

                // find the first matching directory within the org folder
                // which starts with the order ID, regardless of what comes after it
                string OrgDirSearchPattern = orgId.ToString() + "*";
                string OrgDirPath;
                var DirInfo = new DirectoryInfo(jobServerPath);
                // find org folder first (key client info folder should then appear within that)
                var MatchingOrgDirs = DirInfo.GetDirectories(OrgDirSearchPattern, SearchOption.TopDirectoryOnly);

                if (MatchingOrgDirs.Count() == 0)
                {
                    string newOrgDirSearchPattern = orgId.ToString();
                    var newMatchingOrgDirs = DirInfo.GetDirectories(newOrgDirSearchPattern, SearchOption.TopDirectoryOnly);
                    if (newMatchingOrgDirs.Count() == 0)
                    {
                        string newOrgFolder = Path.Combine(jobServerPath, newOrgDirSearchPattern);
                        Directory.CreateDirectory(newOrgFolder);
                        var MatchingOrgDirsPriceList = DirInfo.GetDirectories(newOrgDirSearchPattern, SearchOption.TopDirectoryOnly);
                        OrgDirPath = MatchingOrgDirsPriceList[0].FullName;
                    }
                    else
                        OrgDirPath = newMatchingOrgDirs[0].FullName;
                }
                // no org folder found, so don't bother searching further
                else
                    OrgDirPath = MatchingOrgDirs[0].FullName;

                // now look for the key client info folder within the org folder
                string ExpectedKeyClientInfoPath = Path.Combine(OrgDirPath, "Key Client Info");
                if (Directory.Exists(ExpectedKeyClientInfoPath) == false)
                    Directory.CreateDirectory(ExpectedKeyClientInfoPath);

                string ExpectedPriceListInfoPath = Path.Combine(ExpectedKeyClientInfoPath, "Price list Info");
                if (Directory.Exists(ExpectedPriceListInfoPath) == true)
                    return ExpectedPriceListInfoPath;
                else
                    Directory.CreateDirectory(ExpectedPriceListInfoPath);
                return ExpectedPriceListInfoPath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<IEnumerable<ApprovedOrBlockedLinguistTableViewModel>> GetApprovedOrBlockedLinguists(int dataObjectId, int dataTypeId)
        {
            var res = new List<ApprovedOrBlockedLinguistTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select 
LanguageServices.Name,
SourceLangs.Name,
TargetLangs.Name,
ABLS.LinguisticSupplierID, 
(CASE WHEN Ls.SupplierTypeID = 2 THEN LS.AgencyOrTeamName ELSE(ISNULL(LS.MainContactFirstName,'')+' '+ISNULL(LS.MainContactSurname,'')) END) as LinguistName, 
ABLS.Status,
ABLS.WorkingPatternID,
--CAST((select (CAST(ABLS.WorkingTimeStartHours as varchar(2)) + ':'+ CAST(ABLS.WorkingTimeStartMinutes as varchar(2)))) as time),
ABLS.WorkingTimeStartHours,
ABLS.WorkingTimeStartMinutes,
ABLS.WorkingTimeEndHours,
ABLS.WorkingTimeEndMinutes,
ISNULL(SoftwareApplications.NameAndVersion, 'N/A'),
ABLS.AppliesToDataObjectTypeID, 
ABLS.AppliesToDataObjectID, 
ISNULL(ABLS.Notes, '')

from ApprovedOrBlockedLinguisticSuppliers as ABLS

INNER JOIN LocalLanguageInfo AS SourceLangs ON SourceLangs.LanguageIANAcodeBeingDescribed = ABLS.SourceLangIANACode
INNER JOIN LocalLanguageInfo AS TargetLangs ON TargetLangs.LanguageIANAcodeBeingDescribed = ABLS.TargetLangIANACode
INNER JOIN LinguisticSuppliers AS LS ON ABLS.LinguisticSupplierID = LS.ID
INNER JOIN LanguageServices ON ABLS.LanguageServiceID = LanguageServices.ID
left JOIN SoftwareApplications ON ABLS.DefaultSoftwareApplicationID = SoftwareApplications.ID

WHERE	AppliesToDataObjectTypeID = " + dataTypeId + @"
AND	AppliesToDataObjectID = " + dataObjectId + @"
AND	SourceLangs.LanguageIANAcode = 'en' -- to help with sorting in the intranet
AND	TargetLangs.LanguageIANAcode = 'en' -- to help with sorting in the intranet
 
ORDER BY LanguageServiceID,
SourceLangs.Name,
TargetLangs.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {

                    var row = new ApprovedOrBlockedLinguistTableViewModel();

                    row.LanguageServiceName = await result.GetFieldValueAsync<string>(0);
                    row.SourceLangName = await result.GetFieldValueAsync<string>(1);
                    row.TargetLangName = await result.GetFieldValueAsync<string>(2);
                    row.LinguistId = await result.GetFieldValueAsync<int>(3);
                    row.LinguistName = await result.GetFieldValueAsync<string>(4);
                    row.Status = await result.GetFieldValueAsync<byte>(5);
                    row.WorkingPatternId = await result.GetFieldValueAsync<byte>(6);
                    row.WorkingTimeStartHours = Convert.IsDBNull(result.GetValue(7)) ? null : await result.GetFieldValueAsync<short>(7);
                    row.WorkingTimeStartMinutes = Convert.IsDBNull(result.GetValue(8)) ? null : await result.GetFieldValueAsync<short>(8);
                    row.WorkingTimeEndHours = Convert.IsDBNull(result.GetValue(9)) ? null : await result.GetFieldValueAsync<short>(9);
                    row.WorkingTimeEndMinutes = Convert.IsDBNull(result.GetValue(10)) ? null : await result.GetFieldValueAsync<short>(10);
                    row.SoftwareApplication = await result.GetFieldValueAsync<string>(11);
                    row.AppliesToDataObjectTypeID = await result.GetFieldValueAsync<byte>(12);
                    row.AppliesToDataObjectID = await result.GetFieldValueAsync<int>(13);
                    row.Notes = await result.GetFieldValueAsync<string>(14);

                    res.Add(row);
                }
            }
            return res.OrderBy(x => x.LinguistName);
        }

        public void DisableTranslateOnlineForAllExtranetUsersForOrg(int orgId)
        {
            string connectionstring = configuration.GetConnectionString("DefaultConnection");
            SqlConnection SQLConn = new SqlConnection(connectionstring);
            SqlDataAdapter ConversionDataAdapter = new SqlDataAdapter("procDisableExtranetUsersFromTranslateOnlineForOrg", SQLConn);

            ConversionDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            // Add & set the date parameters
            ConversionDataAdapter.SelectCommand.Parameters.Add("@OrgID", SqlDbType.Int).Value = orgId;

            //return value parameter:
            var ReturnValParam = ConversionDataAdapter.SelectCommand.Parameters.Add("@RowCount", SqlDbType.Int);
            ReturnValParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                ConversionDataAdapter.SelectCommand.Connection.Open();
                ConversionDataAdapter.SelectCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error running stored procedure to disable translate online.");
            }
            finally
            {
                try
                {
                    ConversionDataAdapter.SelectCommand.Connection.Close();
                    ConversionDataAdapter.Dispose();
                }
                catch (SqlException se)
                {
                    // Log an event in the Application Event Log.
                    throw;
                }
            }
        }

        public async Task<int> GetTranslateonlineRemainingUsers(int orgId)
        {
            var dbOrgEntity = await orgRepository.All().Where(x => x.Id == orgId).FirstOrDefaultAsync();
            var contacts = await contactRepository.All().Where(x => x.DeletedDate == null && x.OrgId == orgId)
                .Join(extranetUserRepository.All(), contact => contact.Id, extranetUser => extranetUser.DataObjectId, (contact, extranetUser) => new { contact, extranetUser })
                .Select(x => new TranslateOnlineContactInfo()
                {
                    ContactId = x.contact.Id,
                    ContactEmail = x.contact.EmailAddress,
                    ContactExtranetUsername = x.extranetUser.UserName,
                    ExtranettranslateOnlineAllowed = x.extranetUser.TranslateonlineAllowed
                }).ToListAsync();

            if (dbOrgEntity == null)
                return -1;

            int RemainingUsers = 0;
            int ActiveUsers = 0;
            int MaxUsers = 0;

            if (dbOrgEntity.TranslateonlineStatus != null)
                if (dbOrgEntity.TranslateonlineStatus != "Trial" && dbOrgEntity.TranslateonlineStatus != "")
                {
                    if (dbOrgEntity.TranslateonlineStatus.Contains("-"))
                        MaxUsers = int.Parse(dbOrgEntity.TranslateonlineStatus.Split("-")[1]);
                    else
                        MaxUsers = 999;

                    if (contacts != null)
                    {
                        foreach (var OrgContact in contacts)
                        {
                            if (OrgContact.ContactEmail != null && OrgContact.ContactEmail.Contains("@translateplus.com") == false)
                                if (OrgContact.ContactExtranetUsername != null && OrgContact.ContactExtranetUsername != "")
                                    if (OrgContact.ExtranettranslateOnlineAllowed == true)
                                        ActiveUsers += 1;
                        }
                    }
                    RemainingUsers = MaxUsers - ActiveUsers;
                }
                else
                    if (dbOrgEntity.TranslateonlineStatus == "Trial")
                    RemainingUsers = 999;

            return RemainingUsers;
        }

        public async Task<int> GetDesignPlusRemainingUsers(int orgId)
        {
            var dbOrgEntity = await orgRepository.All().Where(x => x.Id == orgId).FirstOrDefaultAsync();
            var contacts = await contactRepository.All().Where(x => x.DeletedDate == null && x.OrgId == orgId)
                .Join(extranetUserRepository.All(), contact => contact.Id, extranetUser => extranetUser.DataObjectId, (contact, extranetUser) => new { contact, extranetUser })
                .Select(x => new DesignPlusContactInfo()
                {
                    ContactId = x.contact.Id,
                    ContactEmail = x.contact.EmailAddress,
                    ContactExtranetUsername = x.extranetUser.UserName,
                    ExtranetDesignPlusAllowed = x.extranetUser.DesignplusEnabled
                }).ToListAsync();

            if (dbOrgEntity == null)
                return -1;

            int RemainingUsers = 0;
            int ActiveUsers = 0;
            int MaxUsers = 0;

            if (dbOrgEntity.DesignplusStatus != null)
                if (dbOrgEntity.DesignplusStatus != "Trial" && dbOrgEntity.DesignplusStatus != "")
                {
                    if (dbOrgEntity.DesignplusStatus.Contains("-"))
                        MaxUsers = int.Parse(dbOrgEntity.DesignplusStatus.Split("-")[1]);
                    else
                        MaxUsers = 999;

                    if (contacts != null)
                    {
                        foreach (var OrgContact in contacts)
                        {
                            if (OrgContact.ContactEmail.Contains("@translateplus.com") == false)
                                if (OrgContact.ContactExtranetUsername != null && OrgContact.ContactExtranetUsername != "")
                                    if (OrgContact.ExtranetDesignPlusAllowed == true)
                                        ActiveUsers += 1;
                        }
                    }
                    RemainingUsers = MaxUsers - ActiveUsers;
                }
                else
                    if (dbOrgEntity.DesignplusStatus == "Trial")
                    RemainingUsers = 999;

            return RemainingUsers;
        }

        public async Task<IEnumerable<ContactModel>> GetAllExtranetUsersForOrg(int orgId)
        {
            if (orgId == 0)
                return null;

            return await contactRepository.All().Where(x => x.DeletedDate == null && x.OrgId == orgId)
                .Join(extranetUserRepository.All().Where(x => x.DataObjectTypeId == 1), c => c.Id, e => e.DataObjectId, (c, e) => new { c, e })
                .Join(extraneAccessLevelRepo.All(), a => a.e.AccessLevelId, accessLevel => accessLevel.Id, (a, accessLevel) => new { a, accessLevel })
                .Select(x => new ContactModel()
                {
                    Id = x.a.c.Id,
                    OrgId = x.a.c.OrgId,
                    Name = x.a.c.Name,
                    LandlineCountryId = x.a.c.LandlineCountryId,
                    LandlineNumber = x.a.c.LandlineNumber,
                    MobileCountryId = x.a.c.MobileCountryId,
                    MobileNumber = x.a.c.MobileNumber,
                    FaxCountryId = x.a.c.FaxCountryId,
                    FaxNumber = x.a.c.FaxNumber,
                    EmailAddress = x.a.c.EmailAddress,
                    SkypeId = x.a.c.SkypeId,
                    JobTitle = x.a.c.JobTitle,
                    Department = x.a.c.Department,
                    Notes = x.a.c.Notes,
                    ExtranetOnlyNotifyOnDeliveryOfLastJobItem = x.a.c.ExtranetOnlyNotifyOnDeliveryOfLastJobItem,
                    CreatedDate = x.a.c.CreatedDate,
                    CreatedByEmployeeId = x.a.c.CreatedByEmployeeId,
                    LastModifiedDate = x.a.c.LastModifiedDate,
                    LastModifiedByEmployeeId = x.a.c.LastModifiedByEmployeeId,
                    DeletedDate = x.a.c.DeletedDate,
                    DeletedByEmployeeId = x.a.c.DeletedByEmployeeId,
                    HighLowMarginApprovalNeeded = x.a.c.HighLowMarginApprovalNeeded,
                    ExcludeZeroMarginJobsFromApproval = x.a.c.ExcludeZeroMarginJobsFromApproval,
                    SpendFrequencyAlertDays = x.a.c.SpendFrequencyAlertDays,
                    SpendFrequencyAlertLastIssued = x.a.c.SpendFrequencyAlertLastIssued,
                    IncludeInNotificationsOn = x.a.c.IncludeInNotificationsOn,
                    IncludeInNotifications = x.a.c.IncludeInNotifications,
                    InvoiceOrgName = x.a.c.InvoiceOrgName,
                    InvoiceAddress1 = x.a.c.InvoiceAddress1,
                    InvoiceAddress2 = x.a.c.InvoiceAddress2,
                    InvoiceAddress3 = x.a.c.InvoiceAddress3,
                    InvoiceAddress4 = x.a.c.InvoiceAddress4,
                    InvoiceCountyOrState = x.a.c.InvoiceCountyOrState,
                    InvoicePostcodeOrZip = x.a.c.InvoicePostcodeOrZip,
                    InvoiceCountryId = x.a.c.InvoiceCountryId,
                    EmailUnsubscribedDateTime = x.a.c.EmailUnsubscribedDateTime,
                    EmailUnsubscribedReason = x.a.c.EmailUnsubscribedReason,
                    GdpracceptedDateTime = x.a.c.GdpracceptedDateTime,
                    OptedInForMarketingCampaign = x.a.c.OptedInForMarketingCampaign,
                    LastRespondedToOptInPopup = x.a.c.LastRespondedToOptInPopup,
                    GdpracceptedViaIplus = x.a.c.GdpracceptedViaIplus,
                    GdprrejectedDateTime = x.a.c.GdprrejectedDateTime,
                    GdprverballyAccepted = x.a.c.GdprverballyAccepted,
                    Gdprstatus = x.a.c.Gdprstatus,
                    TpintroductionSource = x.a.c.TpintroductionSource,
                    ExtranetUserName = x.a.e.UserName,
                    ExtranetUserDesignPlusEnabled = x.a.e.DesignplusEnabled,
                    ExtranetUserAccessLevel = new ExtranetUserAccessLevelModel()
                    {
                        Id = x.accessLevel.Id,
                        Name = x.accessLevel.Name,
                        Description = x.accessLevel.Description,
                        CanViewDetailsOfOtherOrgUsers = x.accessLevel.CanViewDetailsOfOtherOrgUsers,
                        CanViewDetailsOfOtherGroupUsers = x.accessLevel.CanViewDetailsOfOtherGroupUsers,
                        CanViewDetailsOfOtherOrgOrders = x.accessLevel.CanViewDetailsOfOtherOrgOrders,
                        CanViewDetailsOfOtherGroupOrders = x.accessLevel.CanViewDetailsOfOtherGroupOrders,
                        CanDownloadOtherOrgCompletedOrders = x.accessLevel.CanDownloadOtherOrgCompletedOrders,
                        CanDownloadOtherGroupCompletedOrders = x.accessLevel.CanDownloadOtherGroupCompletedOrders,
                        CanRequestWrittenServicesWork = x.accessLevel.CanRequestWrittenServicesWork,
                        CanRequestInterpretingServicesWork = x.accessLevel.CanRequestInterpretingServicesWork,
                        CanRequestWorkAboveOrgValueThreshold = x.accessLevel.CanRequestWorkAboveOrgValueThreshold,
                        CanApproveOrgWorkRequestsAboveThreshold = x.accessLevel.CanApproveOrgWorkRequestsAboveThreshold,
                        CanApproveGroupWorkRequestsAboveThreshold = x.accessLevel.CanApproveGroupWorkRequestsAboveThreshold,
                        CanViewPricingAndCosts = x.accessLevel.CanViewPricingAndCosts,
                        CanAddAndManageOtherOrgExtranetUsers = x.accessLevel.CanAddAndManageOtherOrgExtranetUsers,
                        CanAddAndManageOtherGroupExtranetUsers = x.accessLevel.CanAddAndManageOtherGroupExtranetUsers,
                        CanReviewOwnJobsInOwnLanguageCombos = x.accessLevel.CanReviewOwnJobsInOwnLanguageCombos,
                        CanReviewOtherOrgJobsInOwnLanguageCombos = x.accessLevel.CanReviewOtherOrgJobsInOwnLanguageCombos,
                        CanReviewOtherGroupJobsInOwnLanguageCombos = x.accessLevel.CanReviewOtherGroupJobsInOwnLanguageCombos,
                        CanReviewOtherOrgJobsInAnyLanguageCombo = x.accessLevel.CanReviewOtherOrgJobsInAnyLanguageCombo,
                        CanReviewOtherGroupJobsInAnyLanguageCombo = x.accessLevel.CanReviewOtherGroupJobsInAnyLanguageCombo,
                        CanRerouteReviewJobsToOtherOrgExtranetUsers = x.accessLevel.CanRerouteReviewJobsToOtherOrgExtranetUsers,
                        CanRerouteReviewJobsToOtherGroupExtranetUsers = x.accessLevel.CanRerouteReviewJobsToOtherGroupExtranetUsers,
                        CanSignOffReviewRequestsWithoutViewingFirst = x.accessLevel.CanSignOffReviewRequestsWithoutViewingFirst,
                        CanAddAndEditGlossaryEntries = x.accessLevel.CanAddAndEditGlossaryEntries,
                        CanAccessCmsfunctionality = x.accessLevel.CanAccessCmsfunctionality,
                        CanRequestTranslationFromWithinCms = x.accessLevel.CanRequestTranslationFromWithinCms,
                        CanAddEditAndLockCmsreleases = x.accessLevel.CanAddEditAndLockCmsreleases,
                        CanAddAndEditCmspublications = x.accessLevel.CanAddAndEditCmspublications,
                        CanViewLinguisticSuppliers = x.accessLevel.CanViewLinguisticSuppliers
                    }
                }).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<String> JobServerLocationForApp(int orgId)
        {
            var jobServerId = await orgRepository.All().Where(o => o.Id == orgId).Select(j => j.JobServerLocation).FirstOrDefaultAsync();
            var globalVariables = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);

            string jobServerLocationForApp = globalVariables.LondonJobDriveBaseDirectoryPathForApp;
            if (jobServerId == 0)
            {
                jobServerLocationForApp = globalVariables.LondonJobDriveBaseDirectoryPathForApp;
            }
            else if (jobServerId == 1)
            {
                jobServerLocationForApp = globalVariables.SofiaJobDriveBaseDirectoryPathForApp;
            }
            else if (jobServerId == 2)
            {
                jobServerLocationForApp = globalVariables.ParisJobDriveBaseDirectoryPathForApp;
            }

            return jobServerLocationForApp;
        }

        public async Task<String> JobServerLocationForUser(int orgId)
        {
            var jobServerId = await orgRepository.All().Where(o => o.Id == orgId).Select(j => j.JobServerLocation).FirstOrDefaultAsync();
            var globalVariables = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);

            string jobServerLocationForUser = globalVariables.LondonJobDriveBaseDirectoryPathForUser;
            if (jobServerId == 0)
            {
                jobServerLocationForUser = globalVariables.LondonJobDriveBaseDirectoryPathForUser;
            }
            else if (jobServerId == 1)
            {
                jobServerLocationForUser = globalVariables.SofiaJobDriveBaseDirectoryPathForUser;
            }
            else if (jobServerId == 2)
            {
                jobServerLocationForUser = globalVariables.ParisJobDriveBaseDirectoryPathForUser;
            }

            return jobServerLocationForUser;
        }
    }

    class TranslateOnlineContactInfo
    {
        public int ContactId { get; set; }
        public string ContactEmail { get; set; }
        public string ContactExtranetUsername { get; set; }
        public bool ExtranettranslateOnlineAllowed { get; set; }
    }

    class DesignPlusContactInfo
    {
        public int ContactId { get; set; }
        public string ContactEmail { get; set; }
        public string ContactExtranetUsername { get; set; }
        public bool ExtranetDesignPlusAllowed { get; set; }
    }
}
