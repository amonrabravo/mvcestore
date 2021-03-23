using Microsoft.EntityFrameworkCore;
using MvcEStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcEStoreData
{
    public class Brand : BaseEntity
    {
        #region Properties

        public string Name { get; set; }

        public string Picture { get; set; }

        public int SortOrder { get; set; }

        #endregion

        #region Navigation

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

        #endregion
        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Brand>(entity =>
            {

                entity
                .Property(p => p.Picture)
                .IsRequired()
                .IsUnicode(false);

                entity
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

                entity
                .HasIndex(p => p.Name)
                .IsUnique();

                entity
                .HasMany(p => p.Products)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.SetNull);


            });

        }

    }
}