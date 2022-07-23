﻿using Microsoft.AspNetCore.Mvc;
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
    }
}
