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
   public class TPLinguisticSupplierInvoices : ITPLinguisticSupplierInvoices
    {
        private readonly IRepository<LinguisticSupplierInvoice> invoiceRepository;


        public TPLinguisticSupplierInvoices(IRepository<LinguisticSupplierInvoice> repository)
        {
            this.invoiceRepository = repository;

        }

        public async Task<List<Data.LinguisticSupplierInvoice>> GetInvoicesByLinguistID(int LinguistID)
        {
            var result = await invoiceRepository.All().Where(o => o.LinguisticSupplierId == LinguistID).ToListAsync();
            return result;
        }

    }
}
