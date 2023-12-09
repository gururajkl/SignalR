using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Models;
using System.Diagnostics;

namespace SignalRSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<DeathlyHallowsHub> hubContext;

        public HomeController(ILogger<HomeController> logger, IHubContext<DeathlyHallowsHub> hubContext)
        {
            _logger = logger;
            this.hubContext = hubContext;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
