using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;
using System.IO;
using Services.Interfaces;

namespace SmartAdmin.WebUI.Controllers
{
    public class OrgDetailsController : Controller
    {
        private readonly ITPOrgsLogic orgService;

        public OrgDetailsController(ITPOrgsLogic service)
        {
            this.orgService = service;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllOrgSuggestionsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            var result = await orgService.GetAllOrgSuggestionsResults<Org>(stringToUse);

            return Ok(result);
        }
    }
}
