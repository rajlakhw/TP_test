using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using Data;

namespace Services.Interfaces
{
    public interface ITPLinguisticSupplierInvoiceTemplate : IService
    {
        Task<LinguisticSupplierInvoiceTemplate> GetById(int ID);
        Task<List<LinguisticSupplierInvoiceTemplate>> GetAllLinguisticSupplierInvoiceTemplates(int LinguistId);
        Task<LinguisticSupplierInvoiceTemplate> Update(LinguisticSupplierInvoiceTemplate model);
    }
}
