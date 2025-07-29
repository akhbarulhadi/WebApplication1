using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Genders
    {
        [Key]
        [RegularExpression("^[^\\s]+$", ErrorMessage = "Gender Code must not contain spaces.")]
        public char GenderCode { get; set; }

        [Required]
        public required string GenderNm { get; set; }
    }
}
