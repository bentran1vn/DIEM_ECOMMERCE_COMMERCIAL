using DiemEcommerce.Web.Helpers;
using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Match;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace DiemEcommerce.Web.Services;

public class MatchService : IMatchService
{
    private readonly ApiHelper _apiHelper;

    public MatchService(
        HttpClient httpClient, 
        IHttpContextAccessor httpContextAccessor,
        IOptions<ApiOptions> apiOptions)
    {
        _apiHelper = new ApiHelper(httpClient, httpContextAccessor, apiOptions);
    }

    public async Task<PaginatedList<MatchViewModel>?> GetAllMatchesAsync(int pageIndex = 1, int pageSize = 10, string? searchTerm = null, List<Guid>? categoryIds = null)
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
        
        if (categoryIds != null && categoryIds.Any())
        {
            foreach (var categoryId in categoryIds)
            {
                queryParams.Add($"categoryIds={categoryId}");
            }
        }
        
        string endpoint = $"matches?{string.Join("&", queryParams)}";
        
        var response = await _apiHelper.GetAsync<ApiResponse<PaginatedList<MatchViewModel>>>(endpoint);

        return response.Value;
    }

    public async Task<MatchDetailViewModel?> GetMatchByIdAsync(Guid id)
    {
        var response = await _apiHelper.GetAsync<ApiResponse<MatchDetailViewModel>>($"matches/{id}");
        return response.Value; 
    }

    public async Task<PaginatedList<MatchViewModel>?> GetMatchesByFactoryIdAsync(Guid factoryId)
    {
        return await _apiHelper.GetAsync<PaginatedList<MatchViewModel>>($"factories/{factoryId}/matches");
    }
}