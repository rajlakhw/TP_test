using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;
using ViewModels.DesignPlus;

namespace Services.Interfaces
{
    public interface ITPDesignPlusService : IService
    {
        Task<DesignPlusModel> GetDesignPlusFileDetails(int designPlusFile);
    }
}
