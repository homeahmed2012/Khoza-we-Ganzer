using AMMM.Ganzer.App.Data;
using AMMM.Ganzer.App.Models;
using AMMM.Ganzer.App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft;
using Newtonsoft.Json;

namespace AMMM.Ganzer.App.Controllers
{
    //[Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddRide()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRide(AddRideViewModel rideViewModel)
        {
            if (ModelState.IsValid)
            {
                var newRide = new Ride()
                {
                    Name = rideViewModel.Name,
                    RideType = (int)rideViewModel.RideType,
                    RideDate = DateOnly.Parse(rideViewModel.RideDate),
                    Description = rideViewModel.Description,
                    RideTime = TimeOnly.Parse(rideViewModel.RideTime),
                    Distance = rideViewModel.Distance,
                    GatheringPoint = rideViewModel.GatheringPoint,
                    Distenation = rideViewModel.Distenation,
                    Temperature = rideViewModel.Temperature,
                    WindDirection = rideViewModel.WindDirection,
                    WindIntensity = (int?)rideViewModel.WindIntensity,
                    Rain = (int?)rideViewModel.Rain,
                    IsActive = true
                };

                _dbContext.Rides.Add(newRide);
                _dbContext.SaveChanges();
                
                return RedirectToAction("Index");
            };
            return View(rideViewModel);
        }

        [HttpGet]
        [Route("DeleteRide/{id:int}")]
        public IActionResult DeleteRide(int id)
        {
            var ride = _dbContext.Rides.FirstOrDefault(r => r.RideID == id);
            if(ride != null)
            {
                _dbContext.Rides.Remove(ride);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("RidesList");
        }

        [HttpGet]
        public IActionResult RidesList()
        {
            var rides = _dbContext.Rides
                .Where(r => r.IsActive)
                .OrderByDescending(r => r.RideDate).Take(20)
                .ToList();

            return View(rides);
        }


        [HttpGet]
        [Route("TakeRideAttendance/{rideId:int}")]
        public IActionResult TakeRideAttendance(int rideId)
        {
            var ride = _dbContext.Rides.Include("ApplicationUsers").FirstOrDefault(r => r.RideID == rideId);
            if(ride != null)
            {
                var rideAttendees = ride.ApplicationUsers.Select(u => u.Id).ToList();
                var bikers = _dbContext.Users.Select(u => new AttendeesViewModel()
                {
                    UserId = u.Id,
                    Name = u.Name,
                    Phone = u.PhoneNumber,
                    Email = u.Email,
                    Attend = rideAttendees.Contains(u.Id)
                }).ToList();
                ViewBag.rideId = rideId;
                return View(bikers);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddUserToRide(string userId, int rideId)
        {
            var ride = _dbContext.Rides.FirstOrDefault(r => r.RideID == rideId);
            var user = _dbContext.Users.Include("Rides").FirstOrDefault(u => u.Id == userId);
            if(user != null && ride != null)
            {
                user.Rides.Add(ride);
                user.Points += GetRidePoints(ride.RideType);
                _dbContext.SaveChanges();
                return Json(JsonConvert.SerializeObject(new { attend = true }));
            }

            return Json(JsonConvert.SerializeObject(new { attend = false }));
        }

        private int GetRidePoints(int rideType)
        {
            switch (rideType)
            {
                case 1:
                    return 4;
                case 2:
                    return 5;
                case 3:
                    return 2;
                case 4:
                    return 3;
                case 5:
                    return 7;
                case 6:
                    return 6;
                case 7:
                    return 10;
                default:
                    return 0;
            }
        }
    }
}
