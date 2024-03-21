using System;
using System.Collections.Generic;

namespace MyReservations.Models;

public partial class Attraction
{
    public int AttractionsId { get; set; }

    public string? AttractionsName { get; set; }

    public string? AttractionsDesc { get; set; }

    public string? AttractionsHours { get; set; }

    public string? AttractionsLocation { get; set; }

    public string? AttractionsImages { get; set; }

    public int? AttractionsTickets { get; set; }
}
