using HotelWizard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using HotelWizard.ViewModels;
using Microsoft.Extensions.Localization;

namespace HotelWizard.Controllers
{
	public class AccountController : Controller
	{
		// GET: AccountController1
		ApplicationContext db;

        public AccountController(ApplicationContext context)
		{
			db = context;
		}


        [HttpGet]
        public IActionResult UserOffice()
        {
            ModelUsers user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
			// будет список доступных услуг: бар, развлечения
			// до какого числа он чилит в отеле - возможность продлить
			// кнопка выхода из учётки
            return View();
        }


        #region Auntefication

        public async Task<IActionResult> Autorisation()
		{
			if (User.Identity.IsAuthenticated)
			{
				// Ваш код, который будет выполнен, если пользователь авторизован
				var rooms = await db.Rooms.ToListAsync();
				return View();
			}
			else
			{
				// Ваш код, который будет выполнен, если пользователь не авторизован
				return View("Autorisation");
				// Пример перенаправления на страницу входа
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Autorisation(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				ModelUsers user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
				if (user != null)
				{
					await Authenticate(model.Email); // аутентификация

					return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError("", "Некорректные логин и(или) пароль");
			}
			return View(model);
		}

		private async Task Authenticate(string userName)
		{
			// создаем один claim
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
			};
			// создаем объект ClaimsIdentity
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), new AuthenticationProperties
            {
                IsPersistent = true, // или false в зависимости от ваших требований
                ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1) // установите желаемый срок действия
            });
        }

		[HttpGet]
		public IActionResult Registration()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Registration(RegisterViewModel userData)
		{
			if (ModelState.IsValid)
			{
				ModelUsers user = await db.Users.FirstOrDefaultAsync(u => u.Email == userData.Email);
				if (user == null)
				{
					// добавляем пользователя в бд
					db.Users.Add(new ModelUsers { Email = userData.Email, Password = userData.Password });
					await db.SaveChangesAsync();

					await Authenticate(userData.Email); // аутентификация

					return RedirectToAction("Index", "Home");
				}
				else
					ModelState.AddModelError("", "Некорректные логин и(или) пароль");
			}
			return View(userData);
		}
        #endregion

    }
}
