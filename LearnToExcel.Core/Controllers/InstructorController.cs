using System.Collections.Generic;
using LearnToExcel.Core.Data;
using LearnToExcel.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LearnToExcel.Core.Models.InstructorViewModels;
using Microsoft.EntityFrameworkCore;

namespace LearnToExcel.Core.Controllers
{
    [Authorize(Roles = "administrator")]
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InstructorController> _logger;
        public InstructorController(ApplicationDbContext context, ILogger<InstructorController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index(int? id, int? courseId)
        {
            var viewModel = new InstructorIndexData();
            var instructors = await _context.Instructors.OrderBy(i => i.Surname).ToListAsync();
            viewModel.Instructors = instructors;
            List<CourseInstructor> courses =
                new List<CourseInstructor>(await _context.CourseInstructors.Include(c => c.Course).ThenInclude(d => d.Department).ToListAsync());
            viewModel.CourseInstructors = courses;
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var instructor = new Instructor();
            await PopulateAssignedCourseData(instructor);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Instructor vm, string[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(vm);
                await _context.SaveChangesAsync();
                await UpdateInstructorCourses(selectedCourses, vm);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Instructor instructor = await _context.Instructors.SingleAsync(i => i.Id == id);
            await PopulateAssignedCourseData(instructor);
            if (instructor == null)
            {
                return BadRequest();
            }
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, Instructor vm, string[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                var instructorToUpdate = _context.Instructors.Single(i => i.Id == id);
                if (instructorToUpdate != null)
                {
                    instructorToUpdate.FirstName = vm.FirstName;
                    instructorToUpdate.Surname = vm.Surname;
                    instructorToUpdate.Email = vm.Email;
                    instructorToUpdate.HireDate = vm.HireDate;
                    instructorToUpdate.Gender = vm.Gender;
                    _context.Entry(instructorToUpdate).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    if (selectedCourses != null)
                    {
                       await UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                    }
                }

            }
            await PopulateAssignedCourseData(vm);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = await _context.Courses.Include(c => c.Department).ToListAsync();
            //var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseId));
            var instructorCourses = new HashSet<int>(_context.CourseInstructors.Where(i => i.InstructorId == instructor.Id).Select(c => c.CourseId));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseId = course.CourseId,
                    Title = string.Format("{0} - {1}", course.Title, course.Department.Name),
                    Assigned = instructorCourses.Contains(course.CourseId)

                });
            }
            ViewBag.Courses = viewModel;
        }

        private async Task UpdateInstructorCourses(string[] selectedCourses, Instructor instructor)
        {
            var courseInstructors = _context.CourseInstructors.Where(ci => ci.InstructorId == instructor.Id).ToList();
            var courses = _context.Courses.ToList();
            if (selectedCourses == null)
            {
                return;
            }

            var selectedCoursesHs = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(courseInstructors.Select(c => c.CourseId));

            foreach (var course in courses)
            {
                if (selectedCoursesHs.Contains(course.CourseId.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseId))
                    {
                        _context.CourseInstructors.Add(new CourseInstructor { CourseId = course.CourseId, InstructorId = instructor.Id });
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseId))
                    {
                        var toRemove = courseInstructors.Single(ci => ci.CourseId == course.CourseId);
                        _context.CourseInstructors.Remove(toRemove);
                    }
                }
            }
            await _context.SaveChangesAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Instructor instructor = await _context.Instructors.Where(i => i.Id == id).SingleAsync();
            if (instructor == null)
            {
                return BadRequest();
            }
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
