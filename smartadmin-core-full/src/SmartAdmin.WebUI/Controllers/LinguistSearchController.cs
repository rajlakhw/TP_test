using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Services;
using Services.Interfaces;

namespace SmartAdmin.WebUI.Controllers
{
    public class LinguistSearchController : Controller
    {


        private IConfiguration Configuration;
        private readonly ICommonCachedService cachedService;
        private ILinguistSearch linguistSearch;
        private ITPEmployeesService tpservice;



        public LinguistSearchController(IConfiguration _configuration, ITPEmployeesService _empservice,
            ICommonCachedService cachedService, ILinguistSearch linguistSearch)
        {
            Configuration = _configuration;
            tpservice = _empservice;
            this.cachedService = cachedService;
            this.linguistSearch = linguistSearch;
        }


        public async Task<IActionResult> Index()
        {
                  
                var model = new ViewModels.LinguistSearch.LinguistSearchViewModel();
                model.LanguageServices = await cachedService.GetAllLanguageServicesCached();
                model.DefaultLanguageServices = await cachedService.GetAllDefaultLanguageServicesCached();
                model.Countries = await cachedService.GetAllCountriesCached();
                model.Languages = await cachedService.GetAllLanguagesCached();
                model.Softwares = await cachedService.GetSoftware();
                model.MasterFields = await cachedService.GetMasterFields();
                model.supplierStatus = await cachedService.GetSupplierStatusCached();
                model.SpecialRates = await cachedService.GetSupplierSpecialisedRate();
                model.SupplierAvailability = await cachedService.GetAvailability();
                model.MediaTypes = await cachedService.GetMediaTypes();
                model.gdprStatusList = await cachedService.getGdprStatus();
                model.RateUnitsList = await cachedService.getRateUnits();
                model.AppliesToList = await cachedService.getAppliesTo();
                model.LinguisticProfessionalBodies = await cachedService.getLingProfessionalBodies();
                model.VendorTypes = await cachedService.getLingVendorTypes();
            return View("Views/Linguists/LinguistSearch.cshtml", model);
         
       
            
        }

        [HttpPost("api/GetSubFields")]
        public async Task<IEnumerable<ViewModels.Common.DropdownOptionViewModel>> GetSubFields(string SelectedFields)
        {
            List<int> NumbersOnly = new List<int>();

            foreach (char field in SelectedFields)
            {
                if (Char.IsDigit(field))
                {
                    NumbersOnly.Add(Int32.Parse(field.ToString()));
                }
            }

            var SubFields = await cachedService.GetSubFields(NumbersOnly);

            return SubFields;
        }



        public async Task<IActionResult> Results(string rbAccNumber,string NumberValue, string linguistName,
            string EmailValue, string PostcodeValue, string KeyWords, string MasterFieldsValues, string LanguageServicesValues
            , string SubFieldsValues, string MediaTypesValues, string myRangeValue
            , string SourceLanguagesValues, string TargetLanguageValues
            , string SpecialRatesValues, string supplierStatusValues
            , string CountriesNationalyValues, string CountryOfResidenceValues
            , string SoftwaresValues, string gdprStatusListValues
            , string SupplierAvailabilityValues, string RateUnitsListValues, string AppliesToValues, string SupplierNDAValue
            , string SupplierMemoryMatchValue, string SupplierEncryptedValue, string rbPreferred, string rbPreferredObject,
            string PreferredObjectValue, string rbJobsObject, string JobsDoneForClient,string ProfessionalBodiesValues, string VendorTypeValues)
        {
           
            if (NumberValue != null && rbAccNumber == "accNumber")
            {
                var SearchResults  = await linguistSearch.GetLinguistResultsById(NumberValue);
                var model = new ViewModels.LinguistSearch.LinguistSearchViewModel();
                if (NumberValue != null) { model.exactID = int.Parse(NumberValue); }
                if (SearchResults != null) { model.LinguistSearchList = SearchResults; }
                if (NumberValue != null) { model.exactID = int.Parse(NumberValue); }
                if (SpecialRatesValues != null) { model.SpecialRateIds = SpecialRatesValues; }
                if (supplierStatusValues != null) { model.StatusIds = supplierStatusValues; }
                if (SourceLanguagesValues != null) { model.SourceLanguageIanacodes = SourceLanguagesValues; }
                if (TargetLanguageValues != null) { model.TargetLanguageIanacodes = TargetLanguageValues; }
                if (CountriesNationalyValues != null) { model.CountryOfNationalityIds = CountriesNationalyValues; }
                if (CountryOfResidenceValues != null) { model.CountryOfResidenceIds = CountryOfResidenceValues; }
                if (SoftwaresValues != null) { model.SoftwareIds = SoftwaresValues; }
                if (gdprStatusListValues != null) { model.gdprStatusIds = gdprStatusListValues; }
                if (SupplierAvailabilityValues != null) { model.AvailabilityIds = SupplierAvailabilityValues; }
                if (RateUnitsListValues != null) { model.rateUnitsId = int.Parse(RateUnitsListValues); }
                if (AppliesToValues != null) { model.AppliesToId = int.Parse(AppliesToValues); }
                if (SupplierNDAValue != null) { model.suppliersNDA = SupplierNDAValue; }
                if (SupplierEncryptedValue != null) { model.suppliersEncryptedComputer = SupplierEncryptedValue; }
                if (linguistName != null) { model.linguistName = linguistName; }
                if (PostcodeValue != null) { model.postcodeValue = PostcodeValue; }
                if (EmailValue != null) { model.emailValue = EmailValue; }
               
                if (MasterFieldsValues != null) { model.MasterFieldsIds = MasterFieldsValues; }
                if (LanguageServicesValues != null) { model.LanguageServiceIds = LanguageServicesValues; model.defaultLanguageServiceIds = LanguageServicesValues; }
                if (SubFieldsValues != null) { model.SubFieldsIds = SubFieldsValues; }
                if (MediaTypesValues != null) { model.MediaTypeIds = MediaTypesValues; }
                if (SupplierEncryptedValue != null) { model.suppliersEncryptedComputer = SupplierEncryptedValue; }
                if (SupplierNDAValue != null) { model.suppliersNDA = SupplierNDAValue; }
                if (KeyWords != null) { model.keyWords = KeyWords; }
                if (ProfessionalBodiesValues != null) { model.ProfessionalBodyIds = ProfessionalBodiesValues; }
                if (VendorTypeValues != null) { model.VendorTypeIds = VendorTypeValues; }
                model.LanguageServices = await cachedService.GetAllLanguageServicesCached();
                model.Countries = await cachedService.GetAllCountriesCached();
                model.DefaultLanguageServices = await cachedService.GetAllDefaultLanguageServicesCached();
                model.Languages = await cachedService.GetAllLanguagesCached();
                model.Softwares = await cachedService.GetSoftware();
                model.MasterFields = await cachedService.GetMasterFields();
                model.supplierStatus = await cachedService.GetSupplierStatusCached();
                model.SpecialRates = await cachedService.GetSupplierSpecialisedRate();
                model.SupplierAvailability = await cachedService.GetAvailability();
                model.MediaTypes = await cachedService.GetMediaTypes();
                model.gdprStatusList = await cachedService.getGdprStatus();
                model.RateUnitsList = await cachedService.getRateUnits();
                model.AppliesToList = await cachedService.getAppliesTo();
                model.LinguisticProfessionalBodies = await cachedService.getLingProfessionalBodies();
                model.VendorTypes = await cachedService.getLingVendorTypes();
                return View("Views/Linguists/LinguistSearch.cshtml", model);
            }
            else
            {
                var SearchResults = await linguistSearch.GetLinguistResults(NumberValue, linguistName, EmailValue, PostcodeValue,
        KeyWords, MasterFieldsValues, LanguageServicesValues, SubFieldsValues, MediaTypesValues, myRangeValue,
        SourceLanguagesValues, TargetLanguageValues, SpecialRatesValues, supplierStatusValues, CountriesNationalyValues, CountryOfResidenceValues,
        SoftwaresValues, gdprStatusListValues, SupplierAvailabilityValues, RateUnitsListValues, AppliesToValues, SupplierNDAValue,
        SupplierMemoryMatchValue, SupplierEncryptedValue, rbPreferred, rbPreferredObject, PreferredObjectValue, rbJobsObject, JobsDoneForClient,ProfessionalBodiesValues, VendorTypeValues);
                var model = new ViewModels.LinguistSearch.LinguistSearchViewModel();
                if (NumberValue != null) { model.exactID = int.Parse(NumberValue); }
                if (SpecialRatesValues != null) { model.SpecialRateIds = SpecialRatesValues; }
                if (supplierStatusValues != null) { model.StatusIds = supplierStatusValues; }
                if (SourceLanguagesValues != null) { model.SourceLanguageIanacodes = SourceLanguagesValues; }
                if (TargetLanguageValues != null) { model.TargetLanguageIanacodes = TargetLanguageValues; }
                if (CountriesNationalyValues != null) { model.CountryOfNationalityIds = CountriesNationalyValues; }
                if (CountryOfResidenceValues != null) { model.CountryOfResidenceIds = CountryOfResidenceValues; }
                if (SoftwaresValues != null) { model.SoftwareIds = SoftwaresValues; }
                if (gdprStatusListValues != null) { model.gdprStatusIds = gdprStatusListValues; }
                if (SupplierAvailabilityValues != null) { model.AvailabilityIds = SupplierAvailabilityValues; }
                if (RateUnitsListValues != null) { model.rateUnitsId = int.Parse(RateUnitsListValues); }
                if (AppliesToValues != null) { model.AppliesToId = int.Parse(AppliesToValues); }
                if (SupplierNDAValue != null) { model.suppliersNDA = SupplierNDAValue; }
                if (SupplierEncryptedValue != null) { model.suppliersEncryptedComputer = SupplierEncryptedValue; }
                if (linguistName != null) { model.linguistName = linguistName; }
                if (PostcodeValue != null) { model.postcodeValue = PostcodeValue; }
                if (EmailValue != null) { model.emailValue = EmailValue; }
                if (SearchResults != null) { model.LinguistSearchList = SearchResults; }
                if (MasterFieldsValues != null) { model.MasterFieldsIds = MasterFieldsValues; }
                if (LanguageServicesValues != null) { model.LanguageServiceIds = LanguageServicesValues; model.defaultLanguageServiceIds = LanguageServicesValues; }
                if (SubFieldsValues != null) { model.SubFieldsIds = SubFieldsValues; }
                if (MediaTypesValues != null) { model.MediaTypeIds = MediaTypesValues; }
                if (SupplierEncryptedValue != null) { model.suppliersEncryptedComputer = SupplierEncryptedValue; }
                if (SupplierNDAValue != null) { model.suppliersNDA = SupplierNDAValue; }
                if (rbPreferred != null) { model.prefSuppliersType = rbPreferred; }
                if (rbPreferredObject != null) { model.prefSuppliersForType = rbPreferredObject; }
                if (KeyWords != null) { model.keyWords = KeyWords; }
                if (PreferredObjectValue != null) { model.prefSuppliersID = PreferredObjectValue; }
                if (rbJobsObject != null) { model.JobsDoneType = rbJobsObject; }
                if (JobsDoneForClient != null) { model.JobsID = JobsDoneForClient; }
                if (ProfessionalBodiesValues != null) { model.ProfessionalBodyIds = ProfessionalBodiesValues; }
                if (VendorTypeValues != null) { model.VendorTypeIds = VendorTypeValues; }
                model.LanguageServices = await cachedService.GetAllLanguageServicesCached();
                model.Countries = await cachedService.GetAllCountriesCached();
                model.DefaultLanguageServices = await cachedService.GetAllDefaultLanguageServicesCached();
                model.Languages = await cachedService.GetAllLanguagesCached();
                model.Softwares = await cachedService.GetSoftware();
                model.MasterFields = await cachedService.GetMasterFields();
                model.supplierStatus = await cachedService.GetSupplierStatusCached();
                model.SpecialRates = await cachedService.GetSupplierSpecialisedRate();
                model.SupplierAvailability = await cachedService.GetAvailability();
                model.MediaTypes = await cachedService.GetMediaTypes();
                model.gdprStatusList = await cachedService.getGdprStatus();
                model.RateUnitsList = await cachedService.getRateUnits();
                model.AppliesToList = await cachedService.getAppliesTo();
                model.LinguisticProfessionalBodies = await cachedService.getLingProfessionalBodies();
                model.VendorTypes = await cachedService.getLingVendorTypes();
                return View("Views/Linguists/LinguistSearch.cshtml", model);
            }
         

            
        }


    }
}
