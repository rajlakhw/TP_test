using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using SmartAdmin.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;
using SmartAdmin.WebUI.Models.Timesheet;
using System.Web;
using Newtonsoft.Json;
using System.Collections;
using Newtonsoft.Json.Converters;



namespace SmartAdmin.WebUI.Controllers
{
    public class TimesheetController : Controller
    {

        private readonly ITPTimesheetService timesheetService;
        private readonly ITPEmployeesService employeesService;
        private readonly ITPOrgsLogic orgService;
        private readonly IOrgGroupService orgGroupService;
        private readonly ITPEndClient endClientService;
        private readonly IEmailUtilsService emailService;


        public TimesheetController(ITPTimesheetService service, ITPEmployeesService service1,
            ITPOrgsLogic service2, ITPEndClient service3, IOrgGroupService service4, IEmailUtilsService service5)
        {
            this.timesheetService = service;
            this.employeesService = service1;
            this.orgService = service2;
            this.endClientService = service3;
            this.orgGroupService = service4;
            this.emailService = service5;
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(List<Data.TimesheetLogBreakdown> timeLogList)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var contentToUpdate = HttpContext.Request.Body;
            var streamreader = new StreamReader(contentToUpdate);
            var content = streamreader.ReadToEndAsync();
            var jsonString = content.Result;

            List<Data.TimesheetLogBreakdown> AllBreakdowns = JsonConvert.DeserializeObject<List<Data.TimesheetLogBreakdown>>(jsonString);

            for (var i = 0; i < AllBreakdowns.Count(); i++)
            {
                var thisBreakdown = AllBreakdowns.ElementAt(i);
                if (thisBreakdown.Id == -1)
                {
                    if (thisBreakdown.CategoryId > 0 && (thisBreakdown.TaskHours > 0 || thisBreakdown.TaskMinutes > 0))
                    {
                        var result = await timesheetService.CreateNewBreakdown<TimesheetLogBreakdown>(thisBreakdown.TimesheetId, thisBreakdown.CategoryId, thisBreakdown.TaskHours, thisBreakdown.TaskMinutes, LoggedInEmployee.Id);
                    }
                }
                else
                {
                    if (thisBreakdown.TaskHours > 0 || thisBreakdown.TaskMinutes > 0)
                    {
                        var result = await timesheetService.UpdateBreakdown<TimesheetLogBreakdown>(thisBreakdown.Id, thisBreakdown.CategoryId, thisBreakdown.TaskHours, thisBreakdown.TaskMinutes, LoggedInEmployee.Id);
                    }
                    else
                    {
                        await timesheetService.Delete(thisBreakdown.Id, LoggedInEmployee.Id);
                    }

                }

            }

            //var logDate = timesheetService.GetTimesheetLogById(AllBreakdowns.ElementAt(0).TimesheetID).Result.TimeLogDate.ToString("dd-MM-yyyy");

            //return this.RedirectToAction("MyTimesheet", new { weekToShow = logDate });
            return View();
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateTimesheetAsync(List<Data.TimesheetLogBreakdown> timeLogList)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var contentToUpdate = HttpContext.Request.Body;
            var streamreader = new StreamReader(contentToUpdate);
            var content = streamreader.ReadToEndAsync();
            var jsonString = content.Result;
            //string currentURL = Url.ActionLink("Update", "Timesheet");
            //string jsonString = Uri.UnescapeDataString(currentURL.Substring(currentURL.IndexOf("Update/") + "Update/".Length));
            Data.Timesheet ThisTimesheet = JsonConvert.DeserializeObject<Data.Timesheet>(jsonString);


            if (ThisTimesheet.Id == -1)
            {
                var thisOrg = await orgService.GetOrgDetails((int)ThisTimesheet.OrgId);
                if (thisOrg != null)
                {
                    var result = await timesheetService.CreateNewTimesheetLog<Timesheet>(LoggedInEmployee.Id, ThisTimesheet.OrgId, ThisTimesheet.TimeLogDate, ThisTimesheet.EndClientId, ThisTimesheet.BrandId,
                                                                                        ThisTimesheet.CategoryId, ThisTimesheet.CampaignId);

                }

            }
            else
            {
                var result = await timesheetService.UpdateTimesheetLog<Timesheet>(ThisTimesheet.Id, LoggedInEmployee.Id, ThisTimesheet.OrgId, ThisTimesheet.TimeLogDate, ThisTimesheet.EndClientId, ThisTimesheet.BrandId,
                                                                                         ThisTimesheet.CategoryId, ThisTimesheet.CampaignId);
            }

            //var logDate = timesheetService.GetTimesheetLogById(AllBreakdowns.ElementAt(0).TimesheetID).Result.TimeLogDate.ToString("dd-MM-yyyy");

            //return this.RedirectToAction("MyTimesheet", new { weekToShow = logDate });
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> BulkDeleteAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var AllLogIdsToDeleteString = content.Result;
            String[] AllLogIdsToDelete = AllLogIdsToDeleteString.Split(",");

            for (var i = 0; i < AllLogIdsToDelete.Length; i++)
            {
                int BreakdownId = Int32.Parse(AllLogIdsToDelete[i]);
                await timesheetService.Delete(BreakdownId, LoggedInEmployee.Id);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ApproveThisWeek()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            Employee employeeForApproval = await employeesService.IdentifyCurrentUserById(Int32.Parse(stringToProcess.Split("$")[0]));
            DateTime StartDateRange = DateTime.Parse(stringToProcess.Split("$")[1]);
            DateTime EndDateRange = StartDateRange.AddDays(6);
            Employee loggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            await timesheetService.ApproveTimesheetForWeek<TimesheetsApproval>(employeeForApproval.Id, StartDateRange, EndDateRange, loggedInEmployee.Id);

            var CCRecipeints = loggedInEmployee.EmailAddress;
            //if logged in employee is different to manager
            var ManagerObject = employeesService.GetManagerOfEmployee(employeeForApproval.Id).Result;
            if (loggedInEmployee.Id != ManagerObject.Id)
            {
                CCRecipeints += ", " + ManagerObject.EmailAddress;
            }

            //send email to manager and current employee that the timesheet is ready to approve
            emailService.SendMail("my plus <myplus@translateplus.com>", employeeForApproval.EmailAddress,
                                  "Timesheet approved",
                                  String.Format("<p>Dear {0},<br/><br/>Your timesheet for the period <b>{1}</b> to <b>{2}</b> has been approved by {3}.</p>",
                                                employeeForApproval.FirstName,
                                                StartDateRange.ToString("dddd dd MMMM yyyy"), EndDateRange.ToString("dddd dd MMMM yyyy"),
                                                loggedInEmployee.FirstName + " " + loggedInEmployee.Surname),
                                  CCRecipients: CCRecipeints);


            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UnlockTimesheetForEditing()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            Employee employeeForUnlocking = await employeesService.IdentifyCurrentUserById(Int32.Parse(stringToProcess.Split("$")[0]));
            DateTime StartDateRange = DateTime.Parse(stringToProcess.Split("$")[1]);
            DateTime EndDateRange = StartDateRange.AddDays(6);
            Employee loggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            await timesheetService.UnlockTimesheetForEditing(employeeForUnlocking.Id, StartDateRange, EndDateRange, loggedInEmployee.Id);

            var CCRecipeints = loggedInEmployee.EmailAddress;
            //if logged in employee is different to manager
            var ManagerObject = employeesService.GetManagerOfEmployee(employeeForUnlocking.Id).Result;
            if (loggedInEmployee.Id != ManagerObject.Id)
            {
                CCRecipeints += ", " + ManagerObject.EmailAddress;
            }

            //send email to manager and current employee that the timesheet is unlocked for editing
            emailService.SendMail("my plus <myplus@translateplus.com>", employeeForUnlocking.EmailAddress,
                                  "Timesheet unlocked for editing",
                                  String.Format("<p>Dear {0},<br/><br/>Your timesheet for the period <b>{1}</b> to <b>{2}</b> has been unlocked for editing by {3}.</p>",
                                                employeeForUnlocking.FirstName,
                                                StartDateRange.ToString("dddd dd MMMM yyyy"), EndDateRange.ToString("dddd dd MMMM yyyy"),
                                                loggedInEmployee.FirstName + " " + loggedInEmployee.Surname),
                                  CCRecipients: CCRecipeints);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTimesheet()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            Employee employeeForApproval = await employeesService.IdentifyCurrentUserById(Int32.Parse(stringToProcess.Split("$")[0]));
            DateTime StartDateRange = DateTime.Parse(stringToProcess.Split("$")[1]);
            DateTime EndDateRange = StartDateRange.AddDays(6);

            await timesheetService.SubmitTimesheet(employeeForApproval.Id, StartDateRange, EndDateRange);

           
            var ManagerObject = employeesService.GetManagerOfEmployee(employeeForApproval.Id).Result;
            if (ManagerObject == null) { ManagerObject = employeeForApproval; }

            //send email to manager and current employee that the timesheet is ready to approve
            emailService.SendMail("my plus <myplus@translateplus.com>", ManagerObject.EmailAddress,
                                  "Timesheet ready for approval",
                                  String.Format("<p>Dear {0},<br/><br/>{1} has submitted their timesheet for the period <b>{2}</b> to <b>{3}</b>." +
                                                "<br/><br/>Please review and <a href='https://myplusbeta.publicisgroupe.net/timesheet/MyTeamsTimesheet/{4}/{5}'>approve</a> their timesheet in the system.</p>",
                                                ManagerObject.FirstName, employeeForApproval.FirstName + " " + employeeForApproval.Surname,
                                                StartDateRange.ToString("dddd dd MMMM yyyy"), EndDateRange.ToString("dddd dd MMMM yyyy"),
                                                StartDateRange.ToString("dd-MM-yyyy"), employeeForApproval.Id),
                                  CCRecipients: employeeForApproval.EmailAddress);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTimesheetLog()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var timesheetLogJsonString = stringToUse.Split("$tp_timesheet$")[0];
            var breakdownJsonString = stringToUse.Split("$tp_timesheet$")[1];

            List<Data.TimesheetLogBreakdown> AllBreakdowns = JsonConvert.DeserializeObject<List<Data.TimesheetLogBreakdown>>(breakdownJsonString);
            List<Data.Timesheet> NewLog = JsonConvert.DeserializeObject<List<Data.Timesheet>>(timesheetLogJsonString);

            bool toCreateNewLog = false;
            for (var i = 0; i < AllBreakdowns.Count(); i++)
            {
                var thisBreakdown = AllBreakdowns[i];
                if (thisBreakdown.CategoryId > 0 && (thisBreakdown.TaskHours > 0 || thisBreakdown.TaskMinutes > 0))
                {
                    var taskCategory = timesheetService.GetTimesheetCategory<TimesheetTaskCategory>(thisBreakdown.CategoryId);
                    if (taskCategory.Result.CategoryTypeId == 1 || taskCategory.Result.CategoryTypeId == 2)
                    {
                        if (NewLog[0].OrgId != null)
                        {
                            toCreateNewLog = true; break;
                        }
                        else
                        {
                            toCreateNewLog = false; break;
                        }
                    }
                    else
                    {
                        toCreateNewLog = true; break;
                    }

                }
            }

            if (toCreateNewLog == true)
            {
                var timesheetID = 0;
                var timesheetLog = await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(NewLog[0].OrgId, NewLog[0].EmployeeId.Value, NewLog[0].TimeLogDate,
                                                                                                       NewLog[0].EndClientId, NewLog[0].BrandId, NewLog[0].CategoryId, NewLog[0].CampaignId);
                if (timesheetLog == null)
                {
                    //var thisOrg = await orgService.GetOrgDetails((int)NewLog[0].OrgId);
                    //if (thisOrg != null)
                    //{
                        var result = await timesheetService.CreateNewTimesheetLog<Timesheet>(NewLog[0].EmployeeId.Value, NewLog[0].OrgId, NewLog[0].TimeLogDate, NewLog[0].EndClientId, NewLog[0].BrandId, NewLog[0].CategoryId, NewLog[0].CampaignId);
                        timesheetID = result;
                    //}
                }
                else
                {
                    timesheetID = timesheetLog.Id;
                }

                for (var i = 0; i < AllBreakdowns.Count(); i++)
                {
                    var thisBreakdown = AllBreakdowns[i];
                    if (thisBreakdown.CategoryId > 0 && (thisBreakdown.TaskHours > 0 || thisBreakdown.TaskMinutes > 0))
                    {
                        var result1 = await timesheetService.CreateNewBreakdown<TimesheetLogBreakdown>(timesheetID, thisBreakdown.CategoryId, thisBreakdown.TaskHours, thisBreakdown.TaskMinutes, LoggedInEmployee.Id);
                    }
                }

            }
            //String[] AllLogIdsToDelete = AllLogIdsToDeleteString.Split(",");

            //for (var i = 0; i < AllLogIdsToDelete.Length; i++)
            //{
            //    int BreakdownId = Int32.Parse(AllLogIdsToDelete[i]);
            //    await timesheetService.Delete(BreakdownId, LoggedInEmployee.Id);
            //}

            //return this.RedirectToAction("MyTimesheet", new { weekToShow = NewLog[0].TimeLogDate.ToString("dd-MM-yyyy") });
            return Ok();
        }



        //[Route("[controller]/[action]")]
        //[HttpPost]
        //public async Task<IActionResult> UpdateAsync(String timeLogList)
        //{
        //    Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
        //    var test = Url.Content("timeLogList");
        //    string currentURL = Url.ActionLink("Update", "Timesheet");
        //    string jsonString = Uri.UnescapeDataString(currentURL.Substring(currentURL.IndexOf("Update/") + "Update/".Length));
        //    List<Data.TimesheetLogBreakdown> AllBreakdowns = JsonConvert.DeserializeObject<List<Data.TimesheetLogBreakdown>>(jsonString);

        //    for (var i = 0; i < AllBreakdowns.Count(); i++)
        //    {
        //        var thisBreakdown = AllBreakdowns.ElementAt(i);
        //        var result = await timesheetService.Update<TimesheetLogBreakdown>(thisBreakdown.Id, thisBreakdown.CategoryID, thisBreakdown.TaskHours, thisBreakdown.TaskMinutes, LoggedInEmployee.Id);
        //    }

        //    var logDate = timesheetService.GetTimesheetLogById(AllBreakdowns.ElementAt(0).TimesheetID).Result.TimeLogDate.ToString("dd-MM-yyyy");

        //    return this.RedirectToAction("MyTimesheet", new { weekToShow = logDate });
        //    //return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateAsync()
        //{
        //    Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
        //    string currentURL = Url.ActionLink("Update", "Timesheet");

        //    string jsonString = Uri.UnescapeDataString(currentURL.Substring(currentURL.IndexOf("Update/") + "Update/".Length));
        //    List<Data.TimesheetLogBreakdown> AllBreakdowns = JsonConvert.DeserializeObject<List<Data.TimesheetLogBreakdown>>(jsonString);

        //    for (var i = 0; i < AllBreakdowns.Count(); i++)
        //    {
        //        var thisBreakdown = AllBreakdowns.ElementAt(i);
        //        var result = await timesheetService.Update<TimesheetLogBreakdown>(thisBreakdown.Id, thisBreakdown.CategoryID, thisBreakdown.TaskHours, thisBreakdown.TaskMinutes, LoggedInEmployee.Id);
        //    }

        //    return this.RedirectToAction("MyTimesheet", new { weekToShow = ViewBag.WeekStarting.toString() });
        //    //return View();
        //}

        //public async Task<IActionResult> UpdateAsync(String Base64String)
        //{
        //    Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(Environment.UserName);
        //    DateTime StartDate = TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Monday);
        //    DateTime EndDate = TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Sunday);
        //    IEnumerable<Timesheet> results = await timesheetService.GetAllTimesheetLogs<Timesheet>(LoggedInEmployee.Id, StartDate, EndDate);
        //    //return this.RedirectToAction("MyTimesheet");
        //    return View();
        //}

        //public IActionResult Update()
        //{
        //    return View();
        //}

        //public IActionResult MyTimesheet() => View();

        //public async Task<IActionResult> MyTimesheetAsync()
        //{
        //    IEnumerable<Timesheet> results = await service.GetAllTimesheetLogs<Timesheet>();
        //    return View(results);
        //}

        //public IActionResult MyTimesheet()
        //{
        //    //TPTimesheetLogic thisObject = new TPTimesheetLogic(context);
        //    //IEnumerable<Timesheet> result = thisObject.GetAllTimesheetLogs();
        //    IEnumerable<Timesheet> result = TPTestLogic.GetAllTimesheetLogs(context);
        //    TPCommonGeneralUtils.CurrentEmployee = TPEmployeesLogic.IdentifyCurrentUser(Environment.UserName, context);
        //    ViewBag.CurrentEmployee = TPCommonGeneralUtils.CurrentEmployee.FirstName + ' ' + TPCommonGeneralUtils.CurrentEmployee.Surname;
        //    return View(result);

        //}

        [Route("[controller]/[action]/{weekToShow}")]
        public async Task<IActionResult> MyTimesheetAsync(string weekToShow)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("Marovcha");
            ViewBag.CurrentEmployee = LoggedInEmployee.FirstName + ' ' + LoggedInEmployee.Surname;
            ViewBag.CurrentEmployeeId = LoggedInEmployee.Id;
            //DateTime CurrentDate = ViewBag.WeekStarting;
            //DateTime CurrentDate = currentWeek.Value.AddDays(7);
            DateTime CurrentDate = DateTime.Parse(weekToShow);
            //DateTime CurrentDate = new DateTime(2021, 08, 23);
            DateTime StartDate = TPCommonGeneralUtils.GetDateFromGivenWeek(CurrentDate, DayOfWeek.Monday);
            DateTime EndDate = TPCommonGeneralUtils.GetDateFromGivenWeek(CurrentDate, DayOfWeek.Sunday);
            ViewBag.DateSelected = CurrentDate;
            ViewBag.DateStartingLongString = StartDate.ToString("dd MMM yyyy");

            if (TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Monday) == StartDate)
            {
                ViewBag.IsCurrentWeek = true;
            }
            else
            {
                ViewBag.IsCurrentWeek = false;
            }

            //Hashtable RowHeaders = await timesheetService.GetAllRowHeaderOrgCategoriesForAWeek(LoggedInEmployee.Id, StartDate, EndDate);
            List<Data.Org> RowHeaders = await timesheetService.GetAllRowHeaderOrgCategoriesForAWeek<Org>(LoggedInEmployee.Id, StartDate, EndDate);
            List<Data.Timesheet> MondayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> TuesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> WednesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> ThursdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> FridayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SaturdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SundayLogs = new List<Data.Timesheet>();
            List<String> AllDailyTotalStrings = new List<String>();
            List<String> AllWeeklyTotalStrings = new List<String>();

            //IEnumerable<Data.Timesheet> WednesdayLogs = null;
            //IEnumerable<Data.Timesheet> ThursdayLogs = null;
            //IEnumerable<Data.Timesheet> FridayLogs = null;

            for (var i = 0; i < RowHeaders.Count; i++)
            {
                MondayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday)));
                TuesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday)));
                WednesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday)));
                ThursdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday)));
                FridayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday)));
                SaturdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday)));
                SundayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday)));
                AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForOrg(LoggedInEmployee.Id, StartDate, EndDate, RowHeaders.ElementAt(i).Id));

            }

            List<Data.Timesheet> AllNonClientTasksForWeek = new List<Timesheet>();

            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday)));
            AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForOrg(LoggedInEmployee.Id, StartDate, EndDate, null));

            TimesheetOverviewModel results = new TimesheetOverviewModel()
            {
                //AllTimesheetLogsForCurrentView = await timesheetService.GetAllTimesheetLogs<Data.Timesheet>(LoggedInEmployee.Id, StartDate, EndDate),
                AllRowHeaderForAWeek = RowHeaders,
                AllMondayTimeLogs = MondayLogs,
                AllTuesdayTimeLogs = TuesdayLogs,
                AllWednesdayTimeLogs = WednesdayLogs,
                AllThursdayTimeLogs = ThursdayLogs,
                AllFridayTimeLogs = FridayLogs,
                AllSaturdayTimeLogs = SaturdayLogs,
                AllSundayTimeLogs = SundayLogs,
                StartDate = StartDate,
                EndDate = EndDate,
                AllTimesheetLogsBreakdown = await timesheetService.GetAllTimesheetLogBreakdown<TimesheetLogBreakdown>(LoggedInEmployee.Id, StartDate, EndDate),
                AllTimesheetCategories = await timesheetService.GetAllTimesheetCategories<TimesheetTaskCategory>(LoggedInEmployee.Id, StartDate, EndDate),
                AllChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 1),
                AllNonChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 2),
                AllNonClientTasksForEmployee = AllNonClientTasksForWeek,
                AllNonClientActivitiesForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 3),
                WeeklyTotalString = AllWeeklyTotalStrings,
                DailyTotalString = await timesheetService.GetAllDailyTotals(LoggedInEmployee.Id, StartDate, EndDate),
                ApprovalDetails = await timesheetService.GetApprovalDetailsForTimesheets(LoggedInEmployee.Id, StartDate),
                EndClientList = await endClientService.GetAllEndClient()

            };
            if (results.ApprovalDetails != null && results.ApprovalDetails.ApprovedByEmpId != null)
            {
                Employee ApprovedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.ApprovedByEmpId.GetValueOrDefault()).Result;
                ViewBag.ApprovedByEmployee = ApprovedByEmp.FirstName + ' ' + ApprovedByEmp.Surname;
            }

            if (results.ApprovalDetails != null && results.ApprovalDetails.UnlockedByEmpId != null)
            {
                Employee UnlockedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.UnlockedByEmpId).Result;
                ViewBag.UnlockedByEmployee = UnlockedByEmp.FirstName + ' ' + UnlockedByEmp.Surname;
            }
            if (results.ApprovalDetails != null && results.ApprovalDetails.SubmissionDateTime != null)
            {
                ViewBag.TimesheetSubmitted = true;
            }

            return View(results);
        }


        [Route("[controller]/[action]/{weekToShow}")]
        public async Task<IActionResult> MyTimesheetTandPAsync(string weekToShow)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("rodnorri");
            ViewBag.CurrentEmployee = LoggedInEmployee.FirstName + ' ' + LoggedInEmployee.Surname;
            ViewBag.CurrentEmployeeId = LoggedInEmployee.Id;
            //DateTime CurrentDate = ViewBag.WeekStarting;
            //DateTime CurrentDate = currentWeek.Value.AddDays(7);
            DateTime CurrentDate = DateTime.Parse(weekToShow);
            //DateTime CurrentDate = new DateTime(2021, 08, 23);
            DateTime StartDate = TPCommonGeneralUtils.GetDateFromGivenWeek(CurrentDate, DayOfWeek.Monday);
            DateTime EndDate = TPCommonGeneralUtils.GetDateFromGivenWeek(CurrentDate, DayOfWeek.Sunday);
            ViewBag.DateSelected = CurrentDate;
            if (TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Monday) == StartDate)
            {
                ViewBag.IsCurrentWeek = true;
            }
            else
            {
                ViewBag.IsCurrentWeek = false;
            }

            ViewBag.DateStartingLongString = StartDate.ToString("dd MMM yyyy");

            Hashtable RowHeaders = await timesheetService.GetAllTandPRowHeadersForAWeek(LoggedInEmployee.Id, StartDate, EndDate);
            //List<Data.Org> RowHeaders = await timesheetService.GetAllRowHeaderOrgCategoriesForAWeek<Org>(LoggedInEmployee.Id, StartDate, EndDate);
            List<Data.Timesheet> MondayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> TuesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> WednesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> ThursdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> FridayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SaturdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SundayLogs = new List<Data.Timesheet>();
            List<String> AllDailyTotalStrings = new List<String>();
            List<String> AllWeeklyTotalStrings = new List<String>();

            List<Data.EndClient> EndClients = new List<EndClient>();
            List<Data.EndClientData> GroupeCategories = new List<EndClientData>();
            List<Data.EndClientData> GroupeBrands = new List<EndClientData>();
            List<Data.EndClientData> GroupeCampaigns = new List<EndClientData>();


            //IEnumerable<Data.Timesheet> WednesdayLogs = null;
            //IEnumerable<Data.Timesheet> ThursdayLogs = null;
            //IEnumerable<Data.Timesheet> FridayLogs = null;

            //for (var i = 0; i < RowHeaders.Count; i++)
            foreach (Dictionary<int, Org> key in RowHeaders.Keys)
            {

                Dictionary<int, Org> thisDict = (Dictionary<int, Org>)key;
                Org currentOrg = thisDict.ElementAt(0).Value;

                var AllFieldStrings = RowHeaders[thisDict].ToString().Split("$");
                int EndClientID = 0;
                int BrandID = 0;
                int CategoryID = 0;
                int CampaignID = 0;
                if (AllFieldStrings.Count() > 0)
                {
                    if (AllFieldStrings[0] != "") { EndClientID = Int32.Parse(AllFieldStrings[0]); }
                    if (AllFieldStrings[1] != "") { BrandID = Int32.Parse(AllFieldStrings[1]); }
                    if (AllFieldStrings[2] != "") { CategoryID = Int32.Parse(AllFieldStrings[2]); }
                    if (AllFieldStrings[3] != "") { CampaignID = Int32.Parse(AllFieldStrings[3]); }
                }

                MondayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday), EndClientID, BrandID, CategoryID, CampaignID));
                TuesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday), EndClientID, BrandID, CategoryID, CampaignID));
                WednesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday), EndClientID, BrandID, CategoryID, CampaignID));
                ThursdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday), EndClientID, BrandID, CategoryID, CampaignID));
                FridayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday), EndClientID, BrandID, CategoryID, CampaignID));
                SaturdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday), EndClientID, BrandID, CategoryID, CampaignID));
                SundayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday), EndClientID, BrandID, CategoryID, CampaignID));
                AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForTandPOrg(LoggedInEmployee.Id, StartDate, EndDate, currentOrg.Id, EndClientID, BrandID, CategoryID, CampaignID));

                if (RowHeaders[key] == null || RowHeaders[key].ToString() == "$$$")
                {
                    EndClients.Add(null);
                    GroupeBrands.Add(null);
                    GroupeCategories.Add(null);
                    GroupeCampaigns.Add(null);
                }
                else
                {
                    var GroupeDataString = RowHeaders[key].ToString();
                    var AllGroupeFields = GroupeDataString.Split("$");
                    var ClientIDString = AllGroupeFields[0];
                    if (ClientIDString != "")
                    {
                        EndClients.Add(await endClientService.GetEndClientByID<EndClient>(Int32.Parse(ClientIDString)));
                    }
                    else
                    {
                        EndClients.Add(null);
                    }

                    var BrandIDString = AllGroupeFields[1];
                    if (BrandIDString != "")
                    {
                        GroupeBrands.Add(await endClientService.GetEndClientDataByID<EndClient>(Int32.Parse(BrandIDString)));
                    }
                    else
                    {
                        GroupeBrands.Add(null);
                    }

                    var CategoryIDString = AllGroupeFields[2];
                    if (CategoryIDString != "")
                    {
                        GroupeCategories.Add(await endClientService.GetEndClientDataByID<EndClient>(Int32.Parse(CategoryIDString)));
                    }
                    else
                    {
                        GroupeCategories.Add(null);
                    }

                    var CampaignIDString = AllGroupeFields[3];
                    if (CampaignIDString != "")
                    {
                        GroupeCampaigns.Add(await endClientService.GetEndClientDataByID<EndClient>(Int32.Parse(CampaignIDString)));
                    }
                    else
                    {
                        GroupeCampaigns.Add(null);
                    }

                }


            }

            List<Data.Timesheet> AllNonClientTasksForWeek = new List<Timesheet>();

            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday)));
            AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForTandPOrg(LoggedInEmployee.Id, StartDate, EndDate, null, null, null, null, null));

            TimesheetOverviewModel results = new TimesheetOverviewModel()
            {
                //AllTimesheetLogsForCurrentView = await timesheetService.GetAllTimesheetLogs<Data.Timesheet>(LoggedInEmployee.Id, StartDate, EndDate),
                AllTandPRowHeaderForAWeek = RowHeaders,
                AllMondayTimeLogs = MondayLogs,
                AllTuesdayTimeLogs = TuesdayLogs,
                AllWednesdayTimeLogs = WednesdayLogs,
                AllThursdayTimeLogs = ThursdayLogs,
                AllFridayTimeLogs = FridayLogs,
                AllSaturdayTimeLogs = SaturdayLogs,
                AllSundayTimeLogs = SundayLogs,
                StartDate = StartDate,
                EndDate = EndDate,
                AllTimesheetLogsBreakdown = await timesheetService.GetAllTimesheetLogBreakdown<TimesheetLogBreakdown>(LoggedInEmployee.Id, StartDate, EndDate),
                AllTimesheetCategories = await timesheetService.GetAllTimesheetCategories<TimesheetTaskCategory>(LoggedInEmployee.Id, StartDate, EndDate),
                AllChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 1),
                AllNonChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 2),
                AllNonClientTasksForEmployee = AllNonClientTasksForWeek,
                AllNonClientActivitiesForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 3),
                WeeklyTotalString = AllWeeklyTotalStrings,
                DailyTotalString = await timesheetService.GetAllDailyTotals(LoggedInEmployee.Id, StartDate, EndDate),
                ApprovalDetails = await timesheetService.GetApprovalDetailsForTimesheets(LoggedInEmployee.Id, StartDate),
                AllEndClients = EndClients,
                AllGroupeBrands = GroupeBrands,
                AllGroupeCategories = GroupeCategories,
                AllGroupeCampaigns = GroupeCampaigns,
                EndClientList = await endClientService.GetAllEndClient()

            };
            if (results.ApprovalDetails != null && results.ApprovalDetails.ApprovedByEmpId != null)
            {
                Employee ApprovedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.ApprovedByEmpId.GetValueOrDefault()).Result;
                ViewBag.ApprovedByEmployee = ApprovedByEmp.FirstName + ' ' + ApprovedByEmp.Surname;
            }

            if (results.ApprovalDetails != null && results.ApprovalDetails.UnlockedByEmpId != null)
            {
                Employee UnlockedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.UnlockedByEmpId).Result;
                ViewBag.UnlockedByEmployee = UnlockedByEmp.FirstName + ' ' + UnlockedByEmp.Surname;
            }
            if (results.ApprovalDetails != null && results.ApprovalDetails.SubmissionDateTime != null)
            {
                ViewBag.TimesheetSubmitted = true;
            }

            return View(results);
        }


        [Route("[controller]/[action]/{weekToShow}/{employeeId}")]
        public async Task<IActionResult> MyTeamsTimesheetAsync(string weekToShow, int employeeId)
        {
            Employee loggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee loggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("umenizam");
            EmployeeDepartment loggedInEmployeeDept = await employeesService.GetEmployeeDepartment(loggedInEmployee.Id);
            EmployeeDepartment selectedEmployeeDept = await employeesService.GetEmployeeDepartment(employeeId); ;
            int selectedDeptId = 0;
            if (selectedEmployeeDept != null)
            {
                selectedDeptId = selectedEmployeeDept.Id;
            }

            if (loggedInEmployee.IsTeamManager == true || loggedInEmployee.AccessLevel == 2 ||
                loggedInEmployee.AccessLevel == 3 || loggedInEmployee.AccessLevel == 4)
            {
                //proceed
            }
            else
            {
                //access denied
                return Redirect("/Page/Locked");
            }

            if (loggedInEmployeeDept.Id == 10 || selectedDeptId == 10)
            {
                return RedirectToAction("MyTeamsTimesheetTandP", new { weekToShow = weekToShow, employeeId = employeeId });
            }

            ViewBag.TeamForApproval = employeesService.GetEmployeeTeam<EmployeeTeam>(loggedInEmployee.Id).Result.Name;
            //DateTime CurrentDate = ViewBag.WeekStarting;
            //DateTime CurrentDate = currentWeek.Value.AddDays(7);
            DateTime CurrentDate;
            Employee employeeForApproval = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee employeeForApproval = await employeesService.IdentifyCurrentUser<Employee>("umenizam");

            if (weekToShow == "default" || employeeId == 0)
            {
                CurrentDate = DateTime.Now.AddDays(-7);
                ViewBag.CurrentEmployee = "";
                ViewBag.CurrentEmployeeId = 0;
            }
            else
            {
                employeeForApproval = await employeesService.IdentifyCurrentUserById(employeeId);
                CurrentDate = DateTime.Parse(weekToShow);
                ViewBag.CurrentEmployee = employeeForApproval.FirstName + ' ' + employeeForApproval.Surname;
                ViewBag.CurrentEmployeeId = employeeForApproval.Id;
            }

            //DateTime CurrentDate = new DateTime(2021, 08, 23);
            DateTime StartDate = TPCommonGeneralUtils.GetDateFromGivenWeek(CurrentDate, DayOfWeek.Monday);
            DateTime EndDate = TPCommonGeneralUtils.GetDateFromGivenWeek(CurrentDate, DayOfWeek.Sunday);
            ViewBag.DateSelected = CurrentDate;
            ViewBag.DateSelectedString = CurrentDate.ToString("dd-MM-yyyy");
            ViewBag.DateStartingLongString = StartDate.ToString("dd MMM yyyy");

            if (TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Monday) == StartDate)
            {
                ViewBag.IsCurrentWeek = true;
            }
            else
            {
                ViewBag.IsCurrentWeek = false;
            }

            List<Data.Org> RowHeaders = await timesheetService.GetAllRowHeaderOrgCategoriesForAWeek<Org>(employeeForApproval.Id, StartDate, EndDate);
            List<Data.Timesheet> MondayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> TuesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> WednesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> ThursdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> FridayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SaturdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SundayLogs = new List<Data.Timesheet>();
            List<String> AllDailyTotalStrings = new List<String>();
            List<String> AllWeeklyTotalStrings = new List<String>();

            //IEnumerable<Data.Timesheet> WednesdayLogs = null;
            //IEnumerable<Data.Timesheet> ThursdayLogs = null;
            //IEnumerable<Data.Timesheet> FridayLogs = null;

            for (var i = 0; i < RowHeaders.Count; i++)
            {
                MondayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday)));
                TuesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday)));
                WednesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday)));
                ThursdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday)));
                FridayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday)));
                SaturdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday)));
                SundayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday)));
                AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForOrg(employeeForApproval.Id, StartDate, EndDate, RowHeaders.ElementAt(i).Id));

            }

            List<Data.Timesheet> AllNonClientTasksForWeek = new List<Timesheet>();

            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday)));
            AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForOrg(employeeForApproval.Id, StartDate, EndDate, null));

            TimesheetOverviewModel results = new TimesheetOverviewModel()
            {
                //AllTimesheetLogsForCurrentView = await timesheetService.GetAllTimesheetLogs<Data.Timesheet>(employeeForApproval.Id, StartDate, EndDate),
                AllRowHeaderForAWeek = RowHeaders,
                AllMondayTimeLogs = MondayLogs,
                AllTuesdayTimeLogs = TuesdayLogs,
                AllWednesdayTimeLogs = WednesdayLogs,
                AllThursdayTimeLogs = ThursdayLogs,
                AllFridayTimeLogs = FridayLogs,
                AllSaturdayTimeLogs = SaturdayLogs,
                AllSundayTimeLogs = SundayLogs,
                StartDate = StartDate,
                EndDate = EndDate,
                AllTimesheetLogsBreakdown = await timesheetService.GetAllTimesheetLogBreakdown<TimesheetLogBreakdown>(employeeForApproval.Id, StartDate, EndDate),
                AllTimesheetCategories = await timesheetService.GetAllTimesheetCategories<TimesheetTaskCategory>(employeeForApproval.Id, StartDate, EndDate),
                AllChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(employeeForApproval.Id).Result, 1),
                AllNonChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(employeeForApproval.Id).Result, 2),
                AllNonClientTasksForEmployee = AllNonClientTasksForWeek,
                AllNonClientActivitiesForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(employeeForApproval.Id).Result, 3),
                WeeklyTotalString = AllWeeklyTotalStrings,
                DailyTotalString = await timesheetService.GetAllDailyTotals(employeeForApproval.Id, StartDate, EndDate),
                AllEmployeesForApproval = await employeesService.GetAllEmployeeForThisManager<Employee>(loggedInEmployee.Id),
                ApprovalDetails = await timesheetService.GetApprovalDetailsForTimesheets(employeeForApproval.Id, StartDate)
            };

            if (results.ApprovalDetails != null && results.ApprovalDetails.ApprovedByEmpId != null)
            {
                Employee ApprovedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.ApprovedByEmpId.GetValueOrDefault()).Result;
                ViewBag.ApprovedByEmployee = ApprovedByEmp.FirstName + ' ' + ApprovedByEmp.Surname;
            }

            if (results.ApprovalDetails != null && results.ApprovalDetails.UnlockedByEmpId != null)
            {
                Employee UnlockedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.UnlockedByEmpId).Result;
                ViewBag.UnlockedByEmployee = UnlockedByEmp.FirstName + ' ' + UnlockedByEmp.Surname;
            }

            if (results.ApprovalDetails != null && results.ApprovalDetails.SubmissionDateTime != null)
            {
                ViewBag.TimesheetSubmitted = true;
            }

            return View(results);
        }

        [Route("[controller]/[action]/{weekToShow}/{employeeId}")]
        public async Task<IActionResult> MyTeamsTimesheetTandPAsync(string weekToShow, int employeeId)
        {
            Employee loggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee loggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("umenizam");
            EmployeeDepartment selectedEmployeeDept = await employeesService.GetEmployeeDepartment(employeeId);
            int selectedDeptId = 0;
            if (selectedEmployeeDept != null)
            {
                selectedDeptId = selectedEmployeeDept.Id;
            }

            if (loggedInEmployee.IsTeamManager == true || loggedInEmployee.AccessLevel == 2 ||
               loggedInEmployee.AccessLevel == 3 ||  loggedInEmployee.AccessLevel == 4)
            {
                //proceed
            }
            else
            {
                //access denied
                return Redirect("/Page/Locked");
            }

            if (selectedDeptId == 9)
            {
                return RedirectToAction("MyTeamsTimesheet", new { weekToShow = weekToShow, employeeId = employeeId });
            }

            ViewBag.TeamForApproval = employeesService.GetEmployeeTeam<EmployeeTeam>(loggedInEmployee.Id).Result.Name;
            //DateTime CurrentDate = ViewBag.WeekStarting;
            //DateTime CurrentDate = currentWeek.Value.AddDays(7);
            DateTime CurrentDate;
            Employee employeeForApproval = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee employeeForApproval = await employeesService.IdentifyCurrentUser<Employee>("umenizam");
            if (weekToShow == "default")
            {
                CurrentDate = DateTime.Now.AddDays(-7);
                ViewBag.CurrentEmployee = "";
                ViewBag.CurrentEmployeeId = 0;
            }
            else
            {
                employeeForApproval = await employeesService.IdentifyCurrentUserById(employeeId);
                CurrentDate = DateTime.Parse(weekToShow);
                ViewBag.CurrentEmployee = employeeForApproval.FirstName + ' ' + employeeForApproval.Surname;
                ViewBag.CurrentEmployeeId = employeeForApproval.Id;
            }

            //DateTime CurrentDate = new DateTime(2021, 08, 23);
            DateTime StartDate = TPCommonGeneralUtils.GetDateFromGivenWeek(CurrentDate, DayOfWeek.Monday);
            DateTime EndDate = TPCommonGeneralUtils.GetDateFromGivenWeek(CurrentDate, DayOfWeek.Sunday);
            ViewBag.DateSelected = CurrentDate;
            ViewBag.DateSelectedString = CurrentDate.ToString("dd-MM-yyyy");
            ViewBag.DateStartingLongString = StartDate.ToString("dd MMM yyyy");

            if (TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Monday) == StartDate)
            {
                ViewBag.IsCurrentWeek = true;
            }
            else
            {
                ViewBag.IsCurrentWeek = false;
            }

            Hashtable RowHeaders = await timesheetService.GetAllTandPRowHeadersForAWeek(employeeForApproval.Id, StartDate, EndDate);
            List<Data.Timesheet> MondayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> TuesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> WednesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> ThursdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> FridayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SaturdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SundayLogs = new List<Data.Timesheet>();
            List<String> AllDailyTotalStrings = new List<String>();
            List<String> AllWeeklyTotalStrings = new List<String>();

            List<Data.EndClient> EndClients = new List<EndClient>();
            List<Data.EndClientData> GroupeCategories = new List<EndClientData>();
            List<Data.EndClientData> GroupeBrands = new List<EndClientData>();
            List<Data.EndClientData> GroupeCampaigns = new List<EndClientData>();

            //IEnumerable<Data.Timesheet> WednesdayLogs = null;
            //IEnumerable<Data.Timesheet> ThursdayLogs = null;
            //IEnumerable<Data.Timesheet> FridayLogs = null;

            foreach (Dictionary<int, Org> key in RowHeaders.Keys)
            {
                Dictionary<int, Org> thisDict = (Dictionary<int, Org>)key;
                Org currentOrg = thisDict.ElementAt(0).Value;

                var AllFieldStrings = RowHeaders[thisDict].ToString().Split("$");
                int EndClientID = 0;
                int BrandID = 0;
                int CategoryID = 0;
                int CampaignID = 0;
                if (AllFieldStrings.Count() > 0)
                {
                    if (AllFieldStrings[0] != "") { EndClientID = Int32.Parse(AllFieldStrings[0]); }
                    if (AllFieldStrings[1] != "") { BrandID = Int32.Parse(AllFieldStrings[1]); }
                    if (AllFieldStrings[2] != "") { CategoryID = Int32.Parse(AllFieldStrings[2]); }
                    if (AllFieldStrings[3] != "") { CampaignID = Int32.Parse(AllFieldStrings[3]); }
                }

                MondayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday), EndClientID, BrandID, CategoryID, CampaignID));
                TuesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday), EndClientID, BrandID, CategoryID, CampaignID));
                WednesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday), EndClientID, BrandID, CategoryID, CampaignID));
                ThursdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday), EndClientID, BrandID, CategoryID, CampaignID));
                FridayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday), EndClientID, BrandID, CategoryID, CampaignID));
                SaturdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday), EndClientID, BrandID, CategoryID, CampaignID));
                SundayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday), EndClientID, BrandID, CategoryID, CampaignID));
                AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForTandPOrg(employeeForApproval.Id, StartDate, EndDate, currentOrg.Id, EndClientID, BrandID, CategoryID, CampaignID));

                var GroupeDataString = RowHeaders[key].ToString();
                var AllGroupeFields = GroupeDataString.Split("$");
                var ClientIDString = AllGroupeFields[0];
                if (ClientIDString != "")
                {
                    EndClients.Add(await endClientService.GetEndClientByID<EndClient>(Int32.Parse(ClientIDString)));
                }
                else
                {
                    EndClients.Add(null);
                }

                var BrandIDString = AllGroupeFields[1];
                if (BrandIDString != "")
                {
                    GroupeBrands.Add(await endClientService.GetEndClientDataByID<EndClient>(Int32.Parse(BrandIDString)));
                }
                else
                {
                    GroupeBrands.Add(null);
                }

                var CategoryIDString = AllGroupeFields[2];
                if (CategoryIDString != "")
                {
                    GroupeCategories.Add(await endClientService.GetEndClientDataByID<EndClient>(Int32.Parse(CategoryIDString)));
                }
                else
                {
                    GroupeCategories.Add(null);
                }

                var CampaignIDString = AllGroupeFields[3];
                if (CampaignIDString != "")
                {
                    GroupeCampaigns.Add(await endClientService.GetEndClientDataByID<EndClient>(Int32.Parse(CampaignIDString)));
                }
                else
                {
                    GroupeCampaigns.Add(null);
                }

            }

            List<Data.Timesheet> AllNonClientTasksForWeek = new List<Timesheet>();

            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, employeeForApproval.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday)));
            AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForTandPOrg(employeeForApproval.Id, StartDate, EndDate, null, null, null, null, null));

            TimesheetOverviewModel results = new TimesheetOverviewModel()
            {
                //AllTimesheetLogsForCurrentView = await timesheetService.GetAllTimesheetLogs<Data.Timesheet>(employeeForApproval.Id, StartDate, EndDate),
                AllTandPRowHeaderForAWeek = RowHeaders,
                AllMondayTimeLogs = MondayLogs,
                AllTuesdayTimeLogs = TuesdayLogs,
                AllWednesdayTimeLogs = WednesdayLogs,
                AllThursdayTimeLogs = ThursdayLogs,
                AllFridayTimeLogs = FridayLogs,
                AllSaturdayTimeLogs = SaturdayLogs,
                AllSundayTimeLogs = SundayLogs,
                StartDate = StartDate,
                EndDate = EndDate,
                AllTimesheetLogsBreakdown = await timesheetService.GetAllTimesheetLogBreakdown<TimesheetLogBreakdown>(employeeForApproval.Id, StartDate, EndDate),
                AllTimesheetCategories = await timesheetService.GetAllTimesheetCategories<TimesheetTaskCategory>(employeeForApproval.Id, StartDate, EndDate),
                AllChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(employeeForApproval.Id).Result, 1),
                AllNonChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(employeeForApproval.Id).Result, 2),
                AllNonClientTasksForEmployee = AllNonClientTasksForWeek,
                AllNonClientActivitiesForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(employeeForApproval.Id).Result, 3),
                WeeklyTotalString = AllWeeklyTotalStrings,
                DailyTotalString = await timesheetService.GetAllDailyTotals(employeeForApproval.Id, StartDate, EndDate),
                AllEmployeesForApproval = await employeesService.GetAllEmployeeForThisManager<Employee>(loggedInEmployee.Id),
                ApprovalDetails = await timesheetService.GetApprovalDetailsForTimesheets(employeeForApproval.Id, StartDate),
                AllEndClients = EndClients,
                AllGroupeBrands = GroupeBrands,
                AllGroupeCategories = GroupeCategories,
                AllGroupeCampaigns = GroupeCampaigns,
                EndClientList = await endClientService.GetAllEndClient()
            };

            if (results.ApprovalDetails != null && results.ApprovalDetails.ApprovedByEmpId != null)
            {
                Employee ApprovedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.ApprovedByEmpId.GetValueOrDefault()).Result;
                ViewBag.ApprovedByEmployee = ApprovedByEmp.FirstName + ' ' + ApprovedByEmp.Surname;
            }

            if (results.ApprovalDetails != null && results.ApprovalDetails.UnlockedByEmpId != null)
            {
                Employee UnlockedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.UnlockedByEmpId).Result;
                ViewBag.UnlockedByEmployee = UnlockedByEmp.FirstName + ' ' + UnlockedByEmp.Surname;
            }

            if (results.ApprovalDetails != null && results.ApprovalDetails.SubmissionDateTime != null)
            {
                ViewBag.TimesheetSubmitted = true;
            }

            return View(results);
        }



        public async Task<IActionResult> MyTimesheetAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("rodnorri");

            EmployeeDepartment employeeDept = await employeesService.GetEmployeeDepartment(LoggedInEmployee.Id);
            if (employeeDept.Id == 10)
            {
                return RedirectToAction("MyTimesheetTandP");
            }
            
            ViewBag.CurrentEmployee = LoggedInEmployee.FirstName + ' ' + LoggedInEmployee.Surname;
            ViewBag.CurrentEmployeeId = LoggedInEmployee.Id;
           
            DateTime StartDate = TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Monday);
            DateTime EndDate = TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Sunday);
            
            ViewBag.DateSelected = StartDate;
            ViewBag.DateSelectedString = StartDate.ToString("yyyy-MM-dd");
            ViewBag.DateStartingLongString = StartDate.ToString("dd MMM yyyy");
            ViewBag.IsCurrentWeek = true;
            List<Data.Org> RowHeaders = await timesheetService.GetAllRowHeaderOrgCategoriesForAWeek<Org>(LoggedInEmployee.Id, StartDate, EndDate);
            List<Data.Timesheet> MondayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> TuesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> WednesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> ThursdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> FridayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SaturdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SundayLogs = new List<Data.Timesheet>();
            List<String> AllDailyTotalStrings = new List<String>();
            List<String> AllWeeklyTotalStrings = new List<String>();

            
            for (var i = 0; i < RowHeaders.Count; i++)
            {
                MondayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday)));
                TuesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday)));
                WednesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday)));
                ThursdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday)));
                FridayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday)));
                SaturdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday)));
                SundayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(RowHeaders.ElementAt(i).Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday)));
                AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForOrg(LoggedInEmployee.Id, StartDate, EndDate, RowHeaders.ElementAt(i).Id));
            }

            List<Data.Timesheet> AllNonClientTasksForWeek = new List<Timesheet>();

            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday)));
            AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForOrg(LoggedInEmployee.Id, StartDate, EndDate, null));

            TimesheetOverviewModel results = new TimesheetOverviewModel()
            {
                AllRowHeaderForAWeek = RowHeaders,
                AllMondayTimeLogs = MondayLogs,
                AllTuesdayTimeLogs = TuesdayLogs,
                AllWednesdayTimeLogs = WednesdayLogs,
                AllThursdayTimeLogs = ThursdayLogs,
                AllFridayTimeLogs = FridayLogs,
                AllSaturdayTimeLogs = SaturdayLogs,
                AllSundayTimeLogs = SundayLogs,
                StartDate = StartDate,
                EndDate = EndDate,
                AllTimesheetLogsBreakdown = await timesheetService.GetAllTimesheetLogBreakdown<TimesheetLogBreakdown>(LoggedInEmployee.Id, StartDate, EndDate),
                AllTimesheetCategories = await timesheetService.GetAllTimesheetCategories<TimesheetTaskCategory>(LoggedInEmployee.Id, StartDate, EndDate),
                AllChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 1),
                AllNonChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 2),
                AllNonClientTasksForEmployee = AllNonClientTasksForWeek,
                AllNonClientActivitiesForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 3),
                WeeklyTotalString = AllWeeklyTotalStrings,
                DailyTotalString = await timesheetService.GetAllDailyTotals(LoggedInEmployee.Id, StartDate, EndDate),
                ApprovalDetails = await timesheetService.GetApprovalDetailsForTimesheets(LoggedInEmployee.Id, StartDate)
            };
            if (results.ApprovalDetails != null && results.ApprovalDetails.ApprovedByEmpId != null)
            {
                Employee ApprovedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.ApprovedByEmpId.GetValueOrDefault()).Result;
                ViewBag.ApprovedByEmployee = ApprovedByEmp.FirstName + ' ' + ApprovedByEmp.Surname;
            }

            if (results.ApprovalDetails != null && results.ApprovalDetails.UnlockedByEmpId != null)
            {
                Employee UnlockedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.UnlockedByEmpId).Result;
                ViewBag.UnlockedByEmployee = UnlockedByEmp.FirstName + ' ' + UnlockedByEmp.Surname;
            }
            if (results.ApprovalDetails != null && results.ApprovalDetails.SubmissionDateTime != null)
            {
                ViewBag.TimesheetSubmitted = true;
            }

            return View(results);
        }

        public async Task<IActionResult> MyTimesheetTandPAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("rodnorri");
            ViewBag.CurrentEmployee = LoggedInEmployee.FirstName + ' ' + LoggedInEmployee.Surname;
            ViewBag.CurrentEmployeeId = LoggedInEmployee.Id;
            DateTime StartDate = TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Monday);
            DateTime EndDate = TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Sunday);
            ViewBag.DateSelected = StartDate;
            ViewBag.DateSelectedString = StartDate.ToString("yyyy-MM-dd");
            ViewBag.DateStartingLongString = StartDate.ToString("dd MMM yyyy");
            if (TPCommonGeneralUtils.GetDateFromGivenWeek(DateTime.Today, DayOfWeek.Monday) == StartDate)
            {
                ViewBag.IsCurrentWeek = true;
            }
            else
            {
                ViewBag.IsCurrentWeek = false;
            }

            Hashtable RowHeaders = await timesheetService.GetAllTandPRowHeadersForAWeek(LoggedInEmployee.Id, StartDate, EndDate);
            //List<Data.Org> RowHeaders = await timesheetService.GetAllRowHeaderOrgCategoriesForAWeek<Org>(LoggedInEmployee.Id, StartDate, EndDate);
            List<Data.Timesheet> MondayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> TuesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> WednesdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> ThursdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> FridayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SaturdayLogs = new List<Data.Timesheet>();
            List<Data.Timesheet> SundayLogs = new List<Data.Timesheet>();
            List<String> AllDailyTotalStrings = new List<String>();
            List<String> AllWeeklyTotalStrings = new List<String>();

            List<Data.EndClient> EndClients = new List<EndClient>();
            List<Data.EndClientData> GroupeCategories = new List<EndClientData>();
            List<Data.EndClientData> GroupeBrands = new List<EndClientData>();
            List<Data.EndClientData> GroupeCampaigns = new List<EndClientData>();


            foreach (Dictionary<int, Org> key in RowHeaders.Keys)
            {
                Dictionary<int, Org> thisDict = (Dictionary<int, Org>)key;
                Org currentOrg = thisDict.ElementAt(0).Value;

                var AllFieldStrings = RowHeaders[thisDict].ToString().Split("$");
                int EndClientID = 0;
                int BrandID = 0;
                int CategoryID = 0;
                int CampaignID = 0;
                if (AllFieldStrings.Count() > 0)
                {
                    if (AllFieldStrings[0] != "") { EndClientID = Int32.Parse(AllFieldStrings[0]); }
                    if (AllFieldStrings[1] != "") { BrandID = Int32.Parse(AllFieldStrings[1]); }
                    if (AllFieldStrings[2] != "") { CategoryID = Int32.Parse(AllFieldStrings[2]); }
                    if (AllFieldStrings[3] != "") { CampaignID = Int32.Parse(AllFieldStrings[3]); }
                }


                MondayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday), EndClientID, BrandID, CategoryID, CampaignID));
                TuesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday), EndClientID, BrandID, CategoryID, CampaignID));
                WednesdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday), EndClientID, BrandID, CategoryID, CampaignID));
                ThursdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday), EndClientID, BrandID, CategoryID, CampaignID));
                FridayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday), EndClientID, BrandID, CategoryID, CampaignID));
                SaturdayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday), EndClientID, BrandID, CategoryID, CampaignID));
                SundayLogs.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(currentOrg.Id, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday), EndClientID, BrandID, CategoryID, CampaignID));
                AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForTandPOrg(LoggedInEmployee.Id, StartDate, EndDate, currentOrg.Id, EndClientID, BrandID, CategoryID, CampaignID));

                var GroupeDataString = RowHeaders[key].ToString();
                var AllGroupeFields = GroupeDataString.Split("$");
                var ClientIDString = AllGroupeFields[0];
                if (ClientIDString != "")
                {
                    EndClients.Add(await endClientService.GetEndClientByID<EndClient>(Int32.Parse(ClientIDString)));
                }
                else
                {
                    EndClients.Add(null);
                }

                var BrandIDString = AllGroupeFields[1];
                if (BrandIDString != "")
                {
                    GroupeBrands.Add(await endClientService.GetEndClientDataByID<EndClient>(Int32.Parse(BrandIDString)));
                }
                else
                {
                    GroupeBrands.Add(null);
                }

                var CategoryIDString = AllGroupeFields[2];
                if (CategoryIDString != "")
                {
                    GroupeCategories.Add(await endClientService.GetEndClientDataByID<EndClient>(Int32.Parse(CategoryIDString)));
                }
                else
                {
                    GroupeCategories.Add(null);
                }

                var CampaignIDString = AllGroupeFields[3];
                if (CampaignIDString != "")
                {
                    GroupeCampaigns.Add(await endClientService.GetEndClientDataByID<EndClient>(Int32.Parse(CampaignIDString)));
                }
                else
                {
                    GroupeCampaigns.Add(null);
                }

            }

            List<Data.Timesheet> AllNonClientTasksForWeek = new List<Timesheet>();

            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Monday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Tuesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Wednesday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Thursday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Friday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Saturday)));
            AllNonClientTasksForWeek.Add(await timesheetService.GetTimesheetLogForAGivenDate<Data.Timesheet>(0, LoggedInEmployee.Id, TPCommonGeneralUtils.GetDateFromGivenWeek(StartDate, DayOfWeek.Sunday)));
            AllWeeklyTotalStrings.Add(await timesheetService.GetWeeklyTotalForTandPOrg(LoggedInEmployee.Id, StartDate, EndDate, null, null, null, null, null));

            TimesheetOverviewModel results = new TimesheetOverviewModel()
            {
                AllTandPRowHeaderForAWeek = RowHeaders,
                AllMondayTimeLogs = MondayLogs,
                AllTuesdayTimeLogs = TuesdayLogs,
                AllWednesdayTimeLogs = WednesdayLogs,
                AllThursdayTimeLogs = ThursdayLogs,
                AllFridayTimeLogs = FridayLogs,
                AllSaturdayTimeLogs = SaturdayLogs,
                AllSundayTimeLogs = SundayLogs,
                StartDate = StartDate,
                EndDate = EndDate,
                AllTimesheetLogsBreakdown = await timesheetService.GetAllTimesheetLogBreakdown<TimesheetLogBreakdown>(LoggedInEmployee.Id, StartDate, EndDate),
                AllTimesheetCategories = await timesheetService.GetAllTimesheetCategories<TimesheetTaskCategory>(LoggedInEmployee.Id, StartDate, EndDate),
                AllChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 1),
                AllNonChargeableTasksForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 2),
                AllNonClientTasksForEmployee = AllNonClientTasksForWeek,
                AllNonClientActivitiesForEmployee = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id).Result, 3),
                WeeklyTotalString = AllWeeklyTotalStrings,
                DailyTotalString = await timesheetService.GetAllDailyTotals(LoggedInEmployee.Id, StartDate, EndDate),
                ApprovalDetails = await timesheetService.GetApprovalDetailsForTimesheets(LoggedInEmployee.Id, StartDate),
                AllEndClients = EndClients,
                AllGroupeBrands = GroupeBrands,
                AllGroupeCategories = GroupeCategories,
                AllGroupeCampaigns = GroupeCampaigns,
                EndClientList = await endClientService.GetAllEndClient()

            };
            if (results.ApprovalDetails != null && results.ApprovalDetails.ApprovedByEmpId != null)
            {
                Employee ApprovedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.ApprovedByEmpId.GetValueOrDefault()).Result;
                ViewBag.ApprovedByEmployee = ApprovedByEmp.FirstName + ' ' + ApprovedByEmp.Surname;
            }

            if (results.ApprovalDetails != null && results.ApprovalDetails.UnlockedByEmpId != null)
            {
                Employee UnlockedByEmp = employeesService.IdentifyCurrentUserById(results.ApprovalDetails.UnlockedByEmpId).Result;
                ViewBag.UnlockedByEmployee = UnlockedByEmp.FirstName + ' ' + UnlockedByEmp.Surname;
            }
            if (results.ApprovalDetails != null && results.ApprovalDetails.SubmissionDateTime != null)
            {
                ViewBag.TimesheetSubmitted = true;
            }
            return View(results);
        }
        public async Task<string> GetCurrentTimesheetLog(int OrgID, int EmployeeID, DateTime LogDate, string LogTypeToReturn, bool ReturnHourString)
        {
            Timesheet currentTimesheet = await timesheetService.GetTimesheetLogForAGivenDate<Timesheet>(OrgID, EmployeeID, LogDate);
            if (LogTypeToReturn == "ClientCharge")
            {
                if (ReturnHourString == true)
                {
                    return currentTimesheet.ClientChargeInHours.ToString();
                }
                else
                {
                    return currentTimesheet.ClientChargeInMinutes.ToString();
                }
            }
            else if (LogTypeToReturn == "NonChargeableTime")
            {
                if (ReturnHourString == true)
                {
                    return currentTimesheet.NonChargeableTimeInHours.ToString();
                }
                else
                {
                    return currentTimesheet.NonChargeableTimeInMinutes.ToString();
                }
            }
            else
            {
                return "";
            }



        }

        [HttpPost]
        public async Task<IActionResult> GetAllBrandsForEndClientAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            var result = await endClientService.GetAllBrands(Int32.Parse(stringToUse));

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCategoriesForEndClientAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            var result = await endClientService.GetAllCategories(Int32.Parse(stringToUse));

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCampaignForEndClientAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            var result = await endClientService.GetAllCampaigns(Int32.Parse(stringToUse));

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetTimesheetDetailsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            var result = await timesheetService.GetTimesheetLogById(Int32.Parse(stringToUse));

            return Ok(result);
        }

        public async Task<IActionResult> TimesheetReportingAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("saucecca");
            EmployeeTeam employeeTeam = await employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id);
            EmployeeDepartment employeeDept = await employeesService.GetEmployeeDepartment(LoggedInEmployee.Id);

            if (LoggedInEmployee.AccessLevel == 1 && LoggedInEmployee.Id != 203)
            {
                return Redirect("/Page/Locked");
            }
            
            ViewBag.AllowToViewAllDepartments = false;
            ViewBag.AllowToViewAllTeamsInSelectedDept = false;

            ViewBag.CurrentTeam = employeeTeam;
            ViewBag.CurrentDept = employeeDept;

            TimesheetReportingModel results = null;

            List<EndClient> allEndClientsToShow = null;
            List<EndClientData> allBrandsToShow = null;
            List<EndClientData> allCategoriesToShow = null;
            List<EndClientData> allCampaignsToShow = null;

            if (employeeDept.Id == 10 || LoggedInEmployee.AccessLevel == 4)
            {
                allEndClientsToShow = await endClientService.GetAllEndClientLoggedInTimesheet();
                allBrandsToShow = await endClientService.GetAllBrandsLoggedInTimesheet();
                allCategoriesToShow = await endClientService.GetAllCategoriesLoggedInTimesheet();
                allCampaignsToShow = await endClientService.GetAllCampaignsLoggedInTimesheet();
            }

            if (LoggedInEmployee.AccessLevel == 2 || LoggedInEmployee.JobTitle.Equals("Assistant Team Manager") == true)
            {
                var allEmployees = await employeesService.GetAllEmployeesInTeam<Employee>(employeeTeam.Id);
                //For all TMs in T&P dept, show all employees of T&P dept
                if (employeeDept.Id == 10)
                {
                    allEmployees = await employeesService.GetAllEmployeesInDepartment<Employee>(employeeDept.Id);
                }
                results = new TimesheetReportingModel()
                {
                    AllEmployees = allEmployees,
                    AllDepartments = null,
                    AllTeams = null,
                    AllOrgGroupsIdAndName = await timesheetService.GetAllOrgGroupsIdAndName(),
                    AllOrgsIdAndName = await timesheetService.GetAllOrgsIdAndName(),
                    AllEndClients = allEndClientsToShow,
                    AllBrands = allBrandsToShow,
                    AllCategories = allCategoriesToShow,
                    AllCampaigns = allCampaignsToShow,
                    AllClientChargeableActivities = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 1),
                    AllClientNonChargeableActivities = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 2),
                    AllNonClientActivities = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 3)
                };
            }
            else if (LoggedInEmployee.AccessLevel == 3)
            {
                ViewBag.AllowToViewAllTeamsInSelectedDept = true;

                results = new TimesheetReportingModel()
                {
                    AllEmployees = await employeesService.GetAllEmployeesInDepartment<Employee>(employeeDept.Id),
                    AllDepartments = null,
                    AllTeams = await employeesService.GetAllTeams<EmployeeTeam>(employeeDept.Id, showTeamsWithNoEmployees: false),
                    AllOrgGroupsIdAndName = await timesheetService.GetAllOrgGroupsIdAndName(),
                    AllOrgsIdAndName = await timesheetService.GetAllOrgsIdAndName(),
                    AllEndClients = allEndClientsToShow,
                    AllBrands = allBrandsToShow,
                    AllCategories = allCategoriesToShow,
                    AllCampaigns = allCampaignsToShow,
                    AllClientChargeableActivities = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 1),
                    AllClientNonChargeableActivities = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 2),
                    AllNonClientActivities = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 3)
                };
            }
            else if (LoggedInEmployee.AccessLevel == 4 || LoggedInEmployee.Id == 203)
            {
                ViewBag.AllowToViewAllDepartments = true;
                ViewBag.AllowToViewAllTeamsInSelectedDept = true;

                results = new TimesheetReportingModel()
                {
                    AllEmployees = await employeesService.GetAllEmployees<Employee>(false, false),
                    AllDepartments = await employeesService.GetAllDepartments<EmployeeDepartment>(showDeptWithNoEmployees: false),
                    AllTeams = await employeesService.GetAllTeams<EmployeeTeam>(showTeamsWithNoEmployees: false),
                    AllOrgGroupsIdAndName = await timesheetService.GetAllOrgGroupsIdAndName(),
                    AllOrgsIdAndName = await timesheetService.GetAllOrgsIdAndName(),
                    AllEndClients = allEndClientsToShow,
                    AllBrands = allBrandsToShow,
                    AllCategories = allCategoriesToShow,
                    AllCampaigns = allCampaignsToShow,
                    AllClientChargeableActivities = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 1),
                    AllClientNonChargeableActivities = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 2),
                    AllNonClientActivities = await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 3)
                };
            }

            return View(results);


        }

        [HttpPost]
        public async Task<IActionResult> GetAllTeamsForDepartmentsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            if (stringToUse == "")
            {
                Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
                EmployeeDepartment employeeDept = await employeesService.GetEmployeeDepartment(LoggedInEmployee.Id);

                if (LoggedInEmployee.AccessLevel == 3)
                {
                    var result = await employeesService.GetAllTeams<EmployeeTeam>(employeeDept.Id, showTeamsWithNoEmployees: false);
                    return Ok(result);
                }
                else if (LoggedInEmployee.AccessLevel == 4 || LoggedInEmployee.Id == 203)
                {
                    var result = await employeesService.GetAllTeams<EmployeeTeam>(showTeamsWithNoEmployees: false);
                    return Ok(result);
                }
                else
                {
                    return Ok(null);
                }

            }
            else
            {
                var AllTeamsForDepartments = await employeesService.GetAllTeamsForListOfDepartments<EmployeeTeam>(stringToUse);
                return Ok(AllTeamsForDepartments);
            }

        }

        [HttpPost]
        public async Task<IActionResult> GetAllEmployeesForDepartmentsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            if (stringToUse == "")
            {
                Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
                EmployeeTeam employeeTeam = await employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id);
                EmployeeDepartment employeeDept = await employeesService.GetEmployeeDepartment(LoggedInEmployee.Id);

                if (LoggedInEmployee.AccessLevel == 2)
                {
                    var result = await employeesService.GetAllEmployeesInTeam<Employee>(employeeTeam.Id);
                    return Ok(result);
                }
                else if (LoggedInEmployee.AccessLevel == 3)
                {
                    var result = await employeesService.GetAllEmployeesInDepartment<Employee>(employeeDept.Id);
                    return Ok(result);
                }
                else if (LoggedInEmployee.AccessLevel == 4 || LoggedInEmployee.Id == 203)
                {
                    var result = await employeesService.GetAllEmployees<Employee>(false, false);
                    return Ok(result);
                }
                else
                {
                    return Ok(null);
                }

            }
            else
            {
                var AllEmployeesForDepartments = await employeesService.GetAllEmployeesForListOfDepartments<Employee>(stringToUse);
                return Ok(AllEmployeesForDepartments);
            }

        }

        [HttpPost]
        public async Task<IActionResult> GetAllEmployeesForTeamsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            if (stringToUse == "")
            {
                Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
                EmployeeTeam employeeTeam = await employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id);
                EmployeeDepartment employeeDept = await employeesService.GetEmployeeDepartment(LoggedInEmployee.Id);

                if (LoggedInEmployee.AccessLevel == 2)
                {
                    var result = await employeesService.GetAllEmployeesInTeam<Employee>(employeeTeam.Id);
                    return Ok(result);
                }
                else if (LoggedInEmployee.AccessLevel == 3)
                {
                    var result = await employeesService.GetAllEmployeesInDepartment<Employee>(employeeDept.Id);
                    return Ok(result);
                }
                else if (LoggedInEmployee.AccessLevel == 4 || LoggedInEmployee.Id == 203)
                {
                    var result = await employeesService.GetAllEmployees<Employee>(false, false);
                    return Ok(result);
                }
                else
                {
                    return Ok(null);
                }

            }
            else
            {
                var AllOrgsForTeam = await employeesService.GetAllEmployeesInTeamString(stringToUse);
                return Ok(AllOrgsForTeam);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetAllOrgsForOrgGroupsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            var AllOrgsForOrgGroup = await orgService.GetAllTimesheetOrgsForOrgGroupString(stringToUse);
            return Ok(AllOrgsForOrgGroup);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllActivitiesForGivenTypesAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            ArrayList activitiesArrayList = new ArrayList();

            if (stringToUse == "")
            {
                activitiesArrayList.Add(await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 1));
                activitiesArrayList.Add(await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 2));
                activitiesArrayList.Add(await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, 3));

                return Ok(activitiesArrayList);
            }
            else
            {
                var allActivities = stringToUse.Split(",", StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < allActivities.Length; i++)
                {
                    activitiesArrayList.Add(await timesheetService.GetAllTimesheetForTeam<TimesheetTaskCategory>(null, byte.Parse(allActivities[i])));
                }
                return Ok(activitiesArrayList);
            }

        }

        [HttpPost]
        public async Task<IActionResult> GetAllBrandsForEndClientsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var AllBrandsForEndClient = await endClientService.GetAllTimesheetBrandsForEndClientIDs(stringToUse);
            return Ok(AllBrandsForEndClient);


        }

        [HttpPost]
        public async Task<IActionResult> GetAllCategoriesForEndClientsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var AllCategoriesForEndClient = await endClientService.GetAllTimesheetCategoriesForEndClientIDs(stringToUse);
            return Ok(AllCategoriesForEndClient);


        }

        [HttpPost]
        public async Task<IActionResult> GetAllCampaignsForEndClientsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var AllCampaignsForEndClient = await endClientService.GetAllTimesheetCampaignsForEndClientIDs(stringToUse);
            return Ok(AllCampaignsForEndClient);


        }

        [HttpPost]
        public async Task<IActionResult> ExportTimesheetReportAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var JsonSettings = new JsonSerializerSettings { Culture = new System.Globalization.CultureInfo("en-GB") };
            List<Models.Timesheets.TimesheetReportExportModel> AllDetails = JsonConvert.DeserializeObject<List<Models.Timesheets.TimesheetReportExportModel>>(stringToUse, JsonSettings);

 
            var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
            var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
            var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
            string tempBatchFilePath = Path.Combine("\\\\FREDCPAPPDM0004\\translateplusSystemFiles\\Temporary zip folder\\temp batch files", FileName);
            System.IO.File.WriteAllText(tempBatchFilePath, System.String.Empty);
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            System.IO.File.AppendAllText(tempBatchFilePath, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
            System.IO.File.AppendAllText(tempBatchFilePath, "<!-- translate plus process automation batch file -->" + Environment.NewLine);

            System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<translateplusBatch BatchFormatVersion=\"1.2\" OwnerEmployeeName=\"{0}\" NotifyByEmail=\"{1}\">",
                                                                       LoggedInEmployee.NetworkUserName, LoggedInEmployee.EmailAddress) + Environment.NewLine);
            System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<task Type=\"TimesheetReporting\" TaskNumber=\"1\" StartDate=\"{0}\" EndDate=\"{1}\"" +
                                                                        " DepartmentString=\"{2}\"  TeamString=\"{3}\" EmployeeString=\"{4}\"  OrgGroupString=\"{5}\"" +
                                                                        " OrgString=\"{6}\" EndClientString=\"{7}\" BrandString=\"{8}\" CategoryString=\"{9}\"" +
                                                                        " CampaignString=\"{10}\" ActivityTypeString=\"{11}\" ActivityNameString=\"{12}\" ApprovalStatusString=\"{13}\" EmployeeRequestingReport=\"{14}\"/>",
                                                                      AllDetails[0].StartDate, AllDetails[0].EndDate, AllDetails[0].DepartmentString, AllDetails[0].TeamString,
                                                                      AllDetails[0].EmployeeString, AllDetails[0].OrgGroupString, AllDetails[0].OrgString, AllDetails[0].EndClientString,
                                                                      AllDetails[0].BrandString, AllDetails[0].CategoryString, AllDetails[0].CampaignString, AllDetails[0].ActivityTypeString,
                                                                      AllDetails[0].ActivityNameString, AllDetails[0].ApprovalStatusString, LoggedInEmployee.Id)
                                                        + Environment.NewLine);
            System.IO.File.AppendAllText(tempBatchFilePath, "</translateplusBatch>" + Environment.NewLine);

            System.IO.File.Copy(tempBatchFilePath, BatchFilePath);
            System.IO.File.Delete(tempBatchFilePath);
            return Ok();
        }

    }

}
