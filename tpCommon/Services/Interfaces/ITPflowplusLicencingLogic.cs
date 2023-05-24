using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;
using ViewModels.flowplusLicences;

namespace Services.Interfaces
{
    public interface ITPflowplusLicencingLogic : IService
    {
        Task<flowPlusLicenceMapping> GetflowPlusLicencingDetailsForDataObject(int dataObjectID, short dataObjectTypeID);
        Task<flowPlusLicenceMapping> GetflowPlusLicencingMappingDetails(int mappingId);
        Task<flowPlusLicenceModel> GetflowPlusLicence(int licenceID);
        Task<flowPlusLicences> GetflowPlusLicenceObj(int licenceID);
        Task<flowPlusApplications> GetflowPlusAppDetails(int appId);
        Task<List<flowPlusChargeFrequency>> GetAllFlowPlusCostFrequencies();
        Task<flowPlusLicenceMapping> CreateFlowPlusLicenceMapping(int AccessForDataObjectID, byte AccessForDataObjectTypeID, bool CreateSingleOrderForAllLicences, string Notes,
                                                                       int? flowplusLicenceID, int? reviewPlusLicenceID, int? translateOnlineLicenceID,
                                                                       int? designPlusLicenceID, int? AIOrMTLicenceID, int? CMSLicenceID);
        Task<flowPlusLicenceMapping> UpdateFlowPlusLicenceMapping(int AccessForDataObjectID, byte AccessForDataObjectTypeID, bool CreateSingleOrderForAllLicences, string Notes,
                                                                       int? flowplusLicenceID, int? reviewPlusLicenceID, int? translateOnlineLicenceID,
                                                                       int? designPlusLicenceID, int? AIOrMTLicenceID, int? CMSLicenceID);
        Task<flowPlusLicences> CreateFlowPlusLicence(byte ApplicationID, decimal AppCost, bool DemoEnabled, bool IsEnabled, short LastEnabledByEmpID,
                                                                         int? OrderContactID);


        Task<flowPlusLicences> UpdateFlowPlusLicence(int LicenceId, decimal AppCost, bool DemoEnabled, bool IsEnabled, short loggedInEmployee,
                                                                         int? OrderContactID);
        Task<flowPlusLicences> UpdateOrderSetUpDatesOfLicence(int licenceId, DateTime previousOrderCreationDate, DateTime nextOrderCreationDate);

    }
}
