using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiemEcommerce.Web.Pages.Shared.Components.CartSummary;

public class CartSummaryViewComponent : ViewComponent
{
    private readonly ICartService _cartService;

    public CartSummaryViewComponent(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var cart = await _cartService.GetCartAsync();
        return View(cart);
    }
}