using GeekShopping.ProductAPI.Data.Dtos;
using GeekShopping.ProductAPI.Repository;
using GeekShopping.ProductAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }


    [HttpGet]
    public async Task<IActionResult> FindAll()
    {
        var products = await _productRepository.FindAll();

        return Ok(products);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> FindById(long id)
    {
        var product = await _productRepository.FindById(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(ProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var product = await _productRepository.Create(dto);
        return Ok(product);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update(ProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var product = await _productRepository.Update(dto);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> Delete(long id)
    {
        var product = await _productRepository.Delete(id);

        if (!product)
        {
            return BadRequest();
        }

        return NoContent();
    }
}