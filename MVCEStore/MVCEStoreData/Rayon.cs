using Microsoft.EntityFrameworkCore;
using MvcEStoreData.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcEStoreData
{
    public class Rayon : BaseEntity
    {
        #region Properties

        [Display(Name = "Reyon Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [MaxLength(50, ErrorMessage = "{0} alanı en fazla {1} karakter olamalıdır!")]
        public string Name { get; set; }

        public int SortOrder { get; set; }

        #endregion

        #region Navigation

        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();

        #endregion

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Rayon>(entity =>
            {

                entity
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

                entity
                .HasIndex(p => new { p.Name })
                .IsUnique();

                entity
                .HasMany(p => p.Categories)
                .WithOne(p => p.Rayon)
                .HasForeignKey(p => p.RayonId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        }

    }
}