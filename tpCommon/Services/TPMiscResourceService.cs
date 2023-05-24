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
    public class TPMiscResourceService : ITPMiscResourceService
    {
        private IRepository<MiscResource> MiscRepository;

        public TPMiscResourceService(IRepository<MiscResource> repository)
        {
            this.MiscRepository = repository;
        }

        public async Task<MiscResource> GetMiscResourceByName(string ResourceName, string LangIANACode)
        {
            var result = await MiscRepository.All().Where(m => m.ResourceName.Equals(ResourceName) && m.LangIanacode.Equals(LangIANACode)).FirstOrDefaultAsync();

            return result;

        }
    }
}
