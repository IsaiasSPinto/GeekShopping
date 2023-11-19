using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CuponAPI.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options)
        {
        }

        public DbSet<Cupon> Cupon { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cupon>().HasData(
                new Cupon
                {
                    Id = 1,
                    CuponCode = "GEEK10",
                    DiscountAmount = 10,
                },
                new Cupon
                {
                    Id = 2,
                    CuponCode = "GEEK20",
                    DiscountAmount = 20,
                }
            );
        }
    }
}
