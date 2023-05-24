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
    public class QuoteAndOrderDiscountsAndSurchargesCategories : IQuoteAndOrderDiscountsAndSurchargesCategories
    {
        private readonly IRepository<QuoteAndOrderDiscountsAndSurchargesCategory> DiscountAndSurchargeRepository;

        public QuoteAndOrderDiscountsAndSurchargesCategories(IRepository<QuoteAndOrderDiscountsAndSurchargesCategory> repository)
        {
            this.DiscountAndSurchargeRepository = repository;
        }

        public async Task<QuoteAndOrderDiscountsAndSurchargesCategory> GetById(int ID)
        {
            var result = await DiscountAndSurchargeRepository.All().Where(a => a.Id == ID).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<QuoteAndOrderDiscountsAndSurchargesCategory>> GetAll() => await DiscountAndSurchargeRepository.All().ToListAsync();

        public async Task<IEnumerable<QuoteAndOrderDiscountsAndSurchargesCategory>> GetAllDiscountCategories() => await DiscountAndSurchargeRepository.All().Where(a => a.AppliesToDiscountOrSurcharge == true).ToListAsync();
        public async Task<IEnumerable<QuoteAndOrderDiscountsAndSurchargesCategory>> GetAllSurchargeCategories() => await DiscountAndSurchargeRepository.All().Where(a => a.AppliesToDiscountOrSurcharge == false).ToListAsync();
    }
}
