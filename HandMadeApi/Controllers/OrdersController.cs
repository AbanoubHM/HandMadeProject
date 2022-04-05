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
using HandMadeApi.Models.DTO.NewFolder;

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
        // GET: api/OrderHeaders/5
        [HttpGet("{userid}")]
        public async Task<ActionResult<IEnumerable<OrderProductDto>>> GetOrderHeader(string userid)
        {
            var orderHeader = await _context.CartHeaders.Where(c => c.ClientID == userid).FirstOrDefaultAsync();

            if (orderHeader == null)
            {
                return NotFound();
            }

            List<OrderDetails> orderDetails = await _context.OrderDetails.Where(c => c.OrderHeaderID == orderHeader.ID).ToListAsync();
            List<OrderProductDto> products = new List<OrderProductDto>();
            foreach (var item in orderDetails)
            {
                Product p1 = await _context.Products.FindAsync(item.ProductID);
                if (p1 != null)
                {
                    OrderProductDto pp = new OrderProductDto() { product = p1, Quantity = item.Quantity };
                    products.Add(pp);
                }
            }
            return products;
        }

        //POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderHeader>> PostOrder(PostOrderDto order)
        {
            OrderHeader orderHeader = await _context.OrderHeaders.Where(e => e.ClientID == order.UserID).SingleOrDefaultAsync();
            if (orderHeader == null)
            {
                orderHeader = new OrderHeader() { ClientID = order.UserID, OrderDateTime = DateTime.Now, City = order.City, State = order.State, Street = order.Street, Paid = false,};
                _context.OrderHeaders.Add(orderHeader);
                await _context.SaveChangesAsync();
                
                //cartHeader = await _context.CartHeaders.Where(e => e.ClientID == clientId).SingleOrDefaultAsync();
            }
            else
            {
                //bool flag = false;
                //await _context.CartDetails.Where(c => c.CartHeaderID == cartHeader.ID).ForEachAsync(c => {
                //    if (c.ProductID == productId)
                //    {
                //        c.Quantity += quantity;
                //        flag = true;
                //    }
                //});
                //if (flag)
                //{
                //    await _context.SaveChangesAsync();
                //    return Ok();
                //}
            }


            //CartDetails cartDetails = new CartDetails() { CartHeaderID = cartHeader.ID, ProductID = productId, Quantity = quantity };
            //_context.CartDetails.Add(cartDetails);


            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
