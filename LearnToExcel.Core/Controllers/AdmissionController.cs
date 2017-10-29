using System.Threading.Tasks;
using LearnToExcel.Core.Data;
using LearnToExcel.Core.Models;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewAdmissions(StudentRegistration vm)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    FirstName = vm.FirstName,
                    Surname = vm.Surname,
                    DateOfBirth = vm.DateofBirth,
                    IsActive = false,
                    KnownMedicalConditions = vm.MedicalConditions,
                    ShchoolYear = vm.YearAtSchool,
                    PaymentDay = vm.PaymentDay,
                    Gender = vm.Gender
                };

                var parent = new Parent
                {
                    Title = vm.Parent.Title,
                    FirstName = vm.Parent.FirstName,
                    Surname = vm.Parent.Surname,
                    Address = vm.Parent.Address,
                    PostalCode = vm.Parent.PostalCode
                };

                var mothersPhone = new Contact
                {
                    ContactTypeId = 1, // mom's phone
                    Name = vm.PrimaryContact
                };

                var fathersPhone = new Contact
                {
                    ContactTypeId = 2, // dad's phone
                    Name = vm.SecondaContact
                };

                var parentEmail = new Contact
                {
                    ContactTypeId = 3, //email
                    Name = vm.ParentsEmail
                };
                var contacts = new[]
                {
                    mothersPhone, fathersPhone, parentEmail
                };

                parent.Contacts = contacts;
                student.Parent = parent;
                student.DepartmentId = 1;
                
                _context.Students.Add(student);
                
                await _context.SaveChangesAsync();
                return View();
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