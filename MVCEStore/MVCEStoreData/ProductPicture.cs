using Microsoft.EntityFrameworkCore;
using MVCEStoreData.Infrastructure;

namespace MVCEStoreData
{
    public class ProductPicture : BaseEntity
    {
        #region Properties

        public string Picture { get; set; }

        public int ProductId { get; set; }

        #endregion

        #region Navigation

        public virtual Product Product { get; set; }

        #endregion

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<ProductPicture>(entity =>
            {
                entity
                .Property(p => p.Picture)
                .IsRequired()
                .IsUnicode(false);

            });
        }
    }
}