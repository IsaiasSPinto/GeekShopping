namespace GeekShopping.CartApi.Data.Dtos;

public class CartDatailDto
{
    public long Id { get; set; }
    public long CartHeaderId { get; set; }
    public CartHeaderDto CartHeader { get; set; }
    public long ProductId { get; set; }
    public ProductDto Product { get; set; }
    public int Count { get; set; }
}
