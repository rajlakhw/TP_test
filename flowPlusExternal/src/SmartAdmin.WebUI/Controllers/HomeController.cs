using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViewModels.HomePage;
using Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Data;
using System.IO;
using ViewModels.Organisation;

namespace flowPlusExternal.Controllers
{
    public class HomeController : Controller
    {

        private readonly ITPEnquiriesService enquiriesService;
        private readonly ITPJobOrderService jobOrderService;
        private readonly UserManager<ExtranetUsersTemp> _userManager;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly ITPCurrenciesLogic currenciesLogic;

        public HomeController(ITPEnquiriesService service, ITPJobOrderService service1,
                              ITPExtranetUserService service2, UserManager<ExtranetUsersTemp> userManager,
                              ITPCurrenciesLogic _currenciesLogic)
        {
            this.enquiriesService = service;
            this.jobOrderService = service1;
            this.extranetUserService = service2;
            this._userManager = userManager;
            this.currenciesLogic = _currenciesLogic;
        }
        public async Task<IActionResult> IndexAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            //string extranetUserName = User.FindFirst(ClaimTypes.Name).Value;

            ViewBag.DataObjectID = await extranetUserService.GetPermittedDataObjectID(extranetUserName);
            ViewBag.DataObjectTypeId = await extranetUserService.GetPermittedDataObjectTypeID(extranetUserName);
            var defaultCurrencyId = extranetUserService.GetExtranetUserOrg(extranetUserName).Result.InvoiceCurrencyId;
            if (defaultCurrencyId == null) { defaultCurrencyId = 4; }
            ViewData["CurrencyPrefix"] = currenciesLogic.GetById(defaultCurrencyId.Value).Result.Prefix;
            //flowPlusExtClientHomePage Result = new flowPlusExtClientHomePage()
            //{
            //    NumberOfPendingQuotes = await enquiriesService.GetNumberOfPendingEnquiriesForClient(extranetUserName),
            //    ValueOfPendingQuotes = await enquiriesService.GetValueOfPendingEnquiriesForClient(extranetUserName),
            //    NumberOfOpenProjects = await jobOrderService.GetNumberOfOpenJobOrdersForClient(extranetUserName),
            //    ValueOfOpenProjects = await jobOrderService.GetValueOfOpenJobOrdersForClient(extranetUserName),
            //    NumberOfServiceInProgressProjects = await jobOrderService.GetNumberOfJobOrdersServiceInProgressForClient(extranetUserName),
            //    ValueOfServiceInProgressProjects = await jobOrderService.GetValueOfJobOrdersServiceInProgressForClient(extranetUserName),
            //    NumberOfInReviewProjects = await jobOrderService.GetNumberOfJobOrdersInReviewForClient(extranetUserName),
            //    ValueOfInReviewProjects = await jobOrderService.GetValueOfJobOrdersInReviewForClient(extranetUserName),
            //    NumberOfFinalChecksProjects = await jobOrderService.GetNumberOfJobOrdersInFinalChecksForClient(extranetUserName),
            //    ValueOfFinalChecksProjects = await jobOrderService.GetValueOfJobOrdersInFinalChecksForClient(extranetUserName),
            //    NumberOfReadyToCollectProjects = await jobOrderService.GetNumberOfJobOrdersReadyToCollectForClient(extranetUserName),
            //    ValueOfReadyToCollectProjects = await jobOrderService.GetValueOfJobOrdersReadyToCollectForClient(extranetUserName),
            //    TotalNumberOfJobItemsInOpenProjects = await jobOrderService.GetNumberOfJobItemsInOpenOrdersForClient(extranetUserName),
            //    TotalNumberOfCompletedJobItems = await jobOrderService.GetNumberOfCompleteJobItemsInOpenOrdersForClient(extranetUserName),
            //    TotalWordCountInOpenProjects = await jobOrderService.GetWordCountOfAllJobItemsInOpenOrdersForClient(extranetUserName),
            //    TotalWordCountOfCompletedJobItems = await jobOrderService.GetWordCountOfCompleteJobItemsInOpenOrdersForClient(extranetUserName)

            //};
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllJobOrdersDataTableComponentData([FromBody] DataTables dataParams)
        {
            var data = await jobOrderService.GetAllJobOrdersForQuickProjOverview(dataParams.dataObjectId, dataParams.dataTypeId, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value, dataParams.parameters.order.FirstOrDefault().column, dataParams.parameters.order.FirstOrDefault().dir);

            int totalRecords;
            int filteredRecords;
            (totalRecords, filteredRecords) = await jobOrderService.GetAllJobOrdersCountForQuickProjOverview(dataParams.dataObjectId, dataParams.dataTypeId, dataParams.parameters.search.value);

            return Ok(new { data, recordsTotal = totalRecords, recordsFiltered = filteredRecords });
        }

        [HttpPost]
        public async Task<IActionResult> GetNumberOfPendingQuotesAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            int numberOfPendingEnq = await enquiriesService.GetNumberOfPendingEnquiriesForClient(extranetUserName);

            return Ok(numberOfPendingEnq);
        }

        [HttpPost]
        public async Task<IActionResult> GetValueOfPendingQuotesAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal valueOfPendingEnq = await enquiriesService.GetValueOfPendingEnquiriesForClient(extranetUserName);

            return Ok(valueOfPendingEnq.ToString("N2"));
        }

        [HttpPost]
        public async Task<IActionResult> GetNumberOfOpenProjectsAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            int numberOfOpenProj = await jobOrderService.GetNumberOfOpenJobOrdersForClient(extranetUserName);

            return Ok(numberOfOpenProj);
        }

        [HttpPost]
        public async Task<IActionResult> GetValueOfOpenProjectsAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal valueOfOpenProj = await jobOrderService.GetValueOfOpenJobOrdersForClient(extranetUserName);

            return Ok(valueOfOpenProj.ToString("N2"));
        }

        [HttpPost]
        public async Task<IActionResult> GetNumberOfJobsInProgressAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal numberOfInProgressProj = await jobOrderService.GetNumberOfJobOrdersServiceInProgressForClient(extranetUserName);

            return Ok(numberOfInProgressProj);
        }

        [HttpPost]
        public async Task<IActionResult> GetValueOfJobsInProgressAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal valueOfInProgressProj = await jobOrderService.GetValueOfJobOrdersServiceInProgressForClient(extranetUserName);

            return Ok(valueOfInProgressProj.ToString("N2"));
        }

        [HttpPost]
        public async Task<IActionResult> GetNumberOfJobsInReviewAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal numberOfInReviewProj = await jobOrderService.GetNumberOfJobOrdersInReviewForClient(extranetUserName);

            return Ok(numberOfInReviewProj);
        }

        [HttpPost]
        public async Task<IActionResult> GetValueOfJobsInReviewAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal valueOfInReviewProj = await jobOrderService.GetValueOfJobOrdersInReviewForClient(extranetUserName);

            return Ok(valueOfInReviewProj.ToString("N2"));
        }

        [HttpPost]
        public async Task<IActionResult> GetNumberOfJobsInFinalChecksAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal numberOfInFinalChecksProj = await jobOrderService.GetNumberOfJobOrdersInFinalChecksForClient(extranetUserName);

            return Ok(numberOfInFinalChecksProj);
        }

        [HttpPost]
        public async Task<IActionResult> GetValueOfJobsInFinalChecksAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal valueOfInFinalChecksProj = await jobOrderService.GetValueOfJobOrdersInFinalChecksForClient(extranetUserName);

            return Ok(valueOfInFinalChecksProj.ToString("N2"));
        }

        [HttpPost]
        public async Task<IActionResult> GetNumberOfReadyToCollectJobsAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal numberOfReadyToCollectProj = await jobOrderService.GetNumberOfJobOrdersReadyToCollectForClient(extranetUserName);

            return Ok(numberOfReadyToCollectProj);
        }

        [HttpPost]
        public async Task<IActionResult> GetValueOfReadyToCollectJobsAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal valueOfReadyToCollectProj = await jobOrderService.GetValueOfJobOrdersReadyToCollectForClient(extranetUserName);

            return Ok(valueOfReadyToCollectProj.ToString("N2"));
        }

        [HttpPost]
        public async Task<IActionResult> GetNumberOfCompletedJobItemsAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal numberOfCompletedItems = await jobOrderService.GetNumberOfCompleteJobItemsInOpenOrdersForClient(extranetUserName);

            return Ok(numberOfCompletedItems);
        }

        [HttpPost]
        public async Task<IActionResult> GetCompletedJobItemsPercentageAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            var TotalNumberOfJobItemsInOpenProjects = await jobOrderService.GetNumberOfJobItemsInOpenOrdersForClient(extranetUserName);
            var TotalNumberOfCompletedJobItems = await jobOrderService.GetNumberOfCompleteJobItemsInOpenOrdersForClient(extranetUserName);

            decimal Percentage = 0;
            if (TotalNumberOfJobItemsInOpenProjects != 0 && TotalNumberOfCompletedJobItems != 0)
            {
                Percentage = (decimal)TotalNumberOfCompletedJobItems / (decimal)TotalNumberOfJobItemsInOpenProjects * 100;
            }

            return Ok(Percentage);
        }

        [HttpPost]
        public async Task<IActionResult> GetTotalCompletedJobItemsStatsStringAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            var TotalNumberOfJobItemsInOpenProjects = await jobOrderService.GetNumberOfJobItemsInOpenOrdersForClient(extranetUserName);
            var TotalNumberOfCompletedJobItems = await jobOrderService.GetNumberOfCompleteJobItemsInOpenOrdersForClient(extranetUserName);

            string stringToReturn = String.Format("{0} out of {1} items have been completed", TotalNumberOfCompletedJobItems, TotalNumberOfJobItemsInOpenProjects);

            return Ok(stringToReturn);
        }


        [HttpPost]
        public async Task<IActionResult> GetNumberOfCompletedWordCountAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            decimal numberOfCompletedWordCounts = await jobOrderService.GetWordCountOfCompleteJobItemsInOpenOrdersForClient(extranetUserName);

            return Ok(numberOfCompletedWordCounts);
        }

        [HttpPost]
        public async Task<IActionResult> GetCompletedWordCountPercentageAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            var TotalNumberOfWordCountInOpenProjects = await jobOrderService.GetWordCountOfAllJobItemsInOpenOrdersForClient(extranetUserName);
            var TotalNumberCompletedWordCount = await jobOrderService.GetWordCountOfCompleteJobItemsInOpenOrdersForClient(extranetUserName);

            decimal Percentage = 0;
            if (TotalNumberOfWordCountInOpenProjects != 0 && TotalNumberCompletedWordCount != 0)
            {
                Percentage = (decimal)TotalNumberCompletedWordCount / (decimal)TotalNumberOfWordCountInOpenProjects * 100;
            }

            return Ok(Percentage);
        }

        [HttpPost]
        public async Task<IActionResult> GetTotalCompletedWordCountStatsStringAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            var TotalNumberOfWordCountInOpenProjects = await jobOrderService.GetWordCountOfAllJobItemsInOpenOrdersForClient(extranetUserName);
            var TotalNumberCompletedWordCount = await jobOrderService.GetWordCountOfCompleteJobItemsInOpenOrdersForClient(extranetUserName);

            string stringToReturn = String.Format("{0} out of {1} words have been completed", TotalNumberCompletedWordCount, TotalNumberOfWordCountInOpenProjects);

            return Ok(stringToReturn);
        }

        [HttpPost]

        public async Task<IActionResult> UpdateUILanguage()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var LangIANACode = content.Result.Replace("\"", "");

            await extranetUserService.UpdateCurrentExtranetUserDefaultLanguage(LangIANACode);

            return Ok();

        }
    }
}
