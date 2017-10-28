using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnToExcel.Core.Data;
using LearnToExcel.Core.Models.AdmissionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LearnToExcel.Core.Controllers
{
    [Authorize]
    public class AdmissionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdmissionController> _logger;
        public AdmissionController(ApplicationDbContext context, ILogger<AdmissionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult NewAdmissions()
        {
            return View();
        }


        public async Task<IActionResult> NewStudent(StudentRegistration vm)
        {
            if (ModelState.IsValid)
            {
                
            }
            return View(vm);
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