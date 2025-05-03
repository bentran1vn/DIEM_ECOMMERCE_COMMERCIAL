using DiemEcommerce.Web.Models.Factory;
using DiemEcommerce.Web.Models.Match;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IMatchService _matchService;
    private readonly IFactoryService _factoryService;
    private readonly ICartService _cartService;

    public IndexModel(
        IMatchService matchService, 
        IFactoryService factoryService,
        ICartService cartService)
    {
        _matchService = matchService;
        _factoryService = factoryService;
        _cartService = cartService;
    }

    public List<MatchViewModel> FeaturedMatches { get; set; } = new List<MatchViewModel>();
    public List<FactoryViewModel> FeaturedFactories { get; set; } = new List<FactoryViewModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        // Get featured matches (just getting the latest ones for now)
        var matches = await _matchService.GetAllMatchesAsync(pageIndex: 1, pageSize: 4);
        if (matches != null)
        {
            FeaturedMatches = matches.Items;
        }

        // Get featured factories (just getting the latest ones for now)
        var factories = await _factoryService.GetAllFactoriesAsync(pageIndex: 1, pageSize: 3);
        if (factories != null)
        {
            FeaturedFactories = factories.Items;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid matchId)
    {
        await _cartService.AddToCartAsync(matchId, 1);
        return RedirectToPage();
    }
}