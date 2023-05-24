using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Data;
using Services;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;


namespace flowPlusExternal.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ExtranetUsersTemp> _userManager;
        private readonly ITPExtranetUserService extranetUserService;

        public ResetPasswordModel(UserManager<ExtranetUsersTemp> userManager, ITPExtranetUserService userService)
        {
            _userManager = userManager;
            extranetUserService = userService;
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
            public string ReturnLink { get; set; }

        }

        public async Task<IActionResult> OnGet(string code = null, string username = null, string returnLink = null)
        {
            if (code == null || username == null || returnLink == null)
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
                                Email = username,
                                ReturnLink = returnLink
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
                        Email = username,
                        ReturnLink = returnLink
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
                    // Don't reveal that the user does not exist
                    return RedirectToPage("./ResetPasswordConfirmation", new { returnLink = Input.ReturnLink });
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
                    return RedirectToPage("./ResetPasswordConfirmation", new { returnLink = Input.ReturnLink });
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

                return RedirectToPage("./ResetPasswordConfirmation", new { returnLink = Input.ReturnLink });
            }


            return Page();
        }
    }
}
