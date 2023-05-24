using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Services.Interfaces;
using ViewModels.Common;
using Z.EntityFramework.Plus;
using Global_Settings;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class CommonCachedService : ICommonCachedService
    {
        private readonly IRepository<ClientTechnology> clientTechnoloryRpository;
        private readonly IRepository<Country> countryRepository;
        private readonly IRepository<LocalCountryInfo> localCountryInfoRepository;
        private readonly IRepository<OrgSalesCategory> orgSalesRepository;
        private readonly IRepository<OrgIndustry> orgIndustryRepository;
        private readonly IRepository<DataObjectType> dataObjectTypeRepository;
        private readonly IRepository<OrgIntroductionSource> orgIntroRepository;
        private readonly IRepository<OrgLegalStatus> orgLegalStatusRepository;
        private readonly IRepository<OperatingSystemVersion> systemVersionStatusRepository;
        private readonly IRepository<OrgMainIndustry> orgMainIndustryRepository;
        private readonly IRepository<DecisionReason> decisionRepository;
        private readonly IRepository<AltairRegion> altairRegionRepository;
        private readonly IRepository<Data.OperatingSystem> systemRepository;
        private readonly IRepository<LanguageRateUnit> rateUnitsRepository;
        private readonly IRepository<LocalLanguageInfo> localLanguageInfoRepository;
        private readonly IRepository<LocalCurrencyInfo> localCurrencyInfo;
        private readonly IRepository<LanguageService> languageServicesRepository;
        private readonly IRepository<PaymentMethod> paymentRepository;
        private readonly IRepository<LinguisticSupplierStatus> supplierStatusRepository;
        private readonly IRepository<MasterField> masterFieldsRepository;
        private readonly IRepository<SubField> subFieldsRepository;
        private readonly IRepository<MediaType> mediaTypesRepository;
        private readonly IRepository<LinguisticSupplierType> linguistTypeRepository;
        private readonly IRepository<LinguisticVendorType> linguistVendorTypeRepository;
        private readonly IRepository<Gdprstatus> gdprStatusRepository;    
        private readonly IRepository<LanguageSubjectArea> supplierSubjectAreaRepository;
        private readonly IRepository<SoftwareApplication> softwareRepository;
        private readonly IRepository<LocalPlanningCalendarCategoriesInfo> availabilityRepository;
        private readonly IRepository<ViewLanguagesMultilingualInfo> viewMultiLingualRepository;
        private readonly IRepository<TradosTemplate> templateRepository;
        private readonly IRepository<LinguisticSupplierSoftwareApplication> linguistSupplierSoftwareApplicationRepository;
        private readonly IRepository<TimesheetTaskCategory> timesheetTaskCategory;
        private readonly IRepository<PlanningCalendarCategory> planningCalendarCategory;
        private readonly IRepository<LinguisticSupplierPoliceCheck> linguistPoliceCheckRepository;
        private readonly IRepository<LinguisticSupplierProfessionalBody> linguistProfessionalBodyRepository;
        private readonly IRepository<LanguageServiceCategory> languageServiceCategoryRepository;
        private readonly IRepository<EnquiryStatus> enquiryStatusRepository;

        private readonly MemoryCacheEntryOptions twoWeekCacheOptions = new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromDays(14) };
        private readonly MemoryCacheEntryOptions oneHourCacheOptions = new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromHours(1) };

        public CommonCachedService(
            IRepository<ClientTechnology> clientTechnoloryRpository,
            IRepository<Country> countryRepository,
            IRepository<Gdprstatus> gdprStatusRepository,
            IRepository<MediaType> mediaTypesRepository,
            IRepository<DataObjectType> dataObjectTypeRepository,
            IRepository<SubField> subFieldsRepository,
            IRepository<MasterField> masterFieldsRepository,
            IRepository<LocalCountryInfo> localCountryInfoRepository,
            IRepository<OrgSalesCategory> orgSalesRepository,
            IRepository<LanguageRateUnit> rateUnitsRepository,
            IRepository<OrgIndustry> orgIndustryRepository,
            IRepository<OrgIntroductionSource> orgIntroRepository,
            IRepository<OrgLegalStatus> orgLegalStatusRepository,
            IRepository<OrgMainIndustry> orgMainIndustryRepository,
            IRepository<DecisionReason> decisionRepository,
            IRepository<OperatingSystemVersion> systemVersionStatusRepository,
            IRepository<Data.OperatingSystem> systemRepository,
            IRepository<LinguisticSupplierStatus> supplierStatusRepository,
            IRepository<AltairRegion> altairRegionRepository,
            IRepository<LinguisticSupplierType> linguistTypeRepository,
            IRepository<LinguisticVendorType> linguistVendorTypeRepository,
            IRepository<LocalLanguageInfo> localLanguageInfoRepository,
            IRepository<LanguageService> languageServicesRepository,
            IRepository<SoftwareApplication> softwareRepository,
            IRepository<LocalPlanningCalendarCategoriesInfo> availabilityRepository,
            IRepository<LanguageSubjectArea> supplierSubjectAreaRepository,
            IRepository<PaymentMethod> paymentRepository,
            IRepository<LocalCurrencyInfo> localCurrencyInfo,
            IRepository<ViewLanguagesMultilingualInfo> viewMultiLingualRepository1,
            IRepository<TradosTemplate> templateRepository1,
            IRepository<LinguisticSupplierSoftwareApplication> linguistSupplierSoftwareApplicationRepository,
            IRepository<TimesheetTaskCategory> timesheetTaskCategory,
            IRepository<PlanningCalendarCategory> planningCalendarCategory,
            IRepository<LinguisticSupplierPoliceCheck> linguistPoliceCheckRepository,
            IRepository<LinguisticSupplierProfessionalBody> linguistProfessionalBodyRepository,
            IRepository<LanguageServiceCategory> languageServiceCategoryRepository,
            IRepository<EnquiryStatus> enquiryStatusRepository)
        {
            this.clientTechnoloryRpository = clientTechnoloryRpository;
            this.countryRepository = countryRepository;
            this.dataObjectTypeRepository = dataObjectTypeRepository;
            this.localCountryInfoRepository = localCountryInfoRepository;
            this.orgSalesRepository = orgSalesRepository;
            this.rateUnitsRepository = rateUnitsRepository;
            this.orgIndustryRepository = orgIndustryRepository;
            this.orgIntroRepository = orgIntroRepository;
            this.orgLegalStatusRepository = orgLegalStatusRepository;
            this.orgMainIndustryRepository = orgMainIndustryRepository;
            this.decisionRepository = decisionRepository;
            this.altairRegionRepository = altairRegionRepository;
            this.localLanguageInfoRepository = localLanguageInfoRepository;
            this.languageServicesRepository = languageServicesRepository;
            this.paymentRepository = paymentRepository;
            this.supplierStatusRepository = supplierStatusRepository;
            this.supplierSubjectAreaRepository = supplierSubjectAreaRepository;
            this.softwareRepository = softwareRepository;
            this.mediaTypesRepository = mediaTypesRepository;
            this.gdprStatusRepository = gdprStatusRepository;
            this.localCurrencyInfo = localCurrencyInfo;
            this.subFieldsRepository = subFieldsRepository;
            this.linguistTypeRepository = linguistTypeRepository;
            this.linguistVendorTypeRepository = linguistVendorTypeRepository;
            this.masterFieldsRepository = masterFieldsRepository;
            this.availabilityRepository = availabilityRepository;
            this.viewMultiLingualRepository = viewMultiLingualRepository1;
            this.templateRepository = templateRepository1;
            this.systemRepository = systemRepository;
            this.systemVersionStatusRepository = systemVersionStatusRepository;
            this.linguistSupplierSoftwareApplicationRepository = linguistSupplierSoftwareApplicationRepository;
            this.timesheetTaskCategory = timesheetTaskCategory;
            this.planningCalendarCategory = planningCalendarCategory;
            this.linguistPoliceCheckRepository = linguistPoliceCheckRepository;
            this.linguistProfessionalBodyRepository = linguistProfessionalBodyRepository;
            this.languageServiceCategoryRepository = languageServiceCategoryRepository;
            this.enquiryStatusRepository = enquiryStatusRepository;
        }

        public async Task<IEnumerable<AltairRegionViewModel>> GetAllAltairRegionsCached()
        {
            var res = await altairRegionRepository.All().Select(a => new AltairRegionViewModel()
            {
                Id = a.Id,
                CountryId = a.CountryId,
                RegionCode = a.RegionCode,
                RegionName = a.RegionName
            }).FromCacheAsync(twoWeekCacheOptions);
            return res;
        }
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllClientTechnologiesCached()
        {
            var res = await clientTechnoloryRpository.All().Select(c => new DropdownOptionViewModel()
            {
                Id = c.Id,
                Name = c.TechnologyName
            }).FromCacheAsync(twoWeekCacheOptions);
            return res;
        }
        public async Task<IEnumerable<CountryViewModel>> GetAllCountriesCached()
        {
            var res = await countryRepository.All()
                .Join(localCountryInfoRepository.All().Where(x => x.LanguageIanacode == "en"), c => c.Id, lc => lc.CountryId,
                (c, lc) => new CountryViewModel()
                {
                    Id = c.Id,
                    CountryName = lc.CountryName,
                    DiallingPrefix = c.DiallingPrefix,
                    Eea = c.Eea,
                    GeoPoliticalGroup = c.GeoPoliticalGroup,
                    Ibanlength = c.Ibanlength,
                    IsIbancountry = c.IsIbancountry,
                    Isocode = c.Isocode,
                    SageCountryCode = c.SageCountryCode
                }).FromCacheAsync(twoWeekCacheOptions);
            return res;
        }
        public async Task<IEnumerable<OrgIndustrySectorViewModel>> GetAllOrgIndustrySectorsCached()
        {
            var industries = await orgIndustryRepository.All()
                .Select(x => new OrgIndustrySectorViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    MainIndustryId = x.MainIndustryId.GetValueOrDefault(),
                    AltairIndustryId = x.AltairIndustryId.GetValueOrDefault()
                }).FromCacheAsync(twoWeekCacheOptions);
            return industries;
        }

        public async Task<IEnumerable<DropdownOptionViewModel>> GetSubFields(List<int> MasterFields)
        {
            var subfieldsList = await subFieldsRepository.All().Where(s => MasterFields.Contains(s.MasterFieldId)).OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
            return subfieldsList;
        }

        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllOrgIntroductionSourcesCached() => await orgIntroRepository.All().Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.SourceName }).FromCacheAsync(twoWeekCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllOrgLegalStatusCategoriesCached() => await orgLegalStatusRepository.All().Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(twoWeekCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllOrgMainIndustriesCached() => await orgMainIndustryRepository.All().Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.MainIndustryName }).FromCacheAsync(twoWeekCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllOrgSalesCategoriesCached() => await orgSalesRepository.All().Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(twoWeekCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetDecisionReasonsType1Cached() => await decisionRepository.All().Where(x => x.Type == 1).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Reason }).FromCacheAsync(twoWeekCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetDecisionReasonsType0Cached() => await decisionRepository.All().Where(x => x.Type == 0).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Reason }).FromCacheAsync(twoWeekCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllLanguagesCached() => await localLanguageInfoRepository.All().Where(x => x.LanguageIanacode == "en").Select(x => new DropdownOptionViewModel() { StringValue = x.LanguageIanacodeBeingDescribed, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetMostUsedLanguagesCached(Enumerations.ServiceCategory RestrictByService, bool ShowOnlyCommonLangs, 
                                                                                           string DisplayLanguageIANACode = "en", string ExcludeLangIANACode = "", string GetLanguagesForMTEngine = "")
        {

            var result = await viewMultiLingualRepository.All().Where(x => x.LanguageIanacodeOfName == DisplayLanguageIANACode && 
                                                                           (ShowOnlyCommonLangs == false || x.IsCommon == true) &&
                                                                           ((RestrictByService == Enumerations.ServiceCategory.TranslationAndOtherWrittenServices && (x.TpserviceRestrictId == 1 || x.TpserviceRestrictId == null)) ||
                                                                           (RestrictByService == Enumerations.ServiceCategory.FaceToFaceInterpreting && (x.TpserviceRestrictId == 2 || x.TpserviceRestrictId == null)) ||
                                                                           (RestrictByService == Enumerations.ServiceCategory.TelephoneInterpreting && (x.TpserviceRestrictId == 3 || x.TpserviceRestrictId == null))) &&
                                                                           (ExcludeLangIANACode == "" || x.LanguageIanacodeBeingDescribed != ExcludeLangIANACode) &&
                                                                           ((GetLanguagesForMTEngine == "DeepL API Source" && x.TranslatableSourceViaDeepL == true) ||
                                                                           (GetLanguagesForMTEngine == "DeepL API Target" && x.TranslatableTargetViaDeepL == true) ||
                                                                           (GetLanguagesForMTEngine == "Google Translate API" && x.TranslatableViaGoogleApi == true ) ||
                                                                            GetLanguagesForMTEngine == ""))
                                                         .Select(x => new DropdownOptionViewModel() { StringValue = x.LanguageIanacodeBeingDescribed, Name = x.Name })
                                                         .OrderBy(n => n.Name).FromCacheAsync(oneHourCacheOptions);
            return result;
        }


        List<int> defaultLanguageServices = new List<int>(new int[] { 4, 2, 10, 73, 72, 74, 7, 36, 5, 1, 17, 6 });
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllLanguageServicesCached() => await languageServicesRepository.All().Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).OrderBy(x => x.Name).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllDefaultLanguageServicesCached() => await languageServicesRepository.All().Where(x => defaultLanguageServices.Contains(x.Id)).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).OrderBy(x => x.Name).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllPaymentMethodsCached() => await paymentRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(twoWeekCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllCurrencies() => await localCurrencyInfo.All().OrderBy(x => x.CurrencyName).Select(x => new DropdownOptionViewModel() { Id = x.CurrencyId, Name = x.CurrencyName }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetSupplierStatusCached() => await supplierStatusRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetSupplierSpecialisedRate() => await supplierSubjectAreaRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetSoftware() => await softwareRepository.All().OrderBy(x => x.NameAndVersion).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.NameAndVersion }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAvailability() => await availabilityRepository.All().Where(x => x.LangIanacode == "en").OrderBy(x => x.CategoryName).Select(x => new DropdownOptionViewModel() { Id = x.PlanningCalendarCategoryId, Name = x.CategoryName }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetMasterFields() => await masterFieldsRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetOperatingSystems() => await systemRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetOperatingSystemVersions() => await systemVersionStatusRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> GetMediaTypes() => await mediaTypesRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> getGdprStatus() => await gdprStatusRepository.All().OrderBy(x => x.Gdprvalue).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Gdprvalue }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> getRateUnits() => await rateUnitsRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> getLingTypes() => await linguistTypeRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> getLingVendorTypes() => await linguistVendorTypeRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);

        public async Task<IEnumerable<DropdownOptionViewModel>> getAppliesTo()
        {
            List<int> myValues = new List<int>(new int[] { 1, 2, 3 });
            var appliesToResults = await dataObjectTypeRepository.All().Where(x => myValues.Contains(x.Id)).OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
            return appliesToResults;
        }

        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllTradosTemplatesForAnOrg(int orgId, bool templatesToShowForiPlus)
        {
            var templates = new List<DropdownOptionViewModel>();

            var result = await templateRepository.All().Where(t => t.OrgId == orgId && t.DeletedDateTime == null &&
                                                               t.ToShowOnIplus == templatesToShowForiPlus).ToListAsync();

            for (var i = 0; i < result.Count; i++)
            {
                templates.Add(new DropdownOptionViewModel()
                {
                    Id = result.ElementAt(i).Id,
                    Name = result.ElementAt(i).TradosTemplateName,
                    StringValue = result.ElementAt(i).TradosTemplateFilePath
                });
            }

            return templates;
        }
        public async Task<IEnumerable<LinguisticSupplierSoftwareApplication>> getLinguistSupplierSoftwareApplication() => await linguistSupplierSoftwareApplicationRepository.All().OrderBy(x => x.SoftwareApplicationId).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<SubField>> GetSubFields() => await subFieldsRepository.All().OrderBy(x => x.Name).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<TimesheetTaskCategory>> getTimesheetTaskCategory() => await timesheetTaskCategory.All().FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<PlanningCalendarCategory>> getPlanningCalendarCategories() => await planningCalendarCategory.All().FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> getLingPoliceChecks() => await linguistPoliceCheckRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> getLingProfessionalBodies() => await linguistProfessionalBodyRepository.All().OrderBy(x => x.Name).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<DropdownOptionViewModel>> getLanguageServiceCategory() => await languageServiceCategoryRepository.All().Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).FromCacheAsync(oneHourCacheOptions);
        public async Task<IEnumerable<EnquiryStatus>> getEnquiryStatuses() => await enquiryStatusRepository.All().FromCacheAsync(oneHourCacheOptions);

    }
}
