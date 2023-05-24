using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.Common;
using Global_Settings;
using Data;

namespace Services.Interfaces
{
    public interface ICommonCachedService : IService
    {
        Task<IEnumerable<CountryViewModel>> GetAllCountriesCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllClientTechnologiesCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllOrgSalesCategoriesCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllOrgLegalStatusCategoriesCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetOperatingSystemVersions();
        Task<IEnumerable<OrgIndustrySectorViewModel>> GetAllOrgIndustrySectorsCached();
        Task<IEnumerable<AltairRegionViewModel>> GetAllAltairRegionsCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllOrgMainIndustriesCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllOrgIntroductionSourcesCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetDecisionReasonsType1Cached();
        Task<IEnumerable<DropdownOptionViewModel>> GetDecisionReasonsType0Cached();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllLanguagesCached();
        Task<IEnumerable<DropdownOptionViewModel>> getLingTypes();
        Task<IEnumerable<DropdownOptionViewModel>> getLingVendorTypes();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllCurrencies();
        Task<IEnumerable<DropdownOptionViewModel>> GetMostUsedLanguagesCached(Enumerations.ServiceCategory RestrictByService, bool ShowOnlyCommonLangs,
                                                                                           string DisplayLanguageIANACode = "en", string ExcludeLangIANACode = "", string GetLanguagesForMTEngine = "");
        Task<IEnumerable<DropdownOptionViewModel>> GetAllLanguageServicesCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetOperatingSystems();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllDefaultLanguageServicesCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllPaymentMethodsCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetSupplierStatusCached();
        Task<IEnumerable<DropdownOptionViewModel>> GetSupplierSpecialisedRate();
        Task<IEnumerable<DropdownOptionViewModel>> GetSoftware();
        Task<IEnumerable<DropdownOptionViewModel>> GetMasterFields();
        Task<IEnumerable<SubField>> GetSubFields();
        Task<IEnumerable<DropdownOptionViewModel>> GetSubFields(List<int> MasterFields);
        Task<IEnumerable<DropdownOptionViewModel>> GetAvailability();
        Task<IEnumerable<DropdownOptionViewModel>> GetMediaTypes();
        Task<IEnumerable<DropdownOptionViewModel>> getGdprStatus();
        Task<IEnumerable<DropdownOptionViewModel>> getRateUnits();
        Task<IEnumerable<DropdownOptionViewModel>> getAppliesTo();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllTradosTemplatesForAnOrg(int orgId, bool templatesToShowForiPlus);
        Task<IEnumerable<LinguisticSupplierSoftwareApplication>> getLinguistSupplierSoftwareApplication();
        Task<IEnumerable<TimesheetTaskCategory>> getTimesheetTaskCategory();
        Task<IEnumerable<PlanningCalendarCategory>> getPlanningCalendarCategories();
        Task<IEnumerable<DropdownOptionViewModel>> getLingPoliceChecks();
        Task<IEnumerable<DropdownOptionViewModel>> getLingProfessionalBodies();
        Task<IEnumerable<DropdownOptionViewModel>> getLanguageServiceCategory();
        Task<IEnumerable<EnquiryStatus>> getEnquiryStatuses();

    }
}
