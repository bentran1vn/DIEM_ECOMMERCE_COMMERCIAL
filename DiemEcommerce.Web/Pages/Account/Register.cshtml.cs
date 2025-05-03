using System.ComponentModel.DataAnnotations;
using DiemEcommerce.Web.Models.Account;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IAccountService _accountService;

    public RegisterModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var registerRequest = new RegisterRequestModel
            {
                Email = Input.Email,
                Username = Input.Username,
                Password = Input.Password,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Phonenumber = Input.PhoneNumber,
                // Role = 0 for Customer as per requirements
                Role = 0
            };

            var result = await _accountService.RegisterAsync(registerRequest);

            if (result != null)
            {
                TempData["SuccessMessage"] = "Registration successful! Please login to your account.";
                return RedirectToPage("./Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed. Please check your information and try again.");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}