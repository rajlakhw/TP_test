using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class TPLocalCurrencyInfo : ITPLocalCurrencyInfo
    {
        private readonly IRepository<LocalCurrencyInfo> currencyRepository;
        public TPLocalCurrencyInfo(IRepository<LocalCurrencyInfo> repository)
        {
            this.currencyRepository = repository;
        }
        public async Task<LocalCurrencyInfo> GetById(int ID)
        {
            var result = await currencyRepository.All().Where(a => a.CurrencyId == ID).FirstOrDefaultAsync();
            return result;
        }
        public async Task<IEnumerable<LocalCurrencyInfo>> GetAll() => await currencyRepository.All().ToListAsync();
    }
}
