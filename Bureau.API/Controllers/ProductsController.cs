using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bureau.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bureau.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly BureauContext _context;

        public ProductsController(BureauContext context)
        {
            _context = context;
        }

        [Authorize("Bearer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }       
    }
}
