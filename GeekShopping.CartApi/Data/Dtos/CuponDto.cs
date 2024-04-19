namespace GeekShopping.CartApi.Data.Dtos;

public class CuponDto
{
    public long Id { get; set; }
    public string CuponCode { get; set; }
    public decimal DiscountAmount { get; set; }

}
