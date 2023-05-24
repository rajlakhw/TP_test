using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;

namespace SmartAdmin.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharePlusApiController : ControllerBase
    {
        private readonly ISharePlusService service;

        public SharePlusApiController(ISharePlusService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticlesByNameAsync(string term)
        {
            //var res = new List<SharePlusSearchModel>();
            var result = await service.GetSearchResults(term);

            //foreach (var item in result)
            //{
            //    res.Add(new SharePlusSearchModel
            //    {
            //        Id = item.Id,
            //        Value = item.Title
            //    });
            //}

            var userLoggedInId = HttpContext.Session.Get<short>("EmployeeId");
            if(result.Count > 0)
            {
                await service.AddSearchLog(term, true, userLoggedInId);
            }
            else
            {
                await service.AddSearchLog(term, false, userLoggedInId);
            }

            return Ok(result);
        }

        [HttpPut("Helpful")]
        public async Task<IActionResult> HelpfulArticle([FromForm]int articleId)
        {
            if(articleId == 0)
            {
                return NotFound();
            }

            // CHECK IF THE EMPLOYEE HAS ALREADY MARKED THE ARTICLE AS HELPFUL
            var logId = HttpContext.Session.Get<int>("ViewLogId");
            await service.UpdateCurrentArticleViewAsHelpful(logId);
            var res = await service.GetNumberOfTimesArticleWasFoundHelpful(articleId);
            return Ok(new{ helpfulCount = res });
        }
    }
}
