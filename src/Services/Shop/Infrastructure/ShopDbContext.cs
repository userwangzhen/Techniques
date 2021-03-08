using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WangZhen.Techniques.Shop.Api.Infrastructure.Entities;
using WangZhen.Techniques.Shop.Api.Infrastructure.EntityConfigurations;

namespace WangZhen.Techniques.Shop.Api.Infrastructure
{
    public class ShopDbContext:DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options):base(options)
        {

        }

        public DbSet<ShopItem> ShopItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ShopItemEntityTypeConfiguration());
        }


    }

    public class ShopContextDesignFactory : IDesignTimeDbContextFactory<ShopDbContext>
    {
        public ShopDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>()
                 .UseMySql("server=118.178.252.149;port=3506;user=wangzhen;password=duibuqi520@@");

            return new ShopDbContext(optionsBuilder.Options);
        }
    }
}
