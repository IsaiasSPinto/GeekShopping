namespace GeekShopping.OrderApi.Messages;

public class UpdatePaymentResultDto
{
    public long OrderId { get; set; }
    public bool Status { get; set; }
    public string Email { get; set; }
}

