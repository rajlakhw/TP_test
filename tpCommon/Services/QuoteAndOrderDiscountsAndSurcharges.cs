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
    public class QuoteAndOrderDiscountsAndSurcharges : IQuoteAndOrderDiscountsAndSurcharges
    {
        private readonly IRepository<QuoteAndOrderDiscountsAndSurcharge> DiscountAndSurchargeRepository;

        public QuoteAndOrderDiscountsAndSurcharges(IRepository<QuoteAndOrderDiscountsAndSurcharge> repository)
        {
            this.DiscountAndSurchargeRepository = repository;
        }

        public async Task<QuoteAndOrderDiscountsAndSurcharge> GetById(int ID)
        {
            var result = await DiscountAndSurchargeRepository.All().Where(a => a.Id == ID).FirstOrDefaultAsync();

            return result;
        }

        public async Task<QuoteAndOrderDiscountsAndSurcharge> RemoveSurchargeOrDiscount(int ID, short? deletedByUserId)
        {
            var surchargeordiscount = await GetById(ID);

            surchargeordiscount.DeletedDateTime = GeneralUtils.GetCurrentUKTime();
            surchargeordiscount.DeletedByEmployeeId = deletedByUserId;

            DiscountAndSurchargeRepository.Update(surchargeordiscount);
            await DiscountAndSurchargeRepository.SaveChangesAsync();

            return surchargeordiscount;
        }

        public async Task<QuoteAndOrderDiscountsAndSurcharge> UpdateSurchargeOrDiscount(int ID, byte category, string description, bool PercOrValue, decimal amount, short? lastmodifiedByUserId)
        {
            var surchargeordiscount = await GetById(ID);

            surchargeordiscount.DiscountOrSurchargeCategory = category;
            surchargeordiscount.Description = description;
            surchargeordiscount.PercentageOrValue = PercOrValue;
            surchargeordiscount.DiscountOrSurchargeAmount = amount;
            surchargeordiscount.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
            surchargeordiscount.LastModifiedByEmployeeId = lastmodifiedByUserId;

            DiscountAndSurchargeRepository.Update(surchargeordiscount);
            await DiscountAndSurchargeRepository.SaveChangesAsync();

            return surchargeordiscount;
        }

        public async Task<QuoteAndOrderDiscountsAndSurcharge> CreateSurchargeOrDiscount(bool surchargeordiscounttype, byte category, string description, bool PercOrValue, decimal amount, short createdByByUserId)
        {
            QuoteAndOrderDiscountsAndSurcharge surchargeordiscount = new QuoteAndOrderDiscountsAndSurcharge();

            surchargeordiscount.DiscountOrSurcharge = surchargeordiscounttype;
            surchargeordiscount.DiscountOrSurchargeCategory = category;
            surchargeordiscount.Description = description;
            surchargeordiscount.PercentageOrValue = PercOrValue;
            surchargeordiscount.DiscountOrSurchargeAmount = amount;
            surchargeordiscount.CreatedDateTime = GeneralUtils.GetCurrentUKTime();
            surchargeordiscount.CreatedByEmployeeId = createdByByUserId;

            await DiscountAndSurchargeRepository.AddAsync(surchargeordiscount);
            await DiscountAndSurchargeRepository.SaveChangesAsync();

            //using (var cdbontext = new TPCoreProductionContext())
            //{
            //    cdbontext.Add(surchargeordiscount);
            //    cdbontext.SaveChanges();
            //}

            return surchargeordiscount;
        }
    }
}
