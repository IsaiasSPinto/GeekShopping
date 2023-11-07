using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
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
        var products = await _productService.FindAllProducts();

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

        await _productService.CreateProduct(productModel);

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

        await _productService.UpdateProduct(productModel);

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _productService.FindProductById(id);
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
        var response = await _productService.DeleteProduct(productModel.Id);

        if (!response)
        {
            return View(productModel);
        }

        return RedirectToAction(nameof(Index));
    }
}
