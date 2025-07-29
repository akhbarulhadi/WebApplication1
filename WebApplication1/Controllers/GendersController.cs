using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GendersController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var allGenders = await _context.GenderVM
                .FromSqlRaw("EXEC GetGender")
                .ToListAsync();
            return View(allGenders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genders genders)
        {

            if (await _context.Genders.AnyAsync(e => e.GenderCode == genders.GenderCode))
                ModelState.AddModelError("GenderCode", "Gender Code already exists.");

            if (!ModelState.IsValid)
            {
                return View(genders);
            }

            _context.Genders.Add(genders);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data has been successfully saved!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(char? id)
        {

            var genders = await _context.Genders.FindAsync(id);
            if (genders == null)
                return NotFound();

            return View(genders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Genders genders)
        {

            if (!ModelState.IsValid)
            {
                return View(genders);
            }

            var existing = await _context.Genders.FindAsync(genders.GenderCode);
            if (existing == null)
                return NotFound();

            existing.GenderNm = genders.GenderNm;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data has been successfully updated!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(char genderCode)
        {
            var genders = await _context.Genders.FindAsync(genderCode);
            if (genders != null)
                _context.Genders.Remove(genders);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Successfully deleted!";
            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
