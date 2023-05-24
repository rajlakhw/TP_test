using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class TPTimesheetService : ITPTimesheetService
    {
        private readonly IRepository<Timesheet> TimesheetRepository;
        private readonly IRepository<Org> OrgRepository;
        private readonly IRepository<OrgGroup> OrgGroupRepository;
        private readonly IRepository<TimesheetLogBreakdown> TimesheetBreakdownRepo;
        private readonly IRepository<TimesheetTaskCategory> TimesheetCategoryRepo;
        private readonly IRepository<TimesheetsApproval> TimesheetApprovalRepo;
        private readonly IRepository<Employee> EmployeeRepository;
        private readonly IRepository<EmployeeTeam> EmployeeTeamRepo;

        public TPTimesheetService(IRepository<Timesheet> repository,
                                  IRepository<Org> repository1,
                                  IRepository<TimesheetLogBreakdown> repository2,
                                  IRepository<TimesheetTaskCategory> repository3,
                                  IRepository<Employee> repository4,
                                  IRepository<TimesheetsApproval> repository5,
                                  IRepository<EmployeeTeam> repository6,
                                  IRepository<OrgGroup> repository7)
        {
            this.TimesheetRepository = repository;
            this.OrgRepository = repository1;
            this.TimesheetBreakdownRepo = repository2;
            this.TimesheetCategoryRepo = repository3;
            this.EmployeeRepository = repository4;
            this.TimesheetApprovalRepo = repository5;
            this.EmployeeTeamRepo = repository6;
            this.OrgGroupRepository = repository7;
        }

        public async Task<List<Data.Timesheet>> GetAllTimesheetLogs<Timesheet>(int EmployeeID, DateTime StartDate, DateTime EndDate)
        {
            var result = await TimesheetRepository.All().Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate).ToListAsync();
            return result;
        }

        public async Task<List<Data.Org>> GetAllRowHeaderOrgCategoriesForAWeek<Org>(int EmployeeID, DateTime StartDate, DateTime EndDate)
        {
            //var result2 = await TimesheetRepository.All().Where(s => s.EmployeeID == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate).Select(s => s.OrgID).Distinct().ToListAsync();
            //var result2 = await result.Join
            //var result3 = from t in TimesheetRepository.All()
            //              join o in OrgRepository.All()
            //              on t.OrgID equals o.Id
            //              where (t.EmployeeID == EmployeeID && t.TimeLogDate >= StartDate && t.TimeLogDate <= EndDate)
            //              select new { Org = o }.Org;
            //var result = await result3.ToListAsync();

            var result = await TimesheetRepository.All() //first table in inner join
                               .Join(OrgRepository.All(), //second table to inner join with
                                t => t.OrgId, //inner join column from table1
                                o => o.Id, //inner join column from table2
                                (t, o) => new { Timesheet = t, Org = o }) //selection of all table columns
                                .OrderBy(o => o.Org.OrgName)
                                .Where(s => s.Timesheet.EmployeeId == EmployeeID && s.Timesheet.TimeLogDate >= StartDate && s.Timesheet.TimeLogDate <= EndDate) //where clause
                                .Select(o => o.Org) //elements to return
                                .Distinct().ToListAsync(); //distinct result that is required

            //var result1 = await OrgRepository.All().Where(s => s.Id == 83923).ToListAsync();
            return result;
        }

        public async Task<Hashtable> GetAllTandPRowHeadersForAWeek(int EmployeeID, DateTime StartDate, DateTime EndDate)
        {

            var result = await TimesheetRepository.All() //first table in inner join
                               .Join(OrgRepository.All(), //second table to inner join with
                                t => t.OrgId, //inner join column from table1
                                o => o.Id, //inner join column from table2
                                (t, o) => new { Timesheet = t, Org = o }) //selection of all table columns
                                .Where(s => s.Timesheet.EmployeeId == EmployeeID && s.Timesheet.TimeLogDate >= StartDate && s.Timesheet.TimeLogDate <= EndDate) //where clause
                                .Select(o => new { o.Org, o.Timesheet.EndClientId, o.Timesheet.BrandId, o.Timesheet.CategoryId, o.Timesheet.CampaignId }) //elements to return
                                .Distinct().ToListAsync(); //distinct result that is required
            var timesheetEmployee = await EmployeeRepository.All().Where(x => x.Id == EmployeeID && x.TerminateDate == null).FirstOrDefaultAsync();
            var employeeTeam = await EmployeeTeamRepo.All().Where(x => x.Id == timesheetEmployee.TeamId).FirstOrDefaultAsync();
            //Dictionary<Org, string> DictToReturn = new Dictionary<Org, string>(new OrgEqualityComparer());
            Hashtable hashtableToReturn = new Hashtable();


            //if (employeeTeam.DepartmentId == 10 || timesheetEmployee.TeamId == 36)
            if (employeeTeam.DepartmentId == 10)
            {
                for (var i = 0; i < result.Count(); i++)
                {
                    var StringToAttach = result.ElementAt(i).EndClientId.ToString() + '$' + result.ElementAt(i).BrandId.ToString() + '$' + result.ElementAt(i).CategoryId.ToString() + '$' + result.ElementAt(i).CampaignId.ToString();
                    Dictionary<int, Org> thisDict = new Dictionary<int, Org>();
                    thisDict.Add(i + 1, result.ElementAt(i).Org);
                    hashtableToReturn.Add(thisDict, StringToAttach);
                }
            }
            else
            {
                for (var i = 0; i < result.Count(); i++)
                {
                    Dictionary<int, Org> thisDict = new Dictionary<int, Org>();
                    thisDict.Add(i + 1, result.ElementAt(i).Org);
                    hashtableToReturn.Add(thisDict, null);
                }

            }

            //var result1 = await OrgRepository.All().Where(s => s.Id == 83923).ToListAsync();
            return hashtableToReturn;
        }

        public async Task<string> GetWeeklyTotalForOrg(int EmployeeID, DateTime StartDate, DateTime EndDate, int? OrgID)
        {

            if (OrgID != null)
            {
                var Minutes = await TimesheetRepository.All() //first table in inner join
                                 .Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate && s.OrgId == OrgID) //where clause
                                 .GroupBy(t => t.OrgId)
                                 .Select(s => new { TotalClientMinutes = s.Sum(x => x.ClientChargeInMinutes), TotalNonChargeableMinutes = s.Sum(x => x.NonChargeableTimeInMinutes) })
                                 .ToListAsync(); //elements to return
                if (Minutes.Count() > 0)
                {
                    var TotalMinutes = Minutes.ElementAt(0).TotalClientMinutes + Minutes.ElementAt(0).TotalNonChargeableMinutes;

                    var TotalHour = 0;
                    if (TotalMinutes == 60)
                    {
                        TotalMinutes = 0;
                        TotalHour += 1;
                    }
                    else if (TotalMinutes > 60)
                    {
                        TotalHour = (int)Math.Floor((decimal)TotalMinutes / 60);
                        TotalMinutes = (TotalMinutes % 60);
                    }


                    var Hours = await TimesheetRepository.All() //first table in inner join
                                          .Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate && s.OrgId == OrgID) //where clause
                                          .GroupBy(t => t.OrgId)
                                          .Select(s => new { TotalClientHours = s.Sum(x => x.ClientChargeInHours), TotalNonClientHours = s.Sum(x => x.NonChargeableTimeInHours) })
                                          .ToListAsync(); //elements to return

                    TotalHour += (int)(Hours.ElementAt(0).TotalClientHours + Hours.ElementAt(0).TotalNonClientHours);
                    string StringToReturn = TotalHour.ToString() + ":" + TotalMinutes.ToString();
                    return StringToReturn;
                }
                else
                {
                    return "0:0";
                }


            }
            else
            {
                var Minutes = await TimesheetRepository.All() //first table in inner join
                                 .Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate && s.OrgId == null) //where clause
                                 .GroupBy(t => t.OrgId)
                                 .Select(s => new { TotalMinutes = s.Sum(x => (x.NonClientChargeInMinutes)) })
                                 .ToListAsync(); //elements to return

                if (Minutes.Count() > 0)
                {
                    var TotalMinutes = Minutes.ElementAt(0).TotalMinutes;
                    var TotalHour = 0;
                    if (TotalMinutes == 60)
                    {
                        TotalMinutes = 0;
                        TotalHour += 1;
                    }
                    else if (TotalMinutes > 60)
                    {
                        TotalHour = (int)Math.Floor((decimal)TotalMinutes / 60);
                        TotalMinutes = (TotalMinutes % 60);
                    }


                    var Hours = await TimesheetRepository.All() //first table in inner join
                                          .Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate && s.OrgId == null) //where clause
                                          .GroupBy(t => t.OrgId)
                                          .Select(s => new { TotalHours = s.Sum(x => (x.NonClientChargeInHours)) })
                                          .ToListAsync(); //elements to return

                    TotalHour += (int)Hours.ElementAt(0).TotalHours;
                    string StringToReturn = TotalHour.ToString() + ":" + TotalMinutes.ToString();
                    return StringToReturn;
                }
                else
                {
                    return "0:0";
                }

            }

        }


        public async Task<string> GetWeeklyTotalForTandPOrg(int EmployeeID, DateTime StartDate, DateTime EndDate, int? OrgID, int? EndClientID, int? BrandID, int? CategoryID, int? CampaignID)
        {

            if (OrgID != null)
            {
                var Minutes = await TimesheetRepository.All() //first table in inner join
                                 .Where(s => s.EmployeeId == EmployeeID &&
                                        s.TimeLogDate >= StartDate &&
                                        s.TimeLogDate <= EndDate &&
                                        s.OrgId == OrgID &&
                                        ((EndClientID == 0 && s.EndClientId == null || s.EndClientId == EndClientID)) &&
                                        ((BrandID == 0 && s.BrandId == null || s.BrandId == BrandID)) &&
                                        ((CategoryID == 0 && s.CategoryId == null || s.CategoryId == CategoryID)) &&
                                        ((CampaignID == 0 && s.CampaignId == null || s.CampaignId == CampaignID)) 
                                        ) //where clause
                                 .GroupBy(t => t.OrgId)
                                 .Select(s => new { TotalClientMinutes = s.Sum(x => x.ClientChargeInMinutes), TotalNonChargeableMinutes = s.Sum(x => x.NonChargeableTimeInMinutes) })
                                 .ToListAsync(); //elements to return
                if (Minutes.Count() > 0)
                {
                    var TotalMinutes = Minutes.ElementAt(0).TotalClientMinutes + Minutes.ElementAt(0).TotalNonChargeableMinutes;

                    var TotalHour = 0;
                    if (TotalMinutes == 60)
                    {
                        TotalMinutes = 0;
                        TotalHour += 1;
                    }
                    else if (TotalMinutes > 60)
                    {
                        TotalHour = (int)Math.Floor((decimal)TotalMinutes / 60);
                        TotalMinutes = (TotalMinutes % 60);
                    }


                    var Hours = await TimesheetRepository.All() //first table in inner join
                                          .Where(s => s.EmployeeId == EmployeeID &&
                                                 s.TimeLogDate >= StartDate &&
                                                 s.TimeLogDate <= EndDate &&
                                                 s.OrgId == OrgID &&
                                                 ((EndClientID == 0 && s.EndClientId == null || s.EndClientId == EndClientID)) &&
                                                 ((BrandID == 0 && s.BrandId == null || s.BrandId == BrandID)) &&
                                                 ((CategoryID == 0 && s.CategoryId == null || s.CategoryId == CategoryID)) &&
                                                 ((CampaignID == 0 && s.CampaignId == null || s.CampaignId == CampaignID))) //where clause
                                          .GroupBy(t => t.OrgId)
                                          .Select(s => new { TotalClientHours = s.Sum(x => x.ClientChargeInHours), TotalNonClientHours = s.Sum(x => x.NonChargeableTimeInHours) })
                                          .ToListAsync(); //elements to return

                    TotalHour += (int)(Hours.ElementAt(0).TotalClientHours + Hours.ElementAt(0).TotalNonClientHours);
                    string StringToReturn = TotalHour.ToString() + ":" + TotalMinutes.ToString();
                    return StringToReturn;
                }
                else
                {
                    return "0:0";
                }


            }
            else
            {
                var Minutes = await TimesheetRepository.All() //first table in inner join
                                 .Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate && s.OrgId == null) //where clause
                                 .GroupBy(t => t.OrgId)
                                 .Select(s => new { TotalMinutes = s.Sum(x => (x.NonClientChargeInMinutes)) })
                                 .ToListAsync(); //elements to return

                if (Minutes.Count() > 0)
                {
                    var TotalMinutes = Minutes.ElementAt(0).TotalMinutes;
                    var TotalHour = 0;
                    if (TotalMinutes == 60)
                    {
                        TotalMinutes = 0;
                        TotalHour += 1;
                    }
                    else if (TotalMinutes > 60)
                    {
                        TotalHour = (int)Math.Floor((decimal)TotalMinutes / 60);
                        TotalMinutes = (TotalMinutes % 60);
                    }


                    var Hours = await TimesheetRepository.All() //first table in inner join
                                          .Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate && s.OrgId == null) //where clause
                                          .GroupBy(t => t.OrgId)
                                          .Select(s => new { TotalHours = s.Sum(x => (x.NonClientChargeInHours)) })
                                          .ToListAsync(); //elements to return

                    TotalHour += (int)Hours.ElementAt(0).TotalHours;
                    string StringToReturn = TotalHour.ToString() + ":" + TotalMinutes.ToString();
                    return StringToReturn;
                }
                else
                {
                    return "0:0";
                }

            }

        }

        public async Task<List<string>> GetAllDailyTotals(int EmployeeID, DateTime StartDate, DateTime EndDate)
        {

            var StringListToReturn = new List<string>();
            //var Minutes = await TimesheetRepository.All() //first table in inner join
            //                 .Where(s => s.EmployeeID == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate) //where clause
            //                 .GroupBy(t => t.TimeLogDate)
            //                 .Select(s => new { TotalClientMinutes = s.Sum(x => x.ClientChargeInMinutes),
            //                                    TotalNonChargeableMinutes = s.Sum(x => x.NonChargeableTimeInMinutes),
            //                                    TotalNonClientMinutes = s.Sum(x => x.NonClientChargeInMinutes) })
            //                 .ToListAsync(); //elements to return

            var TimeList = await TimesheetRepository.All() //first table in inner join
                             .Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate) //where clause
                             .GroupBy(t => t.TimeLogDate)
                             .Select(s => new
                             {
                                 TotalMinutes = (s.Sum(x => x.ClientChargeInMinutes) +
                                                s.Sum(x => x.NonChargeableTimeInMinutes) +
                                                s.Sum(x => x.NonClientChargeInMinutes)),
                                 TotalHours = (s.Sum(x => x.ClientChargeInHours) +
                                               s.Sum(x => x.NonChargeableTimeInHours) +
                                               s.Sum(x => x.NonClientChargeInHours)),
                                 TimeLogDate = (s.Key)
                             })
                             .ToListAsync(); //elements to return

            var dateToAdd = StartDate;
            var i = 0;
            int? GrandTotalHours = 0;
            int? GrandTotalMinutes = 0;
            Data.Employee employee = await EmployeeRepository.All().Where(x => x.Id == EmployeeID && x.TerminateDate == null).FirstOrDefaultAsync();
            while (dateToAdd <= EndDate)
            {
                if (i < TimeList.Count() && dateToAdd == TimeList.ElementAt(i).TimeLogDate)
                {
                    var TotalMinutes = TimeList.ElementAt(i).TotalMinutes;
                    GrandTotalMinutes += TotalMinutes;
                    var TotalHour = (int)(TimeList.ElementAt(i).TotalHours);
                    GrandTotalHours += TotalHour;
                    if (TotalMinutes == 60)
                    {
                        TotalMinutes = 0;
                        TotalHour += 1;
                    }
                    else if (TotalMinutes > 60)
                    {
                        TotalHour += (int)Math.Floor((decimal)TotalMinutes / 60);
                        TotalMinutes = (TotalMinutes % 60);
                    }

                    if (employee.OfficeId == 11 && TotalHour == 8 && TotalMinutes > 0)
                    {
                        StringListToReturn.Add(TotalHour.ToString() + ":" + TotalMinutes.ToString() + ".red");
                    }
                    else if (employee.OfficeId == 11 && TotalHour > 8)
                    {
                        StringListToReturn.Add(TotalHour.ToString() + ":" + TotalMinutes.ToString() + ".red");
                    }
                    else if (employee.OfficeId != 11 && TotalHour == 7 && TotalMinutes > 30)
                    {
                        StringListToReturn.Add(TotalHour.ToString() + ":" + TotalMinutes.ToString() + ".red");
                    }
                    else if (employee.OfficeId != 11 && TotalHour > 7)
                    {
                        StringListToReturn.Add(TotalHour.ToString() + ":" + TotalMinutes.ToString() + ".red");
                    }
                    else
                    {
                        StringListToReturn.Add(TotalHour.ToString() + ":" + TotalMinutes.ToString());
                    }

                    i++;
                }
                else
                {
                    StringListToReturn.Add("0:0");
                }

                dateToAdd = dateToAdd.AddDays(1);
            }

            if (GrandTotalMinutes == 60)
            {
                GrandTotalMinutes = 0;
                GrandTotalHours += 1;
            }
            else if (GrandTotalMinutes > 60)
            {
                GrandTotalHours += (int)Math.Floor((decimal)GrandTotalMinutes / 60);
                GrandTotalMinutes = (GrandTotalMinutes % 60);
            }
            StringListToReturn.Add(GrandTotalHours.ToString() + ":" + GrandTotalMinutes.ToString());

            return StringListToReturn;

        }

        public async Task<Data.Timesheet> GetTimesheetLogForAGivenDate<Timesheet>(int? OrgID, int EmployeeID, DateTime LogDate ,
                                        int? EndClientID = null, int? BrandID = null, int? CategoryID = null, int? CampaignID = null)
        {
            if (OrgID == 0)
            {
                var result = await TimesheetRepository.All().Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate == LogDate && s.OrgId == null).FirstOrDefaultAsync();
                return result;
            }
            else
            {
                if (EndClientID <= 0) { EndClientID = null; }
                if (BrandID <= 0) { BrandID = null; }
                if (CategoryID <= 0) { CategoryID = null; }
                if (CampaignID <= 0) { CampaignID = null; }
                var result = await TimesheetRepository.All().Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate == LogDate && s.OrgId == OrgID &&
                                                                   s.EndClientId == EndClientID && s.BrandId == BrandID && s.CategoryId == CategoryID &&
                                                                   s.CampaignId == CampaignID).FirstOrDefaultAsync();
                return result;
            }

        }

        public async Task<Dictionary<int, IEnumerable<Data.TimesheetLogBreakdown>>> GetAllTimesheetLogBreakdown<TimesheetLogBreakdown>(int EmployeeID, DateTime StartDate, DateTime EndDate)
        {
            var result = await TimesheetRepository.All().Where(s => s.EmployeeId == EmployeeID && s.TimeLogDate >= StartDate && s.TimeLogDate <= EndDate).ToListAsync();
            var thisDictionary = new Dictionary<int, IEnumerable<Data.TimesheetLogBreakdown>>();
            for (var i = 0; i < result.Count; i++)
            {
                IEnumerable<Data.TimesheetLogBreakdown> allTimesheetBreakdown = await TimesheetBreakdownRepo.All()
                                                                                      .Where(tr => tr.TimesheetId == result.ElementAt(i).Id && tr.DeletedDateTime == null)
                                                                                      .Select(br => br).ToListAsync();
                thisDictionary.Add(result.ElementAt(i).Id, allTimesheetBreakdown);
            }
            return thisDictionary;
        }

        public async Task<Dictionary<int, Data.TimesheetTaskCategory>> GetAllTimesheetCategories<TimesheetTaskCategory>(int EmployeeID, DateTime StartDate, DateTime EndDate)
        {
            var result = await TimesheetBreakdownRepo.All()
                              .Join(TimesheetRepository.All(),
                              br => br.TimesheetId,
                              t => t.Id,
                              (br, t) => new { Breakdown = br, Timesheet = t })
                              .Where(s => s.Timesheet.EmployeeId == EmployeeID && s.Timesheet.TimeLogDate >= StartDate && s.Timesheet.TimeLogDate <= EndDate && s.Breakdown.DeletedDateTime == null)
                              .Select(br => br.Breakdown)
                                  .Join(TimesheetCategoryRepo.All(),
                                        tbr => tbr.CategoryId,
                                        tc => tc.Id,
                                        (tbr, tc) => new { Breakdown = tbr, Category = tc })
                                  .Select(o => o.Category).Distinct()
                                  .ToListAsync();

            //var result = await TimesheetBreakdownRepo.All()
            //      .Join(TimesheetRepository.All(),
            //      br => br.TimesheetID,
            //      t => t.Id,
            //      (br, t) => new { Breakdown = br, Timesheet = t })
            //      .Where(s => s.Timesheet.EmployeeID == EmployeeID && s.Timesheet.TimeLogDate >= StartDate && s.Timesheet.TimeLogDate <= EndDate)
            //      .Select(br => br.Breakdown).ToListAsync();

            var thisDictionary = new Dictionary<int, Data.TimesheetTaskCategory>();
            for (var i = 0; i < result.Count; i++)
            {
                Data.TimesheetTaskCategory thisTimesheetCategory = TimesheetCategoryRepo.All()
                                                                               .Where(tc => tc.Id == result.ElementAt(i).Id)
                                                                               .Select(o => o).FirstOrDefault();
                thisDictionary.Add(result.ElementAt(i).Id, thisTimesheetCategory);
            }
            return thisDictionary;
        }

        public async Task<Data.TimesheetTaskCategory> GetTimesheetCategory<TimesheetTaskCategory>(int CategoryId)
        {
            var result = await TimesheetCategoryRepo.All().Where(tc => tc.Id == CategoryId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<Data.TimesheetTaskCategory>> GetAllTimesheetForTeam<TimesheetTaskCategory>(EmployeeTeam team = null, Byte CategoryTypeID = 0)
        {
            if (team == null && CategoryTypeID == 0)
            {
                var result = await TimesheetCategoryRepo.All()
                                   .OrderBy(o => o.CategoryName).ToListAsync();
                return result;
            }
            else if (team == null && CategoryTypeID > 0)
            {
                var result = await TimesheetCategoryRepo.All()
                                  .OrderBy(o => o.CategoryName)
                                  .Where(tc => tc.CategoryTypeId == CategoryTypeID)
                                  .Select(tc => tc).ToListAsync();
                return result;
            }
            else if (team.Id == 21) //QA team
            {
                var result = await TimesheetCategoryRepo.All()
                                   .OrderBy(o => o.CategoryName)
                                   .Where(tc => (tc.AppliesToAllDepartments == true || tc.AppliesToQa == true) && tc.CategoryTypeId == CategoryTypeID)
                                   .Select(tc => tc).ToListAsync();
                return result;
            }
            else if (team.Id == 12) //VM team
            {
                var result = await TimesheetCategoryRepo.All()
                                   .OrderBy(o => o.CategoryName)
                                   .Where(tc => (tc.AppliesToAllDepartments == true || tc.AppliesToVm == true) && tc.CategoryTypeId == CategoryTypeID)
                                   .Select(tc => tc).ToListAsync();
                return result;
            }
            else if (team.DepartmentId == 9) //GLS
            {
                var result = await TimesheetCategoryRepo.All()
                                   .OrderBy(o => o.CategoryName)
                                   .Where(tc => (tc.AppliesToAllDepartments == true || tc.AppliesToGls == true) && tc.CategoryTypeId == CategoryTypeID)
                                   .Select(tc => tc).ToListAsync();
                return result;
            }
            else if (team.DepartmentId == 10) //T&P
            {
                var result = await TimesheetCategoryRepo.All()
                                   .OrderBy(o => o.CategoryName)
                                   .Where(tc => (tc.AppliesToAllDepartments == true || tc.AppliesToTandP == true) && tc.CategoryTypeId == CategoryTypeID)
                                   .Select(tc => tc).ToListAsync();
                return result;
            }
            else if (team.Id == 1 || team.Id == 2 || team.Id == 3) //Sales team
            {
                var result = await TimesheetCategoryRepo.All()
                                   .OrderBy(o => o.CategoryName)
                                   .Where(tc => (tc.AppliesToAllDepartments == true || tc.AppliesToSales == true) && tc.CategoryTypeId == CategoryTypeID)
                                   .Select(tc => tc).ToListAsync();
                return result;
            }
            else if (team.Id == 4) //Enquiries team
            {
                var result = await TimesheetCategoryRepo.All()
                                   .OrderBy(o => o.CategoryName)
                                   .Where(tc => (tc.AppliesToAllDepartments == true || tc.AppliesToEnquiries == true) && tc.CategoryTypeId == CategoryTypeID)
                                   .Select(tc => tc).ToListAsync();
                return result;
            }
            else if (team.Id == 5) //Marketing
            {
                var result = await TimesheetCategoryRepo.All()
                                   .OrderBy(o => o.CategoryName)
                                   .Where(tc => (tc.AppliesToAllDepartments == true || tc.AppliesToMarketing == true) && tc.CategoryTypeId == CategoryTypeID)
                                   .Select(tc => tc).ToListAsync();
                return result;
            }
            else if (team.DepartmentId == 5) //Tech Department
            {
                var result = await TimesheetCategoryRepo.All()
                                   .OrderBy(o => o.CategoryName)
                                   .Where(tc => (tc.AppliesToAllDepartments == true || tc.AppliesToTech == true) && tc.CategoryTypeId == CategoryTypeID)
                                   .Select(tc => tc).ToListAsync();
                return result;
            }
            else
            {
                var result = await TimesheetCategoryRepo.All()
                                   .OrderBy(o => o.CategoryName)
                                   .Where(tc => (tc.AppliesToAllDepartments == true) && tc.CategoryTypeId == CategoryTypeID)
                                   .Select(tc => tc).ToListAsync();
                return result;
            }

        }

        public async Task<TimesheetLogBreakdown> GetBreakdownById(int breakdownId)
        {
            var result = await TimesheetBreakdownRepo.All().Where(tb => tb.Id == breakdownId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Data.TimesheetsApproval> GetApprovalDetailsForTimesheets(int employeeId, DateTime weekStarting)
        {
            var result = await TimesheetApprovalRepo.All().Where(ta => ta.TimesheetsEmployeeId == employeeId && ta.StartDate == weekStarting).FirstOrDefaultAsync();
            return result;
        }

        public async Task<int> ApproveTimesheetForWeek<TimesheetsApproval>(int EmployeeIdToApprove, DateTime StartDate, DateTime EndDate, int ApprovedByEmpId)
        {
            var thisApproval = await TimesheetApprovalRepo.All().Where(tb => tb.StartDate == StartDate && tb.EndDate == EndDate && tb.TimesheetsEmployeeId == EmployeeIdToApprove ).FirstOrDefaultAsync();

            thisApproval.ApprovedDateTime = GeneralUtils.GetCurrentUKTime();
            thisApproval.ApprovedByEmpId = ApprovedByEmpId;

            TimesheetApprovalRepo.Update(thisApproval);
            await TimesheetApprovalRepo.SaveChangesAsync();

            return thisApproval.Id;
        }

        public async Task<int> UnlockTimesheetForEditing(int EmployeeIdToUnlock, DateTime StartDate, DateTime EndDate, int UnlockedByEmpId)
        {
            var thisApproval = await TimesheetApprovalRepo.All().Where(tb => tb.StartDate == StartDate && tb.EndDate == EndDate && tb.TimesheetsEmployeeId == EmployeeIdToUnlock).FirstOrDefaultAsync();

            thisApproval.UnlockDateTime = GeneralUtils.GetCurrentUKTime();
            thisApproval.UnlockedByEmpId = UnlockedByEmpId;
            thisApproval.SubmissionDateTime = null;

            TimesheetApprovalRepo.Update(thisApproval);
            await TimesheetApprovalRepo.SaveChangesAsync();

            return thisApproval.Id;
        }

        public async System.Threading.Tasks.Task SubmitTimesheet(int EmployeeIdToApprove, DateTime StartDate, DateTime EndDate)
        {
            var existingApproval = await TimesheetApprovalRepo.All().Where(tb => tb.StartDate == StartDate && tb.EndDate == EndDate && tb.TimesheetsEmployeeId == EmployeeIdToApprove).FirstOrDefaultAsync();
            if (existingApproval == null)
            {
                var thisApproval = new TimesheetsApproval()
                {
                    TimesheetsEmployeeId = EmployeeIdToApprove,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    SubmissionDateTime = GeneralUtils.GetCurrentUKTime()
                };

                await TimesheetApprovalRepo.AddAsync(thisApproval);
                await TimesheetApprovalRepo.SaveChangesAsync();
            }

            else {
                existingApproval.SubmissionDateTime = GeneralUtils.GetCurrentUKTime();
                existingApproval.UnlockDateTime = null;
                TimesheetApprovalRepo.Update(existingApproval);
                await TimesheetApprovalRepo.SaveChangesAsync();
            }
            
        }
        public async Task<Timesheet> GetTimesheetLogById(int logId)
        {
            var result = await TimesheetRepository.All().Where(tb => tb.Id == logId).FirstOrDefaultAsync();
            return result;
        }
        public async Task<int> UpdateBreakdown<TimesheetBreakdownModel>(int Id, int CategoryID, short TaskHours, short TaskMinutes, short LastModifiedByEmpId)
        {
            var thisTimesheet = await GetBreakdownById(Id);

            if (TaskHours < 0) { TaskHours = 0; }
            if (TaskMinutes < 0) { TaskMinutes = 0; }

            // maybe add security check later here and autoMapper
            thisTimesheet.CategoryId = CategoryID;
            thisTimesheet.TaskHours = TaskHours;
            thisTimesheet.TaskMinutes = TaskMinutes;
            thisTimesheet.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
            thisTimesheet.LastModifiedByEmpId = LastModifiedByEmpId;

            TimesheetBreakdownRepo.Update(thisTimesheet);
            await TimesheetBreakdownRepo.SaveChangesAsync();

            return Id;
        }

        public async Task<int> UpdateTimesheetLog<Timesheet>(int Id, int EmployeeID, int? OrgID, DateTime TimeLogDate, int? EndClientID = null,
                                                            int? BrandID = null, int? CategoryID = null, int? CampaignID = null)
        {

            var thisTimesheet = await GetTimesheetLogById(Id);

            if (EndClientID <= 0) { EndClientID = null; }
            if (BrandID <= 0) { BrandID = null; }
            if (CategoryID <= 0) { CategoryID = null; }
            if (CampaignID <= 0) { CampaignID = null; }

            // maybe add security check later here and autoMapper
            thisTimesheet.EmployeeId = EmployeeID;
            thisTimesheet.OrgId = OrgID;
            thisTimesheet.TimeLogDate = TimeLogDate;
            thisTimesheet.EndClientId = EndClientID;
            thisTimesheet.BrandId = BrandID;
            thisTimesheet.CategoryId = CategoryID;
            thisTimesheet.CampaignId = CampaignID;

            TimesheetRepository.Update(thisTimesheet);
            await TimesheetRepository.SaveChangesAsync();

            return Id;
        }

        public async Task<int> CreateNewBreakdown<TimesheetBreakdownModel>(int TimesheetLogId, int CategoryID, short TaskHours, short TaskMinutes, short CreatedByEmployeeId)
        {
            if (TaskHours < 0) { TaskHours = 0; }
            if (TaskMinutes < 0) { TaskMinutes = 0; }
            var thisBreakdown = new TimesheetLogBreakdown()
            {
                CategoryId = CategoryID,
                TimesheetId = TimesheetLogId,
                TaskHours = TaskHours,
                TaskMinutes = TaskMinutes,
                CreatedDateTime = GeneralUtils.GetCurrentUKTime(),
                CreatedByEmpId = CreatedByEmployeeId
            };

            await TimesheetBreakdownRepo.AddAsync(thisBreakdown);
            await TimesheetBreakdownRepo.SaveChangesAsync();

            return thisBreakdown.Id;
        }

        public async Task<int> CreateNewTimesheetLog<TimesheetLogModel>(int EmployeeID, int? OrgID, DateTime TimeLogDate, int? EndClientID = null, int? BrandID = null, int? CategoryID = null, int? CampaignID = null)
        {
            if (EndClientID <= 0) { EndClientID = null; }
            if (BrandID <= 0) { BrandID = null; }
            if (CategoryID <= 0) { CategoryID = null; }
            if (CampaignID <= 0) { CampaignID = null; }

            var thisLog = new Timesheet()
            {
                EmployeeId = EmployeeID,
                OrgId = OrgID,
                TimeLogDate = TimeLogDate,
                EndClientId = EndClientID,
                BrandId = BrandID,
                CategoryId = CategoryID,
                CampaignId = CampaignID
            };

            await TimesheetRepository.AddAsync(thisLog);
            await TimesheetRepository.SaveChangesAsync();

            return thisLog.Id;
        }

        public async System.Threading.Tasks.Task Delete(int Id, short DeletedByEmplId)
        {
            var thisTimesheet = await GetBreakdownById(Id);

            thisTimesheet.DeletedByEmpId = DeletedByEmplId;
            thisTimesheet.DeletedDateTime = GeneralUtils.GetCurrentUKTime();
            TimesheetBreakdownRepo.Update(thisTimesheet);
            await TimesheetBreakdownRepo.SaveChangesAsync();
        }

        public async Task<Dictionary<int, string>> GetAllOrgGroupsIdAndName()
        {
            Dictionary<int, string> result = await OrgGroupRepository.All().Select(o => new { o.Id, o.Name, o.DeletedDate }).Where(o => o.DeletedDate == null)
                                                    .Join(OrgRepository.All(),
                                                          gr => gr.Id,
                                                          og => og.OrgGroupId,
                                                          (gr, og) => new { group = gr, org = og})
                                                    .Join(TimesheetRepository.All(),
                                                          gr => gr.org.Id,
                                                          t => t.OrgId,
                                                          (gr, t) => new { group = gr, timesheet = t})
                                                    .Select(og => new { og.group.group.Id, og.group.group.Name }).Distinct().OrderBy(og => og.Name)
                                                    .ToDictionaryAsync(i => i.Id, j => j.Name);
            return result;
        }

        public async Task<Dictionary<int, string>> GetAllOrgsIdAndName()
        {
            Dictionary<int, string> result = await OrgRepository.All().Select(o => new { o.Id, o.OrgName, o.DeletedDate }).Where(o => o.DeletedDate == null)
                                                    .Join(TimesheetRepository.All(),
                                                          og => og.Id,
                                                          t => t.OrgId,
                                                          (og, t) => new { org = og, timesheet = t })
                                                    .Select(og => new {og.org.Id, og.org.OrgName }).Distinct().OrderBy(o => o.OrgName)
                                                    .ToDictionaryAsync(i => i.Id, j => j.OrgName);
            return result;
        }

        public async Task<List<TimesheetTaskCategory>> GetAllActivitiesForGivenTypeIDString(string allTypeIDsString)
        {
            var result = await TimesheetCategoryRepo.All().Where(tc => allTypeIDsString.Contains(tc.CategoryTypeId.ToString()))
                         .ToListAsync();

            return result;
                         
        }


        //public virtual DateTime GetDateFromGivenWeek(this DateTime dt, DayOfWeek weekDay)
        //{
        //    int diff = (7 + (dt.DayOfWeek - weekDay)) % 7;
        //    return dt.AddDays(-1 * diff).Date;
        //}

        //public async Task<IEnumerable<Timesheet>> GetAllTimesheetClentsForAnEmployeeForDateRange<Timesheet>(int EmployeeID, DateTime StartDate, DateTime EndDate)
        //{
        //    using (context)
        //    {
        //        return (IEnumerable<Timesheet>)context.Timesheets.Take(2).ToList();
        //    }
        //}
        ////public async Task<Data.Timesheet> GetTimesheetLogDetails<Timesheet>(int articleId)
        ////{
        ////    //var article = SharePlusArticle();
        ////    using (var context = new TPCoreProductionContext())
        ////    {
        ////        return context.Timesheets.Where(o => o.Id == articleId).FirstOrDefault();
        ////    }
        ////    //return article;
        ////}

        //public async Task<Timesheet> ViewTimeLog<TimesheetLogModel>(int Id)
        //{
        //    using (var context = new TPCoreProductionContext())
        //    {
        //        return context.Timesheets.Where(o => o.Id == Id).FirstOrDefault();
        //    }
        //}

        //public async Task<Timesheet> GetTimesheetLogDetails<TimesheetLogModel>(int articleId)
        //{
        //    //var article = SharePlusArticle();
        //    using (var context = new TPCoreProductionContext())
        //    {
        //        return context.Timesheets.Where(o => o.Id == articleId).FirstOrDefault();
        //    }
        //    //return article;
        //}
    }
}
