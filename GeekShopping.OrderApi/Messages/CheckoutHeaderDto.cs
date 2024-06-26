﻿using GeekShopping.MessageBus;

namespace GeekShopping.OrderApi.Messages;

public class CheckoutHeaderDto : BaseMessage
{
    public string UserId { get; set; }
    public string CuponCode { get; set; } = "";
    public decimal PurchaseAmount { get; set; }
    public decimal DiscountTotal { get; set; }
    public string FristName { get; set; }
    public string LastName { get; set; }
    public DateTime Time { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string CardNumber { get; set; }
    public string CVV { get; set; }
    public string ExpireMountYear { get; set; }

    public int CartTotalItens { get; set; }
    public IEnumerable<CartDatailDto>? CartDetails { get; set; } = new List<CartDatailDto>();
}



