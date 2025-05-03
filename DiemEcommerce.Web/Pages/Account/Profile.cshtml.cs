using DiemEcommerce.Web.Models.Account;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Account;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly IAccountService _accountService;

    public ProfileModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public ProfileResponseModel? Profile { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Profile = await _accountService.GetProfileAsync();

        if (Profile == null)
        {
            // If profile information cannot be retrieved, redirect to login
            return RedirectToPage("./Login");
        }

        return Page();
    }
}