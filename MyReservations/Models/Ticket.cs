using System;
using System.Collections.Generic;

namespace MyReservations.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int UserId { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public decimal? Price { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual User User { get; set; } = null!;
}
