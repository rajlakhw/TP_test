using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;

namespace Services.Interfaces
{
    public interface ITPEndClient : IService
    {
        Task<Data.EndClient> GetEndClientDetails<EndClient>(string EndClientName);

        Task<Data.EndClientData> GetEndClientDataDetails<EndClientData>(int EndClientID, string DataName, int DataID);

        Task InsertNewEndClientData(string Name, int EndClientID, int DataObjectID, int EmployeeID);

        Task<List<ViewModels.TPEndClient.TPEndClientViewModel>> GetAllEndClients();

        Task<List<Data.EndClient>> GetAllEndClient();

        Task<List<Data.EndClientData>> GetAllBrands(int? EndClientID = null);

        Task<List<Data.EndClientData>> GetAllCategories(int? EndClientID = null);

        Task<List<Data.EndClientData>> GetAllCampaigns(int? EndClientID = null);

        Task<List<Data.EndClient>> GetAllEndClientLoggedInTimesheet();

        Task<List<Data.EndClientData>> GetAllBrandsLoggedInTimesheet(int? EndClientID = null);

        Task<List<Data.EndClientData>> GetAllCategoriesLoggedInTimesheet(int? EndClientID = null);

        Task<List<Data.EndClientData>> GetAllCampaignsLoggedInTimesheet(int? EndClientID = null);


        Task InsertNewEndClient(string EndClientName, int EmployeeID);

        Task<Data.EndClient> GetEndClientByID<EndClient>(int EndClientID);

        Task<Data.EndClientData> GetEndClientDataByID<EndClient>(int EndClientDataID);

        Task<List<Data.EndClientData>> GetAllTimesheetBrandsForEndClientIDs(string allEndClientIDString);

        Task<List<Data.EndClientData>> GetAllTimesheetCategoriesForEndClientIDs(string allEndClientIDString);

        Task<List<Data.EndClientData>> GetAllTimesheetCampaignsForEndClientIDs(string allEndClientIDString);

        Task<List<Data.EndClientData>> GetAllBrandsForEndClientIDs(string allEndClientIDString);

        Task<List<Data.EndClientData>> GetAllCategoriesForEndClientIDs(string allEndClientIDString);

        Task<List<Data.EndClientData>> GetAllCampaignsForEndClientIDs(string allEndClientIDString);

    }
}
