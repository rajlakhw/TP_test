using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Data.Repositories;
//using Data;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using ViewModels.JobOrder;
using System.IO;
using Global_Settings;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;
using ViewModels.JobItem;
using System.Text.RegularExpressions;
using Services.Interfaces;
using ViewModels.flowPlusExternal;
using Ionic.Zip;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.flowPlusExternal.ReviewPlus;
using System.Web;
using System.Xml;

namespace Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TPJobOrderService" in both code and config file together.
    public class TPJobOrderService : ITPJobOrderService
    {
        private readonly IRepository<JobOrder> orderRepository;
        private readonly IConfiguration configuration;
        private readonly IRepository<ExtranetUsersTemp> extranetUserRepo;
        private readonly IRepository<ExtranetAccessLevels> extranetAccessLevelsRepo;
        private readonly IRepository<Contact> contactRepo;
        private readonly IRepository<Org> orgRepo;
        private readonly ITPExchangeService exchangeService;
        private readonly IRepository<JobItem> jobitemsRepo;
        private readonly IRepository<JobOrderPgddetail> pgdRepository;
        private readonly IRepository<Currency> currencyRepo;
        private readonly IRepository<JobOrderChannel> channel;
        private readonly IRepository<LocalLanguageInfo> localLangRepo;
        private readonly IRepository<LanguageService> langServiceRepo;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly IRepository<LinguisticSupplier> linguisticSupRepo;
        private readonly IRepository<Employee> employeesRepo;
        private readonly IEmailUtilsService emailUtils;
        private readonly IRepository<ClientDesignFile> clientDesignFileRepo;
        private readonly IRepository<DesignPlusReviewJob> dpReviewJobRepo;
        private readonly IRepository<ClientInvoice> clientInvoiceRepo;
        private readonly IRepository<EmployeeOwnershipRelationship> empOwnershipRelationshipRepo;
        private readonly ITPEmployeeOwnershipsLogic ownershipsLogicService;
        private readonly ITPClientInvoicesLogic clientInvoiceService;
        private readonly ITPTimeZonesService timeZonesService;
        private readonly ITPCurrenciesLogic currencyService;
        //private readonly ITPJobItemService jobItemService;
        private readonly ITPOrgsLogic orgService;
        private readonly ITPOrgGroupsLogic orgGroupService;
        private readonly ITPVolumeDiscountsService volumeDiscountsService;
        private readonly ITPBrandsService brandService;
        private readonly GlobalVariables globalVariables;
        private readonly IRepository<OrgGroup> orgGroupRepo;
        private readonly IRepository<EmployeeTeam> empTeamsRepo;
        private readonly IRepository<QuoteAndOrderDiscountsAndSurcharge> discountsurchargeRepo;
        private readonly ITPMiscResourceService miscResourceService;
        private readonly ITPEnquiriesService enquiryService;
        private readonly ITPQuotesLogic quoteServices;
        private readonly ITPContactsLogic contactService;
        private readonly ITPLanguageLogic langService;
        private readonly ITPEmployeesService empService;
        private readonly IRepository<DeadlineChangeReason> deadlineChangeReasonRepo;

        public TPJobOrderService(IRepository<JobOrder> repository,
            IConfiguration configuration, IRepository<ExtranetUsersTemp> repository1,
            IRepository<ExtranetAccessLevels> repository2,
            IRepository<Contact> repository3,
            IRepository<Org> repository4,
            ITPExchangeService exchangeService1,
            IRepository<JobItem> repository5,
            IRepository<JobOrderPgddetail> pgdRepository,
            IRepository<Currency> repository6,
            IRepository<JobOrderChannel> repository7,
            IRepository<LocalLanguageInfo> repository8,
            IRepository<LanguageService> repository9,
            ITPExtranetUserService extranetUserService,
            IRepository<LinguisticSupplier> linguisticSupRepo,
            IRepository<Employee> employees,
            IEmailUtilsService emailUtils,
            IRepository<ClientDesignFile> clientDesignFileRepo,
            IRepository<DesignPlusReviewJob> dpReviewJobRepo,
            IRepository<ClientInvoice> clientInvoiceRepo,
            ITPEmployeeOwnershipsLogic ownershipsLogicService,
            ITPClientInvoicesLogic clientInvoiceService,
            ITPTimeZonesService tPTimeZonesService,
            ITPCurrenciesLogic tPCurrenciesLogic,
            ITPOrgsLogic tPOrgsLogic,
            ITPVolumeDiscountsService tPVolumeDiscountsService,
            ITPBrandsService tPBrandsService,
            ITPOrgGroupsLogic groupService,
            IRepository<OrgGroup> orgGroupRepo,
            IRepository<EmployeeTeam> empTeamsRepo,
            IRepository<QuoteAndOrderDiscountsAndSurcharge> discountsurchargeRepo,
            ITPMiscResourceService tPMiscResourceService,
            ITPEnquiriesService tpEnquiriesService,
            ITPContactsLogic tPContactsLogic,
            ITPQuotesLogic tPQuotesLogic,
            ITPLanguageLogic tPLanguageLogic,
            ITPEmployeesService tPEmployeesService,
            IRepository<DeadlineChangeReason> _deadlineChangeReasonRepo)
        {
            this.orderRepository = repository;
            this.configuration = configuration;
            this.extranetUserRepo = repository1;
            this.extranetAccessLevelsRepo = repository2;
            this.contactRepo = repository3;
            this.orgRepo = repository4;
            this.exchangeService = exchangeService1;
            this.jobitemsRepo = repository5;
            this.pgdRepository = pgdRepository;
            this.currencyRepo = repository6;
            this.channel = repository7;
            this.localLangRepo = repository8;
            this.langServiceRepo = repository9;
            this.extranetUserService = extranetUserService;
            this.linguisticSupRepo = linguisticSupRepo;
            this.employeesRepo = employees;
            this.emailUtils = emailUtils;
            this.clientDesignFileRepo = clientDesignFileRepo;
            this.dpReviewJobRepo = dpReviewJobRepo;
            this.clientInvoiceRepo = clientInvoiceRepo;
            this.ownershipsLogicService = ownershipsLogicService;
            this.orgGroupRepo = orgGroupRepo;
            this.empTeamsRepo = empTeamsRepo;
            this.discountsurchargeRepo = discountsurchargeRepo;
            this.clientInvoiceService = clientInvoiceService;
            this.timeZonesService = tPTimeZonesService;
            this.currencyService = tPCurrenciesLogic;
            this.orgService = tPOrgsLogic;
            this.volumeDiscountsService = tPVolumeDiscountsService;

            globalVariables = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);
            this.brandService = tPBrandsService;
            this.orgGroupService = groupService;
            this.miscResourceService = tPMiscResourceService;
            this.enquiryService = tpEnquiriesService;
            this.contactService = tPContactsLogic;
            this.quoteServices = tPQuotesLogic;
            this.langService = tPLanguageLogic;
            this.empService = tPEmployeesService;
            this.deadlineChangeReasonRepo = _deadlineChangeReasonRepo;

        }

        public async Task<JobOrder> GetById(int JobOrderID)
        {
            var result = await orderRepository.All().Where(a => a.Id == JobOrderID && a.DeletedDate == null).FirstOrDefaultAsync();
            return result;
        }

        public async Task<JobOrderViewModel> GetViewModelById(int JobOrderID)
        {
            var result = await orderRepository.All().Where(a => a.Id == JobOrderID && a.DeletedDate == null)
                .Select(x => new JobOrderViewModel()
                {
                    Id = x.Id,
                    JobName = System.Web.HttpUtility.HtmlDecode(x.JobName),
                    OverallDeliveryDeadline = x.OverallDeliveryDeadline,
                    GrossMarginPercentage = x.GrossMarginPercentage,
                    AnticipatedGrossMarginPercentage = x.AnticipatedGrossMarginPercentage,
                    SubmittedDateTime = x.SubmittedDateTime,
                    LastModifiedDate = x.LastModifiedDate,
                    ClientNotes = x.ClientNotes,
                    InternalNotes = x.InternalNotes,
                    IsAtrialProject = x.IsAtrialProject,
                    PrintingProject = x.PrintingProject,
                    EscalatedToAccountManager = x.EscalatedToAccountManager,
                    IsHighlyConfidential = x.IsHighlyConfidential,
                    Priority = x.Priority,
                    ClientPonumber = x.ClientPonumber,
                    InvoicingNotes = x.InvoicingNotes,
                    CustomerSpecificField1Value = x.CustomerSpecificField1Value,
                    CustomerSpecificField2Value = x.CustomerSpecificField2Value,
                    CustomerSpecificField3Value = x.CustomerSpecificField3Value,
                    CustomerSpecificField4Value = x.CustomerSpecificField4Value,
                    OverallCompletedDateTime = x.OverallCompletedDateTime,
                    ClientInvoiceId = x.ClientInvoiceId,
                    OverdueReasonId = x.OverdueReasonId,
                    OverdueComment = x.OverdueComment,
                    IsCls = x.IsCls,
                    OriginatedFromEnquiryId = x.OriginatedFromEnquiryId,
                    EndClientId = x.EndClientId,
                    CampaignId = x.CampaignId,
                    BrandId = x.BrandId,
                    CategoryId = x.CategoryId,
                    DiscountAmount = x.DiscountAmount,
                    DiscountId = x.DiscountId,
                    SurchargeAmount = x.SurchargeAmount,
                    SurchargeId = x.SurchargeId,
                    OverallChargeToClient = x.OverallChargeToClient,
                    SubTotalOverallChargeToClient = x.SubTotalOverallChargeToClient,
                    TypeOfOrder = x.TypeOfOrder,
                    OrgHfmcodeBs = x.OrgHfmcodeBs,
                    OrgHfmcodeIs = x.OrgHfmcodeIs,
                    DeadlineChangeReason = x.DeadlineChangeReason,
                    ClientCurrencyId = x.ClientCurrencyId,
                    JobOrderChannelId = x.JobOrderChannelId
                })
                .FirstOrDefaultAsync();

            if (result != null)
            {
                //if (result.IsCls == true)
                //{
                result.PGDDetails = await pgdRepository.All().Where(x => x.JobOrderId == result.Id)
                    .Select(x => new ViewModels.JobOrder.PgdDetails()
                    {
                        Id = x.Id,
                        JobOrderId = x.JobOrderId,
                        ThirdPartyId = x.ThirdPartyId,
                        ProductionContact = x.ProductionContact,
                        ProjectStatus = x.ProjectStatus,
                        Icponumber = x.Icponumber,
                        GlossaryUpdated = x.GlossaryUpdated,
                        Bshfmnumber = x.Bshfmnumber,
                        Isnumber = x.Isnumber,
                        ApprovedEndClientCharge = x.ApprovedEndClientCharge,
                        EndClientChargeCurrencyId = x.EndClientChargeCurrencyId,
                        ApprovedEndClientChargeGbp = x.ApprovedEndClientChargeGbp,
                        Variance = x.Variance
                    }).FirstOrDefaultAsync();
                //}

                if (result.SurchargeId != null)
                {
                    result.SurchargeDetails = await discountsurchargeRepo.All().Where(x => x.Id == result.SurchargeId)
                    .Select(x => new ViewModels.JobOrder.SurchargeDetails()
                    {
                        SurchargeId = x.Id,
                        SurchargeCategory = x.DiscountOrSurchargeCategory,
                        PercentageOrValue = x.PercentageOrValue,
                        Description = x.Description,
                        SurchargeAmount = x.DiscountOrSurchargeAmount
                    }).FirstOrDefaultAsync();
                }

                if (result.DiscountId != null)
                {
                    result.DiscountDetails = await discountsurchargeRepo.All().Where(x => x.Id == result.DiscountId)
                    .Select(x => new ViewModels.JobOrder.DiscountDetails()
                    {
                        DiscountId = x.Id,
                        DiscountCategory = x.DiscountOrSurchargeCategory,
                        PercentageOrValue = x.PercentageOrValue,
                        Description = x.Description,
                        DiscountAmount = x.DiscountOrSurchargeAmount
                    }).FirstOrDefaultAsync();
                }

            }

            return result;
        }

        public String NetworkDirectoryPathForApp(int JobOrderID, int OrgID, byte JobServerLocation, bool ForUser, DateTime? SubmittedDateTime = null)
        {
            String NetworkDirectoryPath = "";

            String OrgDirSearchPattern = OrgID.ToString() + " *";
            String OrgDirPath;
            String OrgDirPathOnJDrive;
            String OrderDirSearchPattern = JobOrderID.ToString() + " *";
            String JobServerLocationForApp;

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            switch (JobServerLocation)
            {
                case 0:
                    JobServerLocationForApp = GlobalVars.LondonJobDriveBaseDirectoryPathForApp;
                    break;
                case 1:
                    JobServerLocationForApp = GlobalVars.SofiaJobDriveBaseDirectoryPathForApp;
                    break;
                case 2:
                    JobServerLocationForApp = GlobalVars.ParisJobDriveBaseDirectoryPathForApp;
                    break;
                default:
                    JobServerLocationForApp = GlobalVars.LondonJobDriveBaseDirectoryPathForApp;
                    break;
            }

            DirectoryInfo DirInfo = new DirectoryInfo(JobServerLocationForApp);

            if (JobServerLocationForApp == GlobalVars.SofiaJobDriveBaseDirectoryPathForApp && SubmittedDateTime > new DateTime(2022, 3, 21, 7, 0, 0))
                DirInfo = new DirectoryInfo(GlobalVars.ParisJobDriveBaseDirectoryPathForApp);

            // find org folder first (job folders should then appear within that)
            DirectoryInfo[] MatchingOrgDirs = DirInfo.GetDirectories(OrgDirSearchPattern, SearchOption.TopDirectoryOnly);

            if (MatchingOrgDirs.Length == 0)
            {
                String newOrgDirSearchPattern = OrgID.ToString();
                DirectoryInfo[] newMatchingOrgDirs = DirInfo.GetDirectories(newOrgDirSearchPattern, SearchOption.TopDirectoryOnly);
                if (newMatchingOrgDirs.Length == 0)
                {
                    NetworkDirectoryPath = "";
                    return NetworkDirectoryPath; // no org folder found, so don't bother searching further
                }
                else
                {
                    OrgDirPath = newMatchingOrgDirs[0].FullName;
                    OrgDirPathOnJDrive = newMatchingOrgDirs[0].FullName;
                }
            }
            else
            {
                OrgDirPath = MatchingOrgDirs[0].FullName;
                OrgDirPathOnJDrive = MatchingOrgDirs[0].FullName;
            }


            // now look for job folder within the org folder
            DirInfo = new DirectoryInfo(OrgDirPath);
            DirectoryInfo[] MatchingOrderDirs =
                DirInfo.GetDirectories(OrderDirSearchPattern, SearchOption.TopDirectoryOnly);
            if (MatchingOrderDirs.Length == 0)
            {
                NetworkDirectoryPath = "";
                return NetworkDirectoryPath; // no org folder found, so don't bother searching further
            }
            else
            {
                NetworkDirectoryPath = (MatchingOrderDirs[0].FullName);
                //return NetworkDirectoryPath;
            }


            if (ForUser == false)
            {
                return NetworkDirectoryPath;
            }
            else
            {
                return ((NetworkDirectoryPath.Replace(GlobalVars.LondonJobDriveBaseDirectoryPathForApp,
                                  GlobalVars.LondonJobDriveBaseDirectoryPathForUser)).Replace(
                                  GlobalVars.SofiaJobDriveBaseDirectoryPathForApp,
                                  GlobalVars.SofiaJobDriveBaseDirectoryPathForUser)).Replace(
                                  GlobalVars.ParisJobDriveBaseDirectoryPathForApp,
                                  GlobalVars.ParisJobDriveBaseDirectoryPathForUser);
                ;
            }
        }


        public String NetworkDirectoryPathForApp(int JobOrderID, bool ForUser = false, DateTime? SubmittedDateTime = null)
        {
            var order = GetById(JobOrderID);
            var contact = contactService.GetById(order.Result.ContactId);
            var org = orgService.GetOrgDetails(contact.Result.OrgId);

            String NetworkDirectoryPath = "";

            String OrgDirSearchPattern = contact.Result.OrgId.ToString() + " *";
            String OrgDirPath;
            String OrgDirPathOnJDrive;
            String OrderDirSearchPattern = JobOrderID.ToString() + " *";
            String JobServerLocationForApp;

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            switch (org.Result.JobServerLocation)
            {
                case 0:
                    JobServerLocationForApp = GlobalVars.LondonJobDriveBaseDirectoryPathForApp;
                    break;
                case 1:
                    JobServerLocationForApp = GlobalVars.SofiaJobDriveBaseDirectoryPathForApp;
                    break;
                case 2:
                    JobServerLocationForApp = GlobalVars.ParisJobDriveBaseDirectoryPathForApp;
                    break;
                default:
                    JobServerLocationForApp = GlobalVars.LondonJobDriveBaseDirectoryPathForApp;
                    break;
            }

            DirectoryInfo DirInfo = new DirectoryInfo(JobServerLocationForApp);

            if (JobServerLocationForApp == GlobalVars.SofiaJobDriveBaseDirectoryPathForApp && SubmittedDateTime > new DateTime(2022, 3, 21, 7, 0, 0))
                DirInfo = new DirectoryInfo(GlobalVars.ParisJobDriveBaseDirectoryPathForApp);

            // find org folder first (job folders should then appear within that)
            DirectoryInfo[] MatchingOrgDirs = DirInfo.GetDirectories(OrgDirSearchPattern, SearchOption.TopDirectoryOnly);

            if (MatchingOrgDirs.Length == 0)
            {
                String newOrgDirSearchPattern = contact.Result.OrgId.ToString();
                DirectoryInfo[] newMatchingOrgDirs = DirInfo.GetDirectories(newOrgDirSearchPattern, SearchOption.TopDirectoryOnly);
                if (newMatchingOrgDirs.Length == 0)
                {
                    NetworkDirectoryPath = "";
                    return NetworkDirectoryPath; // no org folder found, so don't bother searching further
                }
                else
                {
                    OrgDirPath = newMatchingOrgDirs[0].FullName;
                    OrgDirPathOnJDrive = newMatchingOrgDirs[0].FullName;
                }
            }
            else
            {
                OrgDirPath = MatchingOrgDirs[0].FullName;
                OrgDirPathOnJDrive = MatchingOrgDirs[0].FullName;
            }


            // now look for job folder within the org folder
            DirInfo = new DirectoryInfo(OrgDirPath);
            DirectoryInfo[] MatchingOrderDirs =
                DirInfo.GetDirectories(OrderDirSearchPattern, SearchOption.TopDirectoryOnly);
            if (MatchingOrderDirs.Length == 0)
            {
                NetworkDirectoryPath = "";
                return NetworkDirectoryPath; // no org folder found, so don't bother searching further
            }
            else
            {
                NetworkDirectoryPath = (MatchingOrderDirs[0].FullName);
                //return NetworkDirectoryPath;
            }


            if (ForUser == false)
            {
                return NetworkDirectoryPath;
            }
            else
            {
                return ((NetworkDirectoryPath.Replace(GlobalVars.LondonJobDriveBaseDirectoryPathForApp,
                                  GlobalVars.LondonJobDriveBaseDirectoryPathForUser)).Replace(
                                  GlobalVars.SofiaJobDriveBaseDirectoryPathForApp,
                                  GlobalVars.SofiaJobDriveBaseDirectoryPathForUser)).Replace(
                                  GlobalVars.ParisJobDriveBaseDirectoryPathForApp,
                                  GlobalVars.ParisJobDriveBaseDirectoryPathForUser);
                ;
            }
        }
        public async Task<JobOrder> UpdateKeyInformation<JobOrderViewteModel>(int Id, string JobName, DateTime deadline, short? lastModifiedByUserId, bool markcompleted, byte? OverdueReasonID, string OverdueComment, bool? IsCls, int? TypeOfOrder, bool? EscalatedToAccountManager, string? DeadlineChangeReason,int? LinkedJobOrder)
        {
            var joborder = await GetById(Id);

            // maybe add security check later here and autoMapper
            joborder.JobName = JobName;
            joborder.OverallDeliveryDeadline = deadline;
            joborder.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
            joborder.LastModifiedByEmployeeId = lastModifiedByUserId;
            joborder.EscalatedToAccountManager = EscalatedToAccountManager;
            if (OverdueReasonID != null && OverdueReasonID > 0)
            {
                joborder.OverdueReasonId = OverdueReasonID;
            }

            if (DeadlineChangeReason != null && DeadlineChangeReason != "-1")
            {
                joborder.DeadlineChangeReason = DeadlineChangeReason;
            }

            if (joborder.OverdueComment != OverdueComment && OverdueComment != "|")
            {
                joborder.OverdueComment = OverdueComment;
                joborder.OverdueCommentUpdate = DateTime.Today;
            }


            //if (markuncompleted == true)
            //{
            //    joborder.OverallCompletedDateTime = (DateTime?)null;
            //}

            if (markcompleted == true)
            {
                joborder.OverallCompletedDateTime = GeneralUtils.GetCurrentUKTime();
            }
            else
            {
                joborder.OverallCompletedDateTime = (DateTime?)null;
            }

            joborder.IsCls = IsCls;
            joborder.TypeOfOrder = TypeOfOrder;
            if (LinkedJobOrder != null)
            {
                joborder.LinkedJobOrderId = LinkedJobOrder;
            }
            orderRepository.Update(joborder);
            await orderRepository.SaveChangesAsync();

            return joborder;
        }

        public async Task<JobOrder> UpdateProjectInformation<JobOrderViewteModel>(int Id, short ProjectManagerId, byte JobOrderChannelId, string ClientNotes, string InternalNotes, bool IsATrialProject, bool? IsAPrintingProject, bool IsHighlyConfidential, byte? Priority, short? lastModifiedByUserId)
        {
            var joborder = await GetById(Id);

            // maybe add security check later here and autoMapper
            joborder.ProjectManagerEmployeeId = ProjectManagerId;
            joborder.JobOrderChannelId = JobOrderChannelId;
            joborder.ClientNotes = ClientNotes;
            joborder.InternalNotes = InternalNotes;

            joborder.IsAtrialProject = IsATrialProject;
            joborder.PrintingProject = IsAPrintingProject;
            joborder.IsHighlyConfidential = IsHighlyConfidential;
            joborder.Priority = Priority;
            joborder.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
            joborder.LastModifiedByEmployeeId = lastModifiedByUserId;

            orderRepository.Update(joborder);
            await orderRepository.SaveChangesAsync();

            return joborder;
        }

        public async Task<JobOrder> UpdateFinancialInformation<JobOrderViewteModel>(int Id, string InvoicingNotes, string ClientPonumber, DateTime? EarlyInvoiceDateTime, short? EarlyInvoiceByEmpId, string CustomerSpecificField1Value, string CustomerSpecificField2Value, string CustomerSpecificField3Value, string CustomerSpecificField4Value, short? lastModifiedByUserId, string OrgHfmcodeIs, string OrgHfmcodeBs, short ClientCurrency)
        {
            var joborder = await GetById(Id);

            // maybe add security check later here and autoMapper
            joborder.InvoicingNotes = InvoicingNotes;
            joborder.ClientPonumber = ClientPonumber;

            if (EarlyInvoiceDateTime != null)
            {
                joborder.EarlyInvoiceDateTime = EarlyInvoiceDateTime;
            }
            else
            {
                joborder.EarlyInvoiceDateTime = (DateTime?)null;
            }

            joborder.EarlyInvoiceByEmpId = EarlyInvoiceByEmpId;
            joborder.CustomerSpecificField1Value = CustomerSpecificField1Value;
            joborder.CustomerSpecificField2Value = CustomerSpecificField2Value;
            joborder.CustomerSpecificField3Value = CustomerSpecificField3Value;
            joborder.CustomerSpecificField4Value = CustomerSpecificField4Value;
            joborder.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
            joborder.LastModifiedByEmployeeId = lastModifiedByUserId;
            if (OrgHfmcodeBs != "-1") { joborder.OrgHfmcodeBs = OrgHfmcodeBs; }
            if (OrgHfmcodeIs != "-1") { joborder.OrgHfmcodeIs = OrgHfmcodeIs; }
            joborder.ClientCurrencyId = ClientCurrency;

            try
            {
                orderRepository.Update(joborder);
                await orderRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }


            return joborder;
        }

        public Dictionary<string, string> AllTemplatePaths(int JobOrderID, int OrgID, byte JobServerLocation, bool ForUser, DateTime? SubmittedDateTime = null)
        {
            Dictionary<string, string> AllTemplates = new Dictionary<string, string>();
            string JobPath = NetworkDirectoryPathForApp(JobOrderID, OrgID, JobServerLocation, ForUser, SubmittedDateTime);
            if (JobPath != "")
            {
                DirectoryInfo JobFolder = new DirectoryInfo(JobPath);
                string NetworkKeyClientPath = Path.Combine(JobFolder.Parent.FullName, "Key Client Info");
                string ExpectedAutomationFolder = Path.Combine(NetworkKeyClientPath, "Automation");
                var GlobalVars = new GlobalVariables();
                configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);

                if (Directory.Exists(ExpectedAutomationFolder) == false)
                {
                    ExpectedAutomationFolder = ExpectedAutomationFolder.Replace(GlobalVars.ParisJobDriveBaseDirectoryPathForApp, GlobalVars.SofiaJobDriveBaseDirectoryPathForApp);
                }
                else
                {
                    FileInfo[] sdltplFilesFound = new DirectoryInfo(ExpectedAutomationFolder).GetFiles("*.sdltpl");

                    if (sdltplFilesFound.Length == 0)
                    {
                        ExpectedAutomationFolder = ExpectedAutomationFolder.Replace(GlobalVars.ParisJobDriveBaseDirectoryPathForApp, GlobalVars.SofiaJobDriveBaseDirectoryPathForApp);
                    }
                }

                if (Directory.Exists(ExpectedAutomationFolder) == true)
                {
                    FileInfo[] sdltplFilesFound = new DirectoryInfo(ExpectedAutomationFolder).GetFiles("*.sdltpl");

                    if (sdltplFilesFound.Length > 0)
                    {
                        if (sdltplFilesFound.Length > 1)
                        {
                            AllTemplates.Add("Please select template", "Please select template");
                        }
                        foreach (FileInfo sdltpFile in sdltplFilesFound)
                        {
                            AllTemplates.Add(sdltpFile.Name, sdltpFile.FullName);
                        }
                    }
                    else
                    {
                        AllTemplates.Add("There is no Automation folder in the Organisation Key Client Info folder.", "There is no Automation folder in the Organisation Key Client Info folder.");
                    }
                }
            }

            return AllTemplates;
        }

        public async Task<IEnumerable<JobItemsDataTableViewModel>> GetJobItems(int JobOrderId)
        {
            var res = new List<JobItemsDataTableViewModel>();

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string connectionstring = configuration.GetConnectionString(GlobalVars.CurrentAppModeString);

            using (SqlConnection SQLConn = new SqlConnection(connectionstring))
            {
                SQLConn.Open();
                using var command = SQLConn.CreateCommand();
                command.CommandText = @"
                    SELECT [JobItems].[ID]
      ,[JobOrderID]      
	  ,[Source].[Name] [SourceLanguageName]
	  ,[Target].[Name] [TargetLanguageName]
	  ,[LS].[Name] [LanguageServiceName]
	  ,[JobItems].[SupplierCompletionDeadline]
	  ,[JobItems].[WeCompletedItemDateTime]
	  ,[JobItems].[SupplierSentWorkDateTime]
	  ,[JobItems].[SupplierAcceptedWorkDateTime]
	  ,[JobItems].[SupplierCompletedItemDateTime]
	  ,[JobItems].[SourceLanguageIANACode]
	  ,[JobItems].[TargetLanguageIANACode]
	  ,[JobItems].[SupplierIsClientReviewer]
	  ,[JobItems].[LinguisticSupplierOrClientReviewerID]
	  ,ISNUll(NULLIF([Suppliers].[AgencyOrTeamName], ''), ([Suppliers].[MainContactFirstName] + ' ' + [Suppliers].[MainContactSurname])) [SupplierName]
	  ,ISNULL([Contacts].[Name], '') [ContactName]
	  ,[ClientCurrency].[Prefix] [ClientCurrency]
	  ,CONVERT(decimal(38,2),ISNULL([JobItems].[ChargeToClient], 0.00)) [ChargeToClient]
	  ,ISNULL([SupplierCurrency].[Prefix], '') [SupplierCurrency]
	  ,CONVERT(decimal(38,2),ISNULL([JobItems].[PaymentToSupplier], 0.00)) [PaymentToSupplier]
	  ,ISNULL([JobItems].[MinimumSupplierChargeApplied], 0) [MinimumSupplierChargeApplied]
	  ,ISNULL([JobItems].[MarginAfterDiscountSurcharges], 0.00) [Margin]
	  ,ISNULL([JobItems].[WorkMinutes], 0) [WorkMinutes]
	  ,ISNULL([JobItems].[WordCountNew], 0) [NewClientWordCount]
	  ,ISNULL([JobItems].[WordCountNew] +  [JobItems].[WordCountFuzzyBand1] + ISNULL([JobItems].[WordCountFuzzyBand2], 0) + ISNULL([JobItems].[WordCountFuzzyBand3], 0) + ISNULL([JobItems].[WordCountFuzzyBand4], 0) + [JobItems].[WordCountExact] + [JobItems].[WordCountRepetitions] + [JobItems].[WordCountPerfectMatches] + ISNULL([JobItems].[WordCountClientSpecific], 0), 0) AS [TotalClientWordCount]  
	  ,ISNULL([JobItems].[SupplierWordCountNew], 0) [NewSupplierWordCount]
	  ,ISNULL([JobItems].[SupplierWordCountNew] +  [JobItems].[SupplierWordCountFuzzyBand1] + ISNULL([JobItems].[SupplierWordCountFuzzyBand2], 0) + ISNULL([JobItems].[SupplierWordCountFuzzyBand3], 0) + ISNULL([JobItems].[SupplierWordCountFuzzyBand4], 0) + [JobItems].[SupplierWordCountExact] + [JobItems].[SupplierWordCountRepetitions] + [JobItems].[SupplierWordCountPerfectMatches] + ISNULL([JobItems].[SupplierWordCountClientSpecific], 0), 0) AS [TotalSupplierWordCount]
      ,(CASE WHEN ([JobItems].[WeCompletedItemDateTime] IS NULL AND [JobItems].[SupplierSentWorkDateTime] IS NULL) THEN 'Not started'
	  WHEN [JobItems].[WeCompletedItemDateTime] IS NULL AND [JobItems].[SupplierSentWorkDateTime] IS NOT NULL AND [JobItems].[SupplierAcceptedWorkDateTime] IS NULL THEN 'Sent to supplier but not accepted yet'
	  WHEN [JobItems].[WeCompletedItemDateTime] IS NULL AND [JobItems].[SupplierSentWorkDateTime] IS NOT NULL AND [JobItems].[SupplierAcceptedWorkDateTime] IS NOT NULL AND [JobItems].[SupplierCompletedItemDateTime] IS NULL AND ([JobItems].[SupplierCompletionDeadline] IS NULL OR ([JobItems].[SupplierCompletionDeadline] IS NOT NULL AND (dbo.funcGetCurrentUKTime() < [JobItems].[SupplierCompletionDeadline]))) THEN 'In progress with supplier'
	  WHEN [JobItems].[WeCompletedItemDateTime] IS NULL AND [JobItems].[SupplierSentWorkDateTime] IS NOT NULL AND [JobItems].[SupplierAcceptedWorkDateTime] IS NOT NULL AND [JobItems].[SupplierCompletedItemDateTime] IS NOT NULL THEN 'Completed by supplier'
	  WHEN [JobItems].[WeCompletedItemDateTime] IS NOT NULL THEN 'Completed'
	  WHEN [JobItems].[WeCompletedItemDateTime] IS NULL AND [JobItems].[SupplierSentWorkDateTime] IS NOT NULL AND [JobItems].[SupplierAcceptedWorkDateTime] IS NOT NULL AND [JobItems].[SupplierCompletedItemDateTime] IS NULL AND ([JobItems].[SupplierCompletionDeadline] IS NOT NULL AND (dbo.funcGetCurrentUKTime() > [JobItems].[SupplierCompletionDeadline])) THEN 'Overdue by supplier' 
	  WHEN ([JobItems].[OurCompletionDeadline] IS NOT NULL AND [JobItems].[WeCompletedItemDateTime] IS NULL AND (dbo.funcGetCurrentUKTime() > [JobItems].[OurCompletionDeadline])) THEN 'Overdue' 
	  END) [Status]
	  ,(CASE WHEN [JobItems].[WeCompletedItemDateTime] IS NULL AND [JobItems].[SupplierSentWorkDateTime] IS NULL THEN 'white'
      WHEN [JobItems].[WeCompletedItemDateTime] IS NULL AND[JobItems].[SupplierSentWorkDateTime] IS NOT NULL AND[JobItems].[SupplierAcceptedWorkDateTime] IS NULL THEN '#C3C3C3'
      WHEN [JobItems].[WeCompletedItemDateTime] IS NULL AND[JobItems].[SupplierSentWorkDateTime] IS NOT NULL AND[JobItems].[SupplierAcceptedWorkDateTime] IS NOT NULL AND[JobItems].[SupplierCompletedItemDateTime] IS NULL AND([JobItems].[SupplierCompletionDeadline] IS NULL OR([JobItems].[SupplierCompletionDeadline] IS NOT NULL AND(dbo.funcGetCurrentUKTime() < [JobItems].[SupplierCompletionDeadline]))) THEN '#FFC90E'
      WHEN ([JobItems].[SupplierSentWorkDateTime] IS NOT NULL AND[JobItems].[SupplierAcceptedWorkDateTime] IS NOT NULL AND [JobItems].[SupplierCompletedItemDateTime] IS NOT NULL) OR [JobItems].[WeCompletedItemDateTime] IS NOT NULL THEN '#22B14C'
      WHEN [JobItems].[WeCompletedItemDateTime] IS NULL AND[JobItems].[SupplierSentWorkDateTime] IS NOT NULL AND[JobItems].[SupplierAcceptedWorkDateTime] IS NOT NULL AND[JobItems].[SupplierCompletedItemDateTime] IS NULL AND([JobItems].[SupplierCompletionDeadline] IS NOT NULL AND(dbo.funcGetCurrentUKTime() > [JobItems].[SupplierCompletionDeadline])) THEN '#FF0000'
      WHEN([JobItems].[OurCompletionDeadline] IS NOT NULL AND [JobItems].[WeCompletedItemDateTime] IS NULL AND (dbo.funcGetCurrentUKTime() > [JobItems].[OurCompletionDeadline])) THEN '#FF0000'
      END) [StatusColour]
	  ,(CASE WHEN [JobItems].[IsVisibleToClient] = 1 THEN 'checked'
	  ELSE ''
	  END) [Visible]
     ,(CASE WHEN [JobItems].[IsVisibleToClient] = 1 THEN '(Visible)'
	  ELSE '(Not visible)'
	  END) [VisibleStatus]
     ,[Source].[LanguageIANAcodeBeingDescribed] [SourceLanguageIANACode]
	 ,[Target].[LanguageIANAcodeBeingDescribed] [TargetLanguageIANACode]
	 ,[dbo].[funcGetInitialsFromFullName](ISNUll(NULLIF([Suppliers].[AgencyOrTeamName], ''), ([Suppliers].[MainContactFirstName] + ' ' + [Suppliers].[MainContactSurname]))) [SuppliersInitials]
	 ,[dbo].[funcGetInitialsFromFullName](ISNULL([Contacts].[Name], '')) [ContactsInitials], ISNULL([JobItems].[WebServiceClientStatusID], 0) [WebServiceClientStatusID],ISNULL([JobItems].[ExtranetClientStatusID], 0) [ExtranetClientStatusID]
  FROM [dbo].[JobItems]
  INNER JOIN [LocalLanguageInfo] [Source] ON
  [Source].[LanguageIANAcodeBeingDescribed] = [SourceLanguageIanacode] AND [Source].[LanguageIANAcode] = 'en'
  INNER JOIN [LocalLanguageInfo] [Target] ON
  [Target].[LanguageIANAcodeBeingDescribed] = [TargetLanguageIanacode] AND [Target].[LanguageIANAcode] = 'en'
  INNER JOIN [LanguageServices] [LS] ON
  [LS].[ID] = [LanguageServiceID]
  LEFT JOIN [LinguisticSuppliers] [Suppliers] ON
  [Suppliers].[ID] = [JobItems].[LinguisticSupplierOrClientReviewerID] AND [JobItems].[SupplierIsClientReviewer] = 0
  LEFT JOIN [Contacts] ON
  [Contacts].[ID] = [JobItems].[LinguisticSupplierOrClientReviewerID] AND [JobItems].[SupplierIsClientReviewer] = 1
  INNER JOIN [JobOrders] ON
  [JobOrders].[ID] = [JobItems].[JobOrderID]
  INNER JOIN [Currencies] [ClientCurrency] ON
  [ClientCurrency].[ID] = [JobOrders].[ClientCurrencyID]
  LEFT JOIN [Currencies] [SupplierCurrency] ON
  [SupplierCurrency].[ID] = [JobItems].[PaymentToSupplierCurrencyID]
  WHERE [DeletedDateTime] IS NULL
  AND [JobOrderID] = " + JobOrderId + @"
ORDER BY [SourceLanguageName] ASC, [TargetLanguageName] ASC, ISNULL([SupplierCompletionDeadline], ISNULL([OurCompletionDeadline], dbo.funcGetCurrentUKTime())) ASC, [JobItems].[ID] ASC";
                //ORDER BY [SourceLanguageName] ASC, [TargetLanguageName] ASC, [OurCompletionDeadline] ASC, ISNULL([SupplierCompletionDeadline], dbo.funcGetCurrentUKTime()) ASC, [JobItems].[ID] ASC";

                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);

                SQLConn.Close();
                SQLConn.Dispose();
            }
            return res;
        }

        private async Task<List<JobItemsDataTableViewModel>> BindPropertiesForDataTable(System.Data.Common.DbDataReader result, List<JobItemsDataTableViewModel> res)
        {
            while (await result.ReadAsync())
            {
                var a = new JobItemsDataTableViewModel();

                a.Id = await result.GetFieldValueAsync<int>(0);
                a.JobOrderId = await result.GetFieldValueAsync<int>(1);
                a.SourceLanguageName = await result.GetFieldValueAsync<string>(2);
                a.TargetLanguageName = await result.GetFieldValueAsync<string>(3);
                a.LanguageServiceName = await result.GetFieldValueAsync<string>(4);

                if (result.IsDBNull(5))
                    a.SupplierCompletionDeadline = null;
                else
                    a.SupplierCompletionDeadline = await result.GetFieldValueAsync<DateTime?>(5);

                if (result.IsDBNull(6))
                    a.WeCompletedItemDateTime = null;
                else
                    a.WeCompletedItemDateTime = await result.GetFieldValueAsync<DateTime?>(6);

                if (result.IsDBNull(7))
                    a.SupplierSentWorkDateTime = null;
                else
                    a.SupplierSentWorkDateTime = await result.GetFieldValueAsync<DateTime?>(7);

                if (result.IsDBNull(8))
                    a.SupplierAcceptedWorkDateTime = null;
                else
                    a.SupplierAcceptedWorkDateTime = await result.GetFieldValueAsync<DateTime?>(8);

                if (result.IsDBNull(9))
                    a.SupplierCompletedItemDateTime = null;
                else
                    a.SupplierCompletedItemDateTime = await result.GetFieldValueAsync<DateTime?>(9);

                a.SourceLanguageIANACode = await result.GetFieldValueAsync<string>(10);
                a.TargetLanguageIANACode = await result.GetFieldValueAsync<string>(11);
                a.SupplierIsClientReviewer = await result.GetFieldValueAsync<Boolean?>(12);

                if (result.IsDBNull(13))
                    a.LinguisticSupplierOrClientReviewerId = null;
                else
                    a.LinguisticSupplierOrClientReviewerId = await result.GetFieldValueAsync<int?>(13);

                if (result.IsDBNull(14))
                    a.SupplierName = null;
                else
                    a.SupplierName = await result.GetFieldValueAsync<string>(14);

                if (result.IsDBNull(15))
                    a.ContactName = null;
                else
                    a.ContactName = await result.GetFieldValueAsync<string>(15);

                a.ClientCurrency = await result.GetFieldValueAsync<string>(16);
                a.ChargeToClient = await result.GetFieldValueAsync<decimal?>(17);
                a.SupplierCurrency = await result.GetFieldValueAsync<string>(18);
                a.PaymentToSupplier = await result.GetFieldValueAsync<decimal?>(19);
                a.MinimumSupplierChargeApplied = await result.GetFieldValueAsync<Boolean?>(20);
                a.Margin = await result.GetFieldValueAsync<decimal?>(21);
                a.WorkMinutes = await result.GetFieldValueAsync<int>(22);
                a.NewClientWordCount = await result.GetFieldValueAsync<int>(23);
                a.TotalClientWordCount = await result.GetFieldValueAsync<int>(24);
                a.NewSupplierWordCount = await result.GetFieldValueAsync<int>(25);
                a.TotalSupplierWordCount = await result.GetFieldValueAsync<int>(26);
                a.Status = await result.GetFieldValueAsync<string>(27);
                a.StatusColour = await result.GetFieldValueAsync<string>(28);
                a.Visible = await result.GetFieldValueAsync<string>(29);
                a.VisibleStatus = await result.GetFieldValueAsync<string>(30);
                a.SourceLanguageIANACode = await result.GetFieldValueAsync<string>(31);
                a.TargetLanguageIANACode = await result.GetFieldValueAsync<string>(32);
                a.WebServiceClientStatusId = await result.GetFieldValueAsync<short>(35);
                a.ExtranetClientStatusId = await result.GetFieldValueAsync<short>(36);

                if (result.IsDBNull(33))
                    a.SuppliersInitials = null;
                else
                    a.SuppliersInitials = await result.GetFieldValueAsync<string>(33);

                if (result.IsDBNull(34))
                    a.ContactsInitials = null;
                else
                    a.ContactsInitials = await result.GetFieldValueAsync<string>(34);

                if (a.StatusColour == "white")
                    a.StatusBGColour = "white";
                else if (a.StatusColour == "#C3C3C3")
                    a.StatusBGColour = "#F2F2F2";
                else if (a.StatusColour == "#FFC90E")
                    a.StatusBGColour = "#FFF2CC";
                else if (a.StatusColour == "#22B14C")
                    a.StatusBGColour = "#E2F0D9";
                else if (a.StatusColour == "#FF0000")
                    a.StatusBGColour = "#FFC9C9";
                else
                    a.StatusBGColour = "white";

                res.Add(a);

            }
            return res;
        }

        public async Task<IEnumerable<JobOrderDataTableViewModel>> GetAllJobOrdersForDataObjectAndType(int dataObjectId, int dataTypeId, int pageNumber, int pageSize, string searchTerm, int columnToOrderBy, string orderDirection)
        {
            var orders = new List<JobOrderDataTableViewModel>();
            string innerJoinSection = String.Empty;
            string whereSection = String.Empty;

            var columns = new Dictionary<int, string>();
            columns.Add(0, "JobID");
            columns.Add(1, "OriginatedFromEnquiryID");
            columns.Add(2, "JobName");
            columns.Add(3, "ContactName");
            columns.Add(4, "SourceLang");
            columns.Add(5, "TargetLang");
            columns.Add(6, "OrderStatus");
            columns.Add(7, "SubmittedDateTime");
            columns.Add(8, "OverallDeliveryDeadline");
            columns.Add(9, "AnticipatedGrossMarginPercentage");
            columns.Add(10, "Value");
            columns.Add(11, "OverallSterlingPaymentToSuppliers");
            columns.Add(12, "OverallCompletedDateTime");
            columns.Add(13, "SourceLangsCombined");
            columns.Add(14, "TargetLangsCombined");

            if (dataTypeId == ((int)Enumerations.DataObjectTypes.Org))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";
                whereSection = " where Orgs.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.Contact))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Contacts.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where OrgGroups.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null";
            }

            string newQuery = @"select * from(
select JobOrders.ID as JobID, OriginatedFromEnquiryID, JobOrders.JobName, 
JobOrders.OverallDeliveryDeadline, JobOrders.SubmittedDateTime,
(Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'end) as OrderStatus,
Orgs.ID as OrgId, Orgs.OrgName, OrgGroupID, OrgGroups.Name as GroupName, Contacts.ID as ContactId, Contacts.Name as ContactName,
ISNULL(OverallChargeToClient, 0) as Value, JobOrders.AnticipatedGrossMarginPercentage,
(select dbo.funcGetSourceLangOrLangsFullStringJobOrderID(JobOrders.ID)) as SourceLang,
(select dbo.funcGetTargetLangOrLangsFullStringJobOrderID(JobOrders.ID)) as TargetLang,
isnull((select Currencies.Prefix from Currencies where ID = Joborders.ClientCurrencyID),'(none)') as Currency,
JobOrders.OverallSterlingPaymentToSuppliers, JobOrders.OverallCompletedDateTime,
(dbo.funcGetSourceLangOrLangsFullStringJobOrderIDAllLanguages(JobOrders.ID)) as SourceLangsCombined,
(dbo.funcGetTargetLangOrLangsFullStringJobOrderIDAllLanguages(JobOrders.ID)) as TargetLangsCombined,
JobOrders.ClientPONumber, JobOrders.Priority

from JobOrders " + innerJoinSection + whereSection + @"
 ) table1

 where (SourceLang + TargetLang + JobName + [OrderStatus] + OrgName) LIKE N'%" + searchTerm + @"%'

	ORDER BY table1." + columns.FirstOrDefault(x => x.Key == columnToOrderBy).Value + " " + orderDirection + @"

	OFFSET " + pageNumber + @" ROWS
    FETCH NEXT " + pageSize + " ROWS ONLY";

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string connectionstring = configuration.GetConnectionString(GlobalVars.CurrentAppModeString);

            using (SqlConnection SQLConn = new SqlConnection(connectionstring))
            {
                SQLConn.Open();
                using var command = SQLConn.CreateCommand();
                command.CommandText = newQuery;
                using var result = command.ExecuteReader();


                while (await result.ReadAsync())
                {
                    orders.Add(new JobOrderDataTableViewModel()
                    {
                        JobOrderId = await result.GetFieldValueAsync<int>(0),
                        EnquiryId = await result.GetFieldValueAsync<int>(1),
                        JobOrderName = await result.GetFieldValueAsync<string>(2),
                        DeliveryDeadline = await result.GetFieldValueAsync<DateTime>(3),
                        SubmittedDate = await result.GetFieldValueAsync<DateTime>(4),
                        Status = await result.GetFieldValueAsync<string>(5),
                        OrgId = await result.GetFieldValueAsync<int>(6),
                        OrgName = await result.GetFieldValueAsync<string>(7),
                        OrgGroupId = await result.GetFieldValueAsync<int>(8),
                        OrgGroupName = await result.GetFieldValueAsync<string>(9),
                        ContactId = await result.GetFieldValueAsync<int>(10),
                        ContactName = await result.GetFieldValueAsync<string>(11),
                        Value = await result.GetFieldValueAsync<decimal>(12),
                        Margin = await result.GetFieldValueAsync<decimal>(13),
                        SourceLang = await result.GetFieldValueAsync<string>(14),
                        TargetLang = await result.GetFieldValueAsync<string>(15),
                        Currency = await result.GetFieldValueAsync<string>(16),
                        SupplierCost = await result.GetFieldValueAsync<decimal>(17),
                        CompletionDate = await result.IsDBNullAsync(18) ? null : await result.GetFieldValueAsync<DateTime>(18),
                        SourceLangsCombined = await result.GetFieldValueAsync<string>(19),
                        TargetLangsCombined = await result.GetFieldValueAsync<string>(20),
                        PONumber = await result.IsDBNullAsync(21) ? "" : await result.GetFieldValueAsync<string>(21),
                        Priority = await result.IsDBNullAsync(22) ? Convert.ToByte(0) : await result.GetFieldValueAsync<byte>(22),
                        Progress = String.Format("{0:0.0#}", GetJobOrderProgress(await result.GetFieldValueAsync<int>(0)).Result) + "%"
                    });
                }

                SQLConn.Close();
                SQLConn.Dispose();
            }
            return orders;
        }

        public async Task<(int, int)> GetAllJobOrdersCountForDataObjectAndTypeWithDateRange(int dataObjectId, int dataTypeId, string searchTerm, string startDate, string endDate)
        {
            int totalRecords = 0;
            int filteredRecords = 0;

            startDate = startDate.Replace("12:00:00.000", "00:00:00.000");
            endDate = endDate.Replace("12:00:00.000", "23:59:59.998");

            string query = String.Empty;
            string selectSection = String.Empty;
            string innerJoinSection = String.Empty;
            string whereSection = String.Empty;

            selectSection = @"select COUNT(*) from JobOrders";

            //build rest of the query
            if (dataTypeId == ((int)Enumerations.DataObjectTypes.Org))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Orgs.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= '" + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.Contact))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Contacts.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= '" + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where OrgGroups.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= '" + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }

            var newQuery = @"select 
	(select count(*) 
		from JobOrders " + innerJoinSection + whereSection + @") as totalRecords,

(select COUNT(jobCount) from 
	(select count(*) as jobCount , JobName, OrgName,
		(Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
			when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
			when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'end) as OrderStatus,
		(select dbo.funcGetSourceLangOrLangsFullStringJobOrderID(JobOrders.ID)) as SourceLang,
		(select dbo.funcGetTargetLangOrLangsFullStringJobOrderID(JobOrders.ID)) as TargetLang
		
		from JobOrders " + innerJoinSection + whereSection + @"
		GROUP BY JobOrders.ID, JobName, OrgName, JobOrders.OverallCompletedDateTime, JobOrders.OverallDeliveryDeadline) table1

		where (SourceLang + TargetLang + JobName + [OrderStatus] + OrgName) LIKE N'%" + searchTerm + @"%'
) as filteredRecords";

            query = selectSection + innerJoinSection + whereSection;

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);

            string connectionstring = configuration.GetConnectionString(GlobalVars.CurrentAppModeString);

            using (SqlConnection SQLConn = new SqlConnection(connectionstring))
            {
                SQLConn.Open();
                using var command = SQLConn.CreateCommand();
                command.CommandText = newQuery;
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    totalRecords = await result.GetFieldValueAsync<int>(0);
                    filteredRecords = await result.GetFieldValueAsync<int>(1);
                }

                SQLConn.Close();
                SQLConn.Dispose();
            }
            return (totalRecords, filteredRecords);
        }
        public async Task<IEnumerable<JobOrderDataTableViewModel>> GetAllJobOrdersForDataObjectAndTypeWithDateRange(int dataObjectId, int dataTypeId, int pageNumber, int pageSize, string searchTerm, int columnToOrderBy, string orderDirection, string startDate, string endDate)
        {
            var orders = new List<JobOrderDataTableViewModel>();
            string innerJoinSection = String.Empty;
            string whereSection = String.Empty;

            startDate = startDate.Replace("12:00:00.000", "00:00:00.000");
            endDate = endDate.Replace("12:00:00.000", "23:59:59.998");

            var columns = new Dictionary<int, string>();
            columns.Add(0, "JobID");
            columns.Add(1, "OriginatedFromEnquiryID");
            columns.Add(2, "JobName");
            columns.Add(3, "ContactName");
            columns.Add(4, "SourceLang");
            columns.Add(5, "TargetLang");
            columns.Add(6, "OrderStatus");
            columns.Add(7, "SubmittedDateTime");
            columns.Add(8, "OverallDeliveryDeadline");
            columns.Add(9, "AnticipatedGrossMarginPercentage");
            columns.Add(10, "Value");
            columns.Add(11, "OverallSterlingPaymentToSuppliers");
            columns.Add(12, "Completed");
            columns.Add(13, "SourceLangsCombined");
            columns.Add(14, "TargetLangsCombined");

            if (dataTypeId == ((int)Enumerations.DataObjectTypes.Org))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Orgs.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= ' " + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.Contact))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Contacts.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= '" + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where OrgGroups.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= '" + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }

            string newQuery = @"select * from(
select JobOrders.ID as JobID, OriginatedFromEnquiryID, JobOrders.JobName, 
JobOrders.OverallDeliveryDeadline, JobOrders.SubmittedDateTime,
(Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'end) as OrderStatus,
Orgs.ID as OrgId, Orgs.OrgName, OrgGroupID, OrgGroups.Name as GroupName, Contacts.ID as ContactId, Contacts.Name as ContactName,
ISNULL(OverallChargeToClient, 0) as Value, JobOrders.AnticipatedGrossMarginPercentage,
(select dbo.funcGetSourceLangOrLangsFullStringJobOrderID(JobOrders.ID)) as SourceLang,
(select dbo.funcGetTargetLangOrLangsFullStringJobOrderID(JobOrders.ID)) as TargetLang,
isnull((select Currencies.Prefix from Currencies where ID = Joborders.ClientCurrencyID),'(none)') as Currency,
JobOrders.OverallSterlingPaymentToSuppliers, JobOrders.OverallCompletedDateTime,
(dbo.funcGetSourceLangOrLangsFullStringJobOrderIDAllLanguages(JobOrders.ID)) as SourceLangsCombined,
(dbo.funcGetTargetLangOrLangsFullStringJobOrderIDAllLanguages(JobOrders.ID)) as TargetLangsCombined,
JobOrders.ClientPONumber, JobOrders.Priority

from JobOrders " + innerJoinSection + whereSection + @"
 ) table1

 where (SourceLang + TargetLang + JobName + [OrderStatus] + OrgName) LIKE N'%" + searchTerm + @"%'

	ORDER BY table1." + columns.FirstOrDefault(x => x.Key == columnToOrderBy).Value + " " + orderDirection + @"

	OFFSET " + pageNumber + @" ROWS
    FETCH NEXT " + pageSize + " ROWS ONLY";

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string connectionstring = configuration.GetConnectionString(GlobalVars.CurrentAppModeString);

            using (SqlConnection SQLConn = new SqlConnection(connectionstring))
            {
                SQLConn.Open();
                using var command = SQLConn.CreateCommand();
                command.CommandText = newQuery;
                using var result = command.ExecuteReader();

                var priorityDict = new Dictionary<byte, string>();
                priorityDict.Add(0, "None");
                priorityDict.Add(1, "High");
                priorityDict.Add(2, "Medium");
                priorityDict.Add(3, "Low");

                while (await result.ReadAsync())
                {
                    orders.Add(new JobOrderDataTableViewModel()
                    {
                        JobOrderId = await result.GetFieldValueAsync<int>(0),
                        EnquiryId = await result.GetFieldValueAsync<int>(1),
                        JobOrderName = await result.GetFieldValueAsync<string>(2),
                        DeliveryDeadline = await result.GetFieldValueAsync<DateTime>(3),
                        SubmittedDate = await result.GetFieldValueAsync<DateTime>(4),
                        Status = await result.GetFieldValueAsync<string>(5),
                        OrgId = await result.GetFieldValueAsync<int>(6),
                        OrgName = await result.GetFieldValueAsync<string>(7),
                        OrgGroupId = await result.GetFieldValueAsync<int>(8),
                        OrgGroupName = await result.GetFieldValueAsync<string>(9),
                        ContactId = await result.GetFieldValueAsync<int>(10),
                        ContactName = await result.GetFieldValueAsync<string>(11),
                        Value = await result.GetFieldValueAsync<decimal>(12),
                        Margin = await result.GetFieldValueAsync<decimal>(13),
                        SourceLang = await result.GetFieldValueAsync<string>(14) == "0 source languages" ? "--" : await result.GetFieldValueAsync<string>(14),
                        TargetLang = await result.GetFieldValueAsync<string>(15) == "0 target languages" ? "--" : await result.GetFieldValueAsync<string>(15),
                        Currency = await result.GetFieldValueAsync<string>(16),
                        Cost = await result.GetFieldValueAsync<string>(16) + decimal.Round(await result.GetFieldValueAsync<decimal>(17), 2, MidpointRounding.AwayFromZero),
                        CompletionDate = await result.IsDBNullAsync(18) ? null : await result.GetFieldValueAsync<DateTime>(18),
                        SourceLangsCombined = await result.GetFieldValueAsync<string>(19),
                        TargetLangsCombined = await result.GetFieldValueAsync<string>(20),
                        PONumber = await result.IsDBNullAsync(21) ? "" : await result.GetFieldValueAsync<string>(21),
                        Priority = await result.IsDBNullAsync(22) ? Convert.ToByte(0) : await result.GetFieldValueAsync<byte>(22),
                        Progress = String.Format("{0:0.0#}", GetJobOrderProgress(await result.GetFieldValueAsync<int>(0)).Result) + "%",
                        PriorityName = await result.IsDBNullAsync(22) ? priorityDict.Where(x => x.Key == Convert.ToByte(0)).FirstOrDefault().Value : priorityDict.Where(x => x.Key == result.GetFieldValueAsync<byte>(22).Result).FirstOrDefault().Value,
                    });
                }

                SQLConn.Close();
                SQLConn.Dispose();
            }
            return orders;
        }

        public async Task<String> GetCompletionStatusStringForJobOrder(int jobOrderId, string languageIANACode)
        {
            string stringToReturn = "";
            var thisJobOrder = await orderRepository.All().Where(o => o.Id == jobOrderId).FirstOrDefaultAsync();

            var AllJobItems = jobitemsRepo.All().Where(i => i.JobOrderId == jobOrderId && i.DeletedDateTime == null).Count();

            var completedJobItems = jobitemsRepo.All().Where(i => i.JobOrderId == jobOrderId && i.DeletedDateTime == null &&
                                                               (i.WeCompletedItemDateTime != null || (i.LanguageServiceId == 21 && i.SupplierCompletedItemDateTime != null)
                                                            || (i.ExtranetClientStatusId == 2 || i.ExtranetClientStatusId == 3))).Count();


            if (completedJobItems == AllJobItems && AllJobItems == 0 && thisJobOrder.OverallCompletedDateTime == null)
            {
                stringToReturn = miscResourceService.GetMiscResourceByName("JobOrderStatusInProgress", languageIANACode).Result.StringContent;
            }
            else if (completedJobItems == AllJobItems)
            {
                stringToReturn = miscResourceService.GetMiscResourceByName("JobOrderStatusFullyComplete", languageIANACode).Result.StringContent;
            }
            else if (completedJobItems == 0)
            {
                stringToReturn = miscResourceService.GetMiscResourceByName("JobOrderStatusInProgress", languageIANACode).Result.StringContent;
            }
            else if (completedJobItems < AllJobItems)
            {
                //get  miscResources string for the language and return the STringCOntent
                stringToReturn = miscResourceService.GetMiscResourceByName("JobOrderStatusPartiallyComplete", languageIANACode).Result.StringContent;
                //stringToReturn = "";
            }

            return stringToReturn;

        }

        public async Task<(int, int)> GetAllJobOrdersCountForDataObjectAndType(int dataObjectId, int dataTypeId, string searchTerm)
        {
            int totalRecords = 0;
            int filteredRecords = 0;

            string query = String.Empty;
            string selectSection = String.Empty;
            string innerJoinSection = String.Empty;
            string whereSection = String.Empty;

            selectSection = @"select COUNT(*) from JobOrders";

            //build rest of the query
            if (dataTypeId == ((int)Enumerations.DataObjectTypes.Org))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Orgs.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.Contact))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Contacts.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where OrgGroups.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null";
            }

            var newQuery = @"select 
	(select count(*) 
		from JobOrders " + innerJoinSection + whereSection + @") as totalRecords,

(select count(*) 
		from JobOrders " + innerJoinSection + whereSection + @") as filteredRecords";

            query = selectSection + innerJoinSection + whereSection;

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);

            string connectionstring = configuration.GetConnectionString(GlobalVars.CurrentAppModeString);

            using (SqlConnection SQLConn = new SqlConnection(connectionstring))
            {
                SQLConn.Open();
                using var command = SQLConn.CreateCommand();
                command.CommandText = newQuery;
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    totalRecords = await result.GetFieldValueAsync<int>(0);
                    filteredRecords = await result.GetFieldValueAsync<int>(1);
                }

                SQLConn.Close();
                SQLConn.Dispose();
            }
            return (totalRecords, filteredRecords);
        }

        public async Task<List<int>> GetAllOpenJobOrdersForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();



            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All().Where(o => o.DeletedDate == null),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                var result = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId })
                            .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                            .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                            o => o.order.ContactId,
                            c => c.Id,
                            (o, c) => new { OrderId = o.order.Id, orgId = c.OrgId })
                            .Join(orgRepo.All().Where(o => o.DeletedDate == null),
                            c => c.orgId,
                            o => o.Id,
                            (c, o) => new { OrderId = c.OrderId, groupId = o.OrgGroupId }).Where(e => e.groupId == groupID)
                            .Select(e => e.OrderId).Distinct().ToListAsync();

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                List<int> result = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                           .Select(o => new { o.Id, o.ContactId })
                            .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                 o => o.Id,
                                 ji => ji,
                                 (o, ji) => new { order = o })
                                  .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                                   o => o.order.ContactId,
                                   c => c.Id,
                                   (o, c) => new { OrderId = o.order.Id, orgId = c.OrgId }).Where(e => e.orgId == orgID)
                                  .Select(e => e.OrderId).Distinct().ToListAsync();

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                var result = await orderRepository.All().Where(o => o.DeletedDate == null && o.ContactId == clientContactID && o.OverallCompletedDateTime == null)
                             .Select(o => o.Id)
                              .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o,
                                  ji => ji,
                                  (o, ji) => new { order = (int)o }).Select(o => o.order).Distinct().ToListAsync();

                return result;
            }

        }


        public async Task<int> GetNumberOfOpenJobOrdersForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();



            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All().Where(o => o.DeletedDate == null),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                int result = orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId })
                            .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                            .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                            o => o.order.ContactId,
                            c => c.Id,
                            (o, c) => new { OrderId = o.order.Id, orgId = c.OrgId })
                            .Join(orgRepo.All().Where(o => o.DeletedDate == null),
                            c => c.orgId,
                            o => o.Id,
                            (c, o) => new { OrderId = c.OrderId, groupId = o.OrgGroupId }).Where(e => e.groupId == groupID)
                            .Select(e => e.OrderId).Distinct().Count();

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                int result = orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId })
                             .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                                   .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                                    o => o.order.ContactId,
                                    c => c.Id,
                                    (o, c) => new { OrderId = o.order.Id, orgId = c.OrgId }).Where(e => e.orgId == orgID)
                                   .Select(e => e.OrderId).Distinct().Count();

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                int result = orderRepository.All().Where(o => o.DeletedDate == null && o.ContactId == clientContactID && o.OverallCompletedDateTime == null)
                             .Select(o => o.Id)
                              .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o,
                                  ji => ji,
                                  (o, ji) => new { order = o }).Distinct().Count();

                return result;
            }

        }

        public async Task<IEnumerable<JobOrderDataTableViewModel>> GetAllJobOrdersForOrderStatusPage(int dataObjectId, int dataTypeId, int pageNumber, int pageSize, string searchTerm, int columnToOrderBy, string orderDirection, string startDate, string endDate)
        {
            var orders = new List<JobOrderDataTableViewModel>();
            string innerJoinSection = String.Empty;
            string whereSection = String.Empty;

            startDate = startDate.Replace("12:00:00.000", "00:00:00.000");
            endDate = endDate.Replace("12:00:00.000", "23:59:59.998");

            var columns = new Dictionary<int, string>();
            columns.Add(0, "ContactName");
            columns.Add(1, "JobID");
            columns.Add(2, "SubmittedDateTime");
            columns.Add(3, "JobName");
            columns.Add(4, "ClientPONumber");
            columns.Add(5, "SourceLang");
            columns.Add(6, "TargetLang");
            columns.Add(7, "OverallDeliveryDeadline");
            columns.Add(8, "Value");
            columns.Add(9, "Priority");
            columns.Add(10, "OrderStatus");
            //columns.Add(12, "Completed");
            //columns.Add(13, "SourceLangsCombined");
            //columns.Add(14, "TargetLangsCombined");

            if (dataTypeId == ((int)Enumerations.DataObjectTypes.Org))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Orgs.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= ' " + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.Contact))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Contacts.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= '" + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where OrgGroups.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= '" + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }

            string newQuery = @"select * from(
select JobOrders.ID as JobID, OriginatedFromEnquiryID, JobOrders.JobName, 
JobOrders.OverallDeliveryDeadline, JobOrders.SubmittedDateTime,
(Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'end) as OrderStatus,
Orgs.ID as OrgId, Orgs.OrgName, OrgGroupID, OrgGroups.Name as GroupName, Contacts.ID as ContactId, Contacts.Name as ContactName,
ISNULL(OverallChargeToClient, 0) as Value, JobOrders.AnticipatedGrossMarginPercentage,
(select dbo.funcGetSourceLangOrLangsFullStringJobOrderID(JobOrders.ID)) as SourceLang,
(select dbo.funcGetTargetLangOrLangsFullStringJobOrderID(JobOrders.ID)) as TargetLang,
isnull((select Currencies.Prefix from Currencies where ID = Joborders.ClientCurrencyID),'(none)') as Currency,
JobOrders.OverallSterlingPaymentToSuppliers, JobOrders.OverallCompletedDateTime,
(dbo.funcGetSourceLangOrLangsFullStringJobOrderIDAllLanguages(JobOrders.ID)) as SourceLangsCombined,
(dbo.funcGetTargetLangOrLangsFullStringJobOrderIDAllLanguages(JobOrders.ID)) as TargetLangsCombined,
JobOrders.ClientPONumber, JobOrders.Priority

from JobOrders " + innerJoinSection + whereSection + @"
 ) table1

WHERE (ContactName LIKE N'%" + searchTerm + @"%' OR
       CAST(JobID as nvarchar) LIKE N'%" + searchTerm + @"%' OR
       (SUBSTRING(DATENAME(weekday, SubmittedDateTime), 0, 4)  + ' ' + DATENAME(DAY, SubmittedDateTime) + ' ' + DATENAME(MONTH, SubmittedDateTime) + ' ' + DATENAME(YEAR, SubmittedDateTime)) LIKE N'%" + searchTerm + @"%' OR
       JobName LIKE N'%" + searchTerm + @"%' OR
       ClientPONumber LIKE N'%" + searchTerm + @"%' OR
       SourceLang LIKE N'%" + searchTerm + @"%' OR
       TargetLang LIKE N'%" + searchTerm + @"%' OR
       (SUBSTRING(DATENAME(weekday, OverallDeliveryDeadline), 0, 4)  + ' ' + DATENAME(DAY, OverallDeliveryDeadline) + ' ' + DATENAME(MONTH, OverallDeliveryDeadline) + ' ' + DATENAME(YEAR, OverallDeliveryDeadline)) LIKE N'%" + searchTerm + @"%' OR
       CAST(Value as nvarchar) LIKE N'%" + searchTerm + @"%' OR 
       OrderStatus LIKE N'%" + searchTerm + @"%')

	ORDER BY table1." + columns.FirstOrDefault(x => x.Key == columnToOrderBy).Value + " " + orderDirection + @"

	OFFSET " + pageNumber + @" ROWS
    FETCH NEXT " + pageSize + " ROWS ONLY";

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string connectionstring = configuration.GetConnectionString(GlobalVars.CurrentAppModeString);

            using (SqlConnection SQLConn = new SqlConnection(connectionstring))
            {
                SQLConn.Open();
                using var command = SQLConn.CreateCommand();
                command.CommandText = newQuery;
                using var result = command.ExecuteReader();

                var priorityDict = new Dictionary<byte, string>();
                priorityDict.Add(0, "None");
                priorityDict.Add(1, "High");
                priorityDict.Add(2, "Medium");
                priorityDict.Add(3, "Low");

                while (await result.ReadAsync())
                {
                    orders.Add(new JobOrderDataTableViewModel()
                    {
                        JobOrderId = await result.GetFieldValueAsync<int>(0),
                        EnquiryId = await result.GetFieldValueAsync<int>(1),
                        JobOrderName = await result.GetFieldValueAsync<string>(2),
                        DeliveryDeadline = await result.GetFieldValueAsync<DateTime>(3),
                        SubmittedDate = await result.GetFieldValueAsync<DateTime>(4),
                        Status = await GetCompletionStatusStringForJobOrder(result.GetFieldValueAsync<int>(0).Result, "en"),
                        OrgId = await result.GetFieldValueAsync<int>(6),
                        OrgName = await result.GetFieldValueAsync<string>(7),
                        OrgGroupId = await result.GetFieldValueAsync<int>(8),
                        OrgGroupName = await result.GetFieldValueAsync<string>(9),
                        ContactId = await result.GetFieldValueAsync<int>(10),
                        ContactName = await result.GetFieldValueAsync<string>(11),
                        Value = await result.GetFieldValueAsync<decimal>(12),
                        Margin = await result.GetFieldValueAsync<decimal>(13),
                        SourceLang = await result.GetFieldValueAsync<string>(14) == "0 source languages" ? "--" : await result.GetFieldValueAsync<string>(14),
                        TargetLang = await result.GetFieldValueAsync<string>(15) == "0 target languages" ? "--" : await result.GetFieldValueAsync<string>(15),
                        Currency = await result.GetFieldValueAsync<string>(16),
                        Cost = await result.GetFieldValueAsync<string>(16) + decimal.Round(await result.GetFieldValueAsync<decimal>(12), 2, MidpointRounding.AwayFromZero).ToString("N2"),
                        CompletionDate = await result.IsDBNullAsync(18) ? null : await result.GetFieldValueAsync<DateTime>(18),
                        SourceLangsCombined = await result.GetFieldValueAsync<string>(19),
                        TargetLangsCombined = await result.GetFieldValueAsync<string>(20),
                        PONumber = await result.IsDBNullAsync(21) ? "" : await result.GetFieldValueAsync<string>(21),
                        Priority = await result.IsDBNullAsync(22) ? Convert.ToByte(0) : await result.GetFieldValueAsync<byte>(22),
                        Progress = String.Format("{0:0.0#}", GetJobOrderProgress(await result.GetFieldValueAsync<int>(0)).Result) + "%",
                        PriorityName = await result.IsDBNullAsync(22) ? priorityDict.Where(x => x.Key == Convert.ToByte(0)).FirstOrDefault().Value : priorityDict.Where(x => x.Key == result.GetFieldValueAsync<byte>(22).Result).FirstOrDefault().Value,
                    });
                }

                SQLConn.Close();
                SQLConn.Dispose();
            }
            return orders;
        }

        public async Task<(int, int)> GetAllJobOrdersCountForOrderStatusPage(int dataObjectId, int dataTypeId, string searchTerm, string startDate, string endDate)
        {
            int totalRecords = 0;
            int filteredRecords = 0;

            startDate = startDate.Replace("12:00:00.000", "00:00:00.000");
            endDate = endDate.Replace("12:00:00.000", "23:59:59.998");

            string query = String.Empty;
            string selectSection = String.Empty;
            string innerJoinSection = String.Empty;
            string whereSection = String.Empty;

            selectSection = @"select COUNT(*) from JobOrders";

            //build rest of the query
            if (dataTypeId == ((int)Enumerations.DataObjectTypes.Org))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Orgs.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= '" + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.Contact))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Contacts.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= '" + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where OrgGroups.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and JobOrders.SubmittedDateTime >= '" + startDate + "' and JobOrders.SubmittedDateTime <= '" + endDate + "'";
            }

            var newQuery = @"select 
	(select count(*) 
		from JobOrders " + innerJoinSection + whereSection + @") as totalRecords,

(select COUNT(jobCount) from 
	(select count(*) as jobCount , JobName, Contacts.Name as ContactName,
        JobOrders.ID as JobID, JobOrders.SubmittedDateTime, JobOrders.OverallDeliveryDeadline,
        JobOrders.ClientPONumber, ISNULL(OverallChargeToClient, 0) as Value,
		(Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
			when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
			when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'end) as OrderStatus,
		(select dbo.funcGetSourceLangOrLangsFullStringJobOrderID(JobOrders.ID)) as SourceLang,
		(select dbo.funcGetTargetLangOrLangsFullStringJobOrderID(JobOrders.ID)) as TargetLang
		
		from JobOrders " + innerJoinSection + whereSection + @"
		GROUP BY JobOrders.ID, JobName, Contacts.Name, JobOrders.SubmittedDateTime, OverallChargeToClient,  JobOrders.OverallCompletedDateTime, JobOrders.OverallDeliveryDeadline, JobOrders.ClientPONumber) table1

		WHERE (ContactName LIKE N'%" + searchTerm + @"%' OR
       CAST(JobID as nvarchar) LIKE N'%" + searchTerm + @"%' OR
       (SUBSTRING(DATENAME(weekday, SubmittedDateTime), 0, 4)  + ' ' + DATENAME(DAY, SubmittedDateTime) + ' ' + DATENAME(MONTH, SubmittedDateTime) + ' ' + DATENAME(YEAR, SubmittedDateTime)) LIKE N'%" + searchTerm + @"%' OR
       JobName LIKE N'%" + searchTerm + @"%' OR
       ClientPONumber LIKE N'%" + searchTerm + @"%' OR
       SourceLang LIKE N'%" + searchTerm + @"%' OR
       TargetLang LIKE N'%" + searchTerm + @"%' OR
       (SUBSTRING(DATENAME(weekday, OverallDeliveryDeadline), 0, 4)  + ' ' + DATENAME(DAY, OverallDeliveryDeadline) + ' ' + DATENAME(MONTH, OverallDeliveryDeadline) + ' ' + DATENAME(YEAR, OverallDeliveryDeadline)) LIKE N'%" + searchTerm + @"%' OR
       CAST(Value as nvarchar) LIKE N'%" + searchTerm + @"%' OR 
       OrderStatus LIKE N'%" + searchTerm + @"%')
) as filteredRecords";

            query = selectSection + innerJoinSection + whereSection;

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);

            string connectionstring = configuration.GetConnectionString(GlobalVars.CurrentAppModeString);

            using (SqlConnection SQLConn = new SqlConnection(connectionstring))
            {
                SQLConn.Open();
                using var command = SQLConn.CreateCommand();
                command.CommandText = newQuery;
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    totalRecords = await result.GetFieldValueAsync<int>(0);
                    filteredRecords = await result.GetFieldValueAsync<int>(1);
                }

                SQLConn.Close();
                SQLConn.Dispose();
            }
            return (totalRecords, filteredRecords);
        }

        public async Task<IEnumerable<JobOrderDataTableViewModel>> GetAllJobOrdersForQuickProjOverview(int dataObjectId, int dataTypeId, int pageNumber, int pageSize, string searchTerm, int columnToOrderBy, string orderDirection)
        {
            var orders = new List<JobOrderDataTableViewModel>();
            string innerJoinSection = String.Empty;
            string whereSection = String.Empty;

            DateTime EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            DateTime StartDate = EndDate.AddMonths(-6);
            StartDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, 0, 0, 0);

            string startDate = StartDate.ToString("yyyy-MM-dd hh:mm:ss");
            string endDate = EndDate.ToString("yyyy-MM-dd hh:mm:ss");

            var columns = new Dictionary<int, string>();
            columns.Add(0, "JobID");
            columns.Add(1, "ContactName");
            columns.Add(2, "JobName");
            columns.Add(3, "OverallDeliveryDeadline");
            columns.Add(4, "OrderStatus");

            if (dataTypeId == ((int)Enumerations.DataObjectTypes.Org))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";
                whereSection = " where Orgs.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and" +
                               " (JobOrders.OverallCompletedDateTime is null OR (JobOrders.OverallCompletedDateTime is not null and JobOrders.SubmittedDateTime >= '" + startDate + @"' and JobOrders.SubmittedDateTime <= '" + endDate + @"'))";

            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.Contact))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Contacts.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and" +
                               " (JobOrders.OverallCompletedDateTime is null OR (JobOrders.OverallCompletedDateTime is not null and JobOrders.SubmittedDateTime >= '" + startDate + @"' and JobOrders.SubmittedDateTime <= '" + endDate + @"'))";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where OrgGroups.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and" +
                               " (JobOrders.OverallCompletedDateTime is null OR (JobOrders.OverallCompletedDateTime is not null and JobOrders.SubmittedDateTime >= '" + startDate + @"' and JobOrders.SubmittedDateTime <= '" + endDate + @"'))";

            }

            string newQuery = @"select * from(
select JobOrders.ID as JobID, OriginatedFromEnquiryID, JobOrders.JobName, 
JobOrders.OverallDeliveryDeadline, JobOrders.SubmittedDateTime,
(Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'end) as OrderStatus,
Orgs.ID as OrgId, Orgs.OrgName, OrgGroupID, OrgGroups.Name as GroupName, Contacts.ID as ContactId, Contacts.Name as ContactName,
ISNULL(OverallChargeToClient, 0) as Value, JobOrders.AnticipatedGrossMarginPercentage,
(select dbo.funcGetSourceLangOrLangsFullStringJobOrderID(JobOrders.ID)) as SourceLang,
(select dbo.funcGetTargetLangOrLangsFullStringJobOrderID(JobOrders.ID)) as TargetLang,
isnull((select Currencies.Prefix from Currencies where ID = Joborders.ClientCurrencyID),'(none)') as Currency,
JobOrders.OverallSterlingPaymentToSuppliers, JobOrders.OverallCompletedDateTime,
(dbo.funcGetSourceLangOrLangsFullStringJobOrderIDAllLanguages(JobOrders.ID)) as SourceLangsCombined,
(dbo.funcGetTargetLangOrLangsFullStringJobOrderIDAllLanguages(JobOrders.ID)) as TargetLangsCombined,
JobOrders.ClientPONumber, JobOrders.Priority

from JobOrders " + innerJoinSection + whereSection + @"
 ) table1

 WHERE (ContactName LIKE N'%" + searchTerm + @"%' OR
       CAST(JobID as nvarchar) LIKE N'%" + searchTerm + @"%' OR
       JobName LIKE N'%" + searchTerm + @"%' OR
       (SUBSTRING(DATENAME(weekday, OverallDeliveryDeadline), 0, 4)  + ' ' + DATENAME(DAY, OverallDeliveryDeadline) + ' ' + DATENAME(MONTH, OverallDeliveryDeadline) + ' ' + DATENAME(YEAR, OverallDeliveryDeadline)) LIKE N'%" + searchTerm + @"%' OR
       OrderStatus LIKE N'%" + searchTerm + @"%')

    ORDER BY table1." + columns.FirstOrDefault(x => x.Key == columnToOrderBy).Value + " " + orderDirection + @"

	OFFSET " + pageNumber + @" ROWS
    FETCH NEXT " + pageSize + " ROWS ONLY";

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string connectionstring = configuration.GetConnectionString(GlobalVars.CurrentAppModeString);

            using (SqlConnection SQLConn = new SqlConnection(connectionstring))
            {
                SQLConn.Open();
                using var command = SQLConn.CreateCommand();
                command.CommandText = newQuery;
                using var result = command.ExecuteReader();


                while (await result.ReadAsync())
                {
                    orders.Add(new JobOrderDataTableViewModel()
                    {
                        ContactName = await result.GetFieldValueAsync<string>(11),
                        JobOrderId = await result.GetFieldValueAsync<int>(0),
                        JobOrderName = await result.GetFieldValueAsync<string>(2),
                        DeliveryDeadline = await result.GetFieldValueAsync<DateTime>(3),
                        Status = await GetCompletionStatusStringForJobOrder(result.GetFieldValueAsync<int>(0).Result, "en")

                    });
                }

                SQLConn.Close();
                SQLConn.Dispose();
            }
            return orders;
        }

        public async Task<(int, int)> GetAllJobOrdersCountForQuickProjOverview(int dataObjectId, int dataTypeId, string searchTerm)
        {
            int totalRecords = 0;
            int filteredRecords = 0;

            string query = String.Empty;
            string selectSection = String.Empty;
            string innerJoinSection = String.Empty;
            string whereSection = String.Empty;

            DateTime EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            DateTime StartDate = EndDate.AddMonths(-6);
            StartDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, 0, 0, 0);

            string startDate = StartDate.ToString("yyyy-MM-dd hh:mm:ss");
            string endDate = EndDate.ToString("yyyy-MM-dd hh:mm:ss");

            selectSection = @"select COUNT(*) from JobOrders";

            //build rest of the query
            if (dataTypeId == ((int)Enumerations.DataObjectTypes.Org))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Orgs.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and" +
                               " (JobOrders.OverallCompletedDateTime is null OR (JobOrders.OverallCompletedDateTime is not null and JobOrders.SubmittedDateTime >= '" + startDate + @"' and JobOrders.SubmittedDateTime <= '" + endDate + @"'))";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.Contact))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where Contacts.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and" +
                               " (JobOrders.OverallCompletedDateTime is null OR (JobOrders.OverallCompletedDateTime is not null and JobOrders.SubmittedDateTime >= '" + startDate + @"' and JobOrders.SubmittedDateTime <= '" + endDate + @"'))";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                innerJoinSection = @"
                inner join Contacts on JobOrders.ContactID = Contacts.ID
                inner join Orgs on Contacts.OrgID = Orgs.ID
                inner join OrgGroups on Orgs.OrgGroupID = OrgGroups.ID";

                whereSection = " where OrgGroups.ID = " + dataObjectId + @" and JobOrders.DeletedDate is null and" +
                               " (JobOrders.OverallCompletedDateTime is null OR (JobOrders.OverallCompletedDateTime is not null and JobOrders.SubmittedDateTime >= '" + startDate + @"' and JobOrders.SubmittedDateTime <= '" + endDate + @"'))";
            }

            var newQuery = @"select 
	(select count(*) 
		from JobOrders " + innerJoinSection + whereSection + @") as totalRecords,

(select COUNT(jobCount) from 
	(select count(*) as jobCount , JobName, Contacts.Name 'ContactName', JobOrders.ID 'JobID',
        JobOrders.OverallDeliveryDeadline, 
		(Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
			when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
			when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'end) as OrderStatus
		
		from JobOrders " + innerJoinSection + whereSection + @"
		GROUP BY JobOrders.ID, JobName, JobOrders.OverallDeliveryDeadline, JobOrders.OverallCompletedDateTime, Contacts.Name, JobOrders.ID, JobOrders.OverallDeliveryDeadline) table1

		WHERE (ContactName LIKE N'%" + searchTerm + @"%' OR
       CAST(JobID as nvarchar) LIKE N'%" + searchTerm + @"%' OR
       JobName LIKE N'%" + searchTerm + @"%' OR
       (SUBSTRING(DATENAME(weekday, OverallDeliveryDeadline), 0, 4)  + ' ' + DATENAME(DAY, OverallDeliveryDeadline) + ' ' + DATENAME(MONTH, OverallDeliveryDeadline) + ' ' + DATENAME(YEAR, OverallDeliveryDeadline)) LIKE N'%" + searchTerm + @"%' OR
       OrderStatus LIKE N'%" + searchTerm + @"%') 

) as filteredRecords";

            query = selectSection + innerJoinSection + whereSection;

            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);

            string connectionstring = configuration.GetConnectionString(GlobalVars.CurrentAppModeString);

            using (SqlConnection SQLConn = new SqlConnection(connectionstring))
            {
                SQLConn.Open();
                using var command = SQLConn.CreateCommand();
                command.CommandText = newQuery;
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    totalRecords = await result.GetFieldValueAsync<int>(0);
                    filteredRecords = await result.GetFieldValueAsync<int>(1);
                }

                SQLConn.Close();
                SQLConn.Dispose();
            }
            return (totalRecords, filteredRecords);
        }
        public async Task<decimal> GetValueOfOpenJobOrdersForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            short? defaultCurrency = extranetUserService.GetExtranetUserOrg(extranetUserName).Result.InvoiceCurrencyId;

            if (defaultCurrency == null)
            {
                defaultCurrency = 4;
            }

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                decimal result = orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                                .Select(o => new { o.Id, o.ContactId, o.ClientCurrencyId, o.OverallChargeToClient })
                                 .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                                   .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                                    o => o.order.ContactId,
                                    c => c.Id,
                                    (o, c) => new { Order = o.order, orgId = c.OrgId })
                                   .Join(orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null),
                                   c => c.orgId,
                                   o => o.Id,
                                   (c, o) => new { Order = c.Order })
                                   .Select(e => e.Order).Distinct()
                                   .Select(o => new { Charge = ((o.OverallChargeToClient == null || o.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.ClientCurrencyId, defaultCurrency.Value, o.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge);

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                decimal result = orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                                .Select(o => new { o.Id, o.ContactId, o.ClientCurrencyId, o.OverallChargeToClient })
                                 .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                                   .Join(contactRepo.All().Where(e => e.OrgId == orgID && e.DeletedDate == null),
                                    o => o.order.ContactId,
                                    c => c.Id,
                                    (o, c) => new { Order = o.order })
                                   .Select(e => e.Order).Distinct()
                                   .Select(o => new { Charge = exchangeService.Convert(o.ClientCurrencyId, defaultCurrency.Value, o.OverallChargeToClient.Value) }).ToList().Sum(a => a.Charge);

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                decimal result = orderRepository.All().Where(o => o.DeletedDate == null && o.ContactId == clientContactID && o.OverallCompletedDateTime == null)
                                .Select(o => new { o.Id, o.ContactId, o.ClientCurrencyId, o.OverallChargeToClient })
                                 .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                                 .Distinct().Select(o => new { Charge = exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value) }).ToList().Sum(a => a.Charge);

                return result;
            }
        }

        public async Task<int> GetNumberOfJobOrdersServiceInProgressForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();



            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                int result = orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                   o => o.orderId,
                                   i => i,
                                   (o, i) => new { orderId = i }).Distinct().Count();

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                int result = contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                            .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                  c => c,
                                  o => o.ContactId,
                                  (c, o) => new { orderId = o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.orderId,
                                  i => i,
                                  (o, i) => new { orderId = i }).Distinct().Count();

                return result;

            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                int result = orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.Id,
                                  i => i,
                                  (o, i) => new { orderId = i }).Distinct().Count();
                return result;
            }

        }

        public async Task<List<int>> GetAllJobOrdersWithServiceInProgressForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();



            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                List<int> result = await orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                   o => o.orderId,
                                   i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                List<int> result = await contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                            .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                  c => c,
                                  o => o.ContactId,
                                  (c, o) => new { orderId = o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.orderId,
                                  i => i,
                                  (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();
                return result;

            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                List<int> result = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.Id,
                                  i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();
                return result;
            }

        }

        public async Task<decimal> GetValueOfJobOrdersServiceInProgressForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            short? defaultCurrency = extranetUserService.GetExtranetUserOrg(extranetUserName).Result.InvoiceCurrencyId;

            if (defaultCurrency == null)
            {
                defaultCurrency = 4;
            }

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                decimal result = orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id, o.OverallChargeToClient, o.ClientCurrencyId }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orders = o })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                   o => o.orders.Id,
                                   i => i,
                                   (o, i) => new { order = o.orders }).Distinct()
                                   .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge);

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                decimal result = contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                            .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id, o.OverallChargeToClient, o.ClientCurrencyId }),
                                  c => c,
                                  o => o.ContactId,
                                  (c, o) => new { orders = o })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.orders.Id,
                                  i => i,
                                  (o, i) => new { order = o.orders }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge);

                return result;

            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                decimal result = orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id, o.OverallChargeToClient, o.ClientCurrencyId })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.Id,
                                  i => i,
                                  (o, i) => new { order = o }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge);
                return result;
            }
        }

        public async Task<int> GetNumberOfJobOrdersInReviewForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            List<int> openJobs = null;
            List<int> InProgressJobs = null;

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId })
                            .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                            .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                            o => o.order.ContactId,
                            c => c.Id,
                            (o, c) => new { OrderId = o.order.Id, orgId = c.OrgId })
                            .Join(orgRepo.All().Where(o => o.DeletedDate == null),
                            c => c.orgId,
                            o => o.Id,
                            (c, o) => new { OrderId = c.OrderId, groupId = o.OrgGroupId }).Where(e => e.groupId == groupID)
                            .Select(e => e.OrderId).Distinct().ToListAsync();

                InProgressJobs = await orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                   o => o.orderId,
                                   i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();



            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId })
                             .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                                   .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                                    o => o.order.ContactId,
                                    c => c.Id,
                                    (o, c) => new { OrderId = o.order.Id, orgId = c.OrgId }).Where(e => e.orgId == orgID)
                                   .Select(e => e.OrderId).Distinct().ToListAsync();

                InProgressJobs = await contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                            .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                  c => c,
                                  o => o.ContactId,
                                  (c, o) => new { orderId = o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.orderId,
                                  i => i,
                                  (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.ContactId == clientContactID && o.OverallCompletedDateTime == null)
                             .Select(o => o.Id)
                              .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o,
                                  ji => ji,
                                  (o, ji) => new { order = (int)o }).Select(o => o.order).Distinct().ToListAsync();

                InProgressJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.Id,
                                  i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();
            }

            var result = openJobs.Where(o => !InProgressJobs.ToArray().Contains(o))
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId == 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o,
                                  ji => ji,
                                  (o, ji) => new { orderId = ji }).Distinct().Count();

            return result;

        }


        public async Task<decimal> GetValueOfJobOrdersInReviewForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            short? defaultCurrency = extranetUserService.GetExtranetUserOrg(extranetUserName).Result.InvoiceCurrencyId;

            if (defaultCurrency == null)
            {
                defaultCurrency = 4;
            }

            List<int> InProgressJobs = null;

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                var openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId, o.OverallChargeToClient, o.ClientCurrencyId })
                            .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                            .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                            o => o.order.ContactId,
                            c => c.Id,
                            (o, c) => new { Order = o.order, orgId = c.OrgId })
                            .Join(orgRepo.All().Where(o => o.DeletedDate == null && o.OrgGroupId == groupID),
                            c => c.orgId,
                            o => o.Id,
                            (c, o) => new { Order = c.Order })
                            .Distinct().ToListAsync();

                InProgressJobs = await orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                   o => o.orderId,
                                   i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

                var result = openJobs.Where(o => !InProgressJobs.Contains(o.Order.Id))
                           .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId == 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                 o => o.Order.Id,
                                 ji => ji,
                                 (o, ji) => new { order = o.Order }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge); ;

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                var openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId, o.OverallChargeToClient, o.ClientCurrencyId })
                             .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                                   .Join(contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID),
                                    o => o.order.ContactId,
                                    c => c.Id,
                                    (o, c) => new { Order = o.order })
                                   .Select(e => e.Order).Distinct().ToListAsync();

                InProgressJobs = await contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                            .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                  c => c,
                                  o => o.ContactId,
                                  (c, o) => new { orderId = o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.orderId,
                                  i => i,
                                  (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

                var result = openJobs.Where(o => !InProgressJobs.Contains(o.Id))
                           .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId == 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                 o => o.Id,
                                 ji => ji,
                                 (o, ji) => new { order = o }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge); ;

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                var openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.ContactId == clientContactID && o.OverallCompletedDateTime == null)
                             .Select(o => new { o.Id, o.OverallChargeToClient, o.ClientCurrencyId })
                              .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o }).Distinct().ToListAsync();

                InProgressJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId != 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.Id,
                                  i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

                var result = openJobs.Where(o => !InProgressJobs.Contains(o.order.Id))
                           .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LanguageServiceId == 21 && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                 o => o.order.Id,
                                 ji => ji,
                                 (o, ji) => new { order = o.order }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge); ;

                return result;
            }

        }

        public async Task<int> GetNumberOfJobOrdersInFinalChecksForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            List<int> openJobs = null;
            List<int> InProgressJobs = null;

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId })
                            .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                            .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                            o => o.order.ContactId,
                            c => c.Id,
                            (o, c) => new { OrderId = o.order.Id, orgId = c.OrgId })
                            .Join(orgRepo.All().Where(o => o.DeletedDate == null),
                            c => c.orgId,
                            o => o.Id,
                            (c, o) => new { OrderId = c.OrderId, groupId = o.OrgGroupId }).Where(e => e.groupId == groupID)
                            .Select(e => e.OrderId).Distinct().ToListAsync();

                InProgressJobs = await orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                   o => o.orderId,
                                   i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();


            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId })
                             .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                                   .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                                    o => o.order.ContactId,
                                    c => c.Id,
                                    (o, c) => new { OrderId = o.order.Id, orgId = c.OrgId }).Where(e => e.orgId == orgID)
                                   .Select(e => e.OrderId).Distinct().ToListAsync();

                InProgressJobs = await contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                            .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                  c => c,
                                  o => o.ContactId,
                                  (c, o) => new { orderId = o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.orderId,
                                  i => i,
                                  (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.ContactId == clientContactID && o.OverallCompletedDateTime == null)
                             .Select(o => o.Id)
                              .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o,
                                  ji => ji,
                                  (o, ji) => new { order = (int)o }).Select(o => o.order).Distinct().ToListAsync();

                InProgressJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.Id,
                                  i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();
            }

            var result = openJobs.Where(o => !InProgressJobs.ToArray().Contains(o))
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null &&
                                                            (i.ExtranetClientStatusId != 3 && i.ExtranetClientStatusId != 2) &&
                                                            (i.WebServiceClientStatusId != 3 && i.WebServiceClientStatusId != 2))
                            .Select(i => i.JobOrderId),
                                  o => o,
                                  ji => ji,
                                  (o, ji) => new { orderId = ji }).Distinct().Count();

            return result;

        }

        public async Task<decimal> GetValueOfJobOrdersInFinalChecksForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            short? defaultCurrency = extranetUserService.GetExtranetUserOrg(extranetUserName).Result.InvoiceCurrencyId;

            if (defaultCurrency == null)
            {
                defaultCurrency = 4;
            }

            List<int> InProgressJobs = null;

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                var openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId, o.OverallChargeToClient, o.ClientCurrencyId })
                            .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                            .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                            o => o.order.ContactId,
                            c => c.Id,
                            (o, c) => new { Order = o.order, orgId = c.OrgId })
                            .Join(orgRepo.All().Where(o => o.DeletedDate == null && o.OrgGroupId == groupID),
                            c => c.orgId,
                            o => o.Id,
                            (c, o) => new { Order = c.Order })
                            .Distinct().ToListAsync();

                InProgressJobs = await orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                   o => o.orderId,
                                   i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

                var result = openJobs.Where(o => !InProgressJobs.Contains(o.Order.Id))
                           .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null &&
                                                            (i.ExtranetClientStatusId != 3 && i.ExtranetClientStatusId != 2) &&
                                                            (i.WebServiceClientStatusId != 3 && i.WebServiceClientStatusId != 2))
                           .Select(i => i.JobOrderId),
                                 o => o.Order.Id,
                                 ji => ji,
                                 (o, ji) => new { order = o.Order }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge); ;

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                var openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId, o.OverallChargeToClient, o.ClientCurrencyId })
                             .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                                   .Join(contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID),
                                    o => o.order.ContactId,
                                    c => c.Id,
                                    (o, c) => new { Order = o.order })
                                   .Select(e => e.Order).Distinct().ToListAsync();

                InProgressJobs = await contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                            .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                  c => c,
                                  o => o.ContactId,
                                  (c, o) => new { orderId = o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.orderId,
                                  i => i,
                                  (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

                var result = openJobs.Where(o => !InProgressJobs.Contains(o.Id))
                           .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null &&
                                                            (i.ExtranetClientStatusId != 3 && i.ExtranetClientStatusId != 2) &&
                                                            (i.WebServiceClientStatusId != 3 && i.WebServiceClientStatusId != 2))
                           .Select(i => i.JobOrderId),
                                 o => o.Id,
                                 ji => ji,
                                 (o, ji) => new { order = o }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge); ;

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                var openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.ContactId == clientContactID && o.OverallCompletedDateTime == null)
                             .Select(o => new { o.Id, o.OverallChargeToClient, o.ClientCurrencyId })
                              .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o }).Distinct().ToListAsync();

                InProgressJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.Id,
                                  i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

                var result = openJobs.Where(o => !InProgressJobs.Contains(o.order.Id))
                           .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null &&
                                                            (i.ExtranetClientStatusId != 3 && i.ExtranetClientStatusId != 2) &&
                                                            (i.WebServiceClientStatusId != 3 && i.WebServiceClientStatusId != 2))
                           .Select(i => i.JobOrderId),
                                 o => o.order.Id,
                                 ji => ji,
                                 (o, ji) => new { order = o.order }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge); ;

                return result;
            }
        }
        public async Task<int> GetNumberOfJobOrdersReadyToCollectForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            List<int> openJobs = null;
            List<int> InProgressJobs = null;

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId })
                            .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                            .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                            o => o.order.ContactId,
                            c => c.Id,
                            (o, c) => new { OrderId = o.order.Id, orgId = c.OrgId })
                            .Join(orgRepo.All().Where(o => o.DeletedDate == null),
                            c => c.orgId,
                            o => o.Id,
                            (c, o) => new { OrderId = c.OrderId, groupId = o.OrgGroupId }).Where(e => e.groupId == groupID)
                            .Select(e => e.OrderId).Distinct().ToListAsync();

                InProgressJobs = await orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                   o => o.orderId,
                                   i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();



            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId })
                             .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                                   .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                                    o => o.order.ContactId,
                                    c => c.Id,
                                    (o, c) => new { OrderId = o.order.Id, orgId = c.OrgId }).Where(e => e.orgId == orgID)
                                   .Select(e => e.OrderId).Distinct().ToListAsync();

                InProgressJobs = await contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                            .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                  c => c,
                                  o => o.ContactId,
                                  (c, o) => new { orderId = o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.orderId,
                                  i => i,
                                  (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.ContactId == clientContactID && o.OverallCompletedDateTime == null)
                             .Select(o => o.Id)
                              .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o,
                                  ji => ji,
                                  (o, ji) => new { order = (int)o }).Select(o => o.order).Distinct().ToListAsync();

                InProgressJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.Id,
                                  i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();
            }

            var result = openJobs.Where(o => !InProgressJobs.ToArray().Contains(o))
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null &&
                                                            (i.ExtranetClientStatusId == 3 || i.ExtranetClientStatusId == 2) ||
                                                            (i.WebServiceClientStatusId == 3 || i.WebServiceClientStatusId == 2))
                            .Select(i => i.JobOrderId),
                                  o => o,
                                  ji => ji,
                                  (o, ji) => new { orderId = ji }).Distinct().Count();

            return result;

        }

        public async Task<decimal> GetValueOfJobOrdersReadyToCollectForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            short? defaultCurrency = extranetUserService.GetExtranetUserOrg(extranetUserName).Result.InvoiceCurrencyId;

            if (defaultCurrency == null)
            {
                defaultCurrency = 4;
            }

            List<int> InProgressJobs = null;

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                var openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId, o.OverallChargeToClient, o.ClientCurrencyId })
                            .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                            .Join(contactRepo.All().Where(c => c.DeletedDate == null),
                            o => o.order.ContactId,
                            c => c.Id,
                            (o, c) => new { Order = o.order, orgId = c.OrgId })
                            .Join(orgRepo.All().Where(o => o.DeletedDate == null && o.OrgGroupId == groupID),
                            c => c.orgId,
                            o => o.Id,
                            (c, o) => new { Order = c.Order })
                            .Distinct().ToListAsync();

                InProgressJobs = await orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                   o => o.orderId,
                                   i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

                var result = openJobs.Where(o => !InProgressJobs.Contains(o.Order.Id))
                           .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null &&
                                                            (i.ExtranetClientStatusId == 3 || i.ExtranetClientStatusId == 2) ||
                                                            (i.WebServiceClientStatusId == 3 || i.WebServiceClientStatusId == 2))
                           .Select(i => i.JobOrderId),
                                 o => o.Order.Id,
                                 ji => ji,
                                 (o, ji) => new { order = o.Order }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge); ;

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                var openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null)
                            .Select(o => new { o.Id, o.ContactId, o.OverallChargeToClient, o.ClientCurrencyId })
                             .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o })
                                   .Join(contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID),
                                    o => o.order.ContactId,
                                    c => c.Id,
                                    (o, c) => new { Order = o.order })
                                   .Select(e => e.Order).Distinct().ToListAsync();

                InProgressJobs = await contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                            .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                  c => c,
                                  o => o.ContactId,
                                  (c, o) => new { orderId = o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.orderId,
                                  i => i,
                                  (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

                var result = openJobs.Where(o => !InProgressJobs.Contains(o.Id))
                           .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null &&
                                                            (i.ExtranetClientStatusId == 3 || i.ExtranetClientStatusId == 2) ||
                                                            (i.WebServiceClientStatusId == 3 || i.WebServiceClientStatusId == 2))
                           .Select(i => i.JobOrderId),
                                 o => o.Id,
                                 ji => ji,
                                 (o, ji) => new { order = o }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge); ;

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                var openJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.ContactId == clientContactID && o.OverallCompletedDateTime == null)
                             .Select(o => new { o.Id, o.OverallChargeToClient, o.ClientCurrencyId })
                              .Join(jobitemsRepo.All().Where(ji => ji.DeletedDateTime == null).Select(ji => ji.JobOrderId),
                                  o => o.Id,
                                  ji => ji,
                                  (o, ji) => new { order = o }).Distinct().ToListAsync();

                InProgressJobs = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime == null).Select(i => i.JobOrderId),
                                  o => o.Id,
                                  i => i,
                                   (o, i) => new { orderId = (int)i }).Select(o => o.orderId).Distinct().ToListAsync();

                var result = openJobs.Where(o => !InProgressJobs.Contains(o.order.Id))
                           .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null &&
                                                            (i.ExtranetClientStatusId == 3 || i.ExtranetClientStatusId == 2) ||
                                                            (i.WebServiceClientStatusId == 3 || i.WebServiceClientStatusId == 2))
                           .Select(i => i.JobOrderId),
                                 o => o.order.Id,
                                 ji => ji,
                                 (o, ji) => new { order = o.order }).Distinct()
                                  .Select(o => new { Charge = ((o.order.OverallChargeToClient == null || o.order.OverallChargeToClient.Value == 0) ? 0 : exchangeService.Convert(o.order.ClientCurrencyId, defaultCurrency.Value, o.order.OverallChargeToClient.Value)) }).ToList().Sum(a => a.Charge); ;

                return result;
            }
        }


        public async Task<int> GetNumberOfJobItemsInOpenOrdersForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                var result = orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null).Select(i => new { i.JobOrderId, i.Id }),
                                   o => o.orderId,
                                   i => i.JobOrderId,
                                   (o, i) => new { itemId = i.Id }).Distinct().Count();
                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                var result = contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null).Select(i => new { i.JobOrderId, i.Id }),
                                   o => o.orderId,
                                   i => i.JobOrderId,
                                   (o, i) => new { itemId = i.Id }).Distinct().Count();

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                var result = orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null).Select(i => new { i.JobOrderId, i.Id }),
                                  o => o.Id,
                                  i => i.JobOrderId,
                                   (o, i) => new { itemId = i.Id }).Distinct().Count();
                return result;
            }

        }

        public async Task<int> GetNumberOfCompleteJobItemsInOpenOrdersForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                var result = orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime != null).Select(i => new { i.JobOrderId, i.Id }),
                                   o => o.orderId,
                                   i => i.JobOrderId,
                                   (o, i) => new { itemId = i.Id }).Distinct().Count();
                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                var result = contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime != null).Select(i => new { i.JobOrderId, i.Id }),
                                   o => o.orderId,
                                   i => i.JobOrderId,
                                   (o, i) => new { itemId = i.Id }).Distinct().Count();

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                var result = orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime != null).Select(i => new { i.JobOrderId, i.Id }),
                                  o => o.Id,
                                  i => i.JobOrderId,
                                   (o, i) => new { itemId = i.Id }).Distinct().Count();
                return result;
            }

        }

        public async Task<int> GetWordCountOfAllJobItemsInOpenOrdersForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                var result = await orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null)
                             .Select(i => new { i.JobOrderId, wordcountTotal = i.WordCountNew + i.WordCountExact + i.WordCountClientSpecific + i.WordCountFuzzyBand1 + i.WordCountFuzzyBand2 + i.WordCountFuzzyBand3 + i.WordCountFuzzyBand4 + i.WordCountPerfectMatches + i.WordCountRepetitions }),
                                   o => o.orderId,
                                   i => i.JobOrderId,
                                   (o, i) => new { i = i.wordcountTotal }).Distinct().SumAsync(s => s.i.Value);
                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                var result = await contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null)
                             .Select(i => new { i.JobOrderId, wordcountTotal = i.WordCountNew + i.WordCountExact + i.WordCountClientSpecific + i.WordCountFuzzyBand1 + i.WordCountFuzzyBand2 + i.WordCountFuzzyBand3 + i.WordCountFuzzyBand4 + i.WordCountPerfectMatches + i.WordCountRepetitions }),
                                   o => o.orderId,
                                   i => i.JobOrderId,
                                   (o, i) => new { i = i.wordcountTotal }).Distinct().SumAsync(s => s.i.Value);

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                var result = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null)
                            .Select(i => new { i.JobOrderId, wordcountTotal = i.WordCountNew + i.WordCountExact + i.WordCountClientSpecific + i.WordCountFuzzyBand1 + i.WordCountFuzzyBand2 + i.WordCountFuzzyBand3 + i.WordCountFuzzyBand4 + i.WordCountPerfectMatches + i.WordCountRepetitions }),
                                  o => o.Id,
                                  i => i.JobOrderId,
                                   (o, i) => new { i = i.wordcountTotal }).Distinct().SumAsync(s => s.i.Value);
                return result;
            }
        }

        public async Task<int> GetWordCountOfCompleteJobItemsInOpenOrdersForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                var result = await orgRepo.All().Where(e => e.OrgGroupId == groupID && e.DeletedDate == null).Select(o => o.Id)
                             .Join(contactRepo.All().Where(c => c.DeletedDate == null).Select(c => new { c.OrgId, c.Id }),
                                   o => o,
                                   c => c.OrgId,
                                   (o, c) => new { contactId = c.Id })
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c.contactId,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime != null)
                             .Select(i => new { i.JobOrderId, wordcountTotal = i.WordCountNew + i.WordCountExact + i.WordCountClientSpecific + i.WordCountFuzzyBand1 + i.WordCountFuzzyBand2 + i.WordCountFuzzyBand3 + i.WordCountFuzzyBand4 + i.WordCountPerfectMatches + i.WordCountRepetitions }),
                                   o => o.orderId,
                                   i => i.JobOrderId,
                                   (o, i) => new { i = i.wordcountTotal }).Distinct().SumAsync(s => s.i.Value);
                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                var result = await contactRepo.All().Where(c => c.DeletedDate == null && c.OrgId == orgID).Select(c => c.Id)
                             .Join(orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null).Select(o => new { o.ContactId, o.Id }),
                                   c => c,
                                   o => o.ContactId,
                                   (c, o) => new { orderId = o.Id })
                             .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime != null)
                             .Select(i => new { i.JobOrderId, wordcountTotal = i.WordCountNew + i.WordCountExact + i.WordCountClientSpecific + i.WordCountFuzzyBand1 + i.WordCountFuzzyBand2 + i.WordCountFuzzyBand3 + i.WordCountFuzzyBand4 + i.WordCountPerfectMatches + i.WordCountRepetitions }),
                                   o => o.orderId,
                                   i => i.JobOrderId,
                                   (o, i) => new { i = i.wordcountTotal }).Distinct().SumAsync(s => s.i.Value);

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                var result = await orderRepository.All().Where(o => o.DeletedDate == null && o.OverallCompletedDateTime == null && o.ContactId == clientContactID).Select(o => new { o.Id })
                            .Join(jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.WeCompletedItemDateTime != null)
                            .Select(i => new { i.JobOrderId, wordcountTotal = i.WordCountNew + i.WordCountExact + i.WordCountClientSpecific + i.WordCountFuzzyBand1 + i.WordCountFuzzyBand2 + i.WordCountFuzzyBand3 + i.WordCountFuzzyBand4 + i.WordCountPerfectMatches + i.WordCountRepetitions }),
                                  o => o.Id,
                                  i => i.JobOrderId,
                                   (o, i) => new { i = i.wordcountTotal }).Distinct().SumAsync(s => s.i.Value);
                return result;
            }
        }

        public async Task<decimal> GetJobOrderProgress(int JobOrderID)
        {
            decimal progress = 0;
            var jobOrder = await orderRepository.All().Where(a => a.Id == JobOrderID && a.DeletedDate == null).FirstOrDefaultAsync();
            if (jobOrder != null)
            {
                if (GetCompletionStatusStringForJobOrder(JobOrderID, "en").Result == "Fully complete")
                {
                    progress = 100;
                }
                else
                {
                    DateTime deadLine = jobOrder.OverallDeliveryDeadline;
                    DateTime createdDate = jobOrder.SubmittedDateTime;
                    var totalTime = (createdDate - deadLine).TotalMinutes;
                    var timeTaken = (createdDate - DateTime.UtcNow).TotalMinutes;

                    if (timeTaken < totalTime)
                    {
                        progress = Convert.ToDecimal((timeTaken / totalTime) * 100);
                        if (progress > 95)
                            progress = 95;
                        if (progress < 0)
                            progress = 0;
                    }
                    else
                    {
                        if (totalTime < 0)
                            progress = 0;
                        else
                            progress = 95;
                    }
                }
            }
            return progress;
        }
        public async Task<OrderDetailModel> GetOrderDetails(int jobOrderId)
        {
            var orderDetails = await GetById(jobOrderId);
            OrderDetailModel order = new OrderDetailModel();
            order.JobOrderId = jobOrderId;
            order.SubmittedDate = orderDetails.SubmittedDateTime;
            order.DeliveryDate = orderDetails.OverallDeliveryDeadline;
            order.ClientNotes = orderDetails.ClientNotes;
            order.JobName = orderDetails.JobName;
            order.PONumber = orderDetails.ClientPonumber;
            order.Currency = currencyRepo.All().Where(x => x.Id == orderDetails.ClientCurrencyId).FirstOrDefault().Symbol;
            order.OrderChannel = channel.All().Where(x => x.Id == orderDetails.JobOrderChannelId).FirstOrDefault().Name;
            order.Cost = currencyRepo.All().Where(x => x.Id == orderDetails.ClientCurrencyId).FirstOrDefault().Prefix + Convert.ToDecimal(orderDetails.OverallSterlingPaymentToSuppliers).ToString("0.00");
            var contact = contactRepo.All().Where(x => x.Id == orderDetails.ContactId).FirstOrDefault();
            order.ContactId = contact.Id;
            order.OrgId = contact.OrgId;
            order.ContactName = contact.Name;
            order.Priority = Convert.ToString(orderDetails.Priority);
            order.SourceFiles = GetSourceFilesForJobOrder(jobOrderId);
            order.TargetFiles = GetTargetFilesForJobOrder(jobOrderId);
            order.ReferenceFiles = GetReferenceFilesForJobOrder(jobOrderId);
            order.IsSourceFileAvailable = order.SourceFiles != "" ? true : false;
            order.IsTargetFileAvailable = order.TargetFiles != "" ? true : false;
            order.IsReferenceFileAvailable = order.ReferenceFiles != "" ? true : false;
            return order;
        }
        public async Task<List<JobItemDetailsModel>> GetJobItemDetails(int JobOrderId)
        {
            var jobOrderData = orderRepository.All().Where(x => x.Id == JobOrderId && x.DeletedDate == null).ToList();
            var jobItemData = jobitemsRepo.All().Where(x => x.JobOrderId == JobOrderId && x.DeletedDateTime == null).ToList();
            var data = (from order in jobOrderData
                        join job in jobItemData on order.Id equals job.JobOrderId
                        join lang in langServiceRepo.All() on job.LanguageServiceId equals lang.Id
                        join cur in currencyRepo.All() on order.ClientCurrencyId equals cur.Id
                        select new
                        {
                            job,
                            lang,
                            order,
                            cur
                        });

            var contactsOrgId = contactRepo.All().Where(x => x.Id == jobOrderData.FirstOrDefault().ContactId).FirstOrDefault().OrgId;
            var contacts = contactRepo.All().Where(x => x.OrgId == contactsOrgId && x.DeletedDate == null).Select(x => new ReviewerContact
            {
                ContactId = x.Id,
                ReviewerName = x.Name
            }).ToList();

            var jobItems = data.Select(x => new JobItemDetailsModel
            {
                JobItemId = x.job.Id,
                LanguageService = x.lang.Name,
                SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.job.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.job.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                Translator = x.lang.Id == 1 ? GetTranslatorOrReviewerName(JobOrderId, x.job.Id).Name ?? "" : "",
                Reviewer = x.lang.Id == 21 ? GetTranslatorOrReviewerName(JobOrderId, x.job.Id).Name ?? "" : "",
                FileName = x.job.FileName,
                Cost = x.cur.Prefix + Convert.ToDecimal(x.job.ChargeToClient).ToString("N2"),
                Status = GetJobItemStatus(JobOrderId, x.job.Id),
                ProofReadingCompDate = x.lang.Id == 21 ? (x.job.SupplierCompletedItemDateTime != null ? Convert.ToString(x.job.SupplierCompletedItemDateTime) : null) : null,
                IsDownloaded = x.job.DownloadedByContactId != null ? true : false,
                Progress = GetJobItemProgress(x.job.Id),
                Editor = contacts,
                SupplierCompletionDeadline = x.job.SupplierCompletionDeadline != null ? ((DateTime)x.job.SupplierCompletionDeadline).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : DateTime.Now.ToString("yyyy-MM-dd")
                // Remaining Fields logic
            }).ToList();
            return await System.Threading.Tasks.Task.FromResult(jobItems);
        }

        public Contact GetTranslatorOrReviewerName(int jobOrderId, int jobItemId)
        {
            var jobItemData = jobitemsRepo.All().Where(x => x.JobOrderId == jobOrderId && x.Id == jobItemId && x.DeletedDateTime == null).FirstOrDefault();
            int langServiceId = jobItemData.LanguageServiceId;
            bool supplierIsClientReviewer = jobItemData.SupplierIsClientReviewer ?? false;
            string clientReviewer = "";
            string linguisticSupplier = "";
            var contact = new Contact();
            if (jobItemData.LinguisticSupplierOrClientReviewerId != null)
            {
                if (supplierIsClientReviewer)
                {
                    contact = contactRepo.All().Where(x => x.Id == jobItemData.LinguisticSupplierOrClientReviewerId).FirstOrDefault();
                    clientReviewer = contact.Name;
                }
                else
                {
                    linguisticSupplier = contactRepo.All().Where(x => x.Id == jobItemData.LinguisticSupplierOrClientReviewerId).FirstOrDefault().Name;
                }
            }
            return contact;

        }
        public string GetJobItemStatus(int jobOrderId, int jobItemId)
        {
            var jobItemData = jobitemsRepo.All().Where(x => x.JobOrderId == jobOrderId && x.Id == jobItemId && x.DeletedDateTime == null).FirstOrDefault();
            var jobOrderData = orderRepository.All().Where(x => x.Id == jobOrderId);
            var data = (from jobOrder in jobOrderData
                        join contact in contactRepo.All() on jobOrder.ContactId equals contact.Id
                        join org in orgRepo.All() on contact.OrgId equals org.Id
                        select new
                        {
                            jobOrder,
                            contact,
                            org
                        }).FirstOrDefault();
            int langServiceId = jobItemData.LanguageServiceId;
            string Status = "";
            if (langServiceId != 21) // Checking for Language service is not of type Client Review
            {
                if (jobItemData.ExtranetClientStatusId == 2 || jobItemData.ExtranetClientStatusId == 3)
                {
                    if (data.jobOrder.IsAcmsproject)
                    {
                        Status = "Complete";
                    }
                    else
                    {
                        if (data.org.Id == 83923 && data.jobOrder.JobOrderChannelId == 21)
                        {
                            Status = string.Format(@"Complete and <a href=""DownloadJobItem.aspx?ItemID={0}&DesignPlusFileID={1}"" class=""ourservicesblue"">available to download</a>", jobItemId.ToString(), jobOrderData.FirstOrDefault().DesignPlusFileId.ToString());
                            //Status = "Complete and available to download";
                        }
                        else
                        {
                            Status = string.Format(@"Complete and <a href=""javascript:downloadJobItem({0})"" class=""ourservicesblue"">available to download</a>", jobItemId.ToString());
                            //Status = "Complete and available to download";
                        }
                    }
                }
                else if (jobItemData.WeCompletedItemDateTime != null /*DateTime.MinValue*/)
                {
                    Status = "Fully complete";
                }
                else
                {
                    Status = "In progress";
                }
            }
            else
            {
                if (langServiceId != 21 && (jobItemData.SupplierIsClientReviewer == false && data.org.Id != 79702))
                    Status = "Review Not Required";
                else if (jobItemData.SupplierSentWorkDateTime == null)
                    Status = "Not yet sent for review";
                else if (jobItemData.SupplierAcceptedWorkDateTime == null)
                    Status = "Sent for review, pending acceptance";
                else if (jobItemData.SupplierCompletedItemDateTime == null)
                    Status = "Review in progress";
                else
                    Status = "Signed off";
            }
            return Status;
        }

        public string GetJobItemProgress(int jobItemId)
        {
            var jobItemData = jobitemsRepo.All().Where(x => x.Id == jobItemId && x.DeletedDateTime == null).FirstOrDefault();
            decimal progress = 0;
            DateTime? completionDate = jobItemData.WeCompletedItemDateTime;
            if (completionDate != null)
            {
                progress = 100;
            }
            else
            {
                DateTime? deadlineDate = jobItemData.OurCompletionDeadline;
                DateTime? createdDate = jobItemData.CreatedDateTime;
                if (createdDate != null && deadlineDate != null)
                {
                    var totalTime = (deadlineDate - createdDate).Value.TotalMinutes;
                    var timeTaken = (DateTime.UtcNow - createdDate).Value.TotalMinutes;
                    if (timeTaken < totalTime)
                    {
                        progress = Convert.ToDecimal((timeTaken / totalTime) * 100);
                        if (progress > 95)
                            progress = 95;
                        if (progress < 0)
                            progress = 0;
                    }
                    else
                    {
                        if (totalTime < 0)
                            progress = 0;
                        else
                            progress = 100;
                    }
                }
            }
            return progress.ToString("0.0") + "%";
        }
        public async Task<int> UpdatePriority(int jobOrderId, string priority, string extranetUserName)
        {
            var extranetUserInfo = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
            var accessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);
            var loggedInExtranetUser = contactRepo.All().Where(x => x.Id == extranetUserInfo.DataObjectId && x.DeletedDate == null).FirstOrDefault();
            var empSalesOwnerships = new Enumerations.EmployeeOwnerships[]
            {
                Enumerations.EmployeeOwnerships.SalesNewBusinessLead,
                Enumerations.EmployeeOwnerships.SalesAccountManagerLead
            };
            var empOpsOwnerships = new Enumerations.EmployeeOwnerships[]
            {
                Enumerations.EmployeeOwnerships.OperationsLead
            };
            var clientIntroOwnerships = new Enumerations.EmployeeOwnerships[]
            {
                Enumerations.EmployeeOwnerships.ClientIntroLead
            };
            DateTime timeZone = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            var jobOrder = orderRepository.All().Where(x => x.Id == jobOrderId && x.DeletedDate == null).FirstOrDefault();
            var jobOrderOrgData = (from order in orderRepository.All().Where(x => x.Id == jobOrderId && x.DeletedDate == null)
                                   join contact in contactRepo.All().Where(x => x.Id == jobOrder.ContactId && x.DeletedDate == null) on order.ContactId equals contact.Id
                                   join org in orgRepo.All() on contact.OrgId equals org.Id
                                   select new
                                   {
                                       CurrentSalesOwners = ownershipsLogicService.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(jobOrder.ContactId, Enumerations.DataObjectTypes.Org, empSalesOwnerships, timeZone, true).Result,
                                       CurrentOpsOwners = ownershipsLogicService.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(jobOrder.ContactId, Enumerations.DataObjectTypes.Org, empOpsOwnerships, timeZone, true).Result.FirstOrDefault(),
                                       CurrentClientIntroOwner = ownershipsLogicService.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(jobOrder.ContactId, Enumerations.DataObjectTypes.Org, clientIntroOwnerships, timeZone, true).Result.FirstOrDefault()
                                   }).FirstOrDefault();

            byte? newPriority = 0;
            var oldPriority = jobOrder.Priority;
            string newPriorityName = "";
            string oldPriorityName = "";
            Dictionary<byte, string> priorities = new Dictionary<byte, string>();
            priorities.Add(0, "None");
            priorities.Add(1, "High");
            priorities.Add(2, "Medium");
            priorities.Add(3, "Low");
            if (oldPriority != null)
            {
                oldPriorityName = priorities.Where(x => x.Key == oldPriority).FirstOrDefault().Value;
            }
            if (priority != null)
            {
                switch (priority)
                {
                    case "0":
                        newPriority = 0;    //None
                        newPriorityName = "None";
                        break;
                    case "1":
                        newPriority = 1;    //High
                        newPriorityName = "High";
                        break;
                    case "2":
                        newPriority = 2;    //Medium
                        newPriorityName = "Medium";
                        break;
                    case "3":
                        newPriority = 3;    //Low
                        newPriorityName = "Low";
                        break;
                }
            }
            jobOrder.Priority = newPriority;
            int rowsAffected = await orderRepository.SaveChangesAsync();
            // Write code to send email
            string emailRecipients = "";

            if (jobOrderOrgData.CurrentSalesOwners.Count() != 0 && jobOrderOrgData.CurrentSalesOwners != null)
            {
                var currentsalesEmployeeData = employeesRepo.All().Where(x => (jobOrderOrgData.CurrentSalesOwners.Select(m => m.EmployeeId)).Contains(x.Id) && x.TerminateDate == null).Select(x => new
                {
                    x.EmailAddress,
                    x.FirstName
                }).ToList();
                foreach (var salesEmployee in currentsalesEmployeeData)
                {
                    if (emailRecipients == "")
                    {
                        emailRecipients = salesEmployee.EmailAddress;
                    }
                    else
                    {
                        emailRecipients += ", " + salesEmployee.EmailAddress;
                    }
                }
            }
            else
            {
                //Email of Sales team manager
                short salesDepTeamId = empTeamsRepo.All().Where(x => x.Name.ToLower() == "sales management").FirstOrDefault().Id;
                var salesTeamManager = employeesRepo.All().Where(x => x.TeamId == salesDepTeamId && x.TerminateDate == null && x.IsTeamManager == true).FirstOrDefault();
                emailRecipients = salesTeamManager.EmailAddress;
            }

            if (jobOrderOrgData.CurrentOpsOwners != null)
            {
                var currentOpsEmployeeData = employeesRepo.All().Where(x => jobOrderOrgData.CurrentOpsOwners.EmployeeId == x.Id && x.TerminateDate == null).Select(x => new
                {
                    x.EmailAddress,
                    x.FirstName,
                    isTeamManager = x.IsTeamManager ?? false,
                    x.Manager
                }).FirstOrDefault();
                emailRecipients += "," + currentOpsEmployeeData.EmailAddress;
                if (!currentOpsEmployeeData.isTeamManager)
                {
                    if (currentOpsEmployeeData.Manager != null)
                    {
                        var manager = employeesRepo.All().Where(x => x.Id == (short)currentOpsEmployeeData.Manager && x.TerminateDate == null).FirstOrDefault();
                        emailRecipients += "," + manager.EmailAddress;
                    }
                }
                if (jobOrderOrgData.CurrentClientIntroOwner != null)
                {
                    var currentClientIntroEmployeeData = employeesRepo.All().Where(x => jobOrderOrgData.CurrentClientIntroOwner.EmployeeId == x.Id && x.TerminateDate == null).Select(x => new
                    {
                        x.EmailAddress,
                        x.FirstName,
                        isTeamManager = x.IsTeamManager ?? false,
                        x.Manager
                    }).FirstOrDefault();
                    var clientIntroManager = employeesRepo.All().Where(x => x.Id == currentClientIntroEmployeeData.Manager && x.TerminateDate == null).FirstOrDefault();
                    emailRecipients += "," + clientIntroManager.EmailAddress;
                }
            }
            else
            {
                if (jobOrderOrgData.CurrentClientIntroOwner != null)
                {
                    var currentClientIntroEmployeeData = employeesRepo.All().Where(x => jobOrderOrgData.CurrentClientIntroOwner.EmployeeId == x.Id && x.TerminateDate == null).Select(x => new
                    {
                        x.EmailAddress,
                        x.FirstName,
                        isTeamManager = x.IsTeamManager ?? false,
                        x.Manager
                    }).FirstOrDefault();
                    var clientIntroManager = employeesRepo.All().Where(x => x.Id == currentClientIntroEmployeeData.Manager && x.TerminateDate == null).FirstOrDefault();
                    emailRecipients += "," + clientIntroManager.EmailAddress;
                }
                else
                {
                    emailRecipients = emailRecipients + ", GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com";
                }
            }
            string emailBody = string.Format(@"<p>Job priority for job order ID <a href=""https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid={0}"">{1} </a> has been updated by <a href=""https://myplusbeta.publicisgroupe.net/Contact?contactid={2}"">{3}</a> from {4} to {5}.</p>", jobOrderId.ToString(), jobOrderId.ToString(), loggedInExtranetUser.Id.ToString(), loggedInExtranetUser.Name, oldPriorityName, newPriorityName);
            //emailUtils.SendMail("i plus <iplus@translateplus.com>", emailRecipients, "Job priority has been updated for job order ID " + jobOrderId.ToString() + " by " + loggedInExtranetUser.Name, emailBody);
            return rowsAffected;
        }

        public async Task<int> CancelJobOrder(int jobOrderId, string extranetUserName, string cancelComments)
        {
            int result = 0;
            try
            {
                var extranetUserInfo = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
                var accessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);
                var loggedInExtranetUser = contactRepo.All().Where(x => x.Id == extranetUserInfo.DataObjectId && x.DeletedDate == null).FirstOrDefault();
                var jobOrder = orderRepository.All().Where(x => x.Id == jobOrderId && x.DeletedDate == null).FirstOrDefault();
                var jobItems = jobitemsRepo.All().Where(x => x.JobOrderId == jobOrder.Id).ToList();
                result = 0; // no. of rows affected.

                //var jobsData = (from order in jobOrder
                //                join jobs in jobItems on order.Id equals jobs.JobOrderId
                //                select new
                //                {
                //                    order,
                //                    jobs
                //                }).ToList();
                if (jobOrder != null)
                {
                    if (jobItems.Count() > 0)
                    {
                        foreach (var item in jobItems)
                        {
                            if (item != null)
                            {
                                var clientReviewer = GetTranslatorOrReviewerName(jobOrderId, item.Id);
                                if (item.LinguisticSupplierOrClientReviewerId != null)
                                {
                                    if (item.SupplierSentWorkDateTime != null && item.SupplierCompletedItemDateTime == null)
                                    {
                                        var linguisticSupplier = linguisticSupRepo.All().Where(x => x.Id == item.LinguisticSupplierOrClientReviewerId).FirstOrDefault();
                                        string emailRecipient = linguisticSupplier.EmailAddress;
                                        string ccEmailRecipient = employeesRepo.All().Where(x => x.Id == jobOrder.ProjectManagerEmployeeId).FirstOrDefault().EmailAddress;
                                        string subject = $"Job item {jobOrder.Id} ({jobOrder.JobName}) has been cancelled by the client";

                                        string emailBody = string.Format("<p>Dear {0},<br /><br />" +
                                        "Our client has just requested that we cancel job item {1} ({2}). Sorry for any inconvenience! " +
                                        "(This is an automatic message because the client has used the 'Cancel' function in i plus.)<br /><br />" +
                                        "We kindly ask you to stop work right away, and to return to us any work you have completed so far. " +
                                        "We’ll be in touch to discuss further details as soon as possible.<br /><br />Thank you for your assistance.",
                                        linguisticSupplier.MainContactFirstName, item.Id.ToString(), jobOrder.JobName);

                                        //emailUtils.SendMail("i plus <iplus@translateplus.com>", emailRecipient, subject, emailBody, true, CCRecipients: ccEmailRecipient, RequestReadReceipt: true);

                                        /* Cancel job item */
                                        item.SupplierSentWorkDateTime = null;
                                        item.SupplierAcceptedWorkDateTime = null;
                                        item.LastModifiedDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                                        //item.LastModifiedByEmployeeId = 
                                        result = await jobitemsRepo.SaveChangesAsync();
                                    }
                                }
                                if (clientReviewer != null)
                                {
                                    if (item.SupplierSentWorkDateTime != null && item.SupplierCompletedItemDateTime == null)
                                    {
                                        string emailRecipient = clientReviewer.EmailAddress;
                                        string ccEmailRecipient = employeesRepo.All().Where(x => x.Id == jobOrder.ProjectManagerEmployeeId).FirstOrDefault().EmailAddress + ", " + loggedInExtranetUser.EmailAddress;
                                        string subject = "";
                                        string emailBody = "";
                                        if (item.LanguageServiceId == 21)
                                        {
                                            subject = string.Format("Review item {0} ({1}) has been cancelled by {2}", item.Id.ToString(), jobOrder.JobName, loggedInExtranetUser.Name);
                                            //emailBody = string.Format("<p>Dear {0},<br /><br />{1} has just requested that review is no longer required for review item {2} ({3}), as the job has been cancelled.<br /><br />Therefore no further work on this is required. We’ll be in touch to discuss further details if necessary.<br /><br />Thank you for your assistance.",
                                            //    clientReviewer.Name, loggedInExtranetUser.Name, item.Id.ToString(), jobOrder.JobName);
                                        }
                                        else if (item.LanguageServiceId == 1)
                                        {
                                            subject = string.Format("Job item {0} ({1}) has been cancelled by {2}", item.Id.ToString(), jobOrder.JobName, loggedInExtranetUser.Name);
                                            //emailBody = string.Format("<p>Dear {0},<br /><br />{1} has just requested that translation is no longer required for job item {2} ({3}), as the job has been cancelled.<br /><br />Therefore no further work on this is required. We’ll be in touch to discuss further details if necessary.<br /><br />Thank you for your assistance.",
                                            //    clientReviewer.Name, loggedInExtranetUser.Name, item.Id.ToString(), jobOrder.JobName);
                                        }
                                        //emailUtils.SendMail("i plus <iplus@translateplus.com>", emailRecipient, subject, emailBody, true, CCRecipients: ccEmailRecipient, RequestReadReceipt: true);

                                        /* Cancel job item */
                                        item.SupplierSentWorkDateTime = null;
                                        item.SupplierAcceptedWorkDateTime = null;
                                        item.LastModifiedDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                                        //item.LastModifiedByEmployeeId = extranetUserInfo.DataObjectId;
                                        //item.DeletedByEmployeeId = extranetUserInfo.DataObjectId;
                                        result = await jobitemsRepo.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }

                    /* Design Plus */
                    if (jobOrder.DesignPlusFileId != null)
                    {

                        var designPlusFiles = (from cdFile in clientDesignFileRepo.All()
                                               join items in jobItems on cdFile.JobItemId equals items.Id
                                               join orders in orderRepository.All() on items.JobOrderId equals orders.Id
                                               where cdFile.DeletedDateTime == null && items.DeletedDateTime == null && orders.DeletedDate == null && orders.Id == jobOrderId
                                               select new
                                               {
                                                   cdFile,
                                                   items,
                                                   orders
                                               }).ToList();
                        var listDesignPlusFiles = designPlusFiles.Select(x => x.cdFile).ToList();
                        foreach (var file in listDesignPlusFiles)
                        {
                            if (file.ReviewId != null)
                            {
                                var dpReviewJob = dpReviewJobRepo.All().Where(x => x.Id == file.ReviewId).FirstOrDefault();
                                if (dpReviewJob.ReviewCompletedDateTime == null)
                                {
                                    var reviewer = contactRepo.All().Where(x => x.Id == dpReviewJob.ReviewerId && x.DeletedDate == null).FirstOrDefault();
                                    string dpEmailSubject = string.Format("design plus review item {0} has been cancelled by {1}", dpReviewJob.Id, loggedInExtranetUser.Name);
                                    string dpEmailBody = string.Format("<p> Dear {0},<br/><br/>{1} has just requested that review is no longer required for review item {2} ({3}), as the job has been cancelled.<br/><br/> Therefore no further work on this is required.We’ll be in touch to discuss further details if necessary.< br />< br /> Thank you for your assistance.",
                                        reviewer.Name, loggedInExtranetUser.Name, dpReviewJob.Id, file.DocumentName);

                                    /*Deleting record*/
                                    dpReviewJob.DeletedDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                                    dpReviewJob.DeletedBy = extranetUserInfo.DataObjectId;
                                    await dpReviewJobRepo.SaveChangesAsync();

                                    // Sending mail to reviewer
                                    //emailUtils.SendMail("i plus <iplus@translateplus.com>", reviewer.EmailAddress, dpEmailSubject, dpEmailBody, true, CCRecipients: loggedInExtranetUser.EmailAddress, RequestReadReceipt: true);
                                }
                            }
                            /* Deleting client design file */
                            file.DeletedDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                            file.DeletedByClientId = extranetUserInfo.DataObjectId;
                            result = await clientDesignFileRepo.SaveChangesAsync();
                        }
                    }

                    string jobUnderCancellation = "UNDER CANCELLATION - " + jobOrder.JobName;
                    // Updating order under cancellation.
                    var clientInvoice = clientInvoiceRepo.All().Where(x => x.Id == jobOrder.ClientInvoiceId && x.DeletedDateTime == null).FirstOrDefault();
                    if (/*clientInvoice == null || */(clientInvoice != null && clientInvoice.IsFinalised))
                    {
                        throw new Exception("The order ID, " + jobOrderId.ToString() + ", has already been invoiced.You cannot modify details for an invoiced and finalised order.");
                    }
                    // Updating JobOrder
                    jobOrder.JobName = jobUnderCancellation;
                    jobOrder.LastModifiedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                    jobOrder.LastModifiedByEmployeeId = globalVariables.iplusEmployeeID;
                    await orderRepository.SaveChangesAsync();

                    /* sending mail to contacts */
                    var projectManager = employeesRepo.All().Where(x => x.Id == jobOrder.ProjectManagerEmployeeId && x.TerminateDate == null).FirstOrDefault();
                    string recipients = projectManager.FirstName;
                    string emailRecipients = projectManager.EmailAddress;
                    var jobOrderContact = contactRepo.All().Where(x => x.Id == jobOrder.ContactId).FirstOrDefault();
                    // Getting sales manager contacts
                    var empOwnerships = new Enumerations.EmployeeOwnerships[]
                    {
                    Enumerations.EmployeeOwnerships.SalesNewBusinessLead,
                    Enumerations.EmployeeOwnerships.SalesAccountManagerLead
                    };
                    DateTime timeZone = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                    var salesOwner = await ownershipsLogicService.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(jobOrder.ContactId, Enumerations.DataObjectTypes.Org, empOwnerships, timeZone);
                    var salesEmployeeIds = salesOwner.Select(x => x.EmployeeId);
                    var salesEmployeeData = employeesRepo.All().Where(x => salesEmployeeIds.Contains(x.Id)).Select(x => new
                    {
                        x.EmailAddress,
                        x.FirstName
                    }).ToList();

                    if (salesEmployeeData.Count() != 0)
                    {
                        foreach (var salesEmployee in salesEmployeeData)
                        {
                            emailRecipients += ", " + salesEmployee.EmailAddress;
                            recipients += " and " + salesEmployee.FirstName;
                        }
                    }
                    string emailSubject = string.Format("Job order {0} (for {1}) has been cancelled by {2}", jobOrderId.ToString(),
                                                      jobOrderContact.Name, loggedInExtranetUser.Name);

                    string contactHref = "<a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + loggedInExtranetUser.Id.ToString() + "\">" + loggedInExtranetUser.Name + "</a>";
                    string orderHref = "<a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + jobOrderId.ToString() + "\">" + jobOrderId.ToString() + "</a>";

                    string emailCancellationBody = string.Format("<p>Dear {0},<br /><br />" +
                            "{1} has just used the 'Cancel' function in i plus to ask us to stop work on job order {2} ('{3}').<br /><br />" +
                            "E-mails have been automatically sent to all linguistic suppliers and/or client reviewers where work is currently in progress. " +
                            "The job order has not been deleted but has been marked with 'UNDER CANCELLATION' in the job name.<br /><br />" +
                            "You will need to discuss with the client whether we still need to apply some charge, based on the progress made by " +
                            "linguists and what they may charge us for part-completion of the work (and where we may have already received fully " +
                            "completed work before the client cancelled the job).<br /><br />" +
                            "If no charges are applied then you can delete the job in the intranet; if we do apply charges then please " +
                            " remove 'UNDER CANCELLATION' from the job name and update details accordingly before marking it as complete.<br /><br />Comments:<br />{4}",
                            recipients, contactHref, orderHref, jobOrder.JobName, cancelComments);

                    //emailUtils.SendMail("i plus <iplus@translateplus.com>", emailRecipients, emailSubject, emailCancellationBody, true, CCRecipients: emailRecipients, RequestReadReceipt: true);



                    // maybe popup this in a success modal.

                    //    String.Format("Thank you. The project manager ({0}) and linguists have been notified that this job has been cancelled. " & _
                    //                   "The job will appear as ""UNDER CANCELLATION"" while we complete any necessary administrative procedures.", _
                    //                   "<a class=""ourservicesblue"" href=""mailto:" & JobOrderToCancel.ProjectManager.EmailAddress & """>" & JobOrderToCancel.ProjectManager.FullName & "</a>")

                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public async Task<int> SubmitReview(int jobItemId, int reviewerId, DateTime? deadlineDate, string extranetUserName)
        {
            var extranetUserInfo = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
            var accessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);
            var loggedInExtranetUser = contactRepo.All().Where(x => x.Id == extranetUserInfo.DataObjectId && x.DeletedDate == null).FirstOrDefault();
            var jobItem = jobitemsRepo.All().Where(x => x.Id == jobItemId).FirstOrDefault();
            string reviewerName = contactRepo.All().Where(x => x.Id == reviewerId).FirstOrDefault().Name;
            var extranetClientReviewer = extranetUserRepo.All().Where(x => x.DataObjectId == reviewerId).FirstOrDefault();
            DateTime? supplierDeadline = deadlineDate;
            if (deadlineDate != null)
            {
                supplierDeadline = TimeZoneInfo.ConvertTime(Convert.ToDateTime(deadlineDate), TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            }


            jobItem.SupplierIsClientReviewer = true;
            jobItem.LinguisticSupplierOrClientReviewerId = reviewerId;
            jobItem.SupplierCompletionDeadline = supplierDeadline;

            // For batch operation

            //Dim AllJobItems As New List(Of TPJobItem)
            //AllJobItems.Add(ThisJobItem)

            //Dim CRBatchFile As New TPProcessAutomationBatch("iplus", "ITSupport@translateplus.com")
            //Dim CRTask As New TPProcessAutomationTask(TPProcessAutomationTask.AutomationTaskTypes.SendJobItemForClientReviewViaExtranet, JobItemIDs:= AllJobItems)
            //CRBatchFile.AppendTask(CRTask)
            //CRBatchFile.Commit()
            var jobOrder = orderRepository.All().Where(x => x.Id == jobItem.JobOrderId).FirstOrDefault();
            var jobOrderContact = contactRepo.All().Where(x => x.Id == jobOrder.ContactId).FirstOrDefault();
            string recipients = loggedInExtranetUser.EmailAddress;
            if (loggedInExtranetUser.EmailAddress != jobOrderContact.EmailAddress)
            {
                recipients += ", " + jobOrderContact.EmailAddress;
            }

            string emailBody = string.Format("<p>Dear {0},<br/><br/> Client review job item <b>{1}</b> (forming part of order number {2}, ('{3}')) has been sent for review to your assigned reviewer" +
                                                            " <b>{4}</b>, with a deadline of <b>{5}</b>. <br/><br/>" +
                                                              "To see further details of this job, please log in via https://flowplus.translateplus.com/ and access the job via 'Project status' page.</p>",
                                                               loggedInExtranetUser.Name, jobItemId.ToString(), jobOrder.Id.ToString(), jobOrder.JobName,
                                                               reviewerName, jobItem.SupplierCompletionDeadline.ToString() +
                                                               "(" + extranetClientReviewer.DefaultTimeZone + ")" + " on " + jobItem.SupplierCompletionDeadline.ToString());

            //emailUtils.SendMail("i plus <iplus@translateplus.com>", recipients, string.Format("Client review job item {0} has been sent for review", jobItemId.ToString()), emailBody);
            // update review status to "SentToReviewerAwaitingAcceptance"
            jobItem.SupplierSentWorkDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            await jobitemsRepo.SaveChangesAsync();
            return 1;
        }
        public string ExtranetAndWSDirectoryPathForApp(int jobOrderId)
        {
            string PathToReturn = "";
            var jobOrder = orderRepository.All().Where(x => x.Id == jobOrderId).FirstOrDefault();
            string ContactDirSearchPattern = jobOrder.ContactId.ToString() + "*";
            string OrderDirSearchPattern = jobOrder.Id.ToString() + "*";

            string ExtranetBaseDir = Path.Combine(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Contacts"), jobOrder.ContactId.ToString());
            string ContactDirPath = ExtranetBaseDir;
            DirectoryInfo DirInfo = new DirectoryInfo(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Contacts"));
            if (Directory.Exists(ExtranetBaseDir) == false)
            {
                PathToReturn = "";
            }
            else
            {
                DirInfo = new DirectoryInfo(ContactDirPath);
                DirectoryInfo[] MatchingEnquiryDirs = DirInfo.GetDirectories(OrderDirSearchPattern, SearchOption.TopDirectoryOnly);
                if (MatchingEnquiryDirs.Count() == 0)
                {
                    PathToReturn = "";
                }
                else
                {
                    PathToReturn = MatchingEnquiryDirs[0].FullName;
                }
            }
            if (PathToReturn == "" && jobOrder.SubmittedDateTime > new DateTime(2018, 1, 1))
            {
                ContactDirPath = Path.Combine(new DirectoryInfo(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Contacts")).FullName, jobOrder.ContactId.ToString());
                Directory.CreateDirectory(Path.Combine(ContactDirPath, jobOrderId.ToString()));
                PathToReturn = Path.Combine(ContactDirPath, jobOrderId.ToString());
            }
            return PathToReturn;
        }
        public string GetSourceFilesForJobOrder(int jobOrderId)
        {
            string sourceFiles = "";
            try
            {
                var sourceDirectoryInfo = new DirectoryInfo(Path.Combine(ExtranetAndWSDirectoryPathForApp(jobOrderId), "Source"));
                if (sourceDirectoryInfo.Exists)
                {
                    var sourceFileSubFolders = sourceDirectoryInfo.GetDirectories();
                    if (sourceFileSubFolders.Count() > 0)
                    {
                        for (int i = 0; i < sourceFileSubFolders.Count(); i++)
                        {
                            if (sourceFileSubFolders[i].GetFiles().Count() > 0)
                            {
                                sourceFiles += string.Format(@"<strong>{0}:</strong> <a href=""javascript:downloadJobOrderSourceOrRefFile({1}, {2}, 'Source')"" class=""ourservicesblue"">{3}</a><br />",
                                         Convert.ToString(i + 1), Convert.ToString(i + 1), Convert.ToString(jobOrderId), sourceFileSubFolders[i].GetFiles()[0].Name);
                                //sourceFiles += string.Format(@"<b>{0}:</b> <a href=""DownloadOrderSourceFile.aspx?FileIndex={1}&OrderID={2}"" class=""ourservicesblue"">{3}</a><br />",
                                //         Convert.ToString(i + 1), Convert.ToString(i + 1), Convert.ToString(jobOrderId), sourceFileSubFolders[i].GetFiles()[0].Name);
                            }
                        }
                    }
                    else if (sourceDirectoryInfo.GetFiles().Count() > 0)
                    {
                        sourceFiles += string.Format(@"<a href=""javascript:downloadJobOrderSourceOrRefFile({0}, '', 'Source')"" class=""ourservicesblue"">{1}</a><br />",
                                        Convert.ToString(jobOrderId), sourceDirectoryInfo.GetFiles()[0].Name);
                        //sourceFiles += string.Format(@"<a href=""DownloadOrderSourceFile.aspx?OrderID={0}"" class=""ourservicesblue"">{1}</a><br />",
                        //                 Convert.ToString(jobOrderId), sourceDirectoryInfo.GetFiles()[0].Name);
                    }
                }
                if (sourceFiles != "")
                {
                    //need to write equivalent C# : code removes trailing <br>   
                    //SourceFilesLiteralString = Strings.Left(SourceFilesLiteralString, Len(SourceFilesLiteralString) - 6)
                }
            }
            catch (Exception)
            {
                throw;
            }
            return sourceFiles;
        }
        public string GetReferenceFilesForJobOrder(int jobOrderId)
        {
            string referenceFiles = "";
            try
            {
                var jobOrder = orderRepository.All().Where(x => x.Id == jobOrderId).FirstOrDefault();
                var refDirectoryInfo = new DirectoryInfo(Path.Combine(ExtranetAndWSDirectoryPathForApp(jobOrderId), "Reference"));
                if (refDirectoryInfo.Exists)
                {
                    var refFiles = refDirectoryInfo.GetFiles("*", SearchOption.AllDirectories);
                    for (int i = 0; i < refFiles.Count(); i++)
                    {
                        if (refFiles[i].Name == "Reference files.zip" && jobOrder.DesignPlusFileId != null)
                        {
                            referenceFiles += string.Format(@"<a href=""javascript:downloadJobOrderSourceOrRefFile({0}, {1}, 'Reference')"" class=""ourservicesblue"">{2}</a> <br />",
                            Convert.ToString(i + 1), Convert.ToString(jobOrderId), refFiles[i].Name);
                            //referenceFiles += string.Format(@"<a href=""DownloadOrderSourceFile.aspx?FileIndex={0}&OrderID={1}&RefFileDownload=1"" class=""ourservicesblue"">{2}</a> <br />",
                            //Convert.ToString(i + 1), Convert.ToString(jobOrderId), refFiles[i].Name);
                        }
                        else if (refFiles[i].Name != "Reference files.zip" && jobOrder.DesignPlusFileId == null)
                        {
                            referenceFiles += string.Format(@"<a href=""javascript:downloadJobOrderSourceOrRefFile({0}, {1}, 'Reference')"" class=""ourservicesblue"">{2}</a> <br />",
                            Convert.ToString(i + 1), Convert.ToString(jobOrderId), refFiles[i].Name);
                            //referenceFiles += string.Format(@"<a href=""DownloadOrderSourceFile.aspx?FileIndex={0}&OrderID={1}&RefFileDownload=1"" class=""ourservicesblue"">{2}</a> <br />",
                            // Convert.ToString(i + 1), Convert.ToString(jobOrderId), refFiles[i].Name);
                        }
                    }
                }
                if (referenceFiles != "")
                {
                    //need to write equivalent C# : code removes trailing <br>
                    //RefFilesLiteralString = Strings.Left(RefFilesLiteralString, Len(RefFilesLiteralString) - 6)
                }
            }
            catch (Exception)
            {

                throw;
            }

            return referenceFiles;
        }
        public string GetTargetFilesForJobOrder(int jobOrderId)
        {
            string targetFiles = "";
            try
            {
                bool filesToCollect = false;
                var jobOrder = orderRepository.All().Where(x => x.Id == jobOrderId).FirstOrDefault();
                var jobItems = jobitemsRepo.All().Where(x => x.JobOrderId == jobOrderId);
                var TargetDirectoryInfo = new DirectoryInfo(Path.Combine(Path.Combine(ExtranetAndWSDirectoryPathForApp(jobOrderId), "Collection"), "TargetFiles"));
                if (TargetDirectoryInfo.Exists && TargetDirectoryInfo.GetFiles("*.zip").Count() == 1)
                {
                    targetFiles = String.Format(@"<a href=""javascript:downloadJobOrder({0})"" class=""ourservicesblue"">{1}</a><br />",
                        Convert.ToString(jobOrderId), "Download completed job items");
                }
                else
                {
                    string baseOrderPath = ExtranetAndWSDirectoryPathForApp(jobOrderId);
                    if (baseOrderPath != "" && Directory.Exists(baseOrderPath) == true)
                    {
                        baseOrderPath = Path.Combine(baseOrderPath, "Collection");
                    }
                    if (baseOrderPath != "" && Directory.Exists(baseOrderPath) == true)
                    {
                        foreach (var item in jobItems)
                        {
                            if (item.LanguageServiceId != 21) //Client review
                            {
                                string ItemDirSearchPattern = item.Id.ToString() + "*";
                                string ItemDirPath = "";
                                DirectoryInfo DirInfo = new DirectoryInfo(baseOrderPath);
                                var MatchingItemDirs = DirInfo.GetDirectories(ItemDirSearchPattern, System.IO.SearchOption.TopDirectoryOnly);
                                if (MatchingItemDirs.Count() > 0)
                                {
                                    ItemDirPath = MatchingItemDirs[0].FullName;
                                    string CollectionDirPath = Path.Combine(ItemDirPath, "Status" + item.ExtranetClientStatusId);
                                    if (Directory.Exists(CollectionDirPath))
                                    {
                                        var CollectionDirInfo = new DirectoryInfo(CollectionDirPath);
                                        var CollectionFileInfo = CollectionDirInfo.GetFiles();
                                        if (CollectionFileInfo.Count() > 0)
                                        {
                                            filesToCollect = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (filesToCollect)
                    {
                        targetFiles = string.Format(@"<a href=""javascript:downloadJobOrder({0})"" class=""ourservicesblue"">{1}</a><br />",
                             Convert.ToString(jobOrderId), "Download completed job items");
                    }
                }

                //' Do not show download link for Expolink group
                //If JobOrderToShow.OrderContact.ParentOrg.OrgGroup.ID = 18496 Then
                //    TargetFilesTableRow.Visible = False
            }
            catch (Exception ex)
            {
                throw;
            }


            return targetFiles;
        }

        /*
         * @params
         * fileDownloadType = Source, Reference
         * fileIndex = index position of file
         */
        public FileContentResult DownloadOrderSourceFile(int jobOrderId, string extranetUserName, string fileDownloadType, string fileIndex)
        {
            FileContentResult fileToReturn = null;
            try
            {
                bool isAllowedToViewOrDownload = false;
                var jobOrder = orderRepository.All().Where(x => x.Id == jobOrderId).FirstOrDefault();
                var loggedInExtranetUser = extranetUserRepo.All().Where(x => x.UserName == extranetUserName).FirstOrDefault();
                var loggedInExtranetUserContact = contactRepo.All().Where(x => x.Id == loggedInExtranetUser.DataObjectId).FirstOrDefault();
                var jobOrderContact = contactRepo.All().Where(x => x.Id == jobOrder.ContactId).FirstOrDefault();
                var extranetAccessLevel = extranetAccessLevelsRepo.All().Where(x => x.Id == loggedInExtranetUser.AccessLevelId).FirstOrDefault();

                var jobOrderContactOrgGroup = (from order in orderRepository.All()
                                               join contact in contactRepo.All() on order.ContactId equals contact.Id
                                               join org in orgRepo.All() on contact.OrgId equals org.Id
                                               join orgGrp in orgGroupRepo.All() on org.OrgGroupId equals orgGrp.Id
                                               select new
                                               {
                                                   jobOrderId = order.Id,
                                                   contactId = contact.Id,
                                                   orgId = org.Id,
                                                   orgGrp = orgGrp
                                               }).FirstOrDefault();

                var loggedInExtUserOrgGroup = (from extUser in extranetUserRepo.All().Where(x => x.UserName == extranetUserName)
                                               join contact in contactRepo.All() on extUser.DataObjectId equals contact.Id
                                               join org in orgRepo.All() on contact.OrgId equals org.Id
                                               join orgGrp in orgGroupRepo.All() on org.OrgGroupId equals orgGrp.Id
                                               select new
                                               {
                                                   extUser = extUser,
                                                   contactId = contact.Id,
                                                   orgId = org.Id,
                                                   orgGrp = orgGrp
                                               }).FirstOrDefault();


                if (jobOrder == null)
                {
                    //Redirect to Access Denied
                }
                else
                {
                    if (loggedInExtranetUser.DataObjectTypeId == Convert.ToInt32(Enumerations.DataObjectTypes.Contact))
                    {
                        if (loggedInExtranetUser.DataObjectId == jobOrder.ContactId)
                            isAllowedToViewOrDownload = true;
                        else if (loggedInExtranetUserContact.OrgId == jobOrderContact.OrgId
                            && extranetAccessLevel.CanDownloadOtherOrgCompletedOrders
                            && (jobOrder.IsHighlyConfidential == false))
                        {
                            isAllowedToViewOrDownload = true;
                        }
                        else if ((loggedInExtUserOrgGroup.orgGrp != null && jobOrderContactOrgGroup.orgGrp != null)
                            && (loggedInExtUserOrgGroup.orgGrp.Id == jobOrderContactOrgGroup.orgGrp.Id)
                            && extranetAccessLevel.CanDownloadOtherGroupCompletedOrders
                            && jobOrder.IsHighlyConfidential == false)
                        {
                            isAllowedToViewOrDownload = true;
                        }
                    }

                    if (isAllowedToViewOrDownload == false)
                    {
                        // Redirect to Access Denied page
                    }
                    else
                    {
                        // Downloads Source file
                        if (fileDownloadType != "Reference")
                        {
                            //checking where file is residing
                            string SourceFileFolderPath = Path.Combine(Path.Combine(ExtranetAndWSDirectoryPathForApp(jobOrderId), "Source"), "File" + fileIndex);
                            if (fileIndex == "")
                            {
                                SourceFileFolderPath = Path.Combine(Path.Combine(ExtranetAndWSDirectoryPathForApp(jobOrderId), "Source"));
                            }
                            if (!Directory.Exists(SourceFileFolderPath))
                            {
                                // Redirect to Access Denied page.
                            }
                            else
                            {
                                var SourceFileDirectoryInfo = new DirectoryInfo(SourceFileFolderPath);
                                if (SourceFileDirectoryInfo.GetFiles().Count() == 0)
                                {
                                    // Redirect to access denied page.
                                }
                                var SharedFileFullPath = SourceFileDirectoryInfo.GetFiles()[0].FullName;
                                fileToReturn = DownloadFile(SharedFileFullPath);

                            }
                        }
                        //Downloads reference file
                        else
                        {
                            string RefFilePath = Path.Combine(ExtranetAndWSDirectoryPathForApp(jobOrderId), "Reference");
                            if (!Directory.Exists(RefFilePath))
                            {
                                // Redirect to Access Denied page.
                            }
                            else
                            {
                                var RefFileDirectory = new DirectoryInfo(RefFilePath);
                                RefFilePath = RefFileDirectory.GetFiles("*", SearchOption.AllDirectories)[Convert.ToInt32(fileIndex) - 1].FullName;
                                if (!File.Exists(RefFilePath))
                                {
                                    // Redirect to Access Denied page.
                                }
                                else
                                {
                                    fileToReturn = DownloadFile(RefFilePath);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return fileToReturn;
        }

        public async Task<FileContentResult> DownloadJobOrder(int jobOrderId, string extranetUserName)
        {
            FileContentResult fileToReturn = null;
            try
            {
                bool isAllowedToViewOrDownload = false;
                var jobOrder = orderRepository.All().Where(x => x.Id == jobOrderId).FirstOrDefault();
                var jobItems = jobitemsRepo.All().Where(x => x.JobOrderId == jobOrderId).ToList();
                var loggedInExtranetUser = extranetUserRepo.All().Where(x => x.UserName == extranetUserName).FirstOrDefault();
                var loggedInExtranetUserContact = contactRepo.All().Where(x => x.Id == loggedInExtranetUser.DataObjectId).FirstOrDefault();
                var jobOrderContact = contactRepo.All().Where(x => x.Id == jobOrder.ContactId).FirstOrDefault();
                var extranetAccessLevel = extranetAccessLevelsRepo.All().Where(x => x.Id == loggedInExtranetUser.AccessLevelId).FirstOrDefault();

                var jobOrderContactOrgGroup = (from order in orderRepository.All()
                                               join contact in contactRepo.All() on order.ContactId equals contact.Id
                                               join org in orgRepo.All() on contact.OrgId equals org.Id
                                               join orgGrp in orgGroupRepo.All() on org.OrgGroupId equals orgGrp.Id
                                               select new
                                               {
                                                   jobOrderId = order.Id,
                                                   contactId = contact.Id,
                                                   orgId = org.Id,
                                                   orgGrp = orgGrp
                                               }).FirstOrDefault();

                var loggedInExtUserOrgGroup = (from extUser in extranetUserRepo.All().Where(x => x.UserName == extranetUserName)
                                               join contact in contactRepo.All() on extUser.DataObjectId equals contact.Id
                                               join org in orgRepo.All() on contact.OrgId equals org.Id
                                               join orgGrp in orgGroupRepo.All() on org.OrgGroupId equals orgGrp.Id
                                               select new
                                               {
                                                   extUser = extUser,
                                                   contactId = contact.Id,
                                                   orgId = org.Id,
                                                   orgGrp = orgGrp
                                               }).FirstOrDefault();


                if (jobOrder == null)
                {
                    //Redirect to Access Denied
                }
                else
                {
                    if (loggedInExtranetUser.DataObjectTypeId == Convert.ToInt32(Enumerations.DataObjectTypes.Contact))
                    {
                        if (loggedInExtranetUser.DataObjectId == jobOrder.ContactId)
                            isAllowedToViewOrDownload = true;
                        else if (loggedInExtranetUserContact.OrgId == jobOrderContact.OrgId
                            && extranetAccessLevel.CanDownloadOtherOrgCompletedOrders
                            && (jobOrder.IsHighlyConfidential == false))
                        {
                            isAllowedToViewOrDownload = true;
                        }
                        else if ((loggedInExtUserOrgGroup.orgGrp != null && jobOrderContactOrgGroup.orgGrp != null)
                            && (loggedInExtUserOrgGroup.orgGrp.Id == jobOrderContactOrgGroup.orgGrp.Id)
                            && extranetAccessLevel.CanDownloadOtherGroupCompletedOrders
                            && jobOrder.IsHighlyConfidential == false)
                        {
                            isAllowedToViewOrDownload = true;
                        }
                    }

                    if (isAllowedToViewOrDownload == false)
                    {
                        // Redirect to Access Denied page
                    }
                    else
                    {
                        string BaseOrderPath = ExtranetAndWSDirectoryPathForApp(jobOrderId);
                        if (BaseOrderPath == "")
                        {
                            //Redirect to access Denied page.
                        }
                        else
                        {
                            BaseOrderPath = Path.Combine(BaseOrderPath, "Collection");
                        }
                        if (!Directory.Exists(Path.Combine(BaseOrderPath, "TargetFiles")))
                        {
                            Directory.CreateDirectory(Path.Combine(BaseOrderPath, "TargetFiles"));
                        }
                        var ZippedTargetFile = new ZipFile();
                        var FilesToZip = new List<string>();
                        bool FilesCollected = false;

                        foreach (var item in jobItems)
                        {
                            if (item.LanguageServiceId != 21) //Client Review
                            {
                                string ItemDirSearchPattern = item.Id.ToString() + "*";
                                string ItemDirPath = "";
                                var DirInfo = new DirectoryInfo(BaseOrderPath);
                                var MatchingItemDirs = DirInfo.GetDirectories(ItemDirSearchPattern, SearchOption.TopDirectoryOnly);
                                if (MatchingItemDirs.Count() == 0)
                                {
                                    // Redirect to access Denied page.
                                }
                                else
                                {
                                    ItemDirPath = MatchingItemDirs[0].FullName;
                                }
                                string CollectionDirPath = Path.Combine(ItemDirPath, "Status" + item.ExtranetClientStatusId.ToString());
                                if (Directory.Exists(CollectionDirPath))
                                {
                                    try
                                    {
                                        var CollectionDirInfo = new DirectoryInfo(CollectionDirPath);
                                        var CollectionFileInfo = CollectionDirInfo.GetFiles();
                                        FilesToZip.Add(CollectionFileInfo[0].FullName);
                                        FilesCollected = true;
                                        if (item.DownloadedByContactId == null || item.DownloadedByContactId == -1)
                                        {
                                            item.DownloadedByContactId = loggedInExtranetUserContact.Id;
                                            await jobitemsRepo.SaveChangesAsync();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Redirect to access denied page.
                                        throw;
                                    }
                                }
                            }
                        }
                        if (FilesCollected)
                        {
                            ZippedTargetFile.AddFiles(fileNames: FilesToZip, preserveDirHierarchy: false, directoryPathInArchive: "");
                            string ZippedMemoryFolder = Path.Combine(Path.Combine(BaseOrderPath, "TargetFiles"), jobOrderId.ToString()) + ".zip";
                            ZippedTargetFile.Save(ZippedMemoryFolder);
                        }
                        string FinalCollectionFilePath = Path.Combine(Path.Combine(BaseOrderPath, "TargetFiles"), jobOrderId.ToString()) + ".zip";
                        fileToReturn = DownloadFile(FinalCollectionFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return fileToReturn;
        }
        public async Task<FileContentResult> DownloadJobItem(int jobItemId, string extranetUserName)
        {
            FileContentResult fileToReturn = null;
            try
            {
                bool isAllowedToViewOrDownload = false;
                var jobItem = jobitemsRepo.All().Where(x => x.Id == jobItemId).FirstOrDefault();
                var jobOrder = orderRepository.All().Where(x => x.Id == jobItem.JobOrderId).FirstOrDefault();
                var loggedInExtranetUser = extranetUserRepo.All().Where(x => x.UserName == extranetUserName).FirstOrDefault();
                var loggedInExtranetUserContact = contactRepo.All().Where(x => x.Id == loggedInExtranetUser.DataObjectId).FirstOrDefault();
                var jobOrderContact = contactRepo.All().Where(x => x.Id == jobOrder.ContactId).FirstOrDefault();
                var extranetAccessLevel = extranetAccessLevelsRepo.All().Where(x => x.Id == loggedInExtranetUser.AccessLevelId).FirstOrDefault();

                var jobOrderContactOrgGroup = (from order in orderRepository.All()
                                               join contact in contactRepo.All() on order.ContactId equals contact.Id
                                               join org in orgRepo.All() on contact.OrgId equals org.Id
                                               join orgGrp in orgGroupRepo.All() on org.OrgGroupId equals orgGrp.Id
                                               select new
                                               {
                                                   jobOrderId = order.Id,
                                                   contactId = contact.Id,
                                                   org = org,
                                                   orgGrp = orgGrp
                                               }).FirstOrDefault();

                var loggedInExtUserOrgGroup = (from extUser in extranetUserRepo.All().Where(x => x.UserName == extranetUserName)
                                               join contact in contactRepo.All() on extUser.DataObjectId equals contact.Id
                                               join org in orgRepo.All() on contact.OrgId equals org.Id
                                               join orgGrp in orgGroupRepo.All() on org.OrgGroupId equals orgGrp.Id
                                               select new
                                               {
                                                   extUser = extUser,
                                                   contactId = contact.Id,
                                                   orgId = org.Id,
                                                   orgGrp = orgGrp
                                               }).FirstOrDefault();


                if (jobItem == null)
                {
                    //Redirect to Access Denied
                }
                else
                {
                    if (loggedInExtranetUser.DataObjectTypeId == Convert.ToInt32(Enumerations.DataObjectTypes.Contact))
                    {
                        if (loggedInExtranetUser.DataObjectId == jobOrder.ContactId)
                            isAllowedToViewOrDownload = true;
                        else if (loggedInExtranetUserContact.OrgId == jobOrderContact.OrgId
                            && extranetAccessLevel.CanDownloadOtherOrgCompletedOrders
                            && (jobOrder.IsHighlyConfidential == false))
                        {
                            isAllowedToViewOrDownload = true;
                        }
                        else if ((loggedInExtUserOrgGroup.orgGrp != null && jobOrderContactOrgGroup.orgGrp != null)
                            && (loggedInExtUserOrgGroup.orgGrp.Id == jobOrderContactOrgGroup.orgGrp.Id)
                            && extranetAccessLevel.CanDownloadOtherGroupCompletedOrders
                            && jobOrder.IsHighlyConfidential == false)
                        {
                            isAllowedToViewOrDownload = true;
                        }
                    }

                    if (isAllowedToViewOrDownload == false)
                    {
                        // Redirect to Access Denied page
                    }
                    else if (jobItem.ExtranetClientStatusId != 2 && jobItem.ExtranetClientStatusId != 3)
                    {
                        // Redirect to Access Denied page.
                    }
                    else
                    {
                        string FinalCollectionDirPath = ExtranetPathForJobItem(jobItemId);
                        if (!Directory.Exists(FinalCollectionDirPath))
                        {
                            // Redirect to access denied page.
                        }
                        string FinalCollectionFilePath;
                        //if(designPlusFileId != null) // leave this part as design plus functionality is going to be redesigned.
                        //{
                        //    var cdFile = clientDesignFileRepo.All().Where(x => x.Id == designPlusFileId);

                        //}
                        try
                        {
                            var FinalCollectionDirInfo = new DirectoryInfo(FinalCollectionDirPath);
                            var FinalCollectionFileInfo = FinalCollectionDirInfo.GetFiles();
                            FinalCollectionFilePath = FinalCollectionFileInfo[0].FullName;
                        }
                        catch (Exception ex)
                        {
                            // Redirect to access denied page.
                            throw;
                        }
                        try
                        {
                            fileToReturn = DownloadFile(FinalCollectionFilePath);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        finally
                        {
                            // sending internal notification when they have downloaded the file.
                            string InternalNotificationEmailAddresses = "";
                            var empOwnerships = new Enumerations.EmployeeOwnerships[]
                            {
                                Enumerations.EmployeeOwnerships.OperationsLead
                            };
                            var empOwnershipsClientIntroLead = new Enumerations.EmployeeOwnerships[]
                            {
                                Enumerations.EmployeeOwnerships.ClientIntroLead
                            };
                            DateTime timeZone = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                            var opsOwner = await ownershipsLogicService.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(jobOrder.ContactId, Enumerations.DataObjectTypes.Org, empOwnerships, timeZone);
                            var clientIntroOwner = await ownershipsLogicService.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(jobOrder.ContactId, Enumerations.DataObjectTypes.Org, empOwnershipsClientIntroLead, timeZone);
                            var opsOwnerEmployeeId = opsOwner.Select(x => x.EmployeeId);
                            var clientOwnerEmployeeId = clientIntroOwner.Select(x => x.EmployeeId);
                            var opsEmployeeData = employeesRepo.All().Where(x => opsOwnerEmployeeId.Contains(x.Id)).Select(x => new
                            {
                                x.EmailAddress,
                                x.FirstName
                            });
                            var clientOwnerEmployeeData = employeesRepo.All().Where(x => clientOwnerEmployeeId.Contains(x.Id)).Select(x => new
                            {
                                x.EmailAddress,
                                x.FirstName
                            });

                            if (opsOwner == null || opsOwner.Count() == 0)
                            {
                                if (clientOwnerEmployeeData.Count() != 0 && clientOwnerEmployeeData != null)
                                {
                                    InternalNotificationEmailAddresses = clientOwnerEmployeeData.FirstOrDefault().EmailAddress;
                                }
                                else
                                {
                                    var projectManager = employeesRepo.All().Where(x => x.Id == jobOrder.ProjectManagerEmployeeId).FirstOrDefault();
                                    InternalNotificationEmailAddresses = projectManager.EmailAddress;
                                    string emailBody = string.Format(@"<p>Org <a href=""https://myplusbeta.publicisgroupe.net/Organisation?id={0}"">{1}</a> still doesn’t have an Ops lead, " +
                                                                      "even though we have actually now delivered completed work to the client.<br/><br/>" +
                                                                      "Please ensure that this organisation is assigned an Ops Lead right away, to ensure appropriate responsibilities are clear.",
                                                                       jobOrderContactOrgGroup.org.Id.ToString(), jobOrderContactOrgGroup.org.OrgName);
                                    //emailUtils.SendMail("i plus <iplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                    //                string.Format(@"{0} still doesn't have an Ops lead", jobOrderContactOrgGroup.org.OrgName), emailBody);
                                }
                            }
                            else
                            {
                                InternalNotificationEmailAddresses = opsEmployeeData.FirstOrDefault().EmailAddress;
                                string emailBody = string.Format(@"<p>Job item <a href=""https://myplusbeta.publicisgroupe.net/JobItem?Id={0}""> +
                                                       {1} </a> has been downloaded by the contact <a href=""https://myplusbeta.publicisgroupe.net/Contact?contactid={2}""> +
                                                       {3} </a>.</p><p>The file was successfully output to the user's web browser, but we cannot know whether they saved the file to their computer and/or opened it directly.</p>",
                                                        jobItemId.ToString(), jobItemId.ToString(), loggedInExtranetUserContact.Id.ToString(), loggedInExtranetUserContact.Name);

                                //emailUtils.SendMail("i plus <iplus@translateplus.com>", InternalNotificationEmailAddresses,
                                //                       "Job item " + jobItemId.ToString() + " collected", emailBody);
                                if (jobItem.DownloadedByContactId == null || jobItem.DownloadedByContactId == -1)
                                {
                                    jobItem.DownloadedByContactId = loggedInExtranetUserContact.Id;
                                    await jobitemsRepo.SaveChangesAsync();
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return fileToReturn;
        }

        public FileContentResult DownloadFile(string sourcefilePath)
        {
            FileInfo fileInfo = new FileInfo(sourcefilePath);
            byte[] thisFileBytes = File.ReadAllBytes(sourcefilePath);
            FileContentResult file = new FileContentResult(thisFileBytes, "application/octet-stream")
            {
                FileDownloadName = fileInfo.Name
            };
            return file;
        }
        public string ExtranetPathForJobItem(int jobItemId)
        {
            var jobItemData = jobitemsRepo.All().Where(x => x.Id == jobItemId).FirstOrDefault();
            var jobOrderData = orderRepository.All().Where(x => x.Id == jobItemData.JobOrderId).FirstOrDefault();
            string PathToReturn = "";
            string JobOrderDir = ExtranetAndWSDirectoryPathForApp(jobOrderData.Id);
            string CollectionFolder = JobOrderDir + "\\Collection";
            string ItemDirSearchPattern = jobItemId.ToString() + "*";

            var DirInfo = new DirectoryInfo(CollectionFolder);
            var MatchingItemDirsInOldDir = DirInfo.GetDirectories(ItemDirSearchPattern, SearchOption.TopDirectoryOnly);
            if (MatchingItemDirsInOldDir.Count() == 0)
            {
                PathToReturn = "";
            }
            else
            {
                PathToReturn = MatchingItemDirsInOldDir[0].FullName + "\\Status" + jobItemData.ExtranetClientStatusId;
            }
            return PathToReturn;
        }
        public string ExtranetNetworkDrive(int uploadedByClientId, int clientDesignFileId)
        {
            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string NewFolderPath = GlobalVars.NewDesignPlusExternalFolderPath + "\\" + uploadedByClientId + "\\" + clientDesignFileId;
            if (!Directory.Exists(NewFolderPath))
            {
                string OldFolderPath = GlobalVars.OldDesignPlusExternalFolderPath + "\\" + uploadedByClientId + "\\" + clientDesignFileId;
                if (!Directory.Exists(OldFolderPath))
                {
                    string OldestFolderPath = GlobalVars.DesignPlusExternalFolderPath + "\\" + uploadedByClientId + "\\" + clientDesignFileId;
                    if (!Directory.Exists(OldestFolderPath))
                    {
                        return GlobalVars.NewDesignPlusExternalFolderPath;
                    }
                    else
                    {
                        return GlobalVars.DesignPlusExternalFolderPath;
                    }
                }
                else
                {
                    return GlobalVars.OldDesignPlusExternalFolderPath;
                }
            }
            else
            {
                return GlobalVars.NewDesignPlusExternalFolderPath;
            }
        }

        public async Task<JobOrder> UpdateEndclientInformation<JobOrderViewteModel>(int Id, int endclientid, int campaignid, int brandid, int categoryid, short? lastModifiedByUserId)
        {
            var joborder = await GetById(Id);

            // maybe add security check later here and autoMapper
            if (endclientid != -1)
            {
                joborder.EndClientId = endclientid;
            }
            else
            {
                joborder.EndClientId = null;
            }

            if (campaignid != -1)
            {
                joborder.CampaignId = campaignid;
            }
            else
            {
                joborder.CampaignId = null;
            }

            if (brandid != -1)
            {
                joborder.BrandId = brandid;
            }
            else
            {
                joborder.BrandId = null;
            }

            if (categoryid != -1)
            {
                joborder.CategoryId = categoryid;
            }
            else
            {
                joborder.CategoryId = null;
            }

            joborder.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
            joborder.LastModifiedByEmployeeId = lastModifiedByUserId;

            orderRepository.Update(joborder);
            await orderRepository.SaveChangesAsync();

            return joborder;
        }

        public async Task<JobOrder> UpdateSurchargeID<JobOrderViewteModel>(int Id, int surchargeId, short? lastModifiedByUserId)
        {
            var joborder = await GetById(Id);

            // maybe add security check later here and autoMapper
            if (surchargeId != -1)
            {
                joborder.SurchargeId = surchargeId;
            }
            else
            {
                joborder.SurchargeId = null;
            }
            joborder.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
            joborder.LastModifiedByEmployeeId = lastModifiedByUserId;

            orderRepository.Update(joborder);
            await orderRepository.SaveChangesAsync();

            return joborder;
        }

        public async Task<JobOrder> UpdateDiscountID<JobOrderViewteModel>(int Id, int discountId, short? lastModifiedByUserId)
        {
            var joborder = await GetById(Id);

            // maybe add security check later here and autoMapper
            if (discountId != -1)
            {
                joborder.DiscountId = discountId;
            }
            else
            {
                joborder.DiscountId = null;
            }
            joborder.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
            joborder.LastModifiedByEmployeeId = lastModifiedByUserId;

            orderRepository.Update(joborder);
            await orderRepository.SaveChangesAsync();

            return joborder;
        }


        public async Task<JobOrder> CreateNewOrder<Order>(int contactId, short projectManagerId, byte jobOrderChannelId, string jobName,
                                                        string clientNotes, string internalNotes, string clientPONumber,
                                                        string customerSpecificField1Value, string customerSpecificField2Value,
                                                        string customerSpecificField3Value, string customerSpecificField4Value,
                                                        DateTime overallDeliveryDeadlineDate, int overallDeliveryDeadlineHour,
                                                        int overallDeliveryDeadlineMinute, short clientCurrencyID,
                                                        int linkedJobOrderID, bool isATrialProject, bool isACMSProject,
                                                        bool isHighlyConfidential, short setUpByEmployeeID, bool isActuallyOnlyAQuote,
                                                        bool createNetworkJobFolder, string folderTemplateName, string invoicingNotes,
                                                        string orgHFMCodeIS = "", string orgHFMCodeBS = "",
                                                        bool extranetNotifyClientReviewersOfDeliveries = false,
                                                        byte priority = 0, bool printingProject = false,
                                                        int endClientID = 0, int brandID = 0,
                                                        int categoryID = 0, int campaignID = 0, bool isCLS = false, int? DiscountId = null, int? SurchargeId = null, decimal? DiscountAmount = 0, decimal? SurchargeAmount = 0, decimal? OverallChargeToClient = 0, decimal? SubTotalOverallChargeToClient = 0)
        {
            var orderContact = await contactRepo.All().Where(o => o.Id == contactId && o.DeletedDate == null).FirstOrDefaultAsync();

            var org = await orgRepo.All().Where(o => o.Id == orderContact.OrgId).FirstOrDefaultAsync();

            if (orderContact == null)
            {
                throw new Exception("The contact ID, " + contactId.ToString() + ", does not exist in the database. An order must be created against a valid contact.");
            }
            else
            {
                try
                {
                    var overdueInvoices = clientInvoiceService.GetOverdueClientInvoicesForOrgID(orderContact.Id);
                    bool needToSendEmail = false;

                    foreach (ClientInvoice invoice in overdueInvoices.Result)
                    {
                        var thisInvoiceViewModel = clientInvoiceService.GetViewModelById(invoice.Id);

                        if (thisInvoiceViewModel.Result.DueDate.Value.AddMonths(2) < timeZonesService.GetCurrentGMT())
                        {
                            needToSendEmail = true;
                            break;
                        }
                        else
                        {
                            var resultAmount = invoice.FinalValueAmount;

                            if (invoice.InvoiceCurrencyId != 4)
                            {
                                resultAmount = exchangeService.Convert(invoice.InvoiceCurrencyId, 4, invoice.FinalValueAmount.Value);
                            }

                            if (resultAmount > 5000)
                            {
                                needToSendEmail = true;
                                break;
                            }

                        }
                    }

                    if (needToSendEmail == true)
                    {
                        //As requested by Per, only Per and Accounts manager to get this notification
                        //04/05/16 update - now also Abdul to receive this notification
                        //21/06/16 update - now remove Accounts manager to receive this notification
                        //15/09/16 update - as Abdul has left, the Accounts manager has requested for Svetlana to receive this notification
                        //16/02/17 update - as Svetlana has left, the Accounts manager has requested for Renuka, Petya and the Accounts manager to receive this notification
                        string EmailRecipients = "Umer.Nizam@translateplus.com, Atanas.Pashov@translateplus.com, silvina.mihaylova@translateplus.com";
                        emailUtils.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Caution: new job order submitted by overdue debtor",
                                                       String.Format("<p>A new job order has been submitted by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid={0}\">{1}</a> of <a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id={2}\">{3}</a> which meets one of the following criteria:</p><br /><br />" +
                                                       "<ul><li><p>at least one overdue invoice by at least 2 months</p></li><li><p>at least one overdue invoice with the amount of over £5,000</p></li></ul><br /><br />" +
                                                       "<p>Please review this client and decide if we should go ahead with this job order.</p>",
                                                       orderContact.Id.ToString(), orderContact.Name, orderContact.OrgId.ToString(), org.OrgName),
                                                       IsExternalNotification: true);
                    }
                }
                catch (Exception ex)
                {
                    //fail silently
                }

            }

            //also ensure that if a linked job order ID has been supplied, that the job order in 
            //question also exists

            var linkedOrder = await orderRepository.All().Where(o => o.Id == linkedJobOrderID && o.DeletedDate == null).FirstOrDefaultAsync();

            if (linkedJobOrderID != 0 && linkedOrder == null)
            {
                throw new Exception("The linked job order ID, " + linkedJobOrderID.ToString() + ", does not exist in the database. You cannot link this order to a non-existent order.");
            }


            int newOrderID;
            JobOrder newOrder;

            //Input strings should have been limited in the interface but
            //limit as a precaution and trim
            if (jobName != null && jobName.Length > 200) { jobName = jobName.Trim().Substring(0, 200); }

            if (clientNotes != null) { clientNotes = clientNotes.Trim(); }
            if (internalNotes != null) { internalNotes = internalNotes.Trim(); }
            if (clientPONumber != null && clientPONumber.Length > 100) { clientPONumber = clientPONumber.Trim().Substring(0, 100); }
            if (customerSpecificField1Value != null) { customerSpecificField1Value = customerSpecificField1Value.Trim(); }
            if (customerSpecificField2Value != null) { customerSpecificField2Value = customerSpecificField2Value.Trim(); }
            if (customerSpecificField3Value != null) { customerSpecificField3Value = customerSpecificField3Value.Trim(); }
            if (customerSpecificField4Value != null) { customerSpecificField4Value = customerSpecificField4Value.Trim(); }
            if (invoicingNotes != null) { invoicingNotes = invoicingNotes.Trim(); }

            DateTime compositeDeliveryDateTime = new DateTime(overallDeliveryDeadlineDate.Year, overallDeliveryDeadlineDate.Month, overallDeliveryDeadlineDate.Day, overallDeliveryDeadlineHour, overallDeliveryDeadlineMinute, 0);

            if (overallDeliveryDeadlineDate.Minute > 0 && overallDeliveryDeadlineDate.Minute <= 30)
            {
                compositeDeliveryDateTime = new DateTime(overallDeliveryDeadlineDate.Year, overallDeliveryDeadlineDate.Month, overallDeliveryDeadlineDate.Day, overallDeliveryDeadlineDate.Hour, 30, 0);

            }
            else if (overallDeliveryDeadlineDate.Minute > 30 && overallDeliveryDeadlineDate.Minute <= 59)
            {
                compositeDeliveryDateTime = new DateTime(overallDeliveryDeadlineDate.Year, overallDeliveryDeadlineDate.Month, overallDeliveryDeadlineDate.Day, overallDeliveryDeadlineDate.Hour + 1, 0, 0);

            }

            if (customerSpecificField2Value == null && orderContact.OrgId == 138545)
            {
                customerSpecificField2Value = "http://nescafegold.marketforward.com";
            }
            else if (customerSpecificField2Value == null && orderContact.OrgId == 138711) //nestle pure life
            {
                customerSpecificField2Value = "http://nestlepurelife.marketforward.com/";
            }
            else if (customerSpecificField2Value == null && orderContact.OrgId == 138994) //meetic
            {
                customerSpecificField2Value = "http://newbiz-prodigious.marketforward.com/";
            }

            newOrder = new JobOrder()
            {
                ContactId = contactId,
                IsActuallyOnlyAquote = isActuallyOnlyAQuote,
                PrintingProject = printingProject,
                ProjectManagerEmployeeId = projectManagerId,
                JobOrderChannelId = jobOrderChannelId,
                JobName = jobName,
                ClientNotes = clientNotes,
                InternalNotes = internalNotes,
                ClientPonumber = clientPONumber,
                OrgHfmcodeBs = orgHFMCodeBS,
                OrgHfmcodeIs = orgHFMCodeIS,
                CustomerSpecificField1Value = customerSpecificField1Value,
                CustomerSpecificField2Value = customerSpecificField2Value,
                CustomerSpecificField3Value = customerSpecificField3Value,
                CustomerSpecificField4Value = customerSpecificField4Value,
                OverallDeliveryDeadline = compositeDeliveryDateTime,
                Priority = priority,
                ClientCurrencyId = clientCurrencyID,
                InvoicingNotes = invoicingNotes,
                EndClientId = endClientID,
                BrandId = brandID,
                CampaignId = campaignID,
                CategoryId = categoryID,
                LinkedJobOrderId = linkedJobOrderID,
                ExtranetNotifyClientReviewersOfDeliveries = extranetNotifyClientReviewersOfDeliveries,
                IsAtrialProject = isATrialProject,
                IsAcmsproject = isACMSProject,
                IsHighlyConfidential = isHighlyConfidential,
                IsCls = isCLS,
                SubmittedDateTime = timeZonesService.GetCurrentGMT(),
                SetupByEmployeeId = setUpByEmployeeID,
                DiscountId = DiscountId,
                SurchargeId = SurchargeId,
                DiscountAmount = DiscountAmount,
                SurchargeAmount = SurchargeAmount,
                OverallChargeToClient = OverallChargeToClient,
                SubTotalOverallChargeToClient = SubTotalOverallChargeToClient
            };

            await orderRepository.AddAsync(newOrder);
            await orderRepository.SaveChangesAsync();

            newOrderID = newOrder.Id;
            //if user wanted, create the job folder on the network
            if (createNetworkJobFolder == true)
            {
                //do not try to create a job folder unless the folder template
                //has been specified (since an empty folder template name would 
                //result in copying over all the template sub-folders)
                if (folderTemplateName == "")
                {
                    throw new Exception("The job has been created, but the corresponding network job folder could not be created as no folder template was specified.");
                }
                else
                {
                    configureNetworkFolders(folderTemplateName, newOrderID);
                }
            }
            //if (org.OrgGroupId != null)
            //{
            //    if (newOrder.DiscountId == null && orgGroupService.HasAnyVolumeDiscountSetUp(org.OrgGroupId.Value) == true)
            //    {
            //        var currentVolumDiscount = volumeDiscountsService.GetCurrentVolumeDiscountForDataObject(org.OrgGroupId.Value, 3);
            //        //TPQuoteAndOrderDiscountsAndSurchargesLogic.CreateDiscountOrSurcharge(True, 3, "Volume discount", False, CurrentVolumeDiscount.DiscountPercentage,
            //        //NewOrder.ID, True)
            //    }
            //}

            //    If NewOrder.OrderChannel.ID = 6 Or NewOrder.OrderChannel.ID = 7 Or NewOrder.OrderChannel.ID = 8 Or NewOrder.OrderChannel.ID = 15 Or NewOrder.OrderChannel.ID = 16 Or NewOrder.OrderChannel.ID = 17 _
            //Or NewOrder.OrderChannel.ID = 21 Or NewOrder.OrderChannel.ID = 22 Or NewOrder.OrderChannel.ID = 23 Or NewOrder.OrderChannel.ID = 24 Then
            //        TPJobOrdersLogic.UpdateJobOrderAutomationData(NewOrder.ID, JobSetUpDate:= TPTimeZonesLogic.GetCurrentGMT())
            //    End If




            return newOrder;


        }

        public async Task<bool> configureNetworkFolders(string FolderTemplateName, int jobOrderId)
        {
            var order = await orderRepository.All().Where(o => o.Id == jobOrderId).FirstOrDefaultAsync();
            var orderContact = await contactRepo.All().Where(o => o.Id == order.ContactId && o.DeletedDate == null).FirstOrDefaultAsync();
            var org = await orgRepo.All().Where(o => o.Id == orderContact.OrgId).FirstOrDefaultAsync();

            string orgDirSearchPattern = orderContact.OrgId + " *";
            string orgDirPath;
            string orderDirSearchPattern = order.Id.ToString() + " *";
            string jobDriveBaseDirectoryPathForApp = await orgService.JobServerLocationForApp(org.Id);



            if (jobDriveBaseDirectoryPathForApp == globalVariables.SofiaJobDriveBaseDirectoryPathForApp)
            {
                jobDriveBaseDirectoryPathForApp = globalVariables.ParisJobDriveBaseDirectoryPathForApp;
            }

            DirectoryInfo DirInfo = new DirectoryInfo(jobDriveBaseDirectoryPathForApp);
            // find org folder first (job folders should then appear within that)
            DirectoryInfo[] MatchingOrgDirs = DirInfo.GetDirectories(orgDirSearchPattern, SearchOption.TopDirectoryOnly);
            if (MatchingOrgDirs.Count() == 0)
            {
                string newOrgDirSearchPattern = orderContact.OrgId.ToString();
                DirectoryInfo[] newMatchingOrgDirs = DirInfo.GetDirectories(newOrgDirSearchPattern, SearchOption.TopDirectoryOnly);
                if (newMatchingOrgDirs.Count() == 0)
                {
                    // no org folder found, so create it now, along with standard sub-folders for translation
                    // memories, invoices, general info, etc.
                    orgDirPath = Path.Combine(jobDriveBaseDirectoryPathForApp, GeneralUtils.MakeStringSafeForFileSystemPath(orderContact.OrgId.ToString()));
                    Directory.CreateDirectory(orgDirPath);
                    Directory.CreateDirectory(Path.Combine(orgDirPath, "Client Invoices"));
                    Directory.CreateDirectory(Path.Combine(orgDirPath, "Key Client Info"));


                    // update Nov 2011: automatically copy into "Key Client Info" the templated "Client Rules.docx" file
                    // (if this fails, don't fail the whole thing)
                    try
                    {
                        // File.Copy("\\fredcpfspdm0003\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML\Client Rules.docx",
                        // Path.Combine(Path.Combine(OrgDirPath, "Key Client Info"), "Client Rules.docx"))
                        File.Copy(@"\\fredcpfspdm0003\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML\Client Rules.docx", Path.Combine(Path.Combine(orgDirPath, "Key Client Info"), "Client Rules.docx"));
                    }
                    catch
                    {
                    }
                    Directory.CreateDirectory(Path.Combine(orgDirPath, "Memories & Glossaries"));

                    // update July 2014: automatically copy into "Memories & Glossaries" folder the templated "Terminolgy Plan.docx" file
                    // (if this fails, don't fail the whole thing)
                    try
                    {
                        File.Copy(@"\\fredcpfspdm0003\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML\Terminology Plan.docx", Path.Combine(Path.Combine(orgDirPath, "Memories & Glossaries"), "Terminology Plan.docx"));
                    }
                    // File.Copy("\\fredcpfspdm0003\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML\Terminology Plan.docx",
                    // Path.Combine(Path.Combine(OrgDirPath, "Memories & Glossaries"), "Terminology Plan.docx"))
                    catch
                    {
                    }

                    Directory.CreateDirectory(Path.Combine(Path.Combine(orgDirPath, "Key Client Info"), "Invoicing Rules"));
                    // automatically copy into "Invoicing rules" the templated "theDOC.docx" file
                    // (if this fails, don't fail the whole thing)
                    try
                    {
                        // File.Copy("\\fredcpfspdm0003\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML\Invoicing instructions.docx",
                        // Path.Combine(Path.Combine(Path.Combine(OrgDirPath, "Key Client Info"), "Invoicing Rules"), "Invoicing instructions.docx"))
                        File.Copy(@"\\fredcpfspdm0003\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML\Invoicing instructions.docx", Path.Combine(Path.Combine(Path.Combine(orgDirPath, "Key Client Info"), "Invoicing Rules"), "Invoicing instructions.docx"));
                    }
                    catch
                    {
                    }
                }
                else
                    orgDirPath = newMatchingOrgDirs[0].FullName;
            }
            else
                orgDirPath = MatchingOrgDirs[0].FullName;
            // now look for an existing job folder within the org folder
            DirInfo = new DirectoryInfo(orgDirPath);
            DirectoryInfo[] MatchingOrderDirs = DirInfo.GetDirectories(orderDirSearchPattern, SearchOption.TopDirectoryOnly);
            string JobPath = "";
            if (MatchingOrderDirs.Count() == 0)
            {
                // no order folder found, so create it now by copying the template job
                // folder directory structure to the relevant location
                // use 40 characters of the job name (or until the end of a word) otherwise it will crash when creating folders due to windows restriction on folder/file path lengths
                string JobName = order.JobName;
                try
                {
                    if (order.JobName.Length > 40)
                    {
                        int index = 40;
                        while (order.JobName.Length > index)
                        {
                            if ((order.JobName[index] == ' ' & order.JobName[index] != (char)9 && order.JobName[index] != (char)10 &&
                                order.JobName[index] != (char)11 && order.JobName[index] != (char)13 &&
                                order.JobName[index] != (char)32))
                                index += 1;
                            else
                                break;
                        }
                        JobName = order.JobName.Substring(0, index).Trim();
                    }
                }
                catch (Exception ex)
                {
                    if (order.JobName.Length > 40)
                        JobName = order.JobName.Substring(0, 40);
                    else
                        JobName = order.JobName;
                }
                JobPath = Path.Combine(orgDirPath, GeneralUtils.MakeStringSafeForFileSystemPath(order.Id.ToString() + " - " + order.SubmittedDateTime.ToString("d MMM yy")));
                GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName), JobPath, true);

                try
                {
                    if (orderContact.OrgId == 139412)
                    {
                        string MoJTrackerFolderPath = @"\\gblonpfspdm0005\Jobs\136951 - MoJ\Key Client Info\MoJ Tracker";
                        string MoJTrackerJobPath = "";

                        if (order.InternalNotes.Contains("Originally from job order") == false)
                        {
                            MoJTrackerJobPath = Path.Combine(MoJTrackerFolderPath, GeneralUtils.MakeStringSafeForFileSystemPath(order.Id.ToString() + " - " + JobName));
                            Directory.CreateDirectory(MoJTrackerJobPath);
                        }
                        else
                        {
                            string MasterOrderID = "";
                            // Dim FormatRegexPattern As String = "(?<=Originally from job order[ ]?)[0-9]*"
                            string FormatRegexPattern = "(?<=Originally from job order[ ]?)[^ ][0-9]*";

                            if (Regex.Matches(order.InternalNotes, FormatRegexPattern).Count > 0)
                                MasterOrderID = Regex.Matches(order.InternalNotes, FormatRegexPattern)[0].Value;

                            DirectoryInfo MoJTrackerFolder = new DirectoryInfo(MoJTrackerFolderPath);

                            if (MoJTrackerFolder.GetDirectories(MasterOrderID + " *", SearchOption.TopDirectoryOnly).Count() == 1)
                                MoJTrackerFolderPath = MoJTrackerFolder.GetDirectories(MasterOrderID + " *", SearchOption.TopDirectoryOnly)[0].FullName;

                            MoJTrackerJobPath = Path.Combine(MoJTrackerFolderPath, GeneralUtils.MakeStringSafeForFileSystemPath(order.Id.ToString()));
                            GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName), MoJTrackerJobPath, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                }



                try
                {

                    // For Terma A/S add a text file in Delivery folder to indicate FTP delivery only.
                    if (orderContact.OrgId == 82430)
                    {

                        // checking for "03 Delivery" folder
                        if (Directory.Exists(JobPath + @"\03 Delivery") == false)
                        {
                            if (File.Exists(JobPath + @"\03 Delivery\FTP DELIVERY ONLY.txt") == false)
                                File.Create(JobPath + @"\03 Delivery\FTP DELIVERY ONLY.txt").Dispose();
                        }
                        else
                            // checking for "04 Delivery" folder
                            if (Directory.Exists(JobPath + @"\04 Delivery") == false)
                        {
                            if (File.Exists(JobPath + @"\04 Delivery\FTP DELIVERY ONLY.txt") == false)
                                File.Create(JobPath + @"\04 Delivery\FTP DELIVERY ONLY.txt").Dispose();
                        }
                        else
                                // checking for "05 Delivery" folder
                                if (Directory.Exists(JobPath + @"\05 Delivery") == false)
                        {
                            if (File.Exists(JobPath + @"\05 Delivery\FTP DELIVERY ONLY.txt") == false)
                                File.Create(JobPath + @"\05 Delivery\FTP DELIVERY ONLY.txt").Dispose();
                        }
                        else
                                    // checking for "06 Delivery" folder
                                    if (Directory.Exists(JobPath + @"\06 Delivery") == false)
                        {
                            if (File.Exists(JobPath + @"\06 Delivery\FTP DELIVERY ONLY.txt") == false)
                                File.Create(JobPath + @"\06 Delivery\FTP DELIVERY ONLY.txt").Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                try
                {
                    // creating 2 extra folders under General docs folder for two BMW orgs 
                    if (orderContact.OrgId == 139233 | orderContact.OrgId == 139232)
                    {
                        Directory.CreateDirectory(JobPath + @"\General Docs\Orders - email notifications");
                        Directory.CreateDirectory(JobPath + @"\General Docs\PO – XMLs");
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                JobPath = MatchingOrderDirs[0].FullName;
                // For different type of network folder template, check if there is all folders in the main job folder

                // if the type of template selected is : 'IFADWorkflow' 
                if (FolderTemplateName == "IFADWorkflow")
                {
                    // checking for Briefs folder
                    if (Directory.Exists(JobPath + @"\Briefs") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\Briefs");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\Briefs"), JobPath + @"\Briefs", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\02 From");
                    }

                    // checking for client invoice folder
                    if (Directory.Exists(JobPath + @"\Client Invoice") == false)
                        Directory.CreateDirectory(JobPath + @"\Client Invoice");

                    // checking for General Docs folder
                    if (Directory.Exists(JobPath + @"\General Docs") == false)
                        Directory.CreateDirectory(JobPath + @"\General Docs");

                    // checking for Reference Material folder
                    if (Directory.Exists(JobPath + @"\Reference Material") == false)
                        Directory.CreateDirectory(JobPath + @"\Reference Material");
                }
                else if (FolderTemplateName == "Interpreting - standard")
                {

                    // checking for Briefs folder
                    if (Directory.Exists(JobPath + @"\Briefs") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\Briefs");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\Briefs"), JobPath + @"\Briefs", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\02 From");
                    } // checking for Briefs folder loop

                    // checking for client invoice folder
                    if (Directory.Exists(JobPath + @"\Client Invoice") == false)
                        Directory.CreateDirectory(JobPath + @"\Client Invoice");

                    // checking for General Docs folder
                    if (Directory.Exists(JobPath + @"\General Docs") == false)
                        Directory.CreateDirectory(JobPath + @"\General Docs");

                    // checking for Quote folder
                    if (Directory.Exists(JobPath + @"\Quote") == false)
                        Directory.CreateDirectory(JobPath + @"\Quote");

                    // checking for Reference Material folder
                    if (Directory.Exists(JobPath + @"\Reference Material") == false)
                        Directory.CreateDirectory(JobPath + @"\Reference Material");
                }
                else if (FolderTemplateName == "Proofreading only")
                {

                    // checking for "01 Source files" folder
                    if (Directory.Exists(JobPath + @"\01 Source files") == false)
                        Directory.CreateDirectory(JobPath + @"\01 Source files");

                    // checking for "02 Proofreading" folder
                    if (Directory.Exists(JobPath + @"\02 Proofreading") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\02 Proofreading");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\02 Proofreading"), JobPath + @"\02 Proofreading", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\02 Proofreading" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Proofreading" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\02 Proofreading" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\02 Proofreading" + @"\02 From");
                    } // checking for "02 Proofreading" folder loop

                    // checking for "03 Delivery" folder
                    if (Directory.Exists(JobPath + @"\03 Delivery") == false)
                        Directory.CreateDirectory(JobPath + @"\03 Delivery");

                    // checking for Briefs folder
                    if (Directory.Exists(JobPath + @"\Briefs") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\Briefs");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\Briefs"), JobPath + @"\Briefs", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\02 From");
                    } // checking for Briefs folder loop

                    // checking for client invoice folder
                    if (Directory.Exists(JobPath + @"\Client Invoice") == false)
                        Directory.CreateDirectory(JobPath + @"\Client Invoice");

                    // checking for General Docs folder
                    if (Directory.Exists(JobPath + @"\General Docs") == false)
                        Directory.CreateDirectory(JobPath + @"\General Docs");

                    // checking for Quote folder
                    if (Directory.Exists(JobPath + @"\Quote") == false)
                        Directory.CreateDirectory(JobPath + @"\Quote");

                    // checking for Reference Material folder
                    if (Directory.Exists(JobPath + @"\Reference Material") == false)
                        Directory.CreateDirectory(JobPath + @"\Reference Material");
                }
                else if (FolderTemplateName == "Translation - standard" | FolderTemplateName == "Translation - complex" | FolderTemplateName == "Translation - with DTP")
                {

                    // checking for "01 Source files" folder
                    if (Directory.Exists(JobPath + @"\01 Source files") == false)
                        Directory.CreateDirectory(JobPath + @"\01 Source files");

                    // checking for "02 Translation" folder
                    if (Directory.Exists(JobPath + @"\02 Translation") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\02 Translation");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\02 Translation"), JobPath + @"\02 Translation", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\02 Translation" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Translation" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\02 Translation" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\02 Translation" + @"\02 From");
                    } // checking for "02 Translation" folder loop

                    // checking for "03 Proofreading" folder
                    if (Directory.Exists(JobPath + @"\03 Proofreading") == false)
                    {
                        if (Directory.Exists(JobPath + @"\04 Proofreading") == false)
                        {
                            Directory.CreateDirectory(JobPath + @"\03 Proofreading");
                            // copy all the sublfoders too from the template folder
                            GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\03 Proofreading"), JobPath + @"\03 Proofreading", true);
                        }
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\03 Proofreading" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\03 Proofreading" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\03 Proofreading" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\03 Proofreading" + @"\02 From");
                    } // checking for "03 Proofreading" folder loop

                    // checking for "04 Delivery" folder
                    if (Directory.Exists(JobPath + @"\04 Delivery") == false)
                    {
                        if (Directory.Exists(JobPath + @"\07 Delivery") == false & Directory.Exists(JobPath + @"\06 Delivery") == false & Directory.Exists(JobPath + @"\08 Delivery") == false & Directory.Exists(JobPath + @"\10 Delivery") == false)
                            Directory.CreateDirectory(JobPath + @"\04 Delivery");
                    }

                    // checking for Briefs folder
                    if (Directory.Exists(JobPath + @"\Briefs") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\Briefs");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\Briefs"), JobPath + @"\Briefs", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\02 From");
                    } // checking for Briefs folder loop

                    // checking for client invoice folder
                    if (Directory.Exists(JobPath + @"\Client Invoice") == false)
                        Directory.CreateDirectory(JobPath + @"\Client Invoice");

                    // checking for General Docs folder
                    if (Directory.Exists(JobPath + @"\General Docs") == false)
                        Directory.CreateDirectory(JobPath + @"\General Docs");

                    // creating 2 extra folders under General docs folder for two BMW orgs 
                    if (orderContact.OrgId == 139233 | orderContact.OrgId == 139232)
                    {
                        Directory.CreateDirectory(JobPath + @"\General Docs\Orders - email notifications");
                        Directory.CreateDirectory(JobPath + @"\General Docs\PO – XMLs");
                    }

                    // checking for Quote folder
                    if (Directory.Exists(JobPath + @"\Quote") == false)
                        Directory.CreateDirectory(JobPath + @"\Quote");

                    // checking for Reference Material folder
                    if (Directory.Exists(JobPath + @"\Reference Material") == false)
                        Directory.CreateDirectory(JobPath + @"\Reference Material");
                }
                else if (FolderTemplateName == "Translation - with client review")
                {

                    // checking for "01 Source files" folder
                    if (Directory.Exists(JobPath + @"\01 Source files") == false)
                        Directory.CreateDirectory(JobPath + @"\01 Source files");

                    // checking for "02 Translation" folder
                    if (Directory.Exists(JobPath + @"\02 Translation") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\02 Translation");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\02 Translation"), JobPath + @"\02 Translation", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\02 Translation" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Translation" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\02 Translation" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\02 Translation" + @"\02 From");
                    } // checking for "02 Translation" folder loop

                    // checking for "03 Proofreading" folder
                    if (Directory.Exists(JobPath + @"\03 Proofreading") == false)
                    {
                        if (Directory.Exists(JobPath + @"\04 Proofreading") == false)
                        {
                            Directory.CreateDirectory(JobPath + @"\03 Proofreading");
                            // copy all the sublfoders too from the template folder
                            GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\03 Proofreading"), JobPath + @"\03 Proofreading", true);
                        }
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\03 Proofreading" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\03 Proofreading" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\03 Proofreading" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\03 Proofreading" + @"\02 From");
                    } // checking for "03 Proofreading" folder loop

                    // checking for "04 Client review" folder
                    if (Directory.Exists(JobPath + @"\04 Client review") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\04 Client review");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\04 Client review"), JobPath + @"\04 Client review", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\04 Client review" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\04 Client review" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\04 Client review" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\04 Client review" + @"\02 From");
                    } // checking for "04 Client review" folder loop

                    // checking for "05 Delivery" folder
                    if (Directory.Exists(JobPath + @"\05 Delivery") == false)
                        Directory.CreateDirectory(JobPath + @"\05 Delivery");

                    // checking for Briefs folder
                    if (Directory.Exists(JobPath + @"\Briefs") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\Briefs");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\Briefs"), JobPath + @"\Briefs", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\02 From");
                    } // checking for Briefs folder loop

                    // checking for client invoice folder
                    if (Directory.Exists(JobPath + @"\Client Invoice") == false)
                        Directory.CreateDirectory(JobPath + @"\Client Invoice");

                    // checking for General Docs folder
                    if (Directory.Exists(JobPath + @"\General Docs") == false)
                        Directory.CreateDirectory(JobPath + @"\General Docs");

                    // checking for Quote folder
                    if (Directory.Exists(JobPath + @"\Quote") == false)
                        Directory.CreateDirectory(JobPath + @"\Quote");

                    // checking for Reference Material folder
                    if (Directory.Exists(JobPath + @"\Reference Material") == false)
                        Directory.CreateDirectory(JobPath + @"\Reference Material");
                }
                else if (FolderTemplateName == "Translation - with DTP")
                {

                    // checking for "01 Source files" folder
                    if (Directory.Exists(JobPath + @"\01 Source files") == false)
                        Directory.CreateDirectory(JobPath + @"\01 Source files");

                    // checking for "02 Translation" folder
                    if (Directory.Exists(JobPath + @"\02 Translation") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\02 Translation");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\02 Translation"), JobPath + @"\02 Translation", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\02 Translation" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Translation" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\02 Translation" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\02 Translation" + @"\02 From");
                    } // checking for "02 Translation" folder loop

                    // checking for "03 DTP" folder
                    if (Directory.Exists(JobPath + @"\03 DTP") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\03 DTP");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\03 DTP"), JobPath + @"\03 DTP", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\03 DTP" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\03 DTP" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\03 DTP" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\03 DTP" + @"\02 From");
                    } // checking for "03 DTP" folder loop

                    // checking for "04 Proofreading" folder
                    if (Directory.Exists(JobPath + @"\04 Proofreading") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\04 Proofreading");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\04 Proofreading"), JobPath + @"\04 Proofreading", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\04 Proofreading" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\04 Proofreading" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\04 Proofreading" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\04 Proofreading" + @"\02 From");
                    } // checking for "04 Proofreading" folder loop


                    // checking for "05 Delivery" folder
                    if (Directory.Exists(JobPath + @"\05 Delivery") == false)
                        Directory.CreateDirectory(JobPath + @"\05 Delivery");

                    // checking for Briefs folder
                    if (Directory.Exists(JobPath + @"\Briefs") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\Briefs");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\Briefs"), JobPath + @"\Briefs", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\02 From");
                    } // checking for Briefs folder loop

                    // checking for client invoice folder
                    if (Directory.Exists(JobPath + @"\Client Invoice") == false)
                        Directory.CreateDirectory(JobPath + @"\Client Invoice");

                    // checking for General Docs folder
                    if (Directory.Exists(JobPath + @"\General Docs") == false)
                        Directory.CreateDirectory(JobPath + @"\General Docs");

                    // checking for Quote folder
                    if (Directory.Exists(JobPath + @"\Quote") == false)
                        Directory.CreateDirectory(JobPath + @"\Quote");

                    // checking for Reference Material folder
                    if (Directory.Exists(JobPath + @"\Reference Material") == false)
                        Directory.CreateDirectory(JobPath + @"\Reference Material");
                }
                else if (FolderTemplateName == "Translation - with DTP + client review")
                {
                    // checking for "01 Source files" folder
                    if (Directory.Exists(JobPath + @"\01 Source files") == false)
                        Directory.CreateDirectory(JobPath + @"\01 Source files");

                    // checking for "02 Translation" folder
                    if (Directory.Exists(JobPath + @"\02 Translation") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\02 Translation");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\02 Translation"), JobPath + @"\02 Translation", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\02 Translation" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Translation" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\02 Translation" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\02 Translation" + @"\02 From");
                        if (Directory.Exists(JobPath + @"\03 DTP") == false)
                        {
                            Directory.CreateDirectory(JobPath + @"\03 DTP");
                            // copy all the sublfoders too from the template folder
                            GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\03 DTP"), JobPath + @"\03 DTP", true);
                        }
                        else
                        {
                            if (Directory.Exists(JobPath + @"\03 DTP" + @"\01 To") == false)
                                Directory.CreateDirectory(JobPath + @"\03 DTP" + @"\01 To");
                            if (Directory.Exists(JobPath + @"\03 DTP" + @"\02 From") == false)
                                Directory.CreateDirectory(JobPath + @"\03 DTP" + @"\02 From");
                        } // checking for "03 DTP" folder loop

                        // checking for "04 Proofreading" folder
                        if (Directory.Exists(JobPath + @"\04 Proofreading") == false)
                        {
                            Directory.CreateDirectory(JobPath + @"\04 Proofreading");
                            // copy all the sublfoders too from the template folder
                            GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\04 Proofreading"), JobPath + @"\04 Proofreading", true);
                        }
                        else
                        {
                            if (Directory.Exists(JobPath + @"\04 Proofreading" + @"\01 To") == false)
                                Directory.CreateDirectory(JobPath + @"\04 Proofreading" + @"\01 To");
                            if (Directory.Exists(JobPath + @"\04 Proofreading" + @"\02 From") == false)
                                Directory.CreateDirectory(JobPath + @"\04 Proofreading" + @"\02 From");
                        } // checking for "04 Proofreading" folder loop

                        // checking for "05 Client review" folder
                        if (Directory.Exists(JobPath + @"\05 Client review") == false)
                        {
                            Directory.CreateDirectory(JobPath + @"\05 Client review");
                            // copy all the sublfoders too from the template folder
                            GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\05 Client review"), JobPath + @"\05 Client review", true);
                        }
                        else
                        {
                            if (Directory.Exists(JobPath + @"\05 Client review" + @"\01 To") == false)
                                Directory.CreateDirectory(JobPath + @"\05 Client review" + @"\01 To");
                            if (Directory.Exists(JobPath + @"\05 Client review" + @"\02 From") == false)
                                Directory.CreateDirectory(JobPath + @"\05 Client review" + @"\02 From");
                        } // checking for "05 Client review" folder loop

                        // checking for "06 Delivery" folder
                        if (Directory.Exists(JobPath + @"\06 Delivery") == false)
                            Directory.CreateDirectory(JobPath + @"\06 Delivery");

                        // checking for Briefs folder
                        if (Directory.Exists(JobPath + @"\Briefs") == false)
                        {
                            Directory.CreateDirectory(JobPath + @"\Briefs");
                            // copy all the sublfoders too from the template folder
                            GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\Briefs"), JobPath + @"\Briefs", true);
                        }
                        else
                        {
                            if (Directory.Exists(JobPath + @"\Briefs" + @"\01 To") == false)
                                Directory.CreateDirectory(JobPath + @"\Briefs" + @"\01 To");
                            if (Directory.Exists(JobPath + @"\Briefs" + @"\02 From") == false)
                                Directory.CreateDirectory(JobPath + @"\Briefs" + @"\02 From");
                        } // checking for Briefs folder loop

                        // checking for client invoice folder
                        if (Directory.Exists(JobPath + @"\Client Invoice") == false)
                            Directory.CreateDirectory(JobPath + @"\Client Invoice");

                        // checking for General Docs folder
                        if (Directory.Exists(JobPath + @"\General Docs") == false)
                            Directory.CreateDirectory(JobPath + @"\General Docs");

                        // checking for Quote folder
                        if (Directory.Exists(JobPath + @"\Quote") == false)
                            Directory.CreateDirectory(JobPath + @"\Quote");

                        // checking for Reference Material folder
                        if (Directory.Exists(JobPath + @"\Reference Material") == false)
                            Directory.CreateDirectory(JobPath + @"\Reference Material");
                    }
                }
                else if (FolderTemplateName == "Transcreation - standard" | FolderTemplateName == "Transcreation - with DTP" | FolderTemplateName == "Transcreation - complex")
                {

                    // checking for "01 Source files" folder
                    if (Directory.Exists(JobPath + @"\01 Source files") == false)
                        Directory.CreateDirectory(JobPath + @"\01 Source files");

                    // checking for "02 Translation" folder
                    if (Directory.Exists(JobPath + @"\02 Translation") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\02 Translation");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\02 Translation"), JobPath + @"\02 Translation", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\02 Translation" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Translation" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\02 Translation" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\02 Translation" + @"\02 From");
                        if (Directory.Exists(JobPath + @"\03 Transcration") == false)
                        {
                            Directory.CreateDirectory(JobPath + @"\03 Translation");
                            if (Directory.Exists(JobPath + @"\03 Transcreation" + @"\01 To") == false)
                                Directory.CreateDirectory(JobPath + @"\Transcreation" + @"\01 To");
                            if (Directory.Exists(JobPath + @"\03 Transcreation" + @"\02 From") == false)
                                Directory.CreateDirectory(JobPath + @"\03 Transcreation" + @"\02 From");
                        }
                    } // checking for "02 Translation" folder loop
                    if (Directory.Exists(JobPath + @"\04 Delivery") == false)
                    {
                        if (Directory.Exists(JobPath + @"\07 Delivery") == false & Directory.Exists(JobPath + @"\06 Delivery") == false & Directory.Exists(JobPath + @"\08 Delivery") == false & Directory.Exists(JobPath + @"\10 Delivery") == false)
                            Directory.CreateDirectory(JobPath + @"\04 Delivery");
                    }

                    if (Directory.Exists(JobPath + @"\03 Proofreading") == false)
                    {
                        if (Directory.Exists(JobPath + @"\04 Proofreading") == false)
                        {
                            Directory.CreateDirectory(JobPath + @"\03 Proofreading");
                            // copy all the sublfoders too from the template folder
                            GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\03 Proofreading"), JobPath + @"\03 Proofreading", true);
                        }
                    }
                    // checking for "03 DTP" folder
                    if (Directory.Exists(JobPath + @"\04 DTP") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\04 DTP");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\04 DTP"), JobPath + @"\04 DTP", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\04 DTP" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\03 DTP" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\04 DTP" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\04 DTP" + @"\02 From");
                    } // checking for "03 DTP" folder loop

                    // checking for "04 Proofreading" folder
                    if (Directory.Exists(JobPath + @"\05 Proofreading") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\05 Proofreading");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\04 Proofreading"), JobPath + @"\05 Proofreading", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\05 Proofreading" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\05 Proofreading" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\05 Proofreading" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\05 Proofreading" + @"\02 From");
                    } // checking for "04 Proofreading" folder loop

                    // checking for "05 Client review" folder
                    if (Directory.Exists(JobPath + @"\06 Client review") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\06 Client review");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\05 Client review"), JobPath + @"\06 Client review", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\06 Client review" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\06 Client review" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\06 Client review" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\06 Client review" + @"\02 From");
                    } // checking for "05 Client review" folder loop

                    // checking for "06 Delivery" folder
                    if (Directory.Exists(JobPath + @"\07 Delivery") == false)
                        Directory.CreateDirectory(JobPath + @"\07 Delivery");

                    // checking for Briefs folder
                    if (Directory.Exists(JobPath + @"\Briefs") == false)
                    {
                        Directory.CreateDirectory(JobPath + @"\Briefs");
                        // copy all the sublfoders too from the template folder
                        GeneralUtils.CopyDirectory(Path.Combine(globalVariables.JobFolderTemplatesPath, FolderTemplateName + @"\Briefs"), JobPath + @"\Briefs", true);
                    }
                    else
                    {
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\01 To") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\01 To");
                        if (Directory.Exists(JobPath + @"\Briefs" + @"\02 From") == false)
                            Directory.CreateDirectory(JobPath + @"\Briefs" + @"\02 From");
                    } // checking for Briefs folder loop

                    // checking for client invoice folder
                    if (Directory.Exists(JobPath + @"\Client Invoice") == false)
                        Directory.CreateDirectory(JobPath + @"\Client Invoice");

                    // checking for General Docs folder
                    if (Directory.Exists(JobPath + @"\General Docs") == false)
                        Directory.CreateDirectory(JobPath + @"\General Docs");

                    // checking for Quote folder
                    if (Directory.Exists(JobPath + @"\Quote") == false)
                        Directory.CreateDirectory(JobPath + @"\Quote");

                    // checking for Reference Material folder
                    if (Directory.Exists(JobPath + @"\Reference Material") == false)
                        Directory.CreateDirectory(JobPath + @"\Reference Material");
                } // end of "template selected is : Translation - with DTP + client review
            } // end of "job folder exists" loop

            // Checking external job folders now
            if (FolderTemplateName != "IFADWorkflow" && FolderTemplateName != "Interpreting - standard")
            {
                string ExternalJobPath = "";

                // firstly check if the job order folder exists for job order 
                if (ExtranetAndWSDirectoryPathForApp(order.Id) != "")
                    ExternalJobPath = ExtranetAndWSDirectoryPathForApp(order.Id);
                else
                {
                    // checking  if the contact exists in the job folders directory
                    string ExternalContactPattern = orderContact.Id.ToString() + "*";
                    string ExternalContactDirPath = globalVariables.ExtranetNetworkBaseDirectoryPath + @"\Contacts";
                    DirectoryInfo ExternalDirInfo = new DirectoryInfo(ExternalContactDirPath);
                    DirectoryInfo[] MatchingContactsDirs = ExternalDirInfo.GetDirectories(ExternalContactPattern, SearchOption.TopDirectoryOnly);
                    string ExternalContactFolderPath = "";
                    if (MatchingContactsDirs.Count() == 0)
                    {
                        ExternalContactFolderPath = Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath + @"\Contacts", GeneralUtils.MakeStringSafeForFileSystemPath(orderContact.Id.ToString()));
                        Directory.CreateDirectory(ExternalContactFolderPath);
                    }
                    else
                        ExternalContactFolderPath = MatchingContactsDirs[0].FullName;

                    // checking for the job folder
                    string ExternalJobPattern = order.Id + "*";
                    DirectoryInfo ExternalJobDirInfo = new DirectoryInfo(ExternalContactFolderPath);
                    DirectoryInfo[] MatchingJobInfo = ExternalJobDirInfo.GetDirectories(ExternalJobPattern, SearchOption.TopDirectoryOnly);

                    if (MatchingJobInfo.Count() == 0)
                    {
                        ExternalJobPath = Path.Combine(ExternalContactFolderPath, order.Id + " - " + order.SubmittedDateTime.ToString("d MMM yy"));
                        Directory.CreateDirectory(ExternalJobPath);
                    }
                    else
                        ExternalJobPath = MatchingJobInfo[0].FullName;
                }


                if (Directory.Exists(Path.Combine(ExternalJobPath, "Source")) == false)
                    Directory.CreateDirectory(Path.Combine(ExternalJobPath, "Source"));


                if (Directory.Exists(Path.Combine(ExternalJobPath, "Collection")) == false)
                    Directory.CreateDirectory(Path.Combine(ExternalJobPath, "Collection"));

                // Looping through each job item to check the folders for all job items are in place
                //foreach (JobItemsDataTableViewModel JobItem in GetJobItems(jobOrderId).Result)
                //{
                //    jobItemService.ConfigureNetworkFolders(JobItem.Id);
                //}

            }
            return true;
        }


        //public async Task<string> ExtranetAndWSDirectoryPathForApp(int orderId)
        //{

        //    var order = await orderRepository.All().Where(o => o.Id == orderId).FirstOrDefaultAsync();
        //    var orderContact = await contactRepo.All().Where(o => o.Id == order.ContactId && o.DeletedDate == null).FirstOrDefaultAsync();
        //    //var org = await orgRepo.All().Where(o => o.Id == orderContact.OrgId).FirstOrDefaultAsync();

        //    // Unlike our internal folder structures, the Extranet and Web Service
        //    // will be based (for now) on individual contacts, rather than orgs.

        //    // Find the first matching job order directory within the contact folder 
        //    // which starts with the order ID, regardless of what comes after it
        //    string ContactDirSearchPattern = orderContact.Id + "*";
        //    string OrderDirSearchPattern = orderId.ToString() + "*";

        //    string ExtranetBaseDir = Path.Combine(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Contacts"), orderContact.Id.ToString());

        //    string ContactDirPath = ExtranetBaseDir;

        //    DirectoryInfo DirInfo = new DirectoryInfo(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Contacts"));
        //    // find contact folder first (job folders should then appear within that)
        //    string PathToReturn = "";

        //    if (Directory.Exists(ExtranetBaseDir) == false)
        //        PathToReturn = "";
        //    else
        //    {
        //        DirInfo = new DirectoryInfo(ContactDirPath);
        //        DirectoryInfo[] MatchingEnquiryDirs = DirInfo.GetDirectories(OrderDirSearchPattern, SearchOption.TopDirectoryOnly);
        //        if (MatchingEnquiryDirs.Count() == 0)
        //            PathToReturn = "";
        //        else
        //            PathToReturn = MatchingEnquiryDirs[0].FullName;
        //    }

        //    if (PathToReturn == "" && order.SubmittedDateTime > new DateTime(2018, 1, 1))
        //    {
        //        ContactDirPath = Path.Combine(new DirectoryInfo(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Contacts")).FullName, orderContact.Id.ToString());
        //        Directory.CreateDirectory(Path.Combine(ContactDirPath, order.Id.ToString()));
        //        PathToReturn = Path.Combine(ContactDirPath, order.Id.ToString());
        //    }

        //    return PathToReturn;
        //}

        public int CopyReferenceFiles(List<FileModel> refFiles, string ExternalRefPath, bool isDesignPlusJob = false, bool OkToDeleteTempRefFile = true)
        {
            int GoodRefFilesUploaded = 0;
            if (ExternalRefPath != "" && Directory.Exists(ExternalRefPath) == false)
            {
                Directory.CreateDirectory(ExternalRefPath);
            }

            var ReferenceFileZip = new ZipFile(ExternalRefPath + @"\Reference files.zip");
            List<String> AllRefFiles = new List<String>();

            foreach (FileModel refFile in refFiles)
            {
                if (refFile.file.Length > 0)
                {
                    if (ExternalRefPath != "")
                    {
                        try
                        {
                            string DirectoryToSavePath = Path.Combine(ExternalRefPath, "File" + (GoodRefFilesUploaded + 1).ToString());
                            Directory.CreateDirectory(DirectoryToSavePath);

                            string refFilePath = Path.Combine(DirectoryToSavePath, refFile.file.FileName);
                            using (Stream fileStream = new FileStream(refFilePath, FileMode.Create))
                            {
                                refFile.file.CopyToAsync(fileStream);
                            }

                            if (isDesignPlusJob == true)
                            {
                                AllRefFiles.Add(DirectoryToSavePath + @"\" + refFile.file.FileName);
                            }
                            GoodRefFilesUploaded += 1;

                        }
                        catch (Exception ex)
                        {
                            string DirectoryToSavePath = Path.Combine(ExternalRefPath, "File" + (GoodRefFilesUploaded + 1).ToString());
                            if (Directory.Exists(DirectoryToSavePath) == false)
                            {
                                Directory.CreateDirectory(DirectoryToSavePath);
                            }

                            string FilePathWithoutFileName = DirectoryToSavePath + @"\";

                            string FileNameWithoutExtension = refFile.file.FileName.Substring(0, refFile.file.FileName.LastIndexOf("."));
                            string FileExtension = refFile.file.FileName.Substring(refFile.file.FileName.LastIndexOf("."));
                            int IdealLengthOfFileName = 259 - FilePathWithoutFileName.Length - FileExtension.Length;
                            string ShorterFileName = FilePathWithoutFileName + FileNameWithoutExtension.Substring(0, IdealLengthOfFileName) + FileExtension;

                            using (Stream fileStream = new FileStream(Path.Combine(DirectoryToSavePath, ShorterFileName), FileMode.Create))
                            {
                                refFile.file.CopyToAsync(fileStream);
                            }

                            if (isDesignPlusJob == true)
                            {
                                AllRefFiles.Add(Path.Combine(DirectoryToSavePath, ShorterFileName));
                            }

                            GoodRefFilesUploaded += 1;

                        }
                    }
                }

            }

            if (AllRefFiles.Count > 0)
            {
                ReferenceFileZip.AddFiles(fileNames: AllRefFiles, preserveDirHierarchy: false, directoryPathInArchive: "");
                ReferenceFileZip.Save();
            }


            return GoodRefFilesUploaded;

        }

        public async Task<bool> OrderExists(int orderId)
        {
            int? result = await orderRepository.All().Where(o => o.Id == orderId && o.DeletedDate == null).Select(o => o.Id).FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void AnnounceThisOrderCreation(int orderId, string SendingEmailAddress, string SubjectLine, bool ExternalNotification, bool AcknowledgeAsQuote,
                                             string UILangIANACode = "", string CustomerSpecificMessage = "", bool IsFileInDTPFormat = true,
                                             bool QuoteApprovalNotification = false, bool IsPrintingProject = false,
                                             bool SendTranslateOnlineJobAutomatically = false, ExtranetUsersTemp extranetUser = null)
        {
            string MessageBodyCore = "";
            string RecipientEmailAddress = "";

            JobOrder order = orderRepository.All().Where(o => o.Id == orderId).FirstOrDefault();
            Contact orderContact = contactRepo.All().Where(c => c.Id == order.ContactId).FirstOrDefault();
            Org org = orgService.GetOrgDetails(orderContact.OrgId).Result;
            OrgGroup group = null;
            if (org.OrgGroupId != null)
            {
                group = orgGroupService.GetOrgGroupDetails(org.OrgGroupId.Value).Result;
            }
            LocalCurrencyInfo clientCurrency = currencyService.GetCurrencyInfo(order.ClientCurrencyId, "en").Result;

            if (ExternalNotification == true)
            {
                if (order.JobOrderChannelId == 7 || order.JobOrderChannelId == 18)
                {
                    RecipientEmailAddress = extranetUser.WebServiceNotificationEmailAddress;
                }
                else
                {
                    RecipientEmailAddress = orderContact.EmailAddress;
                }


                Brand currentBrand = brandService.GetBrandForClient(org.OrgGroupId.Value).Result;

                if (AcknowledgeAsQuote == true)
                {
                    var messageBody = miscResourceService.GetMiscResourceByName("QuoteRequestReceivedEmailBody", "en").Result;
                    MessageBodyCore = "<p>" + String.Format(messageBody.StringContent.Replace("iplus.{tpbrand}.com", currentBrand.DomainName).Replace("i&nbsp;plus", currentBrand.ApplicationName),
                                                            orderContact.Name, order.Id.ToString()) + "</p><br />";
                }
                else if (QuoteApprovalNotification == true)
                {
                    var enquiry = enquiryService.GetEnquiryById(order.OriginatedFromEnquiryId.Value).Result;

                    if (enquiry != null)
                    {
                        var decisionByContact = contactService.GetById(enquiry.DecisionMadeByContactId.Value).Result;
                        RecipientEmailAddress += "," + decisionByContact.EmailAddress;
                        var messageBody = miscResourceService.GetMiscResourceByName("QuoteApprovedEmailBody", "en").Result;

                        var currentQuote = quoteServices.GetCurrentQuote(enquiry.Id).Result;
                        MessageBodyCore = "<p>" + String.Format(messageBody.StringContent.Replace("{tp brand}", currentBrand.CompanyNameToShow).Replace("iplus.{tpbrand}.com", currentBrand.DomainName).Replace("i&nbsp;plus", currentBrand.ApplicationName),
                                                           orderContact.Name, currentQuote.ToString(), order.Id.ToString()) + "</p><br />";
                    }
                }
                else
                {
                    var messageBody = miscResourceService.GetMiscResourceByName("TransRequestReceivedEmailBody1", "en").Result;
                    MessageBodyCore = "<p>" + String.Format(messageBody.StringContent.Replace("{tp brand}", currentBrand.CompanyNameToShow).Replace("iplus.{tpbrand}.com", currentBrand.DomainName).Replace("i&nbsp;plus", currentBrand.ApplicationName),
                                                            orderContact.Name, order.Id.ToString()) + "</p><br />";
                }

                var messageBody1 = miscResourceService.GetMiscResourceByName("EmailCoreOrderDetails1", "en").Result;
                var currencyInfo = currencyService.GetCurrencyInfo(order.ClientCurrencyId, "en").Result;
                MessageBodyCore += "<p>" + String.Format(messageBody1.StringContent,
                                                        System.Web.HttpUtility.HtmlEncode(order.JobName),
                                                        order.OverallDeliveryDeadline.ToString("d MMMM yyyy HH:mm"),
                                                        System.Web.HttpUtility.HtmlEncode(order.ClientPonumber),
                                                        currencyInfo.CurrencyName,
                                                        System.Web.HttpUtility.HtmlEncode(order.ClientNotes),
                                                        GetTotalSourceLangsCountString(orderId, "en"));

                List<JobItem> allJobItems = new List<JobItem>();
                try
                {
                    allJobItems = GetAllJobItems(orderId).Result; ;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(10);
                    allJobItems = GetAllJobItems(orderId).Result; ;
                }
                foreach (JobItem item in allJobItems)
                {
                    if (item.IsVisibleToClient == true && (org.OrgGroupId == 18573 || item.LanguageServiceId != 21))
                    {
                        var targetLang = langService.GetLanguageInfo(item.TargetLanguageIanacode, "en");
                        string rscString = miscResourceService.GetMiscResourceByName("EmailCoreItemDetails", "en").Result.StringContent;
                        if (item.LanguageServiceId == 21)
                        {
                            MessageBodyCore += "Review - " + String.Format(rscString, targetLang.Name, item.Id.ToString()) + " <br />";
                        }
                        else if (item.LanguageServiceId == 7)
                        {
                            MessageBodyCore += String.Format(rscString, targetLang.Name, item.Id.ToString()) + " (proofreading)<br />";
                        }
                        else
                        {
                            MessageBodyCore += String.Format(rscString, targetLang.Name, item.Id.ToString()) + " <br />";
                        }
                    }
                }

                MessageBodyCore += "</p></td></tr></table> <br />";

                //Check to see whether to include others in the e-mail
                if (orderContact.IncludeInNotificationsOn == true && orderContact.IncludeInNotifications != "")
                {
                    RecipientEmailAddress = RecipientEmailAddress + "," + orderContact.IncludeInNotifications;
                }
                else
                {
                    if (org.IncludeInNotificationsOn == true && org.IncludeInNotifications != "")
                    {
                        RecipientEmailAddress = RecipientEmailAddress + "," + org.IncludeInNotifications;
                    }
                }



                if ((orderContact.Id == 82688 || orderContact.Id == 82542 || orderContact.Id == 82505 || orderContact.Id == 82578) &&
                    globalVariables.CurrentAppModeString == "QAandCustomerWSTesting" ||
                  (globalVariables.CurrentAppModeString == "PRODUCTION" && (orderContact.Id == 248322 || orderContact.Id == 249103)))
                {
                    if (order.CustomerSpecificField1Value != "")
                    {
                        RecipientEmailAddress += ", " + order.CustomerSpecificField1Value;
                    }
                }

            }
            else
            {
                List<Employee> allSalesOwners = new List<Employee>();

                var salesOnwer = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesAccountManagerLead).Result;

                if (salesOnwer != null)
                {
                    allSalesOwners.Add(empService.IdentifyCurrentUserById(salesOnwer.EmployeeId).Result);
                }
                salesOnwer = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesNewBusinessLead).Result;

                if (salesOnwer != null)
                {
                    allSalesOwners.Add(empService.IdentifyCurrentUserById(salesOnwer.EmployeeId).Result);
                }

                if (allSalesOwners.Count == 0)
                {
                    RecipientEmailAddress = "";
                }
                else
                {
                    foreach (Employee emp in allSalesOwners)
                    {
                        if (emp != null)
                        {
                            if (RecipientEmailAddress == "")
                            {
                                RecipientEmailAddress = emp.EmailAddress + ", ";
                            }
                            else
                            {
                                RecipientEmailAddress += emp.EmailAddress + ", ";
                            }

                        }
                    }
                }

                var opsOwner = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.OperationsLead).Result;
                var clientIntroOwner = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.ClientIntroLead).Result;

                if (opsOwner == null)
                {
                    if (clientIntroOwner != null)
                    {
                        var clientIntoEmpObj = empService.IdentifyCurrentUserById(clientIntroOwner.EmployeeId).Result;
                        RecipientEmailAddress += clientIntoEmpObj.EmailAddress;
                    }
                    else
                    {
                        RecipientEmailAddress += globalVariables.InternalNotificationRecipientAddress;
                    }
                }
                else
                {
                    var opsOnwerEmpObj = empService.IdentifyCurrentUserById(opsOwner.EmployeeId).Result;
                    Employee opsOwnersManager = null;

                    if (opsOnwerEmpObj.Manager != null)
                    {
                        opsOwnersManager = empService.GetManagerOfEmployee(opsOnwerEmpObj.Manager.Value).Result;
                    }

                    if (opsOwnersManager == null)
                    {
                        RecipientEmailAddress += opsOnwerEmpObj.EmailAddress;
                    }
                    else
                    {
                        RecipientEmailAddress += opsOnwerEmpObj.EmailAddress + ", " + opsOwnersManager.EmailAddress;
                    }

                    if (clientIntroOwner != null)
                    {
                        var clientIntoEmpObj = empService.IdentifyCurrentUserById(clientIntroOwner.EmployeeId).Result;
                        RecipientEmailAddress += clientIntoEmpObj.EmailAddress;
                    }

                }
                string orgGroupString = "";
                if (group != null)
                {
                    orgGroupString = String.Format("<b>Org group:</b></p></td><td><p><a href=\"https://myplusbeta.publicisgroupe.net/OrgGroup?groupid={0}\">", group.Id.ToString()) + group.Name + "</a></p></td></tr><tr><td><p>";
                }

                try
                {
                    if (order.JobOrderChannelId != 7 && order.JobOrderChannelId != 8 && order.JobOrderChannelId != 12 &&
                        order.JobOrderChannelId != 18 && order.JobOrderChannelId != 21 && order.JobOrderChannelId != 22)
                    {
                        MessageBodyCore = "<p><b>Order number:</b> <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + order.Id.ToString() + "\">" + order.Id.ToString() +
                                          "</a></p><br />" + "<p>The source files are in the newly created job folder:<br />" +
                                          "<a href=\"file://" + System.Web.HttpUtility.HtmlEncode(NetworkDirectoryPathForApp(order.Id, ForUser: true)) + "\">" +
                                          System.Web.HttpUtility.HtmlEncode(NetworkDirectoryPathForApp(order.Id, ForUser: true)) + "</a></p><br />" +
                                          "<table><tr><td><p>" +
                                          orgGroupString +
                                          String.Format("<b>Org:</b></p></td><td><p><a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id={0}\">", org.Id.ToString()) + org.OrgName + "</a></p></td></tr><tr><td><p>" +
                                          String.Format("<b>Contact:</b></p></td><td><p><a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid={0}\">", orderContact.Id.ToString()) + orderContact.Name + "</a></p></td></tr><tr><td><p>" +
                                          "<b>Reference:</b></p></td><td><p>" + System.Web.HttpUtility.HtmlEncode(order.JobName) + " </p></td></tr><tr><td><p>" +
                                          "<b>Deadline (GMT): </b></p></td><td><p>" + order.OverallDeliveryDeadline.ToString("d MMMM yyyy HH:mm") + " </p></td></tr><tr><td><p>" +
                                          "<b>Purchase order number:</b></p></td><td><p>" + System.Web.HttpUtility.HtmlEncode(order.ClientPonumber) + "</p></td></tr><tr><td><p>" +
                                          "<b>Currency:</b></p></td><td><p>" + clientCurrency.CurrencyName + "</p></td></tr><tr><td valign='top'><p>" +
                                          "<b>Notes:</b></p></td><td><p>" + System.Web.HttpUtility.HtmlEncode(order.ClientNotes) + "<br/>" +
                                          CustomerSpecificMessage + "</p></td></tr><tr><td><p>" +
                                          "<b>Source language:</b></p></td><td><p>" + GetTotalSourceLangsCountString(orderId, "en") + " </p></td></tr><tr><td valign='top'><p>" +
                                          "<b>Target language(s):</b></p></td><td><p>";
                    }
                    else
                    {
                        MessageBodyCore = "<p><b>Order number:</b> <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + order.Id.ToString() + "\">" + order.Id.ToString() + "</a></p><br />" +
                                            "<p><table><tr><td><p>" +
                                            orgGroupString +
                                            String.Format("<b>Org:</b></p></td><td><p><a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id={0}\">", org.Id.ToString()) + org.OrgName + "</a></p></td></tr><tr><td><p>" +
                                            String.Format("<b>Contact:</b></p></td><td><p><a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid={0}\">", orderContact.Id.ToString()) + orderContact.Name + "</a></p></td></tr><tr><td><p>" +
                                            "<b>Reference:</b></p></td><td><p>" + System.Web.HttpUtility.HtmlEncode(order.JobName) + " <br />" +
                                            "<b>Deadline (GMT): </b></p></td><td><p>" + order.OverallDeliveryDeadline.ToString("d MMMM yyyy HH:mm") + "</p></td></tr><tr><td><p>" +
                                            "<b>Purchase order number:</b></p></td><td><p>" + System.Web.HttpUtility.HtmlEncode(order.ClientPonumber) + "</p></td></tr><tr><td><p>" +
                                            "<b>Currency:</b></p></td><td><p>" + clientCurrency.CurrencyName + "</p></td></tr><tr><td><p>" +
                                            "<b>Notes:</b></p></td><td><p>" + System.Web.HttpUtility.HtmlEncode(order.ClientNotes) + "<br/>" +
                                            CustomerSpecificMessage + "</p></td></tr><tr><td><p>" +
                                            "<b>Source language:</b></p></td><td><p>" + GetTotalSourceLangsCountString(orderId, "en") + "</p></td></tr><tr><td><p>" +
                                            "<b>Target language(s):</b></p></td><td><p>";
                    }


                }

                catch (Exception ex)
                {
                    MessageBodyCore = "<p><b>Order number:</b> <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + order.Id.ToString() + "\">" + order.Id.ToString() + "</a></p><br />" +
                                     "<table><tr><td><p>" +
                                     orgGroupString +
                                     String.Format("<b>Org:</b></p></td><td><p><a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id={0}\">", org.Id.ToString()) + org.OrgName + "</a></p></td></tr><tr><td><p>" +
                                     String.Format("<b>Contact:</b></p></td><td><p><a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid={0}\">", orderContact.Id.ToString()) + orderContact.Name + "</a></p></td></tr><tr><td><p>" +
                                     "<b>Reference:</b></p></td><td><p>" + System.Web.HttpUtility.HtmlEncode(order.JobName) + " <br />" +
                                     "<b>Deadline (GMT): </b></p></td><td><p>" + order.OverallDeliveryDeadline.ToString("d MMMM yyyy HH:mm") + "</p></td></tr><tr><td><p>" +
                                     "<b>Purchase order number:</b></p></td><td><p>" + System.Web.HttpUtility.HtmlEncode(order.ClientPonumber) + "</p></td></tr><tr><td><p>" +
                                     "<b>Currency:</b></p></td><td><p>" + clientCurrency.CurrencyName + "</p></td></tr><tr><td><p>" +
                                     "<b>Notes:</b></p></td><td><p>" + System.Web.HttpUtility.HtmlEncode(order.ClientNotes) + "<br/>" +
                                     CustomerSpecificMessage + "</p></td></tr><tr><td><p>" +
                                     "<b>Source language:</b></p></td><td><p>" + GetTotalSourceLangsCountString(orderId, "en") + "</p></td></tr><tr><td><p>" +
                                     "<b>Target language(s):</b></p></td><td><p>";
                }


                List<JobItem> allJobItems = new List<JobItem>();
                try
                {
                    allJobItems = GetAllJobItems(orderId).Result; ;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(10);
                    allJobItems = GetAllJobItems(orderId).Result; ;
                }

                foreach (JobItem item in allJobItems)
                {
                    //for internal use, it doesn't matter if the items are "client-visible"
                    //or not, but as of April 2011, do notify about client review job items
                    if (item.LanguageServiceId == 21)
                    {
                        MessageBodyCore += "Client review - " + langService.GetLanguageInfo(item.TargetLanguageIanacode, "en").Name + " - Job item number: <a href=\"https://myplusbeta.publicisgroupe.net/JobItem?Id=" + item.Id.ToString() + "\">" + item.Id.ToString() + "</a><br/>";
                    }
                    else if (item.LanguageServiceId == 7)
                    {
                        MessageBodyCore += langService.GetLanguageInfo(item.TargetLanguageIanacode, "en").Name + " - Proofreading job item number: <a href=\"https://myplusbeta.publicisgroupe.net/JobItem?Id=" + item.Id.ToString() + "\">" + item.Id.ToString() + "</a><br/>";
                    }
                    else
                    {
                        MessageBodyCore += langService.GetLanguageInfo(item.TargetLanguageIanacode, "en").Name + " - Job item number: <a href=\"https://myplusbeta.publicisgroupe.net/JobItem?Id=" + item.Id.ToString() + "\">" + item.Id.ToString() + "</a><br/>";
                    }
                }

                MessageBodyCore += "</p></td></tr></table><br/>";

                if (IsPrintingProject == true)
                {
                    MessageBodyCore += "<p> This is a printing/packaging project, and the relevant workflow stated in the Operations Guidebook must be followed at all times. Please speak to your line manager.</p>";
                }

                if (SendTranslateOnlineJobAutomatically == true)
                {
                    MessageBodyCore += "<br/><p> <font color=\"green\">This job order is configured to send translate online jobs automtically to translators.</font></p><br/><br/>";
                }

                if (order.IsMachineTranslationJobFromiPlus == true)
                {
                    MessageBodyCore += "<br/><p> <font color=\"green\">This job order is machine translation job which will be automatically translated and sent to client.</font></p><br/><br/>";
                }

                if (IsFileInDTPFormat == false)
                {
                    MessageBodyCore += "<p><b><font color=\"red\"> The client indicated that they wanted translate plus to carry out full DTP. However, the file type(s) submitted do not appear to be recognised DTP formats, so no DTP items have been created in the intranet, as this may have been a default selection on the part of the client. Please check carefully to see if we should in fact add DTP items for this request. </font></b></p> ";
                }

            }

            emailUtils.SendMail(SendingEmailAddress,
                                    RecipientEmailAddress + ", KavitaJ@translateplus.com",
                                    SubjectLine,
                                    MessageBodyCore, true,
                                    IsExternalNotification: ExternalNotification);

        }


        public async Task<ReviewSignOffModel> GetSignOffData(int jobItemId)
        {
            var jobItem = await jobitemsRepo.All().Where(x => x.Id == jobItemId && x.DeletedDateTime == null).FirstOrDefaultAsync();
            ReviewSignOffModel model = new ReviewSignOffModel();
            model.JobItemId = jobItemId;
            model.TotalNumberOfChangedReviewSegments = jobItem.TotalNumberOfChangedReviewSegments ?? 0;
            model.TotalNumberOfReviewSegments = jobItem.TotalNumberOfReviewSegments ?? 0;
            model.PercentageOfChangedReviewSegments = jobItem.PercentageOfChangedReviewSegments ?? 0;
            model.TranslationsChanged = String.Format(@"Number of translations changed: {0} out of a total of {1} ({2}%)", model.TotalNumberOfChangedReviewSegments, model.TotalNumberOfReviewSegments, model.PercentageOfChangedReviewSegments);
            return model;
        }
        public async Task<int> ApproveReview(string extranetUserName, int jobItemId, string comments)
        {
            var extranetUser = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
            var extranetUserContact = await contactRepo.All().Where(x => x.Id == extranetUser.DataObjectId && x.DeletedDate == null).FirstOrDefaultAsync();
            var extranetUserOrgGroupId = await orgRepo.All().Where(x => x.Id == extranetUserContact.OrgId && x.DeletedDate == null).Select(x => x.OrgGroupId).FirstOrDefaultAsync();
            int rowsAffected = 0;
            try
            {
                var jobItem = await jobitemsRepo.All().Where(x => x.Id == jobItemId && x.DeletedDateTime == null).FirstOrDefaultAsync();
                var jobItemData = (from item in jobitemsRepo.All().Where(x => x.Id == jobItemId && x.DeletedDateTime == null)
                                   join order in orderRepository.All().Where(x => x.DeletedDate == null) on item.JobOrderId equals order.Id
                                   join contact in contactRepo.All().Where(x => x.DeletedDate == null) on order.ContactId equals contact.Id
                                   join org in orgRepo.All().Where(x => x.DeletedDate == null) on contact.OrgId equals org.Id
                                   select new
                                   {
                                       jobItemId = item.Id,
                                       jobOrderName = order.JobName,
                                       jobOrderId = item.JobOrderId,
                                       contactId = contact.Id,
                                       contactName = contact.Name,
                                       contactOrgId = contact.OrgId,
                                       contactOrgName = org.OrgName,
                                       projectManager = order.ProjectManagerEmployeeId

                                   }).FirstOrDefault();
                int projectManagerId = jobItemData.projectManager;
                if (jobItem.LanguageServiceId == 1 && (jobItem.SupplierIsClientReviewer == false && jobItemData.contactOrgId != 79702))
                {
                    // update linguistic supplier status (CompleteAndSignedOffBySupplier)
                    DateTime currentUKTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                    jobItem.SupplierCompletedItemDateTime = currentUKTime;
                    if (comments != null)
                    {
                        jobItem.ExtranetSignoffComment = comments;
                    }
                    if (jobItem.SupplierAcceptedWorkDateTime == null)
                    {
                        jobItem.SupplierAcceptedWorkDateTime = currentUKTime;
                    }
                    rowsAffected = await jobitemsRepo.SaveChangesAsync();
                }
                else
                {
                    // update client review status (CompleteAndSignedOffByReviewer)
                    // update linguistic supplier status (CompleteAndSignedOffBySupplier)
                    DateTime currentUKTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                    jobItem.SupplierCompletedItemDateTime = currentUKTime;
                    if (comments != null)
                    {
                        jobItem.ExtranetSignoffComment = comments;
                    }
                    if (jobItem.SupplierAcceptedWorkDateTime == null)
                    {
                        jobItem.SupplierAcceptedWorkDateTime = currentUKTime;
                    }
                    rowsAffected = await jobitemsRepo.SaveChangesAsync();
                }

                // sending internal notification
                string NotificationRecipients = "";
                var projectManager = await employeesRepo.All().Where(x => x.Id == projectManagerId && x.TerminateDate == null).FirstOrDefaultAsync();
                if (projectManager == null)
                {
                    var empOwnerships = new Enumerations.EmployeeOwnerships[]
                    {
                    Enumerations.EmployeeOwnerships.OperationsLead
                    };
                    DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                    var opsOwner = await ownershipsLogicService.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(jobItemData.contactId, Enumerations.DataObjectTypes.Org, empOwnerships, currentTime);
                    if (opsOwner == null)
                    {
                        NotificationRecipients = "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com";
                    }
                    else
                    {
                        var opsOwnerEmployeeId = opsOwner.Select(x => x.EmployeeId);
                        var opsEmployeeData = employeesRepo.All().Where(x => opsOwnerEmployeeId.Contains(x.Id)).Select(x => new
                        {
                            x.EmailAddress,
                            x.FirstName
                        }).FirstOrDefault();
                        NotificationRecipients = opsEmployeeData.EmailAddress;
                    }
                }
                else
                {
                    NotificationRecipients = projectManager.EmailAddress;
                }
                if (extranetUserContact != null && extranetUserOrgGroupId == 73512)
                {
                    NotificationRecipients += ", Swarovski@translateplus.com";
                }
                string JobInfo = string.Format(@"<table><tr><td><p>Organisation:  </td><td><p><a href=""https://myplusbeta.publicisgroupe.net/Organisation?id={0}""> {0} - {1}</a></td></tr><tr><td><p>Contact: </td><td><p><a href=""https://myplusbeta.publicisgroupe.net/Contact?contactid={2}"">{2} - {3}</a></td></tr><tr><td><p>Job order: </td><td><p><a href=""https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid={4}"">{4} - {5}</a></td></tr></table>",
                                        jobItemData.contactOrgId, jobItemData.contactOrgName, jobItemData.contactId, jobItemData.contactName, jobItemData.jobOrderId, System.Web.HttpUtility.HtmlEncode(jobItemData.jobOrderName));
                string InfoAboutComments = "";
                if (comments == null || comments == "")
                {
                    InfoAboutComments = "No comments were submitted with the approval.";
                }
                else
                {
                    if (jobItem.LanguageServiceId == 1 && ((bool)jobItem.SupplierIsClientReviewer || jobItemData.contactOrgId == 79702))
                    {
                        InfoAboutComments = string.Format(@"The following comments were submitted with the completion on translation, and have been added to the job item's ""Client contact signoff comments"": <br /><br />m<font color=""green"">" + HttpUtility.HtmlEncode(comments) + "</font>");
                    }
                    else if (jobItem.LanguageServiceId == 1 && jobItem.SupplierIsClientReviewer == false)
                    {
                        InfoAboutComments = string.Format(@"The following comments were submitted with the completion on translation, and have been added to the job item's ""Linguist signoff comments"": <br /><br /><font color=""green"">" + HttpUtility.HtmlEncode(comments) + "</font>");
                    }
                    else
                    {
                        InfoAboutComments = string.Format(@"The following comments were submitted with the approval, and have been added to the job item's ""Client review signoff comments"": <br /><br /><font color=""green"">" + HttpUtility.HtmlEncode(comments) + "</font>");
                    }
                }
                if (jobItem.LanguageServiceId == 1 && (bool)jobItem.SupplierIsClientReviewer == false)
                {
                    var linguistData = linguisticSupRepo.All().Where(x => x.Id == jobItem.LinguisticSupplierOrClientReviewerId && x.DeletedDate == null).FirstOrDefault();
                    string linguistName = linguistData.MainContactFirstName + "" + linguistData.MainContactSurname;
                    string emailSubject = string.Format(@"Online translation job item {0} for {1} has been signed off", jobItemId.ToString(), jobItemData.contactName);
                    string emailBody = string.Format(@"<p>Job item ID <a href=""https://myplusbeta.publicisgroupe.net/JobItem?Id={0}""> {1} </a> (assigned to linguistic supplier <a href=""https://myplusbeta.publicisgroupe.net/Linguist?id={2}""> {3} </a>) has been marked as complete in i plus. <br /><br />{4}<br /> {5} <br /><br />We should proceed to any internal checking stages prior to final delivery to the client; or if this has been carried out by an internal translator at the client then the file will be automatically delivered back to the requesting contact.</p>", jobItemId.ToString(), jobItemId.ToString(), linguistData.Id.ToString(), linguistName, JobInfo, InfoAboutComments);
                    emailUtils.SendMail("flow plus <flowplus@translateplus.com>", NotificationRecipients, emailSubject, emailBody, IsExternalNotification: true);
                }

                // code for batch automation.

                //if (jobItem.LanguageServiceId == 1 && jobItem.SupplierIsClientReviewer == false)
                //{
                //    //var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                //    var BatchFileDirPath = "C:\\Users\\niksharm19\\Desktop";
                //    var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                //    var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                //    string tempBatchFilePath = Path.Combine("\\\\FREDCPAPPDM0004\\translateplusSystemFiles\\Temporary zip folder\\temp batch files", FileName);
                //    System.IO.File.WriteAllText(tempBatchFilePath, System.String.Empty);

                //    System.IO.File.AppendAllText(tempBatchFilePath, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
                //    System.IO.File.AppendAllText(tempBatchFilePath, "<!-- translate plus process automation batch file -->" + Environment.NewLine);

                //    System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<translateplusBatch BatchFormatVersion=\"1.2\" OwnerEmployeeName=\"{0}\" NotifyByEmail=\"{1}\">",
                //                                                               projectManager.EmailAddress, "ITSupport@translateplus.com") + Environment.NewLine);
                //    System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<task Type=\"ReceiveJobItemFromLinguisticSupplierViaExtranet\" TaskNumber=\"1\" JobItemID=\"{0}\"/>",
                //                                                                   jobItemId) + Environment.NewLine);
                //    System.IO.File.AppendAllText(tempBatchFilePath, "</translateplusBatch>" + Environment.NewLine);

                //    System.IO.File.Copy(tempBatchFilePath, BatchFilePath);
                //    System.IO.File.Delete(tempBatchFilePath);
                //}
                //else
                //{
                //    //var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                //    //var BatchFileDirPath = "C:\\Users\\niksharm19\\Desktop";
                //    //var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                //    //var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                //    ////string tempBatchFilePath = Path.Combine("\\\\FREDCPAPPDM0004\\translateplusSystemFiles\\Temporary zip folder\\temp batch files", FileName);
                //    //string tempBatchFilePath = Path.Combine("C:\\Users\\niksharm19\\Desktop\\translateplusSystemFiles\\Temporary zip folder\\temp batch files", FileName);
                //    //System.IO.File.WriteAllText(tempBatchFilePath, System.String.Empty);

                //    //System.IO.File.AppendAllText(tempBatchFilePath, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
                //    //System.IO.File.AppendAllText(tempBatchFilePath, "<!-- translate plus process automation batch file -->" + Environment.NewLine);

                //    //System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<translateplusBatch BatchFormatVersion=\"1.2\" OwnerEmployeeName=\"{0}\" NotifyByEmail=\"{1}\">",
                //    //                                                           projectManager.EmailAddress, "ITSupport@translateplus.com") + Environment.NewLine);
                //    //System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<task Type=\"ReceiveJobItemFromClientReviewViaExtranet\" TaskNumber=\"1\" JobItemID=\"{0}\"/>",
                //    //                                                               jobItemId) + Environment.NewLine);
                //    //System.IO.File.AppendAllText(tempBatchFilePath, "</translateplusBatch>" + Environment.NewLine);

                //    //System.IO.File.Copy(tempBatchFilePath, BatchFilePath);
                //    //System.IO.File.Delete(tempBatchFilePath);

                //}
                XmlDocument BatchDoc = new XmlDocument();
                BatchDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine +
                                 "<!-- translate plus process automation batch file -->" + Environment.NewLine +
                                 "<translateplusBatch />");

                XmlNode RootNode = BatchDoc.SelectSingleNode("//translateplusBatch");

                XmlAttribute BatchDocAttr = BatchDoc.CreateAttribute("BatchFormatVersion");
                BatchDocAttr.Value = "1.0";
                RootNode.Attributes.Append(BatchDocAttr);

                BatchDocAttr = BatchDoc.CreateAttribute("OwnerEmployeeName");
                BatchDocAttr.Value = "iplus";
                RootNode.Attributes.Append(BatchDocAttr);

                //write e-mail notification address(es) info
                BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
                BatchDocAttr.Value = projectManager.EmailAddress;
                RootNode.Attributes.Append(BatchDocAttr);


                XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

                if (jobItem.LanguageServiceId == 1 && jobItem.SupplierIsClientReviewer == false)
                {
                    //write task type info
                    BatchDocAttr = BatchDoc.CreateAttribute("Type");
                    BatchDocAttr.Value = "ReceiveJobItemFromLinguisticSupplierViaExtranet";
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);
                }
                else
                {
                    //write task type info
                    BatchDocAttr = BatchDoc.CreateAttribute("Type");
                    BatchDocAttr.Value = "ReceiveJobItemFromClientReviewViaExtranet";
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                }


                //write task number info 
                BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
                BatchDocAttr.Value = "1";
                IndividualTaskNode.Attributes.Append(BatchDocAttr);

                BatchDocAttr = BatchDoc.CreateAttribute("JobItemID");
                BatchDocAttr.Value = jobItemId.ToString();
                IndividualTaskNode.Attributes.Append(BatchDocAttr);

                //now append the node to the doc
                RootNode.AppendChild(IndividualTaskNode);

                var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                BatchDoc.Save(BatchFilePath);

            }
            catch (Exception ex)
            {
                throw;
            }
            return rowsAffected;
        }

        public async Task<string> AutomateFinancialExpressMergeExcel(int EmployeeID, int OrderID)
        {
            var thisEMP = new Employee();
            thisEMP = await empService.IdentifyCurrentUserById(EmployeeID);
            string automationMessage = "Success";
            try
            {
                var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-12";
                var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                string tempBatchFilePath = Path.Combine("\\\\FREDCPAPPDM0004\\translateplusSystemFiles\\Temporary zip folder\\temp batch files", FileName);
                System.IO.File.WriteAllText(tempBatchFilePath, System.String.Empty);

                System.IO.File.AppendAllText(tempBatchFilePath, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
                System.IO.File.AppendAllText(tempBatchFilePath, "<!-- translate plus process automation batch file -->" + Environment.NewLine);

                System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<translateplusBatch BatchFormatVersion=\"1.2\" OwnerEmployeeName=\"{0}\" NotifyByEmail=\"{1}\">",
                                                                           thisEMP.EmailAddress, "ITSupport@translateplus.com, " + thisEMP.EmailAddress) + Environment.NewLine);
                System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<task Type=\"FinancialExpressDelivery\" TaskNumber=\"1\" JobOrderID=\"{0}\" />",
                                                                               OrderID) + Environment.NewLine);
                System.IO.File.AppendAllText(tempBatchFilePath, "</translateplusBatch>" + Environment.NewLine);

                System.IO.File.Copy(tempBatchFilePath, BatchFilePath);
                System.IO.File.Delete(tempBatchFilePath);


            }

            catch (Exception ex)
            {
                throw;
            }
            return automationMessage;
        }
        public async Task<string> AutomateFinancialExpressExcel(int EmployeeID, int OrderID)
        {
            var thisEMP = new Employee();
            thisEMP = await empService.IdentifyCurrentUserById(EmployeeID);
            var order = GetById(OrderID);
            var contact = contactService.GetById(order.Result.ContactId);
            var org = orgService.GetOrgDetails(contact.Result.OrgId);
            string tradosTemplate = org.Result.TradosProjectTemplatePath;
            string automationMessage = "Success";
            try
            {

                //var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-12";
                var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                string tempBatchFilePath = Path.Combine("\\\\FREDCPAPPDM0004\\translateplusSystemFiles\\Temporary zip folder\\temp batch files", FileName);
                System.IO.File.WriteAllText(tempBatchFilePath, System.String.Empty);

                System.IO.File.AppendAllText(tempBatchFilePath, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
                System.IO.File.AppendAllText(tempBatchFilePath, "<!-- translate plus process automation batch file -->" + Environment.NewLine);

                System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<translateplusBatch BatchFormatVersion=\"1.2\" OwnerEmployeeName=\"{0}\" NotifyByEmail=\"{1}\">",
                                                                           thisEMP.EmailAddress, "ITSupport@translateplus.com") + Environment.NewLine);
                System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<task Type=\"AutoProcessJobs\" TaskNumber=\"1\" JobOrderID=\"{0}\" TradosProjectTemplatePath=\"{1}\" ToPreTranslateAfterAnalysis=\"True\" IsFromJobOrderPage=\"False\" UseFilesForAllItems=\"True\" />",
                                                                               OrderID, tradosTemplate) + Environment.NewLine);
                System.IO.File.AppendAllText(tempBatchFilePath, "</translateplusBatch>" + Environment.NewLine);

                System.IO.File.Copy(tempBatchFilePath, BatchFilePath);
                System.IO.File.Delete(tempBatchFilePath);
            }

            catch (Exception ex)
            {
                throw;
            }
            return automationMessage;
        }

        //public string ExtranetFolderForSupplierPath(int jobItemId)
        //{
        //    string path = "";
        //    string extranetToFolderForSupplierPath = "";
        //    var jobItem = jobitemsRepo.All().Where(x => x.Id == jobItemId && x.DeletedDateTime == null).FirstOrDefault();
        //    var lingusticSupplierId = jobItem.LinguisticSupplierOrClientReviewerId;
        //    if (lingusticSupplierId != null)
        //    {
        //        string ItemFolderOnExtranetBase = Path.Combine(Path.Combine(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Suppliers"), lingusticSupplierId.ToString()), jobItemId.ToString());
        //        string ItemFolderOnExtranetBaseNAS1 = Path.Combine(Path.Combine(Path.Combine(globalVariables.OldExtranetNetworkBaseDirectoryPathNAS1, "Suppliers"), lingusticSupplierId.ToString()), jobItemId.ToString());
        //        string ItemFolderOnExtranetBaseDEV1 = Path.Combine(Path.Combine(Path.Combine(globalVariables.OldExtranetNetworkBaseDirectoryPathDEV1, "Suppliers"), lingusticSupplierId.ToString()), jobItemId.ToString());
        //        string ItemDirPath = ItemFolderOnExtranetBase;

        //        if (!Directory.Exists(ItemFolderOnExtranetBase))
        //        {
        //            if (Directory.Exists(ItemFolderOnExtranetBaseNAS1))
        //            {
        //                ItemDirPath = ItemFolderOnExtranetBaseNAS1;
        //            }
        //            else
        //            {
        //                if (Directory.Exists(ItemFolderOnExtranetBaseDEV1))
        //                {
        //                    ItemDirPath = ItemFolderOnExtranetBaseDEV1;
        //                }
        //            }
        //        }
        //        path = ItemDirPath;
        //    }
        //    if (path != "")
        //    {
        //        if (Directory.Exists(Path.Combine(path, "01 To")))
        //        {
        //            extranetToFolderForSupplierPath = Path.Combine(path, "01 To");
        //        }
        //    }
        //    return extranetToFolderForSupplierPath;
        //}
        //public string ExtranetReviewPlusFromReviewDirectoryPathForApp(int jobItemId)
        //{
        //    var jobItem = jobitemsRepo.All().Where(x => x.Id == jobItemId && x.DeletedDateTime == null).FirstOrDefault();
        //    var jobOrder = orderRepository.All().Where(x => x.Id == jobItem.JobOrderId && x.DeletedDate == null).FirstOrDefault();
        //    string sourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == jobItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name;
        //    string targetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == jobItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name;
        //    string ExpectedPath = "";
        //    string ParentContactExtranetFolderPath = ExtranetAndWSDirectoryPathForApp(jobOrder.Id);
        //    if (ParentContactExtranetFolderPath != "")
        //    {
        //        ExpectedPath = Path.Combine(ParentContactExtranetFolderPath, "Review\\" + jobItemId.ToString() + " - " + MakeStringSafeForFileSystemPath(sourceLanguage) +
        //                                           "-" + MakeStringSafeForFileSystemPath(targetLanguage) + "\\FromReview");
        //        if (!Directory.Exists(ExpectedPath))
        //        {
        //            string OldExpectedPath = "";
        //            DirectoryInfo DirInfo = null;
        //            DirectoryInfo[] MatchingContactDirs;
        //            if (Directory.Exists(globalVariables.OldExtranetNetworkBaseDirectoryPathNAS1))
        //            {
        //                DirInfo = new DirectoryInfo(Path.Combine(globalVariables.OldExtranetNetworkBaseDirectoryPathNAS1, "Contacts"));
        //                MatchingContactDirs = DirInfo.GetDirectories(jobOrder.ContactId + "*", SearchOption.TopDirectoryOnly);
        //                if (MatchingContactDirs.Count() > 0)
        //                {
        //                    OldExpectedPath = MatchingContactDirs[0].FullName;
        //                    DirInfo = new DirectoryInfo(OldExpectedPath);
        //                    DirectoryInfo[] MatchingOrderDirs = DirInfo.GetDirectories(jobOrder.Id.ToString() + "*", SearchOption.TopDirectoryOnly);
        //                    if (MatchingOrderDirs.Count() > 0)
        //                    {
        //                        OldExpectedPath = Path.Combine(MatchingOrderDirs[0].FullName, "Review\\" + jobItemId.ToString() + " - " + MakeStringSafeForFileSystemPath(sourceLanguage) +
        //                            "-" + MakeStringSafeForFileSystemPath(targetLanguage));
        //                        if (Directory.Exists(OldExpectedPath))
        //                            ExpectedPath = OldExpectedPath;
        //                    }
        //                    else
        //                    {
        //                        if (Directory.Exists(globalVariables.OldExtranetNetworkBaseDirectoryPathDEV1))
        //                        {
        //                            DirInfo = new DirectoryInfo(Path.Combine(globalVariables.OldExtranetNetworkBaseDirectoryPathDEV1, "Contacts"));
        //                            MatchingContactDirs = DirInfo.GetDirectories(jobOrder.ContactId.ToString() + "*", SearchOption.TopDirectoryOnly);
        //                            if (MatchingContactDirs.Count() > 0)
        //                            {
        //                                OldExpectedPath = MatchingContactDirs[0].FullName;
        //                                DirInfo = new DirectoryInfo(OldExpectedPath);
        //                                MatchingOrderDirs = DirInfo.GetDirectories(jobOrder.Id.ToString() + "*", SearchOption.TopDirectoryOnly);
        //                                if (MatchingOrderDirs.Count() > 0)
        //                                {
        //                                    OldExpectedPath = Path.Combine(MatchingOrderDirs[0].FullName, "Review\\" +
        //                                                   jobItemId.ToString() + " - " + MakeStringSafeForFileSystemPath(sourceLanguage) +
        //                                                   "-" + MakeStringSafeForFileSystemPath(targetLanguage));
        //                                    if (Directory.Exists(OldExpectedPath))
        //                                        ExpectedPath = OldExpectedPath;
        //                                }
        //                                else
        //                                {
        //                                    if (jobItem.CreatedDateTime > new DateTime(2018, 1, 1))
        //                                    {
        //                                        Directory.CreateDirectory(ExpectedPath);
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (jobItem.CreatedDateTime > new DateTime(2018, 1, 1))
        //                                {
        //                                    Directory.CreateDirectory(ExpectedPath);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (Directory.Exists(globalVariables.OldExtranetNetworkBaseDirectoryPathDEV1))
        //                    {
        //                        DirInfo = new DirectoryInfo(Path.Combine(globalVariables.OldExtranetNetworkBaseDirectoryPathDEV1, "Contacts"));
        //                        MatchingContactDirs = DirInfo.GetDirectories(jobOrder.ContactId.ToString() + "*", SearchOption.TopDirectoryOnly);
        //                        if (MatchingContactDirs.Count() > 0)
        //                        {
        //                            OldExpectedPath = MatchingContactDirs[0].FullName;
        //                            DirInfo = new DirectoryInfo(OldExpectedPath);
        //                            DirectoryInfo[] MatchingOrderDirs = DirInfo.GetDirectories(jobOrder.Id.ToString() + "*", SearchOption.TopDirectoryOnly);
        //                            if (MatchingOrderDirs.Count() > 0)
        //                            {
        //                                OldExpectedPath = Path.Combine(MatchingOrderDirs[0].FullName, "Review\\" +
        //                                                       jobItemId.ToString() + " - " + MakeStringSafeForFileSystemPath(sourceLanguage) +
        //                                                       "-" + MakeStringSafeForFileSystemPath(targetLanguage));
        //                                if (Directory.Exists(OldExpectedPath))
        //                                    ExpectedPath = OldExpectedPath;
        //                            }
        //                            else
        //                            {
        //                                if (jobItem.CreatedDateTime > new DateTime(2018, 1, 1))
        //                                {
        //                                    Directory.CreateDirectory(ExpectedPath);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (jobItem.CreatedDateTime > new DateTime(2018, 1, 1))
        //                            {
        //                                Directory.CreateDirectory(ExpectedPath);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else if (Directory.Exists(globalVariables.OldExtranetNetworkBaseDirectoryPathDEV1))
        //            {
        //                DirInfo = new DirectoryInfo(Path.Combine(globalVariables.OldExtranetNetworkBaseDirectoryPathDEV1, "Contacts"));
        //                MatchingContactDirs = DirInfo.GetDirectories(jobOrder.ContactId.ToString() + "*", SearchOption.TopDirectoryOnly);
        //                if (MatchingContactDirs.Count() > 0)
        //                {
        //                    OldExpectedPath = MatchingContactDirs[0].FullName;
        //                    DirInfo = new DirectoryInfo(OldExpectedPath);
        //                    DirectoryInfo[] MatchingOrderDirs = DirInfo.GetDirectories(jobOrder.Id.ToString() + "*", SearchOption.TopDirectoryOnly);
        //                    if (MatchingOrderDirs.Count() > 0)
        //                    {
        //                        OldExpectedPath = Path.Combine(MatchingOrderDirs[0].FullName, "Review\\" +
        //                                               jobItemId.ToString() + " - " + MakeStringSafeForFileSystemPath(sourceLanguage) +
        //                                               "-" + MakeStringSafeForFileSystemPath(targetLanguage));
        //                        if (Directory.Exists(OldExpectedPath))
        //                            ExpectedPath = OldExpectedPath;
        //                    }
        //                    else
        //                    {
        //                        if (jobItem.CreatedDateTime > new DateTime(2018, 1, 1))
        //                        {
        //                            Directory.CreateDirectory(ExpectedPath);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (jobItem.CreatedDateTime > new DateTime(2018, 1, 1))
        //                    {
        //                        Directory.CreateDirectory(ExpectedPath);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return ExpectedPath;
        //}

        private string MakeStringSafeForFileSystemPath(string stringToProcess)
        {
            stringToProcess = stringToProcess.Replace(":", "-");
            stringToProcess = stringToProcess.Replace("<", "-");
            stringToProcess = stringToProcess.Replace(">", "-");
            stringToProcess = stringToProcess.Replace(":", "-");
            stringToProcess = stringToProcess.Replace(@"""", "-");
            stringToProcess = stringToProcess.Replace("/", "-");
            stringToProcess = stringToProcess.Replace(@"\", " - ");
            stringToProcess = stringToProcess.Replace("|", "-");
            stringToProcess = stringToProcess.Replace("?", "-");
            stringToProcess = stringToProcess.Replace("*", "-");
            stringToProcess = stringToProcess.Replace("~", "-");
            stringToProcess = stringToProcess.Replace(@"\t", " ");
            stringToProcess.Trim();
            return stringToProcess;
        }

        public string GetTotalSourceLangsCountString(int orderId, string IANACode)
        {
            var result = GetAllJobItems(orderId).Result;

            List<string> allSourceLang = new List<string>();
            if (result.Count > 0)
            {
                foreach (JobItem item in result)
                {
                    if (allSourceLang.Contains(item.SourceLanguageIanacode) == false)
                    {
                        allSourceLang.Add(item.SourceLanguageIanacode);
                    }
                }

            }

            if (allSourceLang.Count == 1)
            {
                var langObj = langService.GetLanguageInfo(allSourceLang.ElementAt(0), IANACode);
                return langObj.Name;
            }
            else
            {
                if (IANACode == "en")
                {
                    return allSourceLang.Count().ToString() + " languages";
                }
                else if (IANACode == "da")
                {
                    return allSourceLang.Count().ToString() + " sprog";
                }
                else if (IANACode == "de")
                {
                    return allSourceLang.Count().ToString() + " Sprachen";
                }
                else if (IANACode == "sv")
                {
                    return allSourceLang.Count().ToString() + " språk";
                }
                else
                {
                    return allSourceLang.Count().ToString() + " languages";
                }

            }

        }

        //public async Task<List<JobItem>> GetBySourceLangForJobOrderId(int JobOrderID)
        //    var result = await jobitemsRepo.All().Where(a => a.JobOrderId == JobOrderID && a.DeletedDateTime == null && a.SourceLanguageIanacode == SourceLanguageIANACode).OrderBy(a => a.SourceLanguageIanacode).ToListAsync();
        //    return result;
        //}

        //public async Task<List<JobItem>> GetByTargetLangForJobOrderId(int JobOrderID, string TargetLanguageIANACode)
        //{
        //    var result = await jobitemsRepo.All().Where(a => a.JobOrderId == JobOrderID && a.DeletedDateTime == null && a.TargetLanguageIanacode == TargetLanguageIANACode).OrderBy(a => a.OurCompletionDeadline).ToListAsync();
        //    return result;
        //}

        public async Task<List<JobItem>> GetAllJobItems(int JobOrderID)
        {
            var result = await jobitemsRepo.All().Where(a => a.JobOrderId == JobOrderID && a.DeletedDateTime == null).ToListAsync();
            return result;
        }
        public async Task<JobOrder> CreateJobOrder(JobOrder model)
        {


            int NewJobOrderID;
            JobOrder NewJobOrder = null;

            //also ensure that if a linked job order ID has been supplied, that the job order in 


            if (model.LinkedJobOrderId != null)
            {
                var linkedOrder = await orderRepository.All().Where(o => o.Id == model.LinkedJobOrderId && o.DeletedDate == null).FirstOrDefaultAsync();
                if (linkedOrder == null)
                {
                    throw new Exception("The linked job order ID, " + model.LinkedJobOrderId.ToString() + ", does not exist in the database. You cannot link this order to a non-existent order.");
                }
            }
            NewJobOrder = new JobOrder()
            {
                ContactId = model.ContactId,
                IsActuallyOnlyAquote = model.IsActuallyOnlyAquote,
                PrintingProject = model.PrintingProject,
                ProjectManagerEmployeeId = model.ProjectManagerEmployeeId,
                JobOrderChannelId = model.JobOrderChannelId,
                JobName = model.JobName,
                ClientNotes = model.ClientNotes,
                InternalNotes = model.InternalNotes,
                ClientPonumber = model.ClientPonumber,
                OrgHfmcodeBs = model.OrgHfmcodeBs,
                OrgHfmcodeIs = model.OrgHfmcodeIs,
                CustomerSpecificField1Value = model.CustomerSpecificField1Value,
                CustomerSpecificField2Value = model.CustomerSpecificField2Value,
                CustomerSpecificField3Value = model.CustomerSpecificField3Value,
                CustomerSpecificField4Value = model.CustomerSpecificField4Value,
                OverallDeliveryDeadline = model.OverallDeliveryDeadline,
                Priority = model.Priority,
                ClientCurrencyId = model.ClientCurrencyId,
                InvoicingNotes = model.InvoicingNotes,
                EndClientId = model.EndClientId,
                BrandId = model.BrandId,
                CampaignId = model.CampaignId,
                CategoryId = model.CategoryId,
                LinkedJobOrderId = model.LinkedJobOrderId,
                ExtranetNotifyClientReviewersOfDeliveries = model.ExtranetNotifyClientReviewersOfDeliveries,
                IsAtrialProject = model.IsAtrialProject,
                IsAcmsproject = model.IsAcmsproject,
                IsHighlyConfidential = model.IsHighlyConfidential,
                IsCls = model.IsCls,
                SubmittedDateTime = timeZonesService.GetCurrentGMT(),
                SetupByEmployeeId = model.SetupByEmployeeId,
                SurchargeId = model.SurchargeId,
                DiscountId = model.DiscountId,
                TypeOfOrder = model.TypeOfOrder

            };
            await orderRepository.AddAsync(NewJobOrder);
            await orderRepository.SaveChangesAsync();
            NewJobOrderID = NewJobOrder.Id;

            return NewJobOrder;
        }
        public async Task<JobOrder> UpdateOrder(int ID, Int16 ProjectManagerEmployeeID, Int16 OrderChannelID, string JobName, string ClientNotes, string InternalNotes, string ClientPONumber, string CustomerSpecificField1Value, string CustomerSpecificField2Value, string CustomerSpecificField3Value, string CustomerSpecificField4Value, DateTime OverallDeliveryDeadlineDate, int OverallDeliveryDeadlineHour, int OverallDeliveryDeadlineMinute, bool OverallIsComplete, Int16 ClientCurrencyID, int LinkedJobOrderID, int ClientInvoiceID, bool IsATrialProject, bool IsACMSProject, bool IsHighlyConfidential, string InvoicingNotes, DateTime EarlyInvoiceDateTime, int EarlyInvoiceByEmpID, int OverdueReasonID, string OverdueReasonComment, string OrgHFMCodeIS = "", string OrgHFMCodeBS = "", bool PrintingProject = false, DateTime OriginalOverallDeliveryDeadline = default(DateTime), string DeadlineChangeReason = "", Int16 Priority = 0, bool EscalatedToAccountManager = false, int EndClientID = 0, int BrandID = 0, int CategoryID = 0, int CampaignID = 0, bool IsCLS = false)
        {

            var ExistingOrder = await GetById(ID);
            // ensure a valid order ID has been passed
            if (ExistingOrder == null)
                throw new Exception("The order ID, " + ID.ToString() + ", does not exist in the database.");
            else
            {

                // ensure this is not trying to update an order which has already been invoiced and finalised
                //TPJobOrder ExistingOrder = new TPJobOrder(ID);
                var clientInvoice = clientInvoiceRepo.All().Where(x => x.Id == ExistingOrder.ClientInvoiceId && x.DeletedDateTime == null).FirstOrDefault();
                if (clientInvoice != null)
                {
                    if (clientInvoice.IsFinalised == true & ExistingOrder.EarlyInvoiceDateTime == DateTime.MinValue)
                        throw new Exception("The order ID, " + ID.ToString() + ", has already been invoiced. You cannot modify details for an invoiced and finalised order.");
                }
                // ExistingOrder = Nothing

                // also ensure that if a linked job order ID has been supplied, that the job order in 
                // question also exists
                var LinkedJobOrder = await GetById(LinkedJobOrderID);
                if (LinkedJobOrder != null)
                    throw new Exception("The linked job order ID, " + LinkedJobOrderID.ToString() + ", does not exist in the database. You cannot link this order to a non-existent order.");

                // input strings should have been limited in the interface but limit as
                // a precaution and trim
                if (JobName != null && JobName.Length > 200) { JobName = JobName.Trim().Substring(0, 200); }

                if (ClientNotes != null) { ClientNotes = ClientNotes.Trim(); }
                if (InternalNotes != null) { InternalNotes = InternalNotes.Trim(); }
                if (ClientPONumber != null && ClientPONumber.Length > 100) { ClientPONumber = ClientPONumber.Trim().Substring(0, 100); }
                if (CustomerSpecificField1Value != null) { CustomerSpecificField1Value = CustomerSpecificField1Value.Trim(); }
                if (CustomerSpecificField2Value != null) { CustomerSpecificField2Value = CustomerSpecificField2Value.Trim(); }
                if (CustomerSpecificField3Value != null) { CustomerSpecificField3Value = CustomerSpecificField3Value.Trim(); }
                if (CustomerSpecificField4Value != null) { CustomerSpecificField4Value = CustomerSpecificField4Value.Trim(); }
                if (InvoicingNotes != null) { InvoicingNotes = InvoicingNotes.Trim(); }
                if (OverdueReasonComment != null) { OverdueReasonComment = OverdueReasonComment.Trim(); } else { OverdueReasonComment = ""; }
                if (OrgHFMCodeIS != null) { OrgHFMCodeIS = OrgHFMCodeIS.Trim(); }
                if (OrgHFMCodeBS != null) { OrgHFMCodeBS = OrgHFMCodeBS.Trim(); }


                DateTime CompositeDeliveryDateTime;
                CompositeDeliveryDateTime = new DateTime(OverallDeliveryDeadlineDate.Year, OverallDeliveryDeadlineDate.Month, OverallDeliveryDeadlineDate.Day, OverallDeliveryDeadlineHour, OverallDeliveryDeadlineMinute, 0);

                var orderContact = contactRepo.All().Where(x => x.Id == ExistingOrder.ContactId).FirstOrDefault();
                SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);
                var OrderDataAdapter = new SqlDataAdapter();
                OrderDataAdapter.UpdateCommand = new SqlCommand("procUpdateJobOrderDetails", SQLConn);
                {
                    var withBlock = OrderDataAdapter.UpdateCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.CommandTimeout = 300;
                    withBlock.Parameters.Add("@OrderID", SqlDbType.Int).Value = ID;
                    withBlock.Parameters.Add("@UpdatedProjectManagerEmployeeID", SqlDbType.SmallInt).Value = ProjectManagerEmployeeID;
                    withBlock.Parameters.Add("@UpdatedJobOrderChannelID", SqlDbType.SmallInt).Value = OrderChannelID;
                    withBlock.Parameters.Add("@UpdatedJobName", SqlDbType.NVarChar, 200).Value = JobName;
                    withBlock.Parameters.Add("@UpdatedClientNotes", SqlDbType.NVarChar, -1).Value = ClientNotes;
                    withBlock.Parameters.Add("@UpdatedInternalNotes", SqlDbType.NVarChar, -1).Value = InternalNotes;
                    withBlock.Parameters.Add("@UpdatedClientPONumber", SqlDbType.NVarChar, 100).Value = ClientPONumber;
                    if (CustomerSpecificField2Value == null && orderContact.OrgId == 138545)
                    {
                        CustomerSpecificField2Value = "http://nescafegold.marketforward.com";
                    }
                    else if (CustomerSpecificField2Value == null && orderContact.OrgId == 138711) //nestle pure life
                    {
                        CustomerSpecificField2Value = "http://nestlepurelife.marketforward.com/";
                    }
                    else if (CustomerSpecificField2Value == null && orderContact.OrgId == 138994) //meetic
                    {
                        CustomerSpecificField2Value = "http://newbiz-prodigious.marketforward.com/";
                    }
                    withBlock.Parameters.Add("@UpdatedCustomerSpecificField1Value", SqlDbType.NVarChar, -1).Value = CustomerSpecificField1Value;
                    withBlock.Parameters.Add("@UpdatedCustomerSpecificField2Value", SqlDbType.NVarChar, -1).Value = CustomerSpecificField2Value;
                    withBlock.Parameters.Add("@UpdatedCustomerSpecificField3Value", SqlDbType.NVarChar, -1).Value = CustomerSpecificField3Value;
                    withBlock.Parameters.Add("@UpdatedCustomerSpecificField4Value", SqlDbType.NVarChar, -1).Value = CustomerSpecificField4Value;
                    withBlock.Parameters.Add("@UpdatedOverallDeliveryDeadline", SqlDbType.DateTime).Value = CompositeDeliveryDateTime;
                    withBlock.Parameters.Add("@UpdatedPriority", SqlDbType.SmallInt).Value = Priority;
                    withBlock.Parameters.Add("@UpdatedInvoicingNotes", SqlDbType.NVarChar, -1).Value = InvoicingNotes;
                    withBlock.Parameters.Add("@OverdueComment", SqlDbType.NVarChar, -1).Value = OverdueReasonComment;
                    if (OrgHFMCodeIS != "")
                        withBlock.Parameters.Add("@OrgHFMCodeIS", SqlDbType.NVarChar, -1).Value = OrgHFMCodeIS;
                    else if (ExistingOrder != null && ExistingOrder.OrgHfmcodeIs != "" && ExistingOrder.OrgHfmcodeIs != null)
                        withBlock.Parameters.Add("@OrgHFMCodeIS", SqlDbType.NVarChar, -1).Value = ExistingOrder.OrgHfmcodeIs;
                    else
                        withBlock.Parameters.Add("@OrgHFMCodeIS", SqlDbType.NVarChar, -1).Value = DBNull.Value;
                    if (OrgHFMCodeBS != "")
                        withBlock.Parameters.Add("@OrgHFMCodeBS", SqlDbType.NVarChar, -1).Value = OrgHFMCodeBS;
                    else if (ExistingOrder != null && ExistingOrder.OrgHfmcodeBs != "" && ExistingOrder.OrgHfmcodeBs != null)
                        withBlock.Parameters.Add("@OrgHFMCodeBS", SqlDbType.NVarChar, -1).Value = ExistingOrder.OrgHfmcodeBs;
                    else
                        withBlock.Parameters.Add("@OrgHFMCodeBS", SqlDbType.NVarChar, -1).Value = DBNull.Value;

                    if (DeadlineChangeReason != null)
                        withBlock.Parameters.Add("@DeadlineChangeReason", SqlDbType.NVarChar, -1).Value = DeadlineChangeReason;
                    else
                        withBlock.Parameters.Add("@DeadlineChangeReason", SqlDbType.NVarChar, -1).Value = "";

                    withBlock.Parameters.Add("@EscalatedToAccountManager", SqlDbType.Bit).Value = EscalatedToAccountManager;

                    // if order is marked as complete, log date/time but ONLY if it is not already complete; if it
                    // is already complete then retain the original completed date
                    if (OverallIsComplete == true)
                    {
                        var OrderWithExistingDetails = await GetById(ID);
                        if (OrderWithExistingDetails != null)
                        {
                            if (OrderWithExistingDetails.OverallCompletedDateTime == DateTime.MinValue)
                                withBlock.Parameters.Add("@UpdatedOverallCompletedDateTime", SqlDbType.DateTime).Value = GeneralUtils.GetCurrentGMT();
                            else
                                withBlock.Parameters.Add("@UpdatedOverallCompletedDateTime", SqlDbType.DateTime).Value = OrderWithExistingDetails.OverallCompletedDateTime;
                        }
                        OrderWithExistingDetails = null/* TODO Change to default(_) if this is not a reference type */;
                    }
                    else
                        withBlock.Parameters.Add("@UpdatedOverallCompletedDateTime", SqlDbType.DateTime).Value = DBNull.Value;

                    withBlock.Parameters.Add("@UpdatedClientCurrencyID", SqlDbType.SmallInt).Value = ClientCurrencyID;


                    if (EndClientID == 0)
                        withBlock.Parameters.Add("@EndClientID", SqlDbType.Int).Value = DBNull.Value;
                    else
                        withBlock.Parameters.Add("@EndClientID", SqlDbType.Int).Value = EndClientID;

                    if (BrandID == 0)
                        withBlock.Parameters.Add("@BrandID", SqlDbType.Int).Value = DBNull.Value;
                    else
                        withBlock.Parameters.Add("@BrandID", SqlDbType.Int).Value = BrandID;

                    if (CampaignID == 0)
                        withBlock.Parameters.Add("@CampaignID", SqlDbType.Int).Value = DBNull.Value;
                    else
                        withBlock.Parameters.Add("@CampaignID", SqlDbType.Int).Value = CampaignID;

                    if (CategoryID == 0)
                        withBlock.Parameters.Add("@CategoryID", SqlDbType.Int).Value = DBNull.Value;
                    else
                        withBlock.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CategoryID;


                    if (LinkedJobOrderID == 0)
                        withBlock.Parameters.Add("@UpdatedLinkedJobOrderID", SqlDbType.Int).Value = DBNull.Value;
                    else
                        withBlock.Parameters.Add("@UpdatedLinkedJobOrderID", SqlDbType.Int).Value = LinkedJobOrderID;

                    if (ClientInvoiceID == 0)
                        withBlock.Parameters.Add("@UpdatedClientInvoiceID", SqlDbType.Int).Value = DBNull.Value;
                    else
                        withBlock.Parameters.Add("@UpdatedClientInvoiceID", SqlDbType.Int).Value = ClientInvoiceID;

                    // .Parameters.Add("@PrintingProject", SqlDbType.Bit).Value = PrintingProject
                    withBlock.Parameters.Add("@UpdatedIsATrialProject", SqlDbType.Bit).Value = IsATrialProject;
                    withBlock.Parameters.Add("@UpdatedIsACMSProject", SqlDbType.Bit).Value = IsACMSProject;
                    withBlock.Parameters.Add("@UpdatedIsHighlyConfidential", SqlDbType.Bit).Value = IsHighlyConfidential;
                    withBlock.Parameters.Add("@PrintingProject", SqlDbType.Bit).Value = PrintingProject;
                    withBlock.Parameters.Add("@IsCLS", SqlDbType.Bit).Value = IsCLS;

                    if (EarlyInvoiceDateTime == DateTime.MinValue)
                        withBlock.Parameters.Add("@EarlyInvoiceDateTime", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        withBlock.Parameters.Add("@EarlyInvoiceDateTime", SqlDbType.DateTime).Value = EarlyInvoiceDateTime;

                    if (EarlyInvoiceByEmpID == 0)
                        withBlock.Parameters.Add("@EarlyInvoiceByEmpID", SqlDbType.SmallInt).Value = DBNull.Value;
                    else
                        withBlock.Parameters.Add("@EarlyInvoiceByEmpID", SqlDbType.SmallInt).Value = EarlyInvoiceByEmpID;

                    if (OverdueReasonID == 0)
                        withBlock.Parameters.Add("@OverdueReasonID", SqlDbType.SmallInt).Value = DBNull.Value;
                    else
                        withBlock.Parameters.Add("@OverdueReasonID", SqlDbType.SmallInt).Value = OverdueReasonID;

                    // log most recent change
                    withBlock.Parameters.Add("@LastModifiedDate", SqlDbType.DateTime).Value = GeneralUtils.GetCurrentGMT();
                    // NB the Web Service now handles some updates directly during file pre-processing, etc.,
                    // so if no current HTTPContext user, log it as i plus
                    SqlParameter LastModifiedByParameter = withBlock.Parameters.Add("@LastModifiedByEmployeeID", SqlDbType.SmallInt);
                    try
                    {

                        LastModifiedByParameter.Value = globalVariables.iplusEmployeeID;
                    }
                    catch (Exception ex)
                    {
                        LastModifiedByParameter.Value = globalVariables.iplusEmployeeID;
                    }

                    // return value parameter:
                    SqlParameter ReturnValParam = withBlock.Parameters.Add("@RowCount", SqlDbType.Int);
                    ReturnValParam.Direction = ParameterDirection.ReturnValue;
                }

                try
                {
                    OrderDataAdapter.UpdateCommand.Connection.Open();
                    OrderDataAdapter.UpdateCommand.ExecuteNonQuery();
                    if (System.Convert.ToInt32(OrderDataAdapter.UpdateCommand.Parameters["@RowCount"].Value) != 1)
                    {
                        throw new Exception("Order update was not successful.");
                    }
                    return ExistingOrder;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    try
                    {
                        // Clean up
                        OrderDataAdapter.UpdateCommand.Connection.Close();
                        OrderDataAdapter.Dispose();
                    }
                    catch (SqlException SE)
                    {
                        throw new Exception("SQL exception: " + SE.Message);
                    }
                }
            } // if the task ID exists
        }

        public List<int> GetJobOrderIDsByExactNamePOOrNotes(string JobNameSearchStr, string PONumSearchStr, string NotesSearchStr, DateTime SubmittedStartDate, DateTime SubmittedEndDate, int OrgID = 0, bool AlsoCheckForDeletedJobs = false, bool GetJobsForArchiving = false)
        {
            List<int> JobOrderIDsList = new List<int>();

            // tidy variables for relevant searching
            if (SubmittedStartDate == DateTime.MinValue)
                SubmittedStartDate = new DateTime(year: 2008, month: 8, day: 15, hour: 0, minute: 0, second: 0);
            if (SubmittedEndDate == DateTime.MinValue)
                SubmittedEndDate = GeneralUtils.GetCurrentGMT();

            JobNameSearchStr = JobNameSearchStr.Trim();
            if (JobNameSearchStr == "")
                JobNameSearchStr = "%";

            NotesSearchStr = NotesSearchStr.Trim();
            if (NotesSearchStr == "")
                NotesSearchStr = "%";
            else
                NotesSearchStr = "%" + NotesSearchStr + "%";

            PONumSearchStr = PONumSearchStr.Trim();
            if (PONumSearchStr == "")
                PONumSearchStr = "%";
            else
                PONumSearchStr = "%" + PONumSearchStr.Substring(0, 98) + "%";


            SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);

            // set up SQL query
            var OrdersDataAdapter = new SqlDataAdapter("procGetJobOrdersByNamePOOrNotes", SQLConn);

            if (GetJobsForArchiving == true)
                OrdersDataAdapter = new SqlDataAdapter("procGetJobOrdersByNamePOOrNotesForArchiving", SQLConn);

            {
                var withBlock = OrdersDataAdapter.SelectCommand;
                // set up key parameters
                withBlock.CommandType = CommandType.StoredProcedure;
                DateTime StartDateForQuery = new DateTime(SubmittedStartDate.Year, SubmittedStartDate.Month, SubmittedStartDate.Day, 0, 0, 0);
                DateTime EndDateForQuery = new DateTime(SubmittedEndDate.Year, SubmittedEndDate.Month, SubmittedEndDate.Day, 23, 59, 59);
                withBlock.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDateForQuery;
                withBlock.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDateForQuery;
                withBlock.Parameters.Add("@JobNameLike", SqlDbType.NVarChar, 200).Value = JobNameSearchStr;
                withBlock.Parameters.Add("@ClientPONumLike", SqlDbType.NVarChar, 100).Value = PONumSearchStr;
                withBlock.Parameters.Add("@NotesLike", SqlDbType.NVarChar, -1).Value = NotesSearchStr;
                withBlock.Parameters.Add("@OrgID", SqlDbType.Int).Value = OrgID;
                withBlock.Parameters.Add("@AlsoCheckForDeletedJobs", SqlDbType.Bit).Value = AlsoCheckForDeletedJobs;
            }


            try
            {
                DataSet OrderIDs = new DataSet();
                OrdersDataAdapter.Fill(OrderIDs);

                // collect each ID returned
                foreach (DataRow OrderRow in OrderIDs.Tables[0].Rows)
                    JobOrderIDsList.Add(System.Convert.ToInt32(OrderRow["ID"]));
            }
            finally
            {
                try
                {
                    // Clean up
                    OrdersDataAdapter.Dispose();
                    SQLConn.Dispose();
                }
                catch (SqlException SE)
                {
                    // Log an event in the Application Event Log.
                    throw;
                }
            }

            return JobOrderIDsList;
        }



        public async Task<JobOrder> UpdateMachineTranslationTemplateDetails(int jobOrderId, int preTranslateFromTemplateId, int saveTranslationsToTemplateId, byte MTEngine)
        {
            var joborder = await GetById(jobOrderId);

            // maybe add security check later here and autoMapper
            joborder.PreTranslateFromTemplateId = preTranslateFromTemplateId;
            joborder.SaveTranslationsToTemplateId = (saveTranslationsToTemplateId == 0 ? null : saveTranslationsToTemplateId);
            joborder.IsMachineTranslationJobFromiPlus = true;
            joborder.MachineTranslationEngineSelected = MTEngine;

            orderRepository.Update(joborder);
            await orderRepository.SaveChangesAsync();

            return joborder;
        }

        public async Task<List<DeadlineChangeReason>> GetAllDeadlineChangeReason()
        {
            var results = await deadlineChangeReasonRepo.All().Where(d => d.DeletedDateTime == null).ToListAsync();
            return results;
        }
    }
}
