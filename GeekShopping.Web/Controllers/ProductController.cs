using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.FindAllProducts();

        return View(products);
    }

    public async Task<IActionResult> CreateProduct()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductModel productModel)
    {
        if (!ModelState.IsValid)
        {
            return View(productModel);
        }

        await _productService.CreateProduct(productModel);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EditProduct()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(ProductModel productModel)
    {
        if (!ModelState.IsValid)
        {
            return View(productModel);
        }

        await _productService.UpdateProduct(productModel);

        return RedirectToAction(nameof(Index));
    }

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
