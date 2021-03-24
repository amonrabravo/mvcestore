using Microsoft.EntityFrameworkCore;
using MvcEStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcEStoreData
{
    public class Product : BaseEntity
    {
        #region Properties

        public string Name { get; set; }

        public string Picture { get; set; }

        public string ProductCode { get; set; }

        public string Barcode { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public string Descriptions { get; set; }

        public int Reviews { get; set; }

        public int? BrandId { get; set; }

        #endregion

        #region Navigation

        public virtual Brand Brand { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

        public virtual ICollection<ProductPicture> ProductPictures { get; set; } = new HashSet<ProductPicture>();

        #endregion

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Product>(entity =>
            {
                entity
               .Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(250);

                entity
                .HasIndex(p => new { p.Name })
                .IsUnique();

                entity
                .HasIndex(p => new { p.Price })
                .IsUnique(false);

                entity
                .Property(p => p.Price)
                .HasPrecision(18, 4);

                entity
                .Property(p => p.Picture)
                .IsUnicode(false);

                entity
               .Property(p => p.ProductCode)
               .IsRequired();

                entity
               .Property(p => p.Barcode)
               .HasMaxLength(13)
               .IsUnicode(false)
               .IsFixedLength();

                entity
                .HasMany(p => p.OrderItems)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

                entity
                .HasMany(p => p.ProductPictures)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}