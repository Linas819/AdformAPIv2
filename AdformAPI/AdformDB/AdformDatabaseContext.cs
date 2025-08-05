using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdformAPI.AdformDB;

public partial class AdformDatabaseContext : DbContext
{
    public AdformDatabaseContext()
    {
    }

    public AdformDatabaseContext(DbContextOptions<AdformDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderLine> OrderLines { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("discounts_pkey");

            entity.ToTable("discounts");

            entity.HasIndex(e => e.ProductId, "discount_product_id_idx");

            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
            entity.Property(e => e.MinimalQuantity).HasColumnName("minimal_quantity");
            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_product");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderName)
                .HasMaxLength(50)
                .HasColumnName("order_name");
        });

        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasKey(e => e.OrderLineId).HasName("orderlines_pkey");

            entity.ToTable("order_lines");

            entity.HasIndex(e => e.OrderId, "orderlines_order_id_idx");

            entity.HasIndex(e => e.ProductId, "orderlines_product_id_idx");

            entity.Property(e => e.OrderLineId)
                .HasDefaultValueSql("nextval('orderlines_orderline_id_seq'::regclass)")
                .HasColumnName("order_line_id");
            entity.Property(e => e.OrderId)
                .HasDefaultValueSql("nextval('orderlines_order_id_seq'::regclass)")
                .HasColumnName("order_id");
            entity.Property(e => e.ProductId)
                .HasDefaultValueSql("nextval('orderlines_product_id_seq'::regclass)")
                .HasColumnName("product_id");
            entity.Property(e => e.ProductQuantity).HasColumnName("product_quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .HasColumnName("product_name");
            entity.Property(e => e.ProductPrice).HasColumnName("product_price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
