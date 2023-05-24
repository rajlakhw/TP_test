using System.Threading.Tasks;
using System.Collections.Generic;
using Data;
using Services.Common;
using ViewModels.ClientInvoice;

namespace Services.Interfaces
{
    public interface ITPClientInvoicesLogic : IService
    {
        Task<ClientInvoice> GetById(int ClientInvoiceID);
        Task<int> GetAutoClientInvoicingSettingsId(int dataObjectId, int dataTypeId);
        Task<ClientInvoiceViewModel> GetViewModelById(int id);
        Task<List<ClientInvoice>> GetOverdueClientInvoicesForOrgID(int orgID);
    }
}
