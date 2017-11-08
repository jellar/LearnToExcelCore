using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnToExcel.Core.Data;
using LearnToExcel.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using LearnToExcel.Core.Models.AccountViewModels;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace LearnToExcel.Core.Controllers
{
    [Authorize(Roles = "administrator,employee")]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InstructorController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;
        public ManageController(ApplicationDbContext context, ILogger<InstructorController> logger, 
            UserManager<ApplicationUser> userManager, IToastNotification toastNotification)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }
        [Authorize(Roles = "employee")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "administrator")]
        public IActionResult Users()
        {
            return View();
        }

        [Authorize(Roles = "administrator")]
        public JsonResult LoadUsers()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;


                // Getting all Customer data  
                var users = (from i in _context.Instructors
                    from u in _context.Users
                        .Where(u => u.Email == i.Email).DefaultIfEmpty()
                    select new
                    {
                        i.Id,
                        i.FirstName,
                        i.Surname,
                        u.Email
                    }).AsQueryable();

               

                ////Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    users = users.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                ////Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    users = users.Where(m => m.FirstName.Contains(searchValue) ||
                                                         m.Surname.Contains(searchValue));

                }

                //total number of rows count   
                recordsTotal = users.Count();

                //Paging   
                var data = users.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data 

                //return Json(data);
                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = data
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var instructor = await _context.Instructors.SingleOrDefaultAsync(i => i.Id == id);
            if (instructor == null)
            {
                return BadRequest();
            }
            var registerViewModel = new RegisterViewModel()
            {
                Email = instructor.Email
            };
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await CreateUserAsync(vm.Email, vm.Email, "employee", vm.Password);
                if (user == null)
                {
                    _toastNotification.AddErrorToastMessage("User creation failed please try again.");
                    return View(vm);
                }
                return RedirectToAction(nameof(Users));
            }
            return View(vm);
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string email, string role, string password)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true
            };
            
            if (await _userManager.FindByEmailAsync(email) != null) return applicationUser;

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (!result.Succeeded) return applicationUser;
            var resultRole = await _userManager.AddToRoleAsync(applicationUser, role);
            if (!resultRole.Succeeded)
            {
                throw new Exception(
                    $"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, resultRole.Errors)}");
            }

            return applicationUser;

        }

        public async Task<IActionResult> ChangePassword(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var instructor = await _context.Instructors.SingleOrDefaultAsync(i => i.Id == id);
            var user = await _userManager.FindByEmailAsync(instructor.Email);
            if (user == null)
            {
                _toastNotification.AddErrorToastMessage("Not found");
                return BadRequest();
            }
            var registerViewModel = new RegisterViewModel()
            {
                Email = instructor.Email
            };
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user == null)
                {
                    _toastNotification.AddErrorToastMessage("Password reset failed please try again.");
                    return View(vm);
                }
                
                await _userManager.ResetPasswordAsync(user,null, vm.Password);
                return RedirectToAction(nameof(Users));
            }
            return View(vm);
        }
    }
}