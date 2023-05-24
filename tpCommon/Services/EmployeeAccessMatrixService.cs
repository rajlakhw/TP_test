using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.EmployeeModels.EmployeeAccessMatrix;

namespace Services
{
    public class EmployeeAccessMatrixService : IEmployeeAccessMatrixService
    {
        private readonly IRepository<EmployeeAccessMatrix> repository;
        private readonly IRepository<EmployeeAccessMatrixControl> controlsRepo;
        private readonly IRepository<AccessControl> accessLevelsControlsRepo;

        public EmployeeAccessMatrixService(IRepository<EmployeeAccessMatrix> repository,
            IRepository<EmployeeAccessMatrixControl> controlsRepo,
            IRepository<AccessControl> accessLevelsControlsRepo)
        {
            this.repository = repository;
            this.controlsRepo = controlsRepo;
            this.accessLevelsControlsRepo = accessLevelsControlsRepo;
        }

        public async Task<bool> CreateAccessControlsPermissions(short accessLevelId, List<int> accessControlsIds)
        {
            try
            {
                var dbControlIds = await accessLevelsControlsRepo.All().Where(x => x.EmployeeAccessLevelId == accessLevelId).ToListAsync();

                var newControlsIds = accessControlsIds;

                var controlIdsPermissionsToDelete = dbControlIds
                    .Where(x => newControlsIds.Contains(x.AccessMatrixControlId) == false)
                    .Select(x=>x.AccessMatrixControlId)
                    .ToList();

                var controlIdsPermissionsToAdd = newControlsIds
                    .Where(x => dbControlIds.Select(x=>x.AccessMatrixControlId)
                    .Contains(x) == false)
                    .ToList();

                if (controlIdsPermissionsToDelete.Count > 0)
                {
                    var toDeletePermissions = dbControlIds.Where(x => controlIdsPermissionsToDelete.Contains(x.AccessMatrixControlId));
                    foreach (var control in toDeletePermissions)
                    {
                        accessLevelsControlsRepo.Delete(control);
                    }
                }
                if (controlIdsPermissionsToAdd.Count > 0)
                {
                    foreach (var controlId in controlIdsPermissionsToAdd)
                    {
                        var control = new AccessControl()
                        {
                            EmployeeAccessLevelId = accessLevelId,
                            AccessMatrixControlId = controlId,
                        };
                        await accessLevelsControlsRepo.AddAsync(control);
                    }
                }
                await accessLevelsControlsRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }

        public async Task<bool> CreateAccessLevel(short id, string notes)
        {
            try
            {
                var dbLevel = await repository.All().FirstOrDefaultAsync(x => x.Id == id);
                if (dbLevel != null)
                    return false;

                var level = new EmployeeAccessMatrix()
                {
                    EmployeeAccessLevel = id,
                    Notes = notes
                };

                await repository.AddAsync(level);
                await repository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<EmployeeAccessMatrixControlViewModel>> GetAllEmployeeAccessControls()
            => await controlsRepo.All().Select(x =>
                new EmployeeAccessMatrixControlViewModel() { Id = x.Id, ControlName = x.ControlName, Page = x.Page }).ToListAsync();

        public async Task<IEnumerable<AccessLevelControlViewModel>> GetAllEmployeeAccessControlsRelationships()
            => await accessLevelsControlsRepo.All()
            .Select(x => new AccessLevelControlViewModel()
            { Id = x.Id, AccessMatrixControlId = x.AccessMatrixControlId, EmployeeAccessLevelId = x.EmployeeAccessLevelId })
            .ToListAsync();

        public async Task<IEnumerable<EmployeeAccessMatrixViewModel>> GetAllEmployeeAccesses()
            => await repository.All().Select(x => new EmployeeAccessMatrixViewModel()
            {
                Id = x.Id,
                EmployeeAccessLevel = x.EmployeeAccessLevel,
                Notes = x.Notes
            }).ToListAsync();
    }
}
