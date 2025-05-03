using System.Net.Http.Headers;
using System.Text;
using DiemEcommerce.Web.Models.Common;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DiemEcommerce.Web.Helpers;

public class ApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiVersion { get; set; } = "v1";
    public int Timeout { get; set; } = 30;
}

public class ApiHelper
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApiOptions _apiOptions;

    public ApiHelper(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<ApiOptions> apiOptions)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _apiOptions = apiOptions.Value;
        
        // Configure base URL
        _httpClient.BaseAddress = new Uri($"{_apiOptions.BaseUrl}/{_apiOptions.ApiVersion}/");
        _httpClient.Timeout = TimeSpan.FromSeconds(_apiOptions.Timeout);
    }

    private void AddAuthorizationHeader()
    {
        var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
        if (!string.IsNullOrEmpty(accessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        AddAuthorizationHeader();
        
        var response = await _httpClient.GetAsync(endpoint);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
        
        // Handle token refresh if needed
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            bool tokenRefreshed = await RefreshTokenAsync();
            if (tokenRefreshed)
            {
                return await GetAsync<T>(endpoint);
            }
        }
        
        return default;
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        AddAuthorizationHeader();
        
        var jsonContent = new StringContent(
            JsonConvert.SerializeObject(data),
            Encoding.UTF8,
            "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, jsonContent);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(content);
        }
        
        // Handle token refresh if needed
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            bool tokenRefreshed = await RefreshTokenAsync();
            if (tokenRefreshed)
            {
                return await PostAsync<TRequest, TResponse>(endpoint, data);
            }
        }
        
        return default;
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        AddAuthorizationHeader();
        
        var jsonContent = new StringContent(
            JsonConvert.SerializeObject(data),
            Encoding.UTF8,
            "application/json");
        
        var response = await _httpClient.PutAsync(endpoint, jsonContent);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(content);
        }
        
        // Handle token refresh if needed
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            bool tokenRefreshed = await RefreshTokenAsync();
            if (tokenRefreshed)
            {
                return await PutAsync<TRequest, TResponse>(endpoint, data);
            }
        }
        
        return default;
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        AddAuthorizationHeader();
        
        var response = await _httpClient.DeleteAsync(endpoint);
        
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        
        // Handle token refresh if needed
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            bool tokenRefreshed = await RefreshTokenAsync();
            if (tokenRefreshed)
            {
                return await DeleteAsync(endpoint);
            }
        }
        
        return false;
    }

    private async Task<bool> RefreshTokenAsync()
    {
        var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];
        var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
        
        if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(accessToken))
        {
            return false;
        }
        
        var tokenRequest = new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        
        var jsonContent = new StringContent(
            JsonConvert.SerializeObject(tokenRequest),
            Encoding.UTF8,
            "application/json");
        
        _httpClient.DefaultRequestHeaders.Authorization = null;
        var response = await _httpClient.PostAsync("auth/refresh_token", jsonContent);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<ApiResponse<TokenResponse>>(content);
            
            if (tokenResponse != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                };
                
                _httpContextAccessor.HttpContext?.Response.Cookies.Delete("AccessToken");
                _httpContextAccessor.HttpContext?.Response.Cookies.Delete("RefreshToken");
                
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("AccessToken", tokenResponse.Value.AccessToken, cookieOptions);
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("RefreshToken", tokenResponse.Value.RefreshToken, cookieOptions);
                
                return true;
            }
        }
        
        return false;
    }

    private class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTimeOffset RefreshTokenExpiryTime { get; set; }
    }
}