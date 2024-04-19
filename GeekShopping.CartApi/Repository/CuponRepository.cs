using GeekShopping.CartApi.Data.Dtos;
using System.Net.Http.Headers;

namespace GeekShopping.CartApi.Repository;

public class CuponRepository : ICuponRepository
{
    private readonly HttpClient _client;
    public CuponRepository(HttpClient client)
    {
        _client = client;
        
    }
    public async Task<CuponDto> GetCupon(string code, string token)
    {

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetFromJsonAsync<CuponDto>($"/api/v1/cupon/{code}");

        if (response is null)
        {
            return new CuponDto();
        }

        return response;
    }
}
