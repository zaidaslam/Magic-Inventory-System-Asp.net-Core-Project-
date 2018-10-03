using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Assignment2.Models;

namespace Assignment2.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { Constants.FranchiseRole, Constants.OwnerRole };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            await EnsureUserHasRole(userManager, "ownerrole@gmail.com", Constants.OwnerRole);
            await EnsureUserHasRole(userManager, "franchiserole@gmail.com", Constants.FranchiseRole);
            await EnsureUserHasRole(userManager, "Franchise1@example.com", Constants.FranchiseRole);
            await EnsureUserHasRole(userManager, "Franchise2@example.com", Constants.FranchiseRole);
            await EnsureUserHasRole(userManager, "Franchise3@example.com", Constants.FranchiseRole);
            await EnsureUserHasRole(userManager, "Franchise4@example.com", Constants.FranchiseRole);
        }
        private static async Task EnsureUserHasRole(UserManager<ApplicationUser> userManager, string userName, string role)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user != null && !await userManager.IsInRoleAsync(user, role))
            { await userManager.AddToRoleAsync(user, role); }
        }
    }
}
