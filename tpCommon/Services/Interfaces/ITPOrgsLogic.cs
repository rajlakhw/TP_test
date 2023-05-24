using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.Organisation;
using ViewModels.Contact;
using Data;

namespace Services.Interfaces
{
    public interface ITPOrgsLogic : IService
    {
        Task<Org> GetOrgDetails(int ID);
        Task<List<String>> GetAllOrgSuggestionsResults<Org>(string orgIDOrNameToSearch);
        Task<Dictionary<int, string>> GetAllOrgsIdAndName(bool onlyGetOrgsWithJobs = true);
        Task<List<string>> GetAllTimesheetOrgsForOrgGroupString(string AllGroupIDString);
        Task<List<string>> GetAllOrgsForOrgGroupString(string AllGroupIDString, bool onlyGetOrgsWithJobs = true);
        Task<List<string>> GetAllOrgsForInternalExternalFilters(string InternalExternalOrAll);
        Task<OrganisationViewModel> GetOrg(int Id);
        Task<OrgPageUpdateModel> Update(OrgPageUpdateModel org);
        Task<IEnumerable<DefaultInvoiceContactViewModel>> GetDefaultInvoiceContanctsForOrg(int orgId, int? orgGroupId);
        Task<IEnumerable<OrgIndustryRelationshipViewModel>> GetOrgIdustryRelationship(int orgId);
        Task<IEnumerable<OrgTechnologyRelationshipViewModel>> GetOrgTechnologyRelationship(int orgId);
        string NetworkPriceListInfoDirectoryPathForApp(int orgId, byte? jobServerLocationForApp);
        Task<IEnumerable<ApprovedOrBlockedLinguistTableViewModel>> GetApprovedOrBlockedLinguists(int dataObjectId, int dataTypeId);
        void DisableTranslateOnlineForAllExtranetUsersForOrg(int orgId);
        Task<int> GetTranslateonlineRemainingUsers(int orgId);
        Task<int> GetDesignPlusRemainingUsers(int orgId);
        Task<IEnumerable<ContactModel>> GetAllExtranetUsersForOrg(int orgId);
        Task<String> JobServerLocationForUser(int orgId);
        Task<String> JobServerLocationForApp(int orgId);


    }
}
