using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderApi.Model.Context;

public class MySQLContext : DbContext
{
    public MySQLContext(DbContextOptions<MySQLContext> options) : base(options)
    {
    }

    public DbSet<OrderDatail> OrderDatails { get; set; }
    public DbSet<OrderHeader> OrderHeaders { get; set; }
}
