using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels.Common;

namespace ViewModels.LinguistSearch
{
    public class LinguistSearchResults
    {
        public int linguistID { get; set; }
        public string linguistName { get; set; }
        public int numberOfJobs { get; set; }
        public string linguistType { get; set; }
        public string linguistStatus { get; set; }
        public string linguistRate { get; set; }
        public string linguistRateGBP { get; set; }
        public byte appliesToType { get; set; }
        public string lingSourceLang { get; set; }
        public string lingTargetLang { get; set; }
        public string lingService { get; set; }
        public string lingSubjectArea { get; set; }

        public string linguistRateRateUnit { get; set; }
        public byte lingStatusID { get; set; }
        public string minimumCharge { get; set; }
        public string specialisedRate { get; set; }
        public string appliesTo { get; set; }
        public int appliesToId { get; set; }
    }
    public class LinguistSearchViewModel
    {
        public bool isNumber { get; set; }
        public int exactID { get; set; }
        public string linguistName { get; set; }
        public string StatusIds { get; set; }
        public string SpecialRateIds { get; set; }
        public string CountryOfResidenceIds { get; set; }
        public string SoftwareIds { get; set; }
        public string keyWords { get; set; }

        public string AvailabilityIds { get; set; }
        public List<LinguistSearchResults> LinguistSearchList { get; set; }
        public string CountryOfNationalityIds { get; set; }
        public string SourceLanguageIanacodes { get; set; }
        public string TargetLanguageIanacodes { get; set; }
        public IEnumerable<DropdownOptionViewModel> LanguageServices { get; set; }

        public IEnumerable<DropdownOptionViewModel> DefaultLanguageServices { get; set; }
        public IEnumerable<DropdownOptionViewModel> Softwares { get; set; }
        public IEnumerable<DropdownOptionViewModel> SubFields { get; set; }
        public IEnumerable<DropdownOptionViewModel> MediaTypes { get; set; }
        public IEnumerable<DropdownOptionViewModel> gdprStatusList { get; set; }
        public IEnumerable<DropdownOptionViewModel> RateUnitsList { get; set; }
        public IEnumerable<DropdownOptionViewModel> AppliesToList { get; set; }
        public IEnumerable<DropdownOptionViewModel> MasterFields { get; set; }
        public IEnumerable<DropdownOptionViewModel> SupplierAvailability { get; set; }
        public IEnumerable<CountryViewModel> Countries { get; set; }
        public IEnumerable<DropdownOptionViewModel> Languages { get; set; }
        public IEnumerable<DropdownOptionViewModel> SpecialRates { get; set; }
        public IEnumerable<DropdownOptionViewModel> supplierStatus { get; set; }
        public string MediaTypeIds { get; set; }
        public int AppliesToId { get; set; }
        public int rateUnitsId { get; set; }
        public string gdprStatusIds { get; set; }
        public string SubFieldsIds { get; set; }
        public string MasterFieldsIds { get; set; }
        public string LanguageServiceIds { get; set; }
        public string defaultLanguageServiceIds { get; set; }
        public string emailValue { get; set; }
        public string suppliersNDA { get; set; }
        public string prefSuppliersType { get; set; }
        public string prefSuppliersID { get; set; }
        public string prefSuppliersForType { get; set; }
        public string JobsDoneType { get; set; }
        public string JobsID { get; set; }
        public string suppliersNewBreakdown { get; set; }
        public string postcodeValue { get; set; }
        public string suppliersEncryptedComputer { get; set; }
        public bool suppliersByRate { get; set; }
        public IEnumerable<DropdownOptionViewModel> LinguisticProfessionalBodies { get; set; }
        public string ProfessionalBodyIds { get; set; }
        public IEnumerable<DropdownOptionViewModel> VendorTypes { get; set; }
        public string VendorTypeIds { get; set; }


    }

}
