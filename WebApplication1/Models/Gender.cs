using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Gender
    {
        [Key]
        public char GenderCode { get; set; }

        [Required]
        public required string GenderNm { get; set; }
    }
}
