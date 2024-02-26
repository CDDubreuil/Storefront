using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Storefront.DATA.EF.Models;

namespace Storefront.Controllers
{

    public class GenresController : Controller
    {
        private readonly StorefrontProjectContext _context;

        public GenresController(StorefrontProjectContext context)
        {
            _context = context;
        }

        // GET: Genres
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return _context.Genres != null ?
                        View(await _context.Genres.ToListAsync()) :
                        Problem("Entity set 'StorefrontProjectContext.Genres'  is null.");
        }

    
        public PartialViewResult Details(int id)
        {
            return PartialView(_context.Genres.Find(id));
            
        }






    //Ajax Step 1
    // GET: Genres/Details/[Authorize(Roles ="Admin")]

    //        public async Task<IActionResult> Details(int? id)
    //        {
    //            if (id == null || _context.Genres == null)
    //            {
    //                return NotFound();
    //            }

    //            var genre = await _context.Genres
    //                .FirstOrDefaultAsync(m => m.GenreId == id);
    //            if (genre == null)
    //            {
    //                return NotFound();
    //            }

    //            return View(genre);
    //        }

    // GET: Genres/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Genres/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("GenreName,GenreId,GenreDescription")] Genre genre)
    {
        if (ModelState.IsValid)
        {
            _context.Add(genre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(genre);
    }

    // GET: Genres/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Genres == null)
        {
            return NotFound();
        }

        var genre = await _context.Genres.FindAsync(id);
        if (genre == null)
        {
            return NotFound();
        }
        return View(genre);
    }

    // POST: Genres/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("GenreName,GenreId,GenreDescription")] Genre genre)
    {
        if (id != genre.GenreId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(genre);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(genre.GenreId))
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
        return View(genre);
    }

    // GET: Genres/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Genres == null)
        {
            return NotFound();
        }

        var genre = await _context.Genres
            .FirstOrDefaultAsync(m => m.GenreId == id);
        if (genre == null)
        {
            return NotFound();
        }

        return View(genre);
    }

    // POST: Genres/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Genres == null)
        {
            return Problem("Entity set 'StorefrontProjectContext.Genres'  is null.");
        }
        var genre = await _context.Genres.FindAsync(id);
        if (genre != null)
        {
            _context.Genres.Remove(genre);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GenreExists(int id)
    {
        return (_context.Genres?.Any(e => e.GenreId == id)).GetValueOrDefault();
    }
}
}
