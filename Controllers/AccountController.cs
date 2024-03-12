using HotelWizard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using HotelWizard.ViewModels;
using Microsoft.Extensions.Localization;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> UserOffice()
        {
			if (User.IsInRole("Admin"))
			{
                List<string> usersList = await db.Users
					 .Select(user => $"{user.FirstName} {user.Surname} {user.LastName}/{user.Email}")
					 .ToListAsync();       
                ViewData["DataList"] = usersList;
                Console.WriteLine("Зашёл Батя");
            }
            else
			{
                string mail = User.Identity.Name;
				var aaaa = db.Users.ToList();
                ModelUsers user = await db.Users.FirstOrDefaultAsync(u => u.Email == mail);
                int userId = user.Id;
                List<Order> order = db.Orders.Where(order => order.UserId == userId).ToList();
                Console.WriteLine("Зашёл пользователь");
            }
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
					await Authenticate(model.Email, user.Role); // аутентификация
                    return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError("", "Некорректные логин и(или) пароль");
			}
			return View(model);
		}
		private async Task Authenticate(string userName, Role role)
		{
			// создаем один claim
			 var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role.ToString())
            };
            // создаем объект ClaimsIdentity
            //ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
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
					db.Users.Add(new ModelUsers { Email = userData.Email, Password = userData.Password, Role = Role.User,
						FirstName = userData.FirstName, LastName = userData.LastName, Surname = userData.Surname
					});
					await db.SaveChangesAsync();
					await Authenticate(userData.Email, Role.User); // аутентификация
					return RedirectToAction("Index", "Home");
				}
				else
					ModelState.AddModelError("", "Некорректные логин и(или) пароль");
			}
			return View(userData);
		}
		[HttpPost]
		public  IActionResult UpdateUser(UserOfficeViewModel userDataP)
		{
			ModelUsers user = db.Users.FirstOrDefault(u=> u.Id == userDataP.Id);
			string [] fioComponent = userDataP.FIO.Split(' ');
			user.FirstName = fioComponent[0];
			user.Surname = fioComponent[1];
			user.LastName = fioComponent[2];
			user.Email = userDataP.Email;
			db.Users.Update(user);
			db.SaveChanges();
			return Ok(user);
		}
        #endregion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Вызываем SignOut для всех схем аутентификации, чтобы выйти из всех сессий
            HttpContext.SignOutAsync();
            // Редирект на страницу входа (или другую страницу)
            return RedirectToAction("Autorisation", "Account"); 
        }
    }

    public class CopyOfAccountController : Controller
    {
        // GET: AccountController1
        ApplicationContext db;
        public CopyOfAccountController(ApplicationContext context)
        {
            db = context;
        }
        public async Task<IActionResult> UserOffice()
        {
            if (User.IsInRole("Admin"))
            {
                List<string> usersList = await db.Users
                     .Select(user => $"{user.FirstName} {user.Surname} {user.LastName}/{user.Email}")
                     .ToListAsync();
                ViewData["DataList"] = usersList;
                Console.WriteLine("Зашёл Батя");
            }
            else
            {
                string mail = User.Identity.Name;
                var aaaa = db.Users.ToList();
                ModelUsers user = await db.Users.FirstOrDefaultAsync(u => u.Email == mail);
                int userId = user.Id;
                List<Order> order = db.Orders.Where(order => order.UserId == userId).ToList();
                Console.WriteLine("Зашёл пользователь");
            }
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
                    await Authenticate(model.Email, user.Role); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        private async Task Authenticate(string userName, Role role)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role.ToString())
            };
            // создаем объект ClaimsIdentity
            //ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
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
                    db.Users.Add(new ModelUsers
                    {
                        Email = userData.Email,
                        Password = userData.Password,
                        Role = Role.User,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        Surname = userData.Surname
                    });
                    await db.SaveChangesAsync();
                    await Authenticate(userData.Email, Role.User); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(userData);
        }
        [HttpPost]
        public IActionResult UpdateUser(UserOfficeViewModel userDataP)
        {
            ModelUsers user = db.Users.FirstOrDefault(u => u.Id == userDataP.Id);
            string[] fioComponent = userDataP.FIO.Split(' ');
            user.FirstName = fioComponent[0];
            user.Surname = fioComponent[1];
            user.LastName = fioComponent[2];
            user.Email = userDataP.Email;
            db.Users.Update(user);
            db.SaveChanges();
            return Ok(user);
        }
        #endregion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Вызываем SignOut для всех схем аутентификации, чтобы выйти из всех сессий
            HttpContext.SignOutAsync();
            // Редирект на страницу входа (или другую страницу)
            return RedirectToAction("Autorisation", "Account");
        }
    }
}
