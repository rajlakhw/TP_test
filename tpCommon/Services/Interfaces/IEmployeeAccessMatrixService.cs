using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.EmployeeModels.EmployeeAccessMatrix;

namespace Services.Interfaces
{
    public interface IEmployeeAccessMatrixService : IService
    {
        Task<IEnumerable<EmployeeAccessMatrixViewModel>> GetAllEmployeeAccesses();
        Task<IEnumerable<EmployeeAccessMatrixControlViewModel>> GetAllEmployeeAccessControls();
        Task<IEnumerable<AccessLevelControlViewModel>> GetAllEmployeeAccessControlsRelationships();
        Task<bool> CreateAccessLevel(short id, string notes);
        Task<bool> CreateAccessControlsPermissions(short accessLevelId, List<int> accessControlsIds);
    }
}
