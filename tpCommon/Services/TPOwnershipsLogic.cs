using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.Repositories;
using Data;
using Services.Interfaces;
using ViewModels.EmployeeOwnerships;


namespace Services
{
    public class TPOwnershipsLogic : ITPOwnershipsLogic
    {
        private readonly IRepository<EmployeeOwnershipRelationship> employeeOwnershipRelationshipRepo;
        private readonly IRepository<Org> orgRepo;
        private readonly IRepository<EmployeeOwnershipType> employeeOwnershipTypeRepo;
        private readonly IRepository<Employee> employeeRepo;
        public TPOwnershipsLogic(IRepository<EmployeeOwnershipRelationship> repository, IRepository<Org> repository1,
                                 IRepository<EmployeeOwnershipType> repository2, IRepository<Employee> repository3)
        {
            this.employeeOwnershipRelationshipRepo = repository;
            this.orgRepo = repository1;
            this.employeeOwnershipTypeRepo = repository2;
            this.employeeRepo = repository3;
        }

        public async Task<List<EmployeeOwnershipDetailsViewModel>> GetAllOwnershipsForDataObjectID(int dataObjectID, short dataObjectTypeId, DateTime inForceDateTime, string searchTerm, int startPosition, int pageSize)
        {
            if (dataObjectTypeId == 5)
            {
                var result = await employeeOwnershipRelationshipRepo.All()
                               .Where(e => e.EmployeeId == dataObjectID && e.DataObjectTypeId == 2 &&
                                      inForceDateTime > e.InForceStartDateTime && (e.InForceEndDateTime == null || inForceDateTime < e.InForceEndDateTime))
                               .Join(employeeRepo.All(),
                                     o => o.EmployeeId,
                                     e => e.Id,
                                     (o, e) => new { ownershipTable = o, emp = e})
                               .Join(employeeOwnershipTypeRepo.All(),
                                     o => o.ownershipTable.EmployeeOwnershipTypeId,
                                     e => e.Id,
                                     (o, t) => new { ownershipTable = o.ownershipTable , emp = o.emp, type = t })
                               .Join(orgRepo.All(),
                                     e => e.ownershipTable.DataObjectId,
                                     o => o.Id,
                                    (e, o) => new { ownershipTable = e.ownershipTable, Org = o, emp = e.emp, type = e.type })
                               .Where(o => o.Org.DeletedDate == null &&
                                      (searchTerm == "" || (o.Org.Id.ToString().Contains(searchTerm) ||
                                                            o.Org.OrgName.Contains(searchTerm) ||
                                                            o.type.Name.Contains(searchTerm) ||
                                                            o.ownershipTable.InForceStartDateTime.ToString("dddd d MMMM yyyy").Contains(searchTerm) ||
                                                            o.ownershipTable.CommissionPercentage.ToString().Contains(searchTerm) ||
                                                            o.emp.SalesNewBusinessCommissionPercentage.ToString().Contains(searchTerm) ||
                                                            o.emp.SalesAccountManagementCommissionPercentage.ToString().Contains(searchTerm))))
                               .OrderBy(o => o.type.Name)
                               .OrderBy(o => o.Org.OrgName)
                               .Select(s => new EmployeeOwnershipDetailsViewModel()
                               {
                                   OwnershipId = s.ownershipTable.Id,
                                   OwnershipOrgId = s.Org.Id,
                                   OwnershipOrgName = s.Org.Id + " - " + s.Org.OrgName,
                                   EmployeeOwnershipTypeName = s.type.Name,
                                   OwnershipTypeId = s.type.Id,
                                   InForceSinceString = s.ownershipTable.InForceStartDateTime.ToString("d MMMM yyyy"),
                                   CommissionPercentage = (s.type.Id == 1 || s.type.Id == 4 ?
                                                                                (s.ownershipTable.CommissionPercentage != null ? s.ownershipTable.CommissionPercentage.Value.ToString("N2") + "%"
                                                                                                                    : (s.type.Id == 1 ? s.emp.SalesNewBusinessCommissionPercentage.ToString("N2") + "%" : s.emp.SalesAccountManagementCommissionPercentage.ToString("N2") + "%"))
                                                                          : null)
                                                })
                               .Skip(startPosition).Take(pageSize)
                              
                               .ToListAsync();


                return result;
            }
            else
            {
                return null;
            }

        }

        public int GetAllOwnershipsForDataObjectIDCount(int dataObjectID, short dataObjectTypeId, DateTime inForceDateTime , string searchTerm)
        {
            if (dataObjectTypeId == 5)
            {
                var result = employeeOwnershipRelationshipRepo.All()
                               .Where(e => e.EmployeeId == dataObjectID && e.DataObjectTypeId == 2 &&
                                      inForceDateTime > e.InForceStartDateTime && (e.InForceEndDateTime == null || inForceDateTime < e.InForceEndDateTime))
                                .Join(employeeRepo.All(),
                                     o => o.EmployeeId,
                                     e => e.Id,
                                     (o, e) => new { ownershipTable = o, emp = e })
                               .Join(employeeOwnershipTypeRepo.All(),
                                     o => o.ownershipTable.EmployeeOwnershipTypeId,
                                     e => e.Id,
                                     (o, t) => new { ownershipTable = o.ownershipTable, emp = o.emp, type = t })
                               .Join(orgRepo.All(),
                                     e => e.ownershipTable.DataObjectId,
                                     o => o.Id,
                                    (e, o) => new { ownershipTable = e.ownershipTable, Org = o, emp = e.emp, type = e.type })
                               .Where(o => o.Org.DeletedDate == null &&
                                      (searchTerm == "" || (o.Org.Id.ToString().Contains(searchTerm) ||
                                                            o.Org.OrgName.Contains(searchTerm) ||
                                                            o.type.Name.Contains(searchTerm) ||
                                                            o.ownershipTable.InForceStartDateTime.ToString("dddd d MMMM yyyy").Contains(searchTerm) ||
                                                            o.ownershipTable.CommissionPercentage.ToString().Contains(searchTerm) ||
                                                            o.emp.SalesNewBusinessCommissionPercentage.ToString().Contains(searchTerm) ||
                                                            o.emp.SalesAccountManagementCommissionPercentage.ToString().Contains(searchTerm))))
                               .Select(o => o.ownershipTable.Id).Count();

                return result;
            }
            else
            {
                return 0;
            }

        }

        //public async Task<List<Org>> GetAllOwnershipOrgsForDataObjectID(int dataObjectID, short dataObjectTypeId, DateTime inForceDateTime)
        //{
        //    if (dataObjectTypeId == 5)
        //    {
        //        var result = await employeeOwnershipRelationshipRepo.All()
        //                        .Where(e => e.EmployeeId == dataObjectID && e.DataObjectTypeId == 2 &&
        //                               inForceDateTime > e.InForceStartDateTime && (e.InForceEndDateTime == null || inForceDateTime < e.InForceEndDateTime))
        //                        .Join(orgRepo.All(),
        //                              e => e.DataObjectId,
        //                              o => o.Id,
        //                             (e, o) => new { ownershipTable = e, Org = o })
        //                        .Where(o => o.Org.DeletedDate == null)
        //                        .OrderBy(o => o.ownershipTable.DataObjectTypeId)
        //                        .OrderBy(o => o.ownershipTable.EmployeeOwnershipTypeId)
        //                        .OrderBy(o => o.Org.OrgName)
        //                        .ToListAsync();

        //        List<Org> result1 = new List<Org>();
        //        for (var i = 0; i < result.Count(); i++)
        //        {
        //            result1.Add(result[i].Org);
        //        }

        //        return result1;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


        //public async Task<List<EmployeeOwnershipType>> GetAllOwnershipTypesForDataObjectID(int dataObjectID, short dataObjectTypeId, DateTime inForceDateTime)
        //{
        //    if (dataObjectTypeId == 5)
        //    {
        //        var result = await employeeOwnershipRelationshipRepo.All()
        //                         .Where(e => e.EmployeeId == dataObjectID && e.DataObjectTypeId == 2 &&
        //                                inForceDateTime > e.InForceStartDateTime && (e.InForceEndDateTime == null || inForceDateTime < e.InForceEndDateTime))
        //                         .Join(orgRepo.All(),
        //                               e => e.DataObjectId,
        //                               o => o.Id,
        //                              (e, o) => new { ownershipTable = e, Org = o })
        //                         .Where(o => o.Org.DeletedDate == null)
        //                         .OrderBy(o => o.ownershipTable.DataObjectTypeId)
        //                         .OrderBy(o => o.ownershipTable.EmployeeOwnershipTypeId)
        //                         .OrderBy(o => o.Org.OrgName)
        //                         .ToListAsync();

        //        List<EmployeeOwnershipType> result1 = new List<EmployeeOwnershipType>();
        //        for (var i = 0; i < result.Count(); i++)
        //        {
        //           var ownershipType = await employeeOwnershipTypeRepo.All()
        //                                    .Where(t => t.Id == result[i].ownershipTable.EmployeeOwnershipTypeId)
        //                                    .FirstOrDefaultAsync();
        //           result1.Add(ownershipType);
        //        }

        //        return result1;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
