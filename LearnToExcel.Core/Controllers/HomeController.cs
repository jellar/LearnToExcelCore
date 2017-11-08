using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnToExcel.Core.Models;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace LearnToExcel.Core.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IToastNotification _toastNotification;
        public HomeController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        [Authorize(Roles = "administrator,employee")]
        public IActionResult Index()
        {
            _toastNotification.AddInfoToastMessage("Welcome to LearnToExcel portal");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

