﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HandMadeApi.Models.StoreDatabase;
using HandMadeApi.Models.DTO.Products;

namespace HandMadeApi.Controllers
{
    [Route("api/Cart")]
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
        public async Task<ActionResult<IEnumerable<CartProductDto>>> GetCartHeader(string id)
        {
            var cartHeader = await _context.CartHeaders.Where(c => c.ClientID == id).FirstOrDefaultAsync();

            if (cartHeader == null)
            {
                return NotFound();
            }

            List<CartDetails> cartDetails =await _context.CartDetails.Where(c=>c.CartHeaderID == cartHeader.ID).ToListAsync();
            List<CartProductDto> products=new List<CartProductDto>();
            foreach (var item in cartDetails) {
                Product p1 = await _context.Products.FindAsync(item.ProductID);
                if (p1 != null) {
                    CartProductDto pp = new CartProductDto() { product = p1,Quantity=item.Quantity};
                    products.Add(pp);
                }
            }
            return products;
        }

        // PUT: api/CartHeaders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutCartHeader(string clientId, int productId, int quantity = 1) {
            var cartHeader = await _context.CartHeaders.Where(c => c.ClientID == clientId).SingleOrDefaultAsync();

            if (cartHeader == null) {
                return NotFound();
            }
            
            await _context.CartDetails.Where(c => c.CartHeaderID == cartHeader.ID).ForEachAsync(c => {
                if (c.ProductID == productId) {
                    c.Quantity = quantity;
                    
                }
            });
            //_context.Entry(cartHeader).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {


                throw;

            }

            return NoContent();
        }

        // POST: api/CartHeaders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartHeader>> PostCartHeader(string clientId,int productId,int quantity=1)
        {
            CartHeader cartHeader = await _context.CartHeaders.Where(e => e.ClientID == clientId).SingleOrDefaultAsync();
            if (cartHeader == null) {
                cartHeader = new CartHeader() { ClientID = clientId };
                _context.CartHeaders.Add(cartHeader);
                await _context.SaveChangesAsync();
                cartHeader = await _context.CartHeaders.Where(e => e.ClientID == clientId).SingleOrDefaultAsync();
            } else {
                bool flag = false;
                await _context.CartDetails.Where(c => c.CartHeaderID == cartHeader.ID).ForEachAsync(c => {
                    if (c.ProductID == productId) {
                        c.Quantity += quantity;
                        flag = true;
                    }
                });
                if (flag) { 
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }


            CartDetails cartDetails = new CartDetails() { CartHeaderID = cartHeader.ID, ProductID = productId, Quantity = quantity };
            _context.CartDetails.Add(cartDetails);


            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/CartHeaders/5
        [HttpDelete]
        public async Task<IActionResult> DeleteCartHeader(string clientId,int productId)
        {
            var cartHeader = await _context.CartHeaders.Where(e => e.ClientID == clientId).SingleOrDefaultAsync();
            if (cartHeader == null)
            {
                return NotFound();
            }

            List<CartDetails> cartDetails = await _context.CartDetails.Where(e => e.CartHeaderID == cartHeader.ID).ToListAsync();
            foreach (var item in cartDetails) {
                if (item.ProductID == productId) {
                    _context.CartDetails.Remove(item);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
            }

            return NoContent();
        }

        private bool CartHeaderExists(int id)
        {
            return _context.CartHeaders.Any(e => e.ID == id);
        }
    }
}
