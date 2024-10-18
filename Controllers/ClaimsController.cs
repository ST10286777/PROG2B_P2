using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public ClaimsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        // GET: Claims
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var claimsWithDocuments = await _context.Claims
                .Include(c => c.Document) // This includes the related documents
                .ToListAsync();

            return View(claimsWithDocuments);
        }

        // GET: Claims
        public async Task<IActionResult> LecturerIndex()
        {
            var userId = _userManager.GetUserId(User);
            var claims = await _context.Claims
                .Include(c => c.Document)
                .Where(c => c.UserID == userId) // Use Where instead of FirstOrDefault to return multiple claims
                .ToListAsync(); // Get a list of claims

            return View("LecturerIndex", claims); // Pass claims as the model
        }

        // GET: Claims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claims = await _context.Claims.Include(c => c.Document)
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
        public async Task<IActionResult> Create([Bind("ClaimsID,Name,Module,HoursWorked,HourlyWage,SupportingNote")] Claims claims, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                claims.UserID = userId;
                claims.Status = "Pending";

                // Save claim first
                _context.Add(claims);
                await _context.SaveChangesAsync();

                // Handle file upload if a file is selected
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(_uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Save the file information in the database linked to the claim
                    var document = new Document
                    {
                        FileName = fileName,
                        FilePath = filePath,
                        ClaimsID = claims.ClaimsID
                    };
                    _context.Documents.Add(document);
                    await _context.SaveChangesAsync();

                    ViewBag.Message = "File uploaded successfully!";
                }
                else
                {
                    ViewBag.Message = "Please select a file.";
                }

                return RedirectToAction(nameof(LecturerIndex));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaimsID, Name, Module, HoursWorked, HourlyWage, SupportingNote, Status")] Claims claims)
        {
           
            if (id != claims.ClaimsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(User);
                    claims.UserID = userId;
                    claims.Status = "Pending";
                    _context.Update(claims);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(LecturerIndex)); // Ensure this redirect happens on success
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Claims.Any(e => e.ClaimsID == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If ModelState is invalid, return the view with validation errors
            return View(claims);
        }

      



        // GET: Claims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claims = await _context.Claims.Include(c => c.Document)
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


        //GET: Approve or Reject a claim based on its ID & the provided status
        public async Task<IActionResult> EditStatus(int id, string status)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimsID == id);
            if (claim == null)
            {
                return NotFound();
            }

            claim.Status = "Pending"; //Update the status of the claim (either approve / reject)
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Upload()
        {
            return View();
        }




        // POST: Upload Document
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int claimId)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(_uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Save file details to the database
                var document = new Document
                {
                    FileName = fileName,
                    FilePath = filePath,
                    ClaimsID = claimId
                };
                _context.Documents.Add(document);
                await _context.SaveChangesAsync();

                ViewBag.Message = "File uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "Please select a file.";
            }

            return RedirectToAction("Details", new { id = claimId });
        }

        // Download Document
        public IActionResult Download(string fileName)
        {
            var filePath = Path.Combine(_uploadFolder, fileName);
            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", fileName);
            }
            else
            {
                return NotFound();
            }
        }
    }


}

