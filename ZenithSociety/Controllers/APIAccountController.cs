using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZenithSociety.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace ZenithSociety.Controllers
{
    public class APIAccountController : Controller
    {

        private IServiceProvider _services;

        public APIAccountController(IServiceProvider service)
        {
            _services = service;
        }

        // POST api/account/register
        [Produces("application/json")]
        [Route("api/APIAccount")]
        [HttpPost]
        public async Task<IActionResult> Register(string userName, string firstName, string lastName, string email, string password)
        {
            // this should be implemented in client angular site
            if(userName != null && firstName!=null && lastName !=null && email != null && password != null)
            {
                var user = new ApplicationUser { UserName = userName, Email = email, FirstName = firstName, LastName = lastName };
                var _userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                    return Ok(new { code= "success"});
                }

                return BadRequest(result);
            }


            return BadRequest(new { msg = "error" });
        }


        // public async Task<IActionResult> Is
        
    }
}