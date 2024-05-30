using Microsoft.AspNetCore.Identity;
using MyReservations.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyReservations.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

       
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public string? Token { get; set; } 
    }
}