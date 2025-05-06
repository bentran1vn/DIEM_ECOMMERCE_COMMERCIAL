using DiemEcommerce.Web.Models.Cart;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Cart;

public class IndexModel : PageModel
{
    private readonly ICartService _cartService;

    public IndexModel(ICartService cartService)
    {
        _cartService = cartService;
    }

    public CartViewModel Cart { get; set; } = new CartViewModel();

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await _cartService.GetCartAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostIncrementQuantityAsync(Guid matchId)
    {
        var cart = await _cartService.GetCartAsync();
        var item = cart.Items.FirstOrDefault(i => i.MatchId == matchId);
        
        if (item != null)
        {
            await _cartService.UpdateCartItemAsync(matchId, item.Quantity + 1);
        }
        
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDecrementQuantityAsync(Guid matchId)
    {
        var cart = await _cartService.GetCartAsync();
        var item = cart.Items.FirstOrDefault(i => i.MatchId == matchId);
        
        if (item != null && item.Quantity > 1)
        {
            await _cartService.UpdateCartItemAsync(matchId, item.Quantity - 1);
        }
        
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveItemAsync(Guid matchId)
    {
        await _cartService.RemoveFromCartAsync(matchId);
        return RedirectToPage();
    }
}