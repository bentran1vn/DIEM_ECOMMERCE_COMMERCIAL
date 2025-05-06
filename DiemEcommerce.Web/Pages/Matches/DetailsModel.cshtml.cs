using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Feedback;
using DiemEcommerce.Web.Models.Match;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Matches;

public class DetailsModel : PageModel
{
    private readonly IMatchService _matchService;
    private readonly IFeedbackService _feedbackService;
    private readonly ICartService _cartService;

    public DetailsModel(
        IMatchService matchService,
        IFeedbackService feedbackService,
        ICartService cartService)
    {
        _matchService = matchService;
        _feedbackService = feedbackService;
        _cartService = cartService;
    }

    public MatchDetailViewModel? Match { get; set; }
    public PaginatedList<FeedbackViewModel>? Feedbacks { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id, int pageIndex = 1)
    {
        Match = await _matchService.GetMatchByIdAsync(id);
        
        if (Match == null)
        {
            return Page();
        }

        // Get feedback for this match
        Feedbacks = await _feedbackService.GetFeedbackByProductAsync(id, pageIndex);
        
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid matchId, int quantity = 1)
    {
        await _cartService.AddToCartAsync(matchId, quantity);
        return RedirectToPage("./Details", new { id = matchId });
    }
}