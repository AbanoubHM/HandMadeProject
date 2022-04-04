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
using HandMadeApi.Auth;
using RestSharp;
using System.Text.Json;
using RestSharp.Authenticators;
using HandMadeApi.Models.StoreDatabase.Favourite;
using HandMadeApi.Models.DTO.Favourite;

namespace HandMadeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly StoreContext _context;

        public ClientsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(string id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(string id, Client client)
        {
            if (id != client.ID)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            try
            {
                await _context.SaveChangesAsync();
                updateRole(client.ID);
            }
            catch (DbUpdateException)
            {
                if (ClientExists(client.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClient", new { id = client.ID }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        [Authorize("delete:client")]
        public async Task<IActionResult> DeleteClient(string id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(string id)
        {
            return _context.Clients.Any(e => e.ID == id);
        }

        /// <summary>
        /// Gets an access token from authorization service
        /// </summary>
        /// <returns> a temporary access token</returns>
        private static AccessToken getAccessToken()
        {
            var client = new RestClient("https://dev-vxrkxu-x.us.auth0.com/oauth/token");


            var request = new RestRequest() { Method = Method.Post }.AddJsonBody(new
            {
                client_id = "HxBxwjeFxRNSohGKa0XHsdqYfH2E8iYK",
                client_secret = "1ljixj5PzsvsJEbI2wlJGvBKTMH8CZlFhXQ5jWze4xpQ_RIDNPNGxpdDE_7zce8q",
                audience = "https://dev-vxrkxu-x.us.auth0.com/api/v2/",
                grant_type = "client_credentials"
            });


            Task<RestResponse> ress = client.PostAsync(request);

            AccessToken cc = JsonSerializer.Deserialize<AccessToken>(ress.Result.Content);
            return cc;
        }


        public static async void updateRole(string usrId, string role = "rol_hoaMhd72umuo4Z3I")
        {

            var url = $"https://dev-vxrkxu-x.us.auth0.com/api/v2/users/{usrId}/roles";
            var client = new RestClient(url);
            var token = getAccessToken().access_token;
            client.Authenticator = new JwtAuthenticator(token);
            var request = new RestRequest().AddJsonBody(new { roles = new[] { role } });

            var response = client.PostAsync(request).GetAwaiter().GetResult();

        }

        [HttpGet("{id}/role")]
        public async Task<ActionResult<string>> GetRole(string id)
        {
            var url = $"https://dev-vxrkxu-x.us.auth0.com/api/v2/users/{id}/roles";
            var client = new RestClient(url);
            var token = getAccessToken().access_token;
            client.Authenticator = new JwtAuthenticator(token);
            var request = new RestRequest();

            var response = client.GetAsync(request).GetAwaiter().GetResult();
            List<UserRole> userRoles = JsonSerializer.Deserialize<List<UserRole>>(response.Content);
            foreach (var item in userRoles)
            {
                if (item.id == "rol_A4MqRdrRIF1ZiPeE")
                {
                    return "Vendor";
                }
            }
            return "Client";


        }

        // GET: api/Clients/fav/5
        [HttpGet("Favourite/{id}")]
        public async Task<ActionResult<IEnumerable<FavouriteDto>>> GetClientFavourites(string id)
        {
            var clientfavs = await _context.Favs.Where(x => x.UserID == id).ToListAsync();

            if (clientfavs == null)
            {
                return NotFound();
            }
            List<FavouriteDto> favourites = new List<FavouriteDto>();
            foreach (var favourite in clientfavs)
            {
                Product product = await _context.Products.FindAsync(favourite.ProductID);
                FavouriteDto favouriteDto = new FavouriteDto()
                {
                    ProductID = product.ID,
                    Image = product.Image,
                    Name = product.Name,
                    Price = (int)(product.Price - product.SaleValue),
                    Quantity = product.Quantity
                };
                favourites.Add(favouriteDto);
            }
            return favourites;
        }
        [HttpPost("Favourite")]
        public async Task<ActionResult<Fav>> PostClientFavourite(string clientid, int productid)
        {
            Fav fav = await _context.Favs.Where(f => (f.UserID == clientid && f.ProductID == productid)).FirstOrDefaultAsync();
            if (fav == null)
            {
                fav = new Fav() { UserID = clientid, ProductID = productid };
                _context.Favs.Add(fav);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpDelete("Favourite")]
        public async Task<IActionResult> DeleteFavourite(string clientId, int productId, bool deleteall = false)
        {
            List<Fav> favs = await _context.Favs.Where(f=> f.UserID == clientId).ToListAsync();
            if (deleteall)
            {
                foreach (var item in favs)
                {
                    _context.Favs.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
            else if(productId != 0)
            {
                foreach (var item in favs)
                {
                    if (item.ProductID == productId)
                    {
                        _context.Favs.Remove(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return NoContent();
        }
    }
}
