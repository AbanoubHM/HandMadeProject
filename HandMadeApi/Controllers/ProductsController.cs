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
using HandMadeApi.Models.DTO.Products;
using ImageMagick;
using Azure.Storage.Blobs;
using System.IdentityModel.Tokens.Jwt;

namespace HandMadeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly string connection = "DefaultEndpointsProtocol=https;AccountName=handmadestoragedb;AccountKey=VB2TdG+PH9hEHmaKwXQNcTt43j2Bz2kG9xlClkMGVcxezNnG+PRJ2jX7Jq01Z/YGBOxmD/Rz+EG9osGTrkmjEg==;BlobEndpoint=https://handmadestoragedb.blob.core.windows.net/;TableEndpoint=https://handmadestoragedb.table.core.windows.net/;QueueEndpoint=https://handmadestoragedb.queue.core.windows.net/;FileEndpoint=https://handmadestoragedb.file.core.windows.net/";


        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string sort, string search, int categoryid, string storeid, int minprice, int maxprice,int pagesize=12,int pagenumber=1)
        {
          
            //Select only available products
            List<Product> products = await _context.Products.Where(p => p.Quantity > 0).ToListAsync();
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
            products= products.Skip(pagesize * (pagenumber-1)).Take(pagesize).ToList();
            return products;
        }
        // GET: api/Products/1/rate
        [HttpGet("{id}/Reviews")]
        public async Task<ActionResult<IEnumerable<ProductRate>>> GetProductRate(int id)
        {
            var productRates = await _context.ProductRates.Where(p => p.ProductID == id).ToListAsync();
            if (productRates == null)
            {
                return NotFound();
            }
            var TotalRate = 0;
            foreach (var item in productRates)
            {
                TotalRate += item.RateValue;
            }
            var AvgRate = TotalRate / productRates.Count();
            return productRates;
        }
        //Get: api/Products/1/avgrate
        [HttpGet("{id}/AvgRate")]
        public async Task<ActionResult<int>> GetAvgRate(int id)
        {
            var productRates = await _context.ProductRates.Where(p => p.ProductID == id).ToListAsync();
            var TotalRate = 0;
            foreach (var item in productRates)
            {
                TotalRate += item.RateValue;
            }
            var AvgRate = TotalRate / productRates.Count();
            return AvgRate;
        }


        // GET: api/Products/5
        [HttpGet("{id}")]
        [Authorize]
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


        [HttpPost("/upload")]
        public async Task<IActionResult> uploadFile(IFormFile file) {
            try {
                Stream stream1 = new FileStream($"{Directory.GetCurrentDirectory()}/abc",
                                                    FileMode.OpenOrCreate,
                                                    FileAccess.ReadWrite,
                                                    FileShare.ReadWrite,
                                                    4096,
                                                    FileOptions.DeleteOnClose);

                stream1.Position = 0;
                var img = new MagickImage(file.OpenReadStream());
                img.Resize(700, 0);

                BlobClient blobClient = new BlobClient(connection,
                    "test2", "" + DateTime.Now.Ticks + Path.GetExtension(file.FileName));



                img.Write(stream1);
                stream1.Position = 0;

                await blobClient.UploadAsync(stream1);

                stream1.Close();
                string imageUrl = blobClient.Uri.AbsoluteUri;
                return Ok(new { imageUrl });
            } catch (Exception e)  {
                return BadRequest(new {e.Message});
            }
        }


        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductsDTO>> PostProduct(ProductsDTO product)
        {
            
            Product productToAdd = new Product() {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SaleValue = product.SaleValue,
                CategoryName = _context.Categories.Where(e => e.ID == product.CategoryID).Select(x => x.Name).FirstOrDefault(),

                Image = product.Image,
                Quantity = product.Quantity,
                PreparationDays = product.PreparationDays,
                CategoryID = product.CategoryID,
                StoreID = product.StoreID
            };


            _context.Products.Add(productToAdd);
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
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(Request.Headers.Authorization.ToString().Split(' ')[1]);
            var tokenS = jsonToken as JwtSecurityToken;
            var subId = tokenS.Claims.Where(e => e.Type == "sub").FirstOrDefault();
            var role = tokenS.Claims.Where(e => e.Type == "http://roletest.net/roles" && e.Value == "Admin").FirstOrDefault();
            if (role?.Value == "Admin" || subId.Value == product.StoreID) {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok();
            }
            return Unauthorized();


            
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
