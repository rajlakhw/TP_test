using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels.MiscResource;
using Microsoft.AspNetCore.Http;
using Data;
using Global_Settings;
using Services;



namespace flowPlusExternal.ViewComponents
{
    public class MiscResourceViewComponent : ViewComponent
    {
        private readonly ITPMiscResourceService miscResourceService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITPExtranetUserService extranetUserService;

        public MiscResourceViewComponent(ITPMiscResourceService _miscResourceService,
                                         IHttpContextAccessor _httpContextAccessor,
                                         ITPExtranetUserService _extranetUserService)
        {
            this.miscResourceService = _miscResourceService;
            this.httpContextAccessor = _httpContextAccessor;
            this.extranetUserService = _extranetUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string ResourceName)
        {
            var currentUILang = httpContextAccessor.HttpContext.Session.Get<LocalLanguageInfo>("CurrentUILang");
            if (currentUILang == null)
            {
                currentUILang = await extranetUserService.GetCurrentExtranetUserDefaultLanguage();
            }

            var miscResourceObj = new MiscResourceModel()
            {
                StringContent = miscResourceService.GetMiscResourceByName(ResourceName, currentUILang.LanguageIanacodeBeingDescribed).Result.StringContent
            };

            return View(miscResourceObj);
        }

    }
}
