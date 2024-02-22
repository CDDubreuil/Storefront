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


namespace Storefront.Controllers
{
    public class RecordsController : Controller
    {
        private readonly StorefrontProjectContext _context;

        public RecordsController(StorefrontProjectContext context)
        {
            _context = context;
        }

        // GET: Records
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
                ViewBag.Searchterm = searchTerm;
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
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "ArtistName");
            ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusId");
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecordId,RecordName,ArtistId,Year,Price,CoverArt,ExecutiveProducer,StatusId")] Record @record)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "ArtistName", @record.ArtistId);
            ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusId", @record.StatusId);
            return View(@record);
        }

        // GET: Records/Edit/5
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
            ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusId", @record.StatusId);
            return View(@record);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecordId,RecordName,ArtistId,Year,Price,CoverArt,ExecutiveProducer,StatusId")] Record @record)
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
            ViewData["StatusId"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusId", @record.StatusId);
            return View(@record);
        }

        // GET: Records/Delete/5
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
