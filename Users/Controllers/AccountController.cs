using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using Users.Data;
using Users.Models;


namespace Users.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<User> userManager,  IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;

        }

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration configuration;


        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterNewUser(DTOuser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                

                if (ModelState.IsValid)
            {
                User user1 = new()
                {
                    UserName = user.Username,
                    Email = user.Email,

                };
                IdentityResult result = await _userManager.CreateAsync(user1, user.Password);
                if (result.Succeeded)
                {
                    return Ok("success");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest();
        }



        [HttpPost]
        public async Task<IActionResult> Login(dtoLogin login)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ModelState.IsValid)
            {
                User? appuser=await _userManager.FindByNameAsync(login.Username);
                if(appuser != null)
                {
                    if (await _userManager.CheckPasswordAsync(appuser, login.Password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim("tokennum", "75"));
                        claims.Add(new Claim(ClaimTypes.Name, appuser.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, appuser.Email));
                        //claims.Add(new Claim(JwtRegisteredClaimName.Jti, Guid.NewGuid().ToString()));
                        var roles = await _userManager.GetRolesAsync(appuser);// دور كل مستخدم 
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }

                        //sign

                        var key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));//for secret key
                        var sc= new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                        
                        var token = new JwtSecurityToken(// for token
                            claims: claims,
                            issuer: configuration["JWT:Issure"],
                            audience: configuration["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: sc

                            );
                        var _token = new
                        {
                            token=new JwtSecurityTokenHandler().WriteToken(token),
                            exception =token.ValidTo,
                        };
                        return Ok(_token);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "user name not found");
                }
            }
            return BadRequest(ModelState);

        }
    }
}