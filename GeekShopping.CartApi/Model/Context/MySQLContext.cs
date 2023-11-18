using GeekShopping.CartApi.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartApi.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartDatail> CartDatails { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
    }
}
