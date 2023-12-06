using Microsoft.EntityFrameworkCore;
using HotelWizard.Models;
using Newtonsoft.Json;
using System.Configuration;

public class ApplicationContext : DbContext
{
    public DbSet<Reservation> Reservations { get; set; } = null!;
    public DbSet<ModelUsers> Users { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomImage> RoomImages { get; set; } = null!;
    public DbSet<DateBooked> DateBookeds { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
		
		// Database.EnsureDeleted();
		// Database.EnsureCreated();
	}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<RoomImage>().HasData(
			new RoomImage { id = 1, image = "/images/Rooms/1/1.jpeg", RoomId = 1 },
			new RoomImage { id = 2, image = "/images/Rooms/1/2.jpeg", RoomId = 1 },
			new RoomImage { id = 3, image = "/images/Rooms/1/3.jpeg", RoomId = 1 },
			new RoomImage { id = 4, image = "/images/Rooms/1/4.jpeg", RoomId = 1 },
			new RoomImage { id = 5, image = "/images/Rooms/2/1.jpeg", RoomId = 2 },
			new RoomImage { id = 6, image = "/images/Rooms/2/2.jpeg", RoomId = 2 },
			new RoomImage { id = 7, image = "/images/Rooms/2/3.jpeg", RoomId = 2 },
			new RoomImage { id = 8, image = "/images/Rooms/2/4.jpeg", RoomId = 2 },
			new RoomImage { id = 9, image = "/images/Rooms/3/1.jpeg", RoomId = 3 },
			new RoomImage { id = 10, image = "/images/Rooms/3/2.jpeg", RoomId = 3 },
			new RoomImage { id = 11, image = "/images/Rooms/3/3.jpeg", RoomId = 3 },
			new RoomImage { id = 12, image = "/images/Rooms/3/4.jpeg", RoomId = 3 },
			new RoomImage { id = 13, image = "/images/Rooms/3/5.jpeg", RoomId = 3 },
			new RoomImage { id = 14, image = "/images/Rooms/3/6.jpeg", RoomId = 3 },
			new RoomImage { id = 15, image = "/images/Rooms/3/7.jpeg", RoomId = 3 },
			new RoomImage { id = 16, image = "/images/Rooms/3/8.jpeg", RoomId = 3 }
		);


		modelBuilder.Entity<Room>().HasData(
				new Room
				{
					Id = 1,
					Number = "101",
					Type = "I",
					Description = "Стандартный люкс с бассеином",
					PricePerNight = 100.0m
				},
				new Room
				{
					Id = 2,
					Number = "201",
					Type = "II",
					Description = "Обычный номер",
					PricePerNight = 50.0m
				},
				new Room
				{
					Id = 3,
					Number = "202",
					Type = "III",
					Description = "Обычный номер",
					PricePerNight = 50.0m
				}
			);


		modelBuilder.Entity<DateBooked>().HasData(
            new DateBooked
            {
                Id = 1,
                startDate = new DateTime(2023, 10, 6),
                endDate = new DateTime(2023, 10, 8),
                RoomId = 1, // Ссылка на комнату с Id = 1
            },
            new DateBooked
            {
                Id = 2,
                startDate = new DateTime(2023, 10, 10),
                endDate = new DateTime(2023, 10, 12),
                RoomId = 2, // Ссылка на комнату с Id = 2
            },
            new DateBooked
            {
                Id = 3,
                startDate = new DateTime(2023, 10, 7),
                endDate = new DateTime(2023, 10, 10),
                RoomId = 3, // Ссылка на комнату с Id = 3
            }
            //new DateBooked
            //{
            //    Id = 4,
            //    startDate = new DateTime(2023, 9, 12),
            //    endDate = new DateTime(2023, 9, 14),
            //    RoomId = 2, // Ссылка на комнату с Id = 2
            //}
            // Добавьте другие бронирования по аналогии

        );

    }


}