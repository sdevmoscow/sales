using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sales.DataContext
{
    public partial class OrdersContext : DbContext
    {
        public OrdersContext()
        {
        }

        public OrdersContext(DbContextOptions<OrdersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<SalesOrder> SalesOrder { get; set; }
        public virtual DbSet<SalesOrderDetail> SalesOrderDetail { get; set; }
        public virtual DbSet<SalesStatus> SalesStatus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=ms-sql-9.in-solve.ru;Database=1gb_academics;Trusted_Connection=False;User Id=1gb_elizarovsa;Password=za5d8f88aa");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(2000);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(2000);

                entity.Property(e => e.ListPrice).HasColumnType("money");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(2000);
            });

            modelBuilder.Entity<SalesOrder>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(2000);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SalesOrder)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesOrder_Customer");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.SalesOrder)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesOrder_SalesStatus");
            });

            modelBuilder.Entity<SalesOrderDetail>(entity =>
            {
                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SalesOrderDetail)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesOrderDetail_Product");

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.SalesOrderDetail)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesOrderDetail_SalesOrder");
            });

            modelBuilder.Entity<SalesStatus>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(2000);
            });
        }
    }
}
