using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnToExcel.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace LearnToExcel.Core.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize(Roles = "administrator,employee")]
        public IActionResult Index()
        {
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
