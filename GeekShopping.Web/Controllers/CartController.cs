using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICuponService _cuponService;
    public CartController(ICartService cartService, ICuponService cuponService)
    {
        _cartService = cartService;
        _cuponService = cuponService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var cart = await FindUserCart();

        return View(cart);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Checkout()
    {
        var cart = await FindUserCart();

        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Checkout(CartViewModel model)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        try
        {
            var response = await _cartService.Checkout(model.CartHeader, token);
            if (response == null)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Confirmation), new { id = response.Id });
        }
        catch (CuponInvalidException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Checkout));
        }
        catch (Exception)
        {

            return View(model);            
        }

      
    }

    [HttpGet]
    public async Task<IActionResult> Confirmation()
    {
        return View();
    }

    [Authorize]
    [ActionName("ApplyCupon")]
    public async Task<IActionResult> ApplyCupon(CartViewModel model)
    {

        var token = await HttpContext.GetTokenAsync("access_token");
        var userId = User.Claims.Where(u => u.Type == "sub").FirstOrDefault().Value;

        var cart = await _cartService.FindCartByUserId(userId, token);

        cart.CartHeader.CuponCode = model.CartHeader.CuponCode;

        var response = await _cartService.ApplyCupom(cart, token);

        if (!response)
        {
            return View();
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [ActionName("RemoveCupon")]
    public async Task<IActionResult> RemoveCupon()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var userId = User.Claims.Where(u => u.Type == "sub").FirstOrDefault().Value;

        var response = await _cartService.RemoveCupom(userId, token);

        if (!response)
        {
            return View();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Remove(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var userId = User.Claims.Where(u => u.Type == "sub").FirstOrDefault().Value;

        var response = await _cartService.RemoveFromCart(id, token);

        if (!response)
        {
            return View();
        }

        return RedirectToAction(nameof(Index));

    }


    private async Task<CartViewModel> FindUserCart()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var userId = User.Claims.Where(u => u.Type == "sub").FirstOrDefault().Value;

        var cart = await _cartService.FindCartByUserId(userId, token);

        if (cart == null || cart.CartDatails == null)
        {
            return new CartViewModel();
        }

        if (!String.IsNullOrEmpty(cart.CartHeader.CuponCode))
        {
            var cupon = await _cuponService.GetCupon(cart.CartHeader.CuponCode, token);
            if (cupon != null)
            {
                cart.CartHeader.DiscountTotal = cupon.DiscountAmount;
            }
        }

        foreach (var detail in cart.CartDatails)
        {
            cart.CartHeader.PurchaseAmount += (detail.Product.Price * detail.Count);
        }

        cart.CartHeader.PurchaseAmount -= cart.CartHeader.DiscountTotal;

        return cart;
    }
}
