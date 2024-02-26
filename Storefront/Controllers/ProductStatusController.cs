using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storefront.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;

namespace Storefront.Controllers
{
    public class ProductStatusController : Controller
    {
        private readonly StorefrontProjectContext _context;

        public ProductStatusController(StorefrontProjectContext context)
        {
            _context = context;
        }

        // GET: ProductStatus
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return _context.ProductStatuses != null ? 
                          View(await _context.ProductStatuses.ToListAsync()) :
                          Problem("Entity set 'StorefrontProjectContext.ProductStatuses'  is null.");
        }

        // GET: ProductStatus/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductStatuses == null)
            {
                return NotFound();
            }

            var productStatus = await _context.ProductStatuses
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (productStatus == null)
            {
                return NotFound();
            }

            return View(productStatus);
        }

        // GET: ProductStatus/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("StatusId,RecordName,ProductStatus1,StatusDescription")] ProductStatus productStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productStatus);
        }

        // GET: ProductStatus/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductStatuses == null)
            {
                return NotFound();
            }

            var productStatus = await _context.ProductStatuses.FindAsync(id);
            if (productStatus == null)
            {
                return NotFound();
            }
            return View(productStatus);
        }

        // POST: ProductStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,RecordName,ProductStatus1,StatusDescription")] ProductStatus productStatus)
        {
            if (id != productStatus.StatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductStatusExists(productStatus.StatusId))
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
            return View(productStatus);
        }

        // GET: ProductStatus/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductStatuses == null)
            {
                return NotFound();
            }

            var productStatus = await _context.ProductStatuses
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (productStatus == null)
            {
                return NotFound();
            }

            return View(productStatus);
        }

        // POST: ProductStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductStatuses == null)
            {
                return Problem("Entity set 'StorefrontProjectContext.ProductStatuses'  is null.");
            }
            var productStatus = await _context.ProductStatuses.FindAsync(id);
            if (productStatus != null)
            {
                _context.ProductStatuses.Remove(productStatus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductStatusExists(int id)
        {
          return (_context.ProductStatuses?.Any(e => e.StatusId == id)).GetValueOrDefault();
        }
    }
}
