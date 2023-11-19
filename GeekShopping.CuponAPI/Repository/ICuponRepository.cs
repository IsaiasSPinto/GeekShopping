using GeekShopping.CuponAPI.Data.Dtos;

namespace GeekShopping.CuponAPI.Repository;

public interface ICuponRepository
{
    Task<CuponDto> GetCupon(string code);
}
