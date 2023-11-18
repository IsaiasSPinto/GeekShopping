using GeekShopping.CartApi.Data.Dtos;

namespace GeekShopping.CartApi.Repository;

public interface ICartRepository
{
    Task<CartDto> FindCartByUserId(string userId);
    Task<CartDto> SaveOurUpdateCart(CartDto cart);
    Task<bool> RemoveProductFromCart(long cartDetailsId);
    Task<bool> ApplyCupon(string userId, string couponCode);
    Task<bool> RemoveCupon(string userId);
    Task<bool> ClearCart(string userId);
}
