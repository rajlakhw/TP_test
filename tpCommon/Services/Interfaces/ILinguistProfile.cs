using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;

namespace Services.Interfaces
{
    public interface ILinguistProfile : IService
    {
        Task<List<ViewModels.LinguistSearch.LinguistSearchResults>> GetLinguistResults(string NumberValue, string linguistName,
            string EmailValue, string PostcodeValue, string KeyWords, string MasterFieldsValues, string LanguageServicesValues
            , string SubFieldsValues, string MediaTypesValues, string myRangeValue
            , string SourceLanguagesValues, string TargetLanguageValues
            , string SpecialRatesValues, string supplierStatusValues
            , string CountriesNationalyValues, string CountryOfResidenceValues
            , string SoftwaresValues, string gdprStatusListValues
            , string SupplierAvailabilityValues, string RateUnitsListValues, string AppliesToValues, string SupplierNDAValue
            , string SupplierMemoryMatchValue, string SupplierEncryptedValue, string rbPreferred, string rbPreferredObject,
            string PreferredObjectValue, string rbJobsObject, string JobsDoneForClient);

        Task<List<ViewModels.LinguistSearch.LinguistSearchResults>> GetLinguistResultsById(string NumberValue);
    }
}
