using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelWizard.Models
{
    /// <summary>
    /// Модель для представления статуса
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Уникальный идентификатор статуса
        /// </summary>
        [Key]
        [SwaggerSchema(Description = "Уникальный идентификатор статуса")]
        public int Id { get; set; }

        /// <summary>
        /// Название статуса
        /// </summary>
        [SwaggerSchema(Description = "Название статуса")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Копия модели статуса
    /// </summary>
    public class CopyOfStatus
    {
        /// <summary>
        /// Уникальный идентификатор копии статуса
        /// </summary>
        [Key]
        [SwaggerSchema(Description = "Уникальный идентификатор копии статуса")]
        public int Id { get; set; }

        /// <summary>
        /// Название копии статуса
        /// </summary>
        [SwaggerSchema(Description = "Название копии статуса")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Модель для представления информации о номере отеля
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Уникальный идентификатор номера
        /// </summary>
        [Key]
        [SwaggerSchema(Description = "Уникальный идентификатор номера")]
        public int Id { get; set; }

        /// <summary>
        /// Номер комнаты (например, 101, 102)
        /// </summary>
        [SwaggerSchema(Description = "Номер комнаты (например, 101, 102)")]
        public string Number { get; set; }

        /// <summary>
        /// Тип комнаты (например, одноместный, двухместный, люкс)
        /// </summary>
        [SwaggerSchema(Description = "Тип комнаты (например, одноместный, двухместный, люкс)")]
        public string Type { get; set; }

        /// <summary>
        /// Описание комнаты (например, расположение, площадь, дополнительные удобства)
        /// </summary>
        [SwaggerSchema(Description = "Описание комнаты (например, расположение, площадь, дополнительные удобства)")]
        public string Description { get; set; }

        /// <summary>
        /// Цена за ночь проживания в данном номере
        /// </summary>
        [SwaggerSchema(Description = "Цена за ночь проживания в данном номере")]
        public double PricePerNight { get; set; }

        /// <summary>
        /// Коллекция изображений, связанных с данным номером
        /// </summary>
        [SwaggerSchema(Description = "Коллекция изображений, связанных с данным номером")]
        public ICollection<RoomImage> ImageArray { get; set; } = new List<RoomImage>();
    }

    /// <summary>
    /// Модель для представления информации о изображении номера
    /// </summary>
    public class RoomImage
    {
        /// <summary>
        /// Уникальный идентификатор изображения
        /// </summary>
        [Key]
        [SwaggerSchema(Description = "Уникальный идентификатор изображения")]
        public int id { get; set; }

        /// <summary>
        /// Путь к изображению
        /// </summary>
        [SwaggerSchema(Description = "Путь к изображению")]
        public string image { get; set; }

        /// <summary>
        /// Идентификатор номера, к которому относится изображение
        /// </summary>
        [ForeignKey("RoomId")]
        [SwaggerSchema(Description = "Идентификатор номера, к которому относится изображение")]
        public int RoomId { get; set; }
    }

    /// <summary>
    /// Модель для представления информации о заказе
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Уникальный идентификатор заказа
        /// </summary>
        [Key]
        [SwaggerSchema(Description = "Уникальный идентификатор заказа")]
        public int Id { get; set; }

        /// <summary>
        /// Дата начала проживания
        /// </summary>
        [SwaggerSchema(Description = "Дата начала проживания")]
        public DateTime startDate { get; set; }

        /// <summary>
        /// Дата окончания проживания
        /// </summary>
        [SwaggerSchema(Description = "Дата окончания проживания")]
        public DateTime endDate { get; set; }

        /// <summary>
        /// Идентификатор статуса заказа
        /// </summary>
        [SwaggerSchema(Description = "Идентификатор статуса заказа")]
        public int StatusId { get; set; }

        /// <summary>
        /// Идентификатор номера, который был заказан
        /// </summary>
        [SwaggerSchema(Description = "Идентификатор номера, который был заказан")]
        public int RoomId { get; set; }

        /// <summary>
        /// Идентификатор пользователя, сделавшего заказ
        /// </summary>
        [SwaggerSchema(Description = "Идентификатор пользователя, сделавшего заказ")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// Модель для представления информации о пользователе
    /// </summary>
    public class ModelUsers
    {
        public ModelUsers()
        {
            // Задаем роль по умолчанию
            Role = Role.User;
        }

        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        [Key]
        [SwaggerSchema(Description = "Уникальный идентификатор пользователя")]
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [SwaggerSchema(Description = "Имя пользователя")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [SwaggerSchema(Description = "Фамилия пользователя")]
        public string? Surname { get; set; }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        [SwaggerSchema(Description = "Отчество пользователя")]
        public string? LastName { get; set; }

        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        [SwaggerSchema(Description = "Электронная почта пользователя")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [SwaggerSchema(Description = "Пароль пользователя")]
        public string Password { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        [SwaggerSchema(Description = "Роль пользователя")]
        public Role Role { get; set; }

        /// <summary>
        /// Номер телефона пользователя
        /// </summary>
        [SwaggerSchema(Description = "Номер телефона пользователя")]
        public string? PhoneNumber { get; set; }
    }

    /// <summary>
    /// Перечисление для ролей пользователей
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Роль администратора
        /// </summary>
        [EnumMember(Value = "Admin")]
        Admin,

        /// <summary>
        /// Роль пользователя
        /// </summary>
        [EnumMember(Value = "User")]
        User
    }
}
