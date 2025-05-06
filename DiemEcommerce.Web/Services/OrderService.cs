using DiemEcommerce.Web.Helpers;
using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Order;
using DiemEcommerce.Web.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace DiemEcommerce.Web.Services;

public class OrderService : IOrderService
{
    private readonly ApiHelper _apiHelper;

    public OrderService(
        HttpClient httpClient, 
        IHttpContextAccessor httpContextAccessor,
        IOptions<ApiOptions> apiOptions)
    {
        _apiHelper = new ApiHelper(httpClient, httpContextAccessor, apiOptions);
    }

    public async Task<PaginatedList<OrderViewModel>?> GetCustomerOrdersAsync(int pageIndex = 1, int pageSize = 10, string? status = null)
    {
        // Build query parameters
        var queryParams = new List<string>
        {
            $"pageIndex={pageIndex}",
            $"pageSize={pageSize}"
        };
        
        if (!string.IsNullOrWhiteSpace(status))
        {
            queryParams.Add($"status={status}");
        }
        
        string endpoint = $"orders?{string.Join("&", queryParams)}";
        
        var result = await _apiHelper.GetAsync<ApiResponse<PaginatedList<OrderViewModel>>>(endpoint);

        return result.Value;
    }

    public async Task<OrderDetailViewModel?> GetOrderByIdAsync(Guid orderId)
    {
        return await _apiHelper.GetAsync<OrderDetailViewModel>($"orders/{orderId}");
    }

    public async Task<CreateOrderResponseModel?> CreateOrderAsync(CreateOrderRequestModel model)
    {
        return await _apiHelper.PostAsync<CreateOrderRequestModel, CreateOrderResponseModel>("orders", model);
    }

    public async Task<bool> CancelOrderAsync(Guid orderId, string? cancelReason = null)
    {
        var request = new CancelOrderRequestModel
        {
            OrderId = orderId,
            CancelReason = cancelReason
        };
        
        await _apiHelper.DeleteAsync($"orders/{orderId}");
        return true;
    }
}