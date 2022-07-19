using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project_MusicShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var pRN391_Project_MusicShopContext = _context.Albums.Include(a => a.Artist).Include(a => a.Genre);
            return View(await pRN391_Project_MusicShopContext.ToListAsync());
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> ShopAsync()
        {
            var pRN391_Project_MusicShopContext = _context.Albums.Include(a => a.Artist).Include(a => a.Genre);
            return View(await pRN391_Project_MusicShopContext.ToListAsync());
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult Cart()
        {
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
    }
}
