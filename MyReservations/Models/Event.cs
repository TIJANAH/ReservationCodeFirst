using System;
using System.Collections.Generic;

namespace MyReservations.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string? EventName { get; set; }

    public string? EventDesc { get; set; }

    public string? EventHours { get; set; }

    public string? EventLocation { get; set; }

    public int? EventTickets { get; set; }

    public string? EventImages { get; set;}
}
