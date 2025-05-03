using DiemEcommerce.Web.Helpers;
using DiemEcommerce.Web.Models.Category;
using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace DiemEcommerce.Web.Services;

public class CategoryService : ICategoryService
{
    private readonly ApiHelper _apiHelper;

    public CategoryService(
        HttpClient httpClient, 
        IHttpContextAccessor httpContextAccessor,
        IOptions<ApiOptions> apiOptions)
    {
        _apiHelper = new ApiHelper(httpClient, httpContextAccessor, apiOptions);
    }

    public async Task<List<CategoryViewModel>?> GetAllCategoriesAsync()
    {
        var response = await _apiHelper.GetAsync<ApiResponse<List<CategoryViewModel>>>("category");
        return response.Value;
    }
}