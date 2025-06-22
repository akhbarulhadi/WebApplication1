using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Department
    {
        [Key]
        public required string DepartmentCode { get; set; }

        [Required]
        public required string DepartmentNm { get; set; }
    }
}
