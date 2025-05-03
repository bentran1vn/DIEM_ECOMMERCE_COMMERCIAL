using DiemEcommerce.Web.Helpers;
using DiemEcommerce.Web.Models.Account;
using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace DiemEcommerce.Web.Services;

public class AccountService : IAccountService
{
    private readonly ApiHelper _apiHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(
        HttpClient httpClient, 
        IHttpContextAccessor httpContextAccessor,
        IOptions<ApiOptions> apiOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _apiHelper = new ApiHelper(httpClient, httpContextAccessor, apiOptions);
    }

    public async Task<LoginResponseModel?> LoginAsync(LoginRequestModel model)
    {
        var response = await _apiHelper.PostAsync<LoginRequestModel, ApiResponse<LoginResponseModel>>("auth/login", model);
        
        if (response != null)
        {
            // Store tokens in cookies
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = response.Value.RefreshTokenExpiryTime
            };
            
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("AccessToken", response.Value.AccessToken, cookieOptions);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("RefreshToken", response.Value.RefreshToken, cookieOptions);
        }
        
        return response.Value;
    }

    public async Task<RegisterResponseModel?> RegisterAsync(RegisterRequestModel model)
    {
        return await _apiHelper.PostAsync<RegisterRequestModel, RegisterResponseModel>("auth/register", model);
    }

    public async Task<bool> LogoutAsync(string userAccount)
    {
        var request = new { UserAccount = userAccount };
        var response = await _apiHelper.PostAsync<object, object>("auth/logout", request);
        
        if (response != null)
        {
            // Clear cookies
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("AccessToken");
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("RefreshToken");
            return true;
        }
        
        return false;
    }

    public async Task<ForgotPasswordResponseModel?> ForgotPasswordAsync(ForgotPasswordRequestModel model)
    {
        return await _apiHelper.PostAsync<ForgotPasswordRequestModel, ForgotPasswordResponseModel>("auth/forgot_password", model);
    }

    public async Task<VerifyCodeResponseModel?> VerifyCodeAsync(VerifyCodeRequestModel model)
    {
        var response = await _apiHelper.PostAsync<VerifyCodeRequestModel, VerifyCodeResponseModel>("auth/verify_code", model);
        
        if (response != null)
        {
            // Store tokens in cookies
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = response.RefreshTokenExpiryTime
            };
            
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("AccessToken", response.AccessToken, cookieOptions);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("RefreshToken", response.RefreshToken, cookieOptions);
        }
        
        return response;
    }

    public async Task<ChangePasswordResponseModel?> ChangePasswordAsync(ChangePasswordRequestModel model)
    {
        return await _apiHelper.PostAsync<ChangePasswordRequestModel, ChangePasswordResponseModel>("auth/change_password", model);
    }

    public async Task<ProfileResponseModel?> GetProfileAsync()
    {
        var response = await _apiHelper.GetAsync<ApiResponse<ProfileResponseModel>>("auth/me");
        return response.Value;
    }
}