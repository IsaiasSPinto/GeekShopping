
using GeekShopping.CartApi.Data.Dtos;

namespace GeekShopping.CartApi.Repository;

public interface ICuponRepository
{
    Task<CuponDto> GetCupon(string code, string token);
}
