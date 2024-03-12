using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace HotelWizard.Models
{
    public class ModelClass
    {
    }
    // Модель для представления информации о номере отеля
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

		public double PricePerNight { get; set; }

        public ICollection<RoomImage> ImageArray{ get; set; } = new List<RoomImage>();        //public bool IsBooked{ get; set; } // статус

    }

    public class RoomImage
    {
        [Key]
        public int id { get; set; }
        public string image { get; set; }

        public int RoomId { get; set; }
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime startDate { get; set;}
        public DateTime endDate { get; set;}

       // public bool Booked { get; set; }
        public int RoomId { get; set; }// Ссылка на комнату

        public int UserId { get; set; } // Ссылка на гостя
    }


    // Модель для представления информации о госте
    public class ModelUsers
    {
		public ModelUsers()
		{
			// Задаем роль по умолчанию
			Role = Role.User;
		}

        [Key]
		public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public string? PhoneNumber { get; set; }
        // Другие свойства, такие как адрес и т. д.
    }

    public enum Role
    {
        [EnumMember(Value = "Admin")]
        Admin,

        [EnumMember(Value = "User")]
        User
    }

    public class Reservation
    {
        public int Id { get; set; }
     
        public int RoomId { get; set; } // Ссылка на номер отеля
        [ForeignKey("RoomId")]
        public Room Room { get; set; } // Навигационное свойство
        public int GuestId { get; set; } // Ссылка на гостя
        [ForeignKey("GuestId")]

        public ModelUsers Guest { get; set; } // Навигационное свойство
        public int CountDay { get; set; } // Навигационное свойство
                                          //мб стартдэй и енддэй

        [Column(TypeName = "decimal(18, 2)")] 
        public decimal TotalPrice { get; set; }
        // Другие свойства, такие как дополнительные услуги, комментарии и т. д.
    }

    // Модель для представления информации о сотруднике отеля (если необходимо)
  

}
