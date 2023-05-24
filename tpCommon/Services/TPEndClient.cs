using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class TPEndClient : ITPEndClient
    {

        private readonly IRepository<EndClient> endClientRepository;
        private readonly IRepository<Timesheet> timesheetRepository;
        private readonly IRepository<EndClientData> endClientDataRepository;
        public TPEndClient(IRepository<EndClient> repository, IRepository<EndClientData> ecldrepository, IRepository<Timesheet> repository1)
        {
            this.endClientRepository = repository;
            this.endClientDataRepository = ecldrepository;
            this.timesheetRepository = repository1;
        }
        public async Task<Data.EndClient> GetEndClientDetails<EndClient>(string EndClientName)
        {
            var result = await endClientRepository.All().Where(o => o.Name == EndClientName && o.DeletedDateTime == null).SingleOrDefaultAsync();
            return result;
        }

        public async Task<Data.EndClientData> GetEndClientDataDetails<EndClientData>(int EndClientID, string DataName, int DataID)
        {
            var result = await endClientDataRepository.All().Where(o => o.EndClientID == EndClientID && o.DataObjectName == DataName && o.DataObjectTypeID == DataID && o.DeletedDateTime == null).SingleOrDefaultAsync();
            return result;
        }

        public async Task<List<ViewModels.TPEndClient.TPEndClientViewModel>> GetAllEndClients()
        {
            var resultList = await endClientRepository.All().Where(s => s.DeletedDateTime == null).Select(x => new ViewModels.TPEndClient.TPEndClientViewModel() { Id = x.Id, Name = x.Name }).OrderBy(x => x.Name).ToListAsync();
            return resultList;
        }

        public async System.Threading.Tasks.Task InsertNewEndClient(string EndClientName, int EmployeeID)
        {
            var EndClientTable = new EndClient();

            EndClientTable.Name = EndClientName;

            EndClientTable.CreatedBy = EmployeeID;

            EndClientTable.CreatedDateTime = GeneralUtils.GetCurrentUKTime();

            await endClientRepository.AddAsync(EndClientTable);

            await endClientRepository.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task InsertNewEndClientData(string Name, int EndClientID, int DataObjectID, int EmployeeID)
        {
            var EndClientDataTable = new EndClientData();

            EndClientDataTable.EndClientID = EndClientID;

            EndClientDataTable.DataObjectTypeID = DataObjectID;

            EndClientDataTable.DataObjectName = Name;

            EndClientDataTable.CreatedBy = EmployeeID;

            EndClientDataTable.CreatedDateTime = GeneralUtils.GetCurrentUKTime();

            await endClientDataRepository.AddAsync(EndClientDataTable);

            await endClientDataRepository.SaveChangesAsync();
        }

        public async Task<Data.EndClient> GetEndClientByID<EndClient>(int EndClientID)
        {
            var result = await endClientRepository.All().Where(s => s.Id == EndClientID).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Data.EndClientData> GetEndClientDataByID<EndClient>(int EndClientDataID)
        {
            var result = await endClientDataRepository.All().Where(s => s.Id == EndClientDataID).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<Data.EndClient>> GetAllEndClient()
        {
            var resultList = await endClientRepository.All().Where(s => s.DeletedDateTime == null).OrderBy(x => x.Name).ToListAsync();
            return resultList;
        }

        public async Task<List<Data.EndClientData>> GetAllBrands(int? EndClientID = null)
        {
            if (EndClientID == null)
            {
                var resultList = await endClientDataRepository.All().Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 19).OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().Where(s => s.DeletedDateTime == null && s.EndClientID == EndClientID && s.DataObjectTypeID == 19).OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }

        }

        public async Task<List<Data.EndClientData>> GetAllCategories(int? EndClientID = null)
        {
            if (EndClientID == null)
            {
                var resultList = await endClientDataRepository.All().Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 20).OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().Where(s => s.DeletedDateTime == null && s.EndClientID == EndClientID && s.DataObjectTypeID == 20).OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
        }

        public async Task<List<Data.EndClientData>> GetAllCampaigns(int? EndClientID = null)
        {
            if (EndClientID == null)
            {
                var resultList = await endClientDataRepository.All().Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 18).OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().Where(s => s.DeletedDateTime == null && s.EndClientID == EndClientID && s.DataObjectTypeID == 18).OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }

        }

        public async Task<List<Data.EndClient>> GetAllEndClientLoggedInTimesheet()
        {
            var resultList = await endClientRepository.All().
                                    Join(timesheetRepository.All(),
                                         ec => ec.Id,
                                         t => t.EndClientId,
                                         (ec, t) => new {endclient = ec, timesheet = t }).
                                         Where(s => s.endclient.DeletedDateTime == null).
                                         Select(ec => ec.endclient).Distinct().
                                         OrderBy(x => x.Name).ToListAsync();
            return resultList;
        }

        public async Task<List<Data.EndClientData>> GetAllBrandsLoggedInTimesheet(int? EndClientID = null)
        {
            if (EndClientID == null)
            {
                var resultList = await endClientDataRepository.All().
                                         Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 19).
                                          Join(timesheetRepository.All(),
                                                 ecd => ecd.Id,
                                                 t => t.BrandId,
                                                (ecd, t) => new { endclientdata = ecd, timesheet = t })
                                          .Select(ecd => ecd.endclientdata).Distinct()
                                          .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().
                                    Where(s => s.DeletedDateTime == null && s.EndClientID == EndClientID && s.DataObjectTypeID == 19).
                                     Join(timesheetRepository.All(),
                                                 ecd => ecd.Id,
                                                 t => t.BrandId,
                                                (ecd, t) => new { endclientdata = ecd, timesheet = t })
                                          .Select(ecd => ecd.endclientdata).Distinct()
                                          .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }

        }

        public async Task<List<Data.EndClientData>> GetAllCategoriesLoggedInTimesheet(int? EndClientID = null)
        {
            if (EndClientID == null)
            {
                var resultList = await endClientDataRepository.All().Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 20).
                                        Join(timesheetRepository.All(),
                                                 ecd => ecd.Id,
                                                 t => t.CategoryId,
                                                (ecd, t) => new { endclientdata = ecd, timesheet = t })
                                                .Select(ecd => ecd.endclientdata).OrderBy(x => x.DataObjectName).Distinct().OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().Where(s => s.DeletedDateTime == null && s.EndClientID == EndClientID && s.DataObjectTypeID == 20).Distinct().OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
        }

        public async Task<List<Data.EndClientData>> GetAllCampaignsLoggedInTimesheet(int? EndClientID = null)
        {
            if (EndClientID == null)
            {
                var resultList = await endClientDataRepository.All().Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 18).
                                        Join(timesheetRepository.All(),
                                                 ecd => ecd.Id,
                                                 t => t.CampaignId,
                                                (ecd, t) => new { endclientdata = ecd, timesheet = t })
                                                .Select(ecd => ecd.endclientdata).Distinct().OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().Where(s => s.DeletedDateTime == null && s.EndClientID == EndClientID && s.DataObjectTypeID == 18).Distinct().OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }

        }

        public async Task<List<Data.EndClientData>> GetAllTimesheetBrandsForEndClientIDs(string allEndClientIDString)
        {
            if (allEndClientIDString == "")
            {
                var resultList = await endClientDataRepository.All().
                                         Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 19).
                                          Join(timesheetRepository.All(),
                                                 ecd => ecd.Id,
                                                 t => t.BrandId,
                                                (ecd, t) => new { endclientdata = ecd, timesheet = t })
                                          .Select(ecd => ecd.endclientdata).Distinct()
                                          .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().
                                    Where(s => s.DeletedDateTime == null && allEndClientIDString.Contains(s.EndClientID.ToString()) && s.DataObjectTypeID == 19).
                                     Join(timesheetRepository.All(),
                                                 ecd => ecd.Id,
                                                 t => t.BrandId,
                                                (ecd, t) => new { endclientdata = ecd, timesheet = t })
                                          .Select(ecd => ecd.endclientdata).Distinct()
                                          .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
        }

        public async Task<List<Data.EndClientData>> GetAllTimesheetCategoriesForEndClientIDs(string allEndClientIDString)
        {
            if (allEndClientIDString == "")
            {
                var resultList = await endClientDataRepository.All().
                                         Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 20).
                                          Join(timesheetRepository.All(),
                                                 ecd => ecd.Id,
                                                 t => t.CategoryId,
                                                (ecd, t) => new { endclientdata = ecd, timesheet = t })
                                          .Select(ecd => ecd.endclientdata).Distinct()
                                          .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().
                                    Where(s => s.DeletedDateTime == null && allEndClientIDString.Contains(s.EndClientID.ToString()) && s.DataObjectTypeID == 20).
                                     Join(timesheetRepository.All(),
                                                 ecd => ecd.Id,
                                                 t => t.CategoryId,
                                                (ecd, t) => new { endclientdata = ecd, timesheet = t })
                                          .Select(ecd => ecd.endclientdata).Distinct()
                                          .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
        }

        public async Task<List<Data.EndClientData>> GetAllTimesheetCampaignsForEndClientIDs(string allEndClientIDString)
        {
            if (allEndClientIDString == "")
            {
                var resultList = await endClientDataRepository.All().
                                         Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 18).
                                          Join(timesheetRepository.All(),
                                                 ecd => ecd.Id,
                                                 t => t.CampaignId,
                                                (ecd, t) => new { endclientdata = ecd, timesheet = t })
                                          .Select(ecd => ecd.endclientdata).Distinct()
                                          .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().
                                    Where(s => s.DeletedDateTime == null && allEndClientIDString.Contains(s.EndClientID.ToString()) && s.DataObjectTypeID == 18).
                                     Join(timesheetRepository.All(),
                                                 ecd => ecd.Id,
                                                 t => t.CampaignId,
                                                (ecd, t) => new { endclientdata = ecd, timesheet = t })
                                          .Select(ecd => ecd.endclientdata).Distinct()
                                          .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
        }


        public async Task<List<Data.EndClientData>> GetAllBrandsForEndClientIDs(string allEndClientIDString)
        {
            if (allEndClientIDString == "")
            {
                var resultList = await endClientDataRepository.All().
                                         Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 19)
                                          .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().
                                    Where(s => s.DeletedDateTime == null && allEndClientIDString.Contains(s.EndClientID.ToString()) && s.DataObjectTypeID == 19)
                                    .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
        }

        public async Task<List<Data.EndClientData>> GetAllCategoriesForEndClientIDs(string allEndClientIDString)
        {
            if (allEndClientIDString == "")
            {
                var resultList = await endClientDataRepository.All().
                                       Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 20)
                                       .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().
                                       Where(s => s.DeletedDateTime == null && allEndClientIDString.Contains(s.EndClientID.ToString()) && s.DataObjectTypeID == 20)
                                       .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
        }

        public async Task<List<Data.EndClientData>> GetAllCampaignsForEndClientIDs(string allEndClientIDString)
        {
            if (allEndClientIDString == "")
            {
                var resultList = await endClientDataRepository.All().
                                       Where(s => s.DeletedDateTime == null && s.DataObjectTypeID == 18)
                                       .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
            else
            {
                var resultList = await endClientDataRepository.All().
                                       Where(s => s.DeletedDateTime == null && allEndClientIDString.Contains(s.EndClientID.ToString()) && s.DataObjectTypeID == 18)
                                       .OrderBy(x => x.DataObjectName).ToListAsync();
                return resultList;
            }
        }
    }
}
