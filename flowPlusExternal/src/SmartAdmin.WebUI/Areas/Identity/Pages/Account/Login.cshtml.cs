using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Data;
using Services;
using Services.Interfaces;
using System.Text.RegularExpressions;

namespace flowPlusExternal.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ExtranetUsersTemp> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly UserManager<ExtranetUsersTemp> _userManager;
        private readonly ITPflowplusLicencingLogic flowplusLicencingService;


        public LoginModel(SignInManager<ExtranetUsersTemp> signInManager, ILogger<LoginModel> logger, ITPExtranetUserService userService,
            UserManager<ExtranetUsersTemp> userManager, ITPflowplusLicencingLogic _flowplusLicencingService)
        {
            _signInManager = signInManager;
            _logger = logger;
            extranetUserService = userService;
            _userManager = userManager;
            flowplusLicencingService = _flowplusLicencingService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public string CurrentUILang { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }


        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            if (DateTime.Now > new DateTime(2022, 11, 26, 10, 30, 0) && DateTime.Now < new DateTime(2022, 11, 26, 16, 0, 0))
            {
                return Redirect("/Identity/Account/Maintenance");
            }
            else
            {
                returnUrl = returnUrl ?? Url.Content("~/");
            }

            //CurrentUILang = extranetUserService.GetCurrentExtranetUserDefaultLanguage().Result.LanguageIanacodeBeingDescribed;

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = "/Home/Index";

            if (ModelState.IsValid)
            {

                var MatchFound = await extranetUserService.DoPasswordsMatch(Input.Email, Input.Password);

                if (MatchFound == true)
                {
                    var userLoggingIn = await extranetUserService.GetExtranetUserByUsername(Input.Email);

                    var oldUserLogin = await extranetUserService.GetOldIPlusUser(Input.Email);

                    if (userLoggingIn == null && oldUserLogin.DataObjectTypeId == 1)
                    {

                        //Regex rx = new Regex(@"^[a-zA-Z0-9\s,]*$");
                        //bool isNonAlphanumeric = !(rx.IsMatch(Input.Password));

                        //bool isNumeric = Input.Password.Any(char.IsDigit);
                        //if (isNonAlphanumeric == false && isNumeric == false)
                        //{
                        //    ModelState.AddModelError(string.Empty, "Password must contain a number or alphanumeric character.");
                        //    return Page();
                        //}

                        if (oldUserLogin.AccessLevelId == 212)
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }

                        var user = new ExtranetUsersTemp { UserName = Input.Email, Email = Input.Email };
                        var newUser = await _userManager.CreateAsync(user, Input.Password);
                        userLoggingIn = user;

                        await extranetUserService.UpdateflowPlusExternalDetails(Input.Email);

                    }

                    if (userLoggingIn != null && userLoggingIn.AccessLevelId == 212)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }


                    var FreeTrailCutOffDate = new DateTime(2023, 04, 01);
                    if (userLoggingIn.DataObjectTypeId == 1 && GeneralUtils.GetCurrentUKTime() >= FreeTrailCutOffDate)
                    {
                        var currentOrg = await extranetUserService.GetExtranetUserOrg(Input.Email);
                        var currentGroup = await extranetUserService.GetExtranetUserOrgGroup(Input.Email);
                        var flowPlusLicencingGroupLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentGroup.Id, 3);
                        var flowPlusLicencingOrgLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentOrg.Id, 2);

                        bool AccessPermitted = false;
                        if (flowPlusLicencingGroupLevel != null)
                        {
                            var flowPlusLicence = await flowplusLicencingService.GetflowPlusLicenceObj(flowPlusLicencingGroupLevel.flowplusLicenceID.Value);
                            if (flowPlusLicence != null)
                            {
                                AccessPermitted = flowPlusLicence.IsEnabled;
                            }
                        }

                        if (flowPlusLicencingOrgLevel != null)
                        {
                            var flowPlusLicence = await flowplusLicencingService.GetflowPlusLicenceObj(flowPlusLicencingOrgLevel.flowplusLicenceID.Value);
                            if (flowPlusLicence != null)
                            {
                                AccessPermitted = flowPlusLicence.IsEnabled;
                            }
                        }

                        if (AccessPermitted == false)
                        {
                            ModelState.AddModelError(string.Empty, "Your account is not enabled to access flow plus. Please contact our sales team to enable your access.");
                            return Page();
                        }
                    }

                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
