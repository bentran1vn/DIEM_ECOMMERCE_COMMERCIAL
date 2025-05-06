using DiemEcommerce.Web.Helpers;
using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Feedback;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace DiemEcommerce.Web.Services;

public class FeedbackService : IFeedbackService
{
    private readonly ApiHelper _apiHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FeedbackService(
        HttpClient httpClient, 
        IHttpContextAccessor httpContextAccessor,
        IOptions<ApiOptions> apiOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _apiHelper = new ApiHelper(httpClient, httpContextAccessor, apiOptions);
    }

    public async Task<PaginatedList<FeedbackViewModel>?> GetFeedbackByProductAsync(Guid matchId, int pageIndex = 1, int pageSize = 10)
    {
        var response = await _apiHelper.GetAsync<ApiResponse<PaginatedList<FeedbackViewModel>>>(
            $"feedback?matchId={matchId}&pageIndex={pageIndex}&pageSize={pageSize}");
        return response.Value;
    }

    public async Task<PaginatedList<FeedbackViewModel>?> GetFeedbackByCustomerAsync(int pageIndex = 1, int pageSize = 10)
    {
        return await _apiHelper.GetAsync<PaginatedList<FeedbackViewModel>>(
            $"feedback/customer?pageIndex={pageIndex}&pageSize={pageSize}");
    }

    public async Task<FeedbackViewModel?> GetFeedbackByIdAsync(Guid feedbackId)
    {
        return await _apiHelper.GetAsync<FeedbackViewModel>($"feedback/{feedbackId}");
    }

    public async Task<FeedbackViewModel?> CreateFeedbackAsync(CreateFeedbackRequestModel model)
    {
        var formData = new MultipartFormDataContent();
        
        formData.Add(new StringContent(model.OrderDetailId.ToString()), "OrderDetailId");
        formData.Add(new StringContent(model.Rating.ToString()), "Rating");
        formData.Add(new StringContent(model.Comment), "Comment");
        
        if (model.Images != null && model.Images.Count > 0)
        {
            foreach (var image in model.Images)
            {
                var stream = image.OpenReadStream();
                var streamContent = new StreamContent(stream);
                formData.Add(streamContent, "Images", image.FileName);
            }
        }
        
        // Since ApiHelper doesn't directly support multipart form data, we need a custom implementation
        var httpClient = new HttpClient();
        var options = _httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<IOptions<ApiOptions>>();
        if (options != null)
        {
            httpClient.BaseAddress = new Uri($"{options.Value.BaseUrl}/{options.Value.ApiVersion}/");
        }
        
        var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
        if (!string.IsNullOrEmpty(accessToken))
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }
        
        var response = await httpClient.PostAsync("feedback", formData);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<FeedbackViewModel>(content);
        }
        
        return null;
    }

    public async Task<FeedbackViewModel?> UpdateFeedbackAsync(UpdateFeedbackRequestModel model)
    {
        var formData = new MultipartFormDataContent();
        
        formData.Add(new StringContent(model.FeedbackId.ToString()), "FeedbackId");
        formData.Add(new StringContent(model.Rating.ToString()), "Rating");
        formData.Add(new StringContent(model.Comment), "Comment");
        
        if (model.DeleteImages != null && model.DeleteImages.Any())
        {
            foreach (var imageId in model.DeleteImages)
            {
                formData.Add(new StringContent(imageId.ToString()), "DeleteImages");
            }
        }
        
        if (model.NewImages != null && model.NewImages.Count > 0)
        {
            foreach (var image in model.NewImages)
            {
                var stream = image.OpenReadStream();
                var streamContent = new StreamContent(stream);
                formData.Add(streamContent, "NewImages", image.FileName);
            }
        }
        
        // Since ApiHelper doesn't directly support multipart form data, we need a custom implementation
        var httpClient = new HttpClient();
        var options = _httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<IOptions<ApiOptions>>();
        if (options != null)
        {
            httpClient.BaseAddress = new Uri($"{options.Value.BaseUrl}/{options.Value.ApiVersion}/");
        }
        
        var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
        if (!string.IsNullOrEmpty(accessToken))
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }
        
        var response = await httpClient.PutAsync($"feedback/{model.FeedbackId}", formData);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<FeedbackViewModel>(content);
        }
        
        return null;
    }

    public async Task<bool> DeleteFeedbackAsync(Guid feedbackId)
    {
        return await _apiHelper.DeleteAsync($"feedback/{feedbackId}");
    }
}