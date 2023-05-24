using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Global_Settings;
using System.IO;

namespace Services
{
    public class SicknessLogic : ISicknessLogic
    {
        private readonly IRepository<EmployeesSickness> employeeSicknessRepo;
        private readonly IConfiguration configuration;

        public SicknessLogic(IRepository<EmployeesSickness> repository, IConfiguration configuration1)
        {
            this.employeeSicknessRepo = repository;
            this.configuration = configuration1;
        }

        public async Task<List<int>> GetAllSicknessYears(short employeeId)
        {
            var result = await employeeSicknessRepo.All().Where(s => s.EmployeeId == employeeId).Select(s => s.SicknessStartDateTime.Year).Distinct().OrderByDescending(y => y).ToListAsync();
            return result;
        }

        public async Task<List<EmployeesSickness>> GetAllSicknessForEmployee(short employeeId, int year)
        {
            var result = await employeeSicknessRepo.All().Where(s => s.EmployeeId == employeeId &&
                                                                (s.SicknessStartDateTime.Year == year || s.SicknessEndDateTime.Year == year) &&
                                                                s.DeletedDateTime == null).Distinct()
                                                         .OrderBy(o => o.SicknessStartDateTime).ToListAsync();
            return result;
        }

        public String GetDoctorsCertificatePath(string certificatePath)
        {
            var config = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

            var result = "";
            if (File.Exists(config.HRDoctorsCertificateFolderLocation + certificatePath))
            {
                result = config.HRDoctorsCertificateFolderLocation + certificatePath;
            }
            return result;
        }

        public async Task<Decimal> GetTotalNumberOfSickDaysInYearForEmployee(short employeeId, int year)
        {
            var result = await employeeSicknessRepo.All().Where(e => e.EmployeeId == employeeId && e.DeletedDateTime == null
                                                                && (e.SicknessStartDateTime.Year == year || e.SicknessEndDateTime.Year == year))
                                                         .SumAsync(s => s.TotalDays);
            return result;
        }

        public async Task<EmployeesSickness> GetSicknessDetails(int sicknessId)
        {
            var result = await employeeSicknessRepo.All().Where(s => s.Id == sicknessId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<EmployeesSickness> DeleteSickness(int sicknessID, short deletedByEmployeeId)
        {
            var thisSickness = await GetSicknessDetails(sicknessID);

            thisSickness.DeletedDateTime = GeneralUtils.GetCurrentUKTime();
            thisSickness.DeletedByEmployeeId = deletedByEmployeeId;

            employeeSicknessRepo.Update(thisSickness);
            await employeeSicknessRepo.SaveChangesAsync();

            return thisSickness;

        }
    }
}
