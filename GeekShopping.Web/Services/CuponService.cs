using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services;

public class CuponService : ICuponService
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/cupon";
    public CuponService(HttpClient client)
    {
        _client = client;
    }

    public async Task<CuponViewModel> GetCupon(string cuponCode, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _client.GetAsync($"{BasePath}/{cuponCode}");

        return await response.ReadContentAs<CuponViewModel>();
    }
}
