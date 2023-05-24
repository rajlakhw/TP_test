using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class TPLinguisticSupplierInvoiceTemplate : ITPLinguisticSupplierInvoiceTemplate
    {
        private readonly IRepository<LinguisticSupplierInvoiceTemplate> LinguistInvoiceTemplateRepository;

        public TPLinguisticSupplierInvoiceTemplate(IRepository<LinguisticSupplierInvoiceTemplate> repository)
        {
            this.LinguistInvoiceTemplateRepository = repository;
        }

        public async Task<LinguisticSupplierInvoiceTemplate> GetById(int ID)
        {
            var result = await LinguistInvoiceTemplateRepository.All().Where(a => a.Id == ID && a.DeletedDateTime == null).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<LinguisticSupplierInvoiceTemplate>> GetAllLinguisticSupplierInvoiceTemplates(int LinguistId)
        {
            var result = await LinguistInvoiceTemplateRepository.All().Where(a => a.LinguisticSupplierId == LinguistId && a.DeletedDateTime == null).ToListAsync();
            return result;
        }
        public async Task<LinguisticSupplierInvoiceTemplate> Update(LinguisticSupplierInvoiceTemplate model)
        {


            var Details = await LinguistInvoiceTemplateRepository.All().Where(x => x.LinguisticSupplierId == model.LinguisticSupplierId && x.DeletedDateTime == null).FirstOrDefaultAsync();
            if (Details != null)
            {

                Details.Vatnumber = model.Vatnumber;
                Details.BankAccountName = model.BankAccountName;
                Details.BankBranchAddress = model.BankBranchAddress;
                Details.BankBranchCity = model.BankBranchCity;
                Details.BankBranchCountry = model.BankBranchCountry;
                Details.BankBranchPostCode = model.BankBranchPostCode;
                Details.BankAccountName = model.BankAccountName;
                Details.BankAccountNumber = model.BankAccountNumber;
                Details.BankAccountSwiftorBic = model.BankAccountSwiftorBic;
                Details.BankAccountIban = model.BankAccountIban;
                LinguistInvoiceTemplateRepository.Update(Details);
                await LinguistInvoiceTemplateRepository.SaveChangesAsync();
            }

            return Details;

        }
    }
}
