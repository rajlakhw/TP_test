using System;
using System.Linq;
using System.Security.Claims;
using Data;
using Data.Repositories;
using flowPlusExternal.Models.ExtranetUserModel;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;
using static Global_Settings.Enumerations;

namespace SmartAdmin.WebUI.ViewComponents
{
    public class DropDownMenuViewComponent : ViewComponent
    {
        private readonly ITPEmployeesService employeesService;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly IRepository<Contact> contactsRepo;
        private readonly IRepository<LinguisticSupplier> linguisticSupplierRepo;
        private readonly IRepository<ExtranetUsersTemp> extranetUserRepo;

        public DropDownMenuViewComponent(ITPEmployeesService employeesService, ITPExtranetUserService extranetUserService,
            IRepository<Contact> contactsRepo, IRepository<LinguisticSupplier> linguisticSupplierRepo,
            IRepository<ExtranetUsersTemp> _extranetUserRepo)
        {
            this.employeesService = employeesService;
            this.extranetUserService = extranetUserService;
            this.contactsRepo = contactsRepo;
            this.linguisticSupplierRepo = linguisticSupplierRepo;
            this.extranetUserRepo = _extranetUserRepo;
        }
        public IViewComponentResult Invoke()
        {
            string extranetUserName = User.Identity.Name;
            var extranetUserInfo = extranetUserService.GetExtranetUserByUsername(extranetUserName).Result;
            var dataObjectTypeId = extranetUserInfo.DataObjectTypeId;
            var dataObjectId = extranetUserInfo.DataObjectId;

            if (DateTime.Now > new DateTime(2022, 11, 26, 10, 30, 0) && DateTime.Now < new DateTime(2022, 11, 26, 16, 0, 0))
            {
                return View("./Identity/Account/Logout");
            }

            string userProfilebase64String = "";

            if (extranetUserInfo.UserProfileImagePath != null && extranetUserInfo.UserProfileImagePath != "")
            {
                var partImgString = String.Format("data:image/{0};base64, ", extranetUserInfo.UserProfileImagePath.Substring(extranetUserInfo.UserProfileImagePath.LastIndexOf(".") + 1));
                var userImgFilePath = "\\\\10.196.48.130\\ExtranetBase\\UserProfileImages\\iplus\\" + extranetUserInfo.UserProfileImagePath;
                byte[] allbytes = System.IO.File.ReadAllBytes(userImgFilePath);
                userProfilebase64String = partImgString + Convert.ToBase64String(allbytes);
            }


            ExtranetUserModel extranetUserModel = new ExtranetUserModel();
            if (extranetUserInfo != null && dataObjectTypeId == Convert.ToByte(DataObjectTypes.Contact))
            {
                extranetUserModel = contactsRepo.All().Where(x => x.Id == dataObjectId)
                                    .Join(extranetUserRepo.All().Where(e => e.DataObjectTypeId == 1),
                                          c => c.Id,
                                          e => e.DataObjectId,
                                          (c, e) => new
                                          {
                                              contact = c,
                                              extranetUser = e
                                          })
                                    .Select(x => new ExtranetUserModel
                                    {
                                        FullName = x.contact.Name,
                                        EmailAddress = x.contact.EmailAddress,
                                        UserProfileImageBase64 = userProfilebase64String
                                    }).FirstOrDefault();
            }

            if (extranetUserInfo != null && dataObjectTypeId == Convert.ToByte(DataObjectTypes.LinguisticSupplier))
            {
                extranetUserModel = linguisticSupplierRepo.All().Where(x => x.Id == dataObjectId).Select(x => new ExtranetUserModel
                {
                    FullName = x.MainContactFirstName + " " + x.MainContactSurname,
                    EmailAddress = x.EmailAddress,
                }).FirstOrDefault();
            }

            return View(extranetUserModel);
        }
    }
}
