using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class EmployeeController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            IEnumerable<EmployeeVM> allEmployee;
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                allEmployee = await connection.QueryAsync<EmployeeVM>("EXEC getEmployee");
            }
            // KODE INI KALAU UNUTK SUPAYA DAPPER TAHU INI STORE PROCEDURE
            //{
            //    allEmployee = await connection.QueryAsync<EmployeeVM>(
            //        "getEmployee",
            //        commandType: System.Data.CommandType.StoredProcedure);
            //}
            return View(allEmployee.ToList());
        }

        private void LoadDropdownData(string? selectedDeptCode = null, char? selectedGenderCode = null)
        {
            var genderList = _context.GenderVM
                .FromSqlRaw("EXEC GetGender")
                .AsNoTracking()
                .ToList();

            var departmentList = _context.DepartmentVM
                .FromSqlRaw("EXEC GetDepartment")
                .AsNoTracking()
                .ToList();

            ViewBag.GenderList = new SelectList(genderList, "GenderCode", "GenderNm", selectedGenderCode);
            ViewBag.DepartmentList = new SelectList(departmentList, "DepartmentCode", "DepartmentNm", selectedDeptCode);
        }

        public IActionResult Create()
        {
            LoadDropdownData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeVM employeeVM)
        {
            if (await _context.Employee.AnyAsync(employee => employee.SesaId == employeeVM.SesaId))
            {
                ModelState.AddModelError("SesaId", "SESA ID already exists.");
            }
            if (!ModelState.IsValid)
            {
                LoadDropdownData(employeeVM.DepartmentCode, employeeVM.GenderCode);
                return View(employeeVM);
            }

            try
            {
                // Dapatkan koneksi langsung dari DbContext
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    string sql = "EXEC addEmployee @SesaId, @Name, @BirthDate, @GenderCode, @DepartmentCode";

                    // Dapper menggunakan objek anonim untuk parameter, lebih ringkas
                    await connection.ExecuteAsync(sql, new
                    {
                        SesaId = employeeVM.SesaId,
                        Name = employeeVM.Name,
                        BirthDate = employeeVM.BirthDate,
                        GenderCode = employeeVM.GenderCode,
                        DepartmentCode = employeeVM.DepartmentCode
                    });
                };

                TempData["SuccessMessage"] = "Data has been successfully saved!";
                return RedirectToAction(nameof(Index));
            }
            //ini unutk debug
            //catch (DbUpdateException ex)
            //ini untuk production
            catch (Exception ex)
            {
                // Tangani error dari database dengan baik
                ModelState.AddModelError(string.Empty, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
                LoadDropdownData(employeeVM.DepartmentCode, employeeVM.GenderCode);
                return View(employeeVM);
            }

        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            EmployeeVM? employee;
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                string sql = "EXEC getEmployeeById @SesaId";
                employee = await connection.QueryFirstOrDefaultAsync<EmployeeVM>(sql, new { SesaId = id });
            }

            if (employee == null)
                return NotFound();

            LoadDropdownData(employee.DepartmentCode, employee.GenderCode);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeVM employeeVM)
        {

            if (!ModelState.IsValid)
            {
                LoadDropdownData(employeeVM.DepartmentCode, employeeVM.GenderCode);
                return View(employeeVM);
            }

            try
            {
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    string sql = "EXEC updateEmployee @SesaId, @Name, @BirthDate, @GenderCode, @DepartmentCode";

                    await connection.ExecuteAsync(sql, new
                    {
                        SesaId = employeeVM.SesaId,
                        Name = employeeVM.Name,
                        BirthDate = employeeVM.BirthDate,
                        GenderCode = employeeVM.GenderCode,
                        DepartmentCode = employeeVM.DepartmentCode
                    });
                }

                TempData["SuccessMessage"] = "Data has been successfully updated!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
                LoadDropdownData(employeeVM.DepartmentCode, employeeVM.GenderCode);
                return View(employeeVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string sesaId)
        {
            try
            {
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.ExecuteAsync("EXEC deleteEmployee @SesaId", new { SesaId = sesaId });
                }

                TempData["SuccessMessage"] = "Successfully deleted!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
