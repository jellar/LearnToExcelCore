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
using AutoMapper;
using AutoMapper.QueryableExtensions;

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
        public IActionResult Index(int? id, int? courseId)
        {
            var instructors = _context.Instructors
                .OrderBy(i => i.Surname).ToList();


            var courses = new List<Course>();
            var viewModel = new InstructorIndexData()
            {
                Instructors = instructors,
                Courses = courses
            };
            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Instructor vm, string[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(vm);
                var courses = _context.Courses.ToList();

                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses.Include(c => c.Department);
            //var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseId));
            var instructorCourses = new HashSet<int>(instructor.CourseInstructor.Where(i => i.InstructorId == instructor.Id).Select(c => c.CourseId));
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
        private void PopulateAssignedCourseData(Instructor model)
        {
            var allCourses = _context.Courses;
            var instructorCourses = new HashSet<int>(model.CourseInstructors.Select(c => c.CourseID));
            var viewModel = allCourses.Select(course => new Command.AssignedCourseData
            {
                CourseID = course.Id,
                Title = course.Title,
                Assigned = instructorCourses.Contains(course.Id)
            }).ToList();
            model.AssignedCourses = viewModel;
        }
        private void UpdateInstructorCourses(string[] selectedCourses, IEnumerable<Course> courses, Instructor instructor)
        {
            var CourseInstructors = new List<CourseInstructor>();
            if (selectedCourses == null)
            {
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (CourseInstructors.Select(c => c.CourseId));

            foreach (var course in courses)
            {
                if (selectedCoursesHS.Contains(course.CourseId.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseId))
                    {
                        CourseInstructors.Add(new CourseInstructor { Course = course, Instructor = instructor });
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseId))
                    {
                        var toRemove = CourseInstructors.Single(ci => ci.CourseId == course.CourseId);
                        CourseInstructors.Remove(toRemove);
                    }
                }
            }
        }
    }
}