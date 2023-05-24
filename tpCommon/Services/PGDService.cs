using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.PGD;

namespace Services
{
    public class PGDService : IPGDService
    {
        private readonly IRepository<VodropdownItem> vODropdownRepository;
        private readonly IRepository<VodropdownList> vODropdownListsRepository;
        private readonly IRepository<JobOrderPgddetail> orderpgdRepository;
        private readonly IRepository<JobItemPgddetail> itempgdRepository;
        private ITPContactsLogic tpcontactService;
        private ITPJobOrderService tpjobOrderService;
        private ITPOrgsLogic tPOrgsLogic;
        public PGDService(IRepository<VodropdownItem> VODropdownRepository, IRepository<VodropdownList> vODropdownListsRepository, IRepository<JobOrderPgddetail> orderpgdRepository, IRepository<JobItemPgddetail> itempgdRepository, ITPContactsLogic _tpcontactService,ITPJobOrderService _tpjobOrderService, ITPOrgsLogic _tPOrgsLogic)
        {
            vODropdownRepository = VODropdownRepository;
            this.vODropdownListsRepository = vODropdownListsRepository;
            this.orderpgdRepository = orderpgdRepository;
            this.itempgdRepository = itempgdRepository;
            tpcontactService = _tpcontactService;
            tpjobOrderService = _tpjobOrderService;
            tPOrgsLogic = _tPOrgsLogic;
        }

        public async Task<IEnumerable<DropdownListItemViewModel>> GetAllVODropdownListsItems() => await vODropdownRepository.All().Select(x => new DropdownListItemViewModel() { Id = x.Id, VodropdownListId = x.VodropdownListId, Idvalue = x.Idvalue, Exclude = x.Exclude, Value = x.Value }).ToListAsync();
        public async Task<IEnumerable<DropdownListViewModel>> GetAllVODropdownLists() => await vODropdownListsRepository.All().Select(x => new DropdownListViewModel() { Id = x.Id, VodropdownListName = x.VodropdownListName, Required = x.Required }).ToListAsync();

        public async Task<JobOrderPgddetail> GetByJobOrderId(int JobOrderID)
        {
            var result = await orderpgdRepository.All().Where(a => a.JobOrderId == JobOrderID).FirstOrDefaultAsync();

            return result;
        }

        public async Task<JobItemPgddetail> GetByJobItemId(int JobOrderID)
        {
            var result = await itempgdRepository.All().Where(a => a.JobItemId == JobOrderID).FirstOrDefaultAsync();

            return result;
        }

        public async Task<JobOrderPgddetail> UpdateCLSJobOrderInformation(int JobOrderId, string ThirdPartyID, string ProductionContact, string ProjectStatus, string ICPONumber, bool glossaryUpdated)
        {
            var orderPGDdetails = await GetByJobOrderId(JobOrderId);
            
            if (orderPGDdetails != null)
            {
                // maybe add security check later here and autoMapper
                orderPGDdetails.ThirdPartyId = ThirdPartyID;
                orderPGDdetails.ProductionContact = ProductionContact;
                orderPGDdetails.ProjectStatus = ProjectStatus;
                orderPGDdetails.Icponumber = ICPONumber;
                orderPGDdetails.GlossaryUpdated = glossaryUpdated;

                orderpgdRepository.Update(orderPGDdetails);
                await orderpgdRepository.SaveChangesAsync();
            }
            else
            {
                var jobOrderDetails = tpjobOrderService.GetById(JobOrderId);
                var contactDetails = tpcontactService.GetById(jobOrderDetails.Result.ContactId);
                var orgDetails = tpjobOrderService.GetById(contactDetails.Result.OrgId);

                orderPGDdetails = new JobOrderPgddetail()
                {
                    JobOrderId = JobOrderId,
                    ThirdPartyId = ThirdPartyID == "" ? null : ThirdPartyID,
                    ProductionContact = ProductionContact == "" ? null : ProductionContact,
                    ProjectStatus = ProjectStatus == "" ? null : ProjectStatus,
                    Icponumber = ICPONumber == "" ? null : ICPONumber,
                    GlossaryUpdated = glossaryUpdated,
                    Bshfmnumber = orgDetails.Result.OrgHfmcodeBs == null || orgDetails.Result.OrgHfmcodeBs == ""? "GB8042" : orgDetails.Result.OrgHfmcodeBs,
                    Isnumber = orgDetails.Result.OrgHfmcodeIs == null || orgDetails.Result.OrgHfmcodeIs == "" ? "GB8042" : orgDetails.Result.OrgHfmcodeIs,
                    ApprovedEndClientCharge = 0,
                    EndClientChargeCurrencyId = 4

                };
                await orderpgdRepository.AddAsync(orderPGDdetails);
                await orderpgdRepository.SaveChangesAsync();
            }
            return orderPGDdetails;
        }

        public async Task<JobOrderPgddetail> UpdatePGDJobOrderInformation(int JobOrderId, decimal ApprovedEndClientCharge, short endclientcurrencyID, string bshfnumber, string isnumber, decimal ApprovedEndClientChargeGBP)
        {
            var orderPGDdetails = await GetByJobOrderId(JobOrderId);
            if (orderPGDdetails != null)
            {
                // maybe add security check later here and autoMapper
                orderPGDdetails.ApprovedEndClientCharge = ApprovedEndClientCharge;
                orderPGDdetails.EndClientChargeCurrencyId = endclientcurrencyID;
                orderPGDdetails.Bshfmnumber = bshfnumber;
                orderPGDdetails.Isnumber = isnumber;
                orderPGDdetails.ApprovedEndClientChargeGbp = ApprovedEndClientChargeGBP;

                orderpgdRepository.Update(orderPGDdetails);
                await orderpgdRepository.SaveChangesAsync();
            }
            else
            {
                if (bshfnumber == "")
                {
                    bshfnumber = "GB8042";
                }
                if (isnumber == "")
                {
                    isnumber = "GB8042";
                }
                orderPGDdetails = new JobOrderPgddetail()
                {
                    JobOrderId = JobOrderId,
                    ApprovedEndClientCharge = ApprovedEndClientCharge,
                    EndClientChargeCurrencyId = endclientcurrencyID,
                    Bshfmnumber = bshfnumber,
                    Isnumber = isnumber,
                    ApprovedEndClientChargeGbp = ApprovedEndClientChargeGBP,
                    ProjectStatus = "1. Brief"
                };
                await orderpgdRepository.AddAsync(orderPGDdetails);
                await orderpgdRepository.SaveChangesAsync();
            }
            return orderPGDdetails;
        }

        public async Task<JobItemPgddetail> UpdateCLSJobItemInformation(int JobItemId, string Markets, string Service, string AssetsOverview, bool CopydeckStored, string VOTalent, bool BuyoutAgreementSigned, string UsageType, int UsageDuration, System.DateTime UsageStartDate, System.DateTime UsageEndDate)
        {
            var itemPGDdetails = await GetByJobItemId(JobItemId);
            if (itemPGDdetails != null)
            {
                // maybe add security check later here and autoMapper
                itemPGDdetails.Markets = Markets;
                itemPGDdetails.Service = Service;
                itemPGDdetails.AssetsOverview = AssetsOverview;
                itemPGDdetails.CopydeckStored = CopydeckStored;
                itemPGDdetails.Votalent = VOTalent;
                itemPGDdetails.BuyoutAgreementSigned = BuyoutAgreementSigned;
                itemPGDdetails.UsageType = UsageType;
                if (UsageDuration != -1)
                {
                    itemPGDdetails.UsageDuration = UsageDuration;
                }
                if (UsageStartDate != System.DateTime.MinValue)
                {
                    itemPGDdetails.AirDate = UsageStartDate;
                    itemPGDdetails.UsageStartDate = UsageStartDate;
                }

                if (UsageEndDate != System.DateTime.MinValue)
                {
                    itemPGDdetails.UsageEndDate = UsageEndDate;
                }

                itempgdRepository.Update(itemPGDdetails);
                await itempgdRepository.SaveChangesAsync();
            }
            else
            {

                itemPGDdetails = new JobItemPgddetail()
                {
                    JobItemId = JobItemId,
                    Markets = Markets,
                    Service = Service,
                    AssetsOverview = AssetsOverview,
                    CopydeckStored = CopydeckStored,
                    Votalent = VOTalent,
                    BuyoutAgreementSigned = BuyoutAgreementSigned,
                    UsageType = UsageType,
                    UsageDuration = UsageDuration == -1 ? null: UsageDuration,
                    AirDate = UsageStartDate == System.DateTime.MinValue ? null : UsageStartDate,
                    UsageStartDate = UsageStartDate == System.DateTime.MinValue ? null : UsageStartDate,
                    UsageEndDate = UsageEndDate == System.DateTime.MinValue ? null : UsageEndDate
                };
                await itempgdRepository.AddAsync(itemPGDdetails);
                await itempgdRepository.SaveChangesAsync();
            }
            return itemPGDdetails;
        }
    }
}
