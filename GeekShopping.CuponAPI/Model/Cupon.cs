using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GeekShopping.CuponAPI.Model.Base;

namespace GeekShopping.CuponAPI.Model;

[Table("cupon")]
public class Cupon : BaseEntity
{

    [Column("cuponCode")]
    [Required]
    [StringLength(30)]
    public string CuponCode { get; set; }

    [Column("discount_amount")]
    [Required]
    public decimal DiscountAmount { get; set; }

}
