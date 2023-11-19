using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GeekShopping.CuponAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedCupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "cupon",
                columns: new[] { "id", "cuponCode", "discount_amount" },
                values: new object[,]
                {
                    { 1L, "GEEK10", 10m },
                    { 2L, "GEEK20", 20m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "cupon",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "cupon",
                keyColumn: "id",
                keyValue: 2L);
        }
    }
}
