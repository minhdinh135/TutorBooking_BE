using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using EXE101.Services.Interfaces;
using EXE101.Models;
using EXE101.Models.DTOs;
using EXE101.Services.Utils;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EXE101.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IGenericService<User, UserDTO> _userService;
        private readonly IGenericService<Role, RoleDTO> _roleService;
        public IConfiguration _configuration;

        public AuthenticationController(IConfiguration config, ILogger<AuthenticationController> logger,
                IGenericService<User,UserDTO> userService,
                IGenericService<Role, RoleDTO> roleService)
        {
            _logger = logger;
            _configuration = config;
            _userService = userService;
            _roleService = roleService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var users = await _userService.GetAll();
            var userLogin = users.Where(u => u.Email == login.Email).FirstOrDefault();
            if(userLogin == null) return Unauthorized(new { Message = "Email not found" });
            var roles = await _roleService.GetAll();
            roles = roles.Where(r => r.UserId == userLogin.Id).ToList();
            if(PasswordManager.VerifyPassword(login.Password, userLogin.HashPassword)){
                if(roles.Count() == 0) 
                {
                    await _roleService.Add(new RoleDTO { Name = RoleEnum.Client, UserId = userLogin.Id.Value });
                    JwtDTO token = JwtService.CreateJwt(_configuration, userLogin);
                    return Ok(token);
                }
                else if(roles.Any(r => r.Name == RoleEnum.Admin))
                {
                    JwtDTO token = JwtService.CreateJwt(_configuration, userLogin, RoleEnum.Admin);
                    return Ok(token);
                }
                else if(roles.Any(r => r.Name == RoleEnum.Client))
                {
                    JwtDTO token = JwtService.CreateJwt(_configuration, userLogin);
                    return Ok(token);
                }
            }
            return Unauthorized(new { Message = "Wrong password" }); 
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            try{
                if(!ModelState.IsValid){
                    var error = ErrorHandler.GetErrorMessage(ModelState);
                    return BadRequest(new {Message = error});
                }
                var userDTO = new UserDTO {
                    Email = register.Email,
                    UserName = register.Name,
                    PhoneNumber = "000",
                    HashPassword = PasswordManager.HashPassword(register.Password),
                    Address = string.Empty,
                    DateOfBirth = DateTime.Now,
                };
                var user = await _userService.Add(userDTO);
                JwtDTO token = JwtService.CreateJwt(_configuration, user);
                await _roleService.Add(new RoleDTO { Name = RoleEnum.Client, UserId = user.Id.Value });
                return Ok(token);
            }
            catch(Exception ex){
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterDTO register)
        {
            try{
                if(!ModelState.IsValid){
                    var error = ErrorHandler.GetErrorMessage(ModelState);
                    return BadRequest(new {Message = error});
                }
                var userDTO = new UserDTO {
                    Email = register.Email,
                    UserName = register.Name,
                    PhoneNumber = "000",
                    HashPassword = PasswordManager.HashPassword(register.Password),
                    Address = string.Empty,
                    DateOfBirth = DateTime.Now,
                };
                var user = await _userService.Add(userDTO);
                JwtDTO token = JwtService.CreateJwt(_configuration, user, RoleEnum.Admin);
                await _roleService.Add(new RoleDTO { Name = RoleEnum.Admin, UserId = user.Id.Value });
                return Ok(token);
            }
            catch(Exception ex){
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpGet("JwtDecode")]
        [Authorize]
        public Task<IActionResult> JwtDecode(){
            string token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            string header = token.Split(".")[0];
            string payload = token.Split(".")[1];
            string signature = token.Split(".")[2];

            string decodedHeader = Base64UrlEncoder.Decode(header);
            string decodedPayload = Base64UrlEncoder.Decode(payload);

            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            byte[] hashedText = hmac.ComputeHash(Encoding.UTF8.GetBytes(header+ "." + payload));
            //Replace because of base 64 url used by jwt
            string validateSignature = Convert.ToBase64String(hashedText)
                .Replace('+','-')
                .Replace('/','_')
                .TrimEnd('=');

            return Task.FromResult<IActionResult>(
                Ok(decodedHeader+ "\n" + decodedPayload + "\n" +validateSignature)
            );
        }
    }

    public class ErrorHandler{
        public static string? GetErrorMessage(ModelStateDictionary modelState){
            foreach (var modelStateEntry in modelState)
            {
                var errors = modelStateEntry.Value.Errors;
                return errors.FirstOrDefault()?.ErrorMessage;
            }
            return null;
        }
    }

    public class JwtService{
        public static JwtDTO CreateJwt(IConfiguration config, User user, string role = RoleEnum.Client){
            //create claims details based on the user information
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub,
                            config["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti
                                , Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat
                                , DateTime.UtcNow.ToString()),
                        new Claim("id", user.Id.ToString()),
                        new Claim("email", user.Email),
                        new Claim("role", role),
                };
                var key = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                var signIn = new SigningCredentials(
                        key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                        config["Jwt:Issuer"],
                        config["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                string Token = new JwtSecurityTokenHandler().WriteToken(token);
                return new JwtDTO{Token = Token};
        }
    }

    public class LoginDTO{
        public required string Email { get; set; }
        public required string Password {get;set;}
    }

    public class RegisterDTO{
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
        public required string Name { get; set; }
        [MinLength(3, ErrorMessage = "Email must be at least 3 characters")]
        public required string Email { get; set; }
        [MinLength(3, ErrorMessage = "Password must be at least 3 characters")]
        public required string Password {get;set;}
    }

    public class RoleEnum
    {
        public const string Admin = "Admin";
        public const string Client = "Client";
    }

    public class JwtDTO{
        public string Token {get;set;} =null!;
    }
}
