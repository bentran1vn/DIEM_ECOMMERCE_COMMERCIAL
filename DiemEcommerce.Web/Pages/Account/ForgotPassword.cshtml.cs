using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DiemEcommerce.Web.Models.Account;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Account;

public class ForgotPasswordModel : PageModel
{
    private readonly IAccountService _accountService;

    public ForgotPasswordModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Verification code is required")]
    public string Code { get; set; } = string.Empty;

    public bool EmailSent { get; set; }

    public void OnGet()
    {
        // Initialize page
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var forgotPasswordRequest = new ForgotPasswordRequestModel
        {
            Email = Email
        };

        var result = await _accountService.ForgotPasswordAsync(forgotPasswordRequest);

        if (result != null)
        {
            EmailSent = true;
            return Page();
        }

        ModelState.AddModelError(string.Empty, "There was an error sending the verification code. Please try again.");
        return Page();
    }

    public async Task<IActionResult> OnPostVerifyCodeAsync()
    {
        if (!ModelState.IsValid)
        {
            EmailSent = true;
            return Page();
        }

        var verifyCodeRequest = new VerifyCodeRequestModel
        {
            Email = Email,
            Code = Code
        };

        var result = await _accountService.VerifyCodeAsync(verifyCodeRequest);

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

            // Redirect to change password page
            return RedirectToPage("./ChangePassword");
        }

        ModelState.AddModelError(string.Empty, "Invalid verification code. Please try again.");
        EmailSent = true;
        return Page();
    }
}