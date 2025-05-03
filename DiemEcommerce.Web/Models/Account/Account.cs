using System.ComponentModel.DataAnnotations;

namespace DiemEcommerce.Web.Models.Account;

public class LoginRequestModel
{
    [Required(ErrorMessage = "Email or username is required")]
    public string EmailOrUserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseModel
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }
}

public class RegisterRequestModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Phone number is required")]
    public string Phonenumber { get; set; } = string.Empty;
    
    // Role = 0 for Customer as per requirements
    public int Role { get; set; } = 0;
}

public class RegisterResponseModel
{
    public string Message { get; set; } = string.Empty;
}

public class ForgotPasswordRequestModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
}

public class ForgotPasswordResponseModel
{
    public string Message { get; set; } = string.Empty;
}

public class VerifyCodeRequestModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Code is required")]
    public string Code { get; set; } = string.Empty;
}

public class VerifyCodeResponseModel
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }
}

public class ChangePasswordRequestModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "New password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string NewPassword { get; set; } = string.Empty;
}

public class ChangePasswordResponseModel
{
    public string Message { get; set; } = string.Empty;
}

public class ProfileResponseModel
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Phonenumber { get; set; } = string.Empty;
    public DateTimeOffset CreatedOnUtc { get; set; }
    public Subscription? Subcription { get; set; }
}

public class Subscription
{
    public string SubscriptionName { get; set; } = string.Empty;
    public DateTimeOffset SubscriptionEndDate { get; set; }
}