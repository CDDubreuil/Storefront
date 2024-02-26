using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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

      //  GET: CustomerDatas/Create
        public async Task<IActionResult> Create()
        {
            var customer = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentCustomer = await _context.CustomerData.FindAsync(customer);
            if (currentCustomer != null)
            {

                CustomerData model = new()
                {
                    CustomerId = currentCustomer.CustomerId,
                    FirstName = currentCustomer.FirstName,
                    LastName = currentCustomer.LastName,
                    CustomerCity = currentCustomer.CustomerCity,
                    CustomerState = currentCustomer.CustomerState,
                    CustomerZip = currentCustomer.CustomerZip
                };

                return View(model);
            }
            else
            {
                return View(new CustomerData());
            }
        }


    
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,OrderId,CustomerCity,CustomerState,CustomerZip,CustomerCountry,Phone")] CustomerData customerData)
        {
          
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var existingCustomer= await _context.CustomerData.FindAsync(userId);
                if(existingCustomer != null)
                {
                    ModelState.AddModelError("", "A customer record already exists for this user! Please edit the existing record.");
                }
                customerData.CustomerId =userId;

                _context.Add(customerData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "A customer record already exists for this user! Please edit the existing record.");
            }
            return View(customerData);
        }


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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,OrderId,CustomerCity,CustomerState,CustomerZip,CustomerCountry,Phone")] CustomerData customerData)
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