using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class ChartJsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartJsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Siapkan koneksi
            using var connection = new SqlConnection(_context.Database.GetConnectionString());

            // 1. Ambil data untuk Bar Chart
            var departmentData = await connection.QueryAsync<DepartmentChartVM>(
                "chartDepartment",
                commandType: System.Data.CommandType.StoredProcedure);

            // 2. Ambil data untuk Pie Chart
            var genderData = await connection.QueryAsync<GenderChartVM>(
                "chartGender", // Panggil Stored Procedure kedua
                commandType: System.Data.CommandType.StoredProcedure);

            // 3. Masukkan semua data ke dalam satu ViewModel container
            var viewModel = new ChartVM
            {
                DepartmentData = departmentData,
                GenderData = genderData
            };

            // 4. Kirim ViewModel container ke View
            return View(viewModel);
        }
    }
}
