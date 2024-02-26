using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storefront.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using Storefront.UI.MVC.Utilities;
using System.Reflection.Metadata;
using X.PagedList;
using Microsoft.AspNetCore.Hosting;


namespace Storefront.Controllers
{
    public class RecordsController : Controller
    {
        private readonly StorefrontProjectContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecordsController(StorefrontProjectContext context)
        {
            _context = context;
        }

        // GET: Records
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(TiledIndex));
            }
            var records = _context.Records
                .Include(r => r.Artist)
                .Include(r => r.RecordOrders);
            return View(await records.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> TiledIndex(string? searchTerm, int genreId = 0, int page = 1)
        {
            var records = await _context.Records
                .Include(r => r.Artist)
                .ToListAsync();
            if (searchTerm != null)
            {
                ViewBag.SearchTerm = searchTerm;
                searchTerm = searchTerm.ToLower();
                records = records
                    .Where(r => r.SearchString.ToLower().Contains(searchTerm))
                    .ToList();
                ViewBag.NbrResults = records.Count;
            }
            
            ViewBag.Genres = new SelectList(_context.Genres, "GenreId", "GenreName", genreId);
            if (genreId != 0)
            {
                records = records.Where(r => r.Artist.Genre.GenreId == genreId).ToList();
                ViewBag.NbrResults = records.Count;
                var genre = await _context.Genres.FirstOrDefaultAsync(r =>  r.GenreId == genreId);
                ViewBag.GenreName = genre?.GenreName;
                ViewBag.GenreId = genre?.GenreId;
            }
            ViewBag.NbrResults = records.Count;
            return View(records.ToPagedList(page, 6));
        }

        // GET: Records/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var @record = await _context.Records
                .Include(r => r.Artist)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (@record == null)
            {
                return NotFound();
            }

            return View(@record);
        }

        // GET: Records/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            ViewBag.GenreId = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "ArtistName");
          //  ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusId");
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("RecordId,RecordName,ArtistId,Year,Price,CoverArt,ExecutiveProducer")] Record @record)
        { 

            if (ModelState.IsValid)
            {
                if (record.ImageFile != null && record.ImageFile.Length < 4_194_303)
                {
                    record.CoverArt = Guid.NewGuid() + Path.GetExtension(record.ImageName);

                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string fullImagePath = webRootPath + "/img/";
                    using var memoryStream = new MemoryStream();
                    await record.ImageFile.CopyToAsync(memoryStream);

                    using Image img = Image.FromStream(memoryStream);//using System.Drawing;

                    ImageUtility.ResizeImage(fullImagePath, record.CoverArt, img, 500, 100);
                }
                else
                {
                    record.CoverArt = "noimage.png";
                }




                _context.Add(@record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TiledIndex));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", record.Artist.Genre.GenreName);
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "ArtistName", record.ArtistId);
          //  ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusId", @record.StatusId);
          
            return View(@record);
        }

        // GET: Records/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var @record = await _context.Records.FindAsync(id);
            if (@record == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "ArtistName", @record.ArtistId);
           // ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusId", @record.StatusId);
            return View(@record);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("RecordId,RecordName,ArtistId,Year,Price,CoverArt,ExecutiveProducer,Artist.Genre.GenreId")] Record @record)
        {
            if (id != @record.RecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@record);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordExists(@record.RecordId))
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "ArtistName", @record.ArtistId);
           // ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusId", @record.StatusId);
            return View(@record);
        }

        // GET: Records/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var @record = await _context.Records
                .Include(r => r.Artist)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (@record == null)
            {
                return NotFound();
            }

            return View(@record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Records == null)
            {
                return Problem("Entity set 'StorefrontProjectContext.Records'  is null.");
            }
            var @record = await _context.Records.FindAsync(id);
            if (@record != null)
            {
                _context.Records.Remove(@record);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordExists(int id)
        {
          return (_context.Records?.Any(e => e.RecordId == id)).GetValueOrDefault();
        }
    }
}
