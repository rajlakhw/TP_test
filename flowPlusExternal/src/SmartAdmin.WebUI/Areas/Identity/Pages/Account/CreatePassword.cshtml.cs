using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Data;
using Services;
using Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace flowPlusExternal.Areas.Identity.Pages.Account
{

    [AllowAnonymous]
    public class CreatePasswordModel : PageModel
    {
        private readonly SignInManager<ExtranetUsersTemp> _signInManager;
        private readonly UserManager<ExtranetUsersTemp> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ITPExtranetUserService extranetUserService;

        public CreatePasswordModel(UserManager<ExtranetUsersTemp> userManager, ITPExtranetUserService userService,
                                   SignInManager<ExtranetUsersTemp> signInManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            extranetUserService = userService;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string Code { get; set; }

        }
        public async Task<IActionResult> OnGet(string code = null, string username = null)
        {
            if (code == null || username == null)
            {
                return BadRequest("Invalid request url.");
            }
            else
            {
                var user = await extranetUserService.GetExtranetUserByUsername(username);
                var oldUser = await extranetUserService.GetOldIPlusUser(username);
                if (user == null)
                {
                    if (oldUser == null)
                    {
                        return BadRequest("Bad request");
                    }
                    else
                    {
                        if (oldUser.PasswordResetCode.ToString() == code)
                        {
                            Input = new InputModel
                            {
                                Code = code,
                                Email = username
                            };
                            return Page();
                        }
                        else
                        {
                            return BadRequest("Bad request");
                        }

                    }

                }

                else if (user.PasswordResetCode.ToString() == code)
                {
                    Input = new InputModel
                    {
                        Code = code,
                        Email = username
                    };
                    return Page();
                }
                else
                {
                    return BadRequest("Bad request");
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await extranetUserService.GetExtranetUserByUsername(Input.Email);
            var oldUser = await extranetUserService.GetOldIPlusUser(Input.Email);
            if (user == null)
            {
                if (oldUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid attempt.");
                    return Page();
                }

            }

            if (user != null)
            {
                Regex rx = new Regex(@"^[a-zA-Z0-9\s,]*$");
                bool isNonAlphanumeric = !(rx.IsMatch(Input.Password));

                bool isNumeric = Input.Password.Any(char.IsDigit);
                if (isNonAlphanumeric == false && isNumeric == false)
                {
                    ModelState.AddModelError(string.Empty, "Password must contain a number or alphanumeric character.");
                    return Page();
                }

                var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, passwordToken, Input.Password);

                if (oldUser != null)
                {
                    await extranetUserService.ResetPasswordForIPlusLogin(Input.Email, Input.Password);
                }

                if (result.Succeeded)
                {
                    await extranetUserService.UpdatePasswordResetCode(Input.Email);
                    var result1 = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, lockoutOnFailure: false);
                    if (result1.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect("/Home/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid attempt.");
                        return Page();
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            if (oldUser != null)
            {
                await extranetUserService.ResetPasswordForIPlusLogin(Input.Email, Input.Password);
                await extranetUserService.UpdatePasswordResetCodeForOldIplus(Input.Email);

                var result1 = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, lockoutOnFailure: false);
                if (result1.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect("/Home/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid attempt.");
                    return Page();
                }
            }


            return Page();
        }

    }
}
