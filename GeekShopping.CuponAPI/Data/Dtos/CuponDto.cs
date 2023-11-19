using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeekShopping.CuponAPI.Data.Dtos;

public class CuponDto
{
    public long Id { get; set; }
    public string CuponCode { get; set; }
    public decimal DiscountAmount { get; set; }

}
