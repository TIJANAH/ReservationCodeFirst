using System;
using System.Collections.Generic;

namespace MyReservations.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int UserId { get; set; }

    public DateOnly? ReservationDate { get; set; }

    public int? NumberOfGuests { get; set; }

    public virtual User User { get; set; } = null!;
}
