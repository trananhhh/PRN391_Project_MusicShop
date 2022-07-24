using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Project_MusicShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project_MusicShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly PRN391_Project_MusicShopContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, PRN391_Project_MusicShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            /*
            Show show = new Show();
            show.ShowDate = DateTime.Now;
            var cinemaContext = _context.Shows.Include(s => s.Film).Include(s => s.Room);
            ViewData["shows"] = await cinemaContext.OrderByDescending(s => s.ShowId).ToListAsync();
            ViewData["FilmId"] = new SelectList(_context.Films.ToList<Film>(), "FilmId", "Title");
            ViewData["RoomID"] = new SelectList(_context.Rooms.ToList<Room>(), "RoomId", "Name");
            ViewBag.ShowDate = DateTime.Now;
            */
            var pRN391_Project_MusicShopContext = _context.Albums.Include(a => a.Artist).Include(a => a.Genre);
            return View(await pRN391_Project_MusicShopContext.ToListAsync());
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> ShopAsync()
        {
            //var pRN391_Project_MusicShopContext = ;
            ViewData["genres"] = await _context.Genres.ToArrayAsync();
            ViewData["albums"] = await _context.Albums.ToListAsync();
            ViewData["artists"] = await _context.Artists.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShopAsync(List<int>? Genres, List<int>? Artists)
        {
            ViewData["genres"] = await _context.Genres.ToArrayAsync();
            ViewData["artists"] = await _context.Artists.ToListAsync();
            ViewData["albums"] = await _context.Albums
                .Where(a => 
                    (Genres.Count == 0 || Genres.Contains((int)a.GenreId)) && 
                    (Artists.Count == 0 || Artists.Contains((int)a.ArtistId)))
                .ToListAsync();
            ViewData["filterGenres"] = Genres;
            ViewData["filterArtists"] = Artists;
            return View();
        }
        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Album album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.AlbumId == id);

            if (album == null)
            {
                return NotFound();
            }

            return View(album);
            //return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CartAsync(int? albumId, string? price, int? quantity)
        {
            //Update after login features done
            int currentAccountId = 1;

            //List<Order> tmpOrderList = _context.Orders.Where(o => o.AccountId == currentAccountId && o.Total == -1).ToList();
            //Order cartOrder = tmpOrderList[0];
            Order cartOrder = _context.Orders.Where(o => o.AccountId == currentAccountId && o.Total == -1).FirstOrDefault();
            if(cartOrder == null)
            {
                cartOrder = new Order();
                cartOrder.AccountId = currentAccountId;
                cartOrder.Total = -1;
                _context.Orders.Add(cartOrder);
            }
            int cartId = cartOrder.OrderId;

            OrderDetail newOrderDetails = new OrderDetail();
            newOrderDetails.AlbumId = (int)albumId;
            newOrderDetails.UnitPrice = (float)Convert.ToDouble(price);
            newOrderDetails.Quantity = (int)quantity;
            newOrderDetails.OrderId = cartId;
            _context.OrderDetails.Add(newOrderDetails);

            List<OrderDetail> currentCart = new List<OrderDetail>();
            currentCart = await _context.OrderDetails.Where(o => o.OrderId == cartId).Include(a => a.Album).ToListAsync();

            ViewData["currentCart"] = currentCart;

            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(IFormCollection values)
        {
            string username = values["UserName"];
            string password = values["Password"];
            string action = values["action"];
            Account acc1 = _context.Accounts.FirstOrDefault(s => s.Username == username && s.Pword == password);

            if (acc1!=null){
                HttpContext.Session.SetInt32("roleId", acc1.Role);
                HttpContext.Session.SetString("username", acc1.Username);
                return RedirectToAction(nameof(Index));
            }else{
                if (action.Equals("register"))
                {
                    Account acc = new Account(username, password, 1);
                    _context.Add(acc);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("roleId", acc.Role);
                    HttpContext.Session.SetString("username", acc.Username);
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("username", "");
            HttpContext.Session.SetInt32("roleId", -1);

            return RedirectToAction(nameof(Index));
        }
    }
}
