using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    [Produces("application/json")]
    [Route(V)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private const string V = "account";
        private const string USERNAME = "unique_name";
       
        readonly UserManager<Core.Model.User> userManager;
        readonly SignInManager<Core.Model.User> signInManager;
        readonly IConfiguration configuration;
        
        public AccountController(
           UserManager<Core.Model.User> userManager,
           SignInManager<Core.Model.User> signInManager,
           IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, isPersistent: false, lockoutOnFailure: false);

                if (!loginResult.Succeeded)
                {
                    return BadRequest();
                }

                var user = await userManager.FindByNameAsync(loginModel.Username);

                return Ok(GetToken(user));
            }
            return BadRequest(ModelState);

        }

        [Authorize (AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var user = await userManager.FindByNameAsync(
                User.Identity.Name ??
                User.Claims.Where(c => c.Properties.ContainsKey(USERNAME)).Select(c => c.Value).FirstOrDefault()
                );
            return Ok(GetToken(user));

        }


        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
               
                var user = new Core.Model.User( registerModel.Email,registerModel.Username);

                var identityResult = await this.userManager.CreateAsync(user, registerModel.Password);
                if (identityResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(GetToken(user));
                }
                else
                {
                    return BadRequest(identityResult.Errors);
                }
            }
            return BadRequest(ModelState);


        }

        private String GetToken(Core.Model.User user)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new Claim[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.GetValue<String>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(this.configuration.GetValue<int>("Tokens:Lifetime")),
                audience: this.configuration.GetValue<String>("Tokens:Audience"),
                issuer: this.configuration.GetValue<String>("Tokens:Issuer")
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

    }
}
