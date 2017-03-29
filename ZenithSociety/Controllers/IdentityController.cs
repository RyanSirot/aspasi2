using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZenithSociety.Data;
using ZenithSociety.Models.IdentityViewModels;
using ZenithSociety.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ZenithSociety.Models;

namespace ZenithSociety.Controllers
{
    public class IdentityController : Controller
    {
        private ApplicationDbContext db;
        private IServiceProvider _services;


        public IdentityController(ApplicationDbContext context, IServiceProvider services)
        {
            db = context;
            _services = services;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddUsersToRoles()
        {
            if(ValidationHelper.codeForUserRole == 1)
            {
                ViewBag.State = 1;
            } else if(ValidationHelper.codeForUserRole == 2)
            {
                ViewBag.State = 2;
            } else
            {
                ViewBag.State = 0;
            }
            // change the code back 
            ValidationHelper.codeForUserRole = 0;

            // table 1
            UserRoleViewModel model = new UserRoleViewModel();
            var allUsers = db.Users.OrderBy(u => u.UserName).ToList();
            model.Users = allUsers;

            // table 2
            var allRoles = db.Roles.OrderBy(r => r.Name).ToList();
            var roleModels = new List<AspRole>();
            foreach (var role in allRoles)
            {
                roleModels.Add(new AspRole()
                {
                    RoleName = role.Name,
                    RoleId = role.Id
                    
                });
            }
            model.Roles = roleModels;

            // prepare for selectList
            ViewBag.Id = new SelectList(allUsers, "Id", "UserName");
            ViewBag.RoleId = new SelectList(roleModels, "RoleId", "RoleName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUsersToRoles(UserRoleViewModel postModel)
        {
            if(ModelState.IsValid)
            {
                var roleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();

                // find the user
                var user = userManager.FindByIdAsync(postModel.Id).Result;
                // find the role
                var role = roleManager.FindByIdAsync(postModel.RoleId).Result;
                
                if(user!=null && role!=null) // if the user exists and role exists
                {
                    var isInRole = userManager.IsInRoleAsync(user, role.Name).Result;
                    if(isInRole) // if the user has already been in the role
                    {
                        ValidationHelper.codeForUserRole = 2;
                        ValidationHelper.user = user.UserName;
                        ValidationHelper.role = role.Name;
                        return RedirectToAction("AddUsersToRoles");
                    }
                    // add user to the role
                    var result = userManager.AddToRoleAsync(user, role.Name).Result;
                    if(result.Succeeded)
                    {
                        ValidationHelper.codeForUserRole = 1;
                        ValidationHelper.user = user.UserName;
                        ValidationHelper.role = role.Name;
                        return RedirectToAction("AddUsersToRoles");
                    }
                } else
                {
                    // user or role does not exist
                    return RedirectToAction("AddUsersToRoles");
                }
            }

            return RedirectToAction("AddUsersToRoles");

        }

        public IActionResult RemoveUsersToRoles()
        {
            if (ValidationHelper.codeRemoval == 1)
            {
                ViewBag.State = 1;
            }
            else if(ValidationHelper.codeRemoval == 3)
            {
                ViewBag.State = 3;
            } else
            {
                ViewBag.State = 0;
            }
            // change the code back 
            ValidationHelper.codeRemoval = 0;

            // table 1
            UserRoleViewModel model = new UserRoleViewModel();
            var allUsers = db.Users.OrderBy(u => u.UserName).ToList();
            model.Users = allUsers;

            // table 2
            var allRoles = db.Roles.OrderBy(r => r.Name).ToList();
            var roleModels = new List<AspRole>();
            foreach (var role in allRoles)
            {
                roleModels.Add(new AspRole()
                {
                    RoleName = role.Name,
                    RoleId = role.Id

                });
            }
            model.Roles = roleModels;

           
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveUsersToRoles(UserRoleViewModel postModel)
        {
            if (ModelState.IsValid)
            {
                var roleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();

                if(postModel.RoleId == "?" || postModel.Id == "?")
                {
                    // user or role does not exist
                    ValidationHelper.codeRemoval = 3;
                    return RedirectToAction("RemoveUsersToRoles");
                }

                char[] delimiters = { ':' };
                var id = postModel.Id.Split(delimiters)[1];
                var roleId = postModel.RoleId.Split(delimiters)[1];

                // find the user
                var user = userManager.FindByIdAsync(id).Result;
                // find the role
                var role = roleManager.FindByIdAsync(roleId).Result;

                if (user != null && role != null) // if the user exists and role exists
                {
                    var isInRole = userManager.IsInRoleAsync(user, role.Name).Result;
                    if (isInRole) // if the user has already been in the role
                    {
                        // delete the role from user
                        var result = userManager.RemoveFromRoleAsync(user, role.Name);
                        if (result.Result.Succeeded)
                        {
                            ValidationHelper.codeRemoval = 1;
                            ValidationHelper.user = user.UserName;
                            ValidationHelper.role = role.Name;
                            return RedirectToAction("RemoveUsersToRoles");
                        } else
                        {
                           
                            return RedirectToAction("RemoveUsersToRoles");
                        }                 
                    } else
                    {
                        // this user is not in the role, can not be deleted 
                        return RedirectToAction("RemoveUsersToRoles");
                    }       
                }
                else
                {
                    // user or role does not exist
                    ValidationHelper.codeRemoval = 3;
                    return RedirectToAction("RemoveUsersToRoles");
                }
            }

            return RedirectToAction("RemoveUsersToRoles");

        }

        public IActionResult AddUsers()
        {
            if (ValidationHelper.codeForUser == 1)
            {
                ViewBag.State = 1;
            }
            else if (ValidationHelper.codeForUser == 2)
            {
                ViewBag.State = 2;
            }
            else if(ValidationHelper.codeForUser == 3)
            {
                ViewBag.State = 3;
            } else
            {
                ViewBag.State = 0;
            }
            // change the code back 
            ValidationHelper.codeForUser = 0;
            var users = db.Users.OrderBy(u => u.UserName).ToList();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUsers(ApplicationUser user)
        {
            if(ModelState.IsValid)
            {
                var userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
                user.UserName = user.UserName.ToLower(); // change the name to lowercase
                // find the user
                var myUser = userManager.FindByNameAsync(user.UserName).Result;
                if(myUser == null)
                {
                    var result = userManager.CreateAsync(user, user.PasswordHash).Result;
                    if(result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Member"); // add this newly created user to Member role
                        ValidationHelper.codeForUser = 1;
                        ValidationHelper.user = user.UserName;
                        return RedirectToAction("AddUsers");

                    } else
                    {
                        ValidationHelper.codeForUser = 3;
                        ValidationHelper.user = user.UserName;
                        var errs = result.Errors.ToList();
                        ValidationHelper.errors = errs;
                        return RedirectToAction("AddUsers");
                    }

                } else
                {
                    ValidationHelper.codeForUser = 2;
                    ValidationHelper.user = user.UserName;
                    return RedirectToAction("AddUsers");
                }

            }
            return RedirectToAction("AddUsers");
        }

        public IActionResult RemoveRoles()
        {
            if (ValidationHelper.code == 1)
            {
                ViewBag.State = 1;
                ViewBag.Role = ValidationHelper.role;
            }
            else if (ValidationHelper.code == 3)
            {
                ViewBag.State = 3;
                ViewBag.Role = ValidationHelper.role;
            }
            else
            {
                ViewBag.State = 0;
                ViewBag.Role = "";
            }

            // change code back to 0
            ValidationHelper.code = 0;

            var roles = db.Roles.OrderBy(r => r.Name).ToList();
            var roleModels = new List<AspRole>();
            foreach (var role in roles)
            {
                roleModels.Add(new AspRole()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                });
            }

            // prepare for drop downList
            ViewBag.RoleId = new SelectList(roleModels, "RoleId", "RoleName");
            return View(roleModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveRoles(AspRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();

                // remove the role from the table
                var role = roleManager.FindByIdAsync(model.RoleId).Result;
                // check if role is Admin or Member
                string a = "Admin";
                string m = "Member";
                if(role.Name.Equals(a, StringComparison.OrdinalIgnoreCase) || role.Name.Equals(m, StringComparison.OrdinalIgnoreCase) ){
                    ValidationHelper.code = 3;
                    ValidationHelper.role = role.Name;
                    return RedirectToAction("RemoveRoles");
                }
                var task = roleManager.DeleteAsync(role);
                
                if (task.Result.Succeeded)
                {
                    
                        ValidationHelper.code = 1;
                        ValidationHelper.role = role.Name;
                        return RedirectToAction("RemoveRoles");
                    
                }
                else
                {
                    ValidationHelper.code = 3;
                    ValidationHelper.role = role.Name;
                    return RedirectToAction("RemoveRoles");
                }
            }
            return RedirectToAction("RemoveRoles");
        }
    

        /// <summary>
        /// This is a function for user data Json
        /// </summary>
        /// <returns>Json</returns>
        public JsonResult getUserData()
        {
            var users = db.Users.OrderBy(u => u.UserName).ToList();
            List<UserJson> usersJson = new List<UserJson>();
            foreach(var user in users)
            {
                usersJson.Add(new UserJson()
                {
                    id = user.Id,
                    userName = user.UserName
                });
            }

            return Json(usersJson);
        }

        /// <summary>
        /// get roles json data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Identity/getAllRoleData/{id}")]
        public JsonResult getAllRoleData(string id)
        {
            var userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();
            var user = userManager.FindByIdAsync(id).Result;
           
            var roles = db.UserRoles.Where(i => i.UserId == user.Id);
            List<AspRole> aspRoles = new List<AspRole>();
            // var test = roleManager.FindByIdAsync(roles.FirstOrDefault().RoleId).Result;

            // prepare key-value dictionary
            Dictionary<string, string> map = new Dictionary<string, string>();
            var data = db.Roles.ToList();
            foreach(var item in data)
            {
                map.Add(item.Id, item.Name);
            }

            // if the user is a
            if(user.UserName == "a")
            {
                foreach (var role in roles)
                {
                    var rn = map[role.RoleId];
                    if(rn != "Admin")
                    {
                        aspRoles.Add(new AspRole()
                        {
                            RoleId = role.RoleId,
                            RoleName = rn
                        });
                    }
                }

                return Json(aspRoles);
            }

            foreach(var role in roles)
            {
                var rn = map[role.RoleId];
                aspRoles.Add(new AspRole()
                {
                    RoleId = role.RoleId,
                    RoleName = rn
                });
            }
            return Json(aspRoles);
        }
       
    }
}