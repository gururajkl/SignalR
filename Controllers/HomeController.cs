using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Data;
using SignalRSample.Hubs;
using SignalRSample.Models;
using SignalRSample.Models.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SignalRSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<DeathlyHallowsHub> hubContext;
        private readonly IHubContext<OrderHub> orderHubContext;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IHubContext<DeathlyHallowsHub> hubContext,
            ApplicationDbContext context, IHubContext<OrderHub> orderHubContext)
        {
            _logger = logger;
            this.hubContext = hubContext;
            _context = context;
            this.orderHubContext = orderHubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DeathlyHallows(string type)
        {
            if (StaticDetails.deathlyHallowRace.ContainsKey(type))
            {
                StaticDetails.deathlyHallowRace[type]++;
            }

            await hubContext.Clients.All.SendAsync("updateDeathlyHollowsCount",
            StaticDetails.deathlyHallowRace[StaticDetails.cloak],
            StaticDetails.deathlyHallowRace[StaticDetails.stone],
            StaticDetails.deathlyHallowRace[StaticDetails.wand]);

            return View("DeathlyHallows", type.ToString());
        }

        public IActionResult Notification()
        {
            return View();
        }

        public IActionResult DeathlyHallowRace()
        {
            return View();
        }

        public IActionResult HarryPotterHouse()
        {
            return View();
        }

        public IActionResult BasicChat()
        {
            return View();
        }

        [Authorize]
        public IActionResult Chat()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ChatViewModel chatViewModel = new()
            {
                MaxRoomAllowed = 4,
                UserId = userId,
                Rooms = _context.ChatRooms.ToList()
            };

            return View(chatViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Orders Methods.
        [ActionName("Order")]
        public IActionResult Order()
        {
            string[] name = { "Bhrugen", "Ben", "Jess", "Laura", "Ron" };
            string[] itemName = { "Food1", "Food2", "Food3", "Food4", "Food5" };

            Random rand = new Random();
            // Generate a random index less than the size of the array.  
            int index = rand.Next(name.Length);

            Order order = new Order()
            {
                Name = name[index],
                ItemName = itemName[index],
                Count = index
            };

            return View(order);
        }

        [ActionName("Order")]
        [HttpPost]
        public async Task<IActionResult> OrderPost(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            await orderHubContext.Clients.All.SendAsync("newOrderAdded");
            return RedirectToAction(nameof(Order));
        }

        [ActionName("OrderList")]
        public IActionResult OrderList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllOrder()
        {
            var productList = _context.Orders.ToList();
            return Json(new { data = productList });
        }
    }
}
