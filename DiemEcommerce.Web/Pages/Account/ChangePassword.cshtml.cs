using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DiemEcommerce.Web.Models.Account;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Account;

[Authorize]
public class ChangePasswordModel : PageModel
{
    private readonly IAccountService _accountService;

    public ChangePasswordModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    [TempData]
    public string? SuccessMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

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

        var email = User.FindFirstValue(ClaimTypes.Email);
        
        if (string.IsNullOrEmpty(email))
        {
            ModelState.AddModelError(string.Empty, "User email not found. Please login again.");
            return Page();
        }

        var changePasswordRequest = new ChangePasswordRequestModel
        {
            Email = email,
            NewPassword = Input.NewPassword
        };

        var result = await _accountService.ChangePasswordAsync(changePasswordRequest);

        if (result != null)
        {
            SuccessMessage = "Your password has been changed successfully.";
            return Page();
        }

        ModelState.AddModelError(string.Empty, "Failed to change password. Please try again.");
        return Page();
    }
}