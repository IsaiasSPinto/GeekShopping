using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services;

public class CartService : ICartService
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/cart";

    public CartService(HttpClient client)
    {
        _client = client;
    }
    public async Task<CartViewModel> FindCartByUserId(string userId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _client.GetAsync($"{BasePath}/find-cart/{userId}");

        return await response.ReadContentAs<CartViewModel>();
    }

    public async Task<CartViewModel> AddItemToCart(CartViewModel cart, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _client.PostAsJson($"{BasePath}/add-cart", cart);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong when calling API");
        }

        return await response.ReadContentAs<CartViewModel>();
    }

    public async Task<CartViewModel> UpdateCart(CartViewModel cart, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _client.PutAsJsonAsync($"{BasePath}/update-cart", cart);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong when calling API");
        }

        return await response.ReadContentAs<CartViewModel>();
    }

    public async Task<bool> RemoveFromCart(long cartId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        var response = await _client.DeleteAsync($"{BasePath}/remove-cart/{cartId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong when calling API");
        }

        return true;
    }
    public async Task<CartHeaderViewModel> Checkout(CartHeaderViewModel cartHeader, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        var response = await _client.PostAsJson($"{BasePath}/checkout", cartHeader);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong when calling API");
        }

        return await response.ReadContentAs<CartHeaderViewModel>();
    }
    public async Task<bool> ClearCart(string userId, string token)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ApplyCupom(CartViewModel cart, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        var response = await _client.PostAsJson($"{BasePath}/apply-cupon", cart);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong when calling API");
        }

        return true;
    }

    public async Task<bool> RemoveCupom(string userId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        var response = await _client.DeleteAsync($"{BasePath}/remove-cupon/{userId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong when calling API");
        }

        return true;
    }
}
