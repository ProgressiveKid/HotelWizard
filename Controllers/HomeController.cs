using HotelWizard.Models;
using System.Drawing.Printing;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using Spire.Pdf;
using HotelWizard.ViewModels;
namespace HotelWizard.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(ApplicationContext context, IStringLocalizer<HomeController> localizer,
         IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }
        private void SendMessage()
        {
            // Настройки SMTP-сервера Mail.ru
            string smtpServer = "smtp.mail.ru"; //smpt сервер(зависит от почты отправителя)
            int smtpPort = 587; // Обычно используется порт 587 для TLS
            string smtpUsername = "jostonn@mail.ru"; //твоя почта, с которой отправляется сообщение
            string smtpPassword = "123456";//пароль приложения (от почты)
            // Создаем объект клиента SMTP
            using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                // Настройки аутентификации
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(smtpUsername);
                    mailMessage.To.Add("konus228@mail.ru"); // Укажите адрес получателя
                    mailMessage.Subject = "Заголовок сообщения (тема)";
                    mailMessage.Body = $"Текст сообщения";
                    try
                    {
                        // Отправляем сообщение
                        smtpClient.Send(mailMessage);
                        Console.WriteLine("Сообщение успешно отправлено.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
                    }
                }
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetData(string startDate1, string endDate1)
        {
            //Проверка по всем заказам на определенную дату
            DateTime startDate = DateTime.ParseExact(startDate1, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(endDate1, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var listOrders = db.Orders.ToList();
			var listRooms = db.Rooms
				.Include(r => r.ImageArray) // Включить изображения для каждой комнаты
				.ToList(); 
            List <Room> freeRooms = new List<Room>();
            //SendMessage();
              //!Не трогатть
            foreach (var room in listRooms)
            {  // проходися по всем комнатам
                var listOrdersForRoom = db.Orders.Where(u => u.RoomId == room.Id).ToList();
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
            string mail = User.Identity.Name;
            ModelUsers user = db.Users.FirstOrDefault(user => user.Email == mail);
            Order newOrder = new Order()
            {
                startDate = startDate,
                endDate = endDate,
                RoomId = idRoom,
                UserId = user.Id
            };
            db.Orders.Add(newOrder);
            db.SaveChanges();
            return Json("Its okay");
        }
        [Authorize]
		public async Task<IActionResult> Index()
        {
            var rooms = await db.Rooms.ToListAsync();
            ViewData["Rooms"] = rooms;       
            return View();
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
        #region UserController
        #endregion
        #region AdminController
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult GetInfoAboutUser(string nameAndId)
        {
            ModelUsers selectedUser = new ModelUsers
            {
                FirstName = nameAndId.Split(' ')[0],
                Surname = nameAndId.Split(' ')[1],
                LastName = nameAndId.Split(' ')[2].Split('/')[0],
                Email = nameAndId.Split(' ')[2].Split('/')[1],
            };
            ModelUsers user = db.Users.Where(u => u.Email == selectedUser.Email).FirstOrDefault();
            List<Order> listOrders = db.Orders.Where(u => u.UserId == user.Id).ToList();
            UserOfficeViewModel userOfficeViewModel = new UserOfficeViewModel
            {
                Id = user.Id,
                FIO = user.FirstName + " " + user.Surname + " " + user.LastName,
                Email = user.Email,
                ListOrders = listOrders
            };
            return Json(userOfficeViewModel);
        }
        [HttpPost]
        public IActionResult DeleteOrder(int orderIdP, string userEmailP)
        {
            Console.WriteLine("РАботаем");
            ModelUsers user = db.Users.AsNoTracking().FirstOrDefault(u => u.Email == userEmailP);
            Order order = db.Orders.FirstOrDefault(u => u.Id == orderIdP && u.UserId == user.Id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return Ok(user);
        }
        #endregion
    }
}