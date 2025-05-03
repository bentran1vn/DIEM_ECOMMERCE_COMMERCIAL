namespace DiemEcommerce.Web.Models.Cart;

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
    public int TotalItems { get; set; }
    public decimal TotalPrice { get; set; }
}

public class CartItemViewModel
{
    public Guid MatchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public Guid FactoryId { get; set; }
    public string FactoryName { get; set; } = string.Empty;
    public bool HasLimitedStock { get; set; }
    public decimal Total => Price * Quantity;
}

public class CartCheckoutViewModel
{
    public CartViewModel Cart { get; set; } = new CartViewModel();
    public CheckoutDetailsViewModel CheckoutDetails { get; set; } = new CheckoutDetailsViewModel();
}

public class CheckoutDetailsViewModel
{
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public bool IsQrPayment { get; set; }
}