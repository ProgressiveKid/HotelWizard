using HotelWizard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using HotelWizard.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelWizard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        ApplicationContext db;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        public HomeController(ApplicationContext context, IStringLocalizer<HomeController> localizer,
         IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        {
            db = context;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }
        public class DateRange
        {
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        [HttpPost("GetData")]
        [SwaggerOperation(Description = "Метод для получении информации о доступных номерах на определенный период")]
        public async Task<JsonResult> GetData(
            [FromBody] DateRange dateRange)
        {
            //Проверка по всем заказам на определенную дату
            DateTime startDate = DateTime.ParseExact(dateRange.StartDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(dateRange.EndDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var listOrders = db.Orders.ToList();
			var listRooms = db.Rooms
				.Include(r => r.ImageArray) // Включить изображения для каждой комнаты
				.ToList(); 
            List <Room> freeRooms = new List<Room>();

            //!Не трогатть
            foreach (var room in listRooms)
            {  // проходися по всем комнатам
                var listOrdersForRoom = db.Orders.Where(u => u.RoomId == room.Id).ToList();
                bool isRoomAvailable = true;
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

        [HttpPost("MakeOrder")]
        [SwaggerOperation(Description = "Метод для создания заказа на бронирование")]
        public JsonResult MakeOrder(
            [SwaggerParameter(Description = "Дата начала проживания")] string startDate1,
            [SwaggerParameter(Description = "Дата окончания проживания")] string endDate1,
            [SwaggerParameter(Description = "Идентификатор номера для бронирования")] int idRoom)
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
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var rooms = await db.Rooms.ToListAsync();
            ViewData["Rooms"] = rooms;       
            return View();
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("GetInfoAboutUser")]
        [SwaggerOperation(Description = "Получение информации о пользователе")]
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
        
        [HttpPost("DeleteOrder")]
        public IActionResult DeleteOrder(int orderIdP, string userEmailP)
        {
            Console.WriteLine("РАботаем");
            ModelUsers user = db.Users.AsNoTracking().FirstOrDefault(u => u.Email == userEmailP);
            Order order = db.Orders.FirstOrDefault(u => u.Id == orderIdP && u.UserId == user.Id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return Ok(user);
        }

        /// <summary>
        /// Создаёт новые аппартаменты в отеле
        /// </summary>
        /// <param name="room">Информация о номере</param>
        /// <param name="images">Список изображений для номера</param>
        /// <returns>Возвращает результат выполнения операции.</returns>
        [HttpPost("Create")]
        [SwaggerOperation(Description = "Метод для добавления новых номеров в отель с изображениями.")]
        public async Task<IActionResult> Create(
        [SwaggerParameter(Description = "Информация о номере")] Room room,
        [SwaggerParameter(Description = "Список изображений для номера")] List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {

                // Проверяем, все ли изображения имеют правильное расширение
                bool allImagesValid = images.All(image =>
                {
                    var fileExtension = Path.GetExtension(image.FileName).ToLower();
                    return fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".jpg"; ;
                });

                if (!allImagesValid)
                {
                    return Json(new { success = false, error = "Только изображения форматов JPEG и PNG разрешены." });
                }
                if (room.PricePerNight <= 0)
                {
                    return Json(new { success = false, error = "Цена за ночь должна быть больше 0." });
                }
                // Сохраняем номер
                db.Rooms.Add(room);
                await db.SaveChangesAsync();

                // Путь для хранения изображений
                var roomFolder = Path.Combine(_environment.WebRootPath, "images", "Rooms", room.Id.ToString());

                // Создаём папку для комнат, если её нет
                if (!Directory.Exists(roomFolder))
                {
                    Directory.CreateDirectory(roomFolder);
                }

                // Сохранение изображений
                if (images != null && images.Count > 0)
                {
                    int photoNumber = 1; // Нумерация изображений
                    foreach (var image in images)
                    {
                        if (image.Length > 0)
                        {
                            // Генерируем имя файла и путь
                            var fileName = $"{photoNumber}.jpeg";
                            var filePath = Path.Combine(roomFolder, fileName);

                            // Сохраняем файл на диск
                            using var stream = new FileStream(filePath, FileMode.Create);
                            await image.CopyToAsync(stream);

                            // Сохраняем путь к файлу в базе данных
                            var roomImage = new RoomImage
                            {
                                RoomId = room.Id,
                                image = $"/images/Rooms/{room.Id}/{fileName}"
                            };
                            db.RoomImages.Add(roomImage);

                            photoNumber++;
                        }
                    }

                    await db.SaveChangesAsync();
                }

                return Json(new { success = true });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
    }
}