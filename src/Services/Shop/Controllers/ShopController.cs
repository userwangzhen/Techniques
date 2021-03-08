using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WangZhen.Techniques.Shop.Api.Infrastructure;

namespace WangZhen.Techniques.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ShopDbContext _dbContext;
        private readonly ILogger<ShopController> _logger;

        public ShopController(ShopDbContext dbContext, ILogger<ShopController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ItemsAsync()
        {
            var items = await _dbContext.ShopItems.AsNoTracking().ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var item = await _dbContext.ShopItems.FindAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }
    }
}
