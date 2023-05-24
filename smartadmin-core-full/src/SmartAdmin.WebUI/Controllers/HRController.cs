using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Data;
using System.IO;
using ViewModels.HR;
using Services;
using ViewModels.EmployeeOwnerships;
using ViewModels.EmployeeModels;

namespace SmartAdmin.WebUI.Controllers
{
    public class HRController : Controller
    {
        private readonly ITPEmployeesService employeesService;
        private readonly ITPPublicHoliday publicHolidayService;
        private readonly IHolidayService holidayService;
        private readonly INotificationService notificationService;
        private readonly IEmailUtilsService emailService;
        private readonly ITPTasksLogic taskService;
        private readonly ITPOwnershipsLogic ownershipService;
        private readonly ISicknessLogic sicknessService;
        private readonly ITPTrainingLogic trainingService;
        private readonly ITPHolidayAdmin holidayAdminService;

        public HRController(ITPEmployeesService service, ITPPublicHoliday service1, IHolidayService service2,
                            INotificationService service3, IEmailUtilsService service5, ITPTasksLogic service6,
                            ITPOwnershipsLogic service7, ISicknessLogic service8, ITPTrainingLogic service9, ITPHolidayAdmin service10)
        {
            employeesService = service;
            publicHolidayService = service1;
            holidayService = service2;
            notificationService = service3;
            emailService = service5;
            taskService = service6;
            ownershipService = service7;
            sicknessService = service8;
            trainingService = service9;
            holidayAdminService = service10;
        }


        public async Task<IActionResult> EmployeesAsync()
        {

            List<Employee> ListOfEmployees = await employeesService.GetAllEmployees<Employee>(false, false);
            List<string> AllEmployeeTeams = new List<string>();
            List<string> AllEmployeeDepartments = new List<string>();
            List<string> AllEmployeeOffices = new List<string>();

            for (var i = 0; i < ListOfEmployees.Count; i++)
            {
                AllEmployeeTeams.Add(employeesService.GetEmployeeTeam<EmployeeTeam>(ListOfEmployees.ElementAt(i).Id).Result.Name);
                AllEmployeeDepartments.Add(employeesService.GetEmployeeDepartment(ListOfEmployees.ElementAt(i).Id).Result.Name);
                AllEmployeeOffices.Add(employeesService.GetEmployeeOffice(ListOfEmployees.ElementAt(i).Id).Result.Name);
            }

            EmployeesOverviewModel results = new EmployeesOverviewModel()
            {
                AllEmployees = ListOfEmployees,
                AllEmployeeTeamNames = AllEmployeeTeams,
                AllEmployeeDepartmentNames = AllEmployeeDepartments,
                AllEmployeeOfficesNames = AllEmployeeOffices,
                AllDepartments = await employeesService.GetAllDepartments<EmployeeDepartment>(showDeptWithNoEmployees: false),
                AllTeams = await employeesService.GetAllTeams<EmployeeTeam>(showTeamsWithNoEmployees: false),
                AllOffices = await employeesService.GetAllOffices()
            };

            return View(results);
        }

        public IActionResult CompanyPolicies()
        {
            return View();
        }

        public IActionResult GetCompanyPolicyPDFFile(string fileName)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, @"OtherResources\", fileName);
            byte[] thisFileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(thisFileBytes, "application/pdf");
        }

        [Route("[controller]/[action]/{officeId}/{holidayYear}/{publicHolidayIdToEdit?}")]
        public async Task<IActionResult> publicholidaysAsync(int officeId, int holidayYear, int publicHolidayIdToEdit = -1)
        {
            if (holidayYear == 0) { holidayYear = DateTime.Now.Year; }
            PublicHolidayModel results = new PublicHolidayModel()
            {
                AllPublicHolidays = await publicHolidayService.GetAllBankHolidaysForOffice<BankHoliday>(officeId, holidayYear),
                AllHolidayYears = await publicHolidayService.GetAllBankHolidayYears(),
                AllOffices = await employeesService.GetAllOffices(),
                LoggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase()
            };

            if (publicHolidayIdToEdit != -1)
            {
                ViewBag.currentIndex = publicHolidayIdToEdit;
            }

            return View(results);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> AddPublicHoliday()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var allFields = stringToProcess.Split("$");
            string PublicHolidayName = allFields[0];
            ViewBag.NewBankHolidayName = PublicHolidayName;
            DateTime publicHolidayDate = DateTime.Parse(allFields[1]);
            Boolean HalfDay = Boolean.Parse(allFields[2]);
            var AllOfficesString = allFields[3];
            Employee loggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            await publicHolidayService.AddPublicHoliday<BankHoliday>(PublicHolidayName, publicHolidayDate, HalfDay, AllOfficesString, loggedInEmployee.Id);

            return Ok();
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdatePublicHoliday()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var allFields = stringToProcess.Split("$");
            string PublicHolidayName = allFields[0];
            ViewBag.NewBankHolidayName = PublicHolidayName;
            DateTime publicHolidayDate = DateTime.Parse(allFields[1]);
            Boolean HalfDay = Boolean.Parse(allFields[2]);
            var AllOfficesString = allFields[3];
            int holidayId = Int32.Parse(allFields[4]);
            Employee loggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            await publicHolidayService.UpdatePublicHoliday<BankHoliday>(holidayId, PublicHolidayName, publicHolidayDate, HalfDay, AllOfficesString, loggedInEmployee.Id);

            return Ok();
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> DeletePublicHoliday()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            int holidayIdToDelete = Int32.Parse(content.Result);

            Employee loggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            await publicHolidayService.DeletePublicHoliday<BankHoliday>(holidayIdToDelete, loggedInEmployee.Id);

            return Ok();
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> HolidayRequestAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            Data.EmployeeHoliday thisHoliday = await holidayService.GetEmployeeHolidayDetailsForYear(LoggedInEmployee.Id, DateTime.Today.Year);
            DateTime NextWorkingDay = holidayService.GetNextWorkingDayForEmployee(LoggedInEmployee.Id);

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            bool lastDayAmPMRadioVisible = false;
            short startDateFullOrAMOrPM = 0;
            short endDateFullOrAMOrPM = 0;
            DateTime startDate = NextWorkingDay;
            DateTime endDate = NextWorkingDay;

            if (stringToProcess != "")
            {
                var allDataRecieved = stringToProcess.Split("$");
                lastDayAmPMRadioVisible = Boolean.Parse(allDataRecieved[0]);
                startDateFullOrAMOrPM = short.Parse(allDataRecieved[1]);
                endDateFullOrAMOrPM = short.Parse(allDataRecieved[2]);
                startDate = DateTime.Parse(allDataRecieved[3]);
                endDate = DateTime.Parse(allDataRecieved[4]);
            }



            HolidaysViewModel results = new HolidaysViewModel()
            {
                NextAvailableHolidayDate = NextWorkingDay.ToString("dd/MM/yyyy"),
                HolidaysRemaining = thisHoliday.HolidaysRemaing,
                TotalAnnualHolidays = thisHoliday.TotalAnnualHolidays,
                Year = thisHoliday.Year,
                TotalHolidaysForCurrentRequest = holidayService.GetTotalNumberOfDaysForHolidayRequest(LoggedInEmployee.Id, startDate, endDate, startDateFullOrAMOrPM, endDateFullOrAMOrPM, lastDayAmPMRadioVisible),
                NextWorkingDayAfterHolidayString = holidayService.GetNextWorkingDayAfterHolidayString(LoggedInEmployee.Id, lastDayAmPMRadioVisible, startDateFullOrAMOrPM, endDateFullOrAMOrPM, startDate, endDate)
                //LoggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase()
            };
            return Ok(results);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public IActionResult BankHolidayAlert()
        {
            return Ok();
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> IsDateBankHolidayAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var CurrentDate = DateTime.Parse(stringToProcess);

            var results = holidayService.IsFullDayBankHolidayForEmployee(LoggedInEmployee.Id, CurrentDate);

            if (results.Result == false)
            {
                return Ok(false);
            }
            else
            {
                var dateToReturn = holidayService.GetNextWorkingDayForEmployee(LoggedInEmployee.Id);
                return Ok(dateToReturn.ToString("dd/MM/yyyy"));
            }
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> IsDateHalfDayBankHolidayAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var CurrentDate = DateTime.Parse(stringToProcess);

            var results = holidayService.IsHalfDayBankHolidayForEmployee(LoggedInEmployee.Id, CurrentDate);

            return Ok(results.Result);
            //return Ok(true);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> CreateHolidayRequestAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("catwhyte");
            //when the logged in employee is same as the employee whose holiday is being logged in
            Employee thisEmployee = LoggedInEmployee;

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var allDataRecieved = stringToProcess.Split("$");

            bool lastDayAmPMRadioVisible = Boolean.Parse(allDataRecieved[0]); ;
            byte startDateFullOrAMOrPM = 0; //as there is no value 4 in the system
            if (allDataRecieved[1] != "-1") { startDateFullOrAMOrPM = byte.Parse(allDataRecieved[1]); }
            byte endDateFullOrAMOrPM = 0;
            if (allDataRecieved[2] != "-1") { endDateFullOrAMOrPM = byte.Parse(allDataRecieved[2]); }
            DateTime startDate = DateTime.Parse(allDataRecieved[3]);
            DateTime endDate = DateTime.Parse(allDataRecieved[4]);
            decimal totalDaysForRequest = decimal.Parse(allDataRecieved[5]);
            if (allDataRecieved[6] != "")
            {
                thisEmployee = await employeesService.IdentifyCurrentUserById(Int32.Parse(allDataRecieved[6]));
            }

            bool isAValidRequest = true;
            string errorMessageString = "";

            if (double.Parse(totalDaysForRequest.ToString()) < 0.5 || totalDaysForRequest == 0)
            {
                errorMessageString = "Please double check that the dates and \"full day/morning/afternoon\" options are selected correctly.";
                return Ok("Info$" + errorMessageString);
            }

            if (lastDayAmPMRadioVisible == false)
            {
                endDateFullOrAMOrPM = startDateFullOrAMOrPM;
            }

            EmployeeHoliday holidayDetailsForStartDateYear = holidayService.GetEmployeeHolidayDetailsForYear(thisEmployee.Id, startDate.Year).Result;
            EmployeeHoliday holidayDetailsForEndDateYear = holidayService.GetEmployeeHolidayDetailsForYear(thisEmployee.Id, endDate.Year).Result;

            double holidaysAccountedForStartDateYear = 0;
            double holidaysAccountedForEndDateYear = 0;

            bool dateRangeValid = true;

            DateTime counterDate = startDate;

            //here we check separtely if first and last day is booked for holiday
            //the reason for this is that we need to check for first and half day to be off for AM or PM

            //first checking first day
            if (holidayService.IsDateAlreadyBookedForHoliday(thisEmployee.Id, counterDate, startDateFullOrAMOrPM).Result == true)
            {
                dateRangeValid = false;
            }
            //dont need to check any further if the first date itslf is already present in the holiday request table
            if (dateRangeValid == true)
            {
                while (counterDate <= endDate)
                {
                    if (holidayService.IsDateAlreadyBookedForHoliday(thisEmployee.Id, counterDate).Result == true)
                    {
                        dateRangeValid = false;
                        break;
                    }
                    counterDate = counterDate.AddDays(1);
                }

                //dont need to check any further if the DateRangeValid is already set to false
                //checking if last day is already present in the holiday request table
                if (dateRangeValid == true)
                {
                    if (holidayService.IsDateAlreadyBookedForHoliday(thisEmployee.Id, endDate, endDateFullOrAMOrPM).Result == true)
                    {
                        dateRangeValid = false;
                    }
                }

                if (dateRangeValid == true)
                {
                    //if the dates selected for holiday is from one year to another, 
                    //then check for each year if the number of holiday requested is less than or
                    //equal to holidays remainig that year
                    bool requestIsForDifferentYear = false;
                    DateTime lastDayOfStartDateYear = new DateTime(startDate.Year, 12, 31);
                    DateTime firstDayOfEndDateYear = new DateTime(endDate.Year, 1, 1);
                    if (startDate.Year != endDate.Year)
                    {
                        requestIsForDifferentYear = true;
                    }

                    if (requestIsForDifferentYear == true)
                    {
                        holidaysAccountedForStartDateYear = holidayService.GetTotalNumberOfDaysForHolidayRequest(thisEmployee.Id, startDate, lastDayOfStartDateYear,
                                                                                                                 startDateFullOrAMOrPM, 0, lastDayAmPMRadioVisible);

                        holidaysAccountedForEndDateYear = holidayService.GetTotalNumberOfDaysForHolidayRequest(thisEmployee.Id, firstDayOfEndDateYear, endDate, 0,
                                                                                                               endDateFullOrAMOrPM, lastDayAmPMRadioVisible);

                        if (double.Parse(holidayDetailsForStartDateYear.HolidaysRemaing.ToString()) < holidaysAccountedForStartDateYear)
                        {
                            isAValidRequest = false;
                            errorMessageString = String.Format("Holiday request could not be submitted successfully as the requested number of holidays was {0} which is more than remaining holidays i.e. {1} for the year {2}",
                                                         holidaysAccountedForStartDateYear.ToString("G29"), holidayDetailsForStartDateYear.HolidaysRemaing.ToString(), startDate.Year.ToString());
                        }

                        if (double.Parse(holidayDetailsForEndDateYear.HolidaysRemaing.ToString()) < holidaysAccountedForEndDateYear)
                        {
                            isAValidRequest = false;
                            if (errorMessageString == "")
                            {
                                errorMessageString = String.Format("Holiday request could not be submitted as the requested number of holidays was {0} which is more than remaining holidays i.e. {1} for the year {2}",
                                                                    holidaysAccountedForEndDateYear.ToString("G29"), holidayDetailsForEndDateYear.HolidaysRemaing.ToString(),
                                                                    endDate.Year.ToString());
                            }
                            else
                            {
                                errorMessageString += String.Format("<br/><br/>Also for the year {2}, requested number of holidays was {0} which is more than remaining holidays i.e. {1}.",
                                                                    holidaysAccountedForEndDateYear.ToString("G29"), holidayDetailsForEndDateYear.HolidaysRemaing.ToString(),
                                                                    endDate.Year.ToString());
                            }
                        }
                    }

                    else
                    {
                        //else directly check the number of holidays remaining to the total number of days for the request
                        if (double.Parse(holidayDetailsForStartDateYear.HolidaysRemaing.ToString()) < double.Parse(totalDaysForRequest.ToString()))
                        {
                            isAValidRequest = false;
                            errorMessageString = String.Format("Holiday request could not be submitted as the requested number of holidays was {0} which is more than remaining holidays i.e. {1} for the year {2}",
                                                         totalDaysForRequest.ToString("G29"),
                                                         holidayDetailsForStartDateYear.HolidaysRemaing.ToString(),
                                                         startDate.Year);
                        }
                    }

                    if (isAValidRequest == false)
                    {
                        //return the message that the submitted request is invalid as the number of days exceed  the remiaining holidays count
                        return Ok("Error$" + errorMessageString);
                    }
                    else
                    {
                        //if all checks are passed, then go ahead to create a holiday request

                        string halfOrFullStartDay = "";
                        string halfOrFullEndDay = "";

                        if (startDateFullOrAMOrPM == 1)
                        {
                            halfOrFullStartDay = " (Morning)";
                        }
                        else if (startDateFullOrAMOrPM == 2)
                        {
                            halfOrFullStartDay = " (Afternoon)";
                        }

                        if (endDateFullOrAMOrPM == 1)
                        {
                            halfOrFullEndDay = " (Morning)";
                        }


                        string holidayStartDateString = startDate.ToString("dddd d MMMM yyyy") + halfOrFullStartDay;
                        string holidayEndDateString = endDate.ToString("dddd d MMMM yyyy") + halfOrFullEndDay;
                        string partOfEmailBody = "";
                        string partOfSubjectLine = "";
                        string employeesToBeCCed = "";

                        if (thisEmployee.Id != LoggedInEmployee.Id)
                        {

                            partOfEmailBody = String.Format("has requested a holiday for <a href=\"http://myplus/Employee.aspx?EmployeeID={0}\">{1}</a> from",
                                                        thisEmployee.Id, thisEmployee.FirstName + ' ' + thisEmployee.Surname);

                            partOfSubjectLine = "has requested a holiday for " + thisEmployee.FirstName + ' ' + thisEmployee.Surname + " from";
                            employeesToBeCCed = thisEmployee.EmailAddress + "," + LoggedInEmployee.EmailAddress;
                        }
                        else
                        {
                            partOfEmailBody = "has requested a holiday from";
                            partOfSubjectLine = "has requested a holiday from";
                            employeesToBeCCed = thisEmployee.EmailAddress;
                        }

                        EmployeeHolidayRequest newRequest = null;

                        if (requestIsForDifferentYear == true)
                        {
                            newRequest = holidayService.AddHolidayRequest(thisEmployee.Id, startDate, startDateFullOrAMOrPM,
                                                                                                 endDate, endDateFullOrAMOrPM, totalDaysForRequest,
                                                                                                 LoggedInEmployee.Id, holidaysAccountedForStartDateYear,
                                                                                                 holidaysAccountedForEndDateYear).Result;
                        }
                        else
                        {
                            newRequest = holidayService.AddHolidayRequest(thisEmployee.Id, startDate, startDateFullOrAMOrPM,
                                                                     endDate, endDateFullOrAMOrPM, totalDaysForRequest, LoggedInEmployee.Id,
                                                                     double.Parse(totalDaysForRequest.ToString()),
                                                                     double.Parse(totalDaysForRequest.ToString())).Result;

                        }
                        if (newRequest != null)
                        {


                            //send notification
                            string recipientsEmailAddress = "";
                            string firstNamesString = "";
                            string subjectLine = "";
                            string emailBody = "";
                            //if (thisEmployee.Manager != null)
                            //{
                            Employee manager = employeesService.GetManagerOfEmployee(thisEmployee.Id).Result;
                            if (manager.Id != thisEmployee.Id)
                            {
                                recipientsEmailAddress = manager.EmailAddress;
                                firstNamesString = manager.FirstName;
                            }
                            //}


                            //if manager not found, then get recipients for the notification from the database
                            //if (recipientsEmailAddress == "")
                            //{
                            List<Employee> recipientsList = notificationService.GetAllNotificationRecipients(thisEmployee.Id, 1).Result;
                            for (var i = 0; i < recipientsList.Count; i++)
                            {
                                if (recipientsEmailAddress == "")
                                {
                                    recipientsEmailAddress = recipientsList.ElementAt(i).EmailAddress;
                                    firstNamesString = recipientsList.ElementAt(i).FirstName;
                                }
                                else
                                {
                                    recipientsEmailAddress += ", " + recipientsList.ElementAt(i).EmailAddress;
                                    firstNamesString += '/' + recipientsList.ElementAt(i).FirstName;
                                }
                            }

                            //}

                            subjectLine = String.Format("{0} " + partOfSubjectLine + " {1} to {2}", LoggedInEmployee.FirstName + ' ' + LoggedInEmployee.Surname, holidayStartDateString, holidayEndDateString);
                            emailBody = String.Format("<p>Dear {0}, <br/><br/>" +
                                                      "<a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{1}\">{2}</a> " + partOfEmailBody + " <b>{3}</b> to <b>{4}</b>. <br/><br/>" +
                                                      "You will need to <b>approve</b> or <b>decline</b> this request via their <a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{1}\">profile page</a>. <br/><br/>",
                                                        firstNamesString, LoggedInEmployee.Id,
                                                        LoggedInEmployee.FirstName + ' ' + LoggedInEmployee.Surname, holidayStartDateString,
                                                        holidayEndDateString, newRequest.Id);

                            //send notification
                            emailService.SendMail("my plus <myplus@translateplus.com>", recipientsEmailAddress,
                                                  subjectLine, emailBody,
                                                  CCRecipients: employeesToBeCCed);

                            //emailService.SendMail("my plus <myplus@translateplus.com>", "KavitaJ@translateplus.com",
                            //                     subjectLine, emailBody,
                            //                     CCRecipients: employeesToBeCCed);



                            return Ok(String.Format("Success${0} to {1}.", holidayStartDateString, holidayEndDateString));
                        }
                        else
                        {
                            return Ok("Error$Error occured while trying to submit the holiday request");
                        }


                    }
                }
                else
                {
                    string MessagsString = "Error$Cannot submit this request as one or more dates in this holiday request are already marked for holiday. " +
                                        "Please check your existing holidays and delete/update your exsting holiday request(s).";
                    return Ok(MessagsString);
                }
            }



            else
            {
                //if any date in date range are in holiday database already
                string MessagsString = "Error$Cannot submit this request as one or more dates in this holiday request are already marked for holiday. " +
                                        "Please check your existing holidays and delete/update your exsting holiday request(s).";

                return Ok(MessagsString);
            }

        }

        [Route("[controller]/[action]/{employeeToViewId}/{holidayYear?}/{sicknessyear?}/{trainingYear?}")]
        public async Task<IActionResult> EmployeeProfileAsync(short employeeToViewId, int holidayYear = 0, int sicknessYear = 0, int trainingYear = 0)
        {
            ViewModels.EmployeeModels.EmployeeViewModel LoggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            
            Employee employeeToView = await employeesService.IdentifyCurrentUserById(employeeToViewId);

            int holidayYearSelected = DateTime.Now.Year;
            if (holidayYear > 0)
            {
                holidayYearSelected = holidayYear;
            }
            ViewBag.holidayYearFromURL = holidayYear;

            int sicknessYearSelected = DateTime.Now.Year;
            if (sicknessYear > 0)
            {
                sicknessYearSelected = sicknessYear;
            }
            ViewBag.sicknessYearFromURL = sicknessYear;

            int trainingYearSelected = DateTime.Now.Year;
            if (trainingYear > 0)
            {
                trainingYearSelected = trainingYear;
            }
            ViewBag.trainingYearFromURL = trainingYear;

            bool loggedInEmployeeCanApproveHolidays = false;
            bool loggedInEmployeeCanViewSickness = false;

            Employee manager = await employeesService.GetManagerOfEmployee(employeeToViewId);

            if (manager != null)
            {
                if (manager.Id != LoggedInEmployee.Id)
                {
                    loggedInEmployeeCanApproveHolidays = await holidayService.CheckIfEmployeeHasAccessToApproveHolidays(LoggedInEmployee.Id, employeeToViewId);
                    loggedInEmployeeCanViewSickness = await holidayService.CheckIfEmployeeHasAccessToAccessSickness(LoggedInEmployee.Id, employeeToViewId);
                }
                else
                {
                    loggedInEmployeeCanApproveHolidays = true;
                    loggedInEmployeeCanViewSickness = true;
                }
            }
            else
            {
                loggedInEmployeeCanApproveHolidays = await holidayService.CheckIfEmployeeHasAccessToApproveHolidays(LoggedInEmployee.Id, employeeToViewId);
                loggedInEmployeeCanViewSickness = await holidayService.CheckIfEmployeeHasAccessToAccessSickness(LoggedInEmployee.Id, employeeToViewId);
            }

            var allTasks = await taskService.GetAllTasksForDataObjectID(employeeToViewId, 5);
            List<Employee> allTasksCreatedBy = new List<Employee>();
            List<String> allTasksDetails = new List<String>();
            List<String> allDataObjectNameString = new List<String>();

            ViewBag.OnwershipCount = ownershipService.GetAllOwnershipsForDataObjectIDCount(employeeToViewId, 5, DateTime.Today, "");
            

            for (var i = 0; i < allTasks.Count; i++)
            {
                allTasksCreatedBy.Add(await employeesService.IdentifyCurrentUserById(allTasks[i].CreatedByEmployeeId));

                String ActionString = "????";
                switch (allTasks[i].TaskTypeId)
                {
                    case 1: // task type call
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Called";
                        }
                        else
                        {
                            ActionString = "Call";
                        }
                        break;

                    case 8: // task type chase invoice
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Chased invoice";
                        }
                        else
                        {
                            ActionString = "Chase invoice";
                        }
                        break;

                    case 5: // task type chase a quote
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Chased quote for";
                        }
                        else
                        {
                            ActionString = "Chase quote for";
                        }
                        break;

                    case 2: // task type e-mail
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "E-mailed";
                        }
                        else
                        {
                            ActionString = "E-mail";
                        }
                        break;

                    case 3: // task type meeting
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Met";
                        }
                        else
                        {
                            ActionString = "Meet";
                        }
                        break;

                    case 6: // task type send proposal to
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Sent proposal to";
                        }
                        else
                        {
                            ActionString = "Send proposal to";
                        }
                        break;

                    case 4: // task type send quote to
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Sent quote to";
                        }
                        else
                        {
                            ActionString = "Send quote to";
                        }
                        break;

                    case 7: // task type general reminder
                        ActionString = "General reminder:";
                        break;

                    case 9: // task type compliment
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Handled a compliment relating to";
                        }
                        else
                        {
                            ActionString = "Handle a compliment relating to";
                        }
                        break;

                    case 10: // task type complaint
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Handled a complaint relating to";
                        }
                        else
                        {
                            ActionString = "Handle a complaint relating to";
                        }
                        break;

                    case 11: // task type GoldClientMeeting
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Met gold client";
                        }
                        else
                        {
                            ActionString = "Meet gold client";
                        }
                        break;

                    case 13: // task type Survey
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Sent survey to";
                        }
                        else
                        {
                            ActionString = "Send survey to";
                        }
                        break;

                    case 14: // task type MarketingCampaign
                        if (allTasks[i].CompletedDateTime != null)
                        {
                            ActionString = "Sent marketing campaign to";
                        }
                        else
                        {
                            ActionString = "Send marketing campaign to";
                        }
                        break;

                }

                allTasksDetails.Add(ActionString);

            }



            EmployeeProfileModel results = new EmployeeProfileModel()
            {
                EmployeeToBeViewed = employeeToView,
                Team = await employeesService.GetEmployeeTeam<EmployeeTeam>(employeeToViewId),
                Department = await employeesService.GetEmployeeDepartment(employeeToViewId),
                Office = await employeesService.GetEmployeeOffice(employeeToViewId),
                loggedInEmployee = LoggedInEmployee,
                AllHolidayYearDetails = await holidayService.GetAllHolidayDetailsForEmployee(employeeToViewId),
                AllHolidayRequests = await holidayService.GetAllHolidayRequestsForEmployeeForYear(employeeToViewId, holidayYearSelected),
                holidayYearSelected = holidayYearSelected,
                HolidayDetailsForThisYear = await holidayService.GetHolidayDetailsForYear(employeeToViewId, holidayYearSelected),
                IsloggedInEmployeeAllowedToApproveHoliday = loggedInEmployeeCanApproveHolidays,
                IsloggedInEmployeeAllowedToViewSickness = loggedInEmployeeCanViewSickness,
                AllEmployeeTasks = allTasks,
                //AllOwnerships = await ownershipService.GetAllOwnershipsForDataObjectID(employeeToViewId, 5, DateTime.Now),
                AllTasksCreatedByEmployee = allTasksCreatedBy,
                AllTasksActionString = allTasksDetails,
                //AllOwnershipOrgs = await ownershipService.GetAllOwnershipOrgsForDataObjectID(employeeToViewId, 5, DateTime.Now),
                //AllOwnershipTypes = await ownershipService.GetAllOwnershipTypesForDataObjectID(employeeToViewId, 5, DateTime.Now),
                AllSicknessYears = await sicknessService.GetAllSicknessYears(employeeToViewId),
                AllEmployeeSickness = await sicknessService.GetAllSicknessForEmployee(employeeToViewId, sicknessYearSelected),
                SicknessYearSelected = sicknessYearSelected,
                TotalSicknessDaysForYear = await sicknessService.GetTotalNumberOfSickDaysInYearForEmployee(employeeToViewId, sicknessYearSelected),
                AllTrainingYears = await trainingService.GetAllYearsOfTrainingForEmployee(employeeToViewId),
                AllEmployeeTraining = await trainingService.GetAllTrainingSessionsForEmployeeForGivenYear(employeeToViewId, trainingYearSelected),
                AllTrainingCourseNames = await trainingService.GetAllTrainingCourseNamesForEmployeePage(employeeToViewId, trainingYearSelected),
                TrainingYearSelected = trainingYearSelected,
                AllTrainingTrainerNames = await trainingService.GetAllTrainersForEmployeePage(employeeToViewId, trainingYearSelected),
                AllTrainingAttendence = await trainingService.GetAllTrainingAttendenceForEmployeePage(employeeToViewId, trainingYearSelected)
            };

            return View(results);
        }

        [HttpPost]
        public async Task<IActionResult> GetHolidayRequestDetailsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var holidayRequestID = Int32.Parse(stringToProcess);

            var results = await holidayService.GetHolidayRequestDetails(holidayRequestID);

            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> GetSicknessDetailsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var sicknessID = Int32.Parse(stringToProcess);

            var results = await sicknessService.GetSicknessDetails(sicknessID);

            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeeTaskInfoAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var taskID = Int32.Parse(stringToProcess);

            var results = await taskService.GetTaskDetails(taskID);

            return Ok(results);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteHolidayRequestAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("catwhyte");
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var holidayRequestId = Int32.Parse(stringToProcess);

            var results = await holidayService.DeleteHolidayRequest(holidayRequestId, LoggedInEmployee.Id);

            if (results != null)
            {


                var thisEmployee = await employeesService.IdentifyCurrentUserById(results.EmployeeId);

                Employee manager = employeesService.GetManagerOfEmployee(thisEmployee.Id).Result;

                Employee HrManager = employeesService.GetCurrentManagerOfTeam(16).Result;

                //send notification
                string recipientsEmailAddress = "";
                string firstNamesString = "";

                if (manager != null)
                {
                    recipientsEmailAddress = manager.EmailAddress;
                    firstNamesString = manager.FirstName;
                }


                //if manager not found, then get recipients for the notification from the database
                List<Employee> recipientsList = notificationService.GetAllNotificationRecipients(thisEmployee.Id, 1).Result;
                for (var i = 0; i < recipientsList.Count; i++)
                {
                    if (recipientsEmailAddress == "")
                    {
                        recipientsEmailAddress = recipientsList.ElementAt(i).EmailAddress;
                        firstNamesString = recipientsList.ElementAt(i).FirstName;
                    }
                    else
                    {
                        recipientsEmailAddress += ", " + recipientsList.ElementAt(i).EmailAddress;
                        firstNamesString += '/' + recipientsList.ElementAt(i).FirstName;
                    }
                }

                if (recipientsEmailAddress == "")
                {
                    recipientsEmailAddress = "HR@translateplus.com";
                    firstNamesString = "HR";
                }

                string halfOrFullStartDay = "";
                string halfOrFullEndDay = "";

                if (results.StartDateAmorPmorFullDay == 1)
                {
                    halfOrFullStartDay = " (Morning)";
                }
                else if (results.StartDateAmorPmorFullDay == 2)
                {
                    halfOrFullStartDay = " (Afternoon)";
                }

                if (results.EndDateAmorPmorFullDay == 1)
                {
                    halfOrFullEndDay = " (Morning)";
                }



                string holidayStartDateString = results.HolidayStartDateTime.ToString("dddd d MMMM yyyy") + halfOrFullStartDay;
                string holidayEndDateString = results.HolidayEndDateTime.ToString("dddd d MMMM yyyy") + halfOrFullEndDay;

                string subjectLine = String.Format("{0}'s holiday request from {1} to {2} has been deleted", thisEmployee.FirstName + ' ' + thisEmployee.Surname,
                                                   holidayStartDateString, holidayEndDateString);
                string emailBody = String.Format("<p>Dear {0}, <br/><br/>" +
                                                 "<a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{1}\">{2}</a> has deleted <a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{3}\">{4}</a>'s holiday request from <b>{5}</b> to <b>{6}</b>.",
                                                 firstNamesString, LoggedInEmployee.Id, LoggedInEmployee.FirstName + " " + LoggedInEmployee.Surname,
                                                 thisEmployee.Id, thisEmployee.FirstName + " " + thisEmployee.Surname,
                                                 holidayStartDateString, holidayEndDateString);
                string ccEmailAddresses = LoggedInEmployee.EmailAddress;


                if (thisEmployee.Id != LoggedInEmployee.Id)
                {
                    ccEmailAddresses += ", " + thisEmployee.EmailAddress;

                }

                if (results.Status == 1)
                {
                    ccEmailAddresses += ", Oana.Borhan@translateplus.com, tsveta.ilieva@translateplus.com," + HrManager.EmailAddress;
                }


                //send notification
                emailService.SendMail("my plus <myplus@translateplus.com>", recipientsEmailAddress,
                                      subjectLine, emailBody,
                                      CCRecipients: ccEmailAddresses);

            }


            return Ok(results);


        }

        [HttpPost]
        public async Task<IActionResult> DeleteSicknessAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("catwhyte");
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var sicknessId = Int32.Parse(stringToProcess);

            var results = await sicknessService.DeleteSickness(sicknessId, LoggedInEmployee.Id);
                      
            return Ok(results);

        }


        [HttpPost]
        public async Task<IActionResult> GetAllTeamHolidaysForDateRangeAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var holidayRequestID = Int32.Parse(stringToProcess);

            var results = await holidayService.GetAllTeamHolidaysInSameDateRange(holidayRequestID);

            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var results = await employeesService.GetAllEmployees<Employee>(false, false);

            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeeObjectByIdAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var employeeId = Int32.Parse(stringToProcess);

            var results = await employeesService.IdentifyCurrentUserById(employeeId);

            return Ok(results);
        }



        [HttpPost]
        public async Task<IActionResult> DeclineHolidayRequestAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("adrmetca");
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var holidayRequestId = Int32.Parse(stringToProcess);

            var results = await holidayService.DeclineHolidayRequest(holidayRequestId, LoggedInEmployee.Id);

            if (results != null)
            {

                var thisEmployee = await employeesService.IdentifyCurrentUserById(results.EmployeeId);

                Employee manager = employeesService.GetManagerOfEmployee(thisEmployee.Id).Result;

                //send notification
                string recipientsEmailAddress = thisEmployee.EmailAddress;
                string firstNamesString = thisEmployee.FirstName;

                string halfOrFullStartDay = "";
                string halfOrFullEndDay = "";

                if (results.StartDateAmorPmorFullDay == 1)
                {
                    halfOrFullStartDay = " (Morning)";
                }
                else if (results.StartDateAmorPmorFullDay == 2)
                {
                    halfOrFullStartDay = " (Afternoon)";
                }

                if (results.EndDateAmorPmorFullDay == 1)
                {
                    halfOrFullEndDay = " (Morning)";
                }


                string holidayStartDateString = results.HolidayStartDateTime.ToString("dddd d MMMM yyyy") + halfOrFullStartDay;
                string holidayEndDateString = results.HolidayEndDateTime.ToString("dddd d MMMM yyyy") + halfOrFullEndDay;

                string subjectLine = String.Format("Your holiday request from {0} to {1} has been declined", holidayStartDateString, holidayEndDateString);
                string emailBody = String.Format("<p>Dear {0}, <br/><br/>" +
                                                 "This is to notify that your holiday request from <b>{1}</b> to <b>{2}</b> has been declined by <a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{3}\">{4}</a>.",
                                                 firstNamesString, holidayStartDateString, holidayEndDateString,
                                                 LoggedInEmployee, LoggedInEmployee.FirstName + " " + LoggedInEmployee.Surname);
                string ccEmailAddresses = LoggedInEmployee.EmailAddress;

                //*******************************************************
                //uncomment after test
                //if (manager.Id != LoggedInEmployee.Id)
                //{
                //    if (manager.Id != thisEmployee.Id)
                //    {
                //        ccEmailAddresses += ", " + manager.EmailAddress;
                //    }
                //}


                //List<Employee> recipientsList = notificationService.GetAllNotificationRecipients(thisEmployee.Id, 2).Result;
                //for (var i = 0; i < recipientsList.Count; i++)
                //{
                //    if (ccEmailAddresses == "")
                //    {
                //        ccEmailAddresses = recipientsList.ElementAt(i).EmailAddress;
                //    }
                //    else
                //    {
                //        ccEmailAddresses += ", " + recipientsList.ElementAt(i).EmailAddress;
                //    }
                //}

                //ccEmailAddresses += ", Oana.Borhan@translateplus.com";
                //*******************************************************
                //uncomment after test

                //send notification
                emailService.SendMail("my plus <myplus@translateplus.com>", recipientsEmailAddress,
                                      subjectLine, emailBody,
                                      CCRecipients: ccEmailAddresses);

            }


            return Ok(results);

        }


        [HttpPost]
        public async Task<IActionResult> UpdateEmployeeTaskAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("adrmetca");
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var allFields = stringToProcess.Split("$");

            int taskId = Int32.Parse(allFields[0]);
            short employeeId = short.Parse(allFields[1]);
            DateTime dueDate = DateTime.Parse(allFields[2]);
            String notes = allFields[3];
            bool isHotTask = Boolean.Parse(allFields[4]);
            bool isCompletedTask = Boolean.Parse(allFields[5]);

            var results = await taskService.UpdateEmployeeTask(taskId, employeeId, dueDate, notes, isHotTask, isCompletedTask, LoggedInEmployee.Id);

            return Ok(results);

        }

        [HttpPost]
        public async Task<IActionResult> ApproveHolidayRequestAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("adrmetca");

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var holidayRequestId = Int32.Parse(stringToProcess);

            var results = await holidayService.ApproveHolidayRequest(holidayRequestId, LoggedInEmployee.Id);


            if (results != null)
            {

                var thisEmployee = await employeesService.IdentifyCurrentUserById(results.EmployeeId);

                Employee manager = employeesService.GetManagerOfEmployee(thisEmployee.Id).Result;

                Employee HrManager = employeesService.GetCurrentManagerOfTeam(16).Result;

                //send notification
                string recipientsEmailAddress = thisEmployee.EmailAddress;
                string firstNamesString = thisEmployee.FirstName;

                string halfOrFullStartDay = "";
                string halfOrFullEndDay = "";

                if (results.StartDateAmorPmorFullDay == 1)
                {
                    halfOrFullStartDay = " (Morning)";
                }
                else if (results.StartDateAmorPmorFullDay == 2)
                {
                    halfOrFullStartDay = " (Afternoon)";
                }

                if (results.EndDateAmorPmorFullDay == 1)
                {
                    halfOrFullEndDay = " (Morning)";
                }



                string holidayStartDateString = results.HolidayStartDateTime.ToString("dddd d MMMM yyyy") + halfOrFullStartDay;
                string holidayEndDateString = results.HolidayEndDateTime.ToString("dddd d MMMM yyyy") + halfOrFullEndDay;

                string subjectLine = String.Format("Your holiday request from {0} to {1} has been approved", holidayStartDateString, holidayEndDateString);
                string emailBody = String.Format("<p>Dear {0}, <br/><br/>" +
                                                 "Your holiday request from <b>{1}</b> to <b>{2}</b> has been approved by <a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{3}\">{4}</a>.",
                                                 firstNamesString, holidayStartDateString, holidayEndDateString,
                                                               LoggedInEmployee, LoggedInEmployee.FirstName + " " + LoggedInEmployee.Surname);
                string ccEmailAddresses = LoggedInEmployee.EmailAddress;

                //*******************************************************
                //uncomment after test
                //if (manager.Id != LoggedInEmployee.Id)
                //{
                //    if (manager.Id != thisEmployee.Id)
                //    {
                //        ccEmailAddresses += ", " + manager.EmailAddress;
                //    }
                //}


                //List<Employee> recipientsList = notificationService.GetAllNotificationRecipients(thisEmployee.Id, 2).Result;
                //for (var i = 0; i < recipientsList.Count; i++)
                //{
                //    if (ccEmailAddresses == "")
                //    {
                //        ccEmailAddresses = recipientsList.ElementAt(i).EmailAddress;
                //    }
                //    else
                //    {
                //        ccEmailAddresses += ", " + recipientsList.ElementAt(i).EmailAddress;
                //    }
                //}

                //ccEmailAddresses += ", Oana.Borhan@translateplus.com, tsveta.ilieva@translateplus.com," + HrManager.EmailAddress;

                //*******************************************************
                //uncomment after test


                //send notification
                emailService.SendMail("my plus <myplus@translateplus.com>", recipientsEmailAddress,
                                      subjectLine, emailBody,
                                      CCRecipients: ccEmailAddresses);

            }

            return Ok(results);

        }

        [HttpPost]
        public async Task<IActionResult> GetAllOwnerships([FromBody] OwnershipDataTable dataParams)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var data = await ownershipService.GetAllOwnershipsForDataObjectID(dataParams.dataObjectId, 5, DateTime.Today, dataParams.parameters.search.value, dataParams.parameters.start, dataParams.parameters.length);

            var recordCount = ownershipService.GetAllOwnershipsForDataObjectIDCount(dataParams.dataObjectId, 5, DateTime.Today, "");

            var filteredCount = recordCount;
            if (dataParams.parameters.search.value != "")
            {
                filteredCount = ownershipService.GetAllOwnershipsForDataObjectIDCount(dataParams.dataObjectId, 5, DateTime.Today, dataParams.parameters.search.value);
            }

            return Ok(new { data, recordsTotal = recordCount, recordsFiltered = filteredCount }); ;
        }

        public IActionResult GetSicknessCertificatePDFFile(string certificatePath)
        {
            var filePath = sicknessService.GetDoctorsCertificatePath(certificatePath);
            if (filePath != "")
            {
                var contentType = "";
                var fileType = certificatePath.Substring(certificatePath.IndexOf("."));
                if (fileType.ToLower() == ".jpg" || fileType.ToLower() == ".jpeg")
                {
                    contentType = "image/jpeg";
                }
                else if (fileType.ToLower() == ".png")
                {
                    contentType = "image/png";
                }
                else if (fileType.ToLower() == ".pdf")
                {
                    contentType = "application/pdf";
                }
                else if (fileType.ToLower() == ".zip")
                {
                    contentType = "application/zip";
                }
                byte[] thisFileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(thisFileBytes, contentType);
            }
            else
            {
                return null;
            }
            
        }

        [Route("[controller]/[action]/{officeId}/{year}/{employeeHolidayIdToEdit?}")]
        public async Task<IActionResult> HolidayAdminAsync(byte officeId, int year, int employeeHolidayIdToEdit = -1)
        {
            if (year == 0) { year = DateTime.Now.Year; }
            HolidayAdminModel results = new HolidayAdminModel()
            {
                AllEmployeeHolidays = await holidayAdminService.GetAllEmployeeHolidaysForOffice(officeId, year),
                AllHolidayYears = await holidayAdminService.GetAllEmployeeHolidayYears(),
                AllOffices = await employeesService.GetAllOffices(),
                LoggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase()
            };

            if (employeeHolidayIdToEdit != -1)
            {
                ViewBag.currentIndex = employeeHolidayIdToEdit;
            }
            if(results.LoggedInEmployee.AccessLevel != 5 && results.LoggedInEmployee.AccessLevel != 6)
            {
                return Redirect("/Page/Locked");
            }
            return View(results);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateEmployeeHoliday()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var allFields = stringToProcess.Split("$");
            short employeeId = Convert.ToInt16(allFields[0]);
            int totalBaseAnnualHolidays = Convert.ToInt32(allFields[1]);
            int? miscDays = Convert.ToInt32(allFields[2]);
            int? previouslyWorkedDays = Convert.ToInt32(allFields[3]);
            string holidayNotes = allFields[4];
            int year = Convert.ToInt32(allFields[5]);
            byte officeId = Convert.ToByte(allFields[6]);
            short result = await holidayAdminService.UpdateEmployeeHoliday(employeeId, totalBaseAnnualHolidays, miscDays, previouslyWorkedDays, holidayNotes, year);
            return Ok(result);
        }

        public async Task<string> UploadProfilePic(Employee model)
        {

            var file = HttpContext.Request.Form.Files;
            string result = string.Empty;
            string message = string.Empty;
            if (file.Any())
            {
                if (file[0].Name == "ProfilePic")
                {
                    var profilePic = file[0];


                    if (file[0].ContentDisposition != "")
                    {

                        string SelectedFilePath = file[0].FileName;

                        var fileType = SelectedFilePath.Substring(SelectedFilePath.IndexOf("."));
                        if (fileType.ToLower() == ".jpg" || fileType.ToLower() == ".jpeg")
                        {
                            string imgPath = SelectedFilePath.Trim();
                            string fileExtension = (Path.GetExtension(imgPath)).Replace(".", "").ToLower();
                            
                            byte[] imgBytes;
                            using (BinaryReader br = new BinaryReader(profilePic.OpenReadStream()))
                            {
                                imgBytes = br.ReadBytes((int)profilePic.OpenReadStream().Length);
                                // Convert the image in to bytes
                            }
                            
                            var base64 = Convert.ToBase64String(imgBytes);
                            var dataUri = "data:image/" + fileExtension + ";base64, ";
                            result = dataUri + base64;
                            model.ImageBase64 = result;
                            var uploadResult = await employeesService.UpdateEmployeeImage(model);
                            
                            if (uploadResult != null)
                            {
                                message = "Success$Photo successfully uploaded.$" + model.Id;
                            }
                            else {
                                message = "Error$There was an error uploading the photo!$" + model.Id;
                            }
                        }
                        else
                        {
                            message = "Error$Select correct format (.jpeg or .jpg).$" + model.Id;
                        }

                    }
                    else
                    {
                        message = "Error$There was an error uploading the photo!$" + model.Id;
                    }

                }
            }

            return message;
        }
    }
}
