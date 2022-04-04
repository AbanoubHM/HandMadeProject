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
using HandMadeApi.Models.DTO.Products;

namespace HandMadeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly StoreContext _context;

        public OrdersController(StoreContext context)
        {
            _context = context;
        }

        //GET: api/Orders
       [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderHeader>>> GetOrders()
        {
            return await _context.OrderHeaders.ToListAsync();
        }
        //// GET: api/OrderHeaders/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<IEnumerable<OrderProductDto>>> GetOrderHeader(string id)
        //{
        //    var orderHeader = await _context.CartHeaders.Where(c => c.ClientID == id).FirstOrDefaultAsync();

        //    if (orderHeader == null)
        //    {
        //        return NotFound();
        //    }

        //    List<OrderDetails> orderDetails = await _context.OrderDetails.Where(c => c.OrderHeaderID == orderHeader.ID).ToListAsync();
        //    List<OrderProductDto> products = new List<OrderProductDto>();
        //    foreach (var item in orderDetails)
        //    {
        //        Product p1 = await _context.Products.FindAsync(item.ProductID);
        //        if (p1 != null)
        //        {
        //            OrderProductDto pp = new OrderProductDto() { product = p1, Quantity = item.Quantity };
        //            products.Add(pp);
        //        }
        //    }
        //    return products;
        //}

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOrder(int id, OrderHeader order)
        //{
        //    if (id != order.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<OrderHeader>> PostOrder(OrderHeader order) {
        //    List<Product> products = new List<Product>();
        //    foreach (var item in order.Products) {
        //       Product p = _context.Products.Find(item.ID);
        //        products.Add(p);
        //    }
        //    order.Products = products;
        //    //Product p = await _context.Products.FindAsync(id);
        //    //order.Products.Add(p);


        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetOrder", new { id = order.ID }, order);
        //}

        //// DELETE: api/Orders/5
        //[HttpDelete("{id}")]
        //[Authorize("delete:orders")]
        //public async Task<IActionResult> DeleteOrder(int id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool OrderExists(int id)
        //{
        //    return _context.Orders.Any(e => e.ID == id);
        //}
    }
}
