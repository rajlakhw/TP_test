using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Global_Settings;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.EmployeeOwnerships;

namespace Services
{
    public class TPEmployeeOwnershipsLogic : ITPEmployeeOwnershipsLogic
    {
        private readonly IRepository<EmployeeOwnershipRelationship> empOwnershipRepository;
        private readonly IRepository<Employee> empRepository;
        private readonly IRepository<EmployeeOwnershipType> ownershipTypesRepository;

        public TPEmployeeOwnershipsLogic(IRepository<EmployeeOwnershipRelationship> EmpOwnershipRepository,
            IRepository<Employee> empRepository,
            IRepository<EmployeeOwnershipType> ownershipTypesRepository)
        {
            empOwnershipRepository = EmpOwnershipRepository;
            this.empRepository = empRepository;
            this.ownershipTypesRepository = ownershipTypesRepository;
        }

        public async Task<EmployeeOwnershipRelationshipViewModel> GetEmployeeOwnershipForDataObjectAndOwnershipType(int DataObjectID, Enumerations.DataObjectTypes DataObjectType, Enumerations.EmployeeOwnerships OwnershipTypeID, DateTime? InForceAsOfDateTime = null)
        {
            InForceAsOfDateTime = InForceAsOfDateTime ?? GeneralUtils.GetCurrentUKTime();

            var res = await empOwnershipRepository.All().Where(x =>
            x.DataObjectId == DataObjectID &&
            x.DataObjectTypeId == ((byte)DataObjectType) &&
            x.EmployeeOwnershipTypeId == ((short)OwnershipTypeID) &&
            x.InForceStartDateTime < InForceAsOfDateTime &&
            (x.EmployeeOwnershipTypeId == (short)Enumerations.EmployeeOwnerships.ClientIntroLead ||
                x.InForceEndDateTime == null ||
                x.InForceEndDateTime > InForceAsOfDateTime) &&
            (x.EmployeeOwnershipTypeId != (short)Enumerations.EmployeeOwnerships.ClientIntroLead || x.ConfirmToEndDateTime == null))
            .Select(x => new EmployeeOwnershipRelationshipViewModel()
            {
                Id = x.Id,
                DataObjectTypeId = x.DataObjectTypeId,
                DataObjectId = x.DataObjectId,
                EmployeeId = x.EmployeeId,
                EmployeeOwnershipTypeId = x.EmployeeOwnershipTypeId,
                InForceStartDateTime = x.InForceStartDateTime,
                InForceEndDateTime = x.InForceEndDateTime,
                CommissionPercentage = x.CommissionPercentage,
                ReceiveNotifications = x.ReceiveNotifications,
                CreatedDateTime = x.CreatedDateTime,
                CreatedByEmployee = x.CreatedByEmployee,
                ConfirmToEndDateTime = x.ConfirmToEndDateTime,
                ConfirmToEndEmpId = x.ConfirmToEndEmpId
            })
            .FirstOrDefaultAsync();
            return res;
        }

        public async Task<IEnumerable<EmployeeOwnershipDetailsViewModel>> GetEmployeeOwnershipForDataObjectAndOwnershipType(int DataObjectID, Enumerations.DataObjectTypes DataObjectType)
        {
            var InForceAsOfDateTime = GeneralUtils.GetCurrentUKTime();

            var res = await empOwnershipRepository.All().Where(x =>
            (x.DataObjectId == DataObjectID &&
            x.DataObjectTypeId == ((byte)DataObjectType) &&
            (x.EmployeeOwnershipTypeId == ((short)Enumerations.EmployeeOwnerships.ClientIntroLead) ||
            x.InForceStartDateTime < InForceAsOfDateTime) &&
            (x.EmployeeOwnershipTypeId == (short)Enumerations.EmployeeOwnerships.ClientIntroLead ||
                x.InForceEndDateTime == null ||
                x.InForceEndDateTime > InForceAsOfDateTime)) &&
            (x.EmployeeOwnershipTypeId != (short)Enumerations.EmployeeOwnerships.ClientIntroLead || x.ConfirmToEndDateTime == null))

            .Join(empRepository.All(), ownership=>ownership.EmployeeId, employee=>employee.Id, (ownership, employee) => new { ownership, employee })
            .Join(ownershipTypesRepository.All(), a=>a.ownership.EmployeeOwnershipTypeId, ownershipType => ownershipType.Id, (a, ownershipType) => new { a, ownershipType })
            .Select(x => new EmployeeOwnershipDetailsViewModel()
            {
                EmployeeId = x.a.ownership.EmployeeId,
                ProfilePicture = x.a.employee.ImageBase64,
                EmployeeName = x.a.employee.FirstName + " " + x.a.employee.Surname,
                EmployeeJobTitle = x.a.employee.JobTitle,
                InForceSince = x.a.ownership.InForceStartDateTime,
                OwnershipId = x.a.ownership.Id,
                OwnershipTypeId = x.a.ownership.EmployeeOwnershipTypeId,
                OwnershipType = x.ownershipType.Name
            })
            .ToListAsync();
            return res;
        }

        public async Task<IEnumerable<EmployeeOwnershipRelationshipViewModel>> GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(int DataObjectID, Enumerations.DataObjectTypes DataObjectType, Enumerations.EmployeeOwnerships[] OwnershipTypes, DateTime? InForceAsOfDateTime = null, bool ReceivesNotificationOnly = true)
        {
            InForceAsOfDateTime = InForceAsOfDateTime ?? GeneralUtils.GetCurrentUKTime();

            var res = await empOwnershipRepository.All().Where(x =>
            x.DataObjectId == DataObjectID &&
            x.DataObjectTypeId == ((byte)DataObjectType) &&
            OwnershipTypes.Contains((Enumerations.EmployeeOwnerships)x.EmployeeOwnershipTypeId) &&
            x.InForceStartDateTime < InForceAsOfDateTime &&
            x.ReceiveNotifications == ReceivesNotificationOnly &&
            (x.InForceEndDateTime == null ||
            x.InForceEndDateTime > InForceAsOfDateTime))
            .Select(x => new EmployeeOwnershipRelationshipViewModel()
            {
                Id = x.Id,
                DataObjectTypeId = x.DataObjectTypeId,
                DataObjectId = x.DataObjectId,
                EmployeeId = x.EmployeeId,
                EmployeeOwnershipTypeId = x.EmployeeOwnershipTypeId,
                InForceStartDateTime = x.InForceStartDateTime,
                InForceEndDateTime = x.InForceEndDateTime,
                CommissionPercentage = x.CommissionPercentage,
                ReceiveNotifications = x.ReceiveNotifications,
                CreatedDateTime = x.CreatedDateTime,
                CreatedByEmployee = x.CreatedByEmployee,
                ConfirmToEndDateTime = x.ConfirmToEndDateTime,
                ConfirmToEndEmpId = x.ConfirmToEndEmpId
            })
            .ToListAsync();
            return res;
        }
    }
}
