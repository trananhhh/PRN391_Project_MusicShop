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
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if(cart != null)
            {
                ViewBag.total = cart.Sum(item => item.album.Price * item.quantity);
            }
            return View(cart);
        }

        private int isExist(int? id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].album.AlbumId == id)
                {
                    return i;
                }
            }
            return -1;
        }

        [HttpPost]
        public IActionResult CartAsync(int? albumId, string? price, int? quantity)
        {
            //int? roleId = HttpContext.Session.GetInt32("roleId") ?? -1;
            //if(roleId == 1)
            //{
                ProductModel productModel = new ProductModel();
                if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
                {
                    List<Item> cart = new List<Item>();
                    cart.Add(new Item { album = productModel.find(albumId), quantity = quantity });
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                else
                {
                    int index = isExist(albumId);
                    List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

                    if (index != -1)
                    {
                        Item item = cart[index];
                        int? currentQuantity = item.quantity + quantity;
                        item.quantity = currentQuantity;
                    }
                    else if (index == -1)
                    {
                        cart.Add(new Item { album = productModel.find(albumId), quantity = quantity });
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);


                }
                return RedirectToAction("Cart");
            //}
            
            
        }
        public IActionResult Checkout()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (cart != null)
            {
                ViewBag.total = cart.Sum(item => item.album.Price * item.quantity);
            }
            ViewData["cart"] = cart;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckOut([Bind("AccountId", "OrderDate", "FirstName", "LastName", "Address", "City", "State", "Country", "Phone", "Total")] Order order)
        {
            DateTime orderdate = DateTime.Now;
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            _context.Add(new Order(order.AccountId, 
                order.OrderDate, 
                order.FirstName, 
                order.LastName, 
                order.Address, 
                order.City, 
                order.State, 
                order.Country, 
                order.Phone, 
                order.Total));
            _context.SaveChanges();
            Order lastOrder = _context.Orders.OrderBy(x => x.OrderId).LastOrDefault();
            int lastOrderId = 1;
            if (lastOrder != null)
            {
                lastOrderId = lastOrder.OrderId;
            }
            foreach (Item item in cart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = lastOrderId,
                    AlbumId = item.album.AlbumId,
                    Quantity = (int)item.quantity,
                    UnitPrice = (int)item.quantity * item.album.Price
                };
                _context.Add(orderDetail);
                _context.SaveChanges();

            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", null);
            return RedirectToAction("Index", "Orders");
        }

        public ActionResult RemoveFromCart(int AlbumId)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            foreach (Item item in cart)
            {
                if (item.album.AlbumId == AlbumId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            if (cart != null)
            {
                ViewBag.total = cart.Sum(item => item.album.Price * item.quantity);
            }
            return View(nameof(Cart), cart);
        }

        public IActionResult ViewOrder()
        {
            int? accountid = HttpContext.Session.GetInt32("accountid") ?? -1;
            List<Order> listorder = _context.Orders.Where(s => s.AccountId == accountid).ToList();
            return View(listorder);
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
                HttpContext.Session.SetInt32("accountid", acc1.AccountId);

                return RedirectToAction(nameof(Index));
            }else{
                if (action.Equals("register"))
                {
                    Account acc = new Account(username, password, 1);
                    _context.Add(acc);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("roleId", acc.Role);
                    HttpContext.Session.SetString("username", acc.Username);
                    HttpContext.Session.SetInt32("accountid", acc.AccountId);
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("username", "");
            HttpContext.Session.SetInt32("roleId", -1);
            HttpContext.Session.SetInt32("accountid", -1);
            return RedirectToAction(nameof(Index));
        }
    }
}
