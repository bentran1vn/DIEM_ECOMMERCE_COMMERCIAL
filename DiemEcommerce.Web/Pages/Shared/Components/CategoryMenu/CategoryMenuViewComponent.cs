using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiemEcommerce.Web.Pages.Shared.Components.CategoryMenu;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public CategoryMenuViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return View(categories);
    }
}