using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using ViewModels.JobItem;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using System.Text.RegularExpressions;
using Global_Settings;
using System.Web;
using Microsoft.Data.SqlClient;
using ViewModels.Common;

namespace Services
{
    public class TPJobItemService : ITPJobItemService
    {
        private readonly IRepository<JobItem> itemRepository;
        private readonly IRepository<JobOrder> jobOrderRepository;
        private readonly IRepository<ClientInvoice> invoiceRepository;
        private readonly IRepository<Contact> contactRepository;
        private readonly IRepository<Org> orgRepository;
        private readonly IRepository<OrgGroup> orgGroupRepository;
        private readonly ITPJobOrderService orderService;
        private readonly IRepository<JobItemPgddetail> pgdRepository;
        private readonly IConfiguration configuration;
        private readonly ITPExchangeService tPExchangeService;
        private readonly ITPJobOrderService jobOrderService;
        private readonly ITPTimeZonesService timeZonesService;
        private readonly GlobalVariables globalVariables;
        private readonly ICommonCachedService cachedService;
        private readonly ITPOrgsLogic orgsService;
        private readonly ITPLanguageLogic languageService;
        private readonly ITPflowplusLicencingLogic flowplusService;
        private readonly ITPContactsLogic contactService;
        private readonly ITPOrgsLogic orgService;
        private readonly ITPEmployeeOwnershipsLogic ownershipsLogicService;

        public TPJobItemService(IRepository<JobItem> repository,
            IConfiguration configuration,
            IRepository<JobOrder> jobOrderRepository,
            IRepository<ClientInvoice> invoiceRepository,
            ITPExchangeService tPExchangeService,
            IRepository<Contact> contactRepository,
            IRepository<Org> orgRepository,
            ITPJobOrderService orderService,
            IRepository<JobItemPgddetail> pgdRepository,
            ITPJobOrderService jobOrderService,
            IRepository<OrgGroup> orgGroupRepository,
            ITPTimeZonesService tPTimeZonesService,
            ICommonCachedService cachedService,
            ITPOrgsLogic orgsService,
            ITPLanguageLogic _languageService,
            ITPflowplusLicencingLogic _flowplusService,
            ITPContactsLogic _contactService,
            ITPOrgsLogic _orgService,
            ITPEmployeeOwnershipsLogic tPEmployeeOwnerships
            )
        {
            this.itemRepository = repository;
            this.configuration = configuration;
            this.jobOrderRepository = jobOrderRepository;
            this.invoiceRepository = invoiceRepository;
            this.tPExchangeService = tPExchangeService;
            this.contactRepository = contactRepository;
            this.orgRepository = orgRepository;
            this.orderService = orderService;
            this.pgdRepository = pgdRepository;
            this.jobOrderService = jobOrderService;
            this.orgGroupRepository = orgGroupRepository;
            this.timeZonesService = tPTimeZonesService;
            this.cachedService = cachedService;
            this.orgsService = orgsService;
            this.languageService = _languageService;
            globalVariables = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);
            this.flowplusService = _flowplusService;
            this.contactService = _contactService;
            this.orgService = _orgService;
            this.ownershipsLogicService = tPEmployeeOwnerships;

        }

        public async System.Threading.Tasks.Task GenerateJobBrief(int jobItemId)
        {
            var briefModel = new BriefModel();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select JobItems.ID, JobItems.LanguageServiceID, LanguageServices.Name, SL.Name as SourceLangName, TL.Name as TargetLangName,
JobItems.LinguisticSupplierOrClientReviewerID,
(Case when JobItems.SupplierIsClientReviewer = 1 and JobItems.LinguisticSupplierOrClientReviewerID is not null then ISNULL((LinguisticSuppliers.MainContactFirstName +' '+ LinguisticSuppliers.MainContactSurname), LinguisticSuppliers.AgencyOrTeamName) end) as LinguistName,
JobItems.JobOrderID, JobOrders.JobName, Orgs.ID, Orgs.OrgName, Contacts.Name, Contacts.LandlineNumber, Orgs.JobServerLocation, JobItems.SupplierSentWorkDateTime, 
ExtranetUsers.UserName, JobItems.SupplierCompletionDeadline, ExtranetUsers.DefaultTimeZone, 
JobItems.SupplierWordCountNew, JobItems.SupplierWordCountExact, JobItems.SupplierWordCountRepetitions, 
JobItems.SupplierWordCountPerfectMatches, JobItems.SupplierWordCountFuzzyBand1, JobItems.SupplierWordCountFuzzyBand2, JobItems.SupplierWordCountFuzzyBand3,
JobItems.SupplierWordCountFuzzyBand4, Currencies.Prefix, JobItems.PaymentToSupplier, --LocalCurrencyInfo.CurrencyName,
JobItems.DescriptionForSupplierOnly, JobItems.TranslationMemoryRequired, InterpretingLocationAddress1, InterpretingLocationAddress2, InterpretingLocationAddress3,
InterpretingLocationAddress4, InterpretingLocationOrgName, InterpretingLocationCountyOrState, InterpretingLocationPostcodeOrZip, LocalCountryInfo.CountryName,
InterpretingExpectedDurationMinutes ,(Employees.FirstName +' '+ Employees.Surname) as PMname, JobOrders.SubmittedDateTime

from JobItems

left join LanguageServices on LanguageServices.ID = JobItems.LanguageServiceID
left join LinguisticSuppliers on LinguisticSuppliers.ID = JobItems.LinguisticSupplierOrClientReviewerID
inner join LocalLanguageInfo as SL on SL.LanguageIANAcodeBeingDescribed = JobItems.SourceLanguageIANAcode and SL.LanguageIANAcode = 'en'
inner join LocalLanguageInfo as TL on TL.LanguageIANAcodeBeingDescribed = JobItems.TargetLanguageIANAcode and TL.LanguageIANAcode = 'en'
inner join JobOrders on Joborders.ID = JobItems.JobOrderID
inner join Employees on Employees.ID = JobOrders.ProjectManagerEmployeeID
inner join Contacts on Contacts.ID = JobOrders.ContactID
inner join Orgs on Orgs.ID = Contacts.OrgID
left join ExtranetUsers on ExtranetUsers.DataObjectID = JobItems.LinguisticSupplierOrClientReviewerID
--left join LocalCurrencyInfo on LocalCurrencyInfo.CurrencyID = JobItems.PaymentToSupplierCurrencyID
left join Currencies on Currencies.ID = JobItems.PaymentToSupplierCurrencyID
left join LocalCountryInfo on LocalCountryInfo.CountryID = JobItems.InterpretingLocationCountryID and LocalCountryInfo.LanguageIANAcode = 'en'

where JobItems.ID = " + jobItemId.ToString();
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    briefModel.ItemId = await result.GetFieldValueAsync<int>(0);
                    briefModel.LanguageServiceId = await result.GetFieldValueAsync<byte>(1);
                    briefModel.LanguageServiceName = await result.GetFieldValueAsync<string>(2);
                    briefModel.SourceLangName = await result.GetFieldValueAsync<string>(3);
                    briefModel.TargetLangName = await result.GetFieldValueAsync<string>(4);
                    briefModel.LinguistOrReviewerId = Convert.IsDBNull(result.GetValue(5)) ? null : await result.GetFieldValueAsync<int?>(5);
                    briefModel.LinguistName = Convert.IsDBNull(result.GetValue(6)) ? null : await result.GetFieldValueAsync<string>(6);
                    briefModel.JobId = await result.GetFieldValueAsync<int>(7);
                    briefModel.JobName = await result.GetFieldValueAsync<string>(8);
                    briefModel.OrgId = await result.GetFieldValueAsync<int>(9);
                    briefModel.OrgName = await result.GetFieldValueAsync<string>(10);
                    briefModel.ContactName = await result.GetFieldValueAsync<string>(11);
                    briefModel.ContactLandlineNumber = Convert.IsDBNull(result.GetValue(12)) ? null : await result.GetFieldValueAsync<string>(12);
                    briefModel.OrgServerLocation = Convert.IsDBNull(result.GetValue(13)) ? null : await result.GetFieldValueAsync<byte?>(13);
                    briefModel.SupplierSentWorkDateTime = Convert.IsDBNull(result.GetValue(14)) ? null : await result.GetFieldValueAsync<DateTime>(14);
                    briefModel.ExtranetUserName = Convert.IsDBNull(result.GetValue(15)) ? null : await result.GetFieldValueAsync<string>(15);
                    briefModel.SupplierCompletionDeadline = Convert.IsDBNull(result.GetValue(16)) ? null : await result.GetFieldValueAsync<DateTime>(16);
                    briefModel.ExtranetUserDefaultTmieZone = Convert.IsDBNull(result.GetValue(17)) ? null : await result.GetFieldValueAsync<string>(17);
                    briefModel.SupplierWordCountNew = Convert.IsDBNull(result.GetValue(18)) ? null : await result.GetFieldValueAsync<int?>(18);
                    briefModel.SupplierWordCountExact = Convert.IsDBNull(result.GetValue(19)) ? null : await result.GetFieldValueAsync<int?>(19);
                    briefModel.SupplierWordCountRepetitions = Convert.IsDBNull(result.GetValue(20)) ? null : await result.GetFieldValueAsync<int?>(20);
                    briefModel.SupplierWordCountPerfectMatches = Convert.IsDBNull(result.GetValue(21)) ? null : await result.GetFieldValueAsync<int?>(21);
                    briefModel.SupplierWordCountFuzzyBand1 = Convert.IsDBNull(result.GetValue(22)) ? null : await result.GetFieldValueAsync<int?>(22);
                    briefModel.SupplierWordCountFuzzyBand2 = Convert.IsDBNull(result.GetValue(23)) ? null : await result.GetFieldValueAsync<int?>(23);
                    briefModel.SupplierWordCountFuzzyBand3 = Convert.IsDBNull(result.GetValue(24)) ? null : await result.GetFieldValueAsync<int?>(24);
                    briefModel.SupplierWordCountFuzzyBand4 = Convert.IsDBNull(result.GetValue(25)) ? null : await result.GetFieldValueAsync<int?>(25);
                    briefModel.PaymentToSupplierCurrencyPrefix = Convert.IsDBNull(result.GetValue(26)) ? null : await result.GetFieldValueAsync<string>(26);
                    briefModel.PaymentToSupplier = Convert.IsDBNull(result.GetValue(27)) ? null : await result.GetFieldValueAsync<decimal?>(27);
                    briefModel.DescriptionForSupplierOnly = Convert.IsDBNull(result.GetValue(28)) ? null : await result.GetFieldValueAsync<string>(28);
                    briefModel.TranslationMemoryRequired = await result.GetFieldValueAsync<byte>(29);
                    briefModel.InterpretingLocationAddress1 = Convert.IsDBNull(result.GetValue(30)) ? "" : await result.GetFieldValueAsync<string>(30);
                    briefModel.InterpretingLocationAddress2 = Convert.IsDBNull(result.GetValue(31)) ? "" : await result.GetFieldValueAsync<string>(31);
                    briefModel.InterpretingLocationAddress3 = Convert.IsDBNull(result.GetValue(32)) ? "" : await result.GetFieldValueAsync<string>(32);
                    briefModel.InterpretingLocationAddress4 = Convert.IsDBNull(result.GetValue(33)) ? "" : await result.GetFieldValueAsync<string>(33);
                    briefModel.InterpretingLocationOrgName = Convert.IsDBNull(result.GetValue(34)) ? "" : await result.GetFieldValueAsync<string>(34);
                    briefModel.InterpretingLocationCountyOrState = Convert.IsDBNull(result.GetValue(35)) ? "" : await result.GetFieldValueAsync<string>(35);
                    briefModel.InterpretingLocationPostcodeOrZip = Convert.IsDBNull(result.GetValue(36)) ? "" : await result.GetFieldValueAsync<string>(36);
                    briefModel.InterpretingLocationCountryName = Convert.IsDBNull(result.GetValue(37)) ? "" : await result.GetFieldValueAsync<string>(37);
                    briefModel.InterpretingExpectedDurationMinutes = Convert.IsDBNull(result.GetValue(38)) ? null : await result.GetFieldValueAsync<int>(38);
                    briefModel.ProjectManagerFullName = await result.GetFieldValueAsync<string>(39);
                    briefModel.SubmittedDate = await result.GetFieldValueAsync<DateTime>(40);
                }
            }


            bool UseInterpretingBrief = briefModel.LanguageServiceName.ToLower().Contains("interpreting");
            bool UseProofreadingBrief = briefModel.LanguageServiceName.ToLower().Contains("proofreading");

            briefModel.OrderNetworkDirectoryPathForApp = jobOrderService.NetworkDirectoryPathForApp(briefModel.JobId, briefModel.OrgId, briefModel.OrgServerLocation.GetValueOrDefault(), false, briefModel.SubmittedDate);

            try
            {
                string SupplierNameForFileName;
                if (briefModel.LinguistName == null || briefModel.LinguistName == "")
                    SupplierNameForFileName = "";
                else
                    SupplierNameForFileName = " - " + briefModel.LinguistName;

                string ExpectedPath = System.IO.Path.Combine(briefModel.OrderNetworkDirectoryPathForApp, "Briefs") + "\\01 To\\Job brief " + briefModel.ItemId.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(briefModel.TargetLangName + SupplierNameForFileName) + ".docx";

                briefModel.JobBriefFilePathForApp = ExpectedPath;
            }
            catch (Exception ex)
            {
                // doesn't matter why it couldn't determine the file location - just return empty path
                briefModel.JobBriefFilePathForApp = "";
            }

            // copy .docx "template" (overwrite existing brief if it exists)
            try
            {
                // NB copy time sheet for F2F interpreting jobs but for now, anything
                // else gets a proofreading brief or as a fallback, a translation brief
                if (UseInterpretingBrief == true)
                    File.Copy(@"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML\interpreting timesheet brief.docx", briefModel.JobBriefFilePathForApp, true);
                else if (UseProofreadingBrief == true)
                    File.Copy(@"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML\proofreading project brief.docx", briefModel.JobBriefFilePathForApp, true);
                else
                    File.Copy(@"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML\translation project brief.docx", briefModel.JobBriefFilePathForApp, true);
            }
            catch (Exception ex)
            {
                //for now, just explain detail of error if any problem
                throw new Exception("Could not copy job brief template file: " + ex.Message);
            }
            // open package/doc
            WordprocessingDocument BriefDoc = WordprocessingDocument.Open(briefModel.JobBriefFilePathForApp, true);

            using (BriefDoc)
            {
                string docText = String.Empty;
                StreamReader sr = new StreamReader(BriefDoc.MainDocumentPart.GetStream());
                using (sr)
                {
                    docText = sr.ReadToEnd();
                }

                // replace the various "placeholder" texts with values for this job item
                Regex regexText = new Regex("XXJobNameXX");
                docText = regexText.Replace(docText, GeneralUtils.ConvertAmpersandsAndAngledBracketsForXML(briefModel.JobName));

                regexText = new Regex("XXBriefDateXX");
                if (briefModel.SupplierSentWorkDateTime == null)
                    docText = regexText.Replace(docText, GeneralUtils.GetCurrentGMT().ToString("d MMMM yyyy"));
                else
                    docText = regexText.Replace(docText, briefModel.SupplierSentWorkDateTime.GetValueOrDefault().ToString("d MMMM yyyy"));

                regexText = new Regex("XXSourceLangXX");
                docText = regexText.Replace(docText, briefModel.SourceLangName);

                regexText = new Regex("XXTargetLangXX");
                docText = regexText.Replace(docText, briefModel.TargetLangName);

                // NB for interpreting assignments "deadline" = the date/time of the assignment
                regexText = new Regex("XXDeadlineDateXX");
                if (briefModel.SupplierCompletionDeadlineForALoggedInExtranetUser == DateTime.MinValue)
                    // if (no deadline yet, keep this instruction in the file to our internal
                    // employees as a reminder of what to insert.
                    docText = regexText.Replace(docText, "(include day of week)");
                else
                    docText = regexText.Replace(docText, briefModel.SupplierCompletionDeadlineForALoggedInExtranetUser.ToString("dddd d MMMM yyyy"));

                regexText = new Regex("XXDeadlineTimeXX");
                if (briefModel.SupplierCompletionDeadlineForALoggedInExtranetUser == DateTime.MinValue)
                    // if (no deadline yet, keep this instruction in the file to our internal
                    // employees as a reminder of what to insert
                    docText = regexText.Replace(docText, "(include time zone, e.g. UK time)");
                else
                {
                    // include "UK time" specification for translation, but not for interpreting jobs
                    if (UseInterpretingBrief == true)
                        docText = regexText.Replace(docText, briefModel.SupplierCompletionDeadlineForALoggedInExtranetUser.ToString("HH:mm")) + "(" + briefModel.ExtranetUserDefaultTmieZone + ")";
                    else
                        docText = regexText.Replace(docText, briefModel.SupplierCompletionDeadlineForALoggedInExtranetUser.ToString("HH:mm") + " UK time");
                }

                regexText = new Regex("XXNewXX");
                if (briefModel.SupplierWordCountNew == null)
                    // if (word count not set, insert empty string rather than zero
                    docText = regexText.Replace(docText, "");
                else
                    docText = regexText.Replace(docText, briefModel.SupplierWordCountNew.ToString());

                regexText = new Regex("XXExactXX");
                if (briefModel.SupplierWordCountExact == null && briefModel.SupplierWordCountRepetitions == null)
                    // if (word count not set, insert empty string rather than zero
                    docText = regexText.Replace(docText, "");
                else
                {
                    // either exact or new may not be set so avoid problems with nulls/zeroes
                    int ExactPlusRepsWordCount;
                    if (briefModel.SupplierWordCountExact == null)
                        ExactPlusRepsWordCount = briefModel.SupplierWordCountRepetitions.GetValueOrDefault();
                    else if (briefModel.SupplierWordCountRepetitions == null)
                        ExactPlusRepsWordCount = briefModel.SupplierWordCountExact.GetValueOrDefault();
                    else
                    {
                        ExactPlusRepsWordCount = briefModel.SupplierWordCountExact.GetValueOrDefault() + briefModel.SupplierWordCountRepetitions.GetValueOrDefault();
                    }
                    docText = regexText.Replace(docText, ExactPlusRepsWordCount.ToString());
                }

                regexText = new Regex("XXFuzzyBand1XX");
                if (briefModel.SupplierWordCountFuzzyBand1 == null)
                    // if (word count not set, insert empty string rather than zero
                    docText = regexText.Replace(docText, "");
                else
                    docText = regexText.Replace(docText, briefModel.SupplierWordCountFuzzyBand1.ToString());

                regexText = new Regex("XXFuzzyBand2XX");
                if (briefModel.SupplierWordCountFuzzyBand2 == null)
                    // if (word count not set, insert empty string rather than zero
                    docText = regexText.Replace(docText, "");
                else
                    docText = regexText.Replace(docText, briefModel.SupplierWordCountFuzzyBand2.ToString());

                regexText = new Regex("XXFuzzyBand3XX");
                if (briefModel.SupplierWordCountFuzzyBand3 == null)
                    // if (word count not set, insert empty string rather than zero
                    docText = regexText.Replace(docText, "");
                else
                    docText = regexText.Replace(docText, briefModel.SupplierWordCountFuzzyBand3.ToString());

                regexText = new Regex("XXFuzzyBand4XX");
                if (briefModel.SupplierWordCountFuzzyBand4 == null)
                    // if (word count not set, insert empty string rather than zero
                    docText = regexText.Replace(docText, "");
                else
                    docText = regexText.Replace(docText, briefModel.SupplierWordCountFuzzyBand4.ToString());

                regexText = new Regex("XXPerfectXX");
                if (briefModel.SupplierWordCountPerfectMatches == null)
                    // if (word count not set, insert empty string rather than zero
                    docText = regexText.Replace(docText, "");
                else
                    docText = regexText.Replace(docText, briefModel.SupplierWordCountPerfectMatches.ToString());

                regexText = new Regex("XXPaymentToSupplierXX");
                if (briefModel.PaymentToSupplier == null || briefModel.PaymentToSupplier == 0)
                    // if (no supplier payment yet, keep this instruction in the file to our internal
                    // employees as a reminder of what to insert
                    docText = regexText.Replace(docText, "(specify rate and any agreement about match rates)");
                else
                    docText = regexText.Replace(docText, briefModel.PaymentToSupplierCurrencyPrefix + String.Format("{0:F}", briefModel.PaymentToSupplier) + " total");

                regexText = new Regex("XXJobItemIDXX");
                docText = regexText.Replace(docText, briefModel.ItemId.ToString());

                regexText = new Regex("XXDescriptionForSupplierXX");
                if (briefModel.DescriptionForSupplierOnly == null)
                {
                    // if (no descriptive info/brief details yet, keep this instruction in the file to our internal
                    // employees as a reminder of what to insert (but not for interpreting jobs)
                    if (UseInterpretingBrief == true)
                        docText = regexText.Replace(docText, "");
                    else
                        docText = regexText.Replace(docText, "Once you have filled in the details here, please use the \"Finalise and save brief\" button to protect the document so that the translator can only fill out the checklist section below.");
                }
                else
                    // convert hard returns and XML stuff as needed
                    docText = regexText.Replace(docText, GeneralUtils.ConvertHardReturnsForInnerWordML(GeneralUtils.ConvertAmpersandsAndAngledBracketsForXML(briefModel.DescriptionForSupplierOnly)));

                regexText = new Regex("XXMemoryRequiredXX");
                if (briefModel.TranslationMemoryRequired == ((byte)Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons))
                    // if (no descriptive info/brief details yet, keep this instruction in the file to our internal
                    // employees as a reminder of what to insert
                    docText = regexText.Replace(docText, "Yes/No (and mention if (TagEditor or specific versions needed)");
                else
                    docText = regexText.Replace(docText, briefModel.TranslationMemoryRequiredDisplayString);

                regexText = new Regex("XXLinguistNameXX");
                if (briefModel.LinguistName == null || briefModel.LinguistName == "")
                {
                    // for non-interpreting briefs, indicate where the supplier name should go
                    if (UseInterpretingBrief == true)
                        docText = regexText.Replace(docText, "");
                    else
                        docText = regexText.Replace(docText, "(name of linguistic supplier)");
                }
                else
                    docText = regexText.Replace(docText, GeneralUtils.ConvertAmpersandsAndAngledBracketsForXML(briefModel.LinguistName));

                regexText = new Regex("XXOrgNameXX");
                docText = regexText.Replace(docText, GeneralUtils.ConvertAmpersandsAndAngledBracketsForXML(briefModel.OrgName));

                //regexText = new Regex("XXOrgIDXX")
                //docText = regexText.Replace(docText, briefModel.arentJobOrder.OrderContact.ParentOrgID)

                regexText = new Regex("XXContactNameXX");
                docText = regexText.Replace(docText, GeneralUtils.ConvertAmpersandsAndAngledBracketsForXML(briefModel.ContactName));


                regexText = new Regex("XXContactPhoneXX");
                if (briefModel.ContactLandlineNumber == null || briefModel.ContactLandlineNumber == "")
                    docText = regexText.Replace(docText, "");
                else
                    // remove any brackets from phone numbers
                    docText = regexText.Replace(docText, briefModel.ContactLandlineNumber.Replace(")", "").Replace("(", ""));

                regexText = new Regex("XXLocationXX");
                if (briefModel.InterpretingLocationCombinedOrgAddressAndCountry == "")
                    docText = regexText.Replace(docText, "");
                else
                    docText = regexText.Replace(docText, GeneralUtils.ConvertAmpersandsAndAngledBracketsForXML(briefModel.InterpretingLocationCombinedOrgAddressAndCountry));

                regexText = new Regex("XXProjectManagerXX");
                docText = regexText.Replace(docText, GeneralUtils.ConvertAmpersandsAndAngledBracketsForXML(briefModel.ProjectManagerFullName));

                regexText = new Regex("XXExpectedDurationXX");
                if (briefModel.InterpretingExpectedDurationMinutes == 0)
                    docText = regexText.Replace(docText, "");
                else
                {
                    int ExpectedHours = ((int)Math.Floor((decimal)(briefModel.InterpretingExpectedDurationMinutes / 60)));
                    int ExpectedMinutes = briefModel.InterpretingExpectedDurationMinutes.GetValueOrDefault() % 60;

                    if (ExpectedHours == 1 && ExpectedMinutes == 0)
                        docText = regexText.Replace(docText, "1 hour");
                    else if (ExpectedHours == 1)
                        docText = regexText.Replace(docText, ExpectedHours.ToString("N0") + " hour and " + ExpectedMinutes.ToString("N0") + " minutes");
                    else if (ExpectedMinutes == 0)
                        docText = regexText.Replace(docText, ExpectedHours.ToString("N0") + " hours");
                    else
                        docText = regexText.Replace(docText, ExpectedHours.ToString("N0") + " hours and " + ExpectedMinutes.ToString("N0") + " minutes");
                }

                StreamWriter sw = new StreamWriter(BriefDoc.MainDocumentPart.GetStream(FileMode.Create));
                using (sw)
                {
                    sw.Write(docText);
                }
            }
        }

        public async Task<JobItem> GetById(int JobItemID)
        {
            var result = await itemRepository.All().Where(a => a.Id == JobItemID && a.DeletedDateTime == null).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<JobItem>> GetByJobOrderId(int JobOrderID)
        {
            var result = await itemRepository.All().Where(a => a.JobOrderId == JobOrderID && a.DeletedDateTime == null).ToListAsync();
            return result;
        }

        public async Task<List<JobItem>> GetBySourceLangForJobOrderId(int JobOrderID, string SourceLanguageIANACode)
        {
            var result = await itemRepository.All().Where(a => a.JobOrderId == JobOrderID && a.DeletedDateTime == null && a.SourceLanguageIanacode == SourceLanguageIANACode).OrderBy(a => a.SourceLanguageIanacode).ToListAsync();
            return result;
        }

        public async Task<List<JobItem>> GetByTargetLangForJobOrderId(int JobOrderID, string TargetLanguageIANACode)
        {
            var result = await itemRepository.All().Where(a => a.JobOrderId == JobOrderID && a.DeletedDateTime == null && a.TargetLanguageIanacode == TargetLanguageIANACode).OrderBy(a => a.OurCompletionDeadline).ToListAsync();
            return result;
        }

        public async Task<JobItemViewModel> GetViewModelById(int id)
        {
            var result = await itemRepository.All().Where(x => x.Id == id && x.DeletedDateTime == null)
                .Select(x => new JobItemViewModel()
                {
                    Id = x.Id,
                    JobOrderId = x.JobOrderId,
                    IsVisibleToClient = x.IsVisibleToClient,
                    LanguageServiceId = x.LanguageServiceId,
                    SourceLanguageIanacode = x.SourceLanguageIanacode,
                    TargetLanguageIanacode = x.TargetLanguageIanacode,
                    CustomerSpecificField = x.CustomerSpecificField,
                    WordCountNew = x.WordCountNew,
                    WordCountFuzzyBand1 = x.WordCountFuzzyBand1,
                    WordCountFuzzyBand2 = x.WordCountFuzzyBand2,
                    WordCountFuzzyBand3 = x.WordCountFuzzyBand3,
                    WordCountFuzzyBand4 = x.WordCountFuzzyBand4,
                    WordCountExact = x.WordCountExact,
                    WordCountRepetitions = x.WordCountRepetitions,
                    WordCountPerfectMatches = x.WordCountPerfectMatches,
                    WordCountClientSpecific = x.WordCountClientSpecific,
                    TranslationMemoryRequired = x.TranslationMemoryRequired,
                    Pages = x.Pages,
                    Characters = x.Characters,
                    Documents = x.Documents,
                    InterpretingExpectedDurationMinutes = x.InterpretingExpectedDurationMinutes,
                    InterpretingActualDurationMinutes = x.InterpretingActualDurationMinutes,
                    InterpretingLocationOrgName = x.InterpretingLocationOrgName,
                    InterpretingLocationAddress1 = x.InterpretingLocationAddress1,
                    InterpretingLocationAddress2 = x.InterpretingLocationAddress2,
                    InterpretingLocationAddress3 = x.InterpretingLocationAddress3,
                    InterpretingLocationAddress4 = x.InterpretingLocationAddress4,
                    InterpretingLocationCountyOrState = x.InterpretingLocationCountyOrState,
                    InterpretingLocationPostcodeOrZip = x.InterpretingLocationPostcodeOrZip,
                    InterpretingLocationCountryId = x.InterpretingLocationCountryId,
                    AudioMinutes = x.AudioMinutes,
                    WorkMinutes = x.WorkMinutes,
                    SupplierSentWorkDateTime = x.SupplierSentWorkDateTime,
                    SupplierAcceptedWorkDateTime = x.SupplierAcceptedWorkDateTime,
                    SupplierCompletionDeadline = x.SupplierCompletionDeadline,
                    SupplierCompletedItemDateTime = x.SupplierCompletedItemDateTime,
                    OurCompletionDeadline = x.OurCompletionDeadline,
                    WeCompletedItemDateTime = x.WeCompletedItemDateTime,
                    DescriptionForSupplierOnly = x.DescriptionForSupplierOnly,
                    FileName = x.FileName,
                    SupplierIsClientReviewer = x.SupplierIsClientReviewer,
                    LinguisticSupplierOrClientReviewerId = x.LinguisticSupplierOrClientReviewerId,
                    ExtranetSignoffComment = x.ExtranetSignoffComment,
                    ChargeToClient = x.ChargeToClient,
                    PaymentToSupplier = x.PaymentToSupplier,
                    PaymentToSupplierCurrencyId = x.PaymentToSupplierCurrencyId,
                    SupplierInvoicePaidDate = x.SupplierInvoicePaidDate,
                    SupplierInvoicePaidMethodId = x.SupplierInvoicePaidMethodId,
                    ExtranetClientStatusId = x.ExtranetClientStatusId,
                    WebServiceClientStatusId = x.WebServiceClientStatusId,
                    CreatedDateTime = x.CreatedDateTime,
                    CreatedByEmployeeId = x.CreatedByEmployeeId,
                    LastModifiedDateTime = x.LastModifiedDateTime,
                    LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
                    DeletedDateTime = x.DeletedDateTime,
                    DeletedByEmployeeId = x.DeletedByEmployeeId,
                    DeletedFreeTextReason = x.DeletedFreeTextReason,
                    ChargeToClientAfterDiscountSurcharges = x.ChargeToClientAfterDiscountSurcharges,
                    TotalNumberOfReviewSegments = x.TotalNumberOfReviewSegments,
                    TotalNumberOfChangedReviewSegments = x.TotalNumberOfChangedReviewSegments,
                    PercentageOfChangedReviewSegments = x.PercentageOfChangedReviewSegments,
                    DownloadedByContactId = x.DownloadedByContactId,
                    AnticipatedFinalValueAmount = x.AnticipatedFinalValueAmount,
                    SupplierWordCountNew = x.SupplierWordCountNew,
                    SupplierWordCountFuzzyBand1 = x.SupplierWordCountFuzzyBand1,
                    SupplierWordCountFuzzyBand2 = x.SupplierWordCountFuzzyBand2,
                    SupplierWordCountFuzzyBand3 = x.SupplierWordCountFuzzyBand3,
                    SupplierWordCountFuzzyBand4 = x.SupplierWordCountFuzzyBand4,
                    SupplierWordCountExact = x.SupplierWordCountExact,
                    SupplierWordCountRepetitions = x.SupplierWordCountRepetitions,
                    SupplierWordCountPerfectMatches = x.SupplierWordCountPerfectMatches,
                    SupplierWordCountsTakenFromClient = x.SupplierWordCountsTakenFromClient,
                    ContextFieldsList = x.ContextFieldsList,
                    SupplierWordCountClientSpecific = x.SupplierWordCountClientSpecific,
                    SupplierResponsivenessRating = x.SupplierResponsivenessRating,
                    SupplierFollowingTheInstructionsRating = x.SupplierFollowingTheInstructionsRating,
                    SupplierAttitudeRating = x.SupplierAttitudeRating,
                    SupplierQualityOfWorkRating = x.SupplierQualityOfWorkRating,
                    QualityRatingNotGivenReason = x.QualityRatingNotGivenReason,
                    SupplierRatingsGivenDateTime = x.SupplierRatingsGivenDateTime,
                    SupplierRatingsGivenBy = x.SupplierRatingsGivenBy,
                    QualityRatingReminderDisabled = x.QualityRatingReminderDisabled,
                    NotifiedLinguistForApproachingDeadline = x.NotifiedLinguistForApproachingDeadline,
                    AnticipatedGrossMarginPercentage = x.AnticipatedGrossMarginPercentage,
                    MarginAfterDiscountSurcharges = x.MarginAfterDiscountSurcharges,
                    ClientCostCalculatedById = x.ClientCostCalculatedById,
                    ClientCostCalculatedByDateTime = x.ClientCostCalculatedByDateTime,
                    ClientCostCalculatedByPriceList = x.ClientCostCalculatedByPriceList,
                    SupplierCostCalculatedById = x.SupplierCostCalculatedById,
                    SupplierCostCalculatedByDateTime = x.SupplierCostCalculatedByDateTime,
                    SupplierCostCalculatedByPriceList = x.SupplierCostCalculatedByPriceList,
                    MinimumSupplierChargeApplied = x.MinimumSupplierChargeApplied,
                    LanguageServiceCategoryId = x.LanguageServiceCategoryId
                })
                .FirstOrDefaultAsync();

            if (result == null)
                return null;

            //Additional details
            result.AdditionalDetails = await jobOrderRepository.All().Where(x => x.Id == result.JobOrderId)
                .Join(contactRepository.All(), job => job.ContactId, contact => contact.Id, (job, contact) => new { job, contact })
                .Join(orgRepository.All(), a => a.contact.OrgId, org => org.Id, (a, org) => new { a, org })
                .Join(orgGroupRepository.All(), b => b.org.OrgGroupId, group => group.Id, (b, group) => new { b, group })
                .Select(x => new JobItemAdditionalDetails()
                {
                    OrgId = x.b.org.Id,
                    OrgName = x.b.org.OrgName,
                    ContactId = x.b.a.contact.Id,
                    ContactName = x.b.a.contact.Name,
                    ContactEmailAddress = x.b.a.contact.EmailAddress,
                    JobOrderId = x.b.a.job.Id,
                    JobOrderName = x.b.a.job.JobName,
                    ClientInvoiceId = x.b.a.job.ClientInvoiceId.GetValueOrDefault(),
                    OrderOnHold = x.b.a.job.OnHold.GetValueOrDefault(),
                    OrderOverallCompletedDateTime = x.b.a.job.OverallCompletedDateTime,
                    IsCLSJob = x.b.a.job.IsCls.GetValueOrDefault(),
                    TypeOfOrder = x.b.a.job.TypeOfOrder.GetValueOrDefault(),
                    OrderCurrencyId = x.b.a.job.ClientCurrencyId,
                    FuzzyBand1BottomPercentage = x.b.org.FuzzyBand1BottomPercentage,
                    FuzzyBand1TopPercentage = x.b.org.FuzzyBand1TopPercentage,
                    FuzzyBand2BottomPercentage = x.b.org.FuzzyBand2BottomPercentage,
                    FuzzyBand2TopPercentage = x.b.org.FuzzyBand2TopPercentage,
                    FuzzyBand3BottomPercentage = x.b.org.FuzzyBand3BottomPercentage,
                    FuzzyBand3TopPercentage = x.b.org.FuzzyBand3TopPercentage,
                    FuzzyBand4BottomPercentage = x.b.org.FuzzyBand4BottomPercentage,
                    FuzzyBand4TopPercentage = x.b.org.FuzzyBand4TopPercentage,
                    EarlyInvoiceDateTime = x.b.a.job.EarlyInvoiceDateTime,
                    OrgGroupId = x.group.Id,
                    OrgGroupName = x.group.Name,
                    IsOrgGroupDeleted = x.group.DeletedDate != null,
                    //OrgLogoImageBase64 = x.b.org.LogoImageBase64,
                    //OrgCustomerLogoImageBinary = x.b.org.CustomerLogoImageBinary,
                    OrgJobServerLocation = x.b.org.JobServerLocation,
                    OrgGroupOnlyAllowEncryptedSuppliers = x.group.OnlyAllowEncryptedSuppliers,
                    LionBoxArchivingStatus = x.b.a.job.LionBoxArchivingStatus,
                    ArchivedToAmazonS3dateTime = x.b.a.job.ArchivedToAmazonS3dateTime,
                    ArchivedDateTime = x.b.a.job.ArchivedDateTime,
                    JobOrderChannelId = x.b.a.job.JobOrderChannelId,
                    JobOrderDesignPlusFileId = x.b.a.job.DesignPlusFileId,
                    EarlyPaymentDiscount = x.b.org.EarlyPaymentDiscount
                }).FirstOrDefaultAsync();

            byte JobServerLocation = result.AdditionalDetails.OrgJobServerLocation ?? 0;

            var networkfolderpath = orderService.NetworkDirectoryPathForApp(result.AdditionalDetails.JobOrderId, result.AdditionalDetails.OrgId, ((byte)JobServerLocation), false, result.CreatedDateTime);

            if (networkfolderpath == "" && result.AdditionalDetails.LionBoxArchivingStatus == 2)
                result.AdditionalDetails.JobOrderNetworkPath = @"https://lion.app.box.com/folder/72215880016\";

            else if (networkfolderpath == "" && result.AdditionalDetails.LionBoxArchivingStatus == 1)
                result.AdditionalDetails.JobOrderNetworkPath = @"https://lion.app.box.com/folder/94421039140\";

            else if (networkfolderpath == "" && result.AdditionalDetails.ArchivedToAmazonS3dateTime != null)
                result.AdditionalDetails.JobOrderNetworkPath = "Archived to AWS – please contact IT Support for access";

            else if (networkfolderpath == "" && result.AdditionalDetails.ArchivedDateTime != null)
                result.AdditionalDetails.JobOrderNetworkPath = "Files no longer in production – please contact IT Support";

            else if (networkfolderpath != "")
                result.AdditionalDetails.JobOrderNetworkPath = networkfolderpath;

            if (result.AdditionalDetails.IsCLSJob == true || result.AdditionalDetails.TypeOfOrder == 3 || result.AdditionalDetails.TypeOfOrder == 4 || result.AdditionalDetails.TypeOfOrder == 5)
            {
                result.PGDDetails = await pgdRepository.All().Where(x => x.JobItemId == result.Id)
                    .Select(x => new PgdDetails()
                    {
                        Id = x.Id,
                        JobItemId = x.JobItemId,
                        Markets = x.Markets,
                        Service = x.Service,
                        AssetsOverview = x.AssetsOverview,
                        AirDate = x.AirDate,
                        CopydeckStored = x.CopydeckStored,
                        Votalent = x.Votalent,
                        BuyoutAgreementSigned = x.BuyoutAgreementSigned,
                        UsageType = x.UsageType,
                        UsageDuration = x.UsageDuration,
                        UsageStartDate = x.UsageStartDate,
                        UsageEndDate = x.UsageEndDate
                    }).FirstOrDefaultAsync();
            }

            // Margin calculations
            short orderCurrencyId = 0;

            var marginCalc = await jobOrderRepository.All().Where(x => x.Id == result.JobOrderId)
                .Join(invoiceRepository.All(), job => job.ClientInvoiceId, invoice => invoice.Id, (job, invoice) => new { job, invoice })
                .Select(x => new JobItemMarginCalculations()
                {
                    ClientInvoiceId = x.invoice.Id,
                    OrderCurrencyId = x.job.ClientCurrencyId,
                    ClientInvoiceFinalised = x.invoice.IsFinalised,
                    DateToGetRateFrom = x.invoice.FinalisedDateTime == null ? x.invoice.InvoiceDate.GetValueOrDefault() : x.invoice.FinalisedDateTime.GetValueOrDefault()
                }).FirstOrDefaultAsync();

            if (marginCalc == null)
                orderCurrencyId = await jobOrderRepository.All().Where(x => x.Id == result.JobOrderId).Select(x => x.ClientCurrencyId).FirstOrDefaultAsync();
            else
                orderCurrencyId = (short)marginCalc.OrderCurrencyId;


            if (marginCalc != null && marginCalc.ClientInvoiceId != 0 && marginCalc.ClientInvoiceFinalised == true && marginCalc.OrderCurrencyId != 4)
            {
                var exchangeRate = await tPExchangeService.GetHistoricalExchangeRate(4, ((short)marginCalc.OrderCurrencyId), marginCalc.DateToGetRateFrom);
                result.ChargeToClientInGbp = result.ChargeToClient.GetValueOrDefault() * (1 / exchangeRate.Rate);
            }
            else
            {
                try
                {
                    result.ChargeToClientInGbp = tPExchangeService.Convert(orderCurrencyId, 4, result.ChargeToClient.GetValueOrDefault());
                }
                catch (Exception ex)
                { }
            }

            if (marginCalc != null && marginCalc.ClientInvoiceId != 0 && marginCalc.ClientInvoiceFinalised == true && marginCalc.OrderCurrencyId != 4)
            {
                var exchangeRate = await tPExchangeService.GetHistoricalExchangeRate(4, ((short)marginCalc.OrderCurrencyId), marginCalc.DateToGetRateFrom);
                result.ChargeToClientInGbpAfterDiscountSurcharges = result.ChargeToClientAfterDiscountSurcharges.GetValueOrDefault() * (1 / exchangeRate.Rate);
                result.AnticipatedChargeToClientInGbpAfterDiscountSurcharges = result.AnticipatedFinalValueAmount.GetValueOrDefault() * (1 / exchangeRate.Rate);
            }
            else
            {
                try
                {
                    result.ChargeToClientInGbpAfterDiscountSurcharges = tPExchangeService.Convert(orderCurrencyId, 4, result.ChargeToClientAfterDiscountSurcharges.GetValueOrDefault());
                    result.AnticipatedChargeToClientInGbpAfterDiscountSurcharges = tPExchangeService.Convert(orderCurrencyId, 4, result.AnticipatedFinalValueAmount.GetValueOrDefault());
                }
                catch (Exception ex)
                { }
            }

            if (marginCalc != null && marginCalc.ClientInvoiceId != 0 && marginCalc.ClientInvoiceFinalised == true && marginCalc.OrderCurrencyId != 4 && result.PaymentToSupplierCurrencyId != 4 && result.PaymentToSupplierCurrencyId != null)
            {
                var exchangeRate = await tPExchangeService.GetHistoricalExchangeRate(4, result.PaymentToSupplierCurrencyId.GetValueOrDefault(), marginCalc.DateToGetRateFrom);
                result.PaymentToSupplierInGbp = result.PaymentToSupplier.GetValueOrDefault() * (1 / exchangeRate.Rate);
            }
            else
                try
                {
                    result.PaymentToSupplierInGbp = tPExchangeService.Convert(result.PaymentToSupplierCurrencyId.GetValueOrDefault(), 4, result.PaymentToSupplier.GetValueOrDefault());
                }
                catch (Exception)
                { }

            TimeSpan expectedMinutes = TimeSpan.FromMinutes(result.InterpretingExpectedDurationMinutes.GetValueOrDefault());
            TimeSpan actualMinutes = TimeSpan.FromMinutes(result.InterpretingActualMinutes);
            result.InterpretingExpectedHours = (int)expectedMinutes.TotalHours;
            result.InterpretingExpectedMinutes = expectedMinutes.Minutes;
            result.InterpretingActualHours = (int)actualMinutes.TotalHours;
            result.InterpretingActualMinutes = actualMinutes.Minutes;

            TimeSpan audio = TimeSpan.FromMinutes(result.AudioMinutes.GetValueOrDefault());
            TimeSpan work = TimeSpan.FromMinutes(result.WorkMinutes.GetValueOrDefault());
            result.AudioTimeHours = (int)audio.TotalHours;
            result.AudioTimeMinutes = audio.Minutes;
            result.WorkTimeHours = (int)work.TotalHours;
            result.WorkTimeMinutes = work.Minutes;

            return result;
        }

        public async Task<JobItemUpdateModel> Update(JobItemUpdateModel updateModel)
        {
            if (updateModel.Item.Id == 0)
                return null;

            var dbItem = await itemRepository.All().Where(x => x.Id == updateModel.Item.Id).FirstOrDefaultAsync();

            dbItem.LastModifiedByEmployeeId = (short)updateModel.LoggedInEmployeeId;
            dbItem.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();

            if (updateModel.SectionToUpdate == updateModel.keyInfoSection)
            {
                dbItem.IsVisibleToClient = updateModel.Item.IsVisibleToClient;
                dbItem.LanguageServiceId = updateModel.Item.LanguageServiceId;
                dbItem.SourceLanguageIanacode = updateModel.Item.SourceLanguageIanacode;
                dbItem.TargetLanguageIanacode = updateModel.Item.TargetLanguageIanacode;
                dbItem.CustomerSpecificField = updateModel.Item.CustomerSpecificField;
                dbItem.WordCountNew = updateModel.Item.WordCountNew;
                dbItem.WordCountExact = updateModel.Item.WordCountExact;
                dbItem.WordCountRepetitions = updateModel.Item.WordCountRepetitions;
                dbItem.WordCountPerfectMatches = updateModel.Item.WordCountPerfectMatches;
                dbItem.WordCountFuzzyBand1 = updateModel.Item.WordCountFuzzyBand1;
                dbItem.WordCountFuzzyBand2 = updateModel.Item.WordCountFuzzyBand2;
                dbItem.WordCountFuzzyBand3 = updateModel.Item.WordCountFuzzyBand3;
                dbItem.WordCountFuzzyBand4 = updateModel.Item.WordCountFuzzyBand4;
                dbItem.Pages = updateModel.Item.Pages;
                dbItem.Characters = updateModel.Item.Characters;
                dbItem.Documents = updateModel.Item.Documents;
                dbItem.LanguageServiceCategoryId = updateModel.Item.LanguageServiceCategoryId;

                var expectedMinutes = (int)TimeSpan.FromHours(updateModel.Item.InterpretingExpectedHours).TotalMinutes + (int)TimeSpan.FromMinutes(updateModel.Item.InterpretingExpectedMinutes).TotalMinutes;
                var actualMinutes = (int)TimeSpan.FromHours(updateModel.Item.InterpretingActualHours).TotalMinutes + (int)TimeSpan.FromMinutes(updateModel.Item.InterpretingActualMinutes).TotalMinutes;

                dbItem.InterpretingExpectedDurationMinutes = expectedMinutes;
                dbItem.InterpretingActualDurationMinutes = actualMinutes;

                dbItem.InterpretingLocationOrgName = updateModel.Item.InterpretingLocationOrgName;
                dbItem.InterpretingLocationAddress1 = updateModel.Item.InterpretingLocationAddress1;
                dbItem.InterpretingLocationAddress2 = updateModel.Item.InterpretingLocationAddress2;
                dbItem.InterpretingLocationAddress3 = updateModel.Item.InterpretingLocationAddress3;
                dbItem.InterpretingLocationAddress4 = updateModel.Item.InterpretingLocationAddress4;
                dbItem.InterpretingLocationCountyOrState = updateModel.Item.InterpretingLocationCountyOrState;
                dbItem.InterpretingLocationPostcodeOrZip = updateModel.Item.InterpretingLocationPostcodeOrZip;
                dbItem.InterpretingLocationCountryId = updateModel.Item.InterpretingLocationCountryId;

                var audioMinutes = (int)TimeSpan.FromHours(updateModel.Item.AudioTimeHours).TotalMinutes + (int)TimeSpan.FromMinutes(updateModel.Item.AudioTimeMinutes).TotalMinutes;
                var wordMinutes = (int)TimeSpan.FromHours(updateModel.Item.WorkTimeHours).TotalMinutes + (int)TimeSpan.FromMinutes(updateModel.Item.WorkTimeMinutes).TotalMinutes;

                dbItem.AudioMinutes = audioMinutes;
                dbItem.WorkMinutes = wordMinutes;

                dbItem.OurCompletionDeadline = updateModel.Item.OurCompletionDeadline;

                if (updateModel.Item.IsCompleted == true)
                {
                    dbItem.WeCompletedItemDateTime = GeneralUtils.GetCurrentUKTime();
                }
                else
                {
                    dbItem.WeCompletedItemDateTime = null;
                }
            }
            else if (updateModel.SectionToUpdate == updateModel.supplierInfoSection)
            {
                dbItem.LinguisticSupplierOrClientReviewerId = updateModel.Item.LinguisticSupplierOrClientReviewerId;
                dbItem.DescriptionForSupplierOnly = updateModel.Item.DescriptionForSupplierOnly;
                dbItem.TranslationMemoryRequired = updateModel.Item.TranslationMemoryRequired;
                dbItem.SupplierWordCountNew = updateModel.Item.SupplierWordCountNew;
                dbItem.SupplierWordCountExact = updateModel.Item.SupplierWordCountExact;
                dbItem.SupplierWordCountRepetitions = updateModel.Item.SupplierWordCountRepetitions;
                dbItem.SupplierWordCountPerfectMatches = updateModel.Item.SupplierWordCountPerfectMatches;
                dbItem.SupplierWordCountFuzzyBand1 = updateModel.Item.SupplierWordCountFuzzyBand1;
                dbItem.SupplierWordCountFuzzyBand2 = updateModel.Item.SupplierWordCountFuzzyBand2;
                dbItem.SupplierWordCountFuzzyBand3 = updateModel.Item.SupplierWordCountFuzzyBand3;
                dbItem.SupplierWordCountFuzzyBand4 = updateModel.Item.SupplierWordCountFuzzyBand4;
                dbItem.SupplierSentWorkDateTime = updateModel.Item.SupplierSentWorkDateTime;
                dbItem.SupplierAcceptedWorkDateTime = updateModel.Item.SupplierAcceptedWorkDateTime;
                dbItem.SupplierCompletionDeadline = updateModel.Item.SupplierCompletionDeadline;
                dbItem.SupplierCompletedItemDateTime = updateModel.Item.SupplierCompletedItemDateTime;
                dbItem.PaymentToSupplierCurrencyId = updateModel.Item.PaymentToSupplierCurrencyId;
                dbItem.PaymentToSupplier = updateModel.Item.PaymentToSupplier;
                dbItem.MinimumSupplierChargeApplied = updateModel.Item.MinimumSupplierChargeApplied;
                dbItem.SupplierInvoicePaidDate = updateModel.Item.SupplierInvoicePaidDate;
                dbItem.SupplierInvoicePaidMethodId = updateModel.Item.SupplierInvoicePaidMethodId;
                dbItem.SupplierIsClientReviewer = updateModel.Item.SupplierIsClientReviewer;
            }
            else if (updateModel.SectionToUpdate == updateModel.profitabilitySection)
            {
                dbItem.ChargeToClient = updateModel.Item.ChargeToClient;
            }
            else //CLS
            {

            }

            await itemRepository.SaveChangesAsync();

            return updateModel;
        }

        public async Task<JobItem> UpdateJobItemKeyInformation(int jobitemid, DateTime Deadline, decimal ChargeToClient, decimal PaymentToSupplier, bool MinSupplierChargeApplied, int WorkMinutes, int NewClientWordCount, int NewSupplierWordCount, bool Visible, short suppliercurrencyID)
        {
            var jobitem = await GetById(jobitemid);

            // maybe add security check later here and autoMapper
            if (Deadline != DateTime.MinValue)
            {
                jobitem.SupplierCompletionDeadline = Deadline;
            }

            //jobitem.SupplierIsClientReviewer = SupplierOrContact;
            //if (SupplierOrReviewerID != 0)
            //{
            //    jobitem.LinguisticSupplierOrClientReviewerId = SupplierOrReviewerID;
            //}            
            jobitem.ChargeToClient = ChargeToClient;
            jobitem.PaymentToSupplier = PaymentToSupplier;
            jobitem.MinimumSupplierChargeApplied = MinSupplierChargeApplied;
            jobitem.WorkMinutes = WorkMinutes;
            jobitem.WordCountNew = NewClientWordCount;
            jobitem.SupplierWordCountNew = NewSupplierWordCount;
            jobitem.IsVisibleToClient = Visible;

            if (suppliercurrencyID > 0 && jobitem.PaymentToSupplierCurrencyId == null)
            {
                jobitem.PaymentToSupplierCurrencyId = suppliercurrencyID;
            }
            //if (markuncompleted == true)
            //{
            //    jobitem.OverallCompletedDateTime = (DateTime?)null;
            //}

            //if (markcompleted == true)
            //{
            //    jobitem.OverallCompletedDateTime = GeneralUtils.GetCurrentUKTime();
            //}

            itemRepository.Update(jobitem);
            await itemRepository.SaveChangesAsync();

            return jobitem;
        }

        public async Task<JobItem> UpdateJobItemClientDeadline(int jobitemid, DateTime Deadline)
        {
            var jobitem = await GetById(jobitemid);

            // maybe add security check later here and autoMapper
            if (Deadline != DateTime.MinValue)
            {
                jobitem.OurCompletionDeadline = Deadline;
            }

            itemRepository.Update(jobitem);
            await itemRepository.SaveChangesAsync();

            return jobitem;
        }

        public async Task<JobItem> CreateItem(int OrderID, bool IsVisibleToClient, byte LanguageServiceID, string SourceLangIANACode,
                                        string TargetLangIANACode, int WordCountNew, int WordCountFuzzyBand1,
                                        int WordCountFuzzyBand2, int WordCountFuzzyBand3, int WordCountFuzzyBand4,
                                        int WordCountExact, int WordCountRepetitions, int WordCountPerfectMatches,
                                        Enumerations.TranslationMemoryRequiredValues TranslationMemoryRequiredValues,
                                        int Pages, int Characters, int Documents, int InterpretingExpectedDurationMinutes,
                                        string InterpretingLocationOrgName, string InterpretingLocationAddress1,
                                        string InterpretingLocationAddress2, string InterpretingLocationAddress3,
                                        string InterpretingLocationAddress4, string InterpretingLocationCountyOrState,
                                        string InterpretingLocationPostcodeOrZip, byte? InterpretingLocationCountryID,
                                        int AudioMinutes, int WorkMinutes, DateTime SupplierSentWorkDateTime,
                                        DateTime SupplierCompletionDeadline, DateTime OurCompletionDeadline,
                                        string DescriptionForSupplierOnly, string FileName, bool SupplierIsClientReviewer,
                                        int? LinguisticSupplierOrClientReviewerID, decimal ChargeToClient,
                                        decimal PaymentToSupplier, byte? PaymentToSupplierCurrencyID, DateTime SupplierInvoicePaidDate,
                                        byte SupplierInvoicePaidMethodID, short CreatedByEmployeeID, int SupplierWordCountNew,
                                        int SupplierWordCountFuzzyBand1, int SupplierWordCountFuzzyBand2,
                                        int SupplierWordCountFuzzyBand3, int SupplierWordCountFuzzyBand4, int SupplierWordCountExact,
                                        int SupplierWordCountRepetitions, int SupplierWordCountPerfectMatches,
                                        bool SupplierWordCountsTakenFromClient, string CustomerSpecificField = "",
                                        string ContextFieldsList = "", int WordCountClientSpecific = 0,
                                        int SupplierWordCountClientSpecific = 0, bool FromExternalServer = false,
                                        int ClientCostCalculatedByID = 0, DateTime? ClientCostCalculatedByDateTime = null,
                                        bool ClientCostCalculatedByPriceList = false, int SupplierCostCalculatedByID = 0,
                                        DateTime? SupplierCostCalculatedByDateTime = null,
                                        bool MinimumSupplierChargeApplied = false,
                                        bool SupplierCostCalculatedByPriceList = false, byte? LanguageServiceCategoryId = 0)
        {
            bool orderExists = await jobOrderService.OrderExists(OrderID);
            int NewJobItemID;
            JobItem NewJobItem = null;

            if (orderExists == false)
            {
                throw new Exception("The order ID, " + OrderID.ToString() + ", does not exist in the database. A job item must be created against a valid order.");
            }
            else
            {

                if (TargetLangIANACode.Contains(",") == true)
                {
                    string[] allTargetLangs = TargetLangIANACode.Split(",");

                    int counter = 0;
                    foreach (string targetLang in allTargetLangs)
                    {
                        //Input strings should have been limited in the interface but
                        //limit as a precaution and trim
                        if (InterpretingLocationOrgName != null && InterpretingLocationOrgName.Length > 200) { InterpretingLocationOrgName = InterpretingLocationOrgName.Trim().Substring(0, 200); }
                        if (InterpretingLocationAddress1 != null && InterpretingLocationAddress1.Length > 100) { InterpretingLocationAddress1 = InterpretingLocationAddress1.Trim().Substring(0, 100); }
                        if (InterpretingLocationAddress2 != null && InterpretingLocationAddress2.Length > 100) { InterpretingLocationAddress2 = InterpretingLocationAddress2.Trim().Substring(0, 100); }
                        if (InterpretingLocationAddress3 != null && InterpretingLocationAddress3.Length > 100) { InterpretingLocationAddress3 = InterpretingLocationAddress3.Trim().Substring(0, 100); }
                        if (InterpretingLocationAddress4 != null && InterpretingLocationAddress4.Length > 100) { InterpretingLocationAddress4 = InterpretingLocationAddress4.Trim().Substring(0, 100); }
                        if (InterpretingLocationCountyOrState != null && InterpretingLocationCountyOrState.Length > 100) { InterpretingLocationCountyOrState = InterpretingLocationCountyOrState.Trim().Substring(0, 100); }
                        if (InterpretingLocationPostcodeOrZip != null && InterpretingLocationPostcodeOrZip.Length > 20) { InterpretingLocationPostcodeOrZip = InterpretingLocationPostcodeOrZip.Trim().Substring(0, 20); }

                        if (ClientCostCalculatedByDateTime == null)
                        {
                            ClientCostCalculatedByDateTime = DateTime.MinValue;
                        }

                        if (SupplierCostCalculatedByDateTime == null)
                        {
                            SupplierCostCalculatedByDateTime = DateTime.MinValue;
                        }

                        if (LinguisticSupplierOrClientReviewerID <= 0)
                        {
                            LinguisticSupplierOrClientReviewerID = null;
                        }

                        if (PaymentToSupplierCurrencyID == 0)
                        {
                            PaymentToSupplierCurrencyID = null;
                        }

                        if (InterpretingLocationCountryID == 0)
                        {
                            InterpretingLocationCountryID = null;
                        }
                        if (LanguageServiceCategoryId == 0)
                        {
                            LanguageServiceCategoryId = null;
                        }
                        //HTML-encode contents of supplier description (currently a manual fix
                        //rather than using a derivation of Server.HtmlEncode)
                        if (DescriptionForSupplierOnly != null)
                        {
                            DescriptionForSupplierOnly = DescriptionForSupplierOnly.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                        }
                        NewJobItem = new JobItem()
                        {
                            JobOrderId = OrderID,
                            IsVisibleToClient = IsVisibleToClient,
                            LanguageServiceId = LanguageServiceID,
                            SourceLanguageIanacode = SourceLangIANACode,
                            TargetLanguageIanacode = TargetLangIANACode,
                            WordCountNew = WordCountNew,
                            WordCountFuzzyBand1 = WordCountFuzzyBand1,
                            WordCountFuzzyBand2 = WordCountFuzzyBand2,
                            WordCountFuzzyBand3 = WordCountFuzzyBand3,
                            WordCountFuzzyBand4 = WordCountFuzzyBand4,
                            WordCountExact = WordCountExact,
                            WordCountRepetitions = WordCountRepetitions,
                            WordCountPerfectMatches = WordCountPerfectMatches,
                            TranslationMemoryRequired = (byte)TranslationMemoryRequiredValues,
                            Pages = Pages,
                            Characters = Characters,
                            Documents = Documents,
                            InterpretingExpectedDurationMinutes = InterpretingExpectedDurationMinutes,
                            InterpretingLocationOrgName = InterpretingLocationOrgName,
                            InterpretingLocationAddress1 = InterpretingLocationAddress1,
                            InterpretingLocationAddress2 = InterpretingLocationAddress2,
                            InterpretingLocationAddress3 = InterpretingLocationAddress3,
                            InterpretingLocationAddress4 = InterpretingLocationAddress4,
                            InterpretingLocationCountyOrState = InterpretingLocationCountyOrState,
                            InterpretingLocationPostcodeOrZip = InterpretingLocationPostcodeOrZip,
                            InterpretingLocationCountryId = InterpretingLocationCountryID,
                            AudioMinutes = AudioMinutes,
                            WorkMinutes = WorkMinutes,
                            SupplierSentWorkDateTime = SupplierSentWorkDateTime,
                            OurCompletionDeadline = OurCompletionDeadline,
                            DescriptionForSupplierOnly = DescriptionForSupplierOnly,
                            SupplierIsClientReviewer = SupplierIsClientReviewer,
                            LinguisticSupplierOrClientReviewerId = LinguisticSupplierOrClientReviewerID,
                            ChargeToClient = ChargeToClient,
                            PaymentToSupplier = PaymentToSupplier,
                            PaymentToSupplierCurrencyId = PaymentToSupplierCurrencyID,
                            SupplierInvoicePaidDate = SupplierInvoicePaidDate,
                            SupplierInvoicePaidMethodId = SupplierInvoicePaidMethodID,
                            CreatedByEmployeeId = CreatedByEmployeeID,
                            CreatedDateTime = timeZonesService.GetCurrentGMT(),
                            SupplierWordCountNew = SupplierWordCountNew,
                            SupplierWordCountFuzzyBand1 = SupplierWordCountFuzzyBand1,
                            SupplierWordCountFuzzyBand2 = SupplierWordCountFuzzyBand2,
                            SupplierWordCountFuzzyBand3 = SupplierWordCountFuzzyBand3,
                            SupplierWordCountFuzzyBand4 = SupplierWordCountFuzzyBand4,
                            SupplierWordCountExact = SupplierWordCountExact,
                            SupplierWordCountRepetitions = SupplierWordCountRepetitions,
                            SupplierWordCountPerfectMatches = SupplierWordCountPerfectMatches,
                            SupplierWordCountsTakenFromClient = SupplierWordCountsTakenFromClient,
                            CustomerSpecificField = CustomerSpecificField,
                            ContextFieldsList = ContextFieldsList,
                            WordCountClientSpecific = WordCountClientSpecific,
                            SupplierWordCountClientSpecific = SupplierWordCountClientSpecific,
                            ClientCostCalculatedByPriceList = ClientCostCalculatedByPriceList,
                            ClientCostCalculatedByDateTime = ClientCostCalculatedByDateTime,
                            ClientCostCalculatedById = ClientCostCalculatedByID,
                            SupplierCostCalculatedById = SupplierCostCalculatedByID,
                            SupplierCostCalculatedByDateTime = SupplierCostCalculatedByDateTime,
                            SupplierCostCalculatedByPriceList = SupplierCostCalculatedByPriceList,
                            MinimumSupplierChargeApplied = MinimumSupplierChargeApplied,
                            LanguageServiceCategoryId = LanguageServiceCategoryId

                        };

                        await itemRepository.AddAsync(NewJobItem);
                        await itemRepository.SaveChangesAsync();

                        counter++;

                        NewJobItemID = NewJobItem.Id;

                        if (counter == allTargetLangs.Length)
                        {
                            return NewJobItem;
                        }

                    }
                }
                else
                {
                    //Input strings should have been limited in the interface but
                    //limit as a precaution and trim
                    if (InterpretingLocationOrgName != null && InterpretingLocationOrgName.Length > 200) { InterpretingLocationOrgName = InterpretingLocationOrgName.Trim().Substring(0, 200); }
                    if (InterpretingLocationAddress1 != null && InterpretingLocationAddress1.Length > 100) { InterpretingLocationAddress1 = InterpretingLocationAddress1.Trim().Substring(0, 100); }
                    if (InterpretingLocationAddress2 != null && InterpretingLocationAddress2.Length > 100) { InterpretingLocationAddress2 = InterpretingLocationAddress2.Trim().Substring(0, 100); }
                    if (InterpretingLocationAddress3 != null && InterpretingLocationAddress3.Length > 100) { InterpretingLocationAddress3 = InterpretingLocationAddress3.Trim().Substring(0, 100); }
                    if (InterpretingLocationAddress4 != null && InterpretingLocationAddress4.Length > 100) { InterpretingLocationAddress4 = InterpretingLocationAddress4.Trim().Substring(0, 100); }
                    if (InterpretingLocationCountyOrState != null && InterpretingLocationCountyOrState.Length > 100) { InterpretingLocationCountyOrState = InterpretingLocationCountyOrState.Trim().Substring(0, 100); }
                    if (InterpretingLocationPostcodeOrZip != null && InterpretingLocationPostcodeOrZip.Length > 20) { InterpretingLocationPostcodeOrZip = InterpretingLocationPostcodeOrZip.Trim().Substring(0, 20); }

                    //if (ClientCostCalculatedByDateTime == null)
                    //{
                    //    ClientCostCalculatedByDateTime = DateTime.MinValue;
                    //}

                    //if (SupplierCostCalculatedByDateTime == null)
                    //{
                    //    SupplierCostCalculatedByDateTime = DateTime.MinValue;
                    //}

                    if (LinguisticSupplierOrClientReviewerID <= 0)
                    {
                        LinguisticSupplierOrClientReviewerID = null;
                    }

                    if (PaymentToSupplierCurrencyID == 0)
                    {
                        PaymentToSupplierCurrencyID = null;
                    }

                    if (InterpretingLocationCountryID == 0)
                    {
                        InterpretingLocationCountryID = null;
                    }
                    if (LanguageServiceCategoryId == 0)
                    {
                        LanguageServiceCategoryId = null;
                    }
                    //HTML-encode contents of supplier description (currently a manual fix
                    //rather than using a derivation of Server.HtmlEncode)
                    if (DescriptionForSupplierOnly != null)
                    {
                        DescriptionForSupplierOnly = DescriptionForSupplierOnly.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                    }

                    NewJobItem = new JobItem()
                    {
                        JobOrderId = OrderID,
                        IsVisibleToClient = IsVisibleToClient,
                        LanguageServiceId = LanguageServiceID,
                        SourceLanguageIanacode = SourceLangIANACode,
                        TargetLanguageIanacode = TargetLangIANACode,
                        WordCountNew = WordCountNew,
                        WordCountFuzzyBand1 = WordCountFuzzyBand1,
                        WordCountFuzzyBand2 = WordCountFuzzyBand2,
                        WordCountFuzzyBand3 = WordCountFuzzyBand3,
                        WordCountFuzzyBand4 = WordCountFuzzyBand4,
                        WordCountExact = WordCountExact,
                        WordCountRepetitions = WordCountRepetitions,
                        WordCountPerfectMatches = WordCountPerfectMatches,
                        TranslationMemoryRequired = (byte)TranslationMemoryRequiredValues,
                        Pages = Pages,
                        Characters = Characters,
                        Documents = Documents,
                        InterpretingExpectedDurationMinutes = InterpretingExpectedDurationMinutes,
                        InterpretingLocationOrgName = InterpretingLocationOrgName,
                        InterpretingLocationAddress1 = InterpretingLocationAddress1,
                        InterpretingLocationAddress2 = InterpretingLocationAddress2,
                        InterpretingLocationAddress3 = InterpretingLocationAddress3,
                        InterpretingLocationAddress4 = InterpretingLocationAddress4,
                        InterpretingLocationCountyOrState = InterpretingLocationCountyOrState,
                        InterpretingLocationPostcodeOrZip = InterpretingLocationPostcodeOrZip,
                        InterpretingLocationCountryId = InterpretingLocationCountryID,
                        AudioMinutes = AudioMinutes,
                        WorkMinutes = WorkMinutes,
                        OurCompletionDeadline = OurCompletionDeadline,
                        DescriptionForSupplierOnly = DescriptionForSupplierOnly,
                        SupplierIsClientReviewer = SupplierIsClientReviewer,
                        LinguisticSupplierOrClientReviewerId = LinguisticSupplierOrClientReviewerID,
                        ChargeToClient = ChargeToClient,
                        PaymentToSupplier = PaymentToSupplier,
                        PaymentToSupplierCurrencyId = PaymentToSupplierCurrencyID,
                        SupplierInvoicePaidMethodId = SupplierInvoicePaidMethodID,
                        CreatedByEmployeeId = CreatedByEmployeeID,
                        CreatedDateTime = timeZonesService.GetCurrentGMT(),
                        SupplierWordCountNew = SupplierWordCountNew,
                        SupplierWordCountFuzzyBand1 = SupplierWordCountFuzzyBand1,
                        SupplierWordCountFuzzyBand2 = SupplierWordCountFuzzyBand2,
                        SupplierWordCountFuzzyBand3 = SupplierWordCountFuzzyBand3,
                        SupplierWordCountFuzzyBand4 = SupplierWordCountFuzzyBand4,
                        SupplierWordCountExact = SupplierWordCountExact,
                        SupplierWordCountRepetitions = SupplierWordCountRepetitions,
                        SupplierWordCountPerfectMatches = SupplierWordCountPerfectMatches,
                        SupplierWordCountsTakenFromClient = SupplierWordCountsTakenFromClient,
                        CustomerSpecificField = CustomerSpecificField,
                        ContextFieldsList = ContextFieldsList,
                        WordCountClientSpecific = WordCountClientSpecific,
                        SupplierWordCountClientSpecific = SupplierWordCountClientSpecific,
                        ClientCostCalculatedByPriceList = ClientCostCalculatedByPriceList,
                        ClientCostCalculatedByDateTime = ClientCostCalculatedByDateTime,
                        ClientCostCalculatedById = ClientCostCalculatedByID,
                        SupplierCostCalculatedById = SupplierCostCalculatedByID,
                        SupplierCostCalculatedByDateTime = SupplierCostCalculatedByDateTime,
                        SupplierCostCalculatedByPriceList = SupplierCostCalculatedByPriceList,
                        MinimumSupplierChargeApplied = MinimumSupplierChargeApplied,
                        LanguageServiceCategoryId = LanguageServiceCategoryId


                    };

                    await itemRepository.AddAsync(NewJobItem);
                    await itemRepository.SaveChangesAsync();

                    NewJobItemID = NewJobItem.Id;

                }

            }
            return NewJobItem;
        }

        public async System.Threading.Tasks.Task DeliveryFolderCheck(string OldFolderNumber, string NewFolderNumber,
                                    string InternalJobDrivePath, int jobitemid)
        {

            string InternalJobPath = InternalJobDrivePath;
            var jobItem = await itemRepository.All().Where(o => o.Id == jobitemid).FirstOrDefaultAsync();
            var jobOrder = await jobOrderRepository.All().Where(o => o.Id == jobItem.JobOrderId).FirstOrDefaultAsync();
            var orderContact = await contactRepository.All().Where(o => o.Id == jobOrder.ContactId && o.DeletedDate == null).FirstOrDefaultAsync();
            var org = await orgRepository.All().Where(o => o.Id == orderContact.OrgId).FirstOrDefaultAsync();
            var Languages = await cachedService.GetAllLanguagesCached();
            var sourcelang = Languages.Where(o => o.StringValue == jobItem.SourceLanguageIanacode).ToList().FirstOrDefault();
            var targetlang = Languages.Where(o => o.StringValue == jobItem.TargetLanguageIanacode).ToList().FirstOrDefault();
            //Checking if the delivery folder exists with old folder number, and renames the folder with new folder number
            if (Directory.Exists(InternalJobPath + @"\" + OldFolderNumber + " Delivery") == true && Directory.Exists(InternalJobPath + @"\" + NewFolderNumber + " Delivery") == false)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(InternalJobPath + @"\" + OldFolderNumber + " Delivery", NewFolderNumber + " Delivery");
                }

                catch
                {
                }

            }

            //If delivery folder with new number is not already created, then create this folder
            if (Directory.Exists(InternalJobPath + @"\" + NewFolderNumber + " Delivery") == false)
            {

                if (Directory.Exists(InternalJobPath + @"\07 Delivery") == false && Directory.Exists(InternalJobPath + @"\08 Delivery") == false && Directory.Exists(InternalJobPath + @"\10 Delivery") == false)
                {
                    Directory.CreateDirectory(InternalJobPath + @"\" + NewFolderNumber + " Delivery");
                }
                else
                {
                    if (Directory.Exists(InternalJobPath + @"\03 Transcreation") == true)
                    {
                        if (Directory.Exists(InternalJobPath + @"\07 Delivery") == true)
                        {
                            NewFolderNumber = "07";
                        }
                        else if (Directory.Exists(InternalJobPath + @"\08 Delivery") == true)
                        {
                            NewFolderNumber = "08";
                        }
                        else if (Directory.Exists(InternalJobPath + @"\10 Delivery") == true)
                        {
                            NewFolderNumber = "10";
                        }
                    }
                }
            }


            //Checking if the subfolders under Delivery folder have same id as current job item id
            DirectoryInfo DeliveryDirectoryInfo = new DirectoryInfo(InternalJobPath + @"\" + NewFolderNumber + " Delivery");
            foreach (DirectoryInfo DeliverySubfolder in DeliveryDirectoryInfo.GetDirectories())
            {
                if (DeliverySubfolder.Name.Split(" -")[0] == jobitemid.ToString() && DeliverySubfolder.Name != jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name))
                {
                    try
                    {
                        string PathOfDirectoryToRename = InternalJobPath + @"\" + NewFolderNumber + @" Delivery\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                        if (PathOfDirectoryToRename.Length < 248)
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(InternalJobPath + @"\" + NewFolderNumber + @" Delivery\" + DeliverySubfolder.Name, jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name));
                        }
                        else
                        {
                            string NewFolderName = jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                            string ParentFolderPathOfNewFolder = InternalJobPath + @"\" + NewFolderNumber + @" Delivery\";
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(ParentFolderPathOfNewFolder + DeliverySubfolder.Name, NewFolderName.Substring(0, 247 - ParentFolderPathOfNewFolder.Length));
                        }
                    }
                    catch
                    {
                    }
                }
            }

            //If no folder with same id is found, then create a new subfolder for current job item
            if (Directory.Exists(InternalJobPath + @"\" + NewFolderNumber + @" Delivery\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name)) == false)
            {
                try
                {
                    string JobItemPath = InternalJobPath + @"\" + NewFolderNumber + @" Delivery\" + jobitemid + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);

                    //if the length of the folder name is less than limit
                    if (JobItemPath.Length < 248)
                    {
                        Directory.CreateDirectory(JobItemPath);
                    }
                    else
                    {
                        //if the length of the folder name is exceeds the limit, then just name with first 247 characters 
                        Directory.CreateDirectory(JobItemPath.Substring(0, 247));
                    }
                }
                catch { }
            }
        }
        public async System.Threading.Tasks.Task CreateClientReviewSubfolders(string ClientReviewFolderNumber, string InternalDriveJobPath, int jobitemid)
        {

            string InternalJobPath = InternalDriveJobPath;
            var jobItem = await itemRepository.All().Where(o => o.Id == jobitemid).FirstOrDefaultAsync();
            var jobOrder = await jobOrderRepository.All().Where(o => o.Id == jobItem.JobOrderId).FirstOrDefaultAsync();
            var orderContact = await contactRepository.All().Where(o => o.Id == jobOrder.ContactId && o.DeletedDate == null).FirstOrDefaultAsync();
            var org = await orgRepository.All().Where(o => o.Id == orderContact.OrgId).FirstOrDefaultAsync();
            var Languages = await cachedService.GetAllLanguagesCached();
            var sourcelang = Languages.Where(o => o.StringValue == jobItem.SourceLanguageIanacode).ToList().FirstOrDefault();
            var targetlang = Languages.Where(o => o.StringValue == jobItem.TargetLanguageIanacode).ToList().FirstOrDefault();
            //Checking if the subfolders under 'ClientReview / 01 To' folder have same id as current job item id
            DirectoryInfo ClientReviewToDirectoryInfo = new DirectoryInfo(InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\01 To");
            foreach (DirectoryInfo ClientReviewSubfolder in ClientReviewToDirectoryInfo.GetDirectories())
            {
                if (ClientReviewSubfolder.Name.Split(" -")[0] == jobitemid.ToString() && ClientReviewSubfolder.Name != jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name))
                {
                    try
                    {
                        string ItemFolderToBeRenamedPath = InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\01 To\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                        if (ItemFolderToBeRenamedPath.Length < 248)
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\01 To\" + ClientReviewSubfolder.Name, jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name));
                        }
                        else
                        {
                            string NewFolderName = jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                            string ParentFolderToItemFolder = InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\01 To\";
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(ParentFolderToItemFolder + ClientReviewSubfolder.Name, NewFolderName.Substring(0, 247 - ParentFolderToItemFolder.Length));
                        }
                    }
                    catch
                    {
                    }
                }
            }

            //If no folder with same id is found, then create a new subfolder for current job item
            if (Directory.Exists(InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\01 To\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name)) == false)
            {
                try
                {
                    string ClientReviewItemFolder = InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\01 To\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                    if (ClientReviewItemFolder.Length < 248)
                    {
                        Directory.CreateDirectory(ClientReviewItemFolder);
                    }
                    else
                    {
                        Directory.CreateDirectory(ClientReviewItemFolder.Substring(0, 247));
                    }
                }
                catch
                { }
            }


            //Checking if the subfolders under 'ClientReview / 02 From' folder have same id as current job item id
            DirectoryInfo ClientReviewFromDirectoryInfo = new DirectoryInfo(InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\02 From");
            foreach (DirectoryInfo ClientReviewSubfolder in ClientReviewFromDirectoryInfo.GetDirectories())
            {
                if (ClientReviewSubfolder.Name.Split(" -")[0] == jobitemid.ToString()
                    && ClientReviewSubfolder.Name != jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name))
                {
                    try
                    {
                        string ItemFolderToBeRenamedPath = InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\02 From\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                        if (ItemFolderToBeRenamedPath.Length < 248)
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\02 From\" + ClientReviewSubfolder.Name, jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name));
                        }
                        else
                        {
                            string NewFolderName = jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                            string ParentFolderToItemFolder = InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\02 From\";
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(ParentFolderToItemFolder + ClientReviewSubfolder.Name, NewFolderName.Substring(0, 247 - ParentFolderToItemFolder.Length));
                        }
                    }
                    catch
                    { }
                }
            }

            //If no folder with same id is found, then create a new subfolder for current job item
            if (Directory.Exists(InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\02 From\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name)) == false)
            {
                string ClientReviewItemFolder = InternalJobPath + @"\" + ClientReviewFolderNumber + @" Client review\02 From\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                if (ClientReviewItemFolder.Length < 248)
                {
                    Directory.CreateDirectory(ClientReviewItemFolder);
                }
                else
                {
                    Directory.CreateDirectory(ClientReviewItemFolder.Substring(0, 247));
                }
            }
        }
        public async System.Threading.Tasks.Task CreateTranslationSubfolders(string InternalDriveJobPath, int jobitemid)
        {
            string InternalJobPath = InternalDriveJobPath;
            var jobItem = await itemRepository.All().Where(o => o.Id == jobitemid).FirstOrDefaultAsync();
            var jobOrder = await jobOrderRepository.All().Where(o => o.Id == jobItem.JobOrderId).FirstOrDefaultAsync();
            var orderContact = await contactRepository.All().Where(o => o.Id == jobOrder.ContactId && o.DeletedDate == null).FirstOrDefaultAsync();
            var org = await orgRepository.All().Where(o => o.Id == orderContact.OrgId).FirstOrDefaultAsync();
            var Languages = await cachedService.GetAllLanguagesCached();
            var sourcelang = Languages.Where(o => o.StringValue == jobItem.SourceLanguageIanacode).ToList().FirstOrDefault();
            var targetlang = Languages.Where(o => o.StringValue == jobItem.TargetLanguageIanacode).ToList().FirstOrDefault();

            //Checking if the subfolders under 'Translation / 01 To' folder have same id as current job item id
            DirectoryInfo TranslationToDirectoryInfo = new DirectoryInfo(InternalJobPath + @"\02 Translation\01 To");
            foreach (DirectoryInfo TranslationSubfolder in TranslationToDirectoryInfo.GetDirectories())
            {
                if (TranslationSubfolder.Name.Split(" -")[0] == jobitemid.ToString()
                    && TranslationSubfolder.Name != jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name))
                {
                    try
                    {
                        string ItemFolderToBeRenamedPath = InternalJobPath + @"\02 Translation\01 To\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                        if (ItemFolderToBeRenamedPath.Length < 248)
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(InternalJobPath + @"\02 Translation\01 To\" + TranslationSubfolder.Name, jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name));
                        }
                        else
                        {
                            string NewFolderName = jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                            string ParentFolderToItemFolder = InternalJobPath + @"\02 Translation\01 To\";
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(ParentFolderToItemFolder + TranslationSubfolder.Name, NewFolderName.Substring(0, 247 - ParentFolderToItemFolder.Length));
                        }
                    }

                    catch { }
                }
            }

            //If no folder with same id is found, then create a new subfolder for current job item
            if (Directory.Exists(InternalJobPath + @"\02 Translation\01 To\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name)) == false)
            {
                try
                {
                    string TranslationItemFolder = InternalJobPath + @"\02 Translation\01 To\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);


                    if (TranslationItemFolder.Length < 248)
                    {
                        Directory.CreateDirectory(TranslationItemFolder);
                    }
                    else
                    {
                        Directory.CreateDirectory(TranslationItemFolder.Substring(0, 247));
                    }
                }
                catch
                { }
            }

            //Checking if the subfolders under 'Translation / 02 From' folder have same id as current job item id
            DirectoryInfo TranslationFromDirectoryInfo = new DirectoryInfo(InternalJobPath + @"\02 Translation\02 From");
            foreach (DirectoryInfo TranslationSubfolder in TranslationFromDirectoryInfo.GetDirectories())
            {

                if (TranslationSubfolder.Name.Split(" -")[0] == jobitemid.ToString() && TranslationSubfolder.Name != jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name))
                {
                    try
                    {
                        string ItemFolderToBeRenamedPath = InternalJobPath + @"\02 Translation\02 From\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                        if (ItemFolderToBeRenamedPath.Length < 248)
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(InternalJobPath + @"\02 Translation\02 From\" + TranslationSubfolder.Name, jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name));
                        }
                        else
                        {
                            string NewFolderName = jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                            string ParentFolderToItemFolder = InternalJobPath + @"\02 Translation\02 From\";
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(ParentFolderToItemFolder + TranslationSubfolder.Name, NewFolderName.Substring(0, 247 - ParentFolderToItemFolder.Length));
                        }
                    }
                    catch { }
                }
            }

            //If no folder with same id is found, then create a new subfolder for current job item


            if (Directory.Exists(InternalJobPath + @"\02 Translation\02 From\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name)) == false)
            {
                string TranslationItemFolder = InternalJobPath + @"\02 Translation\02 From\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                if (TranslationItemFolder.Length < 248)
                {
                    Directory.CreateDirectory(TranslationItemFolder);
                }
                else
                {
                    Directory.CreateDirectory(TranslationItemFolder.Substring(0, 247));
                }
            }

        }
        public async System.Threading.Tasks.Task configurenetworkfolders(int jobitemid, bool fromexternalserver = false)
        {
            // ***first checking the external drives for parent job folders and subfolders, then checking for the job item folders 
            // like subfolder under 'collection' folder

            string joborderfolderpath = "";
            var jobItem = await itemRepository.All().Where(o => o.Id == jobitemid).FirstOrDefaultAsync();
            var jobOrder = await jobOrderRepository.All().Where(o => o.Id == jobItem.JobOrderId).FirstOrDefaultAsync();
            var orderContact = await contactRepository.All().Where(o => o.Id == jobOrder.ContactId && o.DeletedDate == null).FirstOrDefaultAsync();
            var org = await orgRepository.All().Where(o => o.Id == orderContact.OrgId).FirstOrDefaultAsync();
            var Languages = await cachedService.GetAllLanguagesCached();
            var sourcelang = Languages.Where(o => o.StringValue == jobItem.SourceLanguageIanacode).ToList().FirstOrDefault();
            var targetlang = Languages.Where(o => o.StringValue == jobItem.TargetLanguageIanacode).ToList().FirstOrDefault();

            // first check if the job order folder already exists
            if (orderService.ExtranetAndWSDirectoryPathForApp(jobOrder.Id) != "")
                joborderfolderpath = orderService.ExtranetAndWSDirectoryPathForApp(jobOrder.Id);
            else
            {
                // if we cannot find the extranetbase path of job order, then do the below

                // checking  if the contact exists in the job folders directory
                string externalcontactpattern = orderContact.Id + "*";
                string externalcontactdirpath = globalVariables.ExtranetNetworkBaseDirectoryPath + @"\Contacts";
                DirectoryInfo externaldirinfo = new DirectoryInfo(externalcontactdirpath);
                DirectoryInfo[] matchingcontactsdirs = externaldirinfo.GetDirectories(externalcontactpattern, SearchOption.TopDirectoryOnly);
                string externalcontactfolderpath = "";
                if (matchingcontactsdirs.Count() == 0)
                {
                    externalcontactfolderpath = Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath + @"\Contacts", GeneralUtils.MakeStringSafeForFileSystemPath(orderContact.Id.ToString()));
                    Directory.CreateDirectory(externalcontactfolderpath);
                }
                else
                    externalcontactfolderpath = matchingcontactsdirs[0].FullName;

                // checking for the job folder
                string externaljobpattern = jobOrder.Id + "*";
                DirectoryInfo externaljobdirinfo = new DirectoryInfo(externalcontactfolderpath);
                DirectoryInfo[] matchingjobinfo = externaljobdirinfo.GetDirectories(externaljobpattern, SearchOption.TopDirectoryOnly);

                if (matchingjobinfo.Count() == 0)
                {
                    joborderfolderpath = Path.Combine(externalcontactfolderpath, jobOrder.Id + " - " + jobOrder.SubmittedDateTime.ToString("d mmm yy"));
                    Directory.CreateDirectory(joborderfolderpath);
                }
                else
                    joborderfolderpath = matchingjobinfo[0].FullName;
            }

            if (Directory.Exists(joborderfolderpath + @"\Collection") == false)
                Directory.CreateDirectory(joborderfolderpath + @"\Collection");

            if (Directory.Exists(joborderfolderpath + @"\Source") == false)
                Directory.CreateDirectory(joborderfolderpath + @"\Source");


            // create review folders and its subfolders in external directory if the job item is of language service -'client review'
            if (jobItem.LanguageServiceId == 21 | jobItem.LanguageServiceId == 67)
            {
                if (Directory.Exists(joborderfolderpath + @"\Review") == false)
                    Directory.CreateDirectory(joborderfolderpath + @"\Review");
                string reviewjobitemsfolder = joborderfolderpath + @"\Review";

                DirectoryInfo reviewdirectoryinfo = new DirectoryInfo(reviewjobitemsfolder);

                // for subfolders already existing under collection folder, check if any has same id as the current job item id and has different source & target language name
                // and rename the subfolder where applicable
                foreach (DirectoryInfo reviewsubfolder in reviewdirectoryinfo.GetDirectories())
                {
                    if (reviewsubfolder.Name.Split(" -")[0] == jobitemid.ToString() && reviewsubfolder.Name != jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name))
                    {
                        try
                        {
                            string newjobitemfolderpath = reviewjobitemsfolder + @"\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                            if (newjobitemfolderpath.Length < 248)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(reviewjobitemsfolder + @"\" + reviewsubfolder.Name, jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name));
                            else
                            {
                                string parentfolderpathofjobitem = reviewjobitemsfolder + @"\";
                                string newjobitemfoldername = jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(reviewjobitemsfolder + @"\" + reviewsubfolder.Name, newjobitemfoldername.Substring(0, 247 - parentfolderpathofjobitem.Length));
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                string reviewtoandfromsubfolder = reviewjobitemsfolder + @"\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);

                if (Directory.Exists(reviewtoandfromsubfolder) == false)
                {
                    try
                    {
                        if (reviewtoandfromsubfolder.Length < 248)
                            Directory.CreateDirectory(reviewtoandfromsubfolder);
                        else
                            Directory.CreateDirectory(reviewtoandfromsubfolder.Substring(0, 247));
                    }
                    catch
                    {
                    }
                }

                if (Directory.Exists(reviewtoandfromsubfolder + @"\FromReview") == false)
                    Directory.CreateDirectory(reviewtoandfromsubfolder + @"\FromReview");

                if (Directory.Exists(reviewtoandfromsubfolder + @"\ToReview") == false)
                    Directory.CreateDirectory(reviewtoandfromsubfolder + @"\ToReview");
            }
            else
            {
                DirectoryInfo jobitemsdirectoryinfo = new DirectoryInfo(joborderfolderpath + @"\Collection");

                // for subfolders already existing under collection folder, check if any has same id as the current job item id and has different source & target language name
                // and rename the subfolder where applicable
                foreach (DirectoryInfo jobitemsubfolder in jobitemsdirectoryinfo.GetDirectories())
                {
                    if (jobitemsubfolder.Name.Split(" -")[0] == jobitemid.ToString() && jobitemsubfolder.Name != jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name))
                    {
                        try
                        {
                            string pathtojobitem = joborderfolderpath + @"\Collection\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                            if (pathtojobitem.Length < 248)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(joborderfolderpath + @"\Collection\" + jobitemsubfolder.Name, jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name));
                            else
                            {
                                string parentfoldertojobitem = joborderfolderpath + @"\Collection\";
                                string jobitemname = jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(joborderfolderpath + @"\Collection\" + jobitemsubfolder.Name, jobitemname.Substring(0, 247 - parentfoldertojobitem.Length));
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                string jobitempath = joborderfolderpath + @"\Collection" + @"\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                if (Directory.Exists(jobitempath) == false)
                {
                    try
                    {
                        if (jobitempath.Length < 248)
                            Directory.CreateDirectory(jobitempath);
                        else
                            Directory.CreateDirectory(jobitempath.Substring(0, 247));
                    }
                    catch
                    {
                    }
                }

                if (Directory.Exists(jobitempath + @"\Status2") == false)
                    Directory.CreateDirectory(jobitempath + @"\Status2");

                if (Directory.Exists(jobitempath + @"\Status3") == false)
                    Directory.CreateDirectory(jobitempath + @"\Status3");
            }

            if (jobItem.LanguageServiceId == 1 & (jobItem.SupplierIsClientReviewer == true | orderContact.OrgId == 79702 | orderContact.Id == 220866 | orderContact.Id == 82578))
            {
                if (Directory.Exists(joborderfolderpath + @"\Translation") == false)
                    Directory.CreateDirectory(joborderfolderpath + @"\Translation");
                string translationjobitemsfolder = joborderfolderpath + @"\Translation";

                DirectoryInfo translationdirectoryinfo = new DirectoryInfo(translationjobitemsfolder);

                // for subfolders already existing under collection folder, check if any has same id as the current job item id and has different source & target language name
                // and rename the subfolder where applicable
                foreach (DirectoryInfo translationsubfolder in translationdirectoryinfo.GetDirectories())
                {
                    if (translationsubfolder.Name.Split(" -")[0] == jobitemid.ToString() && translationsubfolder.Name != jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name))
                    {
                        try
                        {
                            string newjobitemfolderpath = translationjobitemsfolder + @"\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                            if (newjobitemfolderpath.Length < 248)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(translationjobitemsfolder + @"\" + translationsubfolder.Name, jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name));
                            else
                            {
                                string parentfolderpathofjobitem = translationjobitemsfolder + @"\";
                                string newjobitemfoldername = jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(translationjobitemsfolder + @"\" + translationsubfolder.Name, newjobitemfoldername.Substring(0, 247 - parentfolderpathofjobitem.Length));
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                string translationtoandfromsubfolder = translationjobitemsfolder + @"\" + jobitemid.ToString() + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourcelang.Name) + "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetlang.Name);

                if (Directory.Exists(translationtoandfromsubfolder) == false)
                {
                    try
                    {
                        if (translationtoandfromsubfolder.Length < 248)
                            Directory.CreateDirectory(translationtoandfromsubfolder);
                        else
                            Directory.CreateDirectory(translationtoandfromsubfolder.Substring(0, 247));
                    }
                    catch
                    {
                    }
                }

                if (Directory.Exists(translationtoandfromsubfolder + @"\FromTranslation") == false)
                    Directory.CreateDirectory(translationtoandfromsubfolder + @"\FromTranslation");

                if (Directory.Exists(translationtoandfromsubfolder + @"\ToTranslation") == false)
                    Directory.CreateDirectory(translationtoandfromsubfolder + @"\ToTranslation");
            }


            // ***for internal job drives
            if (fromexternalserver == false)
            {
                // check if the org folder exists in the network drive for app, if not create the same;
                string orgdirsearchpattern = orderContact.OrgId + "*";
                string orgdirpath;
                string orderdirsearchpattern = jobOrder.Id.ToString() + "*";
                DirectoryInfo dirinfo = new DirectoryInfo(await orgsService.JobServerLocationForApp(orderContact.OrgId));

                if (await orgsService.JobServerLocationForApp(orderContact.OrgId) == globalVariables.SofiaJobDriveBaseDirectoryPathForApp && jobOrder.SubmittedDateTime > new DateTime(2022, 3, 21, 7, 0, 0))
                    dirinfo = new DirectoryInfo(globalVariables.ParisJobDriveBaseDirectoryPathForApp);

                // find org folder first (job folders should then appear within that)
                DirectoryInfo[] matchingorgdirs = dirinfo.GetDirectories(orgdirsearchpattern, SearchOption.TopDirectoryOnly);
                if (matchingorgdirs.Count() == 0)
                {
                    string neworgdirsearchpattern = orderContact.OrgId.ToString();
                    DirectoryInfo[] newmatchingorgdirs = dirinfo.GetDirectories(neworgdirsearchpattern, SearchOption.TopDirectoryOnly);
                    if (newmatchingorgdirs.Count() == 0)
                    {
                        orgdirpath = Path.Combine(await orgsService.JobServerLocationForApp(orderContact.OrgId), GeneralUtils.MakeStringSafeForFileSystemPath(orderContact.OrgId.ToString()));

                        Directory.CreateDirectory(orgdirpath);

                        Directory.CreateDirectory(Path.Combine(orgdirpath, "Client Invoices"));
                        Directory.CreateDirectory(Path.Combine(orgdirpath, "Key Client Info"));
                        // update nov 2011: automatically copy into "key client info" the templated "client rules.docx" file
                        // (if this fails, don't fail the whole thing)
                        try
                        {
                            File.Copy(@"\\fredcpfspdm0003\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML\Client Rules.docx", Path.Combine(Path.Combine(orgdirpath, "Key Client Info"), "Client Rules.docx"));
                        }
                        catch
                        {
                        }
                        Directory.CreateDirectory(Path.Combine(orgdirpath, "Memories & Glossaries"));
                    }
                    else
                        orgdirpath = newmatchingorgdirs[0].FullName;
                }
                else
                    orgdirpath = matchingorgdirs[0].FullName;

                // checking internal job folders
                // if main job folder does not exist in the orgs folder, then create this folder and subfolders
                dirinfo = new DirectoryInfo(orgdirpath);
                DirectoryInfo[] matchingorderdirs = dirinfo.GetDirectories(orderdirsearchpattern, SearchOption.TopDirectoryOnly);
                string internaljobpath = "";
                if (matchingorderdirs.Count() == 0)
                {

                    // no order folder found, so create it now by copying the template job
                    // folder directory structure to the relevant location
                    internaljobpath = Path.Combine(orgdirpath, GeneralUtils.MakeStringSafeForFileSystemPath(jobOrder.Id.ToString() + " - " + jobOrder.SubmittedDateTime.ToString("d mmm yy")));
                    Directory.CreateDirectory(internaljobpath);
                }
                else
                    internaljobpath = matchingorderdirs[0].FullName;

                // if the language service of the job item being created is 'translation', then 
                // create relevant translation folders in internal job folders if not already present


                if (jobItem.LanguageServiceId == 36)
                {
                    DirectoryInfo jobfolderinfo = new DirectoryInfo(internaljobpath);

                    if (Directory.Exists(internaljobpath + @"\01 Source files") == false)
                        // this could happen when the original template selected while job order creation was not one of the translation templates
                        Directory.CreateDirectory(internaljobpath + @"\01 Source files");

                    if (Directory.Exists(internaljobpath + @"\03 Proofreading") == false && Directory.Exists(internaljobpath + @"\04 Proofreading") == false && Directory.Exists(internaljobpath + @"\05 Proofreading") == false & Directory.Exists(internaljobpath + @"\06 Proofreading") == false & Directory.Exists(internaljobpath + @"\07 Proofreading") == false)
                    {
                        try
                        {
                            Directory.CreateDirectory(internaljobpath + @"\03 Proofreading");
                        }
                        catch
                        {
                        }
                    }

                    if (Directory.Exists(internaljobpath + @"\02 Translation") == false)
                    {
                        // this could happen when the original template selected while job order creation was not one of the translation templates
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation");
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation" + @"\01 To");
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation" + @"\02 From");
                    }
                    else if (Directory.Exists(internaljobpath + @"\03 Transcreation") == false)
                    {
                        Directory.CreateDirectory(internaljobpath + @"\03 Transcreation");
                        Directory.CreateDirectory(internaljobpath + @"\03 Transcreation" + @"\01 To");
                        Directory.CreateDirectory(internaljobpath + @"\03 Transcreation" + @"\02 From");
                        if (Directory.Exists(internaljobpath + @"\05 Proofreading") == true)
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\05 Proofreading", "06 Proofreading");
                        if (Directory.Exists(internaljobpath + @"\04 Proofreading") == true)
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\04 Proofreading", "05 Proofreading");
                        if (Directory.Exists(internaljobpath + @"\03 Proofreading") == true)
                        {
                            if (Directory.Exists(internaljobpath + @"\04 Client review") == true)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\04 Client review", "05 Client review");
                            if (Directory.Exists(internaljobpath + @"\05 Delivery") == true)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\05 Delivery", "06 Delivery");
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\03 Proofreading", "04 Proofreading");
                        }
                        if (Directory.Exists(internaljobpath + @"\04 Delivery") == true)
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\04 Delivery", "05 Delivery");
                        if (Directory.Exists(internaljobpath + @"\03 DTP") == true)
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\03 DTP", "04 DTP");
                            if (Directory.Exists(internaljobpath + @"\04 Proofreading") == true)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\04 Proofreading", "05 Proofreading");
                            if (Directory.Exists(internaljobpath + @"\05 Client review") == true)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\05 Client review", "06 Client review");
                            if (Directory.Exists(internaljobpath + @"\06 Delivery") == true)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\06 Delivery", "07 Delivery");
                            else if (Directory.Exists(internaljobpath + @"\05 Delivery") == true)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\05 Delivery", "06 Delivery");
                        }
                    } // transcreation folder exists loop
                }

                if (jobItem.LanguageServiceId == 1 || jobItem.LanguageServiceId == 17)
                {
                    DirectoryInfo jobfolderinfo = new DirectoryInfo(internaljobpath);

                    if (Directory.Exists(internaljobpath + @"\01 Source files") == false)
                        // this could happen when the original template selected while job order creation was not one of the translation templates
                        Directory.CreateDirectory(internaljobpath + @"\01 Source files");

                    if (Directory.Exists(internaljobpath + @"\02 Translation") == false)
                    {

                        // this could happen when the original template selected while job order creation was not one of the translation templates
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation");
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation" + @"\01 To");
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation" + @"\02 From");
                    } // translation folder exists loop

                    if (Directory.Exists(internaljobpath + @"\02 Transcreation") == true)
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\02 Transcreation", "03 Transcreation");
                        // if directory.exists(internaljobpath & "\05 delivery") = true then
                        // fileio.filesystem.deletedirectory(internaljobpath & "\05 delivery", fileio.deletedirectoryoption.deleteallcontents)
                        // end if
                        Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\03 Proofreading", "04 Proofreading");
                        Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\04 Client Review", "05 Client Review");
                        if (Directory.Exists(internaljobpath + @"\05 DTP") == true)
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\05 DTP", "06 DTP");
                        else
                            await DeliveryFolderCheck("04", "05", internaljobpath, jobitemid);
                        if (Directory.Exists(internaljobpath + @"\06 VO") == true)
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\06 VO", "07 VO");
                        if (Directory.Exists(internaljobpath + @"\07 File Prep") == true)
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\07 File Prep", "08 File Prep");
                        if (Directory.Exists(internaljobpath + @"\08 Testing (Vetting)") == true)
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\08 Testing (Vetting)", "09 Testing (Vetting)");
                            await DeliveryFolderCheck("09", "10", internaljobpath, jobitemid);
                        }
                        if (Directory.Exists(internaljobpath + @"\06 Testing (Vetting)") == true)
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\06 Testing (Vetting)", "07 Testing (Vetting)");
                            await DeliveryFolderCheck("07", "08", internaljobpath, jobitemid);
                        }
                    }



                    if (jobItem.LanguageServiceId == 1 & (jobItem.SupplierIsClientReviewer == true | orderContact.OrgId == 79702 | orderContact.Id == 220866 | orderContact.Id == 82578))
                        // create job item folders under to and from subfolders of translation folders
                        await CreateTranslationSubfolders(internaljobpath, jobitemid);

                    // check if dtp folder already exist, if not then do not create dtp folder
                    // since this is just to rename the folders according to order if they exist
                    if (Directory.Exists(internaljobpath + @"\02 DTP") == true && Directory.Exists(internaljobpath + @"\03 DTP") == false)
                    {
                        try
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\02 DTP", "03 DTP");
                        }
                        catch
                        {
                        }
                    }

                    if (Directory.Exists(internaljobpath + @"\03 DTP") == true)
                    {

                        // rename the original "03 proofreading" folder as "04 proofreading"
                        if (Directory.Exists(internaljobpath + @"\03 Proofreading") == true && Directory.Exists(internaljobpath + @"\04 Proofreading") == false)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\03 Proofreading", "04 Proofreading");
                            }
                            catch
                            {
                            }
                        }

                        if (Directory.Exists(internaljobpath + @"\04 Proofreading") == false)
                            Directory.CreateDirectory(internaljobpath + @"\04 Proofreading");

                        // check if client review folder already exist, else do not create client review folder
                        // since this is just to rename the folders according to order if they exist
                        if (Directory.Exists(internaljobpath + @"\04 Client review") == true && Directory.Exists(internaljobpath + @"\05 Client review") == false)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\04 Client review", "05 Client review");
                            }
                            catch
                            {
                            }
                        }

                        if (Directory.Exists(internaljobpath + @"\05 Client review") == true)

                            // check on delivery folder
                            await DeliveryFolderCheck("05", "06", internaljobpath, jobitemid);
                        else

                            // check on delivery folder
                            await DeliveryFolderCheck("04", "05", internaljobpath, jobitemid); // client review condition loop
                    }
                    else
                    {

                        // rename the original "02 proofreading" folder as "03 proofreading"
                        if (Directory.Exists(internaljobpath + @"\02 Proofreading") == true && Directory.Exists(internaljobpath + @"\03 Proofreading") == false)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\02 Proofreading", "03 Proofreading");
                            }
                            catch
                            {
                            }
                        }

                        if (Directory.Exists(internaljobpath + @"\03 Proofreading") == false)
                        {
                            if (Directory.Exists(internaljobpath + @"\04 Proofreading") == false & Directory.Exists(internaljobpath + @"\05 Proofreading") == false & Directory.Exists(internaljobpath + @"\06 Proofreading") == false & Directory.Exists(internaljobpath + @"\07 Proofreading") == false)
                                Directory.CreateDirectory(internaljobpath + @"\03 Proofreading");
                        }

                        // check if client review folder already exist, else do not create client review folder
                        // since this is just to rename the folders according to order if they exist
                        if (Directory.Exists(internaljobpath + @"\03 Client review") == true && Directory.Exists(internaljobpath + @"\04 Client review") == false)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\03 Client review", "04 Client review");
                            }
                            catch
                            {
                            }
                        }

                        if (Directory.Exists(internaljobpath + @"\04 Client review") == true)

                            // check on delivery folder
                            await DeliveryFolderCheck("04", "05", internaljobpath, jobitemid);
                        else

                            // check on delivery folder
                            await DeliveryFolderCheck("03", "04", internaljobpath, jobitemid); // client review condition loop
                    } // dtp condition loop
                } // language service = 1 or 17 loop

                // ***
                // if the language service of the job item being created is 'dtp', then create relevant dtp folders in internal job folders
                if (jobItem.LanguageServiceId == 4)
                {

                    // if the translation folder does not exist, for eg in case of 'proofreading only' template, 
                    // create the translation and source files anyway when creating dtp folder,
                    // this is just to keep the uniformity in overall folder structure

                    if (Directory.Exists(internaljobpath + @"\01 Source files") == false)
                        // this could happen when the original template selected was not one of the translation templates
                        Directory.CreateDirectory(internaljobpath + @"\01 Source files");

                    if (Directory.Exists(internaljobpath + @"\02 Translation") == false)
                    {
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation");
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation" + @"\01 To");
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation" + @"\02 From");
                    }

                    if (Directory.Exists(internaljobpath + @"\03 Transcreation") == true)
                    {
                        if (Directory.Exists(internaljobpath + @"\04 DTP") == false)
                        {
                            Directory.CreateDirectory(internaljobpath + @"\04 DTP");
                            Directory.CreateDirectory(internaljobpath + @"\04 DTP\" + "01 To");
                            Directory.CreateDirectory(internaljobpath + @"\04 DTP\" + "02 From");
                        }
                        // rename the original "03 proofreading" folder as "04 proofreading"
                        if (Directory.Exists(internaljobpath + @"\04 Proofreading") == true && Directory.Exists(internaljobpath + @"\05 Proofreading") == false)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\04 Proofreading", "05 Proofreading");
                            }
                            catch
                            {
                            }
                        }
                        else if (Directory.Exists(internaljobpath + @"\05 Proofreading") == true & Directory.Exists(internaljobpath + @"\06 Client review") == false & Directory.Exists(internaljobpath + @"\06 Delivery") == false)
                            Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\05 Proofreading", "06 Proofreading");

                        // if no proofreading folder exists at all ,then create one
                        if (Directory.Exists(internaljobpath + @"\05 Proofreading") == false & Directory.Exists(internaljobpath + @"\06 Proofreading") == false)
                            Directory.CreateDirectory(internaljobpath + @"\05 Proofreading");


                        if (Directory.Exists(internaljobpath + @"\04 Client review") == true && Directory.Exists(internaljobpath + @"\05 Client review") == false)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\04 Client review", "05 Client review");
                            }
                            catch
                            {
                            }
                        }
                        else if (Directory.Exists(internaljobpath + @"\05 Client review") == true)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\05 Client review", "06 Client review");
                            }
                            catch
                            {
                            }
                        }


                        if (Directory.Exists(internaljobpath + @"\05 Client review") == true)
                            // check delivery folder
                            await DeliveryFolderCheck("05", "06", internaljobpath, jobitemid);
                        else if (Directory.Exists(internaljobpath + @"\06 Client review") == true)
                            await DeliveryFolderCheck("06", "07", internaljobpath, jobitemid);
                    }
                    else
                    {
                        if (Directory.Exists(internaljobpath + @"\03 DTP") == false)
                        {
                            Directory.CreateDirectory(internaljobpath + @"\03 DTP");
                            Directory.CreateDirectory(internaljobpath + @"\03 DTP\" + "01 To");
                            Directory.CreateDirectory(internaljobpath + @"\03 DTP\" + "02 From");
                        }
                        // rename the original "03 proofreading" folder as "04 proofreading"
                        if (Directory.Exists(internaljobpath + @"\03 Proofreading") == true && Directory.Exists(internaljobpath + @"\04 Proofreading") == false)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\03 Proofreading", "04 Proofreading");
                            }
                            catch
                            {
                            }
                        }

                        // if no proofreading folder exists at all ,then create one
                        if (Directory.Exists(internaljobpath + @"\04 Proofreading") == false)
                            Directory.CreateDirectory(internaljobpath + @"\04 Proofreading");


                        if (Directory.Exists(internaljobpath + @"\04 Client review") == true && Directory.Exists(internaljobpath + @"\05 Client review") == false)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\04 Client review", "05 Client review");
                            }
                            catch
                            {
                            }
                        }


                        if (Directory.Exists(internaljobpath + @"\05 Client review") == true)
                            // check delivery folder
                            await DeliveryFolderCheck("05", "06", internaljobpath, jobitemid);
                        else

                            // check delivery folder
                            await DeliveryFolderCheck("04", "05", internaljobpath, jobitemid);
                    }
                } // language service = 4 loop


                // if the language service of the job item is 'client review', then create relevant client review folders in internal job folders
                if (jobItem.LanguageServiceId == 21 | jobItem.LanguageServiceId == 67)
                {

                    // if the translation folder does not exist, for eg in case of 'proofreading only' template, 
                    // create the translation folder anyway when creating 'client review' folder,
                    // this is just to keep the uniformity in overall folder structure, 


                    if (Directory.Exists(internaljobpath + @"\01 Source files") == false)
                        // this could happen when the original template selected was not one of the translation templates
                        Directory.CreateDirectory(internaljobpath + @"\01 Source files");

                    if (Directory.Exists(internaljobpath + @"\02 Translation") == false)
                    {
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation");
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation" + @"\01 To");
                        Directory.CreateDirectory(internaljobpath + @"\02 Translation" + @"\02 From");
                    }

                    if (Directory.Exists(internaljobpath + @"\03 DTP") == true)
                    {
                        // 03 dtp and 04 proofreading would be as it is
                        // checking for '05 client review'
                        if (Directory.Exists(internaljobpath + @"\05 Client review") == false)
                        {
                            Directory.CreateDirectory(internaljobpath + @"\05 Client review");
                            Directory.CreateDirectory(internaljobpath + @"\05 Client review" + @"\01 To");
                            Directory.CreateDirectory(internaljobpath + @"\05 Client review" + @"\02 From");
                        }
                        // create job item folders under to and from subfolders of clinet review
                        await CreateClientReviewSubfolders("05", internaljobpath, jobitemid);

                        if (Directory.Exists(internaljobpath + @"\05 Delivery") == true && Directory.Exists(internaljobpath + @"\06 Delivery") == false)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\05 Delivery", "06 Delivery");
                            }
                            catch
                            {
                            }
                        }

                        if (Directory.Exists(internaljobpath + @"\06 Delivery") == false)
                            Directory.CreateDirectory(internaljobpath + @"\06 Delivery");
                    }
                    else if (Directory.Exists(internaljobpath + @"\03 Transcreation") == false)
                    {

                        // if dtp folder does not exist
                        if (Directory.Exists(internaljobpath + @"\04 Client review") == false)
                        {
                            Directory.CreateDirectory(internaljobpath + @"\04 Client review");
                            Directory.CreateDirectory(internaljobpath + @"\04 Client review" + @"\01 To");
                            Directory.CreateDirectory(internaljobpath + @"\04 Client review" + @"\02 From");
                        }
                        // create job item folders under to and from subfolders of clinet review
                        await CreateClientReviewSubfolders("04", internaljobpath, jobitemid);

                        if (Directory.Exists(internaljobpath + @"\04 Delivery") == true && Directory.Exists(internaljobpath + @"\05 Delivery") == false)
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\04 Delivery", "05 Delivery");
                            }
                            catch
                            {
                            }
                        }

                        if (Directory.Exists(internaljobpath + @"\05 Delivery") == false)
                            Directory.CreateDirectory(internaljobpath + @"\05 Delivery");
                    }
                    else if (Directory.Exists(internaljobpath + @"\03 Transcreation") == true)
                    {
                        if (Directory.Exists(internaljobpath + @"\04 DTP") == true)
                        {
                            // checking for '05 client review'
                            if (Directory.Exists(internaljobpath + @"\06 Client review") == false)
                            {
                                Directory.CreateDirectory(internaljobpath + @"\06 Client review");
                                Directory.CreateDirectory(internaljobpath + @"\06 Client review" + @"\01 To");
                                Directory.CreateDirectory(internaljobpath + @"\06 Client review" + @"\02 From");
                            }

                            if (Directory.Exists(internaljobpath + @"\06 Proofreading") == true)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\06 Proofreading", "05 Proofreading");

                            if (Directory.Exists(internaljobpath + @"\06 Delivery") == true & Directory.Exists(internaljobpath + @"\06 Client review") == true)
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(internaljobpath + @"\06 Delivery", "07 Delivery");
                        }
                        else if (Directory.Exists(internaljobpath + @"\05 Client review") == false)
                        {
                            Directory.CreateDirectory(internaljobpath + @"\05 Client review");
                            Directory.CreateDirectory(internaljobpath + @"\05 Client review" + @"\01 To");
                            Directory.CreateDirectory(internaljobpath + @"\05 Client review" + @"\02 From");

                            await CreateClientReviewSubfolders("05", internaljobpath, jobitemid);
                        }
                        else if (Directory.Exists(internaljobpath + @"\05 Client review") == true)
                            await CreateClientReviewSubfolders("05", internaljobpath, jobitemid);
                    }
                }

                if ((jobItem.LanguageServiceId == 8 | jobItem.LanguageServiceId == 1) && orderContact.OrgId == 139412)
                {
                    string mojtrackerfolderpath = @"\\gblonpfspdm0005\Jobs\136951 - MoJ\Key Client Info\MoJ Tracker";
                    string mojtrackerjobpath = "";

                    DirectoryInfo mojtrackerfolder = new DirectoryInfo(mojtrackerfolderpath);

                    try
                    {
                        if (mojtrackerfolder.GetDirectories(jobOrder.Id.ToString(), SearchOption.AllDirectories).Count() == 1)
                        {
                            mojtrackerjobpath = mojtrackerfolder.GetDirectories(jobOrder.Id.ToString(), SearchOption.AllDirectories)[0].FullName;


                            string deliveryfolderpath = "";
                            if (Directory.Exists(mojtrackerjobpath + @"\04 Delivery") == true)
                                deliveryfolderpath = Path.Combine(mojtrackerjobpath, "04 Delivery");
                            else if (Directory.Exists(mojtrackerjobpath + @"\05 Delivery") == true)
                                deliveryfolderpath = Path.Combine(mojtrackerjobpath, "05 Delivery");
                            else if (Directory.Exists(mojtrackerjobpath + @"\06 Delivery") == true)
                                deliveryfolderpath = Path.Combine(mojtrackerjobpath, "06 Delivery");

                            DirectoryInfo deliveryfolder = new DirectoryInfo(deliveryfolderpath);
                            if (deliveryfolder.GetDirectories(jobitemid.ToString(), SearchOption.TopDirectoryOnly).Count() == 0)
                            {
                                string deliveryjobitempath = Path.Combine(deliveryfolderpath, GeneralUtils.MakeStringSafeForFileSystemPath(jobitemid.ToString() + " - " + sourcelang.Name + "-" + targetlang.Name));
                                Directory.CreateDirectory(deliveryjobitempath);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }

        }

        public async Task<JobItem> CreateJobItem(JobItem model)
        {

            bool orderExists = await jobOrderService.OrderExists(model.JobOrderId);
            int NewJobItemID;
            JobItem NewJobItem = null;

            if (orderExists == false)
            {
                throw new Exception("The order ID, " + model.JobOrderId.ToString() + ", does not exist in the database. A job item must be created against a valid order.");
            }
            else
            {
                NewJobItem = new JobItem()
                {
                    JobOrderId = model.JobOrderId,
                    IsVisibleToClient = model.IsVisibleToClient,
                    LanguageServiceId = model.LanguageServiceId,
                    SourceLanguageIanacode = model.SourceLanguageIanacode,
                    TargetLanguageIanacode = model.TargetLanguageIanacode,
                    WordCountNew = model.WordCountNew,
                    WordCountFuzzyBand1 = model.WordCountFuzzyBand1,
                    WordCountFuzzyBand2 = model.WordCountFuzzyBand2,
                    WordCountFuzzyBand3 = model.WordCountFuzzyBand3,
                    WordCountFuzzyBand4 = model.WordCountFuzzyBand4,
                    WordCountExact = model.WordCountExact,
                    WordCountRepetitions = model.WordCountRepetitions,
                    WordCountPerfectMatches = model.WordCountPerfectMatches,
                    TranslationMemoryRequired = model.TranslationMemoryRequired,
                    Pages = model.Pages,
                    Characters = model.Characters,
                    Documents = model.Documents,
                    InterpretingExpectedDurationMinutes = model.InterpretingExpectedDurationMinutes,
                    InterpretingLocationOrgName = model.InterpretingLocationOrgName,
                    InterpretingLocationAddress1 = model.InterpretingLocationAddress1,
                    InterpretingLocationAddress2 = model.InterpretingLocationAddress2,
                    InterpretingLocationAddress3 = model.InterpretingLocationAddress3,
                    InterpretingLocationAddress4 = model.InterpretingLocationAddress4,
                    InterpretingLocationCountyOrState = model.InterpretingLocationCountyOrState,
                    InterpretingLocationPostcodeOrZip = model.InterpretingLocationPostcodeOrZip,
                    InterpretingLocationCountryId = model.InterpretingLocationCountryId,
                    AudioMinutes = model.AudioMinutes,
                    WorkMinutes = model.WorkMinutes,
                    OurCompletionDeadline = model.OurCompletionDeadline,
                    DescriptionForSupplierOnly = model.DescriptionForSupplierOnly,
                    SupplierIsClientReviewer = model.SupplierIsClientReviewer,
                    LinguisticSupplierOrClientReviewerId = model.LinguisticSupplierOrClientReviewerId,
                    ChargeToClient = model.ChargeToClient,
                    PaymentToSupplier = model.PaymentToSupplier,
                    PaymentToSupplierCurrencyId = model.PaymentToSupplierCurrencyId,
                    SupplierInvoicePaidMethodId = model.SupplierInvoicePaidMethodId,
                    CreatedByEmployeeId = model.CreatedByEmployeeId,
                    CreatedDateTime = timeZonesService.GetCurrentGMT(),
                    SupplierWordCountNew = model.SupplierWordCountNew,
                    SupplierWordCountFuzzyBand1 = model.SupplierWordCountFuzzyBand1,
                    SupplierWordCountFuzzyBand2 = model.SupplierWordCountFuzzyBand2,
                    SupplierWordCountFuzzyBand3 = model.SupplierWordCountFuzzyBand3,
                    SupplierWordCountFuzzyBand4 = model.SupplierWordCountFuzzyBand4,
                    SupplierWordCountExact = model.SupplierWordCountExact,
                    SupplierWordCountRepetitions = model.SupplierWordCountRepetitions,
                    SupplierWordCountPerfectMatches = model.SupplierWordCountPerfectMatches,
                    SupplierWordCountsTakenFromClient = model.SupplierWordCountsTakenFromClient,
                    CustomerSpecificField = model.CustomerSpecificField,
                    ContextFieldsList = model.ContextFieldsList,
                    WordCountClientSpecific = model.WordCountClientSpecific,
                    SupplierWordCountClientSpecific = model.SupplierWordCountClientSpecific,
                    ClientCostCalculatedByPriceList = model.ClientCostCalculatedByPriceList,
                    ClientCostCalculatedByDateTime = model.ClientCostCalculatedByDateTime,
                    ClientCostCalculatedById = model.ClientCostCalculatedById,
                    SupplierCostCalculatedById = model.SupplierCostCalculatedById,
                    SupplierCostCalculatedByDateTime = model.SupplierCostCalculatedByDateTime,
                    SupplierCostCalculatedByPriceList = model.SupplierCostCalculatedByPriceList,
                    MinimumSupplierChargeApplied = model.MinimumSupplierChargeApplied,
                    SupplierCompletionDeadline = model.SupplierCompletionDeadline,
                    LanguageServiceCategoryId = model.LanguageServiceCategoryId


                };
                await itemRepository.AddAsync(NewJobItem);
                await itemRepository.SaveChangesAsync();
                NewJobItemID = NewJobItem.Id;
            }





            return NewJobItem;
        }
        public async Task<List<JobItem>> CheckAvailableTargetLanguages(string sourceLanguageSelected, int jobOrderID)
        {
            var result = await itemRepository.All().Where(a => a.SourceLanguageIanacode == sourceLanguageSelected && a.DeletedDateTime == null && a.JobOrderId == jobOrderID).ToListAsync();
            return result;
        }

        public async Task<int> UpdateReviewerOfJobItem(int jobItemId, int reviewerId)
        {
            var jobItem = await itemRepository.All().Where(i => i.Id == jobItemId).FirstOrDefaultAsync();

            jobItem.LinguisticSupplierOrClientReviewerId = reviewerId;

            var res = await itemRepository.SaveChangesAsync();
            return res;
        }

        public string ExtranetTranslateOnlineFromTranslationDirectoryPathForApp(int jobitemId)
        {
            try
            {
                string ExpectedPath = "";
                //start from the overall review plus folder - if cannot be found, don't continue
                if (ExtranetTranslateOnlineOverallDirectoryPathForApp(jobitemId) == "")
                {
                    ExpectedPath = "";
                }
                else
                {
                    ExpectedPath = Path.Combine(ExtranetTranslateOnlineOverallDirectoryPathForApp(jobitemId), "FromTranslation");
                }
                return ExpectedPath;
            }
            catch
            {
                // doesn't matter why it couldn't determine the file location - just return empty path
                return "";
            }

        }

        public string ExtranetTranslateOnlineOverallDirectoryPathForApp(int jobitemId)
        {
            try
            {
                string ExpectedPath = "";
                var jobItem = GetById(jobitemId).Result;
                //start from the folder for the parent job order - if that can't be located
                //then don't proceed

                string ParentContactExtranetFolderPath = jobOrderService.ExtranetAndWSDirectoryPathForApp(jobItem.JobOrderId);

                if (ParentContactExtranetFolderPath == "")
                {
                    ExpectedPath = "";
                }
                else
                {
                    ExpectedPath = Path.Combine(ParentContactExtranetFolderPath, "Translation", jobitemId + " - " +
                                  GeneralUtils.MakeStringSafeForFileSystemPath(languageService.GetLanguageInfo(jobItem.SourceLanguageIanacode, "en").Name) + "-" +
                                  GeneralUtils.MakeStringSafeForFileSystemPath(languageService.GetLanguageInfo(jobItem.TargetLanguageIanacode, "en").Name));
                }
                return ExpectedPath;
            }
            catch
            {
                //doesn't matter why it couldn't determine the file location - just return empty path
                return "";
            }

        }

        public string ExtranetReviewPlusFromReviewDirectoryPathForApp(int jobItemID)
        {
            try
            {
                string ExpectedPath = "";

                //start from the overall review plus folder - if cannot be found, don't continue
                if (ExtranetReviewPlusOverallDirectoryPathForApp(jobItemID) == "")
                {
                    ExpectedPath = "";
                }
                else
                {
                    ExpectedPath = Path.Combine(ExtranetReviewPlusOverallDirectoryPathForApp(jobItemID), "FromReview");
                }
                return ExpectedPath;
            }
            catch
            {
                //doesn't matter why it couldn't determine the file location - just return empty path
                return "";
            }
        }

        public string ExtranetReviewPlusOverallDirectoryPathForApp(int jobItemId)
        {
            try
            {
                string ExpectedPath = "";
                //start from the folder for the parent job order - if that can't be located
                //then don't proceed
                var jobItem = GetById(jobItemId).Result;
                var sourceLang = languageService.GetLanguageInfo(jobItem.SourceLanguageIanacode, "en");
                var targetLang = languageService.GetLanguageInfo(jobItem.TargetLanguageIanacode, "en");
                string ParentContactExtranetFolderPath = orderService.ExtranetAndWSDirectoryPathForApp(jobItem.JobOrderId);
                if (ParentContactExtranetFolderPath == "")
                {
                    ExpectedPath = "";
                }
                else
                {
                    ExpectedPath = Path.Combine(ParentContactExtranetFolderPath, "Review\\" + jobItemId + " - " + GeneralUtils.MakeStringSafeForFileSystemPath(sourceLang.Name) +
                                                  "-" + GeneralUtils.MakeStringSafeForFileSystemPath(targetLang.Name));
                }

                return ExpectedPath;
            }
            catch
            {
                //doesn't matter why it couldn't determine the file location - just return empty path
                return "";
            }
        }

        public string ExtranetFolderForSupplierPath(int jobItemId)
        {
            string path = "";
            string extranetToFolderForSupplierPath = "";
            var jobItem = GetById(jobItemId).Result;
            var lingusticSupplierId = jobItem.LinguisticSupplierOrClientReviewerId;
            if (lingusticSupplierId != null)
            {
                string ItemFolderOnExtranetBase = Path.Combine(Path.Combine(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Suppliers"), lingusticSupplierId.ToString()), jobItemId.ToString());
                string ItemFolderOnExtranetBaseNAS1 = Path.Combine(Path.Combine(Path.Combine(globalVariables.OldExtranetNetworkBaseDirectoryPathNAS1, "Suppliers"), lingusticSupplierId.ToString()), jobItemId.ToString());
                string ItemFolderOnExtranetBaseDEV1 = Path.Combine(Path.Combine(Path.Combine(globalVariables.OldExtranetNetworkBaseDirectoryPathDEV1, "Suppliers"), lingusticSupplierId.ToString()), jobItemId.ToString());
                string ItemDirPath = ItemFolderOnExtranetBase;

                if (!Directory.Exists(ItemFolderOnExtranetBase))
                {
                    if (Directory.Exists(ItemFolderOnExtranetBaseNAS1))
                    {
                        ItemDirPath = ItemFolderOnExtranetBaseNAS1;
                    }
                    else
                    {
                        if (Directory.Exists(ItemFolderOnExtranetBaseDEV1))
                        {
                            ItemDirPath = ItemFolderOnExtranetBaseDEV1;
                        }
                    }
                }
                path = ItemDirPath;
            }
            if (path != "")
            {
                if (Directory.Exists(Path.Combine(path, "01 To")))
                {
                    extranetToFolderForSupplierPath = Path.Combine(path, "01 To");
                }
            }
            return extranetToFolderForSupplierPath;
        }


        public async Task<bool> UpdateClientReviewStatus(int jobItemId, Enumerations.ReviewStatus NewStatus, string NotesFromClientReviewer = "")
        {
            var thisJobItem = await GetById(jobItemId);
            bool UpdateSuccessful = false;
            string NotesToAdd = thisJobItem.ExtranetSignoffComment;
            NotesFromClientReviewer = NotesFromClientReviewer.Trim();

            if (NotesToAdd == "")
            {
                NotesToAdd = NotesFromClientReviewer;
            }
            else if (NotesFromClientReviewer != "")
            {
                NotesToAdd = NotesToAdd + Environment.NewLine + Environment.NewLine +
                "------------------- " + Environment.NewLine + Environment.NewLine + NotesFromClientReviewer;
            }

            SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);
            SqlDataAdapter ItemDataAdapter = new SqlDataAdapter();

            ItemDataAdapter.UpdateCommand = new SqlCommand("procUpdateJobItemClientReviewStatus", SQLConn);

            ItemDataAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;

            ItemDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@ItemID", SqlDbType.Int)).Value = jobItemId;
            ItemDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@UpdatedClientReviewStatusID", SqlDbType.SmallInt)).Value = NewStatus;
            ItemDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@UpdatedExtranetSignoffComment", SqlDbType.NVarChar, -1)).Value = NotesToAdd;

            //Return value parameter

            var ReturnValParam = ItemDataAdapter.UpdateCommand.Parameters.Add("@RowCount", SqlDbType.Int);
            ReturnValParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                ItemDataAdapter.UpdateCommand.Connection.Open();
                ItemDataAdapter.UpdateCommand.ExecuteScalar();

                if (int.Parse(ItemDataAdapter.UpdateCommand.Parameters["@RowCount"].Value.ToString()) == 0)
                {
                    throw new Exception("Update was not successful.");
                }
                else
                {
                    UpdateSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    ItemDataAdapter.UpdateCommand.Connection.Close();
                    ItemDataAdapter.Dispose();
                }
                catch (SqlException se)
                {
                    // Log an event in the Application Event Log.
                    throw new Exception("SQL exception: " + se.Message);
                }
            }
            return UpdateSuccessful;
        }

        public async Task<JobItem> ApplyToJobItem(int jobItemID, int orgGroupID, int orgID, ViewModels.EmployeeModels.EmployeeViewModel EmployeeCurrentlyLoggedOn, TPWordCountBreakdownBatchModel model, bool applyToClientWordCounts = true, bool applyToClientWordCountsOnly = false)
        {
            JobItem thisJobItem = await GetById(jobItemID);
            if (thisJobItem.Id == 0 || thisJobItem == null)
                return null;

            int WordCountClientSpecific = 0;
            int SupplierWordCountClientSpecific = 0;
            if (thisJobItem != null)
            {
                if (orgGroupID != 0 && orgGroupID == 67437)
                {
                    if (WordCountClientSpecific == 0)
                    {
                        if (thisJobItem.WordCountClientSpecific != null && thisJobItem.WordCountClientSpecific > 0)
                        {
                            WordCountClientSpecific = (int)thisJobItem.WordCountClientSpecific;
                        }
                    }
                    if (SupplierWordCountClientSpecific == 0)
                    {
                        if (thisJobItem.SupplierWordCountClientSpecific != null && thisJobItem.SupplierWordCountClientSpecific > 0)
                        {
                            SupplierWordCountClientSpecific = (int)thisJobItem.SupplierWordCountClientSpecific;
                        }
                    }
                }
            }
            JobItem updateModel = new JobItem();
            updateModel.Id = jobItemID;
            short? InterpretingLocationCountryID = null;
            int? LinguisticSupplierOrClientReviewerID = null;
            short? PaymentToSupplierCurrencyID = null;
            byte? SupplierInvoicePaidMethodID = null;
            bool ItemIsComplete;

            if (thisJobItem.InterpretingLocationCountryId != null) { InterpretingLocationCountryID = thisJobItem.InterpretingLocationCountryId; }
            if (thisJobItem.SupplierInvoicePaidMethodId != null) { SupplierInvoicePaidMethodID = thisJobItem.SupplierInvoicePaidMethodId; }
            if (thisJobItem.SupplierIsClientReviewer == true)
            {
                if (thisJobItem.SupplierIsClientReviewer != null) { LinguisticSupplierOrClientReviewerID = thisJobItem.LinguisticSupplierOrClientReviewerId; }
            }
            else
            {
                if (thisJobItem.LinguisticSupplierOrClientReviewerId != null) { LinguisticSupplierOrClientReviewerID = thisJobItem.LinguisticSupplierOrClientReviewerId; }
            }
            if (thisJobItem.PaymentToSupplierCurrencyId != null) { PaymentToSupplierCurrencyID = thisJobItem.PaymentToSupplierCurrencyId; }

            ItemIsComplete = !(thisJobItem.WeCompletedItemDateTime == DateTime.MinValue || thisJobItem.WeCompletedItemDateTime == null);

            if (applyToClientWordCounts == true)
            {
                if (applyToClientWordCountsOnly == false)
                {
                    if (updateModel.Id == 0)
                        return null;

                    var dbItem = await itemRepository.All().Where(x => x.Id == updateModel.Id).FirstOrDefaultAsync();

                    dbItem.LastModifiedByEmployeeId = EmployeeCurrentlyLoggedOn.Id;
                    dbItem.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
                    dbItem.WordCountNew = model.pNewWords;
                    dbItem.WordCountFuzzyBand1 = model.pFuzzyBand1Words;
                    dbItem.WordCountFuzzyBand2 = model.pFuzzyBand2Words;
                    dbItem.WordCountFuzzyBand3 = model.pFuzzyBand3Words;
                    dbItem.WordCountFuzzyBand4 = model.pFuzzyBand4Words;
                    dbItem.WordCountExact = model.pExactMatchWords;
                    dbItem.WordCountRepetitions = model.pRepetitionsWords;
                    dbItem.WordCountPerfectMatches = model.pMatchPlusOrPerfectMatchWords;

                    dbItem.InterpretingLocationCountryId = InterpretingLocationCountryID;
                    if (ItemIsComplete == true)
                    {
                        dbItem.WeCompletedItemDateTime = GeneralUtils.GetCurrentUKTime();
                    }
                    dbItem.LinguisticSupplierOrClientReviewerId = LinguisticSupplierOrClientReviewerID;
                    dbItem.PaymentToSupplierCurrencyId = PaymentToSupplierCurrencyID;
                    dbItem.SupplierInvoicePaidMethodId = SupplierInvoicePaidMethodID;
                    dbItem.SupplierWordCountNew = model.pLinguisticNewWords;
                    dbItem.SupplierWordCountExact = model.pExactMatchWords;
                    dbItem.SupplierWordCountRepetitions = model.pRepetitionsWords;
                    dbItem.SupplierWordCountPerfectMatches = model.pMatchPlusOrPerfectMatchWords;
                    dbItem.SupplierWordCountFuzzyBand1 = model.pLinguisticFuzzyBand1Words;
                    dbItem.SupplierWordCountFuzzyBand2 = model.pLinguisticFuzzyBand2Words;
                    dbItem.SupplierWordCountFuzzyBand3 = model.pLinguisticFuzzyBand3Words;
                    dbItem.SupplierWordCountFuzzyBand4 = model.pLinguisticFuzzyBand4Words;

                    dbItem.WordCountClientSpecific = WordCountClientSpecific;
                    dbItem.SupplierWordCountClientSpecific = SupplierWordCountClientSpecific;

                    await itemRepository.SaveChangesAsync();

                    return updateModel;
                }
                else
                {
                    if (updateModel.Id == 0)
                        return null;

                    var dbItem = await itemRepository.All().Where(x => x.Id == updateModel.Id).FirstOrDefaultAsync();

                    dbItem.LastModifiedByEmployeeId = EmployeeCurrentlyLoggedOn.Id;
                    dbItem.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
                    dbItem.WordCountNew = model.pNewWords;
                    dbItem.WordCountFuzzyBand1 = model.pFuzzyBand1Words;
                    dbItem.WordCountFuzzyBand2 = model.pFuzzyBand2Words;
                    dbItem.WordCountFuzzyBand3 = model.pFuzzyBand3Words;
                    dbItem.WordCountFuzzyBand4 = model.pFuzzyBand4Words;
                    dbItem.WordCountExact = model.pExactMatchWords;
                    dbItem.WordCountRepetitions = model.pRepetitionsWords;
                    dbItem.WordCountPerfectMatches = model.pMatchPlusOrPerfectMatchWords;

                    dbItem.InterpretingLocationCountryId = InterpretingLocationCountryID;
                    if (ItemIsComplete == true)
                    {
                        dbItem.WeCompletedItemDateTime = GeneralUtils.GetCurrentUKTime();
                    }
                    dbItem.LinguisticSupplierOrClientReviewerId = LinguisticSupplierOrClientReviewerID;
                    dbItem.PaymentToSupplierCurrencyId = PaymentToSupplierCurrencyID;
                    dbItem.SupplierInvoicePaidMethodId = SupplierInvoicePaidMethodID;

                    dbItem.WordCountClientSpecific = WordCountClientSpecific;
                    dbItem.SupplierWordCountClientSpecific = SupplierWordCountClientSpecific;

                    await itemRepository.SaveChangesAsync();

                    return updateModel;
                }
            }
            else
            {
                if (updateModel.Id == 0)
                    return null;
                int ExactWords = model.pExactMatchWords;
                int Repetitions = model.pRepetitionsWords;
                int PerfectMatches = model.pMatchPlusOrPerfectMatchWords;

                if (orgID == 139232 || orgID == 139233)
                {
                    ExactWords = 0;
                    Repetitions = 0;
                    PerfectMatches = 0;
                }

                var dbItem = await itemRepository.All().Where(x => x.Id == updateModel.Id).FirstOrDefaultAsync();

                dbItem.LastModifiedByEmployeeId = EmployeeCurrentlyLoggedOn.Id;
                dbItem.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
                dbItem.InterpretingLocationCountryId = InterpretingLocationCountryID;
                if (ItemIsComplete == true)
                {
                    dbItem.WeCompletedItemDateTime = GeneralUtils.GetCurrentUKTime();
                }
                dbItem.LinguisticSupplierOrClientReviewerId = LinguisticSupplierOrClientReviewerID;
                dbItem.PaymentToSupplierCurrencyId = PaymentToSupplierCurrencyID;
                dbItem.SupplierInvoicePaidMethodId = SupplierInvoicePaidMethodID;
                dbItem.SupplierWordCountNew = model.pLinguisticNewWords;
                dbItem.SupplierWordCountExact = model.pExactMatchWords;
                dbItem.SupplierWordCountRepetitions = model.pRepetitionsWords;
                dbItem.SupplierWordCountPerfectMatches = model.pMatchPlusOrPerfectMatchWords;
                dbItem.SupplierWordCountFuzzyBand1 = model.pLinguisticFuzzyBand1Words;
                dbItem.SupplierWordCountFuzzyBand2 = model.pLinguisticFuzzyBand2Words;
                dbItem.SupplierWordCountFuzzyBand3 = model.pLinguisticFuzzyBand3Words;
                dbItem.SupplierWordCountFuzzyBand4 = model.pLinguisticFuzzyBand4Words;

                dbItem.SupplierWordCountExact = ExactWords;
                dbItem.SupplierWordCountRepetitions = Repetitions;
                dbItem.SupplierWordCountPerfectMatches = PerfectMatches;

                dbItem.WordCountClientSpecific = WordCountClientSpecific;
                dbItem.SupplierWordCountClientSpecific = SupplierWordCountClientSpecific;

                await itemRepository.SaveChangesAsync();

                return updateModel;
            }

        }
        public async Task<bool> ItemExists(int itemId)
        {
            int? result = await itemRepository.All().Where(o => o.Id == itemId && o.DeletedDateTime == null).Select(o => o.Id).FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<JobItemViewModel> RemoveItem(JobItemViewModel updateModel)
        {
            if (updateModel.Id == 0)
                return null;

            var dbItem = await itemRepository.All().Where(x => x.Id == updateModel.Id).FirstOrDefaultAsync();

            dbItem.LastModifiedByEmployeeId = (short)updateModel.LoggedInEmployeeId;
            dbItem.DeletedDateTime = GeneralUtils.GetCurrentUKTime();
            dbItem.DeletedFreeTextReason = updateModel.DeletedFreeTextReason;


            await itemRepository.SaveChangesAsync();

            return updateModel;
        }

        /// <summary>
        /// Sets up the job order for flow plus external applications
        /// </summary>
        /// <param name="SetUpOneJobOrderForAllLicences"></param>
        /// <param name="LicenceMappingId"></param>
        /// <param name="LicenceId"></param>
        /// <returns>returns true if job order creation is successful</returns>
        public async Task<bool> SetupFlowPlusLicencingJobOrder(bool SetUpOneJobOrderForAllLicences, int LicenceMappingId = 0, int LicenceId = 0)
        {
            DateTime overallDeadline = DateTime.Now.AddDays(5);
            var jobOrderCreationSuccessful = false;
            if (DateTime.Now >= new DateTime(2023, 04, 01))
            {
                if (SetUpOneJobOrderForAllLicences == true && LicenceMappingId > 0)
                {
                    flowPlusLicenceMapping mapping = await flowplusService.GetflowPlusLicencingMappingDetails(LicenceMappingId);
                    flowPlusLicences flowPlusLicence = await flowplusService.GetflowPlusLicenceObj(mapping.flowplusLicenceID.Value);

                    var contact = await contactService.GetById(flowPlusLicence.OrderContactID.Value);
                    var org = await orgService.GetOrgDetails(contact.OrgId);
                    var projectManagerId = globalVariables.TBAEmployeeID;
                    var opsOwner = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(contact.OrgId, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.OperationsLead);

                    if (opsOwner != null)
                    {
                        projectManagerId = opsOwner.Result.EmployeeId;
                    }


                    JobOrder newJobOrder = await jobOrderService.CreateNewOrder<JobOrder>(flowPlusLicence.OrderContactID.Value, projectManagerId, 8, "flow plus licensing - " + DateTime.Now.ToString("MMMM"),
                                                "", "", "", "", "", "", "", overallDeadline, 0, 0, org.InvoiceCurrencyId.Value, 0,
                                                false, false, false, globalVariables.iplusEmployeeID, false, false, "", "");

                    //flow plus licencing
                    await CreateItem(newJobOrder.Id, true, 118, "en", "en", 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                 Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                 0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                 overallDeadline, "", "", false, 0, flowPlusLicence.AppCost, 0, 0, DateTime.MinValue, 0,
                                                                                                 (byte)globalVariables.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false);
                    //review plus licencing
                    if (mapping.reviewPlusLicenceID != null && mapping.reviewPlusLicenceID.Value > 0)
                    {
                        flowPlusLicences reviewPlusLicence = await flowplusService.GetflowPlusLicenceObj(mapping.reviewPlusLicenceID.Value);

                        await CreateItem(newJobOrder.Id, true, 119, "en", "en", 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                     Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                     0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                     overallDeadline, "", "", false, 0, reviewPlusLicence.AppCost, 0, 0, DateTime.MinValue, 0,
                                                                                                     (byte)globalVariables.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false);
                    }

                    //translate online licencing
                    if (mapping.translateOnlineLicenceID != null && mapping.translateOnlineLicenceID.Value > 0)
                    {
                        flowPlusLicences translateOnlineLicence = await flowplusService.GetflowPlusLicenceObj(mapping.translateOnlineLicenceID.Value);

                        await CreateItem(newJobOrder.Id, true, 120, "en", "en", 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                     Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                     0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                     overallDeadline, "", "", false, 0, translateOnlineLicence.AppCost, 0, 0, DateTime.MinValue, 0,
                                                                                                     (byte)globalVariables.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false);
                    }

                    //design plus licencing
                    if (mapping.designPlusLicenceID != null && mapping.designPlusLicenceID.Value > 0)
                    {
                        flowPlusLicences designPlusLicence = await flowplusService.GetflowPlusLicenceObj(mapping.designPlusLicenceID.Value);

                        await CreateItem(newJobOrder.Id, true, 121, "en", "en", 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                     Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                     0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                     overallDeadline, "", "", false, 0, designPlusLicence.AppCost, 0, 0, DateTime.MinValue, 0,
                                                                                                     (byte)globalVariables.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false);
                    }

                    //ai or Mt licencing
                    if (mapping.AIOrMTLicenceID != null && mapping.AIOrMTLicenceID.Value > 0)
                    {
                        flowPlusLicences aiOrMTLicence = await flowplusService.GetflowPlusLicenceObj(mapping.AIOrMTLicenceID.Value);

                        await CreateItem(newJobOrder.Id, true, 122, "en", "en", 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                     Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                     0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                     overallDeadline, "", "", false, 0, aiOrMTLicence.AppCost, 0, 0, DateTime.MinValue, 0,
                                                                                                     (byte)globalVariables.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false);
                    }

                    //CMS licencing
                    if (mapping.CMSLicenceID != null && mapping.CMSLicenceID.Value > 0)
                    {
                        flowPlusLicences cmsLicence = await flowplusService.GetflowPlusLicenceObj(mapping.CMSLicenceID.Value);

                        await CreateItem(newJobOrder.Id, true, 123, "en", "en", 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                     Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                     0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                     overallDeadline, "", "", false, 0, cmsLicence.AppCost, 0, 0, DateTime.MinValue, 0,
                                                                                                     (byte)globalVariables.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false);
                    }

                    jobOrderCreationSuccessful = true;

                }
                else if (SetUpOneJobOrderForAllLicences == false && LicenceId > 0)
                {
                    flowPlusLicences thisLicence = await flowplusService.GetflowPlusLicenceObj(LicenceId);

                    var contact = await contactService.GetById(thisLicence.OrderContactID.Value);
                    var org = await orgService.GetOrgDetails(contact.OrgId);
                    var projectManagerId = globalVariables.TBAEmployeeID;
                    var opsOwner = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(contact.OrgId, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.OperationsLead);

                    if (opsOwner != null)
                    {
                        projectManagerId = opsOwner.Result.EmployeeId;
                    }

                    var thisApp = await flowplusService.GetflowPlusAppDetails(thisLicence.ApplicationId);

                    JobOrder newJobOrder = await jobOrderService.CreateNewOrder<JobOrder>(thisLicence.OrderContactID.Value, projectManagerId, 8, thisApp.ApplicationName + " licensing - " + DateTime.Now.ToString("MMMM"),
                                                "", "", "", "", "", "", "", overallDeadline, 0, 0, org.InvoiceCurrencyId.Value, 0,
                                                false, false, false, globalVariables.iplusEmployeeID, false, false, "", "");



                    await CreateItem(newJobOrder.Id, true, thisApp.LanguageServiceID, "en", "en", 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                 Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                 0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                 overallDeadline, "", "", false, 0, thisLicence.AppCost, 0, 0, DateTime.MinValue, 0,
                                                                                                 (byte)globalVariables.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false);
                    jobOrderCreationSuccessful = true;
                }
            }


            return jobOrderCreationSuccessful;
        }
    }
}
