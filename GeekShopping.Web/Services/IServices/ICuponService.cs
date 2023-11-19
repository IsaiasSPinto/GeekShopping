using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices;

public interface ICuponService
{
    Task<CuponViewModel> GetCupon(string cuponCode, string token);
}
