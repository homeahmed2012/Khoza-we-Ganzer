using AMMM.Ganzer.App.Data;
using AMMM.Ganzer.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AMMM.Ganzer.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var user = User.Identity?.IsAuthenticated;
            if (user != null && user == true)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                string userId = _userManager.GetUserId(User);
                return RedirectToAction("Profile", new { userId = userId });
            }
            return View();
        }

        [Authorize]
        [Route("biker/{userId}")]
        public IActionResult Profile(string userId)
        {
            // get the user object
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult AllRides()
        {
            var rides = _dbContext.Rides
                .Where(r => r.IsActive)
                .OrderByDescending(r => r.RideDate).Take(20)
                .ToList();

            return View(rides);
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