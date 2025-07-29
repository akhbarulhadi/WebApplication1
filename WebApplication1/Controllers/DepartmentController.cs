using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DepartmentController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var allDepartment = await _context.DepartmentVM
                .FromSqlRaw("EXEC GetDepartment")
                .ToListAsync();
            return View(allDepartment);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {

            if (await _context.Department.AnyAsync(e => e.DepartmentCode == department.DepartmentCode))
                ModelState.AddModelError("DepartmentCode", "Department Code already exists.");

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            _context.Department.Add(department);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data has been successfully saved!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string? id)
        {

            var department = await _context.Department.FindAsync(id);
            if (department == null)
                return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department department)
        {

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            var existing = await _context.Department.FindAsync(department.DepartmentCode);
            if (existing == null)
                return NotFound();

            existing.DepartmentNm = department.DepartmentNm;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data has been successfully updated!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string departmentCode)
        {
            var department = await _context.Department.FindAsync(departmentCode);
            if (department != null)
                _context.Department.Remove(department);

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
