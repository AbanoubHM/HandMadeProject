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
using HandMadeApi.Models.DTO.Order;
using AutoMapper;

namespace HandMadeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public OrdersController(StoreContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: api/Orders
       [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderHeader>>> GetOrders()
        {
            return await _context.OrderHeaders.ToListAsync();
        }
        // GET: api/OrderHeaders/5
        /// <summary>
        /// Get all orders made by this client
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>List of orders</returns>
        [HttpGet("{userid}")]
        public async Task<ActionResult<IEnumerable<OrderHeader>>> GetOrderHeader(string userid)
        {
            var orderHeaders = await _context.OrderHeaders.Where(c => c.ClientID == userid).ToListAsync();

            if (orderHeaders == null)
            {
                return NotFound();
            }

            
            return orderHeaders;
        }

        /// <summary>
        /// Gets specific order Details and products
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>order Details and products</returns>
        [HttpGet("details/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderDetails>>> GetOrderDetails(int orderId)
        {
            var orderDetails = await _context.OrderDetails.Include(c => c.Product).Where(e=>e.OrderHeaderID==orderId).ToListAsync();

            if (orderDetails == null)
            {
                return NotFound();
            }

            
            return orderDetails;
        }

        //POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderHeader>> PostOrder(PostOrderDto order) {
            var cartHeader = await _context.CartHeaders.Where(e => e.ClientID == order.ClientID).SingleOrDefaultAsync();


            if (cartHeader == null) { return NotFound(); }

            var orderHeader = 
                new OrderHeader() {
                ClientID = order.ClientID,
                Phone = order.Phone,
                Note = order.Note,
                OrderDateTime = DateTime.Now,
                City = order.City,
                State = order.State,
                Street = order.Street,
                Paid = false,
            };
            _context.OrderHeaders.Add(orderHeader);
            await _context.SaveChangesAsync();
            await _context.CartDetails.Include(c => c.Product).Where(e => e.CartHeaderID == cartHeader.ID).ForEachAsync(e => {
                _context.OrderDetails.Add(new OrderDetails() {
                    OrderHeaderID = orderHeader.ID,
                    ProductID = e.ProductID,
                    Quantity = e.Quantity
                }
              );
                _context.Products.Where(x => x.ID == e.ProductID).SingleOrDefault().Quantity-=e.Quantity;
                _context.CartDetails.Remove(e);
            });




            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int id) {
            var order = await _context.OrderHeaders.FindAsync(id);
            if (order == null) {
                return NotFound();
            }

            await _context.OrderDetails.Where(e => e.OrderHeaderID == order.ID).ForEachAsync(e => {
                var p = _context.Products.Where(x => x.ID == e.ProductID).SingleOrDefault();
                p.Quantity += e.Quantity;
                _context.Products.Update(p);
                _context.OrderDetails.Remove(e); });

            _context.OrderHeaders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
