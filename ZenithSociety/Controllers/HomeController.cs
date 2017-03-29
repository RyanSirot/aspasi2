using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZenithSociety.Data;
using ZenithSociety.Models.AccountViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ZenithSociety.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        private IServiceProvider _services;
        

        public HomeController(ApplicationDbContext context, IServiceProvider services)
        {
            db = context;
            _services = services;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Identity()
        {
           
           
            if(ValidationHelper.code == 1)
            {
                ViewBag.State = 1;
                ViewBag.Role = ValidationHelper.role;
            } else if(ValidationHelper.code == 3)
            {
                ViewBag.State = 3;
                ViewBag.Role = ValidationHelper.role;
            } else
            {
                ViewBag.State = 0;
                ViewBag.Role = "";
            }

            // change code back to 0
            ValidationHelper.code = 0;

            var roles = db.Roles.OrderBy(r => r.Name).ToList();
            var roleModels = new List<RoleViewModel>();
            foreach(var role in roles)
            {
                roleModels.Add(new RoleViewModel()
                {
                    RoleName = role.Name
                });
            }
            return View(roleModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Identity(RoleViewModel roleView)
        {

            if (ModelState.IsValid)
            {
                var roleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();
                // let the first letter uppercase the rest lowercase
                var name = roleView.RoleName.ToLower();
                name = name.First().ToString().ToUpper() + name.Substring(1);
                var task = roleManager.RoleExistsAsync(name);
                if (!task.Result)
                {
                    var result = roleManager.CreateAsync(new IdentityRole(name));
                    if(result.Result.Succeeded)
                    {
                        ValidationHelper.code = 1;
                        ValidationHelper.role = roleView.RoleName;
                        return RedirectToAction("Identity");
                    } else
                    {
                        ValidationHelper.code = 2;
                        return RedirectToAction("Identity");
                    }
                } else
                {
                    ValidationHelper.code = 3;
                    ValidationHelper.role = roleView.RoleName;
                    return RedirectToAction("Identity");
                }
            }
            return RedirectToAction("Identity");
        }
    }
}
