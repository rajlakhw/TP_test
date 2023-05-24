using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SmartAdmin.WebUI.ViewComponents
{
    public class JobOrdersTableViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
