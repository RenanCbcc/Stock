using Stock_Back_End.Models.AccountModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Stock_Back_End.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    //var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                    var succeeded = await signInManager.UserManager.CheckPasswordAsync(user, model.Password);
                    if (succeeded)
                    {
                        return Ok(GenerateJwt(model.Email));
                    }
                    return Unauthorized("Login ou senha incorretos.");
                }
                return Unauthorized("Login ou senha incorretos.");

            }

            return BadRequest(ModelState);
        }


        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = model.Name, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return Ok("Usuário criado! Faça o login.");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }

        private string GenerateJwt(string email)
        {
            //token(header + payload ->(rights) + signature)
            var rights = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,DateTime.Now.AddHours(1).ToString())
            };

            var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken
                (
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(1),
                claims: rights,
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

    }

}

