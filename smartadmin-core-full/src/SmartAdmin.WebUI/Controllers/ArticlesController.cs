using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Services;
using Global_Settings;
using ViewModels.SharePlus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using Custom_Exceptions;

namespace SmartAdmin.WebUI.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ISharePlusService service;
        private readonly IConfiguration configuration;
        private readonly ITPEmployeesService employeeService;

        public ArticlesController(ISharePlusService service,
            ITPEmployeesService employeeService,
            IConfiguration configuration)
        {
            this.service = service;
            this.employeeService = employeeService;
            this.configuration = configuration;
        }

        [Route("[controller]")]
        [Route("[controller]/index")]
        public async Task<IActionResult> Index()
        {
            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);

            var a = GlobalVars.LondonJobDriveBaseDirectoryPathForApp;

            var employee = await employeeService.GetLoggedInEmployeeFromSessionOrDatabase();
            if (employee == null) { return NotFound(); }

            SharePlusViewModel results = new SharePlusViewModel()
            {
                Pinned = await service.GetAllPinnedArticles(),
                MostViewed = await service.GetMostViewedArticles(),
                Employee = employee,
                InspirationalQuote = GeneralUtils.GetARandomInspirationalQuote(),
                IsAllowedToEdit = await this.SharePlusAccessPermissions()
            };
            return View(results);
            //var pageNumber = page ?? 1;
            //var pageSize = 10;
            //var onePageOfArticles = results.ToPagedList(pageNumber, pageSize);
            //return View(onePageOfArticles);
        }

        [Route("[controller]/[action]/{searchTerm}")]
        public async Task<IActionResult> ArticlesAsync(string searchTerm)
        {
            if (searchTerm == null)
            {
                return RedirectToAction("Articles");
            }

            var employee = await employeeService.GetLoggedInEmployeeFromSessionOrDatabase();

            var result = new SharePlusViewModel()
            {
                SearchResult = await service.GetSearchResults(searchTerm),
                Employee = employee,
                InspirationalQuote = GeneralUtils.GetARandomInspirationalQuote(),
                IsAllowedToEdit = await this.SharePlusAccessPermissions()
            };
            return View("Index", result);
        }

        public async Task<IActionResult> ArticleAsync(int Id)
        {
            var article = await service.GetById(Id);

            if (article == null || article.CreatedByEmpId == 0) { return NotFound(); }

            var employee = await employeeService.GetLoggedInEmployeeFromSessionOrDatabase();

            var employeeLoggedInId = employee.Id;

            var viewLog = await service.AddArticleViewedLog(article.Id, employeeLoggedInId);
            HttpContext.Session.Set("ViewLogId", viewLog);

            var createdByEmp = await employeeService.IdentifyCurrentUserById(article.CreatedByEmpId);
            var lastModifiedByEmp = new Employee();
            if (article.LastModifiedByEmpId != null && article.LastModifiedDateTime != null)
            {
                lastModifiedByEmp = await employeeService.IdentifyCurrentUserById((int)article.LastModifiedByEmpId);
            }
            var dep = await employeeService.GetEmployeeDepartment(employeeLoggedInId);

            var viewResult = new SharePlusArticleViewModel();

            try
            {
                viewResult.Contents = article.Contents;
                viewResult.CreatedByEmpId = article.CreatedByEmpId;
                viewResult.CreatedByEmployeeFirstName = createdByEmp.FirstName;
                viewResult.CreatedByEmployeeId = article.CreatedByEmpId;
                viewResult.CreatedByEmployeeImageBase64 = createdByEmp.ImageBase64;
                viewResult.CreatedByEmployeeSurname = createdByEmp.Surname;
                viewResult.CreatedDateTime = article.CreatedDateTime;
                viewResult.HistoricalNumberOfViews = article.NumberOfTimesViewed;
                viewResult.Htmlbody = article.Htmlbody;
                viewResult.Id = article.Id;
                viewResult.LastModifiedByEmpName = lastModifiedByEmp.FirstName + " " + lastModifiedByEmp.Surname;
                viewResult.LastModifiedDateTime = article.LastModifiedDateTime;
                viewResult.LastViewedDateTime = article.LastViewedDateTime;
                viewResult.Title = article.Title;
                viewResult.EmployeeLoggedIn = employee;
                viewResult.EmployeeLoggedInDepartmentId = dep.Id;
                viewResult.EmployeeMarkedArticleAsHelpful = await service.CheckIfEmployeeHasMarkedAnArticleAsHelpful(article.Id, employeeLoggedInId);
                viewResult.HelpfulCount = await service.GetNumberOfTimesArticleWasFoundHelpful(article.Id);
                viewResult.IsAllowedToEdit = await this.SharePlusAccessPermissions();
            }
            catch (Exception e)
            {
                throw new CustomSharePlusException(e.Message, viewResult, employeeLoggedInId);
            }
            return View(viewResult);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await service.GetById(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SharePlusUpdateModel model)
        {
            var employeeLoggedIn = await employeeService.GetLoggedInEmployeeFromSessionOrDatabase();

            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Articles");
            }
            var result = await service.Update<SharePlusUpdateModel>(model.Id, model.Title, model.Htmlbody, model.Contents, employeeLoggedIn.Id);
            return this.RedirectToAction("Article", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SharePlusCreateModel model)
        {
            var employeeLoggedIn = await employeeService.GetLoggedInEmployeeFromSessionOrDatabase();

            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Articles");
            }
            var result = await service.Create<SharePlusCreateModel>(model.Title, model.Htmlbody, employeeLoggedIn.Id, model.Contents, model.IsPinnedArticle);
            return this.RedirectToAction("Article", new { id = result });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var employeeLoggedIn = await employeeService.GetLoggedInEmployeeFromSessionOrDatabase();

            if (employeeLoggedIn != null)
            {
                await service.Delete(Id, employeeLoggedIn.Id);
            }
            else
            {
                return NotFound();
            }
            return Ok();
        }

        public async Task<bool> SharePlusAccessPermissions()
        {
            var employeeLoggedIn = await employeeService.GetLoggedInEmployeeFromSessionOrDatabase();

            int[] TeamIds = new int[] { 15, 36, 40, 10, 17, 38, 19, 42 };
            int[] EmpIds = new int[] { 638, 801, 646, 959, 638, 41, 1298, 1352, 706, 13 };
            var dep = await employeeService.GetEmployeeDepartment(employeeLoggedIn.Id);

            if (dep.Id != 5 &
                    TeamIds.Contains(employeeLoggedIn.TeamId) == false &
                    EmpIds.Contains(employeeLoggedIn.Id) == false) { return false; }

            return true;
        }
    }
}
