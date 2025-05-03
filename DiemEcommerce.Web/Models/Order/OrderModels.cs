namespace DiemEcommerce.Web.Models.Order;

public class OrderViewModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public decimal TotalPrice { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset CreatedOnUtc { get; set; }
}

public class OrderDetailViewModel : OrderViewModel
{
    public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
}

public class OrderItemViewModel
{
    public Guid Id { get; set; }
    public Guid MatchId { get; set; }
    public string MatchName { get; set; } = string.Empty;
    public string MatchImageUrl { get; set; } = string.Empty;
    public Guid FactoryId { get; set; }
    public string FactoryName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
    public bool HasFeedback { get; set; }
}

public class OrderItemDto
{
    public Guid MatchId { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderRequestModel
{
    public string? Note { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsQR { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}

public class CreateOrderResponseModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string QrUrl { get; set; } = string.Empty;
    public string SystemBankName { get; set; } = string.Empty;
    public string SystemBankAccount { get; set; } = string.Empty;
    public string SystemBankDescription { get; set; } = string.Empty;
    public DateTimeOffset CreatedOnUtc { get; set; }
}

public class CancelOrderRequestModel
{
    public Guid OrderId { get; set; }
    public string? CancelReason { get; set; }
}