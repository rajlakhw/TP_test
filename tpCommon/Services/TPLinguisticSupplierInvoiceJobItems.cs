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
    public class TPLinguisticSupplierInvoiceJobItems : ITPLinguisticSupplierInvoiceJobItems
    {
        private readonly IRepository<LinguisticSupplierInvoiceJobItem> invoiceRepository;


        public TPLinguisticSupplierInvoiceJobItems(IRepository<LinguisticSupplierInvoiceJobItem> repository)
        {
            this.invoiceRepository = repository;

        }
        public async Task<List<LinguisticSupplierInvoiceJobItem>> GetInvoicesByJobItemID(int jobitemID)
        {
            var result = await invoiceRepository.All().Where(o => o.JobItemId == jobitemID).ToListAsync();
            return result;
        }
    }
}
