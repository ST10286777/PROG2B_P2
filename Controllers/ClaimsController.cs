using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG_P1.Data;
using PROG_P1.Data.Migrations;
using PROG_P1.Models;

namespace PROG_P1.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ClaimsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Claims
        public async Task<IActionResult> Index()
        {
            return View(await _context.Claims.ToListAsync());
        }

        public IActionResult UploadDocument()
        {
            return View();
        }

        // GET: Claims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claims = await _context.Claims
                .FirstOrDefaultAsync(m => m.ClaimsID == id);
            if (claims == null)
            {
                return NotFound();
            }

            return View(claims);
        }

        // GET: Claims/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaimsID,Name,Module,HoursWorked,HourlyWage,TotalEarnings,SupportingNote,Status")] Claims claims)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                claims.UserID = userId;
                claims.Status = "Pending";

                _context.Add(claims);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(claims);
        }

        // GET: Claims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claims = await _context.Claims.FindAsync(id);
            if (claims == null)
            {
                return NotFound();
            }
            return View(claims);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaimsID,Name,Module,HoursWorked,HourlyWage,TotalEarningsSupportingNote,Status")] Claims claims)
        {
            if (id != claims.ClaimsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(claims);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaimsExists(claims.ClaimsID))
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
            return View(claims);
        }

        // GET: Claims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claims = await _context.Claims
                .FirstOrDefaultAsync(m => m.ClaimsID == id);
            if (claims == null)
            {
                return NotFound();
            }

            return View(claims);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claims = await _context.Claims.FindAsync(id);
            if (claims != null)
            {
                _context.Claims.Remove(claims);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClaimsExists(int id)
        {
            return _context.Claims.Any(e => e.ClaimsID == id);
        }


        //GET: Approve or Reject a claim based on its ID & the provided status
        public async Task<IActionResult> Approve(int id, string status)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimsID == id); 
            if (claim == null)
            {
                return NotFound();
            }

            claim.Status = "Approved"; //Update the status of the claim (either approve / reject)
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET: Approve or Reject a claim based on its ID & the provided status
        public async Task<IActionResult> Reject(int id, string status)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimsID == id);
            if (claim == null)
            {
                return NotFound();
            }

            claim.Status = "Rejected"; //Update the status of the claim (either approve / reject)
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
