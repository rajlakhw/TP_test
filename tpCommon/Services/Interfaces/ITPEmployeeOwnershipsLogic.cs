using System.Threading.Tasks;
using Services.Common;
using Global_Settings;
using System;
using ViewModels.EmployeeOwnerships;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ITPEmployeeOwnershipsLogic : IService
    {
        Task<EmployeeOwnershipRelationshipViewModel> GetEmployeeOwnershipForDataObjectAndOwnershipType(int DataObjectID, Enumerations.DataObjectTypes DataObjectType, Enumerations.EmployeeOwnerships OwnershipTypeID, DateTime? InForceAsOfDateTime = null);
        Task<IEnumerable<EmployeeOwnershipRelationshipViewModel>> GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(int DataObjectID, Enumerations.DataObjectTypes DataObjectType, Enumerations.EmployeeOwnerships[] OwnershipTypes, DateTime? InForceAsOfDateTime = null, bool ReceivesNotificationOnly = true);
        Task<IEnumerable<EmployeeOwnershipDetailsViewModel>> GetEmployeeOwnershipForDataObjectAndOwnershipType(int DataObjectID, Enumerations.DataObjectTypes DataObjectType);
    }
}
