using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Order;

namespace DiemEcommerce.Web.Services.Interfaces;

public interface IOrderService
{
    Task<PaginatedList<OrderViewModel>?> GetCustomerOrdersAsync(int pageIndex = 1, int pageSize = 10, string? status = null);
    Task<OrderDetailViewModel?> GetOrderByIdAsync(Guid orderId);
    Task<CreateOrderResponseModel?> CreateOrderAsync(CreateOrderRequestModel model);
    Task<bool> CancelOrderAsync(Guid orderId, string? cancelReason = null);
}