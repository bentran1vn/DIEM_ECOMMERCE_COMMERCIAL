using DiemEcommerce.Web.Models.Order;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiemEcommerce.Web.Pages.Orders;

[Authorize]
public class ConfirmationModel : PageModel
{
    private readonly IOrderService _orderService;

    public ConfirmationModel(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public CreateOrderResponseModel? Order { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var orderDetail = await _orderService.GetOrderByIdAsync(id);
        
        if (orderDetail == null)
        {
            return Page();
        }
        
        // For confirmation page, we need to populate from the order details
        Order = new CreateOrderResponseModel
        {
            Id = orderDetail.Id,
            CustomerId = orderDetail.CustomerId,
            CustomerName = orderDetail.CustomerName,
            Address = orderDetail.Address,
            Phone = orderDetail.Phone,
            Email = orderDetail.Email,
            TotalPrice = orderDetail.TotalPrice,
            Status = orderDetail.Status,
            QrUrl = string.Empty, // Would be populated from the actual create order response
            SystemBankName = string.Empty, // Would be populated from the actual create order response
            SystemBankAccount = string.Empty, // Would be populated from the actual create order response
            SystemBankDescription = string.Empty, // Would be populated from the actual create order response
            CreatedOnUtc = orderDetail.CreatedOnUtc
        };
        
        // Check if user is authorized to view this order
        var customerId = User.FindFirst("CustomerId")?.Value;
        if (customerId != null && Guid.TryParse(customerId, out var userCustomerId))
        {
            if (orderDetail.CustomerId != userCustomerId)
            {
                return Forbid();
            }
        }
        
        return Page();
    }
}