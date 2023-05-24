using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using Global_Settings;
using Microsoft.Extensions.Configuration;
using ViewModels.DesignPlus;

namespace Services
{
    public class TPDesignPlusService : ITPDesignPlusService
    {
        private IRepository<ClientDesignFile> clientDesignRepo;
        private readonly IConfiguration configuration;
        private readonly GlobalVariables GlobalVars;

        public TPDesignPlusService(IRepository<ClientDesignFile> _clientDesignRepo, IConfiguration configuration1)
        {
            this.clientDesignRepo = _clientDesignRepo;
            this.configuration = configuration1;
            GlobalVars = new GlobalVariables();
            configuration1.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
        }

        public async Task<DesignPlusModel> GetDesignPlusFileDetails(int designPlusFile)
        {
            var clientDesignFile = await clientDesignRepo.All().Where(c => c.Id == designPlusFile).FirstOrDefaultAsync();

            var result = new DesignPlusModel(configuration) { DesignPlusFile = clientDesignFile };

            return result;
        }
    }
}
