using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WangZhen.Techniques.Shop.Api.Infrastructure.Entities;

namespace WangZhen.Techniques.Shop.Api.Infrastructure.EntityConfigurations
{
    public class ShopItemEntityTypeConfiguration
        : IEntityTypeConfiguration<ShopItem>
    {
        public void Configure(EntityTypeBuilder<ShopItem> builder)
        {
            builder.ToTable("Shop");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.ImgUrl)
                .IsRequired();

            builder.Property(p => p.sales)
                .IsRequired();

            builder.Property(p => p.Slogan)
                .IsRequired();

            builder.Property(p => p.ExpressLimit)
                .IsRequired();

            builder.Property(p => p.ExpressPrice)
                .IsRequired();
             

        }
    }
}
