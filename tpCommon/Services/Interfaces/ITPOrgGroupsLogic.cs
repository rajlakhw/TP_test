using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Services.Common;
using ViewModels.OrgGroup;
using System;

namespace Services.Interfaces
{
    public interface ITPOrgGroupsLogic : IService
    {
        Task<OrgGroup> GetOrgGroupDetails(int ID);
        Task<List<JobGroupResults>> GetJobOrders(string GroupID);
        Task<(int, int)> GetAllEnquiriesCountForDataObjectAndType(int dataObjectId, int dataTypeId, string searchTerm);
        Task<IEnumerable<EnquiriesGroupResults>> GetEnquiriesbOrdersForDataObjectAndType(int dataObjectId, int dataTypeId, int pageNumber, int pageSize, string searchTerm, int columnToOrderBy, string orderDirection);
        Task<List<Brands>> GetBrandsList();
        Task<OrgGroupViewModel> GroupUpdate(ViewModels.OrgGroup.OrgGroupViewModel group, bool HQUpdate);
        Task<List<OrgResults>> GetOrgs(string GroupID);
        Task<List<OrgPriceListsResults>> GetPriceLists(string GroupID);
        bool HasAnyVolumeDiscountSetUp(int GroupID);
        Task<Boolean> CheckOrgLevelLicencesForGroup(int groupId);
        Task<Boolean> CheckIfGroupLevelLicenceExists(int groupId);
        Task<OrgGroup> UpdateShowProofreadingOptionSetting(int groupId, bool ShowProofreadingOptionToClient);
        Task<OrgGroupViewModel> GroupContractUpdate(ViewModels.OrgGroup.OrgGroupViewModel group);
    }

}
