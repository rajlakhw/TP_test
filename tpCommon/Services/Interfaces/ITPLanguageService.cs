using Data;
using Data.Repositories;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITPLanguageService : IService
    {
        Task<List<LanguageService>> GetLanguageServices();
    }
}
