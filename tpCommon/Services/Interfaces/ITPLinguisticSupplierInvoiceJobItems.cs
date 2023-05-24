using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Services.Interfaces
{
    public interface ITPLinguisticSupplierInvoiceJobItems : IService
    {
        Task<List<LinguisticSupplierInvoiceJobItem>> GetInvoicesByJobItemID(int jobitemID);
    }
}
