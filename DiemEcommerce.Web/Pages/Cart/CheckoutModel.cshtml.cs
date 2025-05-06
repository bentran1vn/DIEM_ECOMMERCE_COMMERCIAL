using System.ComponentModel.DataAnnotations;
using DiemEcommerce.Web.Models.Cart;
using DiemEcommerce.Web.Models.Order;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Cart;

[Authorize]
public class CheckoutModel : PageModel
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;

    public CheckoutModel(
        ICartService cartService,
        IOrderService orderService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    public CartViewModel Cart { get; set; } = new CartViewModel();

    [BindProperty]
    public CheckoutDetailsViewModel CheckoutDetails { get; set; } = new CheckoutDetailsViewModel();

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await _cartService.GetCartAsync();
        
        if (!Cart.Items.Any())
        {
            return RedirectToPage("/Cart/Index");
        }
        
        // Pre-fill with user's data from profile if available
        if (User.Identity?.IsAuthenticated == true)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                CheckoutDetails.Email = email;
            }
        }
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Cart = await _cartService.GetCartAsync();
        
        if (!Cart.Items.Any())
        {
            return RedirectToPage("/Cart/Index");
        }
        
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        // Create order request
        var orderRequest = new CreateOrderRequestModel
        {
            Address = CheckoutDetails.Address,
            Phone = CheckoutDetails.Phone,
            Email = CheckoutDetails.Email,
            Note = CheckoutDetails.Note,
            IsQR = CheckoutDetails.IsQrPayment,
            OrderItems = Cart.Items.Select(i => new OrderItemDto
            {
                MatchId = i.MatchId,
                Quantity = i.Quantity
            }).ToList()
        };
        
        // Submit order
        var result = await _orderService.CreateOrderAsync(orderRequest);
        
        if (result != null)
        {
            // Clear cart after successful order
            await _cartService.ClearCartAsync();
            
            // Redirect to order confirmation
            return RedirectToPage("/Orders/Confirmation", new { id = result.Id });
        }
        
        ModelState.AddModelError(string.Empty, "Failed to create order. Please try again.");
        return Page();
    }
}