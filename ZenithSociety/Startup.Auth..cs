using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ZenithSociety.CustomTokenProvider;
using ZenithSociety.Models;

namespace ZenithSociety
{
    public partial class Startup
    {
        public SymmetricSecurityKey signingKey;

        private IServiceProvider _services;
        private void ConfigureAuth(IApplicationBuilder app, IServiceProvider services)
        {
            _services = services;

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));


            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });



            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    AuthenticationScheme = "Cookie",
            //    CookieName = Configuration.GetSection("TokenAuthentication:CookieName").Value,
            //    TicketDataFormat = new CustomJwtDataFormat(
            //        SecurityAlgorithms.HmacSha256,
            //        tokenValidationParameters)
            //});

            var tokenProviderOptions = new TokenProviderOptions
            {
                Path = Configuration.GetSection("TokenAuthentication:TokenPath").Value,
                Audience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                Issuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
            };

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(tokenProviderOptions));


        }

        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            // var userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = _services.GetRequiredService<SignInManager<ApplicationUser>>();
            var result = signInManager.PasswordSignInAsync(username, password, false, false);


            // Don't do this in production, obviously!
            if (result.Result.Succeeded)
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
            }

            // Credentials are invalid, or account doesn't exist
            return Task.FromResult<ClaimsIdentity>(null);
        }

    }
}
