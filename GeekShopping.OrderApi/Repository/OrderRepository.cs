using GeekShopping.OrderApi.Model;
using GeekShopping.OrderApi.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderApi.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly DbContextOptions<MySQLContext> _context;
    public OrderRepository(DbContextOptions<MySQLContext> context)
    {
        _context = context;        
    }

    public async Task<bool> AddOrder(OrderHeader header)
    {
        await using var _db = new MySQLContext(_context);

        await _db.OrderHeaders.AddAsync(header);

        return await _db.SaveChangesAsync() > 0;
    }

    public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid)
    {
        await using var _db = new MySQLContext(_context);

        var order = await _db.OrderHeaders.FindAsync(orderHeaderId);

        if (order != null)
        {
            order.PaymentStatus = paid;
            await _db.SaveChangesAsync();
        }
        
    }
}

