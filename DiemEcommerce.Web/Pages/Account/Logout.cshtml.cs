using System.Security.Claims;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Account;

[Authorize]
public class LogoutModel : PageModel
{
    private readonly IAccountService _accountService;

    public LogoutModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public void OnGet()
    {
        // No need for any code here
    }

    public async Task<IActionResult> OnPost()
    {
        // Get the user account identifier from claims
        var userAccount = User.FindFirstValue(ClaimTypes.Name);
        
        if (!string.IsNullOrEmpty(userAccount))
        {
            // Call logout API
            await _accountService.LogoutAsync(userAccount);
        }

        // Remove authentication cookie
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        // Clear cookies
        Response.Cookies.Delete("AccessToken");
        Response.Cookies.Delete("RefreshToken");

        return RedirectToPage("/Index");
    }
}