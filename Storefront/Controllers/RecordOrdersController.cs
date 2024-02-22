using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storefront.DATA.EF.Models;

namespace Storefront.Controllers
{
    public class RecordOrdersController : Controller
    {
        private readonly StorefrontProjectContext _context;

        public RecordOrdersController(StorefrontProjectContext context)
        {
            _context = context;
        }

        // GET: RecordOrders
        public async Task<IActionResult> Index()
        {
            var storefrontProjectContext = _context.RecordOrders.Include(r => r.Order);
            return View(await storefrontProjectContext.ToListAsync());
        }

        // GET: RecordOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RecordOrders == null)
            {
                return NotFound();
            }

            var recordOrder = await _context.RecordOrders
                .Include(r => r.Order)
                .FirstOrDefaultAsync(m => m.RecordOrderId == id);
            if (recordOrder == null)
            {
                return NotFound();
            }

            return View(recordOrder);
        }

        // GET: RecordOrders/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerId");
            return View();
        }

        // POST: RecordOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecordOrderId,OrderId,RecordId")] RecordOrder recordOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recordOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerId", recordOrder.OrderId);
            return View(recordOrder);
        }

        // GET: RecordOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RecordOrders == null)
            {
                return NotFound();
            }

            var recordOrder = await _context.RecordOrders.FindAsync(id);
            if (recordOrder == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerId", recordOrder.OrderId);
            return View(recordOrder);
        }

        // POST: RecordOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecordOrderId,OrderId,RecordId")] RecordOrder recordOrder)
        {
            if (id != recordOrder.RecordOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recordOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordOrderExists(recordOrder.RecordOrderId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerId", recordOrder.OrderId);
            return View(recordOrder);
        }

        // GET: RecordOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RecordOrders == null)
            {
                return NotFound();
            }

            var recordOrder = await _context.RecordOrders
                .Include(r => r.Order)
                .FirstOrDefaultAsync(m => m.RecordOrderId == id);
            if (recordOrder == null)
            {
                return NotFound();
            }

            return View(recordOrder);
        }

        // POST: RecordOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RecordOrders == null)
            {
                return Problem("Entity set 'StorefrontProjectContext.RecordOrders'  is null.");
            }
            var recordOrder = await _context.RecordOrders.FindAsync(id);
            if (recordOrder != null)
            {
                _context.RecordOrders.Remove(recordOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordOrderExists(int id)
        {
          return (_context.RecordOrders?.Any(e => e.RecordOrderId == id)).GetValueOrDefault();
        }
    }
}
