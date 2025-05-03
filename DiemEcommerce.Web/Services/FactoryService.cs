using DiemEcommerce.Web.Helpers;
using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Factory;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace DiemEcommerce.Web.Services;

public class FactoryService : IFactoryService
{
    private readonly ApiHelper _apiHelper;

    public FactoryService(
        HttpClient httpClient, 
        IHttpContextAccessor httpContextAccessor,
        IOptions<ApiOptions> apiOptions)
    {
        _apiHelper = new ApiHelper(httpClient, httpContextAccessor, apiOptions);
    }

    public async Task<PaginatedList<FactoryViewModel>?> GetAllFactoriesAsync(int pageIndex = 1, int pageSize = 10, string? searchTerm = null)
    {
        // Build query parameters
        var queryParams = new List<string>
        {
            $"pageIndex={pageIndex}",
            $"pageSize={pageSize}"
        };
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            queryParams.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");
        }
        
        string endpoint = $"factories?{string.Join("&", queryParams)}";
        
        var response = await _apiHelper.GetAsync<ApiResponse<PaginatedList<FactoryViewModel>>>(endpoint);

        return response.Value;
    }

    public async Task<FactoryDetailViewModel?> GetFactoryByIdAsync(Guid id)
    {
        return await _apiHelper.GetAsync<FactoryDetailViewModel>($"factories/{id}");
    }
}