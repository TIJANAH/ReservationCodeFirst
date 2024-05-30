using System;
using System.Collections.Generic;

namespace MyReservations.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public string UserId { get; set; }

    public DateOnly? ReservationDate { get; set; }

    public int? NumberOfGuests { get; set; }

    public virtual User User { get; set; } = null!;

    public int? AttractionId { get; set; }
    public int? RestaurantId { get; set; }
    public int? EventId { get; set; }
    //link with CategoryId
    //link with AttractionId, RestaurantId, EventId
}
