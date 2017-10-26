using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LearnToExcel.Core.Controllers
{
    public class AdmissionController : Controller
    {
        public IActionResult NewAdmissions()
        {
            return View();
        }

        public IActionResult AdmittedAdmissions()
        {
            return View();
        }

        public IActionResult PendingAdmissions()
        {
            return View();
        }
    }
}