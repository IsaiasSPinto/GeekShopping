namespace GeekShopping.Web.Models;

public class CartViewModel
{
    public CartHeaderViewModel CartHeader { get; set; }
    public IEnumerable<CartDatailViewModel> CartDatails { get; set; }
}
