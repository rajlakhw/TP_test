using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.Common;

namespace Services
{
    public class TPCurrenciesLogic : ITPCurrenciesLogic
    {
        private readonly IRepository<Currency> currencyRepository;
        private readonly IRepository<LocalCurrencyInfo> localCurrencyRepository;
        public TPCurrenciesLogic(IRepository<Currency> repository, IRepository<LocalCurrencyInfo> repository1)
        {
            this.currencyRepository = repository;
            this.localCurrencyRepository = repository1;
        }

        public async Task<IEnumerable<Currency>> GetAll() => await currencyRepository.All().ToListAsync();
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllENCurrencies()
        {
            var currencies = new List<DropdownOptionViewModel>();

            var query = "SELECT * FROM viewCurrenciesMultilingualInfo WHERE LanguageIANAcode = 'en'";

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = query;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    currencies.Add(new DropdownOptionViewModel()
                    {
                        Id = await result.GetFieldValueAsync<short>(0),
                        StringValue = await result.GetFieldValueAsync<string>(1),
                        Name = await result.GetFieldValueAsync<string>(4)
                    });
                }
            }

            return currencies;
        }

        public async Task<Currency> GetById(int ID)
        {
            var result = await currencyRepository.All().Where(a => a.Id == ID).FirstOrDefaultAsync();
            return result;
        }

        public async Task<LocalCurrencyInfo> GetCurrencyInfo(int currencyId, string IANACode)
        {
            var result = await currencyRepository.All()
                               .Join(localCurrencyRepository.All().Where(c => c.CurrencyId == currencyId && c.LanguageIanacode == IANACode),
                                     c => c.Id,
                                     l => l.CurrencyId,
                                     (c, l) => new { localCurrency = l }).Select(l => l.localCurrency).FirstOrDefaultAsync();

            return result;

        }
    }
}
