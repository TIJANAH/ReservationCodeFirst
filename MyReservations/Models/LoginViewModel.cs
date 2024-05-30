using System.ComponentModel.DataAnnotations;

namespace MyReservations.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The Username field is required.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]

        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
