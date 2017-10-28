﻿using System;
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
            await CreateUserAsync("employee@learntoexcel.co.uk", "employee@learntoexcel.co.uk", employeeRoleName);

            if (_context.Courses.Any())
            {
                return;
            }
            var courses = new[]
            {
                new Course() {CourseId = 1, Title = "Maths Only", Credits = 15},
                new Course() {CourseId = 2, Title = "English Only", Credits = 15},
                new Course() {CourseId = 3, Title = "English with Maths & Science", Credits = 25}
            };

            foreach (var couse in courses)
            {
                _context.Courses.Add(couse);
            }
            // await _context.SaveChangesAsync();

            if (_context.ContactTypes.Any())
            {
                return;
            }
            var contactTypes = new[]
            {
                new ContactType() {Id = 1, Code = "PRIMARY_CONTACT", Type = "Mother Phone"},
                new ContactType() {Id = 2, Code = "SECONDARY_CONTACT", Type = "Father Phone"},
                new ContactType() {Id = 3, Code = "EMAIL", Type = "Parent Email"},
            };

            foreach (var contactType in contactTypes)
            {
                _context.ContactTypes.Add(contactType);
            }
            await _context.SaveChangesAsync();
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
    }
}
