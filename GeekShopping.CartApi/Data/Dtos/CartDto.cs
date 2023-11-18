namespace GeekShopping.CartApi.Data.Dtos;

public class CartDto
{
    public CartHeaderDto CartHeader { get; set; }
    public IEnumerable<CartDatailDto> CartDatails { get; set; }
}
