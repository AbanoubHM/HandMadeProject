#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HandMadeApi.Models.StoreDatabase;
using Microsoft.AspNetCore.Authorization;

namespace HandMadeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRatesController : ControllerBase
    {
        private readonly StoreContext _context;

        public ProductRatesController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/ProductRates
        [HttpGet]
        [Authorize("read:rate")]
        public async Task<ActionResult<IEnumerable<ProductRate>>> GetProductRates()
        {
            return await _context.ProductRates.ToListAsync();
        }
        // PUT: api/ProductRates/5

        [HttpPut("{id}")]
        [Authorize("update:rate")]
        public async Task<IActionResult> PutProductRate(int id, ProductRate productRate)
        {
            if (id != productRate.ID)
            {
                return BadRequest();
            }

            _context.Entry(productRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductRateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductRates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize("post:rate")]
        public async Task<ActionResult<ProductRate>> PostProductRate(ProductRate productRate)
        {
            _context.ProductRates.Add(productRate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductRate", new { id = productRate.ID }, productRate);
        }

        // DELETE: api/ProductRates/5
        [HttpDelete("{id}")]
        [Authorize("delete:rate")]
        public async Task<IActionResult> DeleteProductRate(int id)
        {
            var productRate = await _context.ProductRates.FindAsync(id);
            if (productRate == null)
            {
                return NotFound();
            }

            _context.ProductRates.Remove(productRate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductRateExists(int id)
        {
            return _context.ProductRates.Any(e => e.ID == id);
        }
    }
}
