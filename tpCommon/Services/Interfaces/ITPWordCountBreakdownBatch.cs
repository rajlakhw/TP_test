using Data;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;


namespace Services.Interfaces
{
    public interface ITPWordCountBreakdownBatch : IService
    {
        TPWordCountBreakdownBatchModel WordCountBreakdownBatch(string LogFilePath, Org ThisOrg, JobOrder ThisJobOrder=null, Global_Settings.Enumerations.MemoryApplications MemoryApplication = Global_Settings.Enumerations.MemoryApplications.NoneOrUnknown, bool LinguisticSupplierWordCountsOnly = false, JobItem thisJobItem = null, bool ToGetWordCountsFromOneFileOnly = false);
    }
}
