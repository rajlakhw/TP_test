
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Data;

namespace SmartAdmin.WebUI.Controllers
{
    public class EnquiriesDashboardController : Controller
    {

        private readonly ITPEnquiriesService enqService;
        private readonly ITPEmployeesService employeesService;
        public EnquiriesDashboardController(ITPEnquiriesService _enqService, ITPEmployeesService _employeesService)
        {
            enqService = _enqService;
            employeesService = _employeesService;
        }


        public async Task<IActionResult> Index(string startSent = "", string endSent = "",
            string startApproved = "", string endApproved = "", string includePGD = "", string forEmployee = "")
        {
            var model = new ViewModels.Enquiries.EnquiriesViewModel();
            var allEmployees = await employeesService.GetAllEmployeesInTeam<Employee>(4);
            List<Employee> allSalesEmployees = await employeesService.GetAllEmployeesInDepartment<Employee>(1);
            allEmployees.AddRange(allSalesEmployees);
            model.EmployeesList = allEmployees;
            if (startSent == "" || startSent == null) { model.startDateSent = DateTime.UtcNow.AddMonths(-6).ToString("dd/MM/yyyy"); } else { model.startDateSent = startSent; };
            if (endSent == "" || endSent == null) { model.endDateSent = DateTime.UtcNow.ToString("dd/MM/yyyy"); } else { model.endDateSent = endSent; }
            if (startApproved == "" || startApproved == null) { model.startDateApproved = DateTime.UtcNow.AddDays(-5).ToString("dd/MM/yyyy"); } else { model.startDateApproved = startApproved; }
            if (endApproved == "" || endApproved == null) { model.endDateApproved = DateTime.UtcNow.ToString("dd/MM/yyyy"); } else { model.endDateApproved = endApproved; }

            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            ViewBag.IncludePGD = includePGD;
            ViewBag.onlyForMe = forEmployee;
            EmployeeDepartment employeeDept = await employeesService.GetEmployeeDepartment(LoggedInEmployee.Id);

            if (forEmployee == "true")
            {
                model.GoneOrRejectedEnquiriesList = await enqService.GetApprovedRejectedEnquiries(model.startDateApproved, model.endDateApproved, LoggedInEmployee.Id);
                model.SentEnquiriesList = await enqService.GetSentEnquiries(model.startDateSent, model.endDateSent, LoggedInEmployee.Id);
                if (includePGD == "true")
                {
                    model.PendingOrNotStartedEnquiriesList = await enqService.GetPendingEnquiries(LoggedInEmployee.Id);
                }
                else
                {
                    model.PendingOrNotStartedEnquiriesList = await enqService.GetPendingEnquiriesNotPGD(LoggedInEmployee.Id);
                }
            }
            else
            {
                if (includePGD == "true")
                {
                    model.PendingOrNotStartedEnquiriesList = await enqService.GetPendingEnquiries();
                }
                else
                {
                    model.PendingOrNotStartedEnquiriesList = await enqService.GetPendingEnquiriesNotPGD();
                }

                model.GoneOrRejectedEnquiriesList = await enqService.GetApprovedRejectedEnquiries(model.startDateApproved, model.endDateApproved);
                model.SentEnquiriesList = await enqService.GetSentEnquiries(model.startDateSent, model.endDateSent);
            }
 

            if (LoggedInEmployee.TeamId == 15 || employeeDept.Id == 1 || LoggedInEmployee.TeamId == 17 || LoggedInEmployee.TeamId == 36)
            {
                ViewBag.OkToEdit = "true";
            }
            else
            {
                ViewBag.OkToEdit = "false";
            }
            var britishZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");



            var newDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, britishZone);


            ViewBag.CurrentDate = newDate.ToString("yyyy/MM/dd HH:mm:ss");
            return View(model);
        }


        [HttpPost("api/EnquiryToUpdate")]
        public async Task<bool> Update(ViewModels.Enquiries.EnquiriesViewModel model)
        {

            var res = await enqService.Update(model.enqToUpdate, model.enqPriorityToUpdate, model.enqAssignedToUpdate, model.enqNotesToUpdate);
       
            return true;
        }

      
    }
}
