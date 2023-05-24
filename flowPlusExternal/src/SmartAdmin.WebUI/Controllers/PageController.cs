using Microsoft.AspNetCore.Mvc;
using flowPlusExternal.Controllers;

namespace SmartAdmin.WebUI.Controllers
{
    public class PageController : Controller
    {
        public IActionResult Locked() => View();

        [HttpGet]
        [Route("[controller]/[action]/{appName}")]
        public IActionResult AccessDisabled(string appName)
        {
            ViewBag.AppName = appName;
            return View();
        }

    }
}
