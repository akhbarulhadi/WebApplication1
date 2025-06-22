using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Department
    {
        [Key]
        [RegularExpression("^[^\\s]+$", ErrorMessage = "Department Code must not contain spaces.")]
        public required string DepartmentCode { get; set; }

        [Required]
        public required string DepartmentNm { get; set; }
    }
}
