using DiemEcommerce.Web.Models.Category;

namespace DiemEcommerce.Web.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryViewModel>?> GetAllCategoriesAsync();
}