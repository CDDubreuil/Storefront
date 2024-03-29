﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storefront.DATA.EF.Models;
using Storefront.UI.MVC.Utilities;

namespace Storefront.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly StorefrontProjectContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArtistsController(StorefrontProjectContext context)
        {
            _context = context;
        }


        // GET: Artists
      
        public async Task<IActionResult> Index()
        {
            var storefrontProjectContext = _context.Artists.Include(a => a.Genre);
            return View(await storefrontProjectContext.ToListAsync());
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }
        // GET: Artists/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreDescription");
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistName,ArtistId,City,State,Country,YearsActive,NumberOfAlbums,Members,GenreId,ArtistImage")] Artist artist)
        {
            if (ModelState.IsValid)
            {


                if(artist.ImageFile != null && artist.ImageFile.Length < 4_194_303)
                {
                    artist.ArtistImage = Guid.NewGuid() + Path.GetExtension(artist.ImageName);

                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string fullImagePath = webRootPath + "/img/";
                    using var memoryStream = new MemoryStream();
                    await artist.ImageFile.CopyToAsync(memoryStream);

                    using Image img = Image.FromStream(memoryStream);
                    ImageUtility.ResizeImage(fullImagePath, artist.ArtistImage, img, 250, 250);

                }

                else
                {
                    artist.ArtistImage = "noimage.png";
                }

                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreDescription", artist.GenreId);
            return View(artist);
        }
        // GET: Artists/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", artist.GenreId);
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistName,ArtistId,City,State,Country,YearsActive,NumberOfAlbums,Members,GenreId,ArtistImage")] Artist artist)
        {
            if (id != artist.ArtistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.ArtistId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreDescription", artist.GenreId);
            return View(artist);
        }
        // GET: Artists/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artists == null)
            {
                return Problem("Entity set 'StorefrontProjectContext.Artists'  is null.");
            }
            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
          return (_context.Artists?.Any(e => e.ArtistId == id)).GetValueOrDefault();
        }
    }
}
