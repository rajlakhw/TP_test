using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data.Repositories;
using Data;

namespace Services.Interfaces
{
    public interface ITPExtranetAccessLevelService : IService
    {
        Task<List<ExtranetAccessLevels>> GetAccessLevelsForClientAsync(int orgId);

    }
}
