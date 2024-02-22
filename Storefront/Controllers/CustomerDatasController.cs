using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storefront.DATA.EF.Models;

namespace Storefront.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerDatasController : Controller
    {
        private readonly StorefrontProjectContext _context;

        public CustomerDatasController(StorefrontProjectContext context)
        {
            _context = context;
        }

        // GET: CustomerDatas
        public async Task<IActionResult> Index()
        {
              return _context.CustomerData != null ? 
                          View(await _context.CustomerData.ToListAsync()) :
                          Problem("Entity set 'StorefrontProjectContext.CustomerData'  is null.");
        }

        // GET: CustomerDatas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.CustomerData == null)
            {
                return NotFound();
            }

            var customerData = await _context.CustomerData
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customerData == null)
            {
                return NotFound();
            }

            return View(customerData);
        }

        // GET: CustomerDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,OrderId,CustomerCity,CustomerState,CustomerZip,CustomerCountry,Phone")] CustomerData customerData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerData);
        }

        // GET: CustomerDatas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.CustomerData == null)
            {
                return NotFound();
            }

            var customerData = await _context.CustomerData.FindAsync(id);
            if (customerData == null)
            {
                return NotFound();
            }
            return View(customerData);
        }

        // POST: CustomerDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CustomerId,FirstName,LastName,OrderId,CustomerCity,CustomerState,CustomerZip,CustomerCountry,Phone")] CustomerData customerData)
        {
            if (id != customerData.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerDataExists(customerData.CustomerId))
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
            return View(customerData);
        }

        // GET: CustomerDatas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.CustomerData == null)
            {
                return NotFound();
            }

            var customerData = await _context.CustomerData
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customerData == null)
            {
                return NotFound();
            }

            return View(customerData);
        }

        // POST: CustomerDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.CustomerData == null)
            {
                return Problem("Entity set 'StorefrontProjectContext.CustomerData'  is null.");
            }
            var customerData = await _context.CustomerData.FindAsync(id);
            if (customerData != null)
            {
                _context.CustomerData.Remove(customerData);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerDataExists(string id)
        {
          return (_context.CustomerData?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
