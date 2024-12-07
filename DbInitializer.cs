using IdentityProject.Constants;
using IdentityProject.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject
{
    public static class DbInitializer
    {
        public static void Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            AddRoles(roleManager);
            AddAdmin(userManager, roleManager);
        }
        private static void AddRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues<UserRoles>())
            {
                if (!roleManager.RoleExistsAsync(role.ToString()).Result)
                {
                    _ = roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });
                }
            }
        }
        private static void AddAdmin(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {

            if (userManager.FindByEmailAsync("admin@code.edu.az").Result is null)
            {

                var user = new User
                {
                    UserName = "admin@code.edu.az",
                    Email = "admin@code.edu.az",
                    Country = "Azerbaycan",
                    City = "Sumqayit"
                };

                var result = userManager.CreateAsync(user, "Admin123!").Result;
                if (!result.Succeeded)
                    throw new Exception("Could not add admin");

                var role = roleManager.FindByNameAsync("Admin").Result;
                if (role?.Name is null)
                    throw new Exception("Could not find admin role");

                var addToResult = userManager.AddToRoleAsync(user, role.Name).Result;
                if (!addToResult.Succeeded)
                    throw new Exception("It was not possible to add the admin role to the user");
            }
        }
    }
}
