using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class TPExtranetAccessLevelService : ITPExtranetAccessLevelService
    {
        private readonly IRepository<ExtranetAccessLevels> extranetAccessLevelsRepo;
        private readonly IRepository<ExtranetAvailableAccessLevel> extranetAvailableAccessLevelsRepo;
        private readonly IRepository<Org> orgRepo;

        public TPExtranetAccessLevelService(IRepository<ExtranetAccessLevels> _extranetAccessLevelsRepo,
                                            IRepository<ExtranetAvailableAccessLevel> _extranetAvailableAccessLevelsRepo,
                                            IRepository<Org> _orgRepo)
        {
            this.extranetAccessLevelsRepo = _extranetAccessLevelsRepo;
            this.extranetAvailableAccessLevelsRepo = _extranetAvailableAccessLevelsRepo;
            this.orgRepo = _orgRepo;
        }
        public async Task<List<ExtranetAccessLevels>> GetAccessLevelsForClientAsync(int orgId)
        {
            var thisOrg = await orgRepo.All().Where(o => o.Id == orgId).FirstOrDefaultAsync();
            var result = await extranetAvailableAccessLevelsRepo.All().Where(a => (a.DataObjectId == orgId && a.DataObjectTypeId == 2) || 
                                                                            (a.DataObjectId == thisOrg.OrgGroupId && a.DataObjectTypeId == 3))
                               .Join(extranetAccessLevelsRepo.All(),
                                     a => a.AccessLevelId,
                                     e => e.Id,
                                     (a,e) => new { AccessLevels = e }).Select(a => a.AccessLevels).ToListAsync();

            // many extranet users will not be assigned any customised roles, so if this is 
            // not the case, then make only the standard roles available
            if (result.Count == 0)
            {
                List<ExtranetAccessLevels> allAccessTypes = new List<ExtranetAccessLevels>();
                allAccessTypes.Add(await extranetAccessLevelsRepo.All().Where(a => a.Id == 1).FirstOrDefaultAsync()); //standard
                allAccessTypes.Add(await extranetAccessLevelsRepo.All().Where(a => a.Id == 2).FirstOrDefaultAsync()); //admin
                allAccessTypes.Add(await extranetAccessLevelsRepo.All().Where(a => a.Id == 3).FirstOrDefaultAsync()); //group admin
                allAccessTypes.Add(await extranetAccessLevelsRepo.All().Where(a => a.Id == 5).FirstOrDefaultAsync()); //reviewer

                result = allAccessTypes;
            }
            return result;
        }
    }
}
