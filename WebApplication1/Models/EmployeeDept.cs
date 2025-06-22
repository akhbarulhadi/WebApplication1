using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class EmployeeDept
    {
        [Key]
        public required string SesaId { get; set; }

        [Required]
        public required string DepartmentCode { get; set; }

        [ForeignKey(nameof(SesaId))]
        public Employee? Employee { get; set; }

        [ForeignKey(nameof(DepartmentCode))]
        public Department? Department { get; set; }

    }
}
