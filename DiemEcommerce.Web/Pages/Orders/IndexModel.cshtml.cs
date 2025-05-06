using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Order;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Orders;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IOrderService _orderService;

    public IndexModel(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public PaginatedList<OrderViewModel>? Orders { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? SelectedStatus { get; set; }

    public async Task<IActionResult> OnGetAsync(int pageIndex = 1)
    {
        Orders = await _orderService.GetCustomerOrdersAsync(pageIndex, 10, SelectedStatus);
        return Page();
    }

    public async Task<IActionResult> OnPostCancelOrderAsync(Guid orderId)
    {
        var result = await _orderService.CancelOrderAsync(orderId);
        
        return RedirectToPage();
    }
}