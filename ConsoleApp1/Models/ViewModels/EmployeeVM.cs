using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class EmployeeVM
    {
        [Key]
        [RegularExpression("^[^\\s]+$", ErrorMessage = "SESA ID must not contain spaces.")]
        public required string SesaId { get; set; }

        public required string Name { get; set; }
        public required DateOnly BirthDate { get; set; }
        public required char GenderCode { get; set; }
        public string? GenderNm { get; set; }
        public string? DepartmentNm { get; set; }
        public required string DepartmentCode { get; set; }
    }
}
