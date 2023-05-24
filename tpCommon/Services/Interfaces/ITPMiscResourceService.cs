using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;

namespace Services.Interfaces
{
    public interface ITPMiscResourceService : IService
    {
        Task<MiscResource> GetMiscResourceByName(string ResourceName, string LangIANACode);
    }
}
