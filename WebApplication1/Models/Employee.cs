using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Employee
    {
        [Key]
        public required string SesaId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required DateOnly BirthDate { get; set; }

        [Required]
        public required char GenderCode { get; set; }

        [ForeignKey(nameof(GenderCode))]
        public Gender? Genders { get; set; }

    }
}
