using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DiemEcommerce.Web.Models.Account;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IAccountService _accountService;

    public LoginModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Email or username is required")]
        public string EmailOrUserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public void OnGet(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var loginRequest = new LoginRequestModel
            {
                EmailOrUserName = Input.EmailOrUserName,
                Password = Input.Password
            };

            var result = await _accountService.LoginAsync(loginRequest);

            if (result != null)
            {
                // Parse JWT token for claims
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(result.AccessToken);

                // Create identity and sign in
                var claims = token.Claims.ToList();
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = result.RefreshTokenExpiryTime
                    });

                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials and try again.");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}