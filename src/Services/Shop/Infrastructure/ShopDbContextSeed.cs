using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WangZhen.Techniques.Shop.Api.Infrastructure.Entities;

namespace WangZhen.Techniques.Shop.Api.Infrastructure
{
    public class ShopDbContextSeed
    {
        public async Task SeedAsync(ShopDbContext dbContext, IWebHostEnvironment env,
            ILogger<ShopDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger,nameof(ShopDbContextSeed));
            await policy.ExecuteAsync(async()=> {
                if (!dbContext.ShopItems.Any())
                {
                    await dbContext.ShopItems.AddRangeAsync(GetInMemoryShopItems());

                    await dbContext.SaveChangesAsync();
                }
            });
        }


        private List<ShopItem> GetInMemoryShopItems()
        {
            return new List<ShopItem>()
            {
                new ShopItem()
                {
                    Name="沃尔玛",
                    ImgUrl="http://www.dell-lee.com/imgs/vue3/near.png",
                    sales=100,
                    ExpressLimit = 0,
                    ExpressPrice=5,
                    Slogan="Vip尊享满89元减4元运费卷"
                },
                new ShopItem()
                {
                    Name="山姆会员店",
                    ImgUrl="http://www.dell-lee.com/imgs/vue3/near.png",
                    sales=10000,
                    ExpressLimit=2,
                    ExpressPrice=10,
                    Slogan="联合利华洗护满18减5"
                }
            };
        }
        private AsyncRetryPolicy CreatePolicy(ILogger<ShopDbContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>()
                .WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timespan, retry, ctx) =>
                {
                    logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                });

        }
    }
}
