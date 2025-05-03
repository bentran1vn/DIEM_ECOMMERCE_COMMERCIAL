using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Match;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Matches;

public class IndexModel : PageModel
{
    private readonly IMatchService _matchService;
    private readonly ICategoryService _categoryService;
    private readonly ICartService _cartService;

    public IndexModel(
        IMatchService matchService, 
        ICategoryService categoryService, 
        ICartService cartService)
    {
        _matchService = matchService;
        _categoryService = categoryService;
        _cartService = cartService;
    }

    public PaginatedList<MatchViewModel>? Matches { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public Guid? CategoryId { get; set; }
    
    public string? CategoryName { get; set; }

    public async Task<IActionResult> OnGetAsync(int pageIndex = 1)
    {
        // Get category name if category filter is active
        if (CategoryId.HasValue)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var category = categories?.FirstOrDefault(c => c.Id == CategoryId);
            CategoryName = category?.Name;
        }

        // Get matches with filters
        var categoryIds = CategoryId.HasValue ? new List<Guid> { CategoryId.Value } : null;
        Matches = await _matchService.GetAllMatchesAsync(pageIndex, 9, SearchTerm, categoryIds);

        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid matchId)
    {
        await _cartService.AddToCartAsync(matchId, 1);
        
        // Redirect to the same page with the same filters
        return RedirectToPage(new { searchTerm = SearchTerm, categoryId = CategoryId });
    }
}