using System;
using System.Linq;
using System.Threading.Tasks;
using LearnToExcel.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace LearnToExcel.Core.Data
{
    public interface IDbInitializer
    {
        Task Initialize();

    }
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;



        public DbInitializer(

            ApplicationDbContext context,

            UserManager<ApplicationUser> userManager,

            RoleManager<IdentityRole> roleManager)

        {

            _context = context;

            _userManager = userManager;

            _roleManager = roleManager;

        }
        public async Task Initialize()
        {
            // create database schema if none exists
            await _context.Database.EnsureCreatedAsync();

            // seed roles and admin user
            const string adminRoleName = "administrator";
            const string employeeRoleName = "employee";
            const string parentRoleName = "parent";
            const string studentRoleName = "student";

            await EnsureRoleAsync(adminRoleName);
            await EnsureRoleAsync(employeeRoleName);
            await EnsureRoleAsync(parentRoleName);
            await EnsureRoleAsync(studentRoleName);

            await CreateUserAsync("admin@learntoexcel.co.uk", "admin@learntoexcel.co.uk", adminRoleName);
            await CreateUserAsync("employee-primary@learntoexcel.co.uk", "employee-primary@learntoexcel.co.uk", employeeRoleName);
            await CreateUserAsync("employee-secondary@learntoexcel.co.uk", "employee-secondary@learntoexcel.co.uk", employeeRoleName);

            await CreateContactTypes();
            await CreateCourses();
           
        }

        private async Task EnsureRoleAsync(string roleName)
        {
            if (await _roleManager.FindByNameAsync(roleName) == null)
            {
                var role = new IdentityRole(roleName);

                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                    throw new Exception($"Seeding \"{roleName}\" role failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string email, string role)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true
            };
            const string password = "Chang3m3.";
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

        private async Task CreateCourses()
        {
            var departments = new[]
            {
                new Department() {Name = "Primary", StartDate = DateTime.Now},
                new Department() {Name = "Secondary", StartDate = DateTime.Now},
            };
            if (!_context.Departments.Any())
            {
                foreach (var department in departments)
                {
                    _context.Departments.Add(department);
                }
            }
            await _context.SaveChangesAsync();
            if (!_context.Courses.Any())
            {
                var courses = new[]
                {
                    new Course() {CourseId = 1, Title = "Maths", Credits = 15 , DepartmentId = departments[0].DepartmentId},
                    new Course() {CourseId = 2, Title = "English", Credits = 15, DepartmentId = departments[0].DepartmentId},
                    new Course() {CourseId = 3, Title = "Maths", Credits = 15,DepartmentId = departments[1].DepartmentId},
                    new Course() {CourseId = 4, Title = "English", Credits = 15,DepartmentId = departments[1].DepartmentId},
                    new Course() {CourseId = 5, Title = "Science", Credits = 15,DepartmentId = departments[1].DepartmentId}
                };

                foreach (var couse in courses)
                {
                    _context.Courses.Add(couse);
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task CreateContactTypes()
        {
            if (!_context.ContactTypes.Any())
            {
                var contactTypes = new[]
                {
                    new ContactType() { Code = "PRIMARY_CONTACT", Type = "Mother Phone"},
                    new ContactType() {Code = "SECONDARY_CONTACT", Type = "Father Phone"},
                    new ContactType() {Code = "EMAIL", Type = "Parent Email"},
                };

                foreach (var contactType in contactTypes)
                {
                    _context.ContactTypes.Add(contactType);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
