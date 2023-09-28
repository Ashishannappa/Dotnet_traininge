using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelSystem.Data;
using HotelSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelSystem.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hotel
        public async Task<IActionResult> Index()
        {
              return _context.HotelTab != null ? 
                          View(await _context.HotelTab.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.HotelTab'  is null.");
        }

        // GET: Hotel/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.HotelTab == null)
            {
                return NotFound();
            }

            var hotelDb = await _context.HotelTab
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelDb == null)
            {
                return NotFound();
            }

            return View(hotelDb);
        }

        // GET: Hotel/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,DOJ")] HotelDb hotelDb)
        {
            if (ModelState.IsValid)
            {
                hotelDb.Id = Guid.NewGuid();
                _context.Add(hotelDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotelDb);
        }

        // GET: Hotel/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.HotelTab == null)
            {
                return NotFound();
            }

            var hotelDb = await _context.HotelTab.FindAsync(id);
            if (hotelDb == null)
            {
                return NotFound();
            }
            return View(hotelDb);
        }

        // POST: Hotel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Email,DOJ")] HotelDb hotelDb)
        {
            if (id != hotelDb.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotelDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelDbExists(hotelDb.Id))
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
            return View(hotelDb);
        }

        // GET: Hotel/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.HotelTab == null)
            {
                return NotFound();
            }

            var hotelDb = await _context.HotelTab
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelDb == null)
            {
                return NotFound();
            }

            return View(hotelDb);
        }

        // POST: Hotel/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.HotelTab == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HotelTab'  is null.");
            }
            var hotelDb = await _context.HotelTab.FindAsync(id);
            if (hotelDb != null)
            {
                _context.HotelTab.Remove(hotelDb);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelDbExists(Guid id)
        {
          return (_context.HotelTab?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
