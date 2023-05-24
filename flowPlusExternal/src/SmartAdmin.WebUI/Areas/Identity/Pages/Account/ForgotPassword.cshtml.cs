using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Data;
using Services;
using Services.Interfaces;
using System;

namespace flowPlusExternal.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly SignInManager<ExtranetUsersTemp> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<ExtranetUsersTemp> _userManager;
        //private readonly IEmailSender _emailSender;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly IEmailUtilsService emailService;
        private readonly ITPBrandsService brandsService;
        private readonly ITPContactsLogic contactService;

        public ForgotPasswordModel(SignInManager<ExtranetUsersTemp> signInManager, ITPExtranetUserService userService,
            ILogger<LogoutModel> logger, UserManager<ExtranetUsersTemp> userManager, IEmailUtilsService emailUtilsService,
            ITPBrandsService service4, ITPContactsLogic tPContactsLogic)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            extranetUserService = userService;
            emailService = emailUtilsService;
            brandsService = service4;
            contactService = tPContactsLogic;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        //public async System.Threading.Tasks.Task OnGet()
        //{
        //    await _signInManager.SignOutAsync();

        //    _logger.LogInformation("User logged out.");
        //}

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await extranetUserService.GetExtranetUserByUsername(Input.Email);
                //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                //{
                //    // Don't reveal that the user does not exist or is not confirmed
                //    return RedirectToPage("./ForgotPasswordConfirmation");
                //}

                //// For more information on how to enable account confirmation and password reset please 
                //// visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = Guid.NewGuid();
                await extranetUserService.UpdatePasswordResetCode(Input.Email, code);
                var username = Input.Email;
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { username, code },
                    protocol: Request.Scheme);

                var contactObject = await contactService.GetById(user.DataObjectId);

                var currentOrg = await extranetUserService.GetExtranetUserOrg(Input.Email);

                var currentBrand = await brandsService.GetBrandById(1);

                if (currentOrg.OrgGroupId != null)
                {
                    currentBrand = await brandsService.GetBrandForClient(currentOrg.OrgGroupId.Value);
                }

                var flowPlusExternalPswrdResetLink = "https://flowplus.translateplus.com/Identity/Account/ResetPassword?code=" + code.ToString() + "&username=" + Input.Email + "&returnLink=flowplus";
                string EmailBody = String.Format("<p>Dear {0}, <br/><br/>The request to reset your <b>i plus</b> account password has been recieved.<br/><br/>" +
                                                "Please click the link below to reset your password<br/><br/>" +
                                                "<a href=\"{1}\">{1}</a><br/><br/>" +
                                                "If you copy and paste this link, please take care to ensure you select only the link itself, avoiding any trailing spaces or tab characters." +
                                                "<br /><br />Yours sincerely, <br /><br />The <b>i plus</b> team</p>",
                                                contactObject.Name, flowPlusExternalPswrdResetLink);
                //string EmailBody = await miscResourceService.GetMiscResourceByName("ForgotPasswordEmailBody", "en").Result.StringContent;
                emailService.SendMail("flow plus <flowplus@translateplus.com>", contactObject.EmailAddress, "Password reset request",
                                            EmailBody, IsExternalNotification: true);
                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");



                //return Page();
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
