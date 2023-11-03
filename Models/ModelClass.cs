﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        //Этаж
        //Расположение
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }
        // TODO добавить параметр - посадочных мест
        public ICollection<RoomImage> ImageArray{ get; set; } = new List<RoomImage>();        //public bool IsBooked{ get; set; } // статус
        //public ICollection<DateBooked> DateBreakFree { get; set; } // когда освободиться
        // Другие свойства, такие как описание, изображение и т. д.
        //public Room()
        //{
        //    DateBreakFree = new List<DateBooked>();
        //}
    }

    public class RoomImage
    {
        [Key]
        public int id { get; set; }
        public string image { get; set; }

        public int RoomId { get; set; }
    }

    public class DateBooked
    {
        [Key]
        public int Id { get; set; }
        public DateTime startDate { get; set;}
        public DateTime endDate { get; set;}

       // public bool Booked { get; set; }

        public int RoomId { get; set; }// Ссылка на комнату

    }
    // Модель для представления информации о госте
    public class Guest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        // Другие свойства, такие как адрес и т. д.
    }

    // Модель для представления информации о бронировании номера
    public class Reservation
    {
        public int Id { get; set; }
     
        public int RoomId { get; set; } // Ссылка на номер отеля
        [ForeignKey("RoomId")]
        public Room Room { get; set; } // Навигационное свойство
        public int GuestId { get; set; } // Ссылка на гостя
        [ForeignKey("GuestId")]

        public Guest Guest { get; set; } // Навигационное свойство
        public int CountDay { get; set; } // Навигационное свойство
        //мб стартдэй и енддэй
        
        public decimal TotalPrice { get; set; }
        // Другие свойства, такие как дополнительные услуги, комментарии и т. д.
    }

    // Модель для представления информации о сотруднике отеля (если необходимо)
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        // Другие свойства, такие как контактные данные и т. д.
    }

}
