using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace MyReservations.Models;

public partial class ReservationsContext : DbContext
{
    public ReservationsContext()
    {
    }

    public ReservationsContext(DbContextOptions<ReservationsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attraction> Attractions { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Event> Events { get; set; }


    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=SEMOS-PC7; Database=Reservations; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attraction>(entity =>
        {
            entity.HasKey(e => e.AttractionsId).HasName("PK__Attracti__692919EB73F87B8F");

            entity.Property(e => e.AttractionsId).HasColumnName("AttractionsID");
            entity.Property(e => e.AttractionsDesc).HasMaxLength(500);
            entity.Property(e => e.AttractionsHours).HasMaxLength(50);
            entity.Property(e => e.AttractionsLocation).HasMaxLength(50);
            entity.Property(e => e.AttractionsName).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2BBE6CF043");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__7944C87049FE7801");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.EventDesc).HasMaxLength(500);
            entity.Property(e => e.EventHours).HasMaxLength(50);
            entity.Property(e => e.EventLocation).HasMaxLength(50);
            entity.Property(e => e.EventName).HasMaxLength(50);
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.PictureId).HasName("PK__Picture__8C2866F88335795E");

            entity.ToTable("Picture");

            entity.Property(e => e.PictureId).HasColumnName("PictureID");
            entity.Property(e => e.PictureUrl)
                .HasMaxLength(200)
                .HasColumnName("PictureURL");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Reservat__B7EE5F043679522C");

            entity.ToTable("Reservation");

            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__UserI__44FF419A");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PK__Restaura__87454CB57F8BF1D3");

            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
            entity.Property(e => e.RestaurantDesc).HasMaxLength(500);
            entity.Property(e => e.RestaurantHours).HasMaxLength(50);
            entity.Property(e => e.RestaurantLocation).HasMaxLength(50);
            entity.Property(e => e.RestaurantName).HasMaxLength(50);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__712CC627AA2FE740");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__UserID__3E52440B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC93D91225");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
