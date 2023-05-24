using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.Common;

namespace Services.Interfaces
{
    public interface ITranslationMemoriesService : IService
    {
        Task<IEnumerable<DropdownOptionViewModel>> GetAllTradosTemplatesForAnOrg(int orgId, bool templatesToShowForiPlus);
    }
}
