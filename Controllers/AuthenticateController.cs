using AdminAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RPFBE.Auth;
using RPFBE.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
//using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly ICodeUnitWebService codeUnitWebService;

        public AuthenticateController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ICodeUnitWebService codeUnitWebService
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.codeUnitWebService = codeUnitWebService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(6),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    idToken = new JwtSecurityTokenHandler().WriteToken(token),
                    expiresIn = token.ValidTo.TimeOfDay.TotalMilliseconds,
                    expireDate = token.ValidTo,
                    user= userRoles,
                    user.Name,
                });
            }
            return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "INVALID_USER" });
            //return Unauthorized();
        }


        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
           // return Ok(model.EmployeeId);
            var res = await codeUnitWebService.EmployeeAccount().LoginEmployeeCoreAsync(model.EmployeeId, Cryptography.Hash(model.Password));
            dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);

            LoginEmpCoreModel loginEmpCore = new LoginEmpCoreModel
            {
                Status = resSerial.Status,
                Rank = resSerial.Rank
            };

        
            if (loginEmpCore.Status)
            {
                try
                {
                    var userExists = await userManager.FindByNameAsync(model.Username);
                    if (userExists != null)
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

                    ApplicationUser user = new ApplicationUser()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.Username,
                        EmployeeId = model.EmployeeId,
                        Name = model.Username,
                        Rank = loginEmpCore.Rank
                    };
                    var result = await userManager.CreateAsync(user, model.Password);
                    var errs = result.Errors.Select(x => "Code: " + x.Code + " Description: " + x.Description).ToArray();
                    if (!result.Succeeded)
                    {
                        //return Ok(new { errs });
                       return StatusCode(StatusCodes.Status500InternalServerError, new { Status ="Error",Message = errs });
                   
                    }
                    if (!await roleManager.RoleExistsAsync(loginEmpCore.Rank))
                        await roleManager.CreateAsync(new IdentityRole(loginEmpCore.Rank));
                    if (!await roleManager.RoleExistsAsync(UserRoles.User))
                        await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                    if (await roleManager.RoleExistsAsync(loginEmpCore.Rank))
                    {
                        await userManager.AddToRoleAsync(user, loginEmpCore.Rank);

                    }

                    return Ok(new Response { Status = "Success", Message = "User created successfully!" });
                }
                catch (Exception x)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed Exception! :"+x.Data });
                }
               
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "INVALID_USER_D365" });
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var userExists = await userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status208AlreadyReported, new Response { Status = "Error", Message = "USER_EXIST" });

                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username,
                    Name = model.Name

                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status208AlreadyReported, new Response { Status = "Error", Message = "User creation failed; "+ result.Errors.ToString() });
                    //return Ok(result);

                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "CREATION_FAILED",ExtMessage= x.Message });
            }
           
        }

    

    }
}
