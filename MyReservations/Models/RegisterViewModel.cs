using System.ComponentModel.DataAnnotations;

namespace MyReservations.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The Name is required.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "The Surname is required.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "The Username is required.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "The Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "The Password is required.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "The Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string RepeatPassword { get; set; }
    }
}
