namespace WebApplication1.ViewModels
{
    public class ChartVM
    {
        public IEnumerable<DepartmentChartVM>? DepartmentData { get; set; }
        public IEnumerable<GenderChartVM>? GenderData { get; set; }
    }
}
