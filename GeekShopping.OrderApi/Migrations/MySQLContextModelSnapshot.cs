﻿// <auto-generated />
using System;
using GeekShopping.OrderApi.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GeekShopping.OrderApi.Migrations
{
    [DbContext(typeof(MySQLContext))]
    partial class MySQLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GeekShopping.OrderApi.Model.OrderDatail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.Property<long>("OrderHeaderId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)")
                        .HasColumnName("price");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint")
                        .HasColumnName("ProductId");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("product_name");

                    b.HasKey("Id");

                    b.HasIndex("OrderHeaderId");

                    b.ToTable("order_detail");
                });

            modelBuilder.Entity("GeekShopping.OrderApi.Model.OrderHeader", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("cvv");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("card_number");

                    b.Property<int>("CartTotalItens")
                        .HasColumnType("int")
                        .HasColumnName("total_itens");

                    b.Property<string>("CuponCode")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("coupon_code");

                    b.Property<decimal>("DiscountTotal")
                        .HasColumnType("decimal(65,30)")
                        .HasColumnName("discount_total");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("email");

                    b.Property<string>("ExpiryMonthYear")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("expiry_month_year");

                    b.Property<string>("FristName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("last_name");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("order_time");

                    b.Property<bool>("PaymentStatus")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("payment_status");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("phone");

                    b.Property<decimal>("PurchaseAmount")
                        .HasColumnType("decimal(65,30)")
                        .HasColumnName("purchase_amount");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("purchase_date");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("order_header");
                });

            modelBuilder.Entity("GeekShopping.OrderApi.Model.OrderDatail", b =>
                {
                    b.HasOne("GeekShopping.OrderApi.Model.OrderHeader", "OrderHeader")
                        .WithMany("CartDetails")
                        .HasForeignKey("OrderHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderHeader");
                });

            modelBuilder.Entity("GeekShopping.OrderApi.Model.OrderHeader", b =>
                {
                    b.Navigation("CartDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
