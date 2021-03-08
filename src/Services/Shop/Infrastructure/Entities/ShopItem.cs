using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WangZhen.Techniques.Shop.Api.Infrastructure.Entities
{
    public class ShopItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public int sales { get; set; }

        public int ExpressLimit { get; set; }

        public int ExpressPrice { get; set; }

        public string Slogan { get; set; }
    }
}
