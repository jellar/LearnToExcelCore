using System.Collections.Generic;
using LearnToExcel.Core.Data;
using LearnToExcel.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
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
        public IActionResult Index(int? id, int? courseId)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = _context.Instructors.OrderBy(i=>i.Surname);
            foreach (var instructor in viewModel.Instructors)
            {
                instructor.Courses = _context.CourseInstructor.Where(ci => ci.InstructorId == instructor.Id)
                    .Select(c => c.Course).Include(d=>d.Department).ToList();
                
            }
           
            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors.Where(
                    i => i.Id == id.Value).Single().Courses;
            }

            if (courseId != null)
            {
                ViewBag.CourseID = courseId.Value;
                // Lazy loading
                //viewModel.Enrollments = viewModel.Courses.Where(
                //    x => x.CourseID == courseID).Single().Enrollments;
                // Explicit loading
                var selectedCourse = viewModel.Courses.Where(x => x.CourseId == courseId).Single();
                _context.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    _context.Entry(enrollment).Reference(x => x.Student).Load();
                }

                viewModel.Enrollments = selectedCourse.Enrollments;
            }

            return View(viewModel);
        }

        public ActionResult Create()
        {
            var instructor = new Instructor
            {
                Courses = new List<Course>()
            };
            PopulateAssignedCourseData(instructor);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Instructor vm, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                var assignedCourses = new CourseInstructor();
               
                vm.Courses = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    assignedCourses.Course = _context.Courses.Find(int.Parse(course));
                    assignedCourses.Instructor = vm;
                    _context.CourseInstructor.Add(assignedCourses);
                }
                
            }
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(vm);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateAssignedCourseData(vm);
            return RedirectToAction(nameof(Index));
        }


        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses.Include(c=>c.Department);
            //var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseId));
            var instructorCourses = new HashSet<int>(_context.CourseInstructor.Where(i=>i.InstructorId == instructor.Id).Select(c=>c.CourseId));
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

        // GET: Instructor/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Instructor instructor = _context.Instructors
                .Where(i => i.Id == id).Single();
            PopulateAssignedCourseData(instructor);
            if (instructor == null)
            {
                return BadRequest();
            }
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCourses)
        {
            var instructorToUpdate = _context.Instructors.Where(i => i.Id == id).Single();
            if (selectedCourses != null)
            {
                var assignedCourses = new CourseInstructor();
                instructorToUpdate.Courses = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    assignedCourses.Course = _context.Courses.Find(int.Parse(course));
                    assignedCourses.Instructor = instructorToUpdate;
                    _context.CourseInstructor.Add(assignedCourses);
                }

            }
            if (ModelState.IsValid)
            {
                //_context.Instructors.Add(instructorToUpdate);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateAssignedCourseData(instructorToUpdate);
            return RedirectToAction(nameof(Index));
        }
    }
}