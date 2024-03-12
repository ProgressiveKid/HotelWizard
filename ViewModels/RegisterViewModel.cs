using System.ComponentModel.DataAnnotations;

namespace HotelWizard.ViewModels
{
	public class RegisterViewModel
	{
		// тут будут и другие поля позже
		[Required(ErrorMessage = "Не указан Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
        [Required(ErrorMessage = "Не указан пароль")]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
		public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Не указан Имя")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Не указан Отчество")]
        public string? Surname { get; set; }
        [Required(ErrorMessage = "Не указано Мамилия")]
        public string? LastName { get; set; }

    }
}
