using DiemEcommerce.Web.Models.Cart;
using DiemEcommerce.Web.Services.Interfaces;
using Newtonsoft.Json;

namespace DiemEcommerce.Web.Services;

public class CartService : ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMatchService _matchService;
    private const string CartSessionKey = "Cart";

    public CartService(IHttpContextAccessor httpContextAccessor, IMatchService matchService)
    {
        _httpContextAccessor = httpContextAccessor;
        _matchService = matchService;
    }

    public async Task<CartViewModel> GetCartAsync()
    {
        var cart = GetCartFromSession();
        
        // Update cart with current match information
        await UpdateCartWithMatchDetails(cart);
        
        return cart;
    }

    public async Task<CartViewModel> AddToCartAsync(Guid matchId, int quantity)
    {
        var cart = GetCartFromSession();
        
        var existingItem = cart.Items.FirstOrDefault(i => i.MatchId == matchId);
        
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var match = await _matchService.GetMatchByIdAsync(matchId);
            
            if (match != null)
            {
                cart.Items.Add(new CartItemViewModel
                {
                    MatchId = matchId,
                    Quantity = quantity,
                    Name = match.Name,
                    Price = match.Price,
                    ImageUrl = match.CoverImages.FirstOrDefault()?.Url ?? string.Empty,
                    FactoryId = match.FactoryId,
                    FactoryName = match.FactoryName
                });
            }
        }
        
        // Calculate total
        CalculateCartTotal(cart);
        
        // Save updated cart to session
        SaveCartToSession(cart);
        
        return cart;
    }

    public async Task<CartViewModel> UpdateCartItemAsync(Guid matchId, int quantity)
    {
        var cart = GetCartFromSession();
        
        var existingItem = cart.Items.FirstOrDefault(i => i.MatchId == matchId);
        
        if (existingItem != null)
        {
            if (quantity <= 0)
            {
                cart.Items.Remove(existingItem);
            }
            else
            {
                existingItem.Quantity = quantity;
            }
            
            // Calculate total
            CalculateCartTotal(cart);
            
            // Save updated cart to session
            SaveCartToSession(cart);
        }
        
        return cart;
    }

    public async Task<CartViewModel> RemoveFromCartAsync(Guid matchId)
    {
        var cart = GetCartFromSession();
        
        var itemToRemove = cart.Items.FirstOrDefault(i => i.MatchId == matchId);
        
        if (itemToRemove != null)
        {
            cart.Items.Remove(itemToRemove);
            
            // Calculate total
            CalculateCartTotal(cart);
            
            // Save updated cart to session
            SaveCartToSession(cart);
        }
        
        return cart;
    }

    public async Task ClearCartAsync()
    {
        _httpContextAccessor.HttpContext?.Session.Remove(CartSessionKey);
    }

    private CartViewModel GetCartFromSession()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var cartJson = session?.GetString(CartSessionKey);
        
        if (string.IsNullOrEmpty(cartJson))
        {
            return new CartViewModel();
        }
        
        return JsonConvert.DeserializeObject<CartViewModel>(cartJson) ?? new CartViewModel();
    }

    private void SaveCartToSession(CartViewModel cart)
    {
        var cartJson = JsonConvert.SerializeObject(cart);
        _httpContextAccessor.HttpContext?.Session.SetString(CartSessionKey, cartJson);
    }

    private void CalculateCartTotal(CartViewModel cart)
    {
        cart.TotalItems = cart.Items.Sum(i => i.Quantity);
        cart.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
    }

    private async Task UpdateCartWithMatchDetails(CartViewModel cart)
    {
        // For each item, fetch the latest match details
        foreach (var item in cart.Items.ToList())
        {
            var match = await _matchService.GetMatchByIdAsync(item.MatchId);
            
            if (match != null)
            {
                // Update with current price and details
                item.Name = match.Name;
                item.Price = match.Price;
                item.ImageUrl = match.CoverImages.FirstOrDefault()?.Url ?? string.Empty;
                item.FactoryId = match.FactoryId;
                item.FactoryName = match.FactoryName;
                
                // Check if the requested quantity is still available
                if (item.Quantity > match.Quantity)
                {
                    item.Quantity = match.Quantity;
                    item.HasLimitedStock = true;
                }
            }
            else
            {
                // Remove item if match no longer exists
                cart.Items.Remove(item);
            }
        }
        
        // Recalculate totals
        CalculateCartTotal(cart);
        
        // Save updated cart back to session
        SaveCartToSession(cart);
    }
}