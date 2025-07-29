using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Employee
    {
        [Key]
        [RegularExpression("^[^\\s]+$", ErrorMessage = "SESA ID must not contain spaces.")]
        public required string SesaId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required DateOnly BirthDate { get; set; }

        [Required]
        public required char GenderCode { get; set; }

        [ForeignKey(nameof(GenderCode))]
        public Genders? Genders { get; set; }

    }
}
