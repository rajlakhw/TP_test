using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Services.Common;
using ViewModels.EmployeeOwnerships;

namespace Services.Interfaces
{
    public interface ITPOwnershipsLogic : IService
    {
        Task<List<EmployeeOwnershipDetailsViewModel>> GetAllOwnershipsForDataObjectID(int dataObjectID, short dataObjectTypeId, DateTime inForceDateTime, string searchTerm, int pageNumber, int pageSize);

        int GetAllOwnershipsForDataObjectIDCount(int dataObjectID, short dataObjectTypeId, DateTime inForceDateTime, string searchTerm);

        //Task<List<Org>> GetAllOwnershipOrgsForDataObjectID(int dataObjectID, short dataObjectTypeId, DateTime inForceDateTime );

        //Task<List<EmployeeOwnershipType>> GetAllOwnershipTypesForDataObjectID(int dataObjectID, short dataObjectTypeId, DateTime inForceDateTime);
    }
}
