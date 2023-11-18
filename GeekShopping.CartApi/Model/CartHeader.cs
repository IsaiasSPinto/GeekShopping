﻿using GeekShopping.CartApi.Model.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartApi.Model;

[Table("cart_header")]
public class CartHeader : BaseEntity
{
    [Column("user_id")]
    public string UserId { get; set; }
    [Column("coupon_code")]
    public string CuponCode { get; set; }
}
