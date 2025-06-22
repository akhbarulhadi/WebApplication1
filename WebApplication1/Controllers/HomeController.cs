using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var allEmployee = await _context.EmployeeDept
                .Include(item => item.Employee!)
                 .ThenInclude(item => item.Genders)
                .Include(item => item.Department)
                .OrderBy(item => item.Employee!.SesaId)
                .ToListAsync();
            return View(allEmployee);
        }

        private void LoadDropdownData(string? selectedDeptCode = null, char? selectedGenderCode = null)
        {
            ViewBag.GenderList = new SelectList(
                _context.Genders.AsNoTracking(), "GenderCode", "GenderNm", selectedGenderCode);

            ViewBag.DepartmentList = new SelectList(
                _context.Department.AsNoTracking(), "DepartmentCode", "DepartmentNm", selectedDeptCode);
        }
        private void ValidateDepartmentCode(string? departmentCode)
        {
            if (string.IsNullOrWhiteSpace(departmentCode))
                ModelState.AddModelError("DepartmentCode", "Please select a department.");
        }

        public IActionResult Create()
        {
            LoadDropdownData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            var departmentCode = Request.Form["DepartmentCode"].ToString();

            ValidateDepartmentCode(departmentCode);

            if (await _context.Employee.AnyAsync(e => e.SesaId == employee.SesaId))
                ModelState.AddModelError("SesaId", "SESA ID already exists.");

            if (!ModelState.IsValid)
            {
                LoadDropdownData(departmentCode, employee.GenderCode);
                return View(employee);
            }

            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();

            _context.EmployeeDept.Add(new EmployeeDept
            {
                SesaId = employee.SesaId,
                DepartmentCode = departmentCode
            });
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data has been successfully saved!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
                return NotFound();

            var employeeDept = await _context.EmployeeDept.FindAsync(id);
            string? selectedDeptCode = employeeDept?.DepartmentCode;

            LoadDropdownData(selectedDeptCode, employee.GenderCode);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            var departmentCode = Request.Form["DepartmentCode"].ToString();

            ValidateDepartmentCode(departmentCode);

            if (!ModelState.IsValid)
            {
                LoadDropdownData(departmentCode, employee.GenderCode);
                return View(employee);
            }

            var existing = await _context.Employee.FindAsync(employee.SesaId);
            if (existing == null)
                return NotFound();

            existing.Name = employee.Name;
            existing.BirthDate = employee.BirthDate;
            existing.GenderCode = employee.GenderCode;

            var employeeDept = await _context.EmployeeDept.FindAsync(employee.SesaId);
            if (employeeDept != null)
            {
                employeeDept.DepartmentCode = departmentCode;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Data has been successfully updated!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string sesaId)
        {
            var employeeDept = await _context.EmployeeDept.FindAsync(sesaId);
            if (employeeDept != null)
                _context.EmployeeDept.Remove(employeeDept);

            var employee = await _context.Employee.FindAsync(sesaId);
            if (employee != null)
                _context.Employee.Remove(employee);

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
