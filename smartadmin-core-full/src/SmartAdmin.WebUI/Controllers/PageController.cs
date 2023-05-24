using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;
using System.Threading.Tasks;

namespace SmartAdmin.WebUI.Controllers
{
    public class PageController : Controller
    {
        private readonly ISearchForAnything searchForServices; 
        public PageController(ISearchForAnything _searchForServices)
        {
            searchForServices = _searchForServices;
        }

        public IActionResult Chat() => View();
        public IActionResult Confirmation() => View();
        public IActionResult Contacts() => View();
        public IActionResult Error() => View();
        public IActionResult Error404() => View();
        public IActionResult ErrorAnnounced() => View();
        public IActionResult Forget() => View();
        public IActionResult ForumDiscussion() => View();
        public IActionResult ForumList() => View();
        public IActionResult ForumThreads() => View();
        public IActionResult InboxGeneral() => View();
        public IActionResult InboxRead() => View();
        public IActionResult InboxWrite() => View();
        public IActionResult Invoice() => View();
        public IActionResult Locked() => View();
        public IActionResult Login() => View();
        public IActionResult LoginAlt() => View();
        public IActionResult Profile() => View();
        public IActionResult Projects() => View();
        public IActionResult Register() => View();


        public async Task<IActionResult>  Search(string searchedFor) {
            var model = new ViewModels.SearchForAnything.SearchForAnythingResults();
            model = await searchForServices.GetAllResults(searchedFor);
            
            model.searchedFor = searchedFor;
            return View(model);
        }
        
    }
}
