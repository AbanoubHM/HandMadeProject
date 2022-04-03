#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HandMadeApi.Models.StoreDatabase;

namespace HandMadeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartHeadersController : ControllerBase
    {
        private readonly StoreContext _context;

        public CartHeadersController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/CartHeaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartHeader>>> GetCartHeaders()
        {
            return await _context.CartHeaders.ToListAsync();
        }

        // GET: api/CartHeaders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartHeader>> GetCartHeader(int id)
        {
            var cartHeader = await _context.CartHeaders.FindAsync(id);

            if (cartHeader == null)
            {
                return NotFound();
            }

            return cartHeader;
        }

        // PUT: api/CartHeaders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartHeader(int id, CartHeader cartHeader)
        {
            if (id != cartHeader.ID)
            {
                return BadRequest();
            }

            _context.Entry(cartHeader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartHeaderExists(id))
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

        // POST: api/CartHeaders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartHeader>> PostCartHeader(CartHeader cartHeader)
        {
            _context.CartHeaders.Add(cartHeader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartHeader", new { id = cartHeader.ID }, cartHeader);
        }

        // DELETE: api/CartHeaders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartHeader(int id)
        {
            var cartHeader = await _context.CartHeaders.FindAsync(id);
            if (cartHeader == null)
            {
                return NotFound();
            }

            _context.CartHeaders.Remove(cartHeader);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartHeaderExists(int id)
        {
            return _context.CartHeaders.Any(e => e.ID == id);
        }
    }
}
