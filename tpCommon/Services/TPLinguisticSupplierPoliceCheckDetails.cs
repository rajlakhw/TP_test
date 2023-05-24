using Data;
using Data.Repositories;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TPLinguisticSupplierPoliceCheckDetails : ITPLinguisticSupplierPoliceCheckDetails
    {
        private readonly IRepository<LinguisticSupplierPoliceCheckDetail> policeCheckDetailRepository;
        public TPLinguisticSupplierPoliceCheckDetails(IRepository<LinguisticSupplierPoliceCheckDetail> policeCheckDetailRepository)
        {
            this.policeCheckDetailRepository = policeCheckDetailRepository;

        }

        public List<LinguisticSupplierPoliceCheckDetail> GetAllChecks(int linguistId)
        {
            var result = policeCheckDetailRepository.All().Where(x => x.LinguistId == linguistId && x.DeletedDate == null).ToList();
            return result;
        }

        public async Task<LinguisticSupplierPoliceCheckDetail> AddPoliceCheck(LinguisticSupplierPoliceCheckDetail model)
        {
            var thisRequest = new LinguisticSupplierPoliceCheckDetail()
            {
                LinguistId = model.LinguistId,
                PoliceCheckId = model.PoliceCheckId,
                PoliceCheckExpiryDate = model.PoliceCheckExpiryDate,
                PoliceCheckIssueDate = model.PoliceCheckIssueDate,
                LastModifiedByEmployeeId = model.LastModifiedByEmployeeId,
                LastModifiedDate = GeneralUtils.GetCurrentUKTime()
            };

            await policeCheckDetailRepository.AddAsync(thisRequest);
            await policeCheckDetailRepository.SaveChangesAsync();

            return thisRequest;
        }

    }
}
