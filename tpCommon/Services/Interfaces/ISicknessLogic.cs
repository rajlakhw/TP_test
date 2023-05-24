using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;

namespace Services.Interfaces
{
    public interface ISicknessLogic: IService
    {
        Task<List<int>> GetAllSicknessYears(short employeeId);

        Task<List<EmployeesSickness>> GetAllSicknessForEmployee(short employeeId, int year);

        String GetDoctorsCertificatePath(string certificatePath);

        Task<Decimal> GetTotalNumberOfSickDaysInYearForEmployee(short employeeId, int year);

        Task<EmployeesSickness> GetSicknessDetails(int sicknessId);

        Task<EmployeesSickness> DeleteSickness(int sicknessID, short deletedByEmployeeId);
    }
}
