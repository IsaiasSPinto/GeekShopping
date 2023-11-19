using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GeekShopping.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        var productList = await _productService.FindAllProducts("");

        return View("Index", productList);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var model = await _productService.FindProductById(id, token);

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DetailsPost(ProductViewModel model)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var cartViewModel = new CartViewModel()
        {
            CartHeader = new CartHeaderViewModel()
            {
                UserId = User.Claims.Where(u => u.Type == "sub").FirstOrDefault().Value
            },
        };

        var cartDetails = new CartDatailViewModel()
        {
            Count = model.Count,
            ProductId = model.Id,
            Product = await _productService.FindProductById(model.Id, token),
            CartHeader = cartViewModel.CartHeader
        };

        List<CartDatailViewModel> cartDetailsList = new List<CartDatailViewModel>();
        cartDetailsList.Add(cartDetails);

        cartViewModel.CartDatails = cartDetailsList;

        await _cartService.AddItemToCart(cartViewModel, token);
        return RedirectToAction(nameof(Details));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        return RedirectToAction(nameof(Index));
    }

}