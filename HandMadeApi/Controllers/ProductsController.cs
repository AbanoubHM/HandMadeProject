#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HandMadeApi.Models.StoreDatabase;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace HandMadeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string sort, string search, int categoryid, string storeid, int minprice, int maxprice)
        {
            //Select only available products
            var products = await _context.Products.Where(p => p.Quantity > 0).ToListAsync();
            //sort
            products = sort switch
            {
                "NA" => products.OrderBy(p => p.Name).ToList(),
                "ND" => products.OrderByDescending(p => p.Name).ToList(),
                "PA" => products.OrderBy(p => p.Price).ToList(),
                "PD" => products.OrderByDescending(p => p.Price).ToList(),
                _ => products.ToList()
            };
            //Search
            if (!string.IsNullOrEmpty(search)) { products = products.Where(p => (p.Name.ToLower().Contains(search.ToLower())) || (p.Description.ToLower().Contains(search.ToLower()))).ToList(); }
            //select category
            if (categoryid != 0){products = products.Where(p => p.CategoryID == categoryid).ToList();}
            //select store
            if (!string.IsNullOrEmpty(storeid)) { products = products.Where(p => p.StoreID == storeid).ToList(); }
            //filter price
            if (minprice != 0){products = products.Where(p => p.Price >= minprice).ToList();}
            if (maxprice != 0){products = products.Where(p => p.Price <= maxprice).ToList();}
            return products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            product.CategoryName = _context.Categories.Where(e => e.ID == product.CategoryID).Select(x => x.Name).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("update:product")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ID }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [Authorize("delete:product")]
        public async Task<IActionResult> DeleteProduct(int id)
        {

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }

        //Resize Image
        //private string ResizeImage(IFormFile file)
        //{
        //    if (file == null)
        //    {
        //        return BadRequest();
        //    }

        //    var image = Image.FromStream(file.OpenReadStream());

        //    var resizedImage = new Bitmap(image, new Size(256, 256));
        //    var stream = new System.IO.MemoryStream();
        //    resizedImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    stream.Position = 0;

        //    return File(stream, "image/jpeg");
        //}
    }
}
