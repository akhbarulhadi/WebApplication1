using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ScottPlot;
using WebApplication1.Data;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Obsolete]
        public async Task<IActionResult> DepartmentChart()
        {
            IEnumerable<DepartmentChartVM> chartData;

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                chartData = await connection.QueryAsync<DepartmentChartVM>(
                    "chartDepartment",
                    commandType: System.Data.CommandType.StoredProcedure);
            }

            var labels = chartData.Select(d => d.DepartmentNm).ToArray();
            var values = chartData.Select(d => (double)d.TotalEmployees).ToArray();

            var plt = new ScottPlot.Plot();
            plt.Title("Jumlah Karyawan per Departemen");

            var barPlot = plt.Add.Bars(values);
            barPlot.Color = Colors.SteelBlue;
            barPlot.ValueLabelStyle.IsVisible = true;
            barPlot.ValueLabelStyle.ForeColor = Colors.White;
            barPlot.ValueLabelStyle.Bold = true;

            // Tambahkan teks secara manual di bawah setiap bar
            for (int i = 0; i < values.Length; i++)
            {
                // Tambahkan teks dan simpan sebagai variabel
                //ini unutk posisi di dalam bar
                //var textPlot = plt.Add.Text(values[i].ToString(), i, 0);
                // ini unut posisi diatas bar
                var textPlot = plt.Add.Text(values[i].ToString(), i, values[i]);

                // FIX: Atur perataan agar pas di tengah bawah
                textPlot.Alignment = Alignment.LowerCenter;

                // Opsional: Atur juga properti lain agar lebih jelas
                textPlot.FontSize = 15;
                textPlot.Bold = false;
            }

            // Pengaturan label nama departemen (tidak diubah)
            var ticks = labels.Select((label, index) => new Tick(index, label)).ToArray();

            plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            plt.Axes.Bottom.TickLabelStyle.Rotation = 0;
            plt.Axes.Bottom.MajorTickStyle.Length = 0;
            plt.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleCenter;
            plt.Axes.Margins(top: 0.1);

            plt.Axes.SetLimits(bottom: 0);

            byte[] imageBytes = plt.GetImageBytes(600, 400);
            return File(imageBytes, "image/png");
        }

        public async Task<IActionResult> GenderChart()
        {
            IEnumerable<GenderChartVM> chartData;

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                chartData = await connection.QueryAsync<GenderChartVM>(
                    "chartGender",
                    commandType: System.Data.CommandType.StoredProcedure
                );
            }

            var labels = chartData.Select(d => d.GenderNm).ToArray();
            var values = chartData.Select(d => (double)d.TotalGenders).ToArray();

            ScottPlot.Color[] customColors = labels.Select(label =>
                label.ToLower() switch
                {
                    "male" => Colors.Gold,
                    "female" => Colors.IndianRed,
                    _ => Colors.Gray
                }).ToArray();

            var plt = new ScottPlot.Plot();
            // HAPUS BARIS INI:
            // plt.Title("Distribusi Gender");

            // FIX 1: Tambahkan judul secara manual menggunakan Text plottable
            var title = plt.Add.Text("Distribusi Gender", 0, 1);
            title.FontSize = 17;
            title.Bold = true;
            title.Alignment = Alignment.LowerCenter;

            // FIX: Buat dan konfigurasikan List<PieSlice> terlebih dahulu
            List<PieSlice> slices = new();
            for (int i = 0; i < values.Length; i++)
            {
                var newSlice = new PieSlice()
                {
                    Value = values[i],
                    // Atur teks DI DALAM slice
                    Label = values[i].ToString(),
                    // Atur teks UNTUK LEGENDA
                    LegendText = labels[i],
                    // FIX: Atur ukuran font untuk label di dalam slice
                    LabelFontSize = 20,
                    // Tebal
                    LabelBold = true
                };
                newSlice.Fill.Color = customColors[i];
                slices.Add(newSlice);
            }

            // FIX: Tambahkan List<PieSlice> yang sudah jadi ke dalam plot
            var pie = plt.Add.Pie(slices);

            // Atur radius untuk memperbesar ukuran pie chart
            plt.Axes.SetLimits(left: -1, right: 1, bottom: -1, top: 1.2);

            // Konfigurasi tambahan
            pie.ExplodeFraction = 0.05;
            pie.SliceLabelDistance = 0.6;

            plt.Legend.IsVisible = true;
            plt.Legend.Alignment = Alignment.UpperLeft;

            // Menghilangkan bingkai
            plt.Axes.Frameless();
            // FIX: Tambahkan baris ini untuk menghilangkan garis-garis petak
            plt.HideGrid();

            byte[] imageBytes = plt.GetImageBytes(600, 400);
            return File(imageBytes, "image/png");
        }
    }
}