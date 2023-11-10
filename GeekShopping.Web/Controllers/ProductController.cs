using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var products = await _productService.FindAllProducts(token);

        return View(products);
    }


    public IActionResult CreateProduct()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProduct(ProductModel productModel)
    {
        if (!ModelState.IsValid)
        {
            return View(productModel);
        }

        var token = await HttpContext.GetTokenAsync("access_token");

        await _productService.CreateProduct(productModel, token);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _productService.FindProductById(id);

        return View(product);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> EditProduct(ProductModel productModel)
    {
        if (!ModelState.IsValid)
        {
            return View(productModel);
        }

        var token = await HttpContext.GetTokenAsync("access_token");

        await _productService.UpdateProduct(productModel, token);

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var product = await _productService.FindProductById(id, token);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> DeleteProduct(ProductModel productModel)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var response = await _productService.DeleteProduct(productModel.Id, token);

        if (!response)
        {
            return View(productModel);
        }

        return RedirectToAction(nameof(Index));
    }
}
