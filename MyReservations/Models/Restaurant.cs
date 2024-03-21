using System;
using System.Collections.Generic;

namespace MyReservations.Models;

public partial class Restaurant
{
    public int RestaurantId { get; set; }

    public string? RestaurantName { get; set; }

    public string? RestaurantDesc { get; set; }

    public string? RestaurantHours { get; set; }

    public string? RestaurantLocation { get; set; }

    public string? RestaurantImages { get; set; } = string.Empty;
}
