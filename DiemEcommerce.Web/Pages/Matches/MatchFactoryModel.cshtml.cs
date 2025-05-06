using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Factory;
using DiemEcommerce.Web.Models.Match;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Matches;

public class MatchFactoryModel : PageModel
{
    private readonly IMatchService _matchService;
    private readonly IFactoryService _factoryService;

    public MatchFactoryModel(IMatchService matchService, IFactoryService factoryService)
    {
        _matchService = matchService;
        _factoryService = factoryService;
    }
    
    public PaginatedList<MatchViewModel>? Matches { get; set; }
    public FactoryViewModel? Factory { get; set; }
    

    public async Task<IActionResult> OnGetAsync(Guid id, int pageIndex = 1)
    {
        Factory = await _factoryService.GetFactoryByIdAsync(id);
        
        Matches = await _matchService.GetMatchesByFactoryIdAsync(id);
        
        if (Factory == null)
        {
            return NotFound();
        }
        
        return Page();
    }

}