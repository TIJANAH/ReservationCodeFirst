using MyReservations.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyReservations.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

       
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}