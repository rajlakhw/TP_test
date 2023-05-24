using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

#nullable disable

namespace Data.QA
{
    public partial class TPCoreQAandCustomerWSTestingNEWContext : IdentityDbContext<ExtranetUserTemp>
    {
        public TPCoreQAandCustomerWSTestingNEWContext()
        {
        }

        public TPCoreQAandCustomerWSTestingNEWContext(DbContextOptions<TPCoreQAandCustomerWSTestingNEWContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessControl> AccessControls { get; set; }
        public virtual DbSet<AltairCorporateGroupe> AltairCorporateGroupes { get; set; }
        public virtual DbSet<AltairRegion> AltairRegions { get; set; }
        public virtual DbSet<ApprovedOrBlockedLinguisticSupplier> ApprovedOrBlockedLinguisticSuppliers { get; set; }
        public virtual DbSet<AutoClientInvoicingSetting> AutoClientInvoicingSettings { get; set; }
        public virtual DbSet<BankHoliday> BankHolidays { get; set; }
        public virtual DbSet<Bestseller2015DetailsTemp> Bestseller2015DetailsTemps { get; set; }
        public virtual DbSet<Bestseller2017DetailsTemp> Bestseller2017DetailsTemps { get; set; }
        public virtual DbSet<BestsellerDetailsTemp> BestsellerDetailsTemps { get; set; }
        public virtual DbSet<BetssonTranslationFlowFile> BetssonTranslationFlowFiles { get; set; }
        public virtual DbSet<BetssonTranslationFlowRequest> BetssonTranslationFlowRequests { get; set; }
        public virtual DbSet<BetssonTranslationFlowSuggestion> BetssonTranslationFlowSuggestions { get; set; }
        public virtual DbSet<BetssonTranslationFlowText> BetssonTranslationFlowTexts { get; set; }
        public virtual DbSet<BirthdayComment> BirthdayComments { get; set; }
        public virtual DbSet<BirthdayLike> BirthdayLikes { get; set; }
        public virtual DbSet<BirthdayWish> BirthdayWishes { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<CallDataRecord> CallDataRecords { get; set; }
        public virtual DbSet<ChatLog> ChatLogs { get; set; }
        public virtual DbSet<ChatSession> ChatSessions { get; set; }
        public virtual DbSet<ClientDecisionReason> ClientDecisionReasons { get; set; }
        public virtual DbSet<ClientDesignFile> ClientDesignFiles { get; set; }
        public virtual DbSet<ClientInvoice> ClientInvoices { get; set; }
        public virtual DbSet<ClientPriceList> ClientPriceLists { get; set; }
        public virtual DbSet<ClientPriceListRate> ClientPriceListRates { get; set; }
        public virtual DbSet<ClientPriceListsHistoricalDatum> ClientPriceListsHistoricalData { get; set; }
        public virtual DbSet<ClientSpecificChecklist> ClientSpecificChecklists { get; set; }
        public virtual DbSet<ClientTechnology> ClientTechnologies { get; set; }
        public virtual DbSet<Computer> Computers { get; set; }
        public virtual DbSet<Conference> Conferences { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CustomFiltersForOrderStatusPage> CustomFiltersForOrderStatusPages { get; set; }
        public virtual DbSet<DataObjectType> DataObjectTypes { get; set; }
        public virtual DbSet<DeadlineChangeReason> DeadlineChangeReasons { get; set; }
        public virtual DbSet<DecisionReason> DecisionReasons { get; set; }
        public virtual DbSet<DesignPlusComment> DesignPlusComments { get; set; }
        public virtual DbSet<DesignPlusDocumentLayer> DesignPlusDocumentLayers { get; set; }
        public virtual DbSet<DesignPlusFileDownload> DesignPlusFileDownloads { get; set; }
        public virtual DbSet<DesignPlusFolder> DesignPlusFolders { get; set; }
        public virtual DbSet<DesignPlusLinksPath> DesignPlusLinksPaths { get; set; }
        public virtual DbSet<DesignPlusPortCount> DesignPlusPortCounts { get; set; }
        public virtual DbSet<DesignPlusProject> DesignPlusProjects { get; set; }
        public virtual DbSet<DesignPlusReviewJob> DesignPlusReviewJobs { get; set; }
        public virtual DbSet<DesignPlusStickyNote> DesignPlusStickyNotes { get; set; }
        public virtual DbSet<DesignPlusTag> DesignPlusTags { get; set; }
        public virtual DbSet<DesignPlusTextChange> DesignPlusTextChanges { get; set; }
        public virtual DbSet<DesignPlusTextFrameLog> DesignPlusTextFrameLogs { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeAccessMatrix> EmployeeAccessMatrices { get; set; }
        public virtual DbSet<EmployeeAccessMatrixControl> EmployeeAccessMatricesControls { get; set; }
        public virtual DbSet<EmployeeAnnualRevenueTargetsAndThreshold> EmployeeAnnualRevenueTargetsAndThresholds { get; set; }
        public virtual DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public virtual DbSet<EmployeeExpenseClaim> EmployeeExpenseClaims { get; set; }
        public virtual DbSet<EmployeeExpenseClaimsMaster> EmployeeExpenseClaimsMasters { get; set; }
        public virtual DbSet<EmployeeHoliday> EmployeeHolidays { get; set; }
        public virtual DbSet<EmployeeHolidayRequest> EmployeeHolidayRequests { get; set; }
        public virtual DbSet<EmployeeInterimTarget> EmployeeInterimTargets { get; set; }
        public virtual DbSet<EmployeeOffice> EmployeeOffices { get; set; }
        public virtual DbSet<EmployeeOwnershipRelationship> EmployeeOwnershipRelationships { get; set; }
        public virtual DbSet<EmployeeOwnershipType> EmployeeOwnershipTypes { get; set; }
        public virtual DbSet<EmployeeStatusType> EmployeeStatusTypes { get; set; }
        public virtual DbSet<EmployeeTeam> EmployeeTeams { get; set; }
        public virtual DbSet<EmployeeTimeLog> EmployeeTimeLogs { get; set; }
        public virtual DbSet<EmployeesSickness> EmployeesSicknesses { get; set; }
        public virtual DbSet<EndClient> EndClients { get; set; }
        public virtual DbSet<EndClientDatum> EndClientData { get; set; }
        public virtual DbSet<Enquiry> Enquiries { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public virtual DbSet<ExpenseCategoryLimit> ExpenseCategoryLimits { get; set; }
        public virtual DbSet<ExtraWorkingDay> ExtraWorkingDays { get; set; }
        public virtual DbSet<ExtranetAccessLevels> ExtranetAccessLevels { get; set; }
        public virtual DbSet<ExtranetAvailableAccessLevel> ExtranetAvailableAccessLevels { get; set; }
        public virtual DbSet<ExtranetSiteMap> ExtranetSiteMaps { get; set; }
        public virtual DbSet<ExtranetSiteMapClientSpecific> ExtranetSiteMapClientSpecifics { get; set; }
        public virtual DbSet<ExtranetSiteMapForBernafon> ExtranetSiteMapForBernafons { get; set; }
        public virtual DbSet<ExtranetSiteMapForBostik> ExtranetSiteMapForBostiks { get; set; }
        public virtual DbSet<ExtranetSiteMapForEon> ExtranetSiteMapForEons { get; set; }
        public virtual DbSet<ExtranetSiteMapForFlsmidth> ExtranetSiteMapForFlsmidths { get; set; }
        public virtual DbSet<ExtranetSiteMapForRb> ExtranetSiteMapForRbs { get; set; }
        public virtual DbSet<ExtranetSiteMapforImc> ExtranetSiteMapforImcs { get; set; }
        public virtual DbSet<ExtranetUser> ExtranetUsers { get; set; }
        public virtual DbSet<ExtranetUsersPassword> ExtranetUsersPasswords { get; set; }
        public virtual DbSet<ExtranetUsersPasswordsLog> ExtranetUsersPasswordsLogs { get; set; }
        public virtual DbSet<ExtranetUsersReviewLanguage> ExtranetUsersReviewLanguages { get; set; }
        public virtual DbSet<ExtranetUsersSecurityQuestion> ExtranetUsersSecurityQuestions { get; set; }
        public virtual DbSet<FileExtensionsForAutomation> FileExtensionsForAutomations { get; set; }
        public virtual DbSet<Ftpdetail> Ftpdetails { get; set; }
        public virtual DbSet<GroupeLanguageServiceRateCard> GroupeLanguageServiceRateCards { get; set; }
        public virtual DbSet<GroupeMasterRate> GroupeMasterRates { get; set; }
        public virtual DbSet<GroupeRateCard> GroupeRateCards { get; set; }
        public virtual DbSet<HighFife> HighFives { get; set; }
        public virtual DbSet<InRiverFieldsBeingTranslated> InRiverFieldsBeingTranslateds { get; set; }
        public virtual DbSet<JabraDetailsTemp> JabraDetailsTemps { get; set; }
        public virtual DbSet<JobItem> JobItems { get; set; }
        public virtual DbSet<JobItemPgddetail> JobItemPgddetails { get; set; }
        public virtual DbSet<JobOrder> JobOrders { get; set; }
        public virtual DbSet<JobOrderAutomationDatum> JobOrderAutomationData { get; set; }
        public virtual DbSet<JobOrderAutomationSetting> JobOrderAutomationSettings { get; set; }
        public virtual DbSet<JobOrderChannel> JobOrderChannels { get; set; }
        public virtual DbSet<JobOrderPgddetail> JobOrderPgddetails { get; set; }
        public virtual DbSet<JobSubmissionTemplate> JobSubmissionTemplates { get; set; }
        public virtual DbSet<JsonXmlnodeMapping> JsonXmlnodeMappings { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LanguageCodesToUseInStudio> LanguageCodesToUseInStudios { get; set; }
        public virtual DbSet<LanguageCombinationsForTop5Supplier> LanguageCombinationsForTop5Suppliers { get; set; }
        public virtual DbSet<LanguageRateUnit> LanguageRateUnits { get; set; }
        public virtual DbSet<LanguageRegion> LanguageRegions { get; set; }
        public virtual DbSet<LanguageService> LanguageServices { get; set; }
        public virtual DbSet<LanguageSubjectArea> LanguageSubjectAreas { get; set; }
        public virtual DbSet<LikedEntity> LikedEntities { get; set; }
        public virtual DbSet<LinguisticSupplier> LinguisticSuppliers { get; set; }
        public virtual DbSet<LinguisticSupplierInvoice> LinguisticSupplierInvoices { get; set; }
        public virtual DbSet<LinguisticSupplierInvoiceEarlyPaymentOption> LinguisticSupplierInvoiceEarlyPaymentOptions { get; set; }
        public virtual DbSet<LinguisticSupplierInvoiceJobItem> LinguisticSupplierInvoiceJobItems { get; set; }
        public virtual DbSet<LinguisticSupplierInvoiceTemplate> LinguisticSupplierInvoiceTemplates { get; set; }
        public virtual DbSet<LinguisticSupplierRate> LinguisticSupplierRates { get; set; }
        public virtual DbSet<LinguisticSupplierSoftwareApplication> LinguisticSupplierSoftwareApplications { get; set; }
        public virtual DbSet<LinguisticSupplierSource> LinguisticSupplierSources { get; set; }
        public virtual DbSet<LinguisticSupplierStatus> LinguisticSupplierStatuses { get; set; }
        public virtual DbSet<LinguisticSupplierType> LinguisticSupplierTypes { get; set; }
        public virtual DbSet<LinguistsAltairDetail> LinguistsAltairDetails { get; set; }
        public virtual DbSet<LocalCountryInfo> LocalCountryInfos { get; set; }
        public virtual DbSet<LocalCurrencyInfo> LocalCurrencyInfos { get; set; }
        public virtual DbSet<LocalDiscountAndSurchargeInfo> LocalDiscountAndSurchargeInfos { get; set; }
        public virtual DbSet<LocalExtranetAccessLevelInfo> LocalExtranetAccessLevelInfos { get; set; }
        public virtual DbSet<LocalJobOrderChannelInfo> LocalJobOrderChannelInfos { get; set; }
        public virtual DbSet<LocalLanguageInfo> LocalLanguageInfos { get; set; }
        public virtual DbSet<LocalLanguageRateUnitInfo> LocalLanguageRateUnitInfos { get; set; }
        public virtual DbSet<LocalLanguageServiceInfo> LocalLanguageServiceInfos { get; set; }
        public virtual DbSet<LocalPlanningCalendarCategoriesInfo> LocalPlanningCalendarCategoriesInfos { get; set; }
        public virtual DbSet<MarketingCampaign> MarketingCampaigns { get; set; }
        public virtual DbSet<MarketingCampaignRecipient> MarketingCampaignRecipients { get; set; }
        public virtual DbSet<MarketingCampaignRecipientsNew> MarketingCampaignRecipientsNews { get; set; }
        public virtual DbSet<MiscResource> MiscResources { get; set; }
        public virtual DbSet<NotificationRecipients> NotificationRecipients { get; set; }
        public virtual DbSet<NotificationTypes> NotificationTypes { get; set; }
        public virtual DbSet<OperatingSystem> OperatingSystems { get; set; }
        public virtual DbSet<OperatingSystemVersion> OperatingSystemVersions { get; set; }
        public virtual DbSet<OrderAndItemAuditLog> OrderAndItemAuditLogs { get; set; }
        public virtual DbSet<Org> Orgs { get; set; }
        public virtual DbSet<OrgAltairDetail> OrgAltairDetails { get; set; }
        public virtual DbSet<OrgGroup> OrgGroups { get; set; }
        public virtual DbSet<OrgIndustry> OrgIndustries { get; set; }
        public virtual DbSet<OrgIndustryRelationship> OrgIndustryRelationships { get; set; }
        public virtual DbSet<OrgIntroductionSource> OrgIntroductionSources { get; set; }
        public virtual DbSet<OrgLegalStatus> OrgLegalStatuses { get; set; }
        public virtual DbSet<OrgMainIndustry> OrgMainIndustries { get; set; }
        public virtual DbSet<OrgReminderSetting> OrgReminderSettings { get; set; }
        public virtual DbSet<OrgSalesCategory> OrgSalesCategories { get; set; }
        public virtual DbSet<OrgTechnologyRelationship> OrgTechnologyRelationships { get; set; }
        public virtual DbSet<OverdueReason> OverdueReasons { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<PerformancePlusManualFigure> PerformancePlusManualFigures { get; set; }
        public virtual DbSet<PerformancePlusTarget> PerformancePlusTargets { get; set; }
        public virtual DbSet<PlanningCalendarAppointment> PlanningCalendarAppointments { get; set; }
        public virtual DbSet<PlanningCalendarCategory> PlanningCalendarCategories { get; set; }
        public virtual DbSet<PreferredLanguage> PreferredLanguages { get; set; }
        public virtual DbSet<PriceListAutomationManagementAudit> PriceListAutomationManagementAudits { get; set; }
        public virtual DbSet<PriceListsAuditLog> PriceListsAuditLogs { get; set; }
        public virtual DbSet<Quote> Quotes { get; set; }
        public virtual DbSet<QuoteAndOrderDiscountsAndSurcharge> QuoteAndOrderDiscountsAndSurcharges { get; set; }
        public virtual DbSet<QuoteAndOrderDiscountsAndSurchargesCategory> QuoteAndOrderDiscountsAndSurchargesCategories { get; set; }
        public virtual DbSet<QuoteItem> QuoteItems { get; set; }
        public virtual DbSet<QuoteTemplate> QuoteTemplates { get; set; }
        public virtual DbSet<ReviewContextField> ReviewContextFields { get; set; }
        public virtual DbSet<ReviewContextFieldsArchived> ReviewContextFieldsArchiveds { get; set; }
        public virtual DbSet<ReviewContextFieldsTransit> ReviewContextFieldsTransits { get; set; }
        public virtual DbSet<ReviewGrade> ReviewGrades { get; set; }
        public virtual DbSet<ReviewPlusSignOffJobItem> ReviewPlusSignOffJobItems { get; set; }
        public virtual DbSet<ReviewPlusTag> ReviewPlusTags { get; set; }
        public virtual DbSet<ReviewPlusTagsArchived> ReviewPlusTagsArchiveds { get; set; }
        public virtual DbSet<ReviewPlusTagsTransit> ReviewPlusTagsTransits { get; set; }
        public virtual DbSet<ReviewQuestion> ReviewQuestions { get; set; }
        public virtual DbSet<ReviewTranslation> ReviewTranslations { get; set; }
        public virtual DbSet<ReviewTranslationComment> ReviewTranslationComments { get; set; }
        public virtual DbSet<ReviewTranslationsArchived> ReviewTranslationsArchiveds { get; set; }
        public virtual DbSet<ReviewTranslationsCopy> ReviewTranslationsCopies { get; set; }
        public virtual DbSet<ReviewTranslationsTransit> ReviewTranslationsTransits { get; set; }
        public virtual DbSet<SalesHotlistEntry> SalesHotlistEntries { get; set; }
        public virtual DbSet<SharePlusArticle> SharePlusArticles { get; set; }
        public virtual DbSet<SharePlusArticleViewLog> SharePlusArticleViewLogs { get; set; }
        public virtual DbSet<SharePlusSearchLog> SharePlusSearchLogs { get; set; }
        public virtual DbSet<SharedDoc> SharedDocs { get; set; }
        public virtual DbSet<SoftwareApplication> SoftwareApplications { get; set; }
        public virtual DbSet<StyleGuideRule> StyleGuideRules { get; set; }
        public virtual DbSet<SystemConfig> SystemConfigs { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskType> TaskTypes { get; set; }
        public virtual DbSet<Timesheet> Timesheets { get; set; }
        public virtual DbSet<TimesheetLogBreakdown> TimesheetLogBreakdowns { get; set; }
        public virtual DbSet<TimesheetTaskCategory> TimesheetTaskCategories { get; set; }
        public virtual DbSet<TradosTemplate> TradosTemplates { get; set; }
        public virtual DbSet<TradosTemplateToTmmapping> TradosTemplateToTmmappings { get; set; }
        public virtual DbSet<TrainingCourse> TrainingCourses { get; set; }
        public virtual DbSet<TrainingCourseType> TrainingCourseTypes { get; set; }
        public virtual DbSet<TrainingSession> TrainingSessions { get; set; }
        public virtual DbSet<TrainingSessionAttendee> TrainingSessionAttendees { get; set; }
        public virtual DbSet<ViewContactsSearchableInfo> ViewContactsSearchableInfos { get; set; }
        public virtual DbSet<ViewContactsWhichHaveBeenContacted> ViewContactsWhichHaveBeenContacteds { get; set; }
        public virtual DbSet<ViewCountriesMultilingualInfo> ViewCountriesMultilingualInfos { get; set; }
        public virtual DbSet<ViewCurrenciesMultilingualInfo> ViewCurrenciesMultilingualInfos { get; set; }
        public virtual DbSet<ViewLanguagesMultilingualInfo> ViewLanguagesMultilingualInfos { get; set; }
        public virtual DbSet<ViewLinguisticSuppliersSearchableInfo> ViewLinguisticSuppliersSearchableInfos { get; set; }
        public virtual DbSet<ViewMarketingEmailAddressesOfAllClientContact> ViewMarketingEmailAddressesOfAllClientContacts { get; set; }
        public virtual DbSet<ViewMarketingEmailAddressesOfAllNonClientContact> ViewMarketingEmailAddressesOfAllNonClientContacts { get; set; }
        public virtual DbSet<ViewOrgsSearchableInfo> ViewOrgsSearchableInfos { get; set; }
        public virtual DbSet<ViewOrgsWhichHaveBeenContacted> ViewOrgsWhichHaveBeenContacteds { get; set; }
        public virtual DbSet<Vodetail> Vodetails { get; set; }
        public virtual DbSet<VodropdownItem> VodropdownItems { get; set; }
        public virtual DbSet<VodropdownItemsOld> VodropdownItemsOlds { get; set; }
        public virtual DbSet<VodropdownList> VodropdownLists { get; set; }
        public virtual DbSet<VolumeDiscount> VolumeDiscounts { get; set; }
        public virtual DbSet<WorkAnnivarsariesComment> WorkAnnivarsariesComments { get; set; }
        public virtual DbSet<WorkAnniversariesLike> WorkAnniversariesLikes { get; set; }
        public virtual DbSet<WorkAnniversariesWish> WorkAnniversariesWishes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=FREDCPDBSSC0016\\TPQA, 1621;Initial Catalog=TPCoreQAandCustomerWSTestingNEW;Integrated Security=True;Connect Timeout=30;Min Pool Size=5;Max Pool Size=32767;Pooling='true';Database=TPCoreQAandCustomerWSTestingNEW");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            //modelBuilder.Entity<ExtranetUserTemp>().ToTable("ExtranetUsersTemp").Property(p => p.UserName).HasColumnName("UserName");
            modelBuilder.Entity<ExtranetUserTemp>().ToTable("ExtranetUsersTemp").Property(p => p.PasswordHash).HasColumnName("HashedPassword");
            //modelBuilder.Entity<ExtranetUserTemp>().ToTable("ExtranetUsersTemp").Property(p => p.Id).HasColumnName("WebServiceGUID");
            //modelBuilder.Entity<ExtranetUserTemp>().ToTable("ExtranetUsersTemp").Property(p => p.PasswordHash).HasColumnName("HashedPassword");

            modelBuilder.Entity<ExtranetUserTemp>().Ignore(c => c.AccessFailedCount)
                                               .Ignore(c => c.Email).Ignore(c => c.NormalizedEmail).Ignore(c => c.EmailConfirmed)
                                               .Ignore(c => c.PhoneNumber).Ignore(c => c.PhoneNumberConfirmed).Ignore(c => c.TwoFactorEnabled)
                                               .Ignore(c => c.LockoutEnabled).Ignore(c => c.LockoutEnd);

            //modelBuilder.Entity<ExtranetUserTemp>().ToTable("ExtranetUsersTemp").Property(p => p.NormalizedUserName).HasColumnName("UserName");

            //modelBuilder.Entity<ApplicationUser>().ToTable("MyUsers").Property(p => p.Id).HasColumnName("UserId");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("MyUserRoles");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("MyUserLogins");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("MyUserClaims");
            //modelBuilder.Entity<IdentityRole>().ToTable("MyRoles");

            modelBuilder.Entity<AccessControl>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AltairCorporateGroupe>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AltairCorporateGroupe");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<AltairRegion>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RegionCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RegionName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<ApprovedOrBlockedLinguisticSupplier>(entity =>
            {
                entity.HasComment("Contains information on translators/other linguistic suppliers who have been approved (by us or directly by the client) to work for particular contacts/orgs/org groups, or who have been blocked for particular contacts/org/org groups (e.g. if we want tokeep using them for most clients, one client says they don't want to use that translator)");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppliesToDataObjectId)
                    .HasColumnName("AppliesToDataObjectID")
                    .HasComment("The ID of the contact/org/org group");

                entity.Property(e => e.AppliesToDataObjectTypeId)
                    .HasColumnName("AppliesToDataObjectTypeID")
                    .HasComment("The type of data object (contact/org/org group) from DataObjectTypes: we might have a translator who is approved for all organisations within a group - or only for one org - or potentially only for one individual contact's work (e.g. if they are in the legal division but another contact is in the marketing division and they want different translators)");

                entity.Property(e => e.DefaultSoftwareApplicationId)
                    .HasColumnName("DefaultSoftwareApplicationID")
                    .HasComment("For some process automation (e.g. Kambi), if we know a translator is using Wordfast, we need to automatically \"segment unknowns\" to make a TTX file compatible. We might want to apply other rules for other applications. If NULL (as in most cases) then this has no effect.");

                entity.Property(e => e.LanguageServiceId)
                    .HasColumnName("LanguageServiceID")
                    .HasComment("The ID of the language service for which they are approved/blocked, e.g. we might have a translator approved for En-Fr and a proofreader approved for that same language combination");

                entity.Property(e => e.LinguisticSupplierId)
                    .HasColumnName("LinguisticSupplierID")
                    .HasComment("The ID of the supplier (from LinguisticSuppliers)");

                entity.Property(e => e.Notes).HasComment("Optional free-text notes (e.g. for a project manager to see any specific arrangements we've made for this translator with this client)");

                entity.Property(e => e.SourceLangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SourceLangIANACode")
                    .HasComment("The source language of the combination for which they are approved/blocked");

                entity.Property(e => e.Status).HasComment("0 = blocked; 1 = approved (by us); 2 = approved (directly by the client); 3 = temporarily unavailable: until we have a full \"priority of multiple suppliers\" (if that's ever needed), this just temporarily deactivates a normally-approved supplier, e.g. if just for 1 week while they're on holiday we want a different approved supplier to receive jobs");

                entity.Property(e => e.TargetLangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("TargetLangIANACode")
                    .HasComment("The target language of the combination for which they are approved/blocked");

                entity.Property(e => e.WorkingPatternId)
                    .HasColumnName("WorkingPatternID")
                    .HasComment("An indication of when they work/when they should be used, e.g. for auto-assignment of incoming jobs. 0 = any time; 1 = working days only (Mon-Fri excluding days when the system is in \"holiday mode\"); 2 = weekends and \"holiday mode\" days only; 3 = weekends and \"holiday mode\" days only plus Friday afternoons: send stuff out on a Friday afternoon if it would mean they will be working on it over the weekend. Currently hard-coded but we may well want to link this to a table for more flexibility.");
            });

            modelBuilder.Entity<AutoClientInvoicingSetting>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceByPonumber).HasColumnName("InvoiceByPONumber");

                entity.Property(e => e.InvoiceDescription)
                    .IsRequired()
                    .HasComment("Mandatory free text field for summary info about the invoice, e.g. describing in more detail about the jobs being invoiced, how prices were calculated, etc.");

                entity.Property(e => e.InvoiceLangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("InvoiceLangIANAcode")
                    .HasComment("The language used for the invoice (used when auto-generating invoice file, etc.)");

                entity.Property(e => e.InvoiceTitle)
                    .HasMaxLength(300)
                    .HasComment("The title/name of this invoice, used as a main heading, e.g. \"Invoice for jobs completed during September 2009\", or \"Invoice for translation of XYZ Manual\". This is optional since often the title of just the invoice number will be sufficient.");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ShowContactNamesInBreakdown).HasComment("Whether the invoice will show the client contact name against each job order in the breakdown section. If all jobs are for the same contact (or if only 1 job is being invoiced), this would probably be set to False; whereas for multiple jobs being invoiced to an org, this would probably be set to True.");

                entity.Property(e => e.ShowCustomerSpecificField1ValueInBreakdown).HasComment("Whether the invoice will show the customer-specific field 1 in the breakdown (the usage/meaning of this field depends on the client)");

                entity.Property(e => e.ShowInterpretingDurationInBreakdown).HasComment("Whether the invoice will show the number of hours/minutes of interpreting time in the jobs breakdown section");

                entity.Property(e => e.ShowJobItemsInBreakdown).HasComment("Whether the invoice will show a full breakdown of all individual job items, rather than just the high-level summmary of orders. Typically I expect that where an invoice is for a single order, we might want to display this level of detail (so the client can see the costs per target language, for example), whereas for batch invoices where we are e.g. invoicing a month's worth of work, this might be set to False to keep the level of information down. But of course it's up to the user.");

                entity.Property(e => e.ShowNotesInBreakdown).HasComment("Whether the client-facing notes should be shown against each job order in the breakdown of what is being invoiced. (For some clients, key information such as their project names may be stored in the Notes field.)");

                entity.Property(e => e.ShowOrderPonumbersInBreakdown)
                    .HasColumnName("ShowOrderPONumbersInBreakdown")
                    .HasComment("Whether the client's PO numbers should be shown against each job order in the breakdown of what is being invoiced. This will normally be False because most clients do not use PO numbers; if they do, it could be set to True if they give us a different PO number for every job, or it could be False if they use a single blanket PO for all work, in which case just one PO number at the top of the invoice would be used, via the ClientPONumber column");

                entity.Property(e => e.ShowPagesOrSlidesInBreakdown).HasComment("Whether the invoice will show the number of pages (for DTP) or slides (for presentations) in the jobs breakdown section");

                entity.Property(e => e.ShowSourceLangsInBreakdown).HasComment("Whether the source language should be shown against each job order in the breakdown of what is being invoiced. If only orders are being shown in the breakdown, i.e. if ShowJobItemsInBreakdown is False, then this will result in the display either of a single source language (if there is only one), or a composite string along the lines of \"x langauges\"");

                entity.Property(e => e.ShowTargetLangsInBreakdown).HasComment("Whether the target language(s) should be shown against each job order in the breakdown of what is being invoiced. If only orders are being shown in the breakdown, i.e. if ShowJobItemsInBreakdown is False, then this will result in the display either of a single target language (if there is only one), or a composite string along the lines of \"x langauges\"");

                entity.Property(e => e.ShowWorkDurationInBreakdown).HasComment("Whether the invoice will show the number of hours/minutes of work\r\n(e.g. for proofreading/software testing/etc.) in the jobs breakdown section");

                entity.Property(e => e.WordCountPresentationOption).HasComment("Category determining how much detail to show for word counts presented in the job items breakdown for this invoice. Currently\r\n        DoNotShow = 0\r\n        ShowTotalsOnly = 1\r\n        ShowMemoryBreakdownStandard = 2\r\n        ShowMemoryBreakdownIncludingPerfectMatch = 3");
            });

            modelBuilder.Entity<BankHoliday>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BankHolidayDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.IsAukbankHoliday).HasColumnName("IsAUKBankHoliday");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Bestseller2015DetailsTemp>(entity =>
            {
                entity.ToTable("Bestseller2015DetailsTemp");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateTimeGenerated)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ItemAttributes).IsRequired();

                entity.Property(e => e.ItemText).IsRequired();

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.SourceLanguage)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.StyleId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("StyleID");

                entity.Property(e => e.TargetLanguage).IsRequired();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Bestseller2017DetailsTemp>(entity =>
            {
                entity.ToTable("Bestseller2017DetailsTemp");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateTimeGenerated)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ItemAttributes).IsRequired();

                entity.Property(e => e.ItemText).IsRequired();

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.SourceLanguage)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.StyleId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("StyleID");

                entity.Property(e => e.TargetLanguage).IsRequired();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<BestsellerDetailsTemp>(entity =>
            {
                entity.ToTable("BestsellerDetailsTemp");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateTimeGenerated)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ItemAttributes).IsRequired();

                entity.Property(e => e.ItemText).IsRequired();

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.JobOrderId).HasColumnName("JobOrderID");

                entity.Property(e => e.SourceLanguage)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.StyleId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("StyleID");

                entity.Property(e => e.TargetLanguage)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<BetssonTranslationFlowFile>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TfazureBlobName)
                    .IsRequired()
                    .HasColumnName("TFAzureBlobName");

                entity.Property(e => e.TffileId).HasColumnName("TFFileID");

                entity.Property(e => e.TffileName)
                    .IsRequired()
                    .HasColumnName("TFFileName");

                entity.Property(e => e.TffileType)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("TFFileType");

                entity.Property(e => e.TptextId).HasColumnName("TPTextID");
            });

            modelBuilder.Entity<BetssonTranslationFlowRequest>(entity =>
            {
                entity.ToTable("BetssonTranslationFlowRequest");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BetssonUniquePublicId)
                    .IsRequired()
                    .HasColumnName("BetssonUniquePublicID");

                entity.Property(e => e.DeadlineByClient).HasColumnType("datetime");

                entity.Property(e => e.TargetLanguage).IsRequired();

                entity.Property(e => e.TpjobOrderId).HasColumnName("TPJobOrderID");

                entity.Property(e => e.TranslationFlowRequestId).HasColumnName("TranslationFlowRequestID");

                entity.Property(e => e.Urlstring)
                    .IsRequired()
                    .HasColumnName("URLString");
            });

            modelBuilder.Entity<BetssonTranslationFlowSuggestion>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TptextId).HasColumnName("TPtextID");
            });

            modelBuilder.Entity<BetssonTranslationFlowText>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CommentCheckedByEmployeeId).HasColumnName("CommentCheckedByEmployeeID");

                entity.Property(e => e.CommentCheckedDateTime).HasColumnType("datetime");

                entity.Property(e => e.TextId).HasColumnName("TextID");

                entity.Property(e => e.TprequestId).HasColumnName("TPRequestID");
            });

            modelBuilder.Entity<BirthdayComment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BirthdayWishId).HasColumnName("BirthdayWishID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.BirthdayWish)
                    .WithMany(p => p.BirthdayComments)
                    .HasForeignKey(d => d.BirthdayWishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BirthdayComments_BirthdayWishes");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.BirthdayComments)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntityComments_Employees");
            });

            modelBuilder.Entity<BirthdayLike>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BirthdayWishId).HasColumnName("BirthdayWishID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.HasOne(d => d.BirthdayWish)
                    .WithMany(p => p.BirthdayLikes)
                    .HasForeignKey(d => d.BirthdayWishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BirthdayLikes_BirthdayWishes");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.BirthdayLikes)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BirthdayLikes_Employees");
            });

            modelBuilder.Entity<BirthdayWish>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.BirthdayWishes)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BirthdayWishes_Employees");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ApplicationName).HasMaxLength(50);

                entity.Property(e => e.CompanyNameToShow).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DomainName).HasMaxLength(200);

                entity.Property(e => e.EmailAddress).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CallDataRecord>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnswerDateTimeCall).HasColumnType("datetime");

                entity.Property(e => e.CallType).HasMaxLength(5);

                entity.Property(e => e.CalledNumber)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.CalledNumberContactIdatTimeOfCall).HasColumnName("CalledNumberContactIDAtTimeOfCall");

                entity.Property(e => e.CallingNumber)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.EmployeeIdatTimeOfCall).HasColumnName("EmployeeIDAtTimeOfCall");

                entity.Property(e => e.EndDateTimeOfCall).HasColumnType("datetime");

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.StartDateTimeOfCall).HasColumnType("datetime");
            });

            modelBuilder.Entity<ChatLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ChatLog");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.SentTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<ChatSession>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ChatSession");

                entity.Property(e => e.ChatSessionId).HasColumnName("ChatSessionID");
            });

            modelBuilder.Entity<ClientDecisionReason>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DecisionReasonId).HasColumnName("DecisionReasonID");

                entity.Property(e => e.Ianacode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("IANACode");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<ClientDesignFile>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CommentUpdatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CompletedByClientId).HasColumnName("CompletedByClientID");

                entity.Property(e => e.CompletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByClientId).HasColumnName("DeletedByClientID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DocumentFileType)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.IdsdelPortNumber).HasColumnName("IDSDelPortNumber");

                entity.Property(e => e.IdsportNumber).HasColumnName("IDSPortNumber");

                entity.Property(e => e.IndesignDocumentId)
                    .HasColumnName("IndesignDocumentID")
                    .HasComment("Document ID that is returned from Indesign server, this could change everytime the document is closed and then opened again in design plus.");

                entity.Property(e => e.IndesignReviewDocumentId).HasColumnName("IndesignReviewDocumentID");

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.JobOrderId).HasColumnName("JobOrderID");

                entity.Property(e => e.LastModifiedByClientId).HasColumnName("LastModifiedByClientID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ParentDesignPlusId).HasColumnName("ParentDesignPlusID");

                entity.Property(e => e.ReviewDocIdsportNumber).HasColumnName("ReviewDocIDSPortNumber");

                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");

                entity.Property(e => e.RvIdspdelPortNumber).HasColumnName("RvIDSPDelPortNumber");

                entity.Property(e => e.SentForReviewDateTime).HasColumnType("datetime");

                entity.Property(e => e.SourceLangCode).HasMaxLength(10);

                entity.Property(e => e.TargetLangCode).HasMaxLength(10);

                entity.Property(e => e.TmalignedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("TMAlignedDateTime");

                entity.Property(e => e.UploadedByClientId).HasColumnName("UploadedByClientID");

                entity.Property(e => e.UploadedDateTime).HasColumnType("datetime");

                entity.Property(e => e.WorkingCopyDocumentId).HasColumnName("WorkingCopyDocumentID");

                entity.Property(e => e.WorkingCopyReviewDocumentId).HasColumnName("WorkingCopyReviewDocumentID");
            });

            modelBuilder.Entity<ClientInvoice>(entity =>
            {
                entity.HasComment("Invoices we have issued to clients for work completed");

                entity.HasIndex(e => new { e.Id, e.InvoiceDate, e.PaymentTermDays, e.PaidDate }, "_dta_index_ClientInvoices_12_1000390633__K1_K4_K30_K32");

                entity.HasIndex(e => new { e.PaidDate, e.Id, e.DeletedDateTime }, "_dta_index_ClientInvoices_12_1000390633__K32_K1_K39");

                entity.HasIndex(e => new { e.PaidDate, e.IsFinalised, e.DeletedDateTime, e.Id, e.InvoiceDate }, "_dta_index_ClientInvoices_12_1000390633__K32_K5_K39_K1_K4");

                entity.HasIndex(e => new { e.IsFinalised, e.Id }, "_dta_index_ClientInvoices_12_1000390633__K5_K1");

                entity.HasIndex(e => new { e.IsFinalised, e.InvoiceDate, e.DeletedDateTime, e.Id }, "_dta_index_ClientInvoices_12_1000390633__K5_K4_K39_K1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnticipatedFinalValueAmountExcludingVat)
                    .HasColumnType("decimal(38, 18)")
                    .HasColumnName("AnticipatedFinalValueAmountExcludingVAT")
                    .HasComputedColumnSql("([dbo].[funcGetAnticipatedFinalValueAmountExcludingVATForInvoiceID]([ID]))", false);

                entity.Property(e => e.AnticipatedFinalVatamount)
                    .HasColumnType("decimal(38, 18)")
                    .HasColumnName("AnticipatedFinalVATAmount")
                    .HasComputedColumnSql("([dbo].[funcGetAnticipatedFinalVATValueAmountForInvoiceID]([ID]))", false);

                entity.Property(e => e.AutoChaseWhenOverdue).HasComment("Whether the system should automatically send out e-mail reminders to the invoicing contact when the invoice is overdue. Set initially by the equivalent default setting against the invoicing org, but can subsequently be changed on a per-invoice basis.");

                entity.Property(e => e.ClientPonumber)
                    .HasMaxLength(100)
                    .HasColumnName("ClientPONumber")
                    .HasComment("Optional purchase order number (some clients will require this to appear on their invoices)");

                entity.Property(e => e.ContactId)
                    .HasColumnName("ContactID")
                    .HasComment("The ID of the contact (from Contacts) to whom we issued/are issuing the invoice.");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The employee who created the invoice");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when creation of the invoice was initiated. Note that this may be different from the InvoiceDate, since CreatedDateTime is for internal use, to track when the invoice record was first created, but not necessarily the date on which we actually issue the invoice to the client");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("The ID of the Employee who deleted the invoice, if it has been deleted");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time at which this invoice was deleted. If null, then the invoice has not been deleted and hence is a \"live\" invoice which will show up in user searches, etc.");

                entity.Property(e => e.DeletedFreeTextReason)
                    .HasMaxLength(500)
                    .HasComment("Unlike contacts, orgs, etc., it will be useful to track the reason why an invoice is deleted since this should not happen often. At some stage this can be replaced with a lookup once we know what standard reasons might be.");

                entity.Property(e => e.EarlyPaymentDiscountDate).HasColumnType("datetime");

                entity.Property(e => e.EonfilesDeletedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("EONFilesDeletedDateTime");

                entity.Property(e => e.ExportedToSageDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If NULL, this client invoice has never been exported to Sage (i.e. for reconciliation in our accounts software); if specified, this was the date/time when the invoice was (most recently) exported to a spreadsheet for importing into Sage.");

                entity.Property(e => e.FinalValueAmount)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetFinalAmountForInvoiceID]([ID]))", false);

                entity.Property(e => e.FinalValueAmountExcludingVat)
                    .HasColumnType("decimal(38, 18)")
                    .HasColumnName("FinalValueAmountExcludingVAT");

                entity.Property(e => e.FinalValueAmountInGbp)
                    .HasColumnType("decimal(38, 18)")
                    .HasColumnName("FinalValueAmountInGBP")
                    .HasComputedColumnSql("([dbo].[funcCurrencyConversion]([InvoiceCurrencyID],(4),[dbo].[funcGetFinalAmountForInvoiceID]([ID])))", false);

                entity.Property(e => e.FinalVatamount)
                    .HasColumnType("decimal(38, 18)")
                    .HasColumnName("FinalVATAmount");

                entity.Property(e => e.FinalisedDateTime).HasColumnType("datetime");

                entity.Property(e => e.InvoiceAddress1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("First line of address is mandatory; have set other lines to be optional, except for country.");

                entity.Property(e => e.InvoiceAddress2).HasMaxLength(100);

                entity.Property(e => e.InvoiceAddress3).HasMaxLength(100);

                entity.Property(e => e.InvoiceAddress4).HasMaxLength(100);

                entity.Property(e => e.InvoiceCountryId)
                    .HasColumnName("InvoiceCountryID")
                    .HasComment("Country may not be NULL, because it is used to ensure compliance with VAT charging, etc.");

                entity.Property(e => e.InvoiceCountyOrState).HasMaxLength(100);

                entity.Property(e => e.InvoiceCurrencyId)
                    .HasColumnName("InvoiceCurrencyID")
                    .HasComment("The ID (from Currencies table) of the currency in which the invoice is issued. Used to ensure that users cannot attempt to add orders with multiple currencies to a single invoice.");

                entity.Property(e => e.InvoiceDate)
                    .HasColumnType("date")
                    .HasComment("The official date shown on our invoice, i.e. the invoicing date for accounting purposes (as distinct from when the database record may have originally been created). Can be NULL while the invoice details are still being finalised");

                entity.Property(e => e.InvoiceDescription)
                    .IsRequired()
                    .HasComment("Mandatory free text field for summary info about the invoice, e.g. describing in more detail about the jobs being invoiced, how prices were calculated, etc.");

                entity.Property(e => e.InvoiceLangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("InvoiceLangIANAcode")
                    .HasComment("The language used for the invoice (used when auto-generating invoice file, etc.)");

                entity.Property(e => e.InvoiceOrgName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The company/org name as appearing on the invoice. Normally this and address details would be picked up automatically from corresopnding field in Orgs; but we have some clients where we need to invoice things to different end-clients.");

                entity.Property(e => e.InvoicePostcodeOrZip).HasMaxLength(20);

                entity.Property(e => e.InvoiceTitle)
                    .HasMaxLength(300)
                    .HasComment("The title/name of this invoice, used as a main heading, e.g. \"Invoice for jobs completed during September 2009\", or \"Invoice for translation of XYZ Manual\". This is optional since often the title of just the invoice number will be sufficient.");

                entity.Property(e => e.IsFinalised).HasComment("0 = the invoice details are still being determined and the invoice is not yet final; 1 = all invoice details have been finalised and the invoice has been (or should have been) sent to the client. If 1 then no details should subsequently be changed under normal circumstances.");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("The employee who last updated the invoice details");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the invoice details were last updated");

                entity.Property(e => e.OverallChargeToClient)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetOverallChargeToClientForClientInvoiceID]([ID]))", false);

                entity.Property(e => e.OwnerEmployeeId)
                    .HasColumnName("OwnerEmployeeID")
                    .HasComment("The ID (from Employees) of the translate plus employee who is the \"owner\" of the invoice in terms of who the client would contact in the event of any queries");

                entity.Property(e => e.PaidDate)
                    .HasColumnType("datetime")
                    .HasComment("The date when we received the payment from the client (i.e. when it's been received in our bank account or a cheque has been received)");

                entity.Property(e => e.PaidMethodId)
                    .HasColumnName("PaidMethodID")
                    .HasComment("The ID (from PaymentMethods) of the method by which we received the payment from the client (i.e. how our invoice was settled)");

                entity.Property(e => e.PaymentTermDays).HasComment("The number of days following issue of this invoice by when we expect/have agreed to receive payment from the client. Set initially by the equivalent default setting against the invoicing org, but can subsequently be changed on a per-invoice basis.");

                entity.Property(e => e.ReminderFeeCharge).HasColumnType("decimal(38, 18)");

                entity.Property(e => e.SecondContactId)
                    .HasColumnName("SecondContactID")
                    .HasComment("If set, a copy of the invoice will also be sent to this contact. (Some clients want us to cc a second contact on invoices in addition to the main addressee.)");

                entity.Property(e => e.ShowContactNamesInBreakdown).HasComment("Whether the invoice will show the client contact name against each job order in the breakdown section. If all jobs are for the same contact (or if only 1 job is being invoiced), this would probably be set to False; whereas for multiple jobs being invoiced to an org, this would probably be set to True.");

                entity.Property(e => e.ShowCustomerSpecificField1ValueInBreakdown).HasComment("Whether the invoice will show the customer-specific field 1 in the breakdown (the usage/meaning of this field depends on the client)");

                entity.Property(e => e.ShowInterpretingDurationInBreakdown).HasComment("Whether the invoice will show the number of hours/minutes of interpreting time in the jobs breakdown section");

                entity.Property(e => e.ShowJobItemsInBreakdown).HasComment("Whether the invoice will show a full breakdown of all individual job items, rather than just the high-level summmary of orders. Typically I expect that where an invoice is for a single order, we might want to display this level of detail (so the client can see the costs per target language, for example), whereas for batch invoices where we are e.g. invoicing a month's worth of work, this might be set to False to keep the level of information down. But of course it's up to the user.");

                entity.Property(e => e.ShowNotesInBreakdown).HasComment("Whether the client-facing notes should be shown against each job order in the breakdown of what is being invoiced. (For some clients, key information such as their project names may be stored in the Notes field.)");

                entity.Property(e => e.ShowOrderPonumbersInBreakdown)
                    .HasColumnName("ShowOrderPONumbersInBreakdown")
                    .HasComment("Whether the client's PO numbers should be shown against each job order in the breakdown of what is being invoiced. This will normally be False because most clients do not use PO numbers; if they do, it could be set to True if they give us a different PO number for every job, or it could be False if they use a single blanket PO for all work, in which case just one PO number at the top of the invoice would be used, via the ClientPONumber column");

                entity.Property(e => e.ShowPagesOrSlidesInBreakdown).HasComment("Whether the invoice will show the number of pages (for DTP) or slides (for presentations) in the jobs breakdown section");

                entity.Property(e => e.ShowSourceLangsInBreakdown).HasComment("Whether the source language should be shown against each job order in the breakdown of what is being invoiced. If only orders are being shown in the breakdown, i.e. if ShowJobItemsInBreakdown is False, then this will result in the display either of a single source language (if there is only one), or a composite string along the lines of \"x langauges\"");

                entity.Property(e => e.ShowTargetLangsInBreakdown).HasComment("Whether the target language(s) should be shown against each job order in the breakdown of what is being invoiced. If only orders are being shown in the breakdown, i.e. if ShowJobItemsInBreakdown is False, then this will result in the display either of a single target language (if there is only one), or a composite string along the lines of \"x langauges\"");

                entity.Property(e => e.ShowWorkDurationInBreakdown).HasComment("Whether the invoice will show the number of hours/minutes of work\r\n(e.g. for proofreading/software testing/etc.) in the jobs breakdown section");

                entity.Property(e => e.Vatrate)
                    .HasColumnType("decimal(5, 3)")
                    .HasColumnName("VATRate")
                    .HasDefaultValueSql("((0.00))")
                    .HasComment("The percentage VAT being added to the invoice value, e.g. 15, 17.5, etc. If 0 then no VAT is applicable (this can be set manually by the user but typically would be pre-selected automatically based on country of invoice)");

                entity.Property(e => e.WordCountPresentationOption).HasComment("Category determining how much detail to show for word counts presented in the job items breakdown for this invoice. Currently\r\n        DoNotShow = 0\r\n        ShowTotalsOnly = 1\r\n        ShowMemoryBreakdownStandard = 2\r\n        ShowMemoryBreakdownIncludingPerfectMatch = 3");
            });

            modelBuilder.Entity<ClientPriceList>(entity =>
            {
                entity.HasComment("Price lists/matrices which represent lists of rates we charge clients for particular services and language combinations. A single org could have multiple price lists for the same services, for example a price list for marketing translation versus a price list containing higher rates for legal translation, or different price lists for different currencies");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The ID (from Employees) of the employee who created this price list");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time this price list was created");

                entity.Property(e => e.CurrencyId)
                    .HasColumnName("CurrencyID")
                    .HasComment("The ID (from Currencies) which applies to the rates of this price list. A price list cannot combine multiple currencies (instead, we should set up multiple price lists)");

                entity.Property(e => e.DataObjectId)
                    .HasColumnName("DataObjectID")
                    .HasComment("The ID of the data object to which the price list applies");

                entity.Property(e => e.DataObjectTypeId)
                    .HasColumnName("DataObjectTypeID")
                    .HasComment("The type of data object (normally Org or OrgGroup but potentially also Contact) to which the price list applies");

                entity.Property(e => e.DefaultTranslationRate)
                    .HasColumnType("money")
                    .HasComment("For any language combinations not specified in the rates attached to this price list, a \"default\" rate must be specified to assist with auto-calculation and estimation. Unless DefaultTranslationRateIsContractual is True, this rate will be flagged up to the client as \"advisory\" only.");

                entity.Property(e => e.DefaultTranslationRateIsContractual).HasComment("True if we have actively agreed with the client that we will always apply the default rate to any languages not actively specified; if False, we/they know that any calculations based on the Default rate are not necessarily binding.");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("If set, the ID (from Employees) of the Employee who makred this price list as deleted");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date/time when this price list was marked as deleted");

                entity.Property(e => e.ExternalNotes).HasComment("Optional notes for external use (visible to clients in the extranet) where we may want to provide additional information which can't be captured as individual rates, such as information about volume discounts, charges for special services, etc. Can contain HTML formatting for display in extranet/intranet.");

                entity.Property(e => e.InForceSinceDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when this price list came into force; can be used for historical calculations where price lists may change over time");

                entity.Property(e => e.InternalNotes).HasComment("Optional notes for our internal use only (not visible to clients) where we may want to capture additional information about calculations/exceptions which are not shared with the customer. Can contain HTML formatting for display in intranet.");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("If set, the ID (from Employees) of the employee who last modified this price list");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date/time when this price list was last modified");

                entity.Property(e => e.MemoryRateForClientSpecificMatches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForExactMatches)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("This represents the % of any translation rate attached to this price list which we charge for the \"Exact (100%) and repetitions\" category. It is a % of the rate, NOT a % discount off the rate. (E.g. if we charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateForFuzzyBand2Matches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForFuzzyBand3Matches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForFuzzyBand4Matches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForFuzzyMatches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForPerfectMatches)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("Perfect matches/in-context exact/match plus");

                entity.Property(e => e.MemoryRateForRepetitions).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasComment("The name of the price list, visible internally and also to clients via the extranet");

                entity.Property(e => e.NoLongerInForceAsOfDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date/time when this price list stopped being in force; typically used when we want to change a price list to new rates, but want to be able to capture historical data");

                entity.Property(e => e.ShowInExtranet).HasComment("1/True if visible to clients in the extranet application; 0/False if only visible internally to translate plus employees. For many price lists, even where clients are set up for accessing the extranet, we may choose not to share the exact details of pricing with them.");

                entity.Property(e => e.ShowMinimumChargesInExtranet).HasComment("True/1 to display the \"Minimum charges\" column to clients in the extranet application. (We may not always want to clarify, especially to begin with, whether or not we apply minimum charges, until we get a feel for the typical size of projects a particular client sends.) If ShowInExtranet is 0/False then this has no effect.");
            });

            modelBuilder.Entity<ClientPriceListRate>(entity =>
            {
                entity.HasComment("The individual rates (combinations of languages, service and pricing) attached to specific price lists");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientPriceListId)
                    .HasColumnName("ClientPriceListID")
                    .HasComment("The ID of the price list (from ClientPriceLists) to which this rate is attached, and from which it \"inherits\" its currency, memory match rates and other aspects");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The ID (from Employees) of the employee who created this specific rate");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The created date/time, but also to be used to mark when this specific rate came into force; can be used for historical calculations where price lists may change over time");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("If set, the ID (from Employees) of the employee who deleted this specific rate");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the deleted date/time, but also to be used to mark when this specific rate stopped being in force; can be used for historical calculations where price lists may change over time");

                entity.Property(e => e.LanguageRateUnitId)
                    .HasColumnName("LanguageRateUnitID")
                    .HasComment("The unit of charging (e.g. per 1,000 words, per word, per hour) of this rate, from LanguageRateUnits");

                entity.Property(e => e.LanguageServiceId)
                    .HasColumnName("LanguageServiceID")
                    .HasComment("The ID (from LanguageServices) of the linguistic service this rate relates to (e.g. translation, interpreting, etc.)");

                entity.Property(e => e.MinimumCharge)
                    .HasColumnType("money")
                    .HasComment("If not zero, this is the minimum charge for this language/service combination (for clients where we have chosen not to apply a minimum charge, this will appear as 0.00)");

                entity.Property(e => e.Rate)
                    .HasColumnType("money")
                    .HasComment("The monetary rate charged (in the currency inherited from the price list this rate is attached to)");

                entity.Property(e => e.SourceLangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SourceLangIANACode")
                    .HasComment("The source language code/subtag (from Languages) for this rate");

                entity.Property(e => e.TargetLangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("TargetLangIANACode")
                    .HasComment("The target language code/subtag (from Languages) for this rate");
            });

            modelBuilder.Entity<ClientPriceListsHistoricalDatum>(entity =>
            {
                entity.HasComment("Used for historically logging changes in memory match percentages over time, in case in future a client wants to report historically on what we would have charged for memory matches (or other similar data) at a certain point of time, while the rates may have changed more recently.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientPriceListId)
                    .HasColumnName("ClientPriceListID")
                    .HasComment("The ID from the main ClientPriceLists table");

                entity.Property(e => e.DefaultTranslationRate).HasColumnType("money");

                entity.Property(e => e.LoggedByEmployeeId)
                    .HasColumnName("LoggedByEmployeeID")
                    .HasComment("The employee who did the update to the main ClientPriceLists table, causing this data to be added/logged");

                entity.Property(e => e.LoggedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time on which this was added and therefore, by definition, when these figures were superseded by the newer, live client price list figures.");

                entity.Property(e => e.MemoryRateForClientSpecificMatches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForExactMatches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForFuzzyBand2Matches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForFuzzyBand3Matches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForFuzzyBand4Matches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForFuzzyMatches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForPerfectMatches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForRepetitions).HasColumnType("decimal(4, 1)");
            });

            modelBuilder.Entity<ClientSpecificChecklist>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppliesToDataObjectId).HasColumnName("AppliesToDataObjectID");

                entity.Property(e => e.AppliesToDataObjectTypeId).HasColumnName("AppliesToDataObjectTypeID");

                entity.Property(e => e.ChecklistText)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.ChecklistType).HasComment("1 for type “project delivery”, and 2 for “job setup” ");
            });

            modelBuilder.Entity<ClientTechnology>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
            });

            modelBuilder.Entity<Computer>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Computer");

                entity.Property(e => e.ComputerId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ComputerID");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Hostname)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MacAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("macAddress");

                entity.Property(e => e.NetworkUserName).HasMaxLength(100);
            });

            modelBuilder.Entity<Conference>(entity =>
            {
                entity.HasComment("A listing of known conferences in the language industry to help us keep track of where we might consider attending and/or sending mailshots about, etc.");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateOfNextConference)
                    .HasColumnType("smalldatetime")
                    .HasComment("If known, the date (approximate if necessary) when the next conference will take place, to help us keep track of when we might want to register or enquire about sponsorship etc");

                entity.Property(e => e.Description).HasComment("Any additional info about the conference");

                entity.Property(e => e.LastDateWeAttended)
                    .HasColumnType("smalldatetime")
                    .HasComment("If anyone from TP has ever attended this conference, this would store the approximate date when we most recently attended");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the conference (omitting any annual numbering, etc. -- i.e. \"Translation and the Computer\", not \"Translation and the Computer 27\" if it happened to be their 27th annual conference at the time it was added to the list)");

                entity.Property(e => e.NumberOfOccasionsWeAttended).HasComment("How many times TP have attended this conference");

                entity.Property(e => e.Website)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The URL of the conference website (if a conference doesn't have some kind of website, it's not worth knowing about, hence this can not be NULL)");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.OrgId }, "_dta_index_Contacts_12_2078630448__K1_K2");

                entity.HasIndex(e => new { e.Id, e.OrgId, e.Name }, "_dta_index_Contacts_12_2078630448__K1_K2_K3");

                entity.HasIndex(e => new { e.OrgId, e.Id }, "_dta_index_Contacts_12_2078630448__K2_K1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Department).HasMaxLength(255);

                entity.Property(e => e.EmailAddress).HasMaxLength(255);

                entity.Property(e => e.EmailUnsubscribedDateTime).HasColumnType("datetime");

                entity.Property(e => e.FaxCountryId).HasColumnName("FaxCountryID");

                entity.Property(e => e.FaxNumber).HasMaxLength(255);

                entity.Property(e => e.GdpracceptedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("GDPRAcceptedDateTime");

                entity.Property(e => e.GdpracceptedViaIplus).HasColumnName("GDPRAcceptedViaIPlus");

                entity.Property(e => e.GdprrejectedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("GDPRRejectedDateTime");

                entity.Property(e => e.Gdprstatus)
                    .HasColumnName("GDPRStatus")
                    .HasComputedColumnSql("([dbo].[funcGetGDPRStatusOfAContact]([ID]))", false);

                entity.Property(e => e.GdprverballyAccepted).HasColumnName("GDPRVerballyAccepted");

                entity.Property(e => e.HighLowMarginApprovalNeeded)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.InvoiceAddress1).HasMaxLength(100);

                entity.Property(e => e.InvoiceAddress2).HasMaxLength(100);

                entity.Property(e => e.InvoiceAddress3).HasMaxLength(100);

                entity.Property(e => e.InvoiceAddress4).HasMaxLength(100);

                entity.Property(e => e.InvoiceCountryId).HasColumnName("InvoiceCountryID");

                entity.Property(e => e.InvoiceCountyOrState).HasMaxLength(100);

                entity.Property(e => e.InvoiceOrgName).HasMaxLength(200);

                entity.Property(e => e.InvoicePostcodeOrZip).HasMaxLength(20);

                entity.Property(e => e.JobTitle).HasMaxLength(255);

                entity.Property(e => e.LandlineCountryId).HasColumnName("LandlineCountryID");

                entity.Property(e => e.LandlineNumber).HasMaxLength(255);

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastRespondedToOptInPopup).HasColumnType("datetime");

                entity.Property(e => e.MobileCountryId).HasColumnName("MobileCountryID");

                entity.Property(e => e.MobileNumber).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.OptedInForMarketingCampaign).HasDefaultValueSql("((0))");

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.SkypeId)
                    .HasMaxLength(150)
                    .HasColumnName("SkypeID")
                    .HasComment("The Skype ID/user name of the contact");

                entity.Property(e => e.SpendFrequencyAlertLastIssued).HasColumnType("datetime");

                entity.Property(e => e.TpintroductionSource).HasColumnName("TPIntroductionSource");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasComment("Generic list of countries for use in addresses, etc.");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("ID of this country. Set to smallint as a precaution in case of the need to drill down into sub-country level for any reason");

                entity.Property(e => e.DiallingPrefix)
                    .HasMaxLength(6)
                    .HasComment("The international dialling prefix (phone/fax) for this country, excluding the + symbol");

                entity.Property(e => e.Eea).HasColumnName("EEA");

                entity.Property(e => e.GeoPoliticalGroup).HasComment("Indicator of what geographical/political region/organisation the country is in, to help with e.g. the EC Sales List. 0 = unknown; 1 = UK (inc N. Ireland); 2 = EU VAT area but not UK; 3 = EFTA; 4 = rest of Europe, including non-VAT area EU members; 5 = rest of world (at present, we don't need to distinguish other regions)");

                entity.Property(e => e.Ibanlength).HasColumnName("IBANLength");

                entity.Property(e => e.IsIbancountry).HasColumnName("IsIBANCountry");

                entity.Property(e => e.Isocode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("ISOcode")
                    .HasComment("The official ISO code for the country; or one derived from it, such as GB-WA for Wales");

                entity.Property(e => e.SageCountryCode).HasMaxLength(5);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasComment("List of currencies we currently support for invoicing/payment purposes. Localisable information is in the LocalCurrencyNames table");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Prefix)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasComment("A currency prefix, usually a single character like a pound sign or dollar sign, which can be used to present currency values.");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength(true)
                    .HasComment("The 3-letter ISO-4217 code for this currency.");
            });

            modelBuilder.Entity<CustomFiltersForOrderStatusPage>(entity =>
            {
                entity.ToTable("CustomFiltersForOrderStatusPage");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedByContactId).HasColumnName("CreatedByContactID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.DeletedByContactId).HasColumnName("DeletedByContactID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EndDateTo).HasColumnType("datetime");

                entity.Property(e => e.FilterName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.LastModifiedByContactId).HasColumnName("LastModifiedByContactID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.PurchaseOrderNumberToShow).HasMaxLength(500);

                entity.Property(e => e.QuoteIdnumber).HasColumnName("QuoteIDNumber");

                entity.Property(e => e.StartDateFrom).HasColumnType("datetime");

                entity.Property(e => e.StatusToShow).HasMaxLength(20);
            });

            modelBuilder.Entity<DataObjectType>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DeadlineChangeReason>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<DecisionReason>(entity =>
            {
                entity.HasComment("A list of possible reasons why a client may have instructed us to proceed with an quote or have rejected a quote");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasComment("The free-text description of the reason for approving or rejecting");

                entity.Property(e => e.Type).HasComment("Is this reason available only when approving a quote (0), or only when rejecting a quote (1), or for either (2; e.g. \"cost\" could be a reason for proceeding or declining). This affects options presented to the user when marking a quote as approved vs. rejected.");
            });

            modelBuilder.Entity<DesignPlusComment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuthorExtranetUserId).HasColumnName("AuthorExtranetUserID");

                entity.Property(e => e.CommentIndex).HasComment("Index of the comment for specific textframe of design plus file");

                entity.Property(e => e.CommentString)
                    .IsRequired()
                    .HasComment("there can be multiple comments against one textframe, therefore separting the comment strings by seprator &TPComments&");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedByUserId).HasColumnName("DeletedByUserID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeliveryCopyNoteLabel).HasMaxLength(200);

                entity.Property(e => e.DesignPlusFileId).HasColumnName("DesignPlusFileID");

                entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ReviewDeliveryCopyNoteLabel).HasMaxLength(200);

                entity.Property(e => e.ReviewWorkingCopyNoteLabel).HasMaxLength(200);

                entity.Property(e => e.RvDeletedByUserId).HasColumnName("RvDeletedByUserID");

                entity.Property(e => e.RvDeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.RvModifiedByUserId).HasColumnName("RvModifiedByUserID");

                entity.Property(e => e.RvModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ShowInOriginalCopy)
                    .HasDefaultValueSql("((0))")
                    .HasComment("0- Include for review, 1- Exclude for review, 2 - Only for review");

                entity.Property(e => e.TextFrameId).HasColumnName("TextFrameID");

                entity.Property(e => e.TextFrameLabelName).HasMaxLength(100);

                entity.Property(e => e.WorkingCopyNoteLabel)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<DesignPlusDocumentLayer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DesignPlusFileId).HasColumnName("DesignPlusFileID");

                entity.Property(e => e.IndesignId).HasColumnName("IndesignID");

                entity.Property(e => e.IndesignName).HasMaxLength(200);
            });

            modelBuilder.Entity<DesignPlusFileDownload>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.DesignPlusFileId).HasColumnName("DesignPlusFileID");

                entity.Property(e => e.DesignPlusReviewJobId).HasColumnName("DesignPlusReviewJobID");

                entity.Property(e => e.DownloadedByContactId).HasColumnName("DownloadedByContactID");

                entity.Property(e => e.DownloadedDateTime).HasColumnType("datetime");

                entity.Property(e => e.FileTypeDownloaded).HasMaxLength(50);
            });

            modelBuilder.Entity<DesignPlusFolder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.FolderName).IsRequired();

                entity.Property(e => e.ParentFolderId).HasColumnName("ParentFolderID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            });

            modelBuilder.Entity<DesignPlusLinksPath>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DesignPlusFileId).HasColumnName("DesignPlusFileID");

                entity.Property(e => e.LinkName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.LinkType)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.OriginalLinkPath)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.PreviewLinkPath)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<DesignPlusPortCount>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DocsOpen).HasComputedColumnSql("([dbo].[funcCountDocsOpenOnPort]([Port]))", false);
            });

            modelBuilder.Entity<DesignPlusProject>(entity =>
            {
                entity.ToTable("DesignPlusProject");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedDateTIme");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ProjectName).IsRequired();
            });

            modelBuilder.Entity<DesignPlusReviewJob>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DesignPlusFileId).HasColumnName("DesignPlusFileID");

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.NotesToReviewer).HasComment("Optional free-text notes sent to the reviewer");

                entity.Property(e => e.ReviewCompletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ReviewDeadline).HasColumnType("datetime");

                entity.Property(e => e.ReviewStatus).HasComment("0-review not yet started, 1-review in progress, 2-review completed");

                entity.Property(e => e.ReviewerId).HasColumnName("ReviewerID");

                entity.Property(e => e.SentForReviewDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DesignPlusStickyNote>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DesignPlusFileId).HasColumnName("DesignPlusFileID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ModifiedByEmployeeId).HasColumnName("ModifiedByEmployeeID");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DesignPlusTag>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientDesignPlusFileId).HasColumnName("ClientDesignPlusFileID");

                entity.Property(e => e.TagString).IsRequired();

                entity.Property(e => e.TextFrameId).HasColumnName("TextFrameID");
            });

            modelBuilder.Entity<DesignPlusTextChange>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChangeDateTime).HasColumnType("datetime");

                entity.Property(e => e.DesignPlusFileId).HasColumnName("DesignPlusFileID");

                entity.Property(e => e.LabelName).HasMaxLength(50);

                entity.Property(e => e.LabelName1).HasMaxLength(100);

                entity.Property(e => e.NewText).IsRequired();

                entity.Property(e => e.OldText).IsRequired();

                entity.Property(e => e.OriginalTextFrameId).HasColumnName("OriginalTextFrameID");

                entity.Property(e => e.Status).HasComment("Status 0-previewed only, 1-saved, 2-discarded");

                entity.Property(e => e.TextFrameId).HasColumnName("TextFrameID");
            });

            modelBuilder.Entity<DesignPlusTextFrameLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DesignPlusFileId).HasColumnName("DesignPlusFileID");

                entity.Property(e => e.OriginalTextFrameId).HasColumnName("OriginalTextFrameID");

                entity.Property(e => e.PreviewVersionTextFrameId).HasColumnName("PreviewVersionTextFrameID");

                entity.Property(e => e.TextFrameId).HasColumnName("TextFrameID");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.EmailAddress).HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.HighFiveCount).HasComputedColumnSql("([dbo].[funcGetNumberOfHighFivesForEmployee]([ID]))", false);

                entity.Property(e => e.HireDate).HasColumnType("smalldatetime");

                entity.Property(e => e.HrhighFiveNotes).HasColumnName("HRHighFiveNotes");

                entity.Property(e => e.InternalExtension).HasMaxLength(5);

                entity.Property(e => e.JobTitle).HasMaxLength(300);

                entity.Property(e => e.LandlineNumber).HasMaxLength(50);

                entity.Property(e => e.LinkedInUrl)
                    .HasMaxLength(150)
                    .HasColumnName("LinkedInURL");

                entity.Property(e => e.LionUserName).HasMaxLength(10);

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.NetworkUserName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.NextOfKinAddress).HasMaxLength(1000);

                entity.Property(e => e.NextOfKinContactPhoneNumber).HasMaxLength(50);

                entity.Property(e => e.NextOfKinName).HasMaxLength(200);

                entity.Property(e => e.OfficeId).HasColumnName("OfficeID");

                entity.Property(e => e.ResignationSubmitted).HasColumnType("datetime");

                entity.Property(e => e.SalesAccountManagementCommissionPercentage).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.SalesNewBusinessCommissionPercentage).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.ShowAnniversariesOnHomePage)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ShowLikesOrCommentsOnHomePage)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SkypeId)
                    .HasMaxLength(150)
                    .HasColumnName("SkypeID");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.Property(e => e.TerminateDate).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<EmployeeAccessMatrix>(entity =>
            {
                entity.ToTable("EmployeeAccessMatrix");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Notes).HasMaxLength(350);
            });

            modelBuilder.Entity<EmployeeAccessMatrixControl>(entity =>
            {
                entity.ToTable("EmployeeAccessMatrixControls");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ControlName).HasMaxLength(350);

                entity.Property(e => e.Page).HasMaxLength(350);
            });

            modelBuilder.Entity<EmployeeAnnualRevenueTargetsAndThreshold>(entity =>
            {
                entity.HasComment("Financial targets, associated with a financial year (starting on 1 September of the year in question), representing what an employee needs to hit - typically this is for sales people, to represent how much business they need to bring in during the year, and where stated, this needs to exceed a certain threshold. The threshold is normally the value of business they brought in during the preceding financial year, but could be adjusted for particular circumstances.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasComment("The ID of the employee to whom this target and/or threshold applies");

                entity.Property(e => e.FinancialYearStarting)
                    .HasColumnType("date")
                    .HasComment("The start of the financial year to which this target/threshold applies");

                entity.Property(e => e.Gbptarget)
                    .HasColumnType("money")
                    .HasColumnName("GBPTarget")
                    .HasComment("The target to hit this financial year");

                entity.Property(e => e.GbpthresholdQ1)
                    .HasColumnType("money")
                    .HasColumnName("GBPThresholdQ1")
                    .HasComment("The threshold which applies this financial year - needs to be actively set to zero if none applies");

                entity.Property(e => e.GbpthresholdQ2)
                    .HasColumnType("money")
                    .HasColumnName("GBPThresholdQ2");

                entity.Property(e => e.GbpthresholdQ3)
                    .HasColumnType("money")
                    .HasColumnName("GBPThresholdQ3");

                entity.Property(e => e.GbpthresholdQ4)
                    .HasColumnType("money")
                    .HasColumnName("GBPThresholdQ4");
            });

            modelBuilder.Entity<EmployeeDepartment>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<EmployeeExpenseClaim>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.AmountInBaseCurrency).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ClaimMasterId).HasColumnName("ClaimMasterID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ExpenseDate).HasColumnType("smalldatetime");

                entity.Property(e => e.ExpenseDetails).HasMaxLength(500);

                entity.Property(e => e.ExpenseEndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.ScanedReceiptName)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmployeeExpenseClaimsMaster>(entity =>
            {
                entity.ToTable("EmployeeExpenseClaimsMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountsEmployeeId).HasColumnName("AccountsEmployeeID");

                entity.Property(e => e.AmountInGbp)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AmountInGBP");

                entity.Property(e => e.ApprovedByAccountsDateTime).HasColumnType("datetime");

                entity.Property(e => e.ApprovedByManagerDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.ExportedToSageDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ManagerEmployeeId).HasColumnName("ManagerEmployeeID");

                entity.Property(e => e.PaidDateTime).HasColumnType("datetime");

                entity.Property(e => e.SubmittedDateTime).HasColumnType("datetime");

                entity.Property(e => e.TotalBaseCurrencyClaimValue).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<EmployeeHoliday>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.HolidaysApproved)
                    .HasColumnType("decimal(5, 1)")
                    .HasComputedColumnSql("([dbo].[funcGetTotalApprovedHolidays]([EmployeeID],[ID]))", false);

                entity.Property(e => e.HolidaysOnGoingOrTaken)
                    .HasColumnType("decimal(5, 1)")
                    .HasComputedColumnSql("([dbo].[funcGetTotalOnGoingOrTakenHolidays]([EmployeeID],[Year]))", false);

                entity.Property(e => e.HolidaysRemaing)
                    .HasColumnType("decimal(5, 1)")
                    .HasComputedColumnSql("([dbo].[funcGetRemainingHolidays]([ID]))", false);

                entity.Property(e => e.HolidaysRequested)
                    .HasColumnType("decimal(5, 1)")
                    .HasComputedColumnSql("([dbo].[funcGetTotalRequestedHolidays]([EmployeeID],[Year]))", false);

                entity.Property(e => e.HolidaysTaken)
                    .HasColumnType("decimal(5, 1)")
                    .HasComputedColumnSql("([dbo].[funcGetTotalTakenHolidays]([EmployeeID],[Year]))", false);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LoyaltyDays).HasComputedColumnSql("([dbo].[funcGetLoyaltyDaysForEmployee]([EmployeeID],[PreviouslyWorkedDays],[Year]))", false);

                entity.Property(e => e.LoyaltyHolidaysTemp).HasDefaultValueSql("((0))");

                entity.Property(e => e.MiscDays).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.TotalAnnualHolidays).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TotalBaseAnnualHolidays).HasColumnType("decimal(5, 1)");
            });

            modelBuilder.Entity<EmployeeHolidayRequest>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApprovedByEmployeeId).HasColumnName("ApprovedByEmployeeID");

                entity.Property(e => e.ApprovedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EndDateAmorPmorFullDay).HasColumnName("EndDateAMOrPMOrFullDay");

                entity.Property(e => e.HolidayEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.HolidayStartDateTime).HasColumnType("datetime");

                entity.Property(e => e.HolidaysInEndDateYear).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.HolidaysInStartDateYear).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.IsStartDateYearSameAsEndDateYear).HasComputedColumnSql("([dbo].[funcCheckIfStartYearSameAsEndYear]([ID],[EmployeeID]))", false);

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.RejectedByEmployeeId).HasColumnName("RejectedByEmployeeID");

                entity.Property(e => e.RejectedDateTime).HasColumnType("datetime");

                entity.Property(e => e.RequestedByEmployeeId).HasColumnName("RequestedByEmployeeID");

                entity.Property(e => e.RequestedDateTime).HasColumnType("datetime");

                entity.Property(e => e.StartDateAmorPmorFullDay).HasColumnName("StartDateAMOrPMOrFullDay");

                entity.Property(e => e.Status).HasComputedColumnSql("([dbo].[funcGetHolidayStatus]([ID]))", false);

                entity.Property(e => e.TotalDays).HasColumnType("decimal(5, 1)");
            });

            modelBuilder.Entity<EmployeeInterimTarget>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.FinancialYearStarting).HasColumnType("date");

                entity.Property(e => e.InterimTargetQ1).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.InterimTargetQ2).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.InterimTargetQ3).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.InterimTargetQ4).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<EmployeeOffice>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<EmployeeOwnershipRelationship>(entity =>
            {
                entity.HasComment("For defining which employees have which types of ownerships of which kinds of data objects, e.g. Joe Bloggs is the Sales Lead for Widgets Ltd");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CommissionPercentage).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.ConfirmToEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.ConfirmToEndEmpId).HasColumnName("ConfirmToEndEmpID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DataObjectId)
                    .HasColumnName("DataObjectID")
                    .HasComment("The database ID from the table used to represent objects of the type specified in DataObjectTypeID. For example, if DataObjectTypeID indicates a data object of type \"Contact\", then DataObjectID will be the ID of the contact in the Contacts table to whom the task relates; as opposed to the LinguisticSuppliers or Orgs tables, etc.");

                entity.Property(e => e.DataObjectTypeId)
                    .HasColumnName("DataObjectTypeID")
                    .HasComment("The ID of the object type (from DataObjectTypes) representing what type of object the ownership relates to, such as to an org, or to a job order, etc.");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasComment("The ID of the employee (from Employees) who is the \"owner\" of the relevant relationship to the relevant data object");

                entity.Property(e => e.EmployeeOwnershipTypeId)
                    .HasColumnName("EmployeeOwnershipTypeID")
                    .HasComment("The type of ownership (from EmployeeOwnershipTypes) which the relevant employee has to the relevant data object");

                entity.Property(e => e.InForceEndDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time from when this ownership was no longer in force (if NULL, it is still in force). Used primarily for commission calculations (i.e. is/was the employee entitled to commission on a job invoiced on a particular date, etc.). If this date is specified (and is not a date/time in the future), then the ownership is no longer valid, so for example this can be used to determine commission validity for a sales person who has left the company but as of that date, commission may be payable to a different sales person, etc.");

                entity.Property(e => e.InForceStartDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time from when this ownership was/has been in force. Used primarily for commission calculations (i.e. is/was the employee entitled to commission on a job invoiced on a particular date, etc.)");
            });

            modelBuilder.Entity<EmployeeOwnershipType>(entity =>
            {
                entity.HasComment("For defining types of ownerships which employees can have with contacts, organisations, groups and job orders, such as Operational Lead, Sales Lead, etc.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasComment("A human-readable description of what this ownership means in terms of translate plus policies/working practices");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the type of ownership an employee can have");
            });

            modelBuilder.Entity<EmployeeStatusType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<EmployeeTeam>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<EmployeeTimeLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.LogDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmployeesSickness>(entity =>
            {
                entity.ToTable("EmployeesSickness");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApprovedByEmployeeId).HasColumnName("ApprovedByEmployeeID");

                entity.Property(e => e.ApprovedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ConfirmedByEmployeeId).HasColumnName("ConfirmedByEmployeeID");

                entity.Property(e => e.ConfirmedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EndDateAmorPmorFullDay).HasColumnName("EndDateAMOrPMOrFullDay");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.RejectedByEmployeeId).HasColumnName("RejectedByEmployeeID");

                entity.Property(e => e.RejectedDateTime).HasColumnType("datetime");

                entity.Property(e => e.SicknessEndDateTime).HasColumnType("datetime");

                entity.Property(e => e.SicknessStartDateTime).HasColumnType("datetime");

                entity.Property(e => e.StartDateAmorPmorFullDay).HasColumnName("StartDateAMOrPMOrFullDay");

                entity.Property(e => e.TotalDays).HasColumnType("decimal(5, 1)");
            });

            modelBuilder.Entity<EndClient>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EndClient");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<EndClientDatum>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DataObjectName).IsRequired();

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EndClientId).HasColumnName("EndClientID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
            });

            modelBuilder.Entity<Enquiry>(entity =>
            {
                entity.HasComment("Represents an enquiry from a client/potential client asking for a quote on a specific project/file(s). Each enquiry should be associated with a minimum of one Quote (see Quotes table), which is our response, detailing what the costs and timings are.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArchivedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ArchivedToAmazonS3dateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("ArchivedToAmazonS3DateTime");

                entity.Property(e => e.ArchivedToLionBoxDateTime).HasColumnType("datetime");

                entity.Property(e => e.ContactId)
                    .HasColumnName("ContactID")
                    .HasComment("The ID of the client contact (from Contacts) who has requested a quote for this project");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The ID of the employee who set up the enquiry in our systems on the client's behalf");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the enquiry was set up in our systems on the client's behalf");

                entity.Property(e => e.DeadlineRequestedByClient)
                    .HasColumnType("datetime")
                    .HasComment("If set, the deadline by when the client has asked for the work to be completed; if NULL then they haven't specified it (which we would interpret as \"as soon as possible within normal speeds\")");

                entity.Property(e => e.DecisionMadeByContactId)
                    .HasColumnName("DecisionMadeByContactID")
                    .HasComment("The ID of the contact who approved or rejected our quote for this enquiry. Most often it will be the same as ContactID, but in some cases a \"higher authority\" at the client may be required to authorise something.");

                entity.Property(e => e.DecisionMadeDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when our quote for this enquiry was approved or rejected");

                entity.Property(e => e.DecisionReasonId)
                    .HasColumnName("DecisionReasonID")
                    .HasComment("If set, this is the ID (from DecisionReasons) of why the client did or did not choose to proceeed. If NULL then we have not yet had a decision from them.");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("If set, the ID of the employee who deleted the enquiry in our systems");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date/time when the enquiry was deleted in our systems");

                entity.Property(e => e.EnqFilesDeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ExternalNotes).HasComment("Optional free-text client-facing notes (typically something they would have added as a comment via the extranet, and which will be visible via the extranet)");

                entity.Property(e => e.InternalNotes).HasComment("Optional free-text notes for our internal use only - e.g. to make a comment for the enquiries team or sales person to be aware of");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("If set, the ID of the employee who last modified the enquiry in our systems");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date/time when the enquiry was last modified in our systems");

                entity.Property(e => e.OrderChannelId)
                    .HasColumnName("OrderChannelID")
                    .HasComment("The ID (see JobOrderChannels) of the channel through which we received the enquiry, e.g. e-mailed, via the extranet, etc.");

                entity.Property(e => e.Status).HasComment("The current status of the enquiry. Currently hard-coded - 0 = draft (i.e. we are preparing a quote), 1 = pending (we are awaiting a decision from the client), 2 = rejected (the client did not want to proceed), 3 = gone ahead (the client has instructed us to proceed)");

                entity.Property(e => e.WentAheadAsJobOrderId).HasColumnName("WentAheadAsJobOrderID");
            });

            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.HasComment("Current and historical exchange rates");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The ID (from Employees) of the employee who added this rate");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasComment("The date this exchange rate was created. As well as for logging purposes, this is used to determine which is the current exchange rate for a currency combination to be used; only the most recent should be used, with older exchange rates being used for historical analysis/reporting. From a user point of view, they \"modify\" exchange rates, but in this table, a \"modification\" actually adds a new rate so that the previous rate is superseded and retained for historical reasons");

                entity.Property(e => e.IsRateCurrentlyInForce).HasComment("1 if this rate is currently the \"live\" rate for the currencies in question; 0 if it is historical. (This could be calculated by finding the most recent CreatedDate for the combination of currencies, but using a 1/0 bit here simplifies queries.)");

                entity.Property(e => e.Rate)
                    .HasColumnType("decimal(12, 6)")
                    .HasComment("The exchange rate itself. Multiplying a monetary value in the source currency by the Rate would give the equivalent in the target currency");

                entity.Property(e => e.SourceCurrencyId)
                    .HasColumnName("SourceCurrencyID")
                    .HasComment("The ID (from Currencies) of the currency being converted from");

                entity.Property(e => e.TargetCurrencyId)
                    .HasColumnName("TargetCurrencyID")
                    .HasComment("The ID (from Currencies) of the currency being converted into");
            });

            modelBuilder.Entity<ExpenseCategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(450);
            });

            modelBuilder.Entity<ExpenseCategoryLimit>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ExpenseCategoryId).HasColumnName("ExpenseCategoryID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LimitValue).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<ExtraWorkingDay>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.WorkingDate).HasColumnType("date");
            });

            modelBuilder.Entity<ExtranetAccessLevels>(entity =>
            {
                entity.HasComment("Profiles/roles available for users logging into the extranet, which determine which functionality they are permitted to access, whether they can see information about other users, and so on. Levels 1-99 are reserved for \"standard/default\" settings, while anything above 100 is for customer/user-specific settings. Access levels which can be visible to/used by particular groups/contacts/orgs are specified in the ExtranetAvailableAccessLevels table.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CanAccessCmsfunctionality)
                    .HasColumnName("CanAccessCMSFunctionality")
                    .HasComment("True if the user is permitted to access the extended CMS functionality. Initially this is intended only for Net Ent (so should be False for everyone else), but could change in future.");

                entity.Property(e => e.CanAddAndEditCmspublications)
                    .HasColumnName("CanAddAndEditCMSPublications")
                    .HasComment("True if the user is permitted to create/edit new CMSPublications. If the user does not have CanAccessCMSFunctionality = True then this will be irrelevant.");

                entity.Property(e => e.CanAddAndEditGlossaryEntries).HasComment("True if the user is permitted to add new glossary entries and to edit existing entries. If the user does not actually have access to any glossaries to begin with then this setting will have no effect.");

                entity.Property(e => e.CanAddAndManageOtherGroupExtranetUsers).HasComment("True if the user is permitted to add new extranet users at the organisation, and for other organisaitons in the group, and to manage their settings/access levels");

                entity.Property(e => e.CanAddAndManageOtherOrgExtranetUsers).HasComment("True if the user is permitted to add new extranet users at the organisation and to manage their settings/access levels");

                entity.Property(e => e.CanAddEditAndLockCmsreleases)
                    .HasColumnName("CanAddEditAndLockCMSReleases")
                    .HasComment("True if the user is permitted to create/edit/lock new CMSReleases. If the user does not have CanAccessCMSFunctionality = True then this will be irrelevant.");

                entity.Property(e => e.CanApproveGroupWorkRequestsAboveThreshold).HasComment("True if the user is permitted to approve requests above the monetary threshold submitted by other users at the organisation or other organisations in the group. This user will be permitted to approve anything, so it does not matter if the thresholds differ across the different orgs in a group.");

                entity.Property(e => e.CanApproveOrgWorkRequestsAboveThreshold).HasComment("True if the user is permitted to approve requests above the org's monetary threshold submitted by other users at the organisation");

                entity.Property(e => e.CanDownloadOtherGroupCompletedOrders).HasComment("True if the user is permitted to download completed job items for orders (jobs) submitted by other extranet users at the same organisation or at other organisations attached to the same org group");

                entity.Property(e => e.CanDownloadOtherOrgCompletedOrders).HasComment("True if the user is permitted to download completed job items for orders (jobs) submitted by other extranet users at the same organisation");

                entity.Property(e => e.CanRequestInterpretingServicesWork).HasComment("True if the user is permitted to submit requests for interpreting jobs (of any size) via the extranet. Typically this would be False for any translator/supplier, or for clients who only perform a \"review\" role and are not authorised to spend money on their company''s behalf, or for clients who will never need interpreting and do not want this option to be visible.");

                entity.Property(e => e.CanRequestTranslationFromWithinCms)
                    .HasColumnName("CanRequestTranslationFromWithinCMS")
                    .HasComment("True if the user can initiate a translation request from within the CMS/LinguisticDatabase functionality, rather than the \"normal\" route via the uploading of one or more files for translation. If CanAccessCMSFunctionality is False this has no effect.");

                entity.Property(e => e.CanRequestWorkAboveOrgValueThreshold).HasComment("True if the user is permitted to request work/submit orders which are higher in monetary value than the \"approval threshold\" for this organisation. If this is False then the requested work will not be permitted to proceed as a job until a nominated approver at the client org has confirmed it is Ok to go ahead. If no threshold is set for this org then this setting will have no effect.");

                entity.Property(e => e.CanRequestWrittenServicesWork).HasComment("True if the user is permitted to submit translation, transcription, proofreading or other written-service jobs (of any size) via the extranet. Typically this would be False for any translator/supplier, or for clients who only perform a \"review\" role and are not authorised to spend money on their company''s behalf.");

                entity.Property(e => e.CanRerouteReviewJobsToOtherGroupExtranetUsers).HasComment("True if the user is permitted to re-route review assignments intended for one extranet user to a different extranet user at the same organisation or at another org in the same org group");

                entity.Property(e => e.CanRerouteReviewJobsToOtherOrgExtranetUsers).HasComment("True if the user is permitted to re-route review assignments intended for one extranet user to a different extranet user at the same organisation");

                entity.Property(e => e.CanReviewOtherGroupJobsInAnyLanguageCombo).HasComment("True if the user is permitted to use the review functionality to edit/review/approve the text of translations in any language combination, which were originally intended to be reviewed by another extranet user at another organisation within the same organisation group");

                entity.Property(e => e.CanReviewOtherGroupJobsInOwnLanguageCombos).HasComment("True if the user is permitted to use the review functionality to edit/review/approve the text of translations in the language combinations which they are \"responsible for\", which were originally intended to be reviewed by another extranet user at other organisations within the same organisation group");

                entity.Property(e => e.CanReviewOtherOrgJobsInOwnLanguageCombos).HasComment("True if the user is permitted to use the review functionality to edit/review/approve the text of translations in any language combination, which were originally intended to be reviewed by another extranet user at the same organisation");

                entity.Property(e => e.CanReviewOwnJobsInOwnLanguageCombos).HasComment("True if the user is permitted to use the review functionality to edit/review/approve the text of translations in the language combinations which they are \"responsible for\". These language combinations are specified in ExtranetUserReviewLanguageCombos.");

                entity.Property(e => e.CanSignOffReviewRequestsWithoutViewingFirst).HasComment("True if the user is allowed to click on/activate an \"Approve this review request\" function before viewing the text for review first. In other words, this means they can say \"I don't want to look at this, I just want to approve it anyway\".");

                entity.Property(e => e.CanViewDetailsOfOtherGroupOrders).HasComment("True if the user is permitted to view a list/details of orders (jobs) submitted by other extranet users at the same organisation and at other organisations in the same org group");

                entity.Property(e => e.CanViewDetailsOfOtherGroupUsers).HasComment("True if the user is permitted to view a list/details of other extranet users at the same organisation and at other organisations within the org group. If the org is not attached to a group then this setting would be ignored.");

                entity.Property(e => e.CanViewDetailsOfOtherOrgOrders).HasComment("True if the user is permitted to view a list/details of orders (jobs) submitted by other extranet users at the same organisation");

                entity.Property(e => e.CanViewDetailsOfOtherOrgUsers).HasComment("True if the user is permitted to view a list/details of other extranet users at the same organisation");

                entity.Property(e => e.CanViewLinguisticSuppliers).HasComment("For a handful of clients we want to allow them to see limited supplier details via the extranet application. This is either (a) ONLY our LinguisticSupplier.ID number, for clients where we have agreed they can see an anonymous identifier for the purposes of agreeing to use approved translators and for contacting us to discuss feedback etc.; or (b) the LinguisticSupplier.ID number and the supplier's name, but ONLY when the supplier is a client-internal translator and not one of our freelances. If this is False then all linguistic supplier info is hidden.");

                entity.Property(e => e.CanViewPricingAndCosts).HasComment("True if the user is permitted to view client costs/pricing information. Some customers may want specific users (e.g. reviewers, who may belong to third-party organisations) to not be able to see what we are charging, while still allowing them to do review or even submit a request for translation, etc.");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasComment("A human-readable description summarising what the current access level permits/is designed for");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The human-readable name of this access level, which will appear in the extranet and also in the intranet when managing users internally");
            });

            modelBuilder.Entity<ExtranetAvailableAccessLevel>(entity =>
            {
                entity.HasNoKey();

                entity.HasComment("Specifies which extranet access levels are available to which contacts/orgs/groups/linguistic suppliers, so that, for example, customer-specific access levels will be available only for them and not for other customers");

                entity.Property(e => e.AccessLevelId)
                    .HasColumnName("AccessLevelID")
                    .HasComment("The ID (from ExtranetAccessLevels) of the access level available to the specified data object");

                entity.Property(e => e.DataObjectId)
                    .HasColumnName("DataObjectID")
                    .HasComment("The ID of the data object this access level is available to");

                entity.Property(e => e.DataObjectTypeId)
                    .HasColumnName("DataObjectTypeID")
                    .HasComment("Which kind of data object this access level is available to");
            });

            modelBuilder.Entity<ExtranetSiteMap>(entity =>
            {
                entity.ToTable("ExtranetSiteMap");

                entity.HasComment("Multilingual site map content for the extranet web UI, including details about whether certain nodes should/shouldn't be displayed depending on the permissions/access level of the currently logged in user");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.DescriptionDa)
                    .HasMaxLength(400)
                    .HasColumnName("Description-da");

                entity.Property(e => e.DescriptionDe)
                    .HasMaxLength(400)
                    .HasColumnName("Description-de");

                entity.Property(e => e.DescriptionEn)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("Description-en")
                    .HasComment("The description of the sitemap node, typically displayed as a tooltip when the mouse is over the title link");

                entity.Property(e => e.DescriptionSv)
                    .HasMaxLength(400)
                    .HasColumnName("Description-sv");

                entity.Property(e => e.PageLink)
                    .HasMaxLength(200)
                    .HasComment("The (relative) link to the name of the page within the extranet application. Typically this would be a simple page name, like ContactUs.aspx, but it could contain querystring parameters too");

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasComment("The ID of the parent node under which this node should appear as a sub-node, for building the tree structure");

                entity.Property(e => e.ShowOnlyForClients).HasComment("A general \"filter\" for whether this node should be displayed for any type of user (NULL), for only translators/linguistic suppliers (0/False), or for only clients (1/True)");

                entity.Property(e => e.TitleDa)
                    .HasMaxLength(200)
                    .HasColumnName("Title-da");

                entity.Property(e => e.TitleDe)
                    .HasMaxLength(200)
                    .HasColumnName("Title-de");

                entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Title-en")
                    .HasComment("The title of the link in the sitemap structure, as displayed on the page to the user");

                entity.Property(e => e.TitleSv)
                    .HasMaxLength(200)
                    .HasColumnName("Title-sv");
            });

            modelBuilder.Entity<ExtranetSiteMapClientSpecific>(entity =>
            {
                entity.ToTable("ExtranetSiteMapClientSpecific");

                entity.HasComment("Multilingual site map content for the extranet web UI, including details about whether certain nodes should/shouldn't be displayed depending on the permissions/access level of the currently logged in user");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.DescriptionDa)
                    .HasMaxLength(400)
                    .HasColumnName("Description-da");

                entity.Property(e => e.DescriptionDe)
                    .HasMaxLength(400)
                    .HasColumnName("Description-de");

                entity.Property(e => e.DescriptionEn)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("Description-en")
                    .HasComment("The description of the sitemap node, typically displayed as a tooltip when the mouse is over the title link");

                entity.Property(e => e.DescriptionSv)
                    .HasMaxLength(400)
                    .HasColumnName("Description-sv");

                entity.Property(e => e.PageLink)
                    .HasMaxLength(200)
                    .HasComment("The (relative) link to the name of the page within the extranet application. Typically this would be a simple page name, like ContactUs.aspx, but it could contain querystring parameters too");

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasComment("The ID of the parent node under which this node should appear as a sub-node, for building the tree structure");

                entity.Property(e => e.ShowOnlyForClients).HasComment("A general \"filter\" for whether this node should be displayed for any type of user (NULL), for only translators/linguistic suppliers (0/False), or for only clients (1/True)");

                entity.Property(e => e.TitleDa)
                    .HasMaxLength(200)
                    .HasColumnName("Title-da");

                entity.Property(e => e.TitleDe)
                    .HasMaxLength(200)
                    .HasColumnName("Title-de");

                entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Title-en")
                    .HasComment("The title of the link in the sitemap structure, as displayed on the page to the user");

                entity.Property(e => e.TitleSv)
                    .HasMaxLength(200)
                    .HasColumnName("Title-sv");
            });

            modelBuilder.Entity<ExtranetSiteMapForBernafon>(entity =>
            {
                entity.ToTable("ExtranetSiteMapForBernafon");

                entity.HasComment("Multilingual site map content for the extranet web UI, including details about whether certain nodes should/shouldn't be displayed depending on the permissions/access level of the currently logged in user");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.DescriptionDa)
                    .HasMaxLength(400)
                    .HasColumnName("Description-da");

                entity.Property(e => e.DescriptionDe)
                    .HasMaxLength(400)
                    .HasColumnName("Description-de");

                entity.Property(e => e.DescriptionEn)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("Description-en")
                    .HasComment("The description of the sitemap node, typically displayed as a tooltip when the mouse is over the title link");

                entity.Property(e => e.DescriptionSv)
                    .HasMaxLength(400)
                    .HasColumnName("Description-sv");

                entity.Property(e => e.PageLink)
                    .HasMaxLength(200)
                    .HasComment("The (relative) link to the name of the page within the extranet application. Typically this would be a simple page name, like ContactUs.aspx, but it could contain querystring parameters too");

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasComment("The ID of the parent node under which this node should appear as a sub-node, for building the tree structure");

                entity.Property(e => e.ShowOnlyForClients).HasComment("A general \"filter\" for whether this node should be displayed for any type of user (NULL), for only translators/linguistic suppliers (0/False), or for only clients (1/True)");

                entity.Property(e => e.TitleDa)
                    .HasMaxLength(200)
                    .HasColumnName("Title-da");

                entity.Property(e => e.TitleDe)
                    .HasMaxLength(200)
                    .HasColumnName("Title-de");

                entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Title-en")
                    .HasComment("The title of the link in the sitemap structure, as displayed on the page to the user");

                entity.Property(e => e.TitleSv)
                    .HasMaxLength(200)
                    .HasColumnName("Title-sv");
            });

            modelBuilder.Entity<ExtranetSiteMapForBostik>(entity =>
            {
                entity.ToTable("ExtranetSiteMapForBostik");

                entity.HasComment("Multilingual site map content for the extranet web UI, including details about whether certain nodes should/shouldn't be displayed depending on the permissions/access level of the currently logged in user");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.DescriptionDa)
                    .HasMaxLength(400)
                    .HasColumnName("Description-da");

                entity.Property(e => e.DescriptionDe)
                    .HasMaxLength(400)
                    .HasColumnName("Description-de");

                entity.Property(e => e.DescriptionEn)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("Description-en")
                    .HasComment("The description of the sitemap node, typically displayed as a tooltip when the mouse is over the title link");

                entity.Property(e => e.DescriptionSv)
                    .HasMaxLength(400)
                    .HasColumnName("Description-sv");

                entity.Property(e => e.PageLink)
                    .HasMaxLength(200)
                    .HasComment("The (relative) link to the name of the page within the extranet application. Typically this would be a simple page name, like ContactUs.aspx, but it could contain querystring parameters too");

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasComment("The ID of the parent node under which this node should appear as a sub-node, for building the tree structure");

                entity.Property(e => e.ShowOnlyForClients).HasComment("A general \"filter\" for whether this node should be displayed for any type of user (NULL), for only translators/linguistic suppliers (0/False), or for only clients (1/True)");

                entity.Property(e => e.TitleDa)
                    .HasMaxLength(200)
                    .HasColumnName("Title-da");

                entity.Property(e => e.TitleDe)
                    .HasMaxLength(200)
                    .HasColumnName("Title-de");

                entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Title-en")
                    .HasComment("The title of the link in the sitemap structure, as displayed on the page to the user");

                entity.Property(e => e.TitleSv)
                    .HasMaxLength(200)
                    .HasColumnName("Title-sv");
            });

            modelBuilder.Entity<ExtranetSiteMapForEon>(entity =>
            {
                entity.ToTable("ExtranetSiteMapForEON");

                entity.HasComment("Multilingual site map content for the extranet web UI, including details about whether certain nodes should/shouldn't be displayed depending on the permissions/access level of the currently logged in user");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.DescriptionDa)
                    .HasMaxLength(400)
                    .HasColumnName("Description-da");

                entity.Property(e => e.DescriptionDe)
                    .HasMaxLength(400)
                    .HasColumnName("Description-de");

                entity.Property(e => e.DescriptionEn)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("Description-en")
                    .HasComment("The description of the sitemap node, typically displayed as a tooltip when the mouse is over the title link");

                entity.Property(e => e.DescriptionSv)
                    .HasMaxLength(400)
                    .HasColumnName("Description-sv");

                entity.Property(e => e.PageLink)
                    .HasMaxLength(200)
                    .HasComment("The (relative) link to the name of the page within the extranet application. Typically this would be a simple page name, like ContactUs.aspx, but it could contain querystring parameters too");

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasComment("The ID of the parent node under which this node should appear as a sub-node, for building the tree structure");

                entity.Property(e => e.ShowOnlyForClients).HasComment("A general \"filter\" for whether this node should be displayed for any type of user (NULL), for only translators/linguistic suppliers (0/False), or for only clients (1/True)");

                entity.Property(e => e.TitleDa)
                    .HasMaxLength(200)
                    .HasColumnName("Title-da");

                entity.Property(e => e.TitleDe)
                    .HasMaxLength(200)
                    .HasColumnName("Title-de");

                entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Title-en")
                    .HasComment("The title of the link in the sitemap structure, as displayed on the page to the user");

                entity.Property(e => e.TitleSv)
                    .HasMaxLength(200)
                    .HasColumnName("Title-sv");
            });

            modelBuilder.Entity<ExtranetSiteMapForFlsmidth>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ExtranetSiteMapForFLSmidth");

                entity.Property(e => e.DescriptionDa)
                    .HasMaxLength(400)
                    .HasColumnName("Description-da");

                entity.Property(e => e.DescriptionDe)
                    .HasMaxLength(400)
                    .HasColumnName("Description-de");

                entity.Property(e => e.DescriptionEn)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("Description-en");

                entity.Property(e => e.DescriptionSv)
                    .HasMaxLength(400)
                    .HasColumnName("Description-sv");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PageLink).HasMaxLength(200);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.TitleDa)
                    .HasMaxLength(200)
                    .HasColumnName("Title-da");

                entity.Property(e => e.TitleDe)
                    .HasMaxLength(200)
                    .HasColumnName("Title-de");

                entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Title-en");

                entity.Property(e => e.TitleSv)
                    .HasMaxLength(200)
                    .HasColumnName("Title-sv");
            });

            modelBuilder.Entity<ExtranetSiteMapForRb>(entity =>
            {
                entity.ToTable("ExtranetSiteMapForRB");

                entity.HasComment("Multilingual site map content for the extranet web UI, including details about whether certain nodes should/shouldn't be displayed depending on the permissions/access level of the currently logged in user");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.DescriptionDa)
                    .HasMaxLength(400)
                    .HasColumnName("Description-da");

                entity.Property(e => e.DescriptionDe)
                    .HasMaxLength(400)
                    .HasColumnName("Description-de");

                entity.Property(e => e.DescriptionEn)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("Description-en")
                    .HasComment("The description of the sitemap node, typically displayed as a tooltip when the mouse is over the title link");

                entity.Property(e => e.DescriptionSv)
                    .HasMaxLength(400)
                    .HasColumnName("Description-sv");

                entity.Property(e => e.PageLink)
                    .HasMaxLength(200)
                    .HasComment("The (relative) link to the name of the page within the extranet application. Typically this would be a simple page name, like ContactUs.aspx, but it could contain querystring parameters too");

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasComment("The ID of the parent node under which this node should appear as a sub-node, for building the tree structure");

                entity.Property(e => e.ShowOnlyForClients).HasComment("A general \"filter\" for whether this node should be displayed for any type of user (NULL), for only translators/linguistic suppliers (0/False), or for only clients (1/True)");

                entity.Property(e => e.TitleDa)
                    .HasMaxLength(200)
                    .HasColumnName("Title-da");

                entity.Property(e => e.TitleDe)
                    .HasMaxLength(200)
                    .HasColumnName("Title-de");

                entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Title-en")
                    .HasComment("The title of the link in the sitemap structure, as displayed on the page to the user");

                entity.Property(e => e.TitleSv)
                    .HasMaxLength(200)
                    .HasColumnName("Title-sv");
            });

            modelBuilder.Entity<ExtranetSiteMapforImc>(entity =>
            {
                entity.ToTable("ExtranetSiteMapforIMC");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.DescriptionDa)
                    .HasMaxLength(400)
                    .HasColumnName("Description-da");

                entity.Property(e => e.DescriptionDe)
                    .HasMaxLength(400)
                    .HasColumnName("Description-de");

                entity.Property(e => e.DescriptionEn)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("Description-en")
                    .HasComment("The description of the sitemap node, typically displayed as a tooltip when the mouse is over the title link");

                entity.Property(e => e.DescriptionSv)
                    .HasMaxLength(400)
                    .HasColumnName("Description-sv");

                entity.Property(e => e.PageLink)
                    .HasMaxLength(200)
                    .HasComment("The (relative) link to the name of the page within the extranet application. Typically this would be a simple page name, like ContactUs.aspx, but it could contain querystring parameters too");

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentID")
                    .HasComment("The ID of the parent node under which this node should appear as a sub-node, for building the tree structure");

                entity.Property(e => e.ShowOnlyForClients).HasComment("A general \"filter\" for whether this node should be displayed for any type of user (NULL), for only translators/linguistic suppliers (0/False), or for only clients (1/True)");

                entity.Property(e => e.TitleDa)
                    .HasMaxLength(200)
                    .HasColumnName("Title-da");

                entity.Property(e => e.TitleDe)
                    .HasMaxLength(200)
                    .HasColumnName("Title-de");

                entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Title-en")
                    .HasComment("The title of the link in the sitemap structure, as displayed on the page to the user");

                entity.Property(e => e.TitleSv)
                    .HasMaxLength(200)
                    .HasColumnName("Title-sv");
            });

            modelBuilder.Entity<ExtranetUser>(entity =>
            {
                entity.HasKey(e => e.UserName);

                entity.HasComment("External users (clients or linguistic suppliers) who are authorised to log in to our extranet.");

                entity.HasIndex(e => e.WebServiceGuid, "_dta_index_ExtranetUsers_9_1396225516__K10_7");

                entity.HasIndex(e => new { e.DataObjectId, e.DataObjectTypeId }, "_dta_index_ExtranetUsers_9_1396225516__K7_K6");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasComment("The user name for logging into the extranet. This must be UNIQUE per user. Unless special circumstances apply, this will be the user's e-mail address.");

                entity.Property(e => e.AccessLevelId)
                    .HasColumnName("AccessLevelID")
                    .HasComment("The permissions/access rights specified in ExtranetAccessLevels");

                entity.Property(e => e.CustomizedHomePageLayout).IsUnicode(false);

                entity.Property(e => e.DataObjectId)
                    .HasColumnName("DataObjectID")
                    .HasComment("The ID of the user to be looked up in the relevant table (e.g. Contacts or LinguisticSuppliers)");

                entity.Property(e => e.DataObjectTypeId)
                    .HasColumnName("DataObjectTypeID")
                    .HasComment("The type of user (see DataObjectTypes) the login is derived from. This is then used to determine where to get user details from (e.g. Contacts or LinguisticSuppliers)");

                entity.Property(e => e.DefaultTimeZone)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("(N'GMT Standard Time')");

                entity.Property(e => e.DesignplusEnabled).HasColumnName("designplusEnabled");

                entity.Property(e => e.FirstEverLoginDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The first time this user ever logged in to the extranet. If NULL, they have never done so. Helps us ascertain when they've started using the system.");

                entity.Property(e => e.HashedPassword)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasComment("A hashed representation of the user's password.");

                entity.Property(e => e.HashedSecretQuestionAnswer).HasMaxLength(512);

                entity.Property(e => e.LastLoginDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The most recent occasion on which they logged in. Can help trouble-shooting/auditing/security.");

                entity.Property(e => e.LockedOutDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date/time when this user was locked out of the system, i.e. is no longer allowed access to our systems, similar to a DeletedDateTime, but for example when a client actively asks us to turn off access for a particular user, but where for historical reasons we may want to show their name in the extranet application, e.g. if they edited a term/string in a linguistic database, etc. We could also at some stage use this for automatically locking someone based on too many incorrect attempts to access the system.");

                entity.Property(e => e.MustResetPasswordOnNextLogin).HasComment("True if when the user next logs in, he/she must reset their password");

                entity.Property(e => e.PasswordLastSetDateTime)
                    .HasColumnType("datetime")
                    .HasComment("When the user's password was last set (either initially, or re-set by the user or by an internal emloyee on their behalf)");

                entity.Property(e => e.PreferredExtranetUilangIanacode)
                    .HasMaxLength(20)
                    .HasColumnName("PreferredExtranetUILangIANACode")
                    .HasComment("If set, the code of the language (from the Languages table) in which this user prefers to view the extranet web application. (If NULL, the application will default to displaying in English.)");

                entity.Property(e => e.PreviousLoginDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The occasion before the most recent when they logged in - enables retention of a current login start while still displaying the previous login time to the user. Can help trouble-shooting/auditing/security.");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The \"salt\" used to minimise risk of dictionary-based password attacks");

                entity.Property(e => e.SecretQuestionAnswerSalt).HasMaxLength(100);

                entity.Property(e => e.SecretQuestionId).HasColumnName("SecretQuestionID");

                entity.Property(e => e.ShowDesignPlusInfoBox).HasDefaultValueSql("((0))");

                entity.Property(e => e.TranslateonlineAllowed).HasColumnName("translateonlineAllowed");

                entity.Property(e => e.UserProfileImagePath)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserSetupByEmployeeId)
                    .HasColumnName("UserSetupByEmployeeID")
                    .HasComment("The ID (from Employees) of the translate plus employee who authorised  this user to access the extranet");

                entity.Property(e => e.UserSetupDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when we authorised  this user to access the extranet");

                entity.Property(e => e.WebServiceGuid)
                    .HasColumnName("WebServiceGUID")
                    .HasComment("If this is set, it enables the user to programmatically access our systems via TPWS (Web Service), where they must provide this matching GUID to gain access");

                entity.Property(e => e.WebServiceNotificationEmailAddress)
                    .HasMaxLength(300)
                    .HasComment("The e-mail address to send any notifications to, if the user is enabled for Web Service access. This is 300 chars because we may want to provide a comma-separated list of multiple addresses");

                entity.Property(e => e.WebServiceNotifyOnFileCollection).HasComment("For Web Service users, 1/True if they will automatically receive confirmation by e-mail for every file successfully collected via the Web Service");

                entity.Property(e => e.WebServiceNotifyOnOrderSubmission).HasComment("For Web Service users, 1/True if they will automatically receive confirmation by e-mail for every job successfully submitted via the Web Service");
            });

            modelBuilder.Entity<ExtranetUsersPassword>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.HashedPassword).HasMaxLength(512);

                entity.Property(e => e.PasswordExpiryDateTime).HasColumnType("datetime");

                entity.Property(e => e.PasswordSetDateTime).HasColumnType("datetime");

                entity.Property(e => e.Salt).HasMaxLength(100);
            });

            modelBuilder.Entity<ExtranetUsersPasswordsLog>(entity =>
            {
                entity.ToTable("ExtranetUsersPasswordsLog");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.HashedPassword).HasMaxLength(512);

                entity.Property(e => e.PasswordSetDateTime).HasColumnType("datetime");

                entity.Property(e => e.Salt).HasMaxLength(100);
            });

            modelBuilder.Entity<ExtranetUsersReviewLanguage>(entity =>
            {
                entity.HasNoKey();

                entity.HasComment("Defines which extranet users are permitted to review translations in specific languages. For example, if a user is permitted to review translations, a customer might want to limit that user to only be able to review French translations. Internally we can then use these assignments to select who we can/should assign review requests to.");

                entity.Property(e => e.ExtranetUserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The user name (from ExtranetUsers) to whom the language permission applies");

                entity.Property(e => e.IsLeadReviewer).HasComment("True = this reviewer is a lead reviewer in the relevant language (used in sorting when picking potential reviewers for automatic assignment)");

                entity.Property(e => e.TargetLangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("TargetLangIANACode")
                    .HasComment("The target language the reviewer can work in (in other words, he/she can edit/approve translations from any source language into this target language)");
            });

            modelBuilder.Entity<ExtranetUsersSecurityQuestion>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LanguageIana)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("LanguageIANA");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(1500);
            });

            modelBuilder.Entity<FileExtensionsForAutomation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FileExtensionsForAutomation");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Ftpdetail>(entity =>
            {
                entity.ToTable("FTPDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmpId).HasColumnName("CreatedByEmpID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.Ftppassword)
                    .IsRequired()
                    .HasColumnName("FTPPassword");

                entity.Property(e => e.FtpuserName)
                    .IsRequired()
                    .HasColumnName("FTPUserName");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedEmpId).HasColumnName("LastModifiedEmpID");

                entity.Property(e => e.MiscFtpname)
                    .HasMaxLength(200)
                    .HasColumnName("MiscFTPName");
            });

            modelBuilder.Entity<GroupeLanguageServiceRateCard>(entity =>
            {
                entity.ToTable("GroupeLanguageServiceRateCard");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.InForceEndDate).HasColumnType("datetime");

                entity.Property(e => e.InForcedSinceDate).HasColumnType("datetime");

                entity.Property(e => e.RateCardId).HasColumnName("RateCardID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.SourceIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SourceIANACode");

                entity.Property(e => e.TargetIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("TargetIANACode");

                entity.Property(e => e.WorstCaseCost)
                    .HasColumnType("decimal(5, 2)")
                    .HasComputedColumnSql("(CONVERT([decimal](5,2),[Cost]+([Cost]*[WorstCasePercentage])/(100),0))", false);

                entity.Property(e => e.WorstCasePercentage).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<GroupeMasterRate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.FixedPcrate)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("FixedPCRate");

                entity.Property(e => e.Fpc)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FPC");

                entity.Property(e => e.InForcedEndDate).HasColumnType("datetime");

                entity.Property(e => e.InForcedStartDate).HasColumnType("datetime");

                entity.Property(e => e.MarkUp).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Ohrate)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("OHRate");

                entity.Property(e => e.PgdcentralCostsRate)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("PGDCentralCostsRate");

                entity.Property(e => e.PmhourlyRate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PMHourlyRate");

                entity.Property(e => e.SupportContribution).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<GroupeRateCard>(entity =>
            {
                entity.ToTable("GroupeRateCard");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RateCard)
                    .IsRequired()
                    .HasMaxLength(1500);
            });

            modelBuilder.Entity<HighFife>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.HighFiveComment).IsRequired();
            });

            modelBuilder.Entity<InRiverFieldsBeingTranslated>(entity =>
            {
                entity.ToTable("InRiverFieldsBeingTranslated");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EntityId).HasColumnName("EntityID");

                entity.Property(e => e.FieldName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.JobOrderId).HasColumnName("JobOrderID");

                entity.Property(e => e.SourceLanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("SourceLanguageIANACode");

                entity.Property(e => e.TargetLanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TargetLanguageIANACode");
            });

            modelBuilder.Entity<JabraDetailsTemp>(entity =>
            {
                entity.ToTable("JabraDetailsTemp");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateTimeGenerated)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ItemAttributes).IsRequired();

                entity.Property(e => e.ItemText).IsRequired();

                entity.Property(e => e.SourceLanguage)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.StyleId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("StyleID");

                entity.Property(e => e.TargetLanguage)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<JobItem>(entity =>
            {
                entity.HasComment("An individual item comprising part of a complete order. An \"item\" is what an individual supplier thinks of as a \"job\", e.g. an English-French translation job of 5000 words -- but from the client's point of view, they may have ordered 5000 words from English into 10 languages, so the French part is only one part of the \"job\". By breaking down items we can track status and cost for all constituent parts of an order, whether or not we choose to show all of that breakdown to the client.");

                entity.HasIndex(e => new { e.DeletedDateTime, e.LanguageServiceId, e.SupplierCompletedItemDateTime, e.Id, e.JobOrderId, e.SourceLanguageIanacode, e.TargetLanguageIanacode, e.LinguisticSupplierOrClientReviewerId }, "_dta_index_JobItems_12_1933614327__K52_K4_K33_K1_K2_K5_K6_K39_7_8_9_10_11_12_13_30_31_40_57_58");

                entity.HasIndex(e => new { e.JobOrderId, e.DeletedDateTime, e.Id, e.CreatedDateTime }, "_dta_index_JobItems_12_2055678371__K2_K48_K1_K44");

                entity.HasIndex(e => new { e.LinguisticSupplierOrClientReviewerId, e.DeletedDateTime, e.SupplierAcceptedWorkDateTime, e.SupplierSentWorkDateTime, e.CreatedDateTime }, "_dta_index_JobItems_12_2055678371__K35_K48_K27_K26_K44_1");

                entity.HasIndex(e => new { e.DeletedDateTime, e.JobOrderId }, "_dta_index_JobItems_12_2055678371__K48_K2_37_38_39");

                entity.HasIndex(e => new { e.DeletedDateTime, e.JobOrderId, e.Id, e.LanguageServiceId }, "_dta_index_JobItems_12_2055678371__K48_K2_K1_K4_7_8_9_10_11_37");

                entity.HasIndex(e => new { e.DeletedDateTime, e.WebServiceClientStatusId, e.JobOrderId, e.Id, e.CreatedDateTime }, "_dta_index_JobItems_9_613446551__K55_K50_K2_K1_K51");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnticipatedFinalValueAmount)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetAnticipatedOverallChargeToClientForJobItemID]([ID]))", false);

                entity.Property(e => e.AnticipatedGrossMarginPercentage)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetAnticipatedGrossMarginPercentageForJobItemID]([ID]))", false);

                entity.Property(e => e.AudioMinutes).HasComment("For transcription jobs, or anything where we might be working with a source audio recording, the number of minutes in the recording (as opposed to the number of WorkMinutes)");

                entity.Property(e => e.ChargeToClient)
                    .HasColumnType("money")
                    .HasComment("The monetary value we are charging the client for this item. Can be zero if we are doing it for free, or if we need to track something for which we will pay a supplier but are not charging separately for the client. The currency must be \"inherited\" from the parent JobOrder.");

                entity.Property(e => e.ChargeToClientAfterDiscountSurcharges)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetChargeToClientForJobItemsAfterDiscountSurcharges]([ID],[JobOrderID]))", false);

                entity.Property(e => e.ClientCostCalculatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ClientCostCalculatedById).HasColumnName("ClientCostCalculatedByID");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The job could be created in the database by a client using the extranet/Web Service, in which case this field would be NULL. However, where we've received work by e-mail/FTP etc., this field tracks the internal employee who entered details into the system.");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the item was added to the database");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("The ID of the employee who deleted this item. If null, then the item has not been deleted and hence is a \"live\" job which will show up in user searches, etc.");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time at which this item was deleted. If null, then the item has not been deleted and hence is a \"live\" job which will show up in user searches, etc.");

                entity.Property(e => e.DeletedFreeTextReason)
                    .HasMaxLength(500)
                    .HasComment("Unlike contacts, orgs, etc., it will be useful to track the reason why a live job item is deleted since this should not happen often. At some stage this can be replaced with a lookup once we know what standard reasons might be.");

                entity.Property(e => e.DescriptionForSupplierOnly).HasComment("A free-text field of any notes/description, which will be visible only to the supplier (whether or not the item as a whole has IsVisibleToClient = 1). Allows us to make potentially non-client-facing notes visible to a supplier via the extranet if needed.");

                entity.Property(e => e.DownloadedByContactId).HasColumnName("DownloadedByContactID");

                entity.Property(e => e.ExtranetClientStatusId)
                    .HasColumnName("ExtranetClientStatusID")
                    .HasComment("Represents a possible status which will be exposed to an external client using the Extranet. This is used to determine whether an item shows up as complete/downloadable, etc., in Extranet web interface.");

                entity.Property(e => e.ExtranetSignoffComment).HasComment("When signing off an item being reviewed in the Extranet, a client reviewer can leave an overall free-text comment about the translation, which is stored here. In future we might also support translation itself via the Extranet, in which case a translator could add a comment when submitting their translation.");

                entity.Property(e => e.FileName).HasComment("For any job involving a file (i.e. usually anything apart from interpreting), this is the name of the file in question.");

                entity.Property(e => e.InternalPgdapprovalByEmployeeId).HasColumnName("InternalPGDApprovalByEmployeeID");

                entity.Property(e => e.InternalPgdapprovalDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("InternalPGDApprovalDateTime");

                entity.Property(e => e.InternalPgdapprovalNotes).HasColumnName("InternalPGDApprovalNotes");

                entity.Property(e => e.InterpretingActualDurationMinutes).HasComment("For an interpreting assignment, after the event, how many minutes (normally a rounded multiple of 60/30) the assignment was deemed to take for the purposes of billing (e.g. an interpreter may have been booked for 1 hour but it ended up taking 1.5 hours = 90 minutes)");

                entity.Property(e => e.InterpretingExpectedDurationMinutes).HasComment("For an interpreting job, the number of minutes (normally a multiple of 60/30/15) which we expect (when the client booked the assignment) for the duration of the assignment.");

                entity.Property(e => e.InterpretingLocationAddress1)
                    .HasMaxLength(100)
                    .HasComment("Address line 1 of where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationAddress2)
                    .HasMaxLength(100)
                    .HasComment("Address line 2 of where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationAddress3)
                    .HasMaxLength(100)
                    .HasComment("Address line 3 of where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationAddress4)
                    .HasMaxLength(100)
                    .HasComment("Address line 4 of where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationCountryId)
                    .HasColumnName("InterpretingLocationCountryID")
                    .HasComment("ID of the country (from Countries) where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationCountyOrState)
                    .HasMaxLength(100)
                    .HasComment("County/state/equivalent of where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationOrgName)
                    .HasMaxLength(200)
                    .HasComment("The name of the organisation where an interpreting assignment will take place. This may be a different place/org to the main site of the org which has ordered/is paying for an interpreter.");

                entity.Property(e => e.InterpretingLocationPostcodeOrZip)
                    .HasMaxLength(20)
                    .HasComment("Postcode/zip code/equivalent of where an interpreting assignment will take place");

                entity.Property(e => e.IsVisibleToClient).HasComment("True/1 if this item will be visible to an external client viewing a breakdown within our extranet or a report. False/0 if we want to track it but not show it to the client; e.g. we may apply a separate proofread phase for which we want to track the item and pay a supplier, but we may not wish to expose that to the client if we're not charging them and they have not separately asked for it.");

                entity.Property(e => e.JobOrderId)
                    .HasColumnName("JobOrderID")
                    .HasComment("The ID (from JobOrders) which this job item is attached to. All items must have a \"parent\" JobOrder ID.");

                entity.Property(e => e.LanguageServiceId)
                    .HasColumnName("LanguageServiceID")
                    .HasComment("The ID (from LanguageServices) representing what kind of activity is involved, e.g. whether this is translation, interpreting, transcription, etc.");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("The ID of the employee who last updated the item details");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the item details were last updated");

                entity.Property(e => e.LinguisticSupplierOrClientReviewerId)
                    .HasColumnName("LinguisticSupplierOrClientReviewerID")
                    .HasComment("The ID of EITHER the translator/interpreter (from LinguisticSuppliers) OR the client reviewer (from Contacts) to whom we assigned the work. The column SupplierIsClientReviewer determines which kind of ID it is.");

                entity.Property(e => e.MarginAfterDiscountSurcharges)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetMarginForJobItemsAfterDiscountSurcharges]([ID],[JobOrderID]))", false);

                entity.Property(e => e.OurCompletionDeadline)
                    .HasColumnType("datetime")
                    .HasComment("The date/time by when we should complete the item for the client (i.e. the delivery deadline for a translation, or the approx end of the assignment for interpreting). This is distinct from a deadline we set a supplier in terms of delivering to us.");

                entity.Property(e => e.Pages).HasComment("For a DTP job or other item where we might charge or pay by page, this captures the number of pages. Can be NULL if irrelevant.");

                entity.Property(e => e.PaymentToSupplier)
                    .HasColumnType("money")
                    .HasComment("The monetary value we are paying to the supplier for this item.");

                entity.Property(e => e.PaymentToSupplierCurrencyId)
                    .HasColumnName("PaymentToSupplierCurrencyID")
                    .HasComment("The ID (from Currencies) of the currency in which we are agreeing/making the payment to the supplier");

                entity.Property(e => e.PercentageOfChangedReviewSegments)
                    .HasColumnType("decimal(4, 1)")
                    .HasComputedColumnSql("([dbo].[funcGetPercentageOfModifiedSegments]([TotalNumberOfReviewSegments],[TotalNumberOfChangedReviewSegments]))", false);

                entity.Property(e => e.QualityRatingNotGivenReason).HasMaxLength(250);

                entity.Property(e => e.QualityRatingReminderDisabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.SourceLanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SourceLanguageIANAcode")
                    .HasComment("The source language (from Languages)");

                entity.Property(e => e.SupplierAcceptedWorkDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the supplier/client reviewer accepted the offer of carrying out the work represented by this job item. This field only needs to be used for jobs offered to suppliers via an external system such as the extranet, or for jobs which are being reviewed by nominated customer reviewers, who may (depending on their business processes and/or agreements with us) need to actively accept review requests (so we/the client want to track when the reviewer accepted the request)");

                entity.Property(e => e.SupplierCompletedItemDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the supplier has delivered the translation/transcription/etc. or has completed the assignment; NULL if it is still \"in progress\" or \"yet to happen\".");

                entity.Property(e => e.SupplierCompletionDeadline)
                    .HasColumnType("datetime")
                    .HasComment("The date/time by when the supplier should complete the item (i.e. the delivery deadline for a translation, or the approx end of the assignment for interpreting)");

                entity.Property(e => e.SupplierCostCalculatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.SupplierCostCalculatedById).HasColumnName("SupplierCostCalculatedByID");

                entity.Property(e => e.SupplierInvoicePaidDate)
                    .HasColumnType("datetime")
                    .HasComment("The date when we actually paid the amount to the supplier (i.e. when their invoice for this job item was settled, after job completion). A date is captured only (time is not signficant, so will always be 00:00).");

                entity.Property(e => e.SupplierInvoicePaidMethodId)
                    .HasColumnName("SupplierInvoicePaidMethodID")
                    .HasComment("The ID of the method (from PaymentMethods) by which we paid the amount to the supplier (i.e. how their invoice for this job item was settled, after job completion)");

                entity.Property(e => e.SupplierIsClientReviewer).HasComment("True if the person to whom this job item has been assigned is a client reviewer at the customer, rather than a translator/interpreter/other supplier");

                entity.Property(e => e.SupplierRatingsGivenDateTime).HasColumnType("datetime");

                entity.Property(e => e.SupplierSentWorkDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when we sent the translation/transcription file/brief to the supplier, or when we notified a supplier about an interpreting assignment. For any deadline timing measurement, this can be used to measure when the job \"started\" as far as the supplier is concerned. If NULL, we haven't yet told the supplier to start.");

                entity.Property(e => e.SupplierWordCountExact).HasComment("If a translation job, the number of exact match words from the translation memory (can be NULL if not a translation job)");

                entity.Property(e => e.SupplierWordCountFuzzyBand1).HasComment("If a translation job, the number of translation memory fuzzy match words in Band 1 (can be NULL if not a translation job). At the moment, there is only 1 fuzzy band -- I am providing for future compatibility when we may need to show a more detailed breakdown of fuzzy percentages.");

                entity.Property(e => e.SupplierWordCountPerfectMatches).HasComment("If a translation job, the number of PerfectMatch (aka XTranslated/In-Context Exact) match words from the translation memory.");

                entity.Property(e => e.SupplierWordCountRepetitions).HasComment("If a translation job, the number of repetition match words from the translation memory (can be NULL if not a translation job)");

                entity.Property(e => e.SupplierWordCountsTakenFromClient).HasDefaultValueSql("((1))");

                entity.Property(e => e.TargetLanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("TargetLanguageIANAcode")
                    .HasComment("The target language (from Languages). Each individual item can have only one target language (for a \"job\" with multiple target languages, it will have multiple JobItem records attached to a single JobOrder).");

                entity.Property(e => e.TranslationMemoryRequired).HasComment("Number indicating whether or not translation memory needs to be used for this job. At present numbers are referenced directy; if other categories need to be established, we might link descriptions of the categories to another table. For now, 0 = False, 1 = True, 2 = Not Known (Historical), 3 = Not Applicable, 4 = True (TagEditor is required), 5 = True (Trados 2009 or 2011 is required); 6 = True (Across is required).");

                entity.Property(e => e.WeCompletedItemDateTime)
                    .HasColumnType("datetime")
                    .HasComment("Date/time when we delivered the file to the client (as distinct to the supplier delivering it to us). NULL if it is still \"in progress\" as far as the client is concerned (whether or not we may have received the work from the supplier). For interpreting assignments, this should be set only when we have received the signed timesheet from the supplier. If this is NULL, the item cannot be invoiced to the client.");

                entity.Property(e => e.WebServiceClientStatusId)
                    .HasColumnName("WebServiceClientStatusID")
                    .HasComment("Represents a possible status which will be exposed to an external client calling the Web Service. This is kept distinct from ExtranetStatus since if someone manually downloads a file via the Extranet, we don't want this to prevent that file being collected via the Web Service.");

                entity.Property(e => e.WordCountExact).HasComment("If a translation job, the number of exact match words from the translation memory (can be NULL if not a translation job)");

                entity.Property(e => e.WordCountFuzzyBand1).HasComment("If a translation job, the number of translation memory fuzzy match words in Band 1 (can be NULL if not a translation job). At the moment, there is only 1 fuzzy band -- I am providing for future compatibility when we may need to show a more detailed breakdown of fuzzy percentages.");

                entity.Property(e => e.WordCountNew).HasComment("If a translation job, the number of new words (can be NULL if not a translation job)");

                entity.Property(e => e.WordCountPerfectMatches).HasComment("If a translation job, the number of PerfectMatch (aka XTranslated/In-Context Exact) match words from the translation memory.");

                entity.Property(e => e.WordCountRepetitions).HasComment("If a translation job, the number of repetition match words from the translation memory (can be NULL if not a translation job)");

                entity.Property(e => e.WorkMinutes).HasComment("For any activity (e.g. proofreading, testing) where we charge/pay based on a number of working hours/minutes, this stores the number of minutes agreed (normally a multiple of 60/30)");
            });

            modelBuilder.Entity<JobItemPgddetail>(entity =>
            {
                entity.ToTable("JobItemPGDDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AirDate).HasColumnType("datetime");

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.Markets).IsRequired();

                entity.Property(e => e.Service)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.UsageEndDate).HasColumnType("datetime");

                entity.Property(e => e.UsageStartDate).HasColumnType("datetime");

                entity.Property(e => e.UsageType).HasMaxLength(1000);

                entity.Property(e => e.Votalent)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("VOTalent");
            });

            modelBuilder.Entity<JobOrder>(entity =>
            {
                entity.HasComment("Orders from clients, which represent actual work going/gone ahead");

                entity.HasIndex(e => new { e.Id, e.DeletedDate, e.ContactId, e.JobName }, "_dta_index_JobOrders_12_1496652675__K1_K30_K2_K5");

                entity.HasIndex(e => new { e.DeletedDate, e.ContactId, e.SubmittedDateTime, e.Id, e.ClientCurrencyId, e.ClientInvoiceId }, "_dta_index_JobOrders_12_187199767__K27_K2_K11_K1_K14_K15_5_8_12");

                entity.HasIndex(e => new { e.DeletedDate, e.ContactId, e.SubmittedDateTime, e.IsActuallyOnlyAquote, e.Id }, "_dta_index_JobOrders_12_187199767__K27_K2_K11_K30_K1");

                entity.HasIndex(e => new { e.ContactId, e.SubmittedDateTime, e.DeletedDate, e.ClientInvoiceId, e.Id }, "_dta_index_JobOrders_12_187199767__K2_K11_K27_K15_K1_14");

                entity.HasIndex(e => new { e.ContactId, e.ClientInvoiceId, e.OverallCompletedDateTime, e.DeletedDate, e.ClientCurrencyId }, "_dta_index_JobOrders_12_187199767__K2_K15_K13_K27_K14_1_4");

                entity.HasIndex(e => new { e.ContactId, e.Id }, "_dta_index_JobOrders_9_1945643316__K2_K1");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Gives the status ID of the job on Extranet, 0 - Job fully completed, ");

                entity.Property(e => e.AnticipatedFinalValueAmount)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetAnticipatedOverallChargeToClientForJobOrderID]([ID]))", false);

                entity.Property(e => e.AnticipatedGrossMarginPercentage)
                    .HasColumnType("decimal(38, 4)")
                    .HasComputedColumnSql("([dbo].[funcGetAnticipatedGrossMarginPercentageForJobOrderID]([ID]))", false);

                entity.Property(e => e.ArchivedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ArchivedToAmazonS3dateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("ArchivedToAmazonS3DateTime");

                entity.Property(e => e.ArchivedToLionBoxDateTime).HasColumnType("datetime");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ClientApprovalRequirementStatus).HasComment("A number indicating whether (and if so, potentially why) this job order requires or required approval to proceed from the client. Typically this will be either 0 (no specific approval is/was required other than a normal request to go ahead) or 1 (it had to be/has to be specifically approved because the value exceeds an agreed threshold)");

                entity.Property(e => e.ClientApprovedByContactId)
                    .HasColumnName("ClientApprovedByContactID")
                    .HasComment("If approval by the client is required for this order to proceed, this captures the ID of the contact who gave that approval. If NULL, then approval either has not yet been received, or is not required.");

                entity.Property(e => e.ClientApprovedByDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If approval by the client is required for this order to proceed, this captures the date/time when that approval was received. If NULL, then approval either has not yet been received, or is not required.");

                entity.Property(e => e.ClientCurrencyId)
                    .HasColumnName("ClientCurrencyID")
                    .HasComment("The ID of the currency (from Currencies) which will be used to invoice/describe costs to the client (which needs to be consistent across the constituent sub-jobs). This is independent from the currency/currencies we may use to pay/negotiate prices with the multiple suppliers on a job.");

                entity.Property(e => e.ClientInvoiceId)
                    .HasColumnName("ClientInvoiceID")
                    .HasComment("The ID (from ClientInvoices) of the invoice we issued to the client for this job. If NULL then we have not yet invoiced the client.");

                entity.Property(e => e.ClientNotes).HasComment("Any notes initially provided by (and visible to) the client, which could contain a brief/instructions/other notes pertinent to the job");

                entity.Property(e => e.ClientPonumber)
                    .HasMaxLength(100)
                    .HasColumnName("ClientPONumber")
                    .HasComment("If specified, a purchase order number provided by the client which would need to be shown on an invoice. May not literally be a number (could contain letters, etc.), hence an nvarchar");

                entity.Property(e => e.CommissionProcessedByEmployeeId).HasColumnName("CommissionProcessedByEmployeeID");

                entity.Property(e => e.CommissionProcessedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ContactId)
                    .HasColumnName("ContactID")
                    .HasComment("The ID of the contact (from Contacts) who placed this order. All other client information (company name, invoice address, etc.) is derived from the relationship with the contact.");

                entity.Property(e => e.CustomerSpecificField1Value).HasComment("Optional customer-specific field for capturing data about any\r\naspect of a job order (the human-readable meaning of this field is stored in Orgs.CustomerSpecificField1Name)");

                entity.Property(e => e.CustomerSpecificField2Value).HasComment("Optional customer-specific field for capturing data about any\r\naspect of a job order (the human-readable meaning of this field is stored in Orgs.CustomerSpecificField2Name)");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("The ID of the employee who deleted this order. If null, then the order has not been deleted and hence is a \"live\" job which will show up in user searches, etc.");

                entity.Property(e => e.DeletedDate)
                    .HasColumnType("datetime")
                    .HasComment("The date/time at which this order was deleted. If null, then the order has not been deleted and hence is a \"live\" job which will show up in user searches, etc.");

                entity.Property(e => e.DeletedFreeTextReason)
                    .HasMaxLength(500)
                    .HasComment("Unlike contacts, orgs, etc., it will be useful to track the reason why a live job is deleted since this should not happen often. At some stage this can be replaced with a lookup once we know what standard reasons might be.");

                entity.Property(e => e.DesignPlusFileId).HasColumnName("DesignPlusFileID");

                entity.Property(e => e.DiscountAmount)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetDiscountAmountForJobOrderID]([ID]))", false);

                entity.Property(e => e.DiscountId).HasColumnName("DiscountID");

                entity.Property(e => e.EarlyInvoiceByEmpId).HasColumnName("EarlyInvoiceByEmpID");

                entity.Property(e => e.EarlyInvoiceDateTime).HasColumnType("datetime");

                entity.Property(e => e.EndClientId).HasColumnName("EndClientID");

                entity.Property(e => e.EonfilesDeletedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("EONFilesDeletedDateTime");

                entity.Property(e => e.ExtranetJobOrderStatus).HasComputedColumnSql("([dbo].[funcGetJobOrderStatus]([ID]))", false);

                entity.Property(e => e.ExtranetNotifyClientReviewersOfDeliveries).HasComment("If True, then when job items are delivered via the extranet, not only will the requester be notified, but any client reviewer who worked on that language will also receive a notification. Put in place initially for Unibet, who wanted e.g. their Greek customer service agent (who does the review) to be notified once we deliver the Greek translation, in addition to the person who is ordering/paying for the work");

                entity.Property(e => e.FinalValueAdjustment).HasColumnType("decimal(38, 18)");

                entity.Property(e => e.GrossMarginPercentage)
                    .HasColumnType("decimal(38, 4)")
                    .HasComputedColumnSql("([dbo].[funcGetGrossMarginPercentageForJobOrderID]([ID]))", false);

                entity.Property(e => e.InternalHighLowMarginApprovedByEmployeeId).HasColumnName("InternalHighLowMarginApprovedByEmployeeID");

                entity.Property(e => e.InternalHighLowMarginApprovedDateTime).HasColumnType("datetime");

                entity.Property(e => e.InternalNotes).HasComment("Any notes for our internal use only, which will NOT be visible to clients (either via the extranet or on invoices)");

                entity.Property(e => e.IsAcmsproject)
                    .HasColumnName("IsACMSProject")
                    .HasComment("True/1 if this order is linked to the translation of content from a client's CMS data residing in one of the Linguistic Databases we maintain for them. Used to check if, for example, it is associated with rows from a Linguistic Databases table for translators to work online directly in the CMS via the extranet");

                entity.Property(e => e.IsActuallyOnlyAquote)
                    .HasColumnName("IsActuallyOnlyAQuote")
                    .HasComment("Interim solution (until full quoting system up and running) to indicate if something is actually a quote rather than a job");

                entity.Property(e => e.IsAtrialProject)
                    .HasColumnName("IsATrialProject")
                    .HasComment("True/1 if we are regarding this order as a \"test piece\" or \"trial project\" in which the client is testing out our services and deciding whether to proceed with translate plus based on the success of this project - i.e where we know it needs to follow special careful procedures because it will be especially closely scrutinised");

                entity.Property(e => e.IsCls).HasColumnName("IsCLS");

                entity.Property(e => e.IsHighlyConfidential).HasComment("True/1 if this order has been marked (normally by the client when submitting a job via the extranet) as highly confidential, in which case, despite other people's normal access levels, this order will only be downloadable on completion by the person who submitted it");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("Must contain a brief summary of what the job is, e.g. \"Product X packaging text\"");

                entity.Property(e => e.JobOrderChannelId)
                    .HasColumnName("JobOrderChannelID")
                    .HasComment("The ID of the channel (from JobOrderChannels) explaining how this order was requested by the client, e.g. by e-mail, by client's admin system, by our web system, etc.");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("The ID of the employee who last updated the order details");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the order details were last updated");

                entity.Property(e => e.LinkedJobOrderId)
                    .HasColumnName("LinkedJobOrderID")
                    .HasComment("If set, this represents the ID of another job order to which the current job order has some connection. Used for a client such as FLSmidth where we need to capture some relationship between a previously translated job and one for which the client subsequently issues another, separate, order for desktop publishing. Presently, no other information about the nature of the relationship is captured.");

                entity.Property(e => e.MachineTranslationEngineSelected).HasComment("1- Google, 2 - DeepL");

                entity.Property(e => e.OffHoldDateTime).HasColumnType("datetime");

                entity.Property(e => e.OnHoldDateTime).HasColumnType("datetime");

                entity.Property(e => e.OrgHfmcodeBs)
                    .HasMaxLength(50)
                    .HasColumnName("OrgHFMCodeBS");

                entity.Property(e => e.OrgHfmcodeIs)
                    .HasMaxLength(50)
                    .HasColumnName("OrgHFMCodeIS");

                entity.Property(e => e.OriginalOverallDeliveryDeadline).HasColumnType("datetime");

                entity.Property(e => e.OriginatedFromEnquiryId)
                    .HasColumnName("OriginatedFromEnquiryID")
                    .HasComputedColumnSql("([dbo].[funcGetOriginatedFromEnquiryID]([ID]))", false);

                entity.Property(e => e.OverallAnticipatedChargeToClientForMarginCalucation)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetAnticipatedOverallChargeToClientForJobOrderIDForMarginCalculation]([ID]))", false);

                entity.Property(e => e.OverallChargeToClient)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetOverallChargeToClientForJobOrderID]([ID]))", false);

                entity.Property(e => e.OverallChargeToClientForMarginCalucation)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetOverallChargeToClientForJobOrderIDForMarginCalculation]([ID]))", false);

                entity.Property(e => e.OverallCompletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time at which we set the whole order to be \"complete\". If an order is made up of multiple files/target languages, this is the date when the last one was delivered. If NULL, the job is still live.");

                entity.Property(e => e.OverallDeliveryDeadline)
                    .HasColumnType("datetime")
                    .HasComment("The final delivery date/time in GMT when the client says the job must be completed. If they have different priorities for different languages/files, these are specified at lower job levels; this will be the latest of all individual deadlines, so if the job is still live after this date/time then it will be regarded as late.");

                entity.Property(e => e.OverallSterlingPaymentToSuppliers)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetOverallSterlingPaymentToSuppliersForJobOrderID]([ID]))", false);

                entity.Property(e => e.OverallSterlingPaymentToSuppliersForMarginCalc)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetOverallSterlingPaymentToSuppliersForJobOrderIDForMarginCalculation]([ID]))", false);

                entity.Property(e => e.OverdueCommentUpdate).HasColumnType("datetime");

                entity.Property(e => e.PreTranslateFromTemplateId).HasColumnName("PreTranslateFromTemplateID");

                entity.Property(e => e.Priority).HasComment("0 - not set, 1 - High, 2 - Medium and 3 - Low");

                entity.Property(e => e.ProjectManagerEmployeeId)
                    .HasColumnName("ProjectManagerEmployeeID")
                    .HasComment("The ID of the employee (from Employees) who is the main project management contact for this job. Will be used to identify who to speak to for any internal questions, and for auto-notification e-mails.");

                entity.Property(e => e.SaveTranslationsToTemplateId).HasColumnName("SaveTranslationsToTemplateID");

                entity.Property(e => e.SetupByEmployeeId)
                    .HasColumnName("SetupByEmployeeID")
                    .HasComment("Unlike other tables, there is no \"CreatedByEmployeeID\" because often the job will be created in the database by a client using the extranet/Web Service, in which case this field would be  NULL. However, where we've received work by e-mail/FTP etc., this field tracks the internal employee who entered details into the system.");

                entity.Property(e => e.SubTotalOverallChargeToClient)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetSubTotalOverallChargeToClientForJobOrderID]([ID]))", false);

                entity.Property(e => e.SubmittedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The GMT date and time when the order was submitted by the client. For anything coming via an automated method (extranet or Web Service), this will be literally when they submitted it. For anything non-automated, this will be the date/time when we created the record internally, which in theory could be minutes or hours after we received an e-mail or phone call from the client requesting work.");

                entity.Property(e => e.SurchargeAmount)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetSurchargeAmountForJobOrderID]([ID]))", false);

                entity.Property(e => e.SurchargeId).HasColumnName("SurchargeID");
            });

            modelBuilder.Entity<JobOrderAutomationDatum>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AddClientCostsDate).HasColumnType("datetime");

                entity.Property(e => e.AddSupplierCostsDate).HasColumnType("datetime");

                entity.Property(e => e.AnalyseSourceDate).HasColumnType("datetime");

                entity.Property(e => e.AssignedSupplierDate).HasColumnType("datetime");

                entity.Property(e => e.AutoDeliveryDate).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.JobSetUpDate).HasColumnType("datetime");

                entity.Property(e => e.SendForClientReviewDate).HasColumnType("datetime");

                entity.Property(e => e.SendToSupplierDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateMemoriesDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<JobOrderAutomationSetting>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OrgId).HasColumnName("OrgID");
            });

            modelBuilder.Entity<JobOrderChannel>(entity =>
            {
                entity.HasComment("Allows us to categorise the channel through which orders/jobs were requested from the client, e.g. by e-mail, by client's admin system, by our web system, etc.");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The description of the channel of an order (currently internal only, so only in English)");
            });

            modelBuilder.Entity<JobOrderPgddetail>(entity =>
            {
                entity.ToTable("JobOrderPGDDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApprovedEndClientCharge).HasColumnType("money");

                entity.Property(e => e.ApprovedEndClientChargeGbp)
                    .HasColumnType("money")
                    .HasColumnName("ApprovedEndClientChargeGBP");

                entity.Property(e => e.Bshfmnumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("BSHFMNumber");

                entity.Property(e => e.CampaignName).HasMaxLength(1000);

                entity.Property(e => e.EndClientChargeCurrencyId).HasColumnName("EndClientChargeCurrencyID");

                entity.Property(e => e.EndClientContact).HasMaxLength(1000);

                entity.Property(e => e.EndClientName).HasMaxLength(1000);

                entity.Property(e => e.Icponumber)
                    .HasMaxLength(1000)
                    .HasColumnName("ICPONumber");

                entity.Property(e => e.Isnumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("ISNumber");

                entity.Property(e => e.JobOrderId).HasColumnName("JobOrderID");

                entity.Property(e => e.ProductionContact).HasMaxLength(1000);

                entity.Property(e => e.ProjectStatus)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.ThirdPartyId)
                    .HasMaxLength(1000)
                    .HasColumnName("ThirdPartyID");

                entity.Property(e => e.Variance)
                    .HasColumnType("decimal(38, 2)")
                    .HasComputedColumnSql("([dbo].[funcGetVarianceJobOrderID]([JobOrderID]))", false);
            });

            modelBuilder.Entity<JobSubmissionTemplate>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CostCenter).HasMaxLength(200);

                entity.Property(e => e.CreatedByContactId).HasColumnName("CreatedByContactID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.CustomField1Value).HasMaxLength(500);

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.DeadlineTimeZone).HasMaxLength(200);

                entity.Property(e => e.DeletedByContactId).HasColumnName("DeletedByContactID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.InvoiceApproverInitials).HasMaxLength(100);

                entity.Property(e => e.InvoiceApprovingEntity).HasMaxLength(500);

                entity.Property(e => e.LastModifiedByContactId).HasColumnName("LastModifiedByContactID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.ProjectNumber).HasMaxLength(200);

                entity.Property(e => e.PurchaseOrderNumber).HasMaxLength(500);

                entity.Property(e => e.RequiresDtp).HasColumnName("RequiresDTP");

                entity.Property(e => e.SourceLangIanacode)
                    .HasMaxLength(50)
                    .HasColumnName("SourceLangIANACode");

                entity.Property(e => e.TargetLangsIanacodes).HasColumnName("TargetLangsIANACodes");

                entity.Property(e => e.TemplateName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<JsonXmlnodeMapping>(entity =>
            {
                entity.ToTable("JsonXMLNodeMapping");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientNodeName).HasMaxLength(200);

                entity.Property(e => e.ExportedDateTime).HasColumnType("datetime");

                entity.Property(e => e.JsonfileName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("JSONFileName");

                entity.Property(e => e.LastImportedDateTime).HasColumnType("datetime");

                entity.Property(e => e.TpxmlnodeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("TPXMLNodeName");

                entity.Property(e => e.XmlnodeClosingString)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("XMLNodeClosingString");

                entity.Property(e => e.XmlnodeString)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("XMLNodeString");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.Ianacode);

                entity.HasComment("A listing of all languages which could be potentially offered for any service, together with associated information");

                entity.Property(e => e.Ianacode)
                    .HasMaxLength(20)
                    .HasColumnName("IANAcode")
                    .HasComment("The IANA subtag, with combining region or script tags (where needed), derived from ISO codes. See http://www.iana.org/assignments/language-subtag-registry");

                entity.Property(e => e.AssociatedIanascript)
                    .HasMaxLength(20)
                    .HasColumnName("AssociatedIANAScript")
                    .HasComment("If known, this specifies the IANA script which should be associated by default for the language in question.");

                entity.Property(e => e.IsCommon).HasComment("1 means it is regarded as a common language; 0 means it is regarded as relatively rare (helps users to avoid seeing the full list of languages if applicable).");

                entity.Property(e => e.TpregionGroupingId)
                    .HasColumnName("TPRegionGroupingID")
                    .HasComment("Based on the LanguageRegions table, this indicates which geographical region the language is most associated with.");

                entity.Property(e => e.TpserviceRestrictId)
                    .HasColumnName("TPServiceRestrictID")
                    .HasComment("Where necessary this determines (based on LanguageServices table) if a language applies only to a particular service, e.g. if it is only relevant to written translation");

                entity.Property(e => e.TranslatableViaGoogleApi).HasColumnName("TranslatableViaGoogleAPI");
            });

            modelBuilder.Entity<LanguageCodesToUseInStudio>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LanguageCodesToUseInStudio");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LanguageIanacodeBeingDescribed).HasColumnName("LanguageIANAcodeBeingDescribed");

                entity.Property(e => e.LanguageIanacodeInStudio).HasColumnName("LanguageIANAcodeInStudio");
            });

            modelBuilder.Entity<LanguageCombinationsForTop5Supplier>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.SourceIanacode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("SourceIANACode");

                entity.Property(e => e.TargetIanacode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TargetIANACode");
            });

            modelBuilder.Entity<LanguageRateUnit>(entity =>
            {
                entity.HasComment("Units of charging rate which we would use for charging clients, or we would use for paying linguistic suppliers, e.g. per 1,000 words, per hour, etc.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of this rate type. Currently internal-only, so always in English");
            });

            modelBuilder.Entity<LanguageRegion>(entity =>
            {
                entity.HasComment("Used to group languages into the main geographical regions of the world with which they are associated. Allows, for example, in multi-select lists to only show languages for specific regions. Note that category Other is for development only.");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .HasComment("The id for this region");

                entity.Property(e => e.RegionNameEn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("RegionName-EN")
                    .HasComment("The English name for this region");
            });

            modelBuilder.Entity<LanguageService>(entity =>
            {
                entity.HasComment("Listing of the services we offer, intended to allow filtering of languages for certain service types (e.g. Simplified Chinese is a written language only, so should not be shown in a list relating to interpreting).");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.IncludeInMarginCalculations)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsForPgdonly).HasColumnName("IsForPGDOnly");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The English name of the service");
            });

            modelBuilder.Entity<LanguageSubjectArea>(entity =>
            {
                entity.HasComment("Subject areas (specialisms) which translators or interpreters may specialise in. Typically translators/interpreters will specialise in one or more particular subject areas.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of this subject area. Currently for internal use only, so always in English.");
            });

            modelBuilder.Entity<LikedEntity>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EntityId).HasColumnName("EntityID");

                entity.HasOne(d => d.Employee)
                    .WithMany()
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LikedEntities_Employees");

                entity.HasOne(d => d.Entity)
                    .WithMany()
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LikedEntities_BirthdayWishes");

                entity.HasOne(d => d.EntityNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.EntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LikedEntities_WorkAnniversariesWishes");
            });

            modelBuilder.Entity<LinguisticSupplier>(entity =>
            {
                entity.HasComment("Stores data on linguistic suppliers, typically translators, interpreters, DTP/multimedia, external agencies: anyone involved in the undertaking of jobs for clients.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address1).HasMaxLength(100);

                entity.Property(e => e.Address2).HasMaxLength(100);

                entity.Property(e => e.Address3).HasMaxLength(100);

                entity.Property(e => e.Address4).HasMaxLength(100);

                entity.Property(e => e.AgencyCompanyRegistrationNumber)
                    .HasMaxLength(50)
                    .IsFixedLength(true)
                    .HasComment("The registration number of the company if this supplier is an agency rather than an individual. This could be in any country so no way of knowing sensible maximum length/structure of the data, hence nvarchar(50).");

                entity.Property(e => e.AgencyNumberOfDtporMultimediaOps)
                    .HasColumnName("AgencyNumberOfDTPOrMultimediaOps")
                    .HasComment("If an agency then this stores the number of DTP operators or multimedia staff which the agency has on their books. Could be used either for us to calculate an overall figure of \"how many DTP operators we have access to including via agencies\", plus when assigning work to see if they may be able to take on a large capacity.");

                entity.Property(e => e.AgencyNumberOfLinguists).HasComment("If an agency then this stores the number of linguists which the agency has on their books. Could be used either for us to calculate an overall figure of \"how many linguists we have access to including via agencies\", plus when assigning work to see if they may be able to take on a large capacity.");

                entity.Property(e => e.AgencyOrTeamName)
                    .HasMaxLength(200)
                    .HasComment("If the supplier is an agency (rather than an individual), this will be the name of the agency. If the supplier is a group of freelances working together as a team, this will be a descriptive name of the team.");

                entity.Property(e => e.AgreedPaymentMethodId)
                    .HasColumnName("AgreedPaymentMethodID")
                    .HasComment("The ID of the payment method which we have agreed with the supplier as the \"normal\" way to pay them (based on their location, bank details, etc.)");

                entity.Property(e => e.ApplicationFormReceivedFromSupplierDate)
                    .HasColumnType("datetime")
                    .HasComment("When we received application details from this supplier (Null if we have not yet received anything)");

                entity.Property(e => e.ApplicationFormSentToSupplierDate)
                    .HasColumnType("datetime")
                    .HasComment("When we sent an application form/invitation to this supplier (Null if we have not yet done so)");

                entity.Property(e => e.ApprovedToAbeAddedtoDbbyEmpId).HasColumnName("ApprovedToABeAddedtoDBByEmpID");

                entity.Property(e => e.ApprovedToAbeAddedtoDbdatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("ApprovedToABeAddedtoDBDatetime");

                entity.Property(e => e.ContractUnlockedByEmployeeId).HasColumnName("ContractUnlockedByEmployeeID");

                entity.Property(e => e.ContractUnlockedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ContractUploadedByEmployeeId).HasColumnName("ContractUploadedByEmployeeID");

                entity.Property(e => e.ContractUploadedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CountryId)
                    .HasColumnName("CountryID")
                    .HasComment("ID of the country of the supplier's primary address. Links to Countries table.");

                entity.Property(e => e.CountyOrState).HasMaxLength(100);

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The ID of the employee who added the supplier to the database");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the supplier was added to the database");

                entity.Property(e => e.CurrencyId)
                    .HasColumnName("CurrencyID")
                    .HasComment("The ID of the currency (from Currencies) which we agree to deal with them in (i.e. they raise their invoices in this currency and we pay them in this currency)");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("The employee who deleted this supplier. If null, then the supplier has not been deleted and hence is a \"live\" supplier which will show up in user searches, etc.");

                entity.Property(e => e.DeletedDate)
                    .HasColumnType("datetime")
                    .HasComment("The date/time at which this supplier was deleted. If null, then the supplier has not been deleted and hence is a \"live\" supplier which will show up in user searches, etc.");

                entity.Property(e => e.EmailAddress).HasMaxLength(100);

                entity.Property(e => e.FaxCountryId)
                    .HasColumnName("FaxCountryID")
                    .HasComment("ID of the country corresponding to the fax number's international dialling prefix. Links to Countries table. This isn't automatically linked to the supplier's CountryID because they might be physically contactable on a fax line based in a different country.");

                entity.Property(e => e.FaxNumber).HasMaxLength(50);

                entity.Property(e => e.Ftpenabled).HasColumnName("FTPEnabled");

                entity.Property(e => e.GdpracceptanceText).HasColumnName("GDPRAcceptanceText");

                entity.Property(e => e.GdpracceptedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("GDPRAcceptedDateTime");

                entity.Property(e => e.GdpraccetedViaIplus).HasColumnName("GDPRAccetedViaIPlus");

                entity.Property(e => e.GdprrejectedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("GDPRRejectedDateTime");

                entity.Property(e => e.Gdprstatus)
                    .HasColumnName("GDPRStatus")
                    .HasComputedColumnSql("([dbo].[funcGetGDPRStatusOfALinguist]([ID]))", false);

                entity.Property(e => e.HasAccessToCar).HasComment("True if the supplier owns or has normal access to a car; may be useful for face-to-face interpreting assignments");

                entity.Property(e => e.IfBilingualOtherMotherTongueIanacode)
                    .HasMaxLength(20)
                    .HasColumnName("IfBilingualOtherMotherTongueIANACode")
                    .HasComment("If this supplier considers themselves bilingual then this is the other language (in addition to mother tongue) in which they consider themselves to be bilingual");

                entity.Property(e => e.IsAclientInternalLinguist)
                    .HasColumnName("IsAClientInternalLinguist")
                    .HasComment("True/1 if this supplier is not a freelance for translate plus, but an internal translator at a client (who wants to use our systems to allocate jobs internally). This means it can be filtered out of searches and/or reports on margins, etc. and limit when we should send out mailshots to, etc.");

                entity.Property(e => e.IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded)
                    .HasColumnName("IsQualityRatingPendingForAnyJobsIFLinguistIsNewlyAdded")
                    .HasComputedColumnSql("([dbo].[funcCheckIfQualityRatingOfAnyJobIsPendingForNewLinguist]([ID],[SupplierStatusID]))", false);

                entity.Property(e => e.IsQualityRatingPendingForFirst2Jobs).HasComputedColumnSql("([dbo].[funcCheckIfQualityRatingOfFirst2JobItemsIsPendingForALinguist]([ID]))", false);

                entity.Property(e => e.LastExportedToSageDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("The ID of the employee who last updated the supplier's details");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the supplier's details were last updated");

                entity.Property(e => e.MainContactDob)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("MainContactDOB")
                    .HasComment("The date of birth of the freelance individual, or of the main contact if an agency/team");

                entity.Property(e => e.MainContactFirstName)
                    .HasMaxLength(100)
                    .HasComment("The first (given) name of an individual linguist, or if an agency/team, then the firs name of the main contact");

                entity.Property(e => e.MainContactGender).HasComment("null = unknown/not applicable, 0 = male, 1 = female. If the supplier is an agency/team rather than an individual then this refers to the gender of the main contact");

                entity.Property(e => e.MainContactNationalityCountryId)
                    .HasColumnName("MainContactNationalityCountryID")
                    .HasComment("The ID of the country (from Countries) representing the nationality of the freelance/agency main contact");

                entity.Property(e => e.MainContactSurname)
                    .HasMaxLength(100)
                    .HasComment("The surname of an individual linguist, or if an agency/team, then the surname of the main contact");

                entity.Property(e => e.MainLandlineCountryId)
                    .HasColumnName("MainLandlineCountryID")
                    .HasComment("ID of the country corresponding to the main landline number's international dialling prefix. Links to Countries table. This isn't automatically linked to the supplier's CountryID because they might be physically contactable on a phone line based in a different country.");

                entity.Property(e => e.MainLandlineNumber)
                    .HasMaxLength(50)
                    .HasComment("The main landline number on which we should contact this supplier");

                entity.Property(e => e.MemoryRateFor50To74Percent)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the \"50-74% matches\" category. It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateFor75To84Percent)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the \"75-84% matches\" category. It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateFor85To94Percent)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the \"85-94% matches\" category. It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateFor95To99Percent)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the \"95-99% matches\" category. It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateForExactMatches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForPerfectMatches)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the PerfectMatches category (also known as XTranslate or In-Context Exact Matches). It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateForRepetitions)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the \"Exact (100%) and repetitions\" category. It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MobileCountryId)
                    .HasColumnName("MobileCountryID")
                    .HasComment("ID of the country corresponding to the mobile number's international dialling prefix. Links to Countries table. This isn't automatically linked to the supplier's CountryID because they might be physically contactable on a mobile based in a different country.");

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.MotherTongueLanguageIanacode)
                    .HasMaxLength(20)
                    .HasColumnName("MotherTongueLanguageIANACode")
                    .HasComment("The IANA code/subtag (from Languages) of this supplier's mother tongue (so we can establish which language combinations they should translate into, etc.)");

                entity.Property(e => e.NdaunlockedByEmployeeId).HasColumnName("NDAUnlockedByEmployeeID");

                entity.Property(e => e.NdaunlockedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("NDAUnlockedDateTime");

                entity.Property(e => e.NdauploadedByEmployeeId).HasColumnName("NDAUploadedByEmployeeID");

                entity.Property(e => e.NdauploadedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("NDAUploadedDateTime");

                entity.Property(e => e.NeedApprovalToBeAddedToDb).HasColumnName("NeedApprovalToBeAddedToDB");

                entity.Property(e => e.NonEeaclauseAcceptedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("NonEEAClauseAcceptedDateTime");

                entity.Property(e => e.NonEeaclauseDeclinedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("NonEEAClauseDeclinedDateTime");

                entity.Property(e => e.Notes).HasComment("Any textual notes about this supplier");

                entity.Property(e => e.OverallRating).HasComputedColumnSql("([dbo].[funcGetOverallRatingForALinguist]([ID]))", false);

                entity.Property(e => e.PostcodeOrZip).HasMaxLength(20);

                entity.Property(e => e.PrimaryOperatingSystemId)
                    .HasColumnName("PrimaryOperatingSystemID")
                    .HasComment("The ID (from OperatingSystems) of the primary OS this supplier uses");

                entity.Property(e => e.PrimaryOperatingSystemVersionId)
                    .HasColumnName("PrimaryOperatingSystemVersionID")
                    .HasComment("The ID (from OperatingSystemVersions) of the version of the primary OS this supplier uses");

                entity.Property(e => e.QualityRatingAvgForFirst2Jobs)
                    .HasColumnType("decimal(3, 2)")
                    .HasComputedColumnSql("([dbo].[funcGetAverageQualityRatingOfFirst2JobItemsForALinguist]([ID]))", false);

                entity.Property(e => e.RatesNotes).HasComment("A free-text field specifically for notes about the linguistic supplier's rates. This is to separate it from more general notes, to capture information which needs to be presented/acted on separately, which cannot be stored in a structured way, e.g. \"Supplier X will do work for 15% less if the job is for a charitable client but only if they are not busy doing something else, otherwise full rate\".");

                entity.Property(e => e.Referee1EmailAddress)
                    .HasMaxLength(100)
                    .HasComment("The e-mail address of the first referee this supplier provided.");

                entity.Property(e => e.Referee1FullAddress).HasComment("The full address (including country) of the first referee the supplier provided. We do not need to store this in a more structured way since we'll never need to search on it.");

                entity.Property(e => e.Referee1Name)
                    .HasMaxLength(200)
                    .HasComment("The name (could be a person or company) of the first referee this supplier provided");

                entity.Property(e => e.Referee1Phone)
                    .HasMaxLength(50)
                    .HasComment("The phone number (could be a landline or mobile) of the first referee this supplier provided. We don't need to store country prefix or anything more detailed as they will only be contacted as a one-off.");

                entity.Property(e => e.Referee2EmailAddress)
                    .HasMaxLength(100)
                    .HasComment("The e-mail address of the second referee this supplier provided.");

                entity.Property(e => e.Referee2FullAddress).HasComment("The full address (including country) of the second referee the supplier provided. We do not need to store this in a more structured way since we'll never need to search on it.");

                entity.Property(e => e.Referee2Name)
                    .HasMaxLength(200)
                    .HasComment("The name (could be a person or company) of the second referee this supplier provided");

                entity.Property(e => e.Referee2Phone)
                    .HasMaxLength(50)
                    .HasComment("The phone number (could be a landline or mobile) of the second referee this supplier provided. We don't need to store country prefix or anything more detailed as they will only be contacted as a one-off.");

                entity.Property(e => e.SapmasterDataReferenceNumber).HasColumnName("SAPMasterDataReferenceNumber");

                entity.Property(e => e.SecondaryEmailAddress)
                    .HasMaxLength(100)
                    .HasComment("An alternative e-mail address we can contact this supplier on");

                entity.Property(e => e.SecondaryEmailIsWorkAddress).HasComment("If this is 1 then it indicates that in the case of an individual freelancer, this is a work e-mail address, so that we perhaps need to take care when contacting them if they have a day-job, and carrying out freelance work for us is a sideline");

                entity.Property(e => e.SecondaryLandlineCountryId)
                    .HasColumnName("SecondaryLandlineCountryID")
                    .HasComment("ID of the country corresponding to the secondary landline number's international dialling prefix. Links to Countries table. This isn't automatically linked to the supplier's CountryID because they might be physically contactable on a phone line based in a different country.");

                entity.Property(e => e.SecondaryLandlineIsWorkNumber).HasComment("If this is 1 then it indicates that in the case of an individual freelancer, this is a work number, so that we perhaps need to take care when contacting them if they have a day-job, and carrying out freelance work for us is a sideline");

                entity.Property(e => e.SecondaryLandlineNumber)
                    .HasMaxLength(50)
                    .HasComment("An alternative landline number we can contact this supplier on");

                entity.Property(e => e.SkypeId)
                    .HasMaxLength(150)
                    .HasColumnName("SkypeID")
                    .HasComment("The Skype ID/user name of the linguistic supplier");

                entity.Property(e => e.SubjectAreaSpecialismsAsDescribedBySupplier).HasComment("Whilst we will store subject areas in a structured format alongisde rates, it may be useful to have a searchable free-text field for how they describe the subject areas they specialise in, so for example if we don't have a narrow enough subject area for \"pesticide products\" and store their subject area as \"agriculture\", it may be useful to search this text for the word \"pesticide\".");

                entity.Property(e => e.SupplierAttitudeRating).HasComputedColumnSql("([dbo].[funcGetAverageAttitudeRatingForALinguist]([ID]))", false);

                entity.Property(e => e.SupplierFollowingTheInstructionsRating).HasComputedColumnSql("([dbo].[funcGetAverageFollwoingTheInstructionsRatingForALinguist]([ID]))", false);

                entity.Property(e => e.SupplierOnTimeDeliveryRating).HasComputedColumnSql("([dbo].[funcGetAveragePunctualityRatingForALinguist]([ID]))", false);

                entity.Property(e => e.SupplierQualityOfWorkRating).HasComputedColumnSql("([dbo].[funcGetAverageQualityRatingForALinguist]([ID]))", false);

                entity.Property(e => e.SupplierResponsivenessRating).HasComputedColumnSql("([dbo].[funcGetAverageResponsivenessRatingForALinguist]([ID]))", false);

                entity.Property(e => e.SupplierSourceId)
                    .HasColumnName("SupplierSourceID")
                    .HasComment("Where we first got this supplier's details from (relates to LinguisticSupplierSources)");

                entity.Property(e => e.SupplierStatusId)
                    .HasColumnName("SupplierStatusID")
                    .HasComment("The current status of the supplier with regard to undertaking work, e.g. approved, banned, unknown, etc., from LinguisticSupplierStatus");

                entity.Property(e => e.SupplierTypeId)
                    .HasColumnName("SupplierTypeID")
                    .HasComment("The type of supplier, whether indivudal freelance, another agency, etc., from LinguisticSupplierTypes.");

                entity.Property(e => e.Vatnumber)
                    .HasMaxLength(50)
                    .HasColumnName("VATNumber")
                    .HasComment("The linguist's VAT number if they are registered for VAT. (If this field is populated then we use it to determine for our VAT reporting which supplier payments will have had VAT applied.)");

                entity.Property(e => e.WebAddress)
                    .HasMaxLength(200)
                    .HasComment("If the supplier has their own website, it can be useful to refer to it for any additional contact/other info in the future");

                entity.Property(e => e.WouldSignWitnessStatement).HasComment("If 1 then the supplier would be happy in principle to sign a witness statement if needed for a particular job");
            });

            modelBuilder.Entity<LinguisticSupplierInvoice>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address1ToShowOnInvoice).HasMaxLength(100);

                entity.Property(e => e.Address2ToShowOnInvoice).HasMaxLength(100);

                entity.Property(e => e.Address3ToShowOnInvoice).HasMaxLength(100);

                entity.Property(e => e.Address4ToShowOnInvoice).HasMaxLength(100);

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BankAccountIban)
                    .HasMaxLength(40)
                    .HasColumnName("BankAccountIBAN");

                entity.Property(e => e.BankAccountNumber).HasMaxLength(50);

                entity.Property(e => e.BankAccountRtnnumber)
                    .HasMaxLength(20)
                    .HasColumnName("BankAccountRTNNumber");

                entity.Property(e => e.BankAccountSortCode).HasMaxLength(20);

                entity.Property(e => e.BankAccountSwiftorBic)
                    .HasMaxLength(40)
                    .HasColumnName("BankAccountSWIFTorBIC");

                entity.Property(e => e.BankBranchAddress).HasMaxLength(300);

                entity.Property(e => e.BankBranchCity).HasMaxLength(50);

                entity.Property(e => e.BankBranchCountry).HasMaxLength(100);

                entity.Property(e => e.BankBranchName).HasMaxLength(150);

                entity.Property(e => e.BankBranchPostCode).HasMaxLength(50);

                entity.Property(e => e.CountryToShowOnInvoice).HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DiscountValue).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.ExportedToSageDateTime).HasColumnType("datetime");

                entity.Property(e => e.InvoiceCurrencyId).HasColumnName("InvoiceCurrencyID");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceLangIanacode)
                    .HasMaxLength(10)
                    .HasColumnName("InvoiceLangIANAcode");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LinguisticSupplierId).HasColumnName("LinguisticSupplierID");

                entity.Property(e => e.LinguisticSupplierInvoiceNumber).HasMaxLength(100);

                entity.Property(e => e.LinguisticSupplierName).HasMaxLength(150);

                entity.Property(e => e.NameToShowOnInvoice).HasMaxLength(200);

                entity.Property(e => e.PaidDate).HasColumnType("datetime");

                entity.Property(e => e.PayPalId)
                    .HasMaxLength(40)
                    .HasColumnName("PayPalID");

                entity.Property(e => e.SkrillId)
                    .HasMaxLength(40)
                    .HasColumnName("SkrillID");

                entity.Property(e => e.SubTotalValue).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.SupplierInvoiceHoldDateTime).HasColumnType("datetime");

                entity.Property(e => e.SupplierInvoiceOffHoldDateTime).HasColumnType("datetime");

                entity.Property(e => e.TotalValue).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.Vatnumber)
                    .HasMaxLength(50)
                    .HasColumnName("VATNumber");

                entity.Property(e => e.Vatrate).HasColumnName("VATRate");

                entity.Property(e => e.Vatvalue)
                    .HasColumnType("decimal(16, 4)")
                    .HasColumnName("VATValue");
            });

            modelBuilder.Entity<LinguisticSupplierInvoiceEarlyPaymentOption>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Discount).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.IanalanguageCode)
                    .HasMaxLength(5)
                    .HasColumnName("IANALanguageCode");
            });

            modelBuilder.Entity<LinguisticSupplierInvoiceJobItem>(entity =>
            {
                entity.HasIndex(e => e.JobItemId, "LinguisticSupplierInvoiceJobItemsIndex");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<LinguisticSupplierInvoiceTemplate>(entity =>
            {
                entity.ToTable("LinguisticSupplierInvoiceTemplate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address1ToShowOnInvoice).HasMaxLength(100);

                entity.Property(e => e.Address2ToShowOnInvoice).HasMaxLength(100);

                entity.Property(e => e.Address3ToShowOnInvoice).HasMaxLength(100);

                entity.Property(e => e.Address4ToShowOnInvoice).HasMaxLength(100);

                entity.Property(e => e.BankAccountIban)
                    .HasMaxLength(40)
                    .HasColumnName("BankAccountIBAN");

                entity.Property(e => e.BankAccountNumber).HasMaxLength(50);

                entity.Property(e => e.BankAccountRtnnumber)
                    .HasMaxLength(20)
                    .HasColumnName("BankAccountRTNNumber");

                entity.Property(e => e.BankAccountSortCode).HasMaxLength(20);

                entity.Property(e => e.BankAccountSwiftorBic)
                    .HasMaxLength(40)
                    .HasColumnName("BankAccountSWIFTorBIC");

                entity.Property(e => e.BankBranchAddress).HasMaxLength(300);

                entity.Property(e => e.BankBranchCity).HasMaxLength(50);

                entity.Property(e => e.BankBranchCountry).HasMaxLength(100);

                entity.Property(e => e.BankBranchName).HasMaxLength(150);

                entity.Property(e => e.BankBranchPostCode).HasMaxLength(50);

                entity.Property(e => e.ClientPonumber)
                    .HasMaxLength(100)
                    .HasColumnName("ClientPONumber");

                entity.Property(e => e.CompanyName).HasMaxLength(200);

                entity.Property(e => e.CompanyRegistrationNumber).HasMaxLength(20);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.InvoiceCurrencyId).HasColumnName("InvoiceCurrencyID");

                entity.Property(e => e.InvoiceLangIanacode)
                    .HasMaxLength(10)
                    .HasColumnName("InvoiceLangIANAcode");

                entity.Property(e => e.LinguisticSupplierId).HasColumnName("LinguisticSupplierID");

                entity.Property(e => e.LinguisticSupplierName).HasMaxLength(150);

                entity.Property(e => e.NameToShowOnInvoice).HasMaxLength(200);

                entity.Property(e => e.PayPalId)
                    .HasMaxLength(100)
                    .HasColumnName("PayPalID");

                entity.Property(e => e.SkrillId)
                    .HasMaxLength(40)
                    .HasColumnName("SkrillID");

                entity.Property(e => e.TemplateName).HasMaxLength(50);

                entity.Property(e => e.Vatnumber)
                    .HasMaxLength(50)
                    .HasColumnName("VATNumber");

                entity.Property(e => e.Vatrate).HasColumnName("VATRate");
            });

            modelBuilder.Entity<LinguisticSupplierRate>(entity =>
            {
                entity.HasComment("The rates which linguistic suppliers say they charge as standard for particular language combinations and subject areas (to assist us in looking up suppliers who could work on a particular job)");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppliesToDataObjectId)
                    .HasColumnName("AppliesToDataObjectID")
                    .HasComment("If NULL, this supplier rate applies for work with any client of translate plus. If this is set then (together with AppliesToDataObjectTypeID) this determines that the supplier has agreed a specific rate for a particular end-client - for example, if for a particularly high-volume or low-complexity client type, they have agreed to charge us less compared to \"normal\" jobs.");

                entity.Property(e => e.AppliesToDataObjectTypeId)
                    .HasColumnName("AppliesToDataObjectTypeID")
                    .HasComment("If NULL, this supplier rate applies for work with any client of translate plus. If this is set then (together with AppliesToDataObjectID) this determines that the supplier has agreed a specific rate for a particular end-client - for example, if for a particularly high-volume or low-complexity client type, they have agreed to charge us less compared to \"normal\" jobs.");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The ID of the employee who added the rate to the database");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the rate was added to the database");

                entity.Property(e => e.CurrencyId)
                    .HasColumnName("CurrencyID")
                    .HasComment("The ID (from Currencies) of the currency in which this rate is expressed");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("The employee who deleted this rate. If null, then the rate has not been deleted and hence is a \"live\" rate which will show up in user searches, etc.");

                entity.Property(e => e.DeletedDate)
                    .HasColumnType("datetime")
                    .HasComment("The date/time at which this rate was deleted. If null, then the rate has not been deleted and hence is a \"live\" rate which will show up in user searches, etc.");

                entity.Property(e => e.LanguageServiceId)
                    .HasColumnName("LanguageServiceID")
                    .HasComment("The ID (from LanguageServices) of the linguistic service this rate relates to (e.g. translation, interpreting, etc.)");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("The ID of the employee who last updated the rate details");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the rate was last updated");

                entity.Property(e => e.MemoryRateFor50To74Percent)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the \"50-74% matches\" category. It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateFor75To84Percent)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the \"75-84% matches\" category. It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateFor85To94Percent)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the \"85-94% matches\" category. It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateFor95To99Percent)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the \"95-99% matches\" category. It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateForClientSpecificPercent).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForExactMatches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForPerfectMatches)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the PerfectMatches category (also known as XTranslate or In-Context Exact Matches). It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MemoryRateForRepetitions)
                    .HasColumnType("decimal(4, 1)")
                    .HasComment("If the supplier offers a reduction for the use of translation memory software, this represents the % of their standard rates which they charge for the \"Exact (100%) and repetitions\" category. It is a % of their standard rate, NOT a % discount off their standard rate. (E.g. if they charge one-third, this should be 33% and not 67%.)");

                entity.Property(e => e.MinimumCharge)
                    .HasColumnType("money")
                    .HasComment("A minimum charge (in the currency specified by CurrencyID) which the supplier may apply for this combination/subject area. If they don't apply one, this will be set to zero (rather than null).");

                entity.Property(e => e.RateUnitId)
                    .HasColumnName("RateUnitID")
                    .HasComment("The ID (from LinguisticSupplierRateTypes) of the unit of charging being expressed (e.g. per 1,000 words, per hour, etc.)");

                entity.Property(e => e.RatesNotes).HasComment("A free-text field specifically for notes about the linguistic supplier's rates. This is to separate it from more general notes, to capture information which needs to be presented/acted on separately, which cannot be stored in a structured way, e.g. \"Supplier X will do work for 15% less if the job is for a charitable client but only if they are not busy doing something else, otherwise full rate\".");

                entity.Property(e => e.SourceLangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SourceLangIANAcode")
                    .HasComment("The source language code/subtag (from Languages) for this rate");

                entity.Property(e => e.StandardRate)
                    .HasColumnType("money")
                    .HasComment("The monetary rate (in the currency specified by CurrencyID)");

                entity.Property(e => e.StandardRateSterlingEquivalent)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcCurrencyConversion]([CurrencyID],(4),[StandardRate]))", false)
                    .HasComment("A computation displaying the value of the StandardRate column expressed in pounds sterling (GBP), used for cross-currency rate comparisons. If CurrencyID is 4 (pounds sterling) already then this will be the same value as StandardRate; otherwise it will be a converted rate.");

                entity.Property(e => e.SubjectAreaId)
                    .HasColumnName("SubjectAreaID")
                    .HasComment("The ID (from LanguageSubjectAreas) of the subject area to which this rate applies");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("SupplierID")
                    .HasComment("The ID of the supplier (from LinguisticSuppliers) to whom this rate/combination applies");

                entity.Property(e => e.TargetLangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("TargetLangIANAcode")
                    .HasComment("The target language code/subtag (from Languages) for this rate");
            });

            modelBuilder.Entity<LinguisticSupplierSoftwareApplication>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.LinguisticSupplierId).HasColumnName("LinguisticSupplierID");

                entity.Property(e => e.SoftwareApplicationId).HasColumnName("SoftwareApplicationID");
            });

            modelBuilder.Entity<LinguisticSupplierSource>(entity =>
            {
                entity.HasComment("Sources of where we may have initially got a supplier's details from, such as an online freelance resource, or a recommendation from another supplier, etc.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of this source (in English only since this is only for internal use)");
            });

            modelBuilder.Entity<LinguisticSupplierStatus>(entity =>
            {
                entity.ToTable("LinguisticSupplierStatus");

                entity.HasComment("Status levels for linguistic suppliers, for categorising/searching/being able to take on work");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of this status category");
            });

            modelBuilder.Entity<LinguisticSupplierType>(entity =>
            {
                entity.HasComment("Types of linguistic supplier for searching/categorising. English-only as only for internal use.");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The name of this supplier category");
            });

            modelBuilder.Entity<LinguistsAltairDetail>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AltairRegionId).HasColumnName("AltairRegionID");

                entity.Property(e => e.City).HasMaxLength(20);

                entity.Property(e => e.CompanyRegistrationNumber).HasMaxLength(200);

                entity.Property(e => e.Einnumber)
                    .HasMaxLength(200)
                    .HasColumnName("EINNumber");

                entity.Property(e => e.Gstnumber)
                    .HasMaxLength(200)
                    .HasColumnName("GSTNumber");

                entity.Property(e => e.Hstnumber)
                    .HasMaxLength(200)
                    .HasColumnName("HSTNumber");

                entity.Property(e => e.LinguistId).HasColumnName("LinguistID");

                entity.Property(e => e.Qstnumber)
                    .HasMaxLength(200)
                    .HasColumnName("QSTNumber");

                entity.Property(e => e.Sinnumber)
                    .HasMaxLength(200)
                    .HasColumnName("SINNumber");

                entity.Property(e => e.Sirennumber)
                    .HasMaxLength(200)
                    .HasColumnName("SIRENNumber");

                entity.Property(e => e.Siretnumber)
                    .HasMaxLength(200)
                    .HasColumnName("SIRETNumber");

                entity.Property(e => e.Ssnnumber)
                    .HasMaxLength(200)
                    .HasColumnName("SSNNumber");

                entity.Property(e => e.StreetNumber).HasMaxLength(10);

                entity.Property(e => e.Tinnumber)
                    .HasMaxLength(200)
                    .HasColumnName("TINNumber");
            });

            modelBuilder.Entity<LocalCountryInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LocalCountryInfo");

                entity.HasComment("Multilingual table of country names, which relates to the Countries and Languages tables");

                entity.HasIndex(e => new { e.LanguageIanacode, e.CountryId, e.CountryName }, "_dta_index_LocalCountryInfo_12_1493580359__K1_K2_K3");

                entity.Property(e => e.CountryId)
                    .HasColumnName("CountryID")
                    .HasComment("The country this row relates to, from the Countries table");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The local name of the country");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcode")
                    .HasComment("The IANA code (subtag) of the language this row relates to, from the Languages table");
            });

            modelBuilder.Entity<LocalCurrencyInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LocalCurrencyInfo");

                entity.HasComment("Multilingual table of currency names, which relates to the Currencies and Languages tables");

                entity.Property(e => e.CurrencyId)
                    .HasColumnName("CurrencyID")
                    .HasComment("The currency this row relates to, from the Currencies table");

                entity.Property(e => e.CurrencyName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The local name of the currency");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcode")
                    .HasComment("The IANA code (subtag) of the language this row relates to, fromthe Languages table");
            });

            modelBuilder.Entity<LocalDiscountAndSurchargeInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LocalDiscountAndSurchargeInfo");

                entity.Property(e => e.DiscountOrSurchargeId).HasColumnName("DiscountOrSurchargeID");

                entity.Property(e => e.DiscountOrSurchargeName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("LanguageIANACode");
            });

            modelBuilder.Entity<LocalExtranetAccessLevelInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LocalExtranetAccessLevelInfo");

                entity.HasComment("Multilingual table of names and descriptions, which relate to the ExtranetAccessLevels and Languages tables. For use e.g. in displaying within the extranet application the name and description of an access level in different languages depending on the currently selected UI language");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasComment("The local description of the access level");

                entity.Property(e => e.ExtranetAccessLevelId)
                    .HasColumnName("ExtranetAccessLevelID")
                    .HasComment("The access level this row relates to, from the ExtranetAccessLevels table");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcode")
                    .HasComment("The IANA code (subtag) of the language this row relates to, from the Languages table");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The local name of the access level");
            });

            modelBuilder.Entity<LocalJobOrderChannelInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LocalJobOrderChannelInfo");

                entity.HasComment("Multilingual table of job order channel names, which relates to the JobOrders and Languages tables. For use e.g. in displaying within the extranet application how a job was submitted, in different UI languages");

                entity.Property(e => e.JobOrderChannelId)
                    .HasColumnName("JobOrderChannelID")
                    .HasComment("The order channel this row relates to, from the JobOrderChannels table");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcode")
                    .HasComment("The IANA code (subtag) of the language this row relates to, from the Languages table");

                entity.Property(e => e.OrderChannelName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The local name of the job order channel");
            });

            modelBuilder.Entity<LocalLanguageInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LocalLanguageInfo");

                entity.HasComment("Multilingual table of language names (i.e. language names as written in a particular language, including English), which relates to the Languages table via the IANA codes (subtags)");

                entity.HasIndex(e => new { e.LanguageIanacode, e.LanguageIanacodeBeingDescribed, e.Name }, "_dta_index_LocalLanguageInfo_12_693577509__K1_K2_K3");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcode")
                    .HasComment("The IANA code (subtag) of the language this row relates to, from the Languages table. This indicates what language the data in the current row is written in - NOT which language is being described.");

                entity.Property(e => e.LanguageIanacodeBeingDescribed)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcodeBeingDescribed")
                    .HasComment("The IANA code (subtag) of the language which the data in this row relates to, from the Languages table. This indicates which language is being described, NOT which language the current row is written in.");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The name of the language being described (LanguageIANAcodeBeingDescribe), as written in the language of this row (LanguageIANAcode)");
            });

            modelBuilder.Entity<LocalLanguageRateUnitInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LocalLanguageRateUnitInfo");

                entity.HasComment("Multilingual table of names, which relate to the LanguageRateUnits and Languages tables. For use e.g. in displaying within the extranet application phrases like \"... per 1,000 words\" in different languages depending on the currently selected UI language");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcode")
                    .HasComment("The IANA code (subtag) of the language this row relates to, from the Languages table");

                entity.Property(e => e.LanguageRateUnitId)
                    .HasColumnName("LanguageRateUnitID")
                    .HasComment("The rate unit this row relates to, from the LanguageRateUnits table");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The local name of the rate unit");
            });

            modelBuilder.Entity<LocalLanguageServiceInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LocalLanguageServiceInfo");

                entity.HasComment("Multilingual table of language service names, which relates to the LanguageServices and Languages tables. For use e.g. in displaying within the extranet application or on invoices what service was ordered, in different UI languages");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcode")
                    .HasComment("The IANA code (subtag) of the language this row relates to, from the Languages table");

                entity.Property(e => e.LanguageServiceId)
                    .HasColumnName("LanguageServiceID")
                    .HasComment("The language service this row relates to, from the LanguageServices table");

                entity.Property(e => e.LanguageServiceName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The local name of the job order channel");
            });

            modelBuilder.Entity<LocalPlanningCalendarCategoriesInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LocalPlanningCalendarCategoriesInfo");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LangIANACode");

                entity.Property(e => e.PlanningCalendarCategoryId).HasColumnName("PlanningCalendarCategoryID");
            });

            modelBuilder.Entity<MarketingCampaign>(entity =>
            {
                entity.HasComment("Campaigns in the sense of a coordinated sending-out of e-mails and/or assignment of tasks to contact particular clients in bulk (e.g. advertising a special offer, or simply letting them know about us for the first time); can also be used for mailings to our linguists (e.g. to notify them about new procedures, etc.)");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The ID of the employee who first set up the details of the campaign (not who launched/sent out e-mails for it once finished)");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the campaign details were first set up (not when it was launched, sent out, etc.)");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasComment("A human-readable description (not visible to campaign recipients) for us to internally explain brief notes about what the campaign is for, etc. (e.g. \"To notify all clients in Switzerland about our new bank details for Swiss Francs\")");

                entity.Property(e => e.EmailFromEmailAddress)
                    .HasMaxLength(300)
                    .HasComment("If set, the e-mail address of the sender who e-mails will appear to be \"from\" as far as recipients are concerned - could be a real employee e-mail address, or something generic like \"accounts@translateplus.com\"");

                entity.Property(e => e.EmailFromName)
                    .HasMaxLength(300)
                    .HasComment("If set, the name of the sender who e-mails will appear to be \"from\" as far as recipients are concerned - could be a real employee name, or something generic like \"translate plus\" or \"translate plus accounts team\"");

                entity.Property(e => e.EmailHtmlbody)
                    .HasColumnName("EmailHTMLBody")
                    .HasComment("If set, an HTML string comprising the body of the e-mail which will be sent out");

                entity.Property(e => e.EmailHtmlsignature)
                    .HasColumnName("EmailHTMLSignature")
                    .HasComment("If set, the signature appearing at the end of the e-mail message. Could be a standard \"from translate plus\" signature, or for a specific sales person, for example.");

                entity.Property(e => e.EmailImageBinary).HasComment("If set, binary image data for a graphic which will appear in the body of the e-mail. In practice, a text code (such as {image}) will be substituted in the HTML body to create a URL link to a graphic generated via our extranet, crafted in a way to retrieve this binary image data while also capturing information about when the image was accessed, for campaign tracking purposes");

                entity.Property(e => e.EmailSubjectLine)
                    .HasMaxLength(300)
                    .HasComment("If set, the subject line of the e-mails which will be sent out. NULL if this campaign will not generate e-mails (e.g. if used only to log a physical mailing campaign)");

                entity.Property(e => e.InternalFollowUpTasksDueDateTime)
                    .HasColumnType("smalldatetime")
                    .HasComment("If set, the date (with no specific time of day) on which follow-up internal tasks should be set for action");

                entity.Property(e => e.InternalFollowUpTasksEmployeeId)
                    .HasColumnName("InternalFollowUpTasksEmployeeID")
                    .HasComment("If specified, the ID of the employee to whom follow-up tasks will be assigned in the intranet. Typical usage would be for sales people to make calls after an e-mail shot has gone out.");

                entity.Property(e => e.InternalFollowUpTasksInstructionNotes).HasComment("If set, the InstructionNotes (to be added in the Tasks table) for follow-up tasks in the intranet");

                entity.Property(e => e.InternalFollowUpTasksTypeId)
                    .HasColumnName("InternalFollowUpTasksTypeID")
                    .HasComment("The ID of the type of task (from the TaskTypes table) for what kind of follow-up tasks should be added to the intranet to follow up on the campaign; e.g. adding Call tasks to a sales person to ring up the recipients after e-mails have gone out. If this is NULL then no follow-up tasks will be created.");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LaunchedByEmployeeId)
                    .HasColumnName("LaunchedByEmployeeID")
                    .HasComment("The employee who set this campaign to be \"launched\", in the sense that after that time it is no longer draft, and will be a live campaign queued up for e-mails to be sent out, tasks to be created, etc.");

                entity.Property(e => e.LaunchedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when this campaign was set to be \"launched\", in the sense that after that time it is no longer draft, and will be a live campaign queued up for e-mails to be sent out, tasks to be created, etc.");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasComment("The name we give internally to the campaign (will not be made visible to recipients)");

                entity.Property(e => e.ProcessingCompletedBySystemDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time at which our systems finished automatically processing the campaign: sending out e-mails, assigning tasks, and so on. This would typically be at an out-of-hours time when the network is not busy and so could be hours or days after the LaunchedDateTime. Once this date/time is set, the campaign can be regarded as \"having been sent out\".");

                entity.Property(e => e.TypeId)
                    .HasColumnName("TypeID")
                    .HasComment("Currently a hard-coded designation: 0 = e-mail shot (generated by translate plus); 1 = e-mail shot (generated by external marketing provider); 2 = physical mailing; 3 = calls only (with no physical or electronic mailing); 4 = e-mail shot (generated by Amazon SES)");
            });

            modelBuilder.Entity<MarketingCampaignRecipient>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.FirstAccessedEmailImageDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<MarketingCampaignRecipientsNew>(entity =>
            {
                entity.ToTable("MarketingCampaignRecipients_New");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.FirstAccessedEmailImageDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<MiscResource>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LangIANACode");

                entity.Property(e => e.ResourceName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StringContent).IsRequired();
            });

            modelBuilder.Entity<NotificationRecipients>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("NotificationRecipients");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.NotificationTypeId).HasColumnName("NotificationTypeID");

                entity.Property(e => e.RecipientEmployeeId).HasColumnName("RecipientEmployeeID");
            });

            modelBuilder.Entity<NotificationTypes>(entity =>
            {
                entity.ToTable("NotificationTypes");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NotificationName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<OperatingSystem>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OperatingSystemVersion>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Osid).HasColumnName("OSID");
            });

            modelBuilder.Entity<OrderAndItemAuditLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OrderAndItemAuditLog");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Org>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.OrgGroupId, e.OrgName }, "_dta_index_Orgs_12_1463728317__K1_K31_K2");

                entity.HasIndex(e => new { e.Id, e.CountryId }, "_dta_index_Orgs_12_1463728317__K1_K9_2");

                entity.HasIndex(e => new { e.OrgGroupId, e.Id, e.OrgName }, "_dta_index_Orgs_12_1463728317__K31_K1_K2");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address1).HasMaxLength(255);

                entity.Property(e => e.Address2).HasMaxLength(100);

                entity.Property(e => e.Address3).HasMaxLength(100);

                entity.Property(e => e.Address4).HasMaxLength(100);

                entity.Property(e => e.ClientSpendCurrentFinancialYear)
                    .HasColumnType("money")
                    .HasComputedColumnSql("([dbo].[funcCurrentFinancialYearSpendByClient]([ID]))", false);

                entity.Property(e => e.ClientSpendLastFinancialYear)
                    .HasColumnType("money")
                    .HasComputedColumnSql("([dbo].[funcLastFinancialYearSpendByClient]([ID]))", false);

                entity.Property(e => e.ClientSpendOverLast12Months)
                    .HasColumnType("money")
                    .HasComputedColumnSql("([dbo].[funcGetSpendByClientOverLast12Months]([ID]))", false);

                entity.Property(e => e.ClientSpendOverLast3Months)
                    .HasColumnType("money")
                    .HasComputedColumnSql("([dbo].[funcGetSpendByClientOverLast3Months]([ID]))", false);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CountyOrState).HasMaxLength(100);

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerLogoImageBinary).HasComment("Binary image data containing a customer logo which can be displayed on their extranet homepage or potentially elsewhere");

                entity.Property(e => e.CustomerSpecificField1Name)
                    .HasMaxLength(100)
                    .HasComment("Optional customer-specific field name for capturing data about any aspect of a job order (used in the intranet and extranet as a label for displaying data from JobOrders.CustomerSpecificField1Value)");

                entity.Property(e => e.CustomerSpecificField2Name)
                    .HasMaxLength(100)
                    .HasComment("Optional customer-specific field name for capturing data about any aspect of a job order (used in the intranet and extranet as a label for displaying data from JobOrders.CustomerSpecificField2Value)");

                entity.Property(e => e.CustomerSpecificField3Name).HasMaxLength(100);

                entity.Property(e => e.CustomerSpecificField4Name).HasMaxLength(100);

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.DesignplusStatus)
                    .HasMaxLength(10)
                    .HasColumnName("designplusStatus");

                entity.Property(e => e.DesignplusTrialStarted)
                    .HasColumnType("datetime")
                    .HasColumnName("designplusTrialStarted");

                entity.Property(e => e.EarlyPaymentDiscount).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.EmailAddress).HasMaxLength(100);

                entity.Property(e => e.EmailUnsubscribedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ExcludeFromDirectMarketing).HasComment("If True, we should not actively call or e-mail-shot this org for direct marketing or sales activity");

                entity.Property(e => e.ExtranetCanSubmitFtpdetailsNotFiles)
                    .HasColumnName("ExtranetCanSubmitFTPDetailsNotFiles")
                    .HasComment("This is for clients that need to upload their files via our FTP site (e.g. due to file size) but still submit the order via the extranet. True/1 means this option is available/visible to them in the extranet application; False/0 means this option is not presented to them.");

                entity.Property(e => e.ExtranetDtpoptionSelectedByDefault)
                    .IsRequired()
                    .HasColumnName("ExtranetDTPOptionSelectedByDefault")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ExtranetIncludeClientReviewByDefault).HasComment("For clients who use the extranet, if this is True then anyone submitting a translation request will have the option of including client review in the workflow selected by default. If ExtranetShowClientReviewOptions is not True then this setting will have no effect.");

                entity.Property(e => e.ExtranetNotifyClientReviewersOfDeliveriesByDefault).HasComment("For clients who use the extranet, if this is True then anyone submitting a translation request will have the option of notifying the reviewers (when a job is complete which they have reviewed) selected by default. If ExtranetShowClientReviewOptions is not True then this setting will have no effect.");

                entity.Property(e => e.ExtranetSendClientReviewDeadlineReminders)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ExtranetShowClientReviewOptions).HasComment("For clients who use the extranet, if this is True then anyone submitting a translation request will be shown options about whether they want client review to be included as part of their workflow or not");

                entity.Property(e => e.ExtranetShowDtpoptions)
                    .IsRequired()
                    .HasColumnName("ExtranetShowDTPOptions")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ExtranetShowPonumberBox)
                    .IsRequired()
                    .HasColumnName("ExtranetShowPONumberBox")
                    .HasDefaultValueSql("((1))")
                    .HasComment("If 1/True (the default), show the Purchase Order Number box in the request services pages in the extranet. If 0/False, hide it.");

                entity.Property(e => e.FaxCountryId).HasColumnName("FaxCountryID");

                entity.Property(e => e.FaxNumber).HasMaxLength(50);

                entity.Property(e => e.FirstPaidJobDate)
                    .HasColumnType("datetime")
                    .HasComputedColumnSql("([dbo].[funcGetClientsFirstPaidJobDate]([ID]))", false);

                entity.Property(e => e.Ftpenabled).HasColumnName("FTPEnabled");

                entity.Property(e => e.HfmcodeBs)
                    .HasMaxLength(50)
                    .HasColumnName("HFMCodeBS");

                entity.Property(e => e.HfmcodeIs)
                    .HasMaxLength(50)
                    .HasColumnName("HFMCodeIS");

                entity.Property(e => e.InvoiceAddress1).HasMaxLength(100);

                entity.Property(e => e.InvoiceAddress2).HasMaxLength(100);

                entity.Property(e => e.InvoiceAddress3).HasMaxLength(100);

                entity.Property(e => e.InvoiceAddress4).HasMaxLength(100);

                entity.Property(e => e.InvoiceBlanketPonumber)
                    .HasMaxLength(100)
                    .HasColumnName("InvoiceBlanketPONumber")
                    .HasComment("A default PO number which will be added to any new invoices (though it can be overwritten when creating the invoice). To help with clients who issue a blanket PO covering a certain amount of money or time for use until further notice, etc.");

                entity.Property(e => e.InvoiceCountryId).HasColumnName("InvoiceCountryID");

                entity.Property(e => e.InvoiceCountyOrState).HasMaxLength(100);

                entity.Property(e => e.InvoiceCurrencyId).HasColumnName("InvoiceCurrencyID");

                entity.Property(e => e.InvoiceDefaultContactId)
                    .HasColumnName("InvoiceDefaultContactID")
                    .HasComment("If 0, by default any new invoices for this org will be created against the contact who requested the first job order selected when invoicing. If this has a real value, then by default new invoices for this org will be created against the contact with the ID specified.");

                entity.Property(e => e.InvoiceDefaultSecondContactId)
                    .HasColumnName("InvoiceDefaultSecondContactID")
                    .HasComment("If this contact is NULL, then by default no second contact will be cc'ed/ If this contact is 0, then by default invoices will be sent to whoever was the contact who ordered the first job attached to the invoice. If this is a real value, then by default new invoices for this org will also be sent to the contact with this ID (some clients ask that we send invoices to 2 different people/e-mail addresses)");

                entity.Property(e => e.InvoiceLangIanacode)
                    .HasMaxLength(20)
                    .HasColumnName("InvoiceLangIANACode")
                    .HasComment("The default language in which invoices should be created for this org");

                entity.Property(e => e.InvoiceOrgName).HasMaxLength(200);

                entity.Property(e => e.InvoicePaymentTermDays).HasComment("The number of days following issue of invoice by when we expect/have agreed to receive payment from the client. Our default would typically be 30.");

                entity.Property(e => e.InvoicePostcodeOrZip).HasMaxLength(20);

                entity.Property(e => e.InvoicedMarginOverLast3Months)
                    .HasColumnType("decimal(18, 2)")
                    .HasComputedColumnSql("([dbo].[funcGetMarginPecentageOfClientSpendOverLast3Months]([ID]))", false);

                entity.Property(e => e.InvoicingAutoChaseOverdueContacts)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Defines the default behaviour for new invoices created for this org, as to whether our system automatically sends out reminder e-mails to the invoicing contact once the invoice becomes overdue");

                entity.Property(e => e.InvoicingAutoCreateAndSendInvoices).HasComment("Defines the default behaviour for job orders created against this org, as to whether our system automatically generates and sends the invoice by e-mail. (Invoices will not be auto-generated if no e-mail address exists for the invoicing contact, or if there are notes present in the InternalNotes field.)");

                entity.Property(e => e.IplusTimeOut).HasColumnName("iplusTimeOut");

                entity.Property(e => e.LastAutoInvoicedDate).HasColumnType("date");

                entity.Property(e => e.LastExportedToSageDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LegalStatusId).HasColumnName("LegalStatusID");

                entity.Property(e => e.LinguistRatingEnabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.LinguisticDatabaseId).HasColumnName("LinguisticDatabaseID");

                entity.Property(e => e.MainIndustryId).HasColumnName("MainIndustryID");

                entity.Property(e => e.MarginsApprovalLowerThreshold).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.MarginsApprovalUpperThreshold).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.OnRedAlertFromEmployeeId).HasColumnName("OnRedAlertFromEmployeeID");

                entity.Property(e => e.OnRedAlertSince).HasColumnType("datetime");

                entity.Property(e => e.OrgGroupId).HasColumnName("OrgGroupID");

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.OriginalorgCodeForXref)
                    .HasMaxLength(255)
                    .HasColumnName("ORIGINALOrgCodeForXRef");

                entity.Property(e => e.PhoneCountryId).HasColumnName("PhoneCountryID");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.PonumbersRequiredForGoAheads).HasColumnName("PONumbersRequiredForGoAheads");

                entity.Property(e => e.PonumbersRequiredForInvoicing)
                    .HasColumnName("PONumbersRequiredForInvoicing")
                    .HasComment("1/True means that this org has told us that technically a PO number is required for us to invoice their jobs. This is used to prompt a user who tries to generate an invoice for this org when the PO number(s) do not exist for the job(s) being invoiced.");

                entity.Property(e => e.PostcodeOrZip).HasMaxLength(255);

                entity.Property(e => e.SalesCategoryId).HasColumnName("SalesCategoryID");

                entity.Property(e => e.SapmasterDataReferenceNumber).HasColumnName("SAPMasterDataReferenceNumber");

                entity.Property(e => e.ShowCustomField4).HasDefaultValueSql("((0))");

                entity.Property(e => e.SpendFrequencyAlertLastIssued).HasColumnType("datetime");

                entity.Property(e => e.TpintroductionSource)
                    .HasColumnName("TPIntroductionSource")
                    .HasDefaultValueSql("((15))");

                entity.Property(e => e.TradosProjectTemplatePath).HasMaxLength(500);

                entity.Property(e => e.TranslateonlineStatus)
                    .HasMaxLength(10)
                    .HasColumnName("translateonlineStatus");

                entity.Property(e => e.TranslateonlineTrialStarted)
                    .HasColumnType("datetime")
                    .HasColumnName("translateonlineTrialStarted");

                entity.Property(e => e.Vatnumber)
                    .HasMaxLength(50)
                    .HasColumnName("VATNumber")
                    .HasComment("The orgs's VAT number (if known; and not needed until they have actually become a client)");

                entity.Property(e => e.WebAddress).HasMaxLength(200);
            });

            modelBuilder.Entity<OrgAltairDetail>(entity =>
            {
                entity.Property(e => e.AltairRegionId).HasColumnName("AltairRegionID");

                entity.Property(e => e.City).HasMaxLength(20);

                entity.Property(e => e.CompanyRegistrationNumber).HasMaxLength(200);

                entity.Property(e => e.CorporateGroupeId).HasColumnName("CorporateGroupeID");

                entity.Property(e => e.Einnumber)
                    .HasMaxLength(200)
                    .HasColumnName("EINNumber");

                entity.Property(e => e.Gstnumber)
                    .HasMaxLength(200)
                    .HasColumnName("GSTNumber");

                entity.Property(e => e.HouseNumber).HasMaxLength(200);

                entity.Property(e => e.Hstnumber)
                    .HasMaxLength(200)
                    .HasColumnName("HSTNumber");

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.OrgLegalName).HasMaxLength(200);

                entity.Property(e => e.Qstnumber)
                    .HasMaxLength(200)
                    .HasColumnName("QSTNumber");

                entity.Property(e => e.Sinnumber)
                    .HasMaxLength(200)
                    .HasColumnName("SINNumber");

                entity.Property(e => e.Sirennumber)
                    .HasMaxLength(200)
                    .HasColumnName("SIRENNumber");

                entity.Property(e => e.Siretnumber)
                    .HasMaxLength(200)
                    .HasColumnName("SIRETNumber");

                entity.Property(e => e.Ssnnumber)
                    .HasMaxLength(200)
                    .HasColumnName("SSNNumber");

                entity.Property(e => e.Tinnumber)
                    .HasMaxLength(200)
                    .HasColumnName("TINNumber");
            });

            modelBuilder.Entity<OrgGroup>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.Name }, "_dta_index_OrgGroups_12_622625261__K1_K2");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.ClientSpendCurrentFinancialYear)
                    .HasColumnType("money")
                    .HasComputedColumnSql("([dbo].[funcCurrentFinancialYearSpendByClientForGroup]([ID]))", false);

                entity.Property(e => e.ClientSpendLastFinancialYear)
                    .HasColumnType("money")
                    .HasComputedColumnSql("([dbo].[funcLastFinancialYearSpendByClientForGroup]([ID]))", false);

                entity.Property(e => e.ClientSpendOverLast12Months)
                    .HasColumnType("money")
                    .HasComputedColumnSql("([dbo].[funcGetSpendByClientOverLast12MonthsForGroup]([ID]))", false);

                entity.Property(e => e.ClientSpendOverLast3Months)
                    .HasColumnType("money")
                    .HasComputedColumnSql("([dbo].[funcGetSpendByClientOverLast3MonthsForGroup]([ID]))", false);

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.FirstPaidJobDate)
                    .HasColumnType("datetime")
                    .HasComputedColumnSql("([dbo].[funcGetClientsFirstPaidJobDateForGroup]([ID]))", false);

                entity.Property(e => e.HqorgId).HasColumnName("HQOrgID");

                entity.Property(e => e.InvoicedMarginOverLast3Months)
                    .HasColumnType("decimal(18, 2)")
                    .HasComputedColumnSql("([dbo].[funcGetMarginPecentageForOrgGroupOverLast3Months]([ID]))", false);

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LinguisticDatabaseId).HasColumnName("LinguisticDatabaseID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.OriginalgroupCodeForXref)
                    .HasMaxLength(255)
                    .HasColumnName("ORIGINALGroupCodeForXRef");
            });

            modelBuilder.Entity<OrgIndustry>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AltairIndustryId).HasColumnName("AltairIndustryID");

                entity.Property(e => e.MainIndustryId).HasColumnName("MainIndustryID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the industry sector. This is in English only as it will be only for internal use.");
            });

            modelBuilder.Entity<OrgIndustryRelationship>(entity =>
            {
                entity.HasComment("Table linking Orgs, OrgIndustriesMain and OrgIndustriesSpecialisms to describe which organisations fall into which industry categories (used for marketing/targeting purposes)");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrgId)
                    .HasColumnName("OrgID")
                    .HasComment("The ID of the organisation being linked");

                entity.Property(e => e.OrgIndustryId)
                    .HasColumnName("OrgIndustryID")
                    .HasComment("The ID of the top-level industry, or the specialist industry sub-division, which the organisation specified by OrgID falls into. Use IsIndustrySpecialism to determine if it's a top-level industry or a specialist sub-division.");
            });

            modelBuilder.Entity<OrgIntroductionSource>(entity =>
            {
                entity.ToTable("OrgIntroductionSource");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SourceName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<OrgLegalStatus>(entity =>
            {
                entity.ToTable("OrgLegalStatus");

                entity.HasComment("Possible legal statuses (relating to public/private ownership) for organisations");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of this legal status. In English only as this will be for internal use only.");
            });

            modelBuilder.Entity<OrgMainIndustry>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.MainIndustryName).IsRequired();
            });

            modelBuilder.Entity<OrgReminderSetting>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataTypeId).HasColumnName("DataTypeID");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<OrgSalesCategory>(entity =>
            {
                entity.HasComment("Information about how \"hot\" an organisation currently is from a sales perspective");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the sales category into which a given organisation can fall. Currently internal only so no multlingual support needed.");
            });

            modelBuilder.Entity<OrgTechnologyRelationship>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.OrgTechnologyId).HasColumnName("OrgTechnologyID");
            });

            modelBuilder.Entity<OverdueReason>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.HasComment("Methods by which we may pay suppliers or we may receive payments from clients.");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The human-readable description of the payment method");
            });

            modelBuilder.Entity<PerformancePlusManualFigure>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.ValueEnquiriesReceived).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Week).HasColumnType("datetime");
            });

            modelBuilder.Entity<PerformancePlusTarget>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChargeToClientTarget).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.Week).HasColumnType("date");
            });

            modelBuilder.Entity<PlanningCalendarAppointment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.ExtranetUserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.Property(e => e.SubjectLine)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<PlanningCalendarCategory>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CategoryRgbcolor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("CategoryRGBColor");
            });

            modelBuilder.Entity<PreferredLanguage>(entity =>
            {
                entity.HasNoKey();

                entity.HasComment("Mappings for which source and/or target languages individual contacts, orgs, groups or linguistic suppliers may have specified as \"preferred\". For example, in the extranet, for a contact to show a particular set of target languages as their preferred \"standard languages\".");

                entity.Property(e => e.CustomerSpecificCode).HasComment("An optional customer-specific code, for example with GN ReSound to pass a value indicating which brand they want, which affects the languages in question. Will default to NULL and have no effect unless set. The value, if set, must be 1 or higher (not zero).");

                entity.Property(e => e.DataObjectId)
                    .HasColumnName("DataObjectID")
                    .HasComment("The ID of the data object to which this preferred language applies");

                entity.Property(e => e.DataObjectTypeId)
                    .HasColumnName("DataObjectTypeID")
                    .HasComment("The type of data object the language preference refers to (e.g. contact, org, group)");

                entity.Property(e => e.IsSource).HasComment("True if this is a preferred source language; false if this is a preferred target language");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANACode")
                    .HasComment("The code of the preferred language ");
            });

            modelBuilder.Entity<PriceListAutomationManagementAudit>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PriceListAutomationManagementAudit");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OrgGroupId).HasColumnName("OrgGroupID");
            });

            modelBuilder.Entity<PriceListsAuditLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PriceListsAuditLog");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Quote>(entity =>
            {
                entity.HasComment("A quote must be attached to an entry in the Enquiries table, and represents our response to an enquiry, detailing costs and timings for carrying out a piece of work. There can be more than one quote attached to a single enquiry, if we've been asked to amend the quote a few times, e.g. if we send a quote, then the client asks for an updated quote with an extra language added, and so on.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddresseeSalutationName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name as we want to show it to go into the salutation line, e.g. \"Dear Peter,\" rather than \"Dear Peter Smith,\", or \"Herrn Smith\", etc.");

                entity.Property(e => e.AssignedToEmployeeId).HasColumnName("AssignedToEmployeeID");

                entity.Property(e => e.ClientPonumber)
                    .HasMaxLength(100)
                    .HasColumnName("ClientPONumber");

                entity.Property(e => e.ClosingSectionText)
                    .IsRequired()
                    .HasComment("Closing text of the quote (after the costs table and the timelines)");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The ID of the employee who set up the quote");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the quote was set up");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("If set, the ID of the employee who deleted the quote");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date/time when the quote was deleted");

                entity.Property(e => e.DiscountAmount)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetDiscountAmountForQuoteID]([ID]))", false);

                entity.Property(e => e.DiscountId).HasColumnName("DiscountID");

                entity.Property(e => e.EnquiryId)
                    .HasColumnName("EnquiryID")
                    .HasComment("The ID of the row in Enquiries which this quote is attached to");

                entity.Property(e => e.InternalNotes).HasComment("Optional free-text notes for internal use only, e.g. something for the sales person or enquiries team member to be aware of when creating the quote");

                entity.Property(e => e.IsCurrentVersion).HasComment("1 if this is the current quote for the corresponding enquiry (for example, if we have been asked to create several different alternative quotes for a particular enquiry, as the client changes their mind about languages, files, etc.). Only one quote can be current at any one time.");

                entity.Property(e => e.LangIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LangIANACode")
                    .HasComment("The base language of the quote - to help with drawing in content from templates and also potentially with any automatic e-mails or similar.");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("If set, the ID of the employee who last modified the quote");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date/time when the quote was last modified");

                entity.Property(e => e.OpeningSectionText)
                    .IsRequired()
                    .HasComment("Opening text of the quote (before the costs table)");

                entity.Property(e => e.OverallChargeToClient)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetOverallChargeToClientForQuoteID]([ID]))", false);

                entity.Property(e => e.QuoteAddress1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Normally this would be picked up automatically from corresopnding field in Orgs; but we have some clients where we need to quote things to different end-clients or to particular legal entities, locations, etc.");

                entity.Property(e => e.QuoteAddress2)
                    .HasMaxLength(100)
                    .HasComment("Normally this would be picked up automatically from corresopnding field in Orgs; but we have some clients where we need to quote things to different end-clients or to particular legal entities, locations, etc.");

                entity.Property(e => e.QuoteAddress3)
                    .HasMaxLength(100)
                    .HasComment("Normally this would be picked up automatically from corresopnding field in Orgs; but we have some clients where we need to quote things to different end-clients or to particular legal entities, locations, etc.");

                entity.Property(e => e.QuoteAddress4)
                    .HasMaxLength(100)
                    .HasComment("Normally this would be picked up automatically from corresopnding field in Orgs; but we have some clients where we need to quote things to different end-clients or to particular legal entities, locations, etc.");

                entity.Property(e => e.QuoteCountryId)
                    .HasColumnName("QuoteCountryID")
                    .HasComment("Normally this would be picked up automatically from corresopnding field in Orgs; but we have some clients where we need to quote things to different end-clients or to particular legal entities, locations, etc.");

                entity.Property(e => e.QuoteCountyOrState)
                    .HasMaxLength(100)
                    .HasComment("Normally this would be picked up automatically from corresopnding field in Orgs; but we have some clients where we need to quote things to different end-clients or to particular legal entities, locations, etc.");

                entity.Property(e => e.QuoteCurrencyId)
                    .HasColumnName("QuoteCurrencyID")
                    .HasComment("The ID (from Currencies) of the currency in which the quote is valued");

                entity.Property(e => e.QuoteDate)
                    .HasColumnType("date")
                    .HasComment("The date to display on the quote as the date it is valid from. Normally this would be the same date as specified in CreatedDateTime, but we might want to alter it.");

                entity.Property(e => e.QuoteFileName).HasMaxLength(500);

                entity.Property(e => e.QuoteOrgName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("The company/org name as appearing on the quote. Normally this and address details would be picked up automatically from corresopnding field in Orgs; but we have some clients where we need to quote things to different end-clients or to particular legal entities, etc.");

                entity.Property(e => e.QuotePostcodeOrZip)
                    .HasMaxLength(20)
                    .HasComment("Normally this would be picked up automatically from corresopnding field in Orgs; but we have some clients where we need to quote things to different end-clients or to particular legal entities, locations, etc.");

                entity.Property(e => e.SalesContactEmployeeId)
                    .HasColumnName("SalesContactEmployeeID")
                    .HasComment("The ID (from Employees) of the employee whose contact details are shown at the end of the quote");

                entity.Property(e => e.ShowCustomerSpecificField3Value).HasDefaultValueSql("((0))");

                entity.Property(e => e.ShowCustomerSpecificField4Value).HasDefaultValueSql("((0))");

                entity.Property(e => e.ShowInterpretingDurationInBreakdown).HasComment("Whether the invoice will show the number of hours/minutes of interpreting time in the jobs breakdown section");

                entity.Property(e => e.ShowPagesOrSlidesInBreakdown).HasComment("Whether the invoice will show the number of pages (for DTP) or slides (for presentations) in the jobs breakdown section");

                entity.Property(e => e.ShowWorkDurationInBreakdown).HasComment("Whether the invoice will show the number of hours/minutes of work\r\n(e.g. for proofreading/software testing/etc.) in the jobs breakdown section");

                entity.Property(e => e.SubTotalOverallChargeToClient)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetSubTotalOverallChargeToClientForQuoteID]([ID]))", false);

                entity.Property(e => e.SurchargeAmount)
                    .HasColumnType("decimal(38, 18)")
                    .HasComputedColumnSql("([dbo].[funcGetSurchargeAmountForQuoteID]([ID]))", false);

                entity.Property(e => e.SurchargeId).HasColumnName("SurchargeID");

                entity.Property(e => e.TimelineUnit).HasComment("Hard-coded to represent the units in which timings would be measured. 1 = days, 2 = weeks");

                entity.Property(e => e.TimelineValue).HasComment("The number of units representing our timings for completing the work, e.g. 5 if 5 days, etc.");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("The title of the quote, which would appear prominently on a resulting file or (in the extranet) as the \"name\" of the quote");

                entity.Property(e => e.WordCountPresentationOption).HasComment("Category determining how much detail to show for word counts presented in the costs table for this quote. Currently\r\n        DoNotShow = 0\r\n        ShowTotalsOnly = 1\r\n        ShowMemoryBreakdownStandard = 2\r\n        ShowMemoryBreakdownIncludingPerfectMatch = 3");
            });

            modelBuilder.Entity<QuoteAndOrderDiscountsAndSurcharge>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DiscountOrSurchargeAmount).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<QuoteAndOrderDiscountsAndSurchargesCategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DiscountOrSurchargeCategory)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<QuoteItem>(entity =>
            {
                entity.HasComment("An individual item comprising part of a complete quote (representing a combination of languages, words/pages/hours, cost, etc.");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AudioMinutes).HasComment("For transcription quotes, or anything where we might be working with a source audio recording, the number of minutes in the recording (as opposed to the number of WorkMinutes)");

                entity.Property(e => e.ChargeToClient)
                    .HasColumnType("money")
                    .HasComment("The monetary value we are quoting to the client for this item. Can be zero if we are doing it for free but want to emphasise that we are not charging for it. The currency must be \"inherited\" from the parent Quote.");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The quote could be created in the database by a client using the extranet/Web Service, in which case this field would be a special extranet employee ID. Where we've received a quote request by e-mail/FTP etc., this field tracks the internal employee who entered details into the system.");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when the item was added to the database");

                entity.Property(e => e.DeletedByEmployeeId)
                    .HasColumnName("DeletedByEmployeeID")
                    .HasComment("If set, the ID of the employee who deleted this item.");

                entity.Property(e => e.DeletedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date/time at which this item was deleted.");

                entity.Property(e => e.ExternalNotes)
                    .HasMaxLength(200)
                    .HasComment("Optional free-text notes which will appear on the quote and hence be visible to the client.");

                entity.Property(e => e.InterpretingExpectedDurationMinutes).HasComment("For an interpreting quote, the number of minutes (normally a multiple of 60/30/15) which we expect (when the client requests the assignment) for the duration of the assignment.");

                entity.Property(e => e.InterpretingLocationAddress1)
                    .HasMaxLength(100)
                    .HasComment("Address line 1 of where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationAddress2)
                    .HasMaxLength(100)
                    .HasComment("Address line 2 of where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationAddress3)
                    .HasMaxLength(100)
                    .HasComment("Address line 3 of where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationAddress4)
                    .HasMaxLength(100)
                    .HasComment("Address line 4 of where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationCountryId)
                    .HasColumnName("InterpretingLocationCountryID")
                    .HasComment("ID of the country (from Countries) where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationCountyOrState)
                    .HasMaxLength(100)
                    .HasComment("County/state/equivalent of where an interpreting assignment will take place");

                entity.Property(e => e.InterpretingLocationOrgName)
                    .HasMaxLength(200)
                    .HasComment("The name of the organisation where an interpreting assignment will take place. This may be a different place/org to the main site of the org which has ordered/is paying for an interpreter.");

                entity.Property(e => e.InterpretingLocationPostcodeOrZip)
                    .HasMaxLength(20)
                    .HasComment("Postcode/zip code/equivalent of where an interpreting assignment will take place");

                entity.Property(e => e.LanguageServiceId)
                    .HasColumnName("LanguageServiceID")
                    .HasComment("The ID (from LanguageServices) representing what kind of activity is involved, e.g. whether this is translation, interpreting, transcription, etc.");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("If set, the ID of the employee who last updated the item details");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("If set, the date/time when the item details were last updated");

                entity.Property(e => e.Pages).HasComment("For a DTP quote or other item where we might charge or pay by page, this captures the number of pages. Can be NULL if irrelevant.");

                entity.Property(e => e.QuoteId)
                    .HasColumnName("QuoteID")
                    .HasComment("The ID (from Quotes) which this item is attached to. All items must have a \"parent\" Quote ID.");

                entity.Property(e => e.SourceLanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SourceLanguageIANAcode")
                    .HasComment("The source language (from Languages)");

                entity.Property(e => e.SupplierWordCountExact).HasComment("If a translation job, the number of exact match words from the translation memory (can be NULL if not a translation job)");

                entity.Property(e => e.SupplierWordCountFuzzyBand1).HasComment("If a translation job, the number of translation memory fuzzy match words in Band 1 (can be NULL if not a translation job). At the moment, there is only 1 fuzzy band -- I am providing for future compatibility when we may need to show a more detailed breakdown of fuzzy percentages.");

                entity.Property(e => e.SupplierWordCountPerfectMatches).HasComment("If a translation job, the number of PerfectMatch (aka XTranslated/In-Context Exact) match words from the translation memory.");

                entity.Property(e => e.SupplierWordCountRepetitions).HasComment("If a translation job, the number of repetition match words from the translation memory (can be NULL if not a translation job)");

                entity.Property(e => e.TargetLanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("TargetLanguageIANAcode")
                    .HasComment("The target language (from Languages). Each individual item can have only one target language (for a quote with multiple target languages, it will have multiple QuoteItem records attached to a single Quote).");

                entity.Property(e => e.WordCountExact).HasComment("If a translation quote, the number of exact match words from the translation memory (can be NULL if not a translation quote)");

                entity.Property(e => e.WordCountFuzzyBand1).HasComment("If a translation quote, the number of translation memory fuzzy match words in Band 1 (can be NULL if not a translation quote). At the moment, there is only 1 fuzzy band -- I am providing for future compatibility when we may need to show a more detailed breakdown of fuzzy percentages.");

                entity.Property(e => e.WordCountNew).HasComment("If a translation quote, the number of new words (can be NULL if not a translation quote)");

                entity.Property(e => e.WordCountPerfectMatches).HasComment("If a translation quote, the number of PerfectMatch (aka XTranslated/In-Context Exact) match words from the translation memory.");

                entity.Property(e => e.WordCountRepetitions).HasComment("If a translation quote, the number of repetition match words from the translation memory (can be NULL if not a translation quote)");

                entity.Property(e => e.WorkMinutes).HasComment("For any activity (e.g. proofreading, testing) where we charge based on a number of working hours/minutes, this stores the number of minutes required (normally a multiple of 60/30)");
            });

            modelBuilder.Entity<QuoteTemplate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANACode");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.TemplateName).HasMaxLength(50);
            });

            modelBuilder.Entity<ReviewContextField>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.Color).HasMaxLength(200);

                entity.Property(e => e.ContextFieldId).HasColumnName("ContextFieldID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(200);
            });

            modelBuilder.Entity<ReviewContextFieldsArchived>(entity =>
            {
                entity.ToTable("ReviewContextFieldsArchived");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.Color).HasMaxLength(200);

                entity.Property(e => e.ContextFieldId).HasColumnName("ContextFieldID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(200);
            });

            modelBuilder.Entity<ReviewContextFieldsTransit>(entity =>
            {
                entity.ToTable("ReviewContextFieldsTransit");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.Color).HasMaxLength(200);

                entity.Property(e => e.ContextFieldId).HasColumnName("ContextFieldID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(200);
            });

            modelBuilder.Entity<ReviewGrade>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<ReviewPlusSignOffJobItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientReviewerId).HasColumnName("ClientReviewerID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.JobOrderId).HasColumnName("JobOrderID");

                entity.Property(e => e.LanguageServiceId).HasColumnName("LanguageServiceID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.ReviewGrade).HasMaxLength(300);

                entity.Property(e => e.SourceLanguageIanacode)
                    .HasMaxLength(350)
                    .HasColumnName("SourceLanguageIANAcode");

                entity.Property(e => e.TargetLanguageIanacode)
                    .HasMaxLength(300)
                    .HasColumnName("TargetLanguageIANAcode");
            });

            modelBuilder.Entity<ReviewPlusTag>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AlternativeTagIdentifier).HasMaxLength(30);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.TagIdentifier)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TagText).IsRequired();
            });

            modelBuilder.Entity<ReviewPlusTagsArchived>(entity =>
            {
                entity.ToTable("ReviewPlusTagsArchived");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AlternativeTagIdentifier).HasMaxLength(30);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.TagIdentifier)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TagText).IsRequired();
            });

            modelBuilder.Entity<ReviewPlusTagsTransit>(entity =>
            {
                entity.ToTable("ReviewPlusTagsTransit");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AlternativeTagIdentifier).HasMaxLength(30);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.TagIdentifier)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TagText).IsRequired();
            });

            modelBuilder.Entity<ReviewQuestion>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<ReviewTranslation>(entity =>
            {
                entity.HasIndex(e => new { e.JobItemId, e.ReviewStatus }, "_dta_index_ReviewTranslations_12_2027258377__K2_K10");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContentType).HasMaxLength(500);

                entity.Property(e => e.ContextFieldIds)
                    .HasMaxLength(200)
                    .HasColumnName("ContextFieldIDs");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasDefaultValueSql("((119))");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((26)/(6))/(2012))");

                entity.Property(e => e.FileName).IsRequired();

                entity.Property(e => e.IsMttranslated).HasColumnName("IsMTTranslated");

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.MttranslationReview).HasColumnName("MTTranslationReview");

                entity.Property(e => e.MttranslationReviewCollapsedTags).HasColumnName("MTTranslationReviewCollapsedTags");

                entity.Property(e => e.OriginalMatchPercentCssclass)
                    .HasMaxLength(30)
                    .HasColumnName("OriginalMatchPercentCSSClass");

                entity.Property(e => e.OriginalMatchPercentage).HasDefaultValueSql("((100))");

                entity.Property(e => e.SourceLanguage)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'en')");

                entity.Property(e => e.SourceText).IsRequired();

                entity.Property(e => e.TargetLanguage)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'es')");

                entity.Property(e => e.TranslationBeforeReview).IsRequired();

                entity.Property(e => e.TranslationDuringReview).IsRequired();
            });

            modelBuilder.Entity<ReviewTranslationComment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.FileName).IsRequired();

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReviewTranslationsArchived>(entity =>
            {
                entity.ToTable("ReviewTranslationsArchived");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ContextFieldIds)
                    .HasMaxLength(200)
                    .HasColumnName("ContextFieldIDs");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.FileName).IsRequired();

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OriginalMatchPercentCssclass)
                    .HasMaxLength(30)
                    .HasColumnName("OriginalMatchPercentCSSClass");

                entity.Property(e => e.SourceLanguage)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SourceText).IsRequired();

                entity.Property(e => e.TargetLanguage)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.TranslationBeforeReview).IsRequired();

                entity.Property(e => e.TranslationDuringReview).IsRequired();
            });

            modelBuilder.Entity<ReviewTranslationsCopy>(entity =>
            {
                entity.ToTable("ReviewTranslationsCopy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasDefaultValueSql("((119))");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((26)/(6))/(2012))");

                entity.Property(e => e.FileName).IsRequired();

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedUserName).HasDefaultValueSql("('Gonzalo Carracedo')");

                entity.Property(e => e.OriginalMatchPercentage).HasDefaultValueSql("((100))");

                entity.Property(e => e.SourceLanguage)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'en')");

                entity.Property(e => e.SourceText).IsRequired();

                entity.Property(e => e.TargetLanguage)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'es')");

                entity.Property(e => e.TranslationBeforeReview).IsRequired();

                entity.Property(e => e.TranslationDuringReview).IsRequired();
            });

            modelBuilder.Entity<ReviewTranslationsTransit>(entity =>
            {
                entity.ToTable("ReviewTranslationsTransit");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContextFieldIds)
                    .HasMaxLength(200)
                    .HasColumnName("ContextFieldIDs");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasDefaultValueSql("((119))");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((26)/(6))/(2012))");

                entity.Property(e => e.FileName).IsRequired();

                entity.Property(e => e.JobItemId).HasColumnName("JobItemID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OriginalMatchPercentCssclass)
                    .HasMaxLength(30)
                    .HasColumnName("OriginalMatchPercentCSSClass");

                entity.Property(e => e.OriginalMatchPercentage).HasDefaultValueSql("((100))");

                entity.Property(e => e.SourceLanguage)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'en')");

                entity.Property(e => e.SourceText).IsRequired();

                entity.Property(e => e.TargetLanguage)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'es')");

                entity.Property(e => e.TranslationBeforeReview).IsRequired();

                entity.Property(e => e.TranslationDuringReview).IsRequired();
            });

            modelBuilder.Entity<SalesHotlistEntry>(entity =>
            {
                entity.HasComment("Entries added to salespeople's hot lists - list of current/interesting opportunities which they are aiming to win as customers of translate plus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdjustedAnnualGbpspend)
                    .HasColumnType("money")
                    .HasColumnName("AdjustedAnnualGBPSpend")
                    .HasComputedColumnSql("(([BestGuessOfAnnualGBPSpend]*[WinChancePercentage])/(100))", false)
                    .HasComment("An adjusted version of what this client spends on an annual basis, taking into account the current likelihood of winning it - used for forecasting. The result of multiplying BestGuessOfAnnualGBPSpend by the WinChancePercentage.");

                entity.Property(e => e.AdjustedGbpspendWithUsThisFy)
                    .HasColumnType("money")
                    .HasColumnName("AdjustedGBPSpendWithUsThisFY")
                    .HasComputedColumnSql("(([BestGuessOfGBPSpendWithUsThisFY]*[WinChancePercentage])/(100))", false)
                    .HasComment("An adjusted version of what this client will spend with us this financial year, taking into account the current likelihood of winning it - used for forecasting. The result of multiplying BestGuessOfGBPSpendWithUsThisFY by the WinChancePercentage.");

                entity.Property(e => e.AppliesToDataObjectId)
                    .HasColumnName("AppliesToDataObjectID")
                    .HasComment("The ID of the data object to which this hot list entry applies - i.e. whose business we are trying to win");

                entity.Property(e => e.AppliesToDataObjectTypeId)
                    .HasColumnName("AppliesToDataObjectTypeID")
                    .HasComment("In most cases a hot list entry would apply to an org, but we could have one against an entire org group or a specific contact (e.g. where we've won all the tech doc business but are now going after the marketing business controlled by a particular contact)");

                entity.Property(e => e.BestGuessOfAnnualGbpspend)
                    .HasColumnType("money")
                    .HasColumnName("BestGuessOfAnnualGBPSpend")
                    .HasComment("The salesperson's best guess (in sterling) of how much they think this customer spends on translation per year");

                entity.Property(e => e.BestGuessOfGbpspendWithUsThisFy)
                    .HasColumnType("money")
                    .HasColumnName("BestGuessOfGBPSpendWithUsThisFY")
                    .HasComment("The sales person's best guess of what they think this customer might realistically spend with us between now and the end of the financial year. Note that once a customer does start spending with us, we take them off the hot list so this should never get too out-of-date (and the salesperson is responsible for keeping it up-to-date)");

                entity.Property(e => e.CreatedByEmployeeId)
                    .HasColumnName("CreatedByEmployeeID")
                    .HasComment("The employee who added this entry to the list. In almost all cases this will be the same as OwnerEmployeeID, but we might want to support the ability for entries to be reassigned to other people if a salesperson leaves/changes role/etc.");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time when this entry was added to the list");

                entity.Property(e => e.LastModifiedByEmployeeId)
                    .HasColumnName("LastModifiedByEmployeeID")
                    .HasComment("The employee who last modified this entry in the list. In almost all cases this will be the same as OwnerEmployeeID, but we might want to support the ability for entries to be reassigned to other people if a salesperson leaves/changes role/etc.");

                entity.Property(e => e.LastModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time the entry was last modified");

                entity.Property(e => e.NextStepsNotes)
                    .IsRequired()
                    .HasComment("An unlimited free-text field (but in practice, usually a relatively short text) describing what we need to do next to win the business. This could in theory be merged with ProgressNotes, but it helps psychologically to push a salesperson into always showing a next step and not just \"what we've done so far\"");

                entity.Property(e => e.OwnerEmployeeId)
                    .HasColumnName("OwnerEmployeeID")
                    .HasComment("Each sales person can only have one hot list as far as presentation to the end-user is concerned; but each hot list will have multiple entries, so the OwnerEmployeeID (from Employees) determines whose hotlist it shows up on.");

                entity.Property(e => e.ProgressNotes)
                    .IsRequired()
                    .HasComment("An unlimited free-text field (but in practice, usually a relatively short text) describing what the opportunity is and where we're at.");

                entity.Property(e => e.RemovedByEmployeeId)
                    .HasColumnName("RemovedByEmployeeID")
                    .HasComment("The employee who removed this entry from the list. In almost all cases this will be the same as OwnerEmployeeID, but we might want to support the ability for entries to be reassigned to other people if a salesperson leaves/changes role/etc.");

                entity.Property(e => e.RemovedDateTime)
                    .HasColumnType("datetime")
                    .HasComment("The date/time this entry was removed from the list. This is \"removed\" rather than the more normal \"deleted\" because an entry can be removed once we win the business and so we don't want to regard it as \"deleted\" in the sense of getting rid of it permanently");

                entity.Property(e => e.RemovedReason).HasComment("If set, a number indicating the reason why it was removed from the hot list. Hard-coded for now.\r\n\r\nNULL = not yet removed, i.e. still on the list (in VB.NET we convert this to 0 to help with ENUMs)\r\n1 = we won the business (i.e. it will now appear in forecasts, etc.)\r\n2 = we did not win the business\r\n3 = we never heard a definite answer, so tidying up the list\r\n4 = deleted as it was an error of some sort");

                entity.Property(e => e.WinChancePercentage).HasComment("A figure of between 1 and 99 indicating the percentage win chance, as judged by the salesperson within our guidelines, indicating how likely they think we are to win this business. Zero is not allowed (as it means we definitely will not win it), nor is 100 (as it means we definitely have won it), as in either case it should be removed from the hot list. The figure is subsequently used in weighted calculations (e.g the AdjustedAnnualGBPSpend and AdjustedGBPSpendWithUsThisFY) to help with overall forecasting.");
            });

            modelBuilder.Entity<SharePlusArticle>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Contents).IsRequired();

                entity.Property(e => e.CreatedByEmpId).HasColumnName("CreatedByEmpID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Htmlbody)
                    .IsRequired()
                    .HasColumnName("HTMLBody");

                entity.Property(e => e.LastModifiedByEmpId).HasColumnName("LastModifiedByEmpID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastViewedByEmpId).HasColumnName("LastViewedByEmpID");

                entity.Property(e => e.LastViewedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(2000);
            });

            modelBuilder.Entity<SharePlusArticleViewLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArticleId).HasColumnName("ArticleID");

                entity.Property(e => e.ViewedByEmployeeId).HasColumnName("ViewedByEmployeeID");

                entity.Property(e => e.ViewedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SharePlusSearchLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SearchText).IsRequired();

                entity.Property(e => e.SearchedByEmpId).HasColumnName("SearchedByEmpID");

                entity.Property(e => e.SearchedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SharedDoc>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientFileName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<SoftwareApplication>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NameAndVersion)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<StyleGuideRule>(entity =>
            {
                entity.HasComment("Contains client-specific style guide information which can be used for automatically checking a document for compliance against rules (such as \"always start a bullet with a capital letter in English\")");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppliesToDataObjectId)
                    .HasColumnName("AppliesToDataObjectID")
                    .HasComment("The ID of the data object to which the style rules apply");

                entity.Property(e => e.AppliesToDataObjectTypeId)
                    .HasColumnName("AppliesToDataObjectTypeID")
                    .HasComment("The type of data object to which the style rules apply");

                entity.Property(e => e.RulesXml)
                    .IsRequired()
                    .HasColumnName("RulesXML")
                    .HasComment("XML content representing the rules. This is more flexible (e.g. for a Microsoft Word add-in calling the Web Service) than lots of different columns for lots of different clients' rules, although perhaps in the future we may stablise on a set of common rules in separate columns");
            });

            modelBuilder.Entity<SystemConfig>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SystemConfig");

                entity.HasComment("Contains 1 row of system-wide configuration settings, so that these can be modified at the database level without needing to re-code/re-build-re-publish applications");

                entity.Property(e => e.KnowledgeBasePassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The current password we use to access our external knowledge base (\"share plus\") account. This is to help update the display of the current information in the intranet but does not play any functional role.");

                entity.Property(e => e.LondonWiFiPassword).HasMaxLength(50);

                entity.Property(e => e.OpsLeadTm).HasColumnName("OpsLeadTM");

                entity.Property(e => e.OutlookReadyEas1).HasColumnName("OutlookReadyEAS1");

                entity.Property(e => e.OutlookReadyLn1nas5).HasColumnName("OutlookReadyLN1NAS5");

                entity.Property(e => e.SofiaWifiPassword).HasMaxLength(50);

                entity.Property(e => e.SystemHolidayModeEmployeeId)
                    .HasColumnName("SystemHolidayModeEmployeeID")
                    .HasComment("If set, the ID of the employee (from the Employees table) who is cc'ed on all i plus notifications in addition to usual recipients, for use on UK public holidays when we might have special staff coverage. If NULL, all systems operate in \"normal\" notification mode");

                entity.Property(e => e.WebConferencingPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The current password we use to access our external account for web conferencing (e.g. WebEx). This is to help update the display of the current information in the intranet but does not play any functional role.");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasIndex(e => new { e.DeletedDate, e.TaskForEmployeeId, e.DueDateTime, e.CompletedDateTime }, "_dta_index_Tasks_12_1006626629__K17_K8_K5_K10_1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompletedByEmployeeId).HasColumnName("CompletedByEmployeeID");

                entity.Property(e => e.CompletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.DueDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.IsHot).HasComment("If 1/True then then the user wants to indicate that this has particuarly high/notable priority");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TaskForEmployeeId).HasColumnName("TaskForEmployeeID");

                entity.Property(e => e.TaskTypeId).HasColumnName("TaskTypeID");
            });

            modelBuilder.Entity<TaskType>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Timesheet>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ClientChargeInHours).HasComputedColumnSql("([dbo].[funcGetClientChargeHourOfaTimesheetLog1]([ID],(1)))", false);

                entity.Property(e => e.ClientChargeInMinutes).HasComputedColumnSql("([dbo].[funcGetClientChargeHourOfaTimesheetLog1]([ID],(0)))", false);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EndClientId).HasColumnName("EndClientID");

                entity.Property(e => e.NonChargeableTimeInHours).HasComputedColumnSql("([dbo].[funcGetNonChargeableTimeOfaTimesheetLog]([ID],(1)))", false);

                entity.Property(e => e.NonChargeableTimeInMinutes).HasComputedColumnSql("([dbo].[funcGetNonChargeableTimeOfaTimesheetLog]([ID],(0)))", false);

                entity.Property(e => e.NonClientChargeInHours).HasComputedColumnSql("([dbo].[funcGetNonClientChargeOfaTimesheetLog1]([ID],(1)))", false);

                entity.Property(e => e.NonClientChargeInMinutes).HasComputedColumnSql("([dbo].[funcGetNonClientChargeOfaTimesheetLog1]([ID],(0)))", false);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.TimeLogDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TimesheetLogBreakdown>(entity =>
            {
                entity.ToTable("TimesheetLogBreakdown");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.TimesheetId).HasColumnName("TimesheetID");
            });

            modelBuilder.Entity<TimesheetTaskCategory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TimesheetTaskCategory");

                entity.Property(e => e.AppliesToGls).HasColumnName("AppliesToGLS");

                entity.Property(e => e.AppliesToHr).HasColumnName("AppliesToHR");

                entity.Property(e => e.AppliesToQa).HasColumnName("AppliesToQA");

                entity.Property(e => e.AppliesToVm).HasColumnName("AppliesToVM");

                entity.Property(e => e.CategoryName).HasMaxLength(500);

                entity.Property(e => e.CategoryTypeId)
                    .HasColumnName("CategoryTypeID")
                    .HasComment("Timesheet task category type\r\n\r\n1 - Client charge (CC)\r\n2 - Non chargeable time (NCT)\r\n3 - Non client charge (NCC)");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
            });

            modelBuilder.Entity<TradosTemplate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DatetedByEmployeeId).HasColumnName("DatetedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.IsDefaultTempleForEmptyTms).HasColumnName("IsDefaultTempleForEmptyTMs");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedEmployeeId).HasColumnName("LastModifiedEmployeeID");

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.TradosTemplateFilePath)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.TradosTemplateName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<TradosTemplateToTmmapping>(entity =>
            {
                entity.ToTable("TradosTemplateToTMMapping");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DatetedByEmployeeId).HasColumnName("DatetedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedEmployeeId).HasColumnName("LastModifiedEmployeeID");

                entity.Property(e => e.SourceIanacode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("SourceIANACode");

                entity.Property(e => e.TargetIanacode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TargetIANACode");

                entity.Property(e => e.TradosTemplateId).HasColumnName("TradosTemplateID");

                entity.Property(e => e.TranslationMemoryPath)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<TrainingCourse>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DurationInMinutes).HasComment("Duration in minutes");

                entity.Property(e => e.IsVisible)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.TrainingName).IsRequired();

                entity.Property(e => e.TrainingOfficerEmployeeId).HasColumnName("TrainingOfficerEmployeeID");

                entity.Property(e => e.TrainingTypeId).HasColumnName("TrainingTypeID");
            });

            modelBuilder.Entity<TrainingCourseType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TypeName).HasMaxLength(500);
            });

            modelBuilder.Entity<TrainingSession>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.SignOffDateTime).HasColumnType("datetime");

                entity.Property(e => e.SignedOffByEmployeeId).HasColumnName("SignedOffByEmployeeID");

                entity.Property(e => e.TrainingCourseId).HasColumnName("TrainingCourseID");

                entity.Property(e => e.TrainingLocation).HasMaxLength(200);

                entity.Property(e => e.TrainingLocationIsAmeetingRoom).HasColumnName("TrainingLocationIsAMeetingRoom");

                entity.Property(e => e.TrainingOfficerEmployeeId).HasColumnName("TrainingOfficerEmployeeID");

                entity.Property(e => e.TrainingSessionDate).HasColumnType("date");
            });

            modelBuilder.Entity<TrainingSessionAttendee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.TrainingSessionId).HasColumnName("TrainingSessionID");
            });

            modelBuilder.Entity<ViewContactsSearchableInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewContactsSearchableInfo");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.EmailAddress).HasMaxLength(255);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.GroupName).HasMaxLength(200);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ViewContactsWhichHaveBeenContacted>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewContactsWhichHaveBeenContacted");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ViewCountriesMultilingualInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewCountriesMultilingualInfo");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Isocode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("ISOcode");

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcode");
            });

            modelBuilder.Entity<ViewCurrenciesMultilingualInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewCurrenciesMultilingualInfo");

                entity.HasComment("View of all Currencies info in multiple languages");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.CurrencyName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LanguageIanacode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcode");

                entity.Property(e => e.Prefix)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<ViewLanguagesMultilingualInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewLanguagesMultilingualInfo");

                entity.HasComment("View of all Languages info in multiple languages. LanguageIANAcodeBeingDescribed indicates the language each row's data describes, while LanguageIANACodeOfName is the language in which the Name is written.");

                entity.Property(e => e.AssociatedIanascript)
                    .HasMaxLength(20)
                    .HasColumnName("AssociatedIANAScript");

                entity.Property(e => e.LanguageIanacodeBeingDescribed)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcodeBeingDescribed");

                entity.Property(e => e.LanguageIanacodeOfName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LanguageIANAcodeOfName");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.TpregionGroupingId).HasColumnName("TPRegionGroupingID");

                entity.Property(e => e.TpserviceRestrictId).HasColumnName("TPServiceRestrictID");

                entity.Property(e => e.TranslatableViaGoogleApi).HasColumnName("TranslatableViaGoogleAPI");
            });

            modelBuilder.Entity<ViewLinguisticSuppliersSearchableInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewLinguisticSuppliersSearchableInfo");

                entity.Property(e => e.AgencyOrTeamName).HasMaxLength(200);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress).HasMaxLength(100);

                entity.Property(e => e.Gdprstatus).HasColumnName("GDPRStatus");

                entity.Property(e => e.MainContactFirstName).HasMaxLength(100);

                entity.Property(e => e.MainContactSurname).HasMaxLength(100);

                entity.Property(e => e.MemoryRateFor50To74Percent).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateFor75To84Percent).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateFor85To94Percent).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateFor95To99Percent).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForExactMatches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForPerfectMatches).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.MemoryRateForRepetitions).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.NdauploadedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("NDAUploadedDateTime");

                entity.Property(e => e.NeedApprovalToBeAddedToDb).HasColumnName("NeedApprovalToBeAddedToDB");

                entity.Property(e => e.NonEeaclauseDeclinedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("NonEEAClauseDeclinedDateTime");

                entity.Property(e => e.PostcodeOrZip).HasMaxLength(20);

                entity.Property(e => e.SupplierId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SupplierID");

                entity.Property(e => e.SupplierStatusId).HasColumnName("SupplierStatusID");

                entity.Property(e => e.SupplierTypeId).HasColumnName("SupplierTypeID");
            });

            modelBuilder.Entity<ViewMarketingEmailAddressesOfAllClientContact>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewMarketingEmailAddressesOfAllClientContacts");

                entity.Property(e => e.ContactEmailAddress).HasMaxLength(255);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreatedByEmployee)
                    .IsRequired()
                    .HasMaxLength(201);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DataObjectType)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LegalStatusId).HasColumnName("LegalStatusID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.OrgIndustryId).HasColumnName("OrgIndustryID");

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ViewMarketingEmailAddressesOfAllNonClientContact>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewMarketingEmailAddressesOfAllNonClientContacts");

                entity.Property(e => e.ContactEmailAddress).HasMaxLength(255);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreatedByEmployee)
                    .IsRequired()
                    .HasMaxLength(201);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DataObjectType)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LegalStatusId).HasColumnName("LegalStatusID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.OrgIndustryId).HasColumnName("OrgIndustryID");

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ViewOrgsSearchableInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewOrgsSearchableInfo");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.EmailAddress).HasMaxLength(100);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.GroupName).HasMaxLength(200);

                entity.Property(e => e.HfmcodeBs)
                    .HasMaxLength(50)
                    .HasColumnName("HFMCodeBS");

                entity.Property(e => e.HfmcodeIs)
                    .HasMaxLength(50)
                    .HasColumnName("HFMCodeIS");

                entity.Property(e => e.OrgId).HasColumnName("OrgID");

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PostcodeOrZip).HasMaxLength(255);
            });

            modelBuilder.Entity<ViewOrgsWhichHaveBeenContacted>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("viewOrgsWhichHaveBeenContacted");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Vodetail>(entity =>
            {
                entity.ToTable("VODetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Brand).HasMaxLength(1000);

                entity.Property(e => e.Campaign).HasMaxLength(1000);

                entity.Property(e => e.Category).HasMaxLength(250);

                entity.Property(e => e.Channel).HasMaxLength(250);

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.EndClient).HasMaxLength(1000);

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("date")
                    .HasComputedColumnSql("(dateadd(month,[UsageDuration],[LiveDate]))", false)
                    .HasComment("Number of months from the live date");

                entity.Property(e => e.LiveDate).HasColumnType("date");

                entity.Property(e => e.ProjectType).HasComment("0 = General linguistic services, 1 = VO, 2 = Admin");

                entity.Property(e => e.Service).HasMaxLength(250);

                entity.Property(e => e.UsageDuration).HasComment("Number of months");

                entity.Property(e => e.UsageRights).HasMaxLength(100);

                entity.Property(e => e.Vocomplexity)
                    .HasColumnName("VOComplexity")
                    .HasComment("0 = Low, 1 = Medium, 2 = High");

                entity.Property(e => e.VoprojectType)
                    .HasColumnName("VOProjectType")
                    .HasComment("0 = New, 1 = Update");
            });

            modelBuilder.Entity<VodropdownItem>(entity =>
            {
                entity.ToTable("VODropdownItems");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idvalue).HasColumnName("IDValue");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.VodropdownListId).HasColumnName("VODropdownListID");
            });

            modelBuilder.Entity<VodropdownItemsOld>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("VODropdownItemsOLD");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Idvalue).HasColumnName("IDValue");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.VodropdownListId).HasColumnName("VODropdownListID");
            });

            modelBuilder.Entity<VodropdownList>(entity =>
            {
                entity.ToTable("VODropdownLists");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByEmployeeId).HasColumnName("CreatedByEmployeeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("date");

                entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeID");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedByEmployeeId).HasColumnName("LastModifiedByEmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("date");

                entity.Property(e => e.VodropdownListName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("VODropdownListName");
            });

            modelBuilder.Entity<VolumeDiscount>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataObjectId).HasColumnName("DataObjectID");

                entity.Property(e => e.DataObjectTypeId).HasColumnName("DataObjectTypeID");

                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.InForceEndDate).HasColumnType("datetime");

                entity.Property(e => e.InForceStartDate).HasColumnType("datetime");

                entity.Property(e => e.RevenueLowerLimit).HasColumnType("money");

                entity.Property(e => e.RevenueUpperLimit).HasColumnType("money");
            });

            modelBuilder.Entity<WorkAnnivarsariesComment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.WorkAnniversaryId).HasColumnName("WorkAnniversaryID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.WorkAnnivarsariesComments)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkAnnivarsariesComments_Employees");

                entity.HasOne(d => d.WorkAnniversary)
                    .WithMany(p => p.WorkAnnivarsariesComments)
                    .HasForeignKey(d => d.WorkAnniversaryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkAnnivarsariesComments_WorkAnniversariesWishes");
            });

            modelBuilder.Entity<WorkAnniversariesLike>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.WorkAnniversaryId).HasColumnName("WorkAnniversaryID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.WorkAnniversariesLikes)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkAnniversariesLikes_Employees");

                entity.HasOne(d => d.WorkAnniversary)
                    .WithMany(p => p.WorkAnniversariesLikes)
                    .HasForeignKey(d => d.WorkAnniversaryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkAnniversariesLikes_WorkAnniversariesWishes");
            });

            modelBuilder.Entity<WorkAnniversariesWish>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DeletedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.WorkAnniversariesWishes)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkAnniversariesWishes_Employees");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
