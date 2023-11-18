using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices;

public interface ICartService
{
    Task<CartViewModel> FindCartByUserId(string userId, string token);
    Task<CartViewModel> AddItemToCart(CartViewModel cart, string token);
    Task<CartViewModel> UpdateCart(CartViewModel cart, string token);
    Task<bool> RemoveFromCart(long cartId, string token);
    Task<bool> ApplyCupom(CartViewModel cart, string cuponCode, string token);
    Task<bool> RemoveCupom(string userId, string token);
    Task<bool> ClearCart(string userId, string token);
    Task<CartViewModel> Checkout(CartHeaderViewModel cartHeader, string token);
}
