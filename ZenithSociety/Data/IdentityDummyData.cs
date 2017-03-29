using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZenithSociety.Models;

namespace ZenithSociety.Data
{
    public class IdentityDummyData
    {
        public static void Initialize(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            // Roles
            string[] roles = new string[] { "Admin", "Member" };



            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    roleStore.CreateAsync(new IdentityRole(role));
                }
            }


            // Users

            var user = new ApplicationUser
            {
                Email = "a@a.a",
                UserName = "a",
                FirstName = "a",
                LastName = "a"
            };


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "P@$$w0rd");
                user.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(user);
            }

            AssignRoles(serviceProvider, user.Email, roles);

            context.SaveChangesAsync();


        }

        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();

            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result;
        }
    }
}
