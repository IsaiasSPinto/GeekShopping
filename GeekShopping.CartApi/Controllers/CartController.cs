using GeekShopping.CartApi.Data.Dtos;
using GeekShopping.CartApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }


        [HttpGet("find-cart/{id}")]
        public async Task<IActionResult> FindById(string userId)
        {
            var cart = await _cartRepository.FindCartByUserId(userId);

            if (cart is null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<IActionResult> AddCart(CartDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var cart = await _cartRepository.SaveOurUpdateCart(dto);
            return Ok(cart);
        }

        [HttpPut("update-cart")]
        public async Task<IActionResult> UpdateCart(CartDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var cart = await _cartRepository.SaveOurUpdateCart(dto);
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<IActionResult> RemoveCart(int id)
        {
            var status = await _cartRepository.RemoveProductFromCart(id);

            if (!status)
            {
                return BadRequest();
            }

            return Ok(status);
        }
    }
}