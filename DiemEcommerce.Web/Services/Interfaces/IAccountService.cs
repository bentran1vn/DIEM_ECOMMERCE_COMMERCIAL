using DiemEcommerce.Web.Models.Account;

namespace DiemEcommerce.Web.Services.Interfaces;

public interface IAccountService
{
    Task<LoginResponseModel?> LoginAsync(LoginRequestModel model);
    Task<RegisterResponseModel?> RegisterAsync(RegisterRequestModel model);
    Task<bool> LogoutAsync(string userAccount);
    Task<ForgotPasswordResponseModel?> ForgotPasswordAsync(ForgotPasswordRequestModel model);
    Task<VerifyCodeResponseModel?> VerifyCodeAsync(VerifyCodeRequestModel model);
    Task<ChangePasswordResponseModel?> ChangePasswordAsync(ChangePasswordRequestModel model);
    Task<ProfileResponseModel?> GetProfileAsync();
}