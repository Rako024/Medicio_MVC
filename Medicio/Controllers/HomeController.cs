using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Medicio.Controllers
{
    public class HomeController : Controller
    {
        UserManager<AppUser> _userManager;
        IDoctorService _doctorService;
        public HomeController(UserManager<AppUser> userManager, IDoctorService doctorService)
        {
            _userManager = userManager;
            _doctorService = doctorService;
        }

        public IActionResult Index()
        {
            List<Doctor> doctors = _doctorService.GetAllDoctors();
            return View(doctors);
        }
    }
}
