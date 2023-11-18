using GeekShopping.CartApi.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartApi.Model;

[Table("cart_detail")]
public class CartDatail : BaseEntity
{
    public long CartHeaderId { get; set; }
    [ForeignKey("CartHeaderId")]
    public CartHeader CartHeader { get; set; }

    public long ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    [Column("count")]
    public int Count { get; set; }
}
