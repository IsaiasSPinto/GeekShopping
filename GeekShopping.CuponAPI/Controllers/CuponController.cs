using GeekShopping.CuponAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CuponAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CuponController : ControllerBase
{
    private readonly ICuponRepository _cuponRepository;
    public CuponController(ICuponRepository cuponRepository)
    {
        _cuponRepository = cuponRepository;
    }

    [HttpGet("{cuponCode}")]
    [Authorize]
    public async Task<IActionResult> GetCuponByCuponCode(string cuponCode)
    {
        var cupon = await _cuponRepository.GetCupon(cuponCode);

        if (cupon == null)
        {
            return NotFound();
        }
        return Ok(cupon);
    }
}