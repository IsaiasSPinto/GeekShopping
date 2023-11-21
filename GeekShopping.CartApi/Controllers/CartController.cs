using GeekShopping.CartApi.Data.Dtos;
using GeekShopping.CartApi.Mensages;
using GeekShopping.CartApi.RabbitMQSender;
using GeekShopping.CartApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IRabbitMQMessageSender _rabbitMQ;

        public CartController(ICartRepository cartRepository, IRabbitMQMessageSender rabbitMQ)
        {
            _cartRepository = cartRepository;
            _rabbitMQ = rabbitMQ;
        }


        [HttpGet("find-cart/{id}")]
        public async Task<IActionResult> FindById(string id)
        {
            var cart = await _cartRepository.FindCartByUserId(id);

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

        [HttpPost("apply-cupon")]
        public async Task<IActionResult> ApplyCupon(CartDto dto)
        {
            var status = await _cartRepository.ApplyCupon(dto.CartHeader.UserId, dto.CartHeader.CuponCode);
            if (!status)
            {
                return BadRequest();
            }

            return Ok(status);
        }

        [HttpDelete("remove-cupon/{userId}")]
        public async Task<IActionResult> RemoveCupon(string userId)
        {
            var status = await _cartRepository.RemoveCupon(userId);
            if (!status)
            {
                return BadRequest();
            }

            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderDto>> Checkout(CheckoutHeaderDto dto)
        {
            if (dto.UserId == null) return BadRequest();

            var cart = await _cartRepository.FindCartByUserId(dto.UserId);
            if (cart == null)
            {
                return NotFound();
            }

            dto.CartDetails = cart.CartDatails;
            dto.Time = DateTime.Now;

            _rabbitMQ.SendMessage(dto, "checkoutqueue");

            return Ok(dto);
        }
    }
}