﻿namespace GeekShopping.CartApi.Model;

public class Cart
{
    public CartHeader CartHeader { get; set; }
    public IEnumerable<CartDatail> CartDatails { get; set; }
}
