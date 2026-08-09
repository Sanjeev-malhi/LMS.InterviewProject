using LMS.Domain.Common;
using LMS.Domain.Constants;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Infrastructure.Seeders;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        await SeedRoles(roleManager);

        var adminUser = await SeedAdmin(userManager);

        await AssignRole(userManager, adminUser, Roles.Admin);
    }

    private static async Task SeedRoles(
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        string[] roles =
        {
            Roles.Admin,
            Roles.Teacher,
            Roles.Student
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = role,
                    NormalizedName = role.ToUpperInvariant()
                });
            }
        }
    }

    private static async Task<ApplicationUser> SeedAdmin(
        UserManager<ApplicationUser> userManager)
    {
        var admin = await userManager.FindByEmailAsync(
            DefaultUsers.AdminEmail);

        if (admin != null)
            return admin;

        admin = new ApplicationUser
        {
            UserName = DefaultUsers.AdminEmail,
            Email = DefaultUsers.AdminEmail,
            FirstName = DefaultUsers.SystemAdminFirstName,
            LastName = DefaultUsers.SystemAdminLastName ,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(
            admin,
            DefaultUsers.AdminPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ",
                result.Errors.Select(x => x.Description));

            throw new Exception(errors);
        }

        return admin;
    }

    private static async Task AssignRole(
        UserManager<ApplicationUser> userManager,
        ApplicationUser user,
        string role)
    {
        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}