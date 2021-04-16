﻿using Microsoft.EntityFrameworkCore;
using MVCEStoreData.Infrastructure;

namespace MVCEStoreData
{
    public class Banner : BaseEntity
    {
        #region Properties

        public string Picture { get; set; }

        public string Url { get; set; }

        public int? CategoryId { get; set; }

        #endregion

        #region Navigation

        public virtual Category Category { get; set; }

        #endregion

        public override void Build(ModelBuilder builder)
        {
            builder.Entity<Banner>(entity =>
            {
                entity
                .Property(p => p.Picture)
                .IsRequired()
                .IsUnicode(false);

            });

        }
    }
}