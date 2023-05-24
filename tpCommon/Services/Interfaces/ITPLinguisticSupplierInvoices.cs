using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITPLinguisticSupplierInvoices : IService
    {
        Task<List<Data.LinguisticSupplierInvoice>> GetInvoicesByLinguistID(int LinguistID);
    }
}
