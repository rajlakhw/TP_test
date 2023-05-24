using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;

namespace Services.Interfaces
{
    public interface ITMSClientSearch : IService
    {
        Task<List<ViewModels.TMSClientSearch.TMSClientTechnology>> GetTechnologies();
        Task<List<ViewModels.TMSClientSearch.TMSCountry>> GetCountries();
        Task<List<ViewModels.TMSClientSearch.OrgGroupResults>> GetOrgGroupResultsByType(string groupType, string groupName);
        Task<List<ViewModels.TMSClientSearch.OrgGroupResults>> GetOrgGroupResultsByID(int groupID);
        Task<List<ViewModels.TMSClientSearch.ContactResults>> GetContactResultsByID(int contactID);
        Task<List<ViewModels.TMSClientSearch.ContactResults>> GetContactResults(string contactName, string contactEmailAddress, string orgName, string groupName, string country);
        Task<List<ViewModels.TMSClientSearch.OrgSearchResults>> GetOrgResultsByID(string orgID);
        Task<List<ViewModels.TMSClientSearch.OrgSearchResults>> GetOrgResults(string orgName, string groupName, string EmailAddressVlaue, string PostCodeValue, string HFMCodeISValue, string HFMCodeBSValue, string CountriesValue, string ClientTechnologyValue);
    }
}
