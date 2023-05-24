using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.PGD;
using Data;

namespace Services.Interfaces
{
    public interface IPGDService : IService
    {
        Task<IEnumerable<DropdownListViewModel>> GetAllVODropdownLists();
        Task<IEnumerable<DropdownListItemViewModel>> GetAllVODropdownListsItems();
        Task<JobOrderPgddetail> GetByJobOrderId(int JobOrderID);
        Task<JobItemPgddetail> GetByJobItemId(int JobOrderID);
        Task<JobOrderPgddetail> UpdateCLSJobOrderInformation(int JobOrderId, string ThirdPartyID, string ProductionContact, string ProjectStatus, string ICPONumber, bool glossaryUpdated);
        Task<JobOrderPgddetail> UpdatePGDJobOrderInformation(int JobOrderId, decimal ApprovedEndClientCharge, short endclientcurrencyID, string bshfnumber, string isnumber, decimal ApprovedEndClientChargeGBP);
        Task<JobItemPgddetail> UpdateCLSJobItemInformation(int JobItemId, string Markets, string Service, string AssetsOverview, bool CopydeckStored, string VOTalent, bool BuyoutAgreementSigned, string UsageType, int UsageDuration, System.DateTime UsageStartDate, System.DateTime UsageEndDate);
    }
}
