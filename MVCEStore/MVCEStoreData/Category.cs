using Microsoft.EntityFrameworkCore;
using MvcEStoreData.Infrastructure;
using System.Collections.Generic;

namespace MvcEStoreData
{
    public class Category : BaseEntity
    {
        #region Properties

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public int RayonId { get; set; }

        #endregion

        #region Navigation

        public virtual Rayon Rayon { get; set; }

        public virtual ICollection<Banner> Banners { get; set; } = new HashSet<Banner>();

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; } = new HashSet<CategoryProduct>();


        #endregion

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Category>(entity =>
            {

                entity
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

                entity
                .HasIndex(p => new { p.Name, p.RayonId })
                .IsUnique();

                entity
                .HasMany(p => p.Banners)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);


            });
        }
    }
}