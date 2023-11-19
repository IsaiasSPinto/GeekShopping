using GeekShopping.CuponAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CuponAPI.Model;

[Table("cart_detail")]
public class CartDatail : BaseEntity
{
    public long CartHeaderId { get; set; }
    [ForeignKey("CartHeaderId")]
    public CartHeader CartHeader { get; set; }

    public long ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Cupon Product { get; set; }
    [Column("count")]
    public int Count { get; set; }
}
