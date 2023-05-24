using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using ViewModels.Common;

namespace Services.Interfaces
{
    public interface ITPEnquiryQuoteItemsService : IService
    {
        Task<EnquiryQuoteItem> GetEnquiryQuoteItemById(int enquiryQuoteItemId);
        Task<List<EnquiryQuoteItem>> GetEnquiryQuoteItemsByEnquiryId(int enquiryId);
        Task<ViewModels.Enquiries.EnquiryQuoteItemsViewModel> GetViewModelById(int enquiryQuoteItemId);
        Task<IEnumerable<ViewModels.Enquiries.EnquiryQuoteItemsViewModel>> GetViewModelByEnquiryId(int enquiryId);
    }
}
