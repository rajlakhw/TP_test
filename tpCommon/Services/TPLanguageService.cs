using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TPLanguageService : ITPLanguageService
    {
        private readonly IRepository<LanguageService> languageRepository;
        public TPLanguageService(IRepository<LanguageService> languageRepository)
        {
            this.languageRepository = languageRepository;
        }
        public async Task<List<LanguageService>> GetLanguageServices()
        {
            var result = await languageRepository.All().Where(s => s.Hide == null).OrderBy(o=>o.Name).ToListAsync(); ;
            return result;
        }
    }
}
