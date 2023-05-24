using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Services.Interfaces;
using System.IO;
using ViewModels.Common;
using Global_Settings;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class TPEnquiryQuoteItemsService : ITPEnquiryQuoteItemsService
    {
        private readonly IRepository<EnquiryQuoteItem> enquiryQuoteItemsRepo;

        public TPEnquiryQuoteItemsService(IRepository<EnquiryQuoteItem> enquiryquoteitemsrepository)
        {
            this.enquiryQuoteItemsRepo = enquiryquoteitemsrepository;
        }

        public async Task<EnquiryQuoteItem> GetEnquiryQuoteItemById(int enquiryQuoteItemId)
        {
            var result = await enquiryQuoteItemsRepo.All().Where(e => e.Id == enquiryQuoteItemId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<EnquiryQuoteItem>> GetEnquiryQuoteItemsByEnquiryId(int enquiryId)
        {
            var result = await enquiryQuoteItemsRepo.All().Where(e => e.EnquiryId == enquiryId && e.DeletedDateTime == null).ToListAsync();

            return result;
        }

        public async Task<ViewModels.Enquiries.EnquiryQuoteItemsViewModel> GetViewModelById(int enquiryQuoteItemId)
        {
            var result = await enquiryQuoteItemsRepo.All().Where(e => e.Id == enquiryQuoteItemId)
                .Select(x => new ViewModels.Enquiries.EnquiryQuoteItemsViewModel()
                {
                    Id = x.Id,
                    LanguageServiceID = x.LanguageServiceId,
                    SourceLanguageIANACode = x.SourceLanguageIanaCode,
                    TargetLanguageIANACode = x.TargetLanguageIanaCode
                })
                .FirstOrDefaultAsync();


            return result;
        }

        public async Task<IEnumerable<ViewModels.Enquiries.EnquiryQuoteItemsViewModel>> GetViewModelByEnquiryId(int enquiryId)
        {
            var result = await enquiryQuoteItemsRepo.All().Where(e => e.EnquiryId == enquiryId && e.DeletedDateTime == null)
                .Select(x => new ViewModels.Enquiries.EnquiryQuoteItemsViewModel()
                {
                    Id = x.Id,
                    LanguageServiceID = x.LanguageServiceId,
                    SourceLanguageIANACode = x.SourceLanguageIanaCode,
                    TargetLanguageIANACode = x.TargetLanguageIanaCode
                })
                .ToListAsync();


            return result;
        }

    }
}
