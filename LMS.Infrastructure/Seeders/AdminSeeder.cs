using LMS.Domain.Common;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Seeders
{
    public static class AdminUserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            const string email = "admin@lms.com";
            const string password = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(email);

            if (adminUser != null)
                return;

            adminUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }
        }
    }
}
