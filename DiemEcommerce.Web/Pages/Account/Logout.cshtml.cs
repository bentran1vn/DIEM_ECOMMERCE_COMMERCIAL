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
    
        // Try multiple cookie deletion approaches
        // 1. Basic deletion
        Response.Cookies.Delete("AccessToken");
        Response.Cookies.Delete("RefreshToken");
    
        // 2. With path
        Response.Cookies.Delete("AccessToken", new CookieOptions { Path = "/" });
        Response.Cookies.Delete("RefreshToken", new CookieOptions { Path = "/" });
    
        // 3. Overwrite with expired cookies
        Response.Cookies.Append("AccessToken", "", new CookieOptions { 
            Expires = DateTime.Now.AddDays(-1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/"
        });
    
        Response.Cookies.Append("RefreshToken", "", new CookieOptions { 
            Expires = DateTime.Now.AddDays(-1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/"
        });
    
        // Direct redirect to home page
        return Redirect("~/");
    }
}