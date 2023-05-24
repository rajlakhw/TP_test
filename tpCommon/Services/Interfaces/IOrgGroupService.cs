using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.Common;
using System;

namespace Services.Interfaces
{
    public interface IOrgGroupService : IService
    {
        //Task<List<string>> GetAllOrgGroupsIdAndName(string orgIDOrNameToSearch);

        //Task<Dictionary<int, string>> GetAllOrgGroupsIdAndName();
        Task<IEnumerable<DropdownOptionViewModel>> GetAllAltairCorporateGroups();
        Task<List<string>> GetAllGroupsForInternalExternalFilters(string InternalExternalOrAll);
        Task<Dictionary<int, string>> GetAllOrgGroupsIdAndName(bool onlyGetGroupWithJobs = true);
    }
}
