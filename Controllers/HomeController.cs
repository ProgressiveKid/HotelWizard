using HotelWizard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace HotelWizard.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost]
        public JsonResult GetData(string startDate1, string endDate1)
        {
            //Проверка по всем заказам на определенную дату
            //TODO сделать так чтобы можно было бранировать на дату выселения предыдщуего человека
            DateTime startDate = DateTime.Parse(startDate1);
            DateTime endDate = DateTime.Parse(endDate1);

            var listOrders = db.DateBookeds.ToList();
			var listRooms = db.Rooms
				.Include(r => r.ImageArray) // Включить изображения для каждой комнаты
				.ToList(); 
            List <Room> freeRooms = new List<Room>();

          //!Не трогатть
            foreach (var room in listRooms)
            {  // проходися по всем комнатам
                var listOrdersForRoom = db.DateBookeds.Where(u => u.RoomId == room.Id).ToList();
                bool isRoomAvailable = false;
                foreach (var order in listOrdersForRoom)
                { //проходимся по всем заказам текущей комнаты
                    if ((startDate >= order.endDate && endDate >= order.endDate) ||
                         (startDate <= order.startDate && endDate <= order.startDate))
                    {
                        isRoomAvailable = true;
                    }
                    else {
                        isRoomAvailable = false;
                        break;
                    }

                }
                if (isRoomAvailable)
                {
					freeRooms.Add(room);
                }

            }
            return Json(freeRooms);
        }

        [HttpPost]

        public JsonResult MakeOrder(string startDate1, string endDate1, int idRoom)
        {
            DateTime startDate = DateTime.Parse(startDate1);
            DateTime endDate = DateTime.Parse(endDate1);
            //Проверка по всем заказам на определенную дату
            //TODO сделать так чтобы можно было бранировать на дату выселения предыдщуего человека
            DateBooked newOrder = new DateBooked()
            {
                startDate = startDate,
                endDate = endDate,
                RoomId = idRoom

            };
       
            db.DateBookeds.Add(newOrder);
            db.SaveChanges();

            return Json("Its okay");

        }


        public async Task<IActionResult> Index()
        {
            var rooms = await db.Rooms.ToListAsync();
            ViewData["Rooms"] = rooms;
            return View();
        }


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
		public async Task<IActionResult> Autorisation(string mailLoginParam, string password)
		{
			return RedirectToAction("Index", "Home");
		}


		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}