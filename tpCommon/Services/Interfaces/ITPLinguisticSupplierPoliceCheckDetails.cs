using Data;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITPLinguisticSupplierPoliceCheckDetails : IService
    {
        List<LinguisticSupplierPoliceCheckDetail> GetAllChecks(int linguistId);
        Task<LinguisticSupplierPoliceCheckDetail> AddPoliceCheck(LinguisticSupplierPoliceCheckDetail model);

    }
}
