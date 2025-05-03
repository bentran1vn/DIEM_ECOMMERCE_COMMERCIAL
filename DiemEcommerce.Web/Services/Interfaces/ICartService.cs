using DiemEcommerce.Web.Models.Cart;

namespace DiemEcommerce.Web.Services.Interfaces;

public interface ICartService
{
    Task<CartViewModel> GetCartAsync();
    Task<CartViewModel> AddToCartAsync(Guid matchId, int quantity);
    Task<CartViewModel> UpdateCartItemAsync(Guid matchId, int quantity);
    Task<CartViewModel> RemoveFromCartAsync(Guid matchId);
    Task ClearCartAsync();
}