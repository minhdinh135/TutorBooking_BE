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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PRN231.Models;
using Microsoft.AspNetCore.Identity;
using BirthdayParty.API;

namespace EXE101.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IGenericService<User, UserDTO> _userService;
        private readonly IGenericService<Role, RoleDTO> _roleService;
        private readonly SignInManager<User> _signIn;
        private readonly UserManager<User> _manager;
        private readonly RoleManager<Role> _roleManager;
        public IConfiguration _configuration;
        private readonly JWTService _jwtService;

        public AuthenticationController(IConfiguration config, ILogger<AuthenticationController> logger,
                IGenericService<User,UserDTO> userService,
                IGenericService<Role, RoleDTO> roleService,
                UserManager<User> manager,
                RoleManager<Role> roleManager, SignInManager<User> signIn,
                JWTService jwtService)
        {
            _logger = logger;
            _configuration = config;
            _userService = userService;
            _roleService = roleService;
            _jwtService = jwtService;
            _manager = manager;
            _roleManager = roleManager;
            _signIn = signIn;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var user = await _manager.FindByEmailAsync(login.Email);
            if (user == null) return Unauthorized("Invalid email!!!");
            var result = await _signIn.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid email or password!!!");
            var roleList = await _manager.GetRolesAsync(user);
            var role = roleList.FirstOrDefault() ?? "";
            var userInfo = new UserDTO();
            var token = _jwtService.CreateJwt(user, role);
            return Ok(token);
        }

        [HttpPost("RegisterStudent")]
        public async Task<ActionResult<UserDTO>> RegisterStudent(RegisterDTO registerDTO)
        {
            if (await _manager.FindByEmailAsync(registerDTO.Email) != null)
            {
                return BadRequest("Email already exists!!!");
            }
            var user = new User
            {
                UserName = registerDTO.Name,
                Email = registerDTO.Email,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = true,
                Gender = "Male",
                Address = "Sai Gon",
                Avatar = "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg",
            };
            var result = await _manager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            bool roleExists = await _roleManager.RoleExistsAsync("Student");
            if (!roleExists) await _roleManager.CreateAsync(new Role("Student"));
            await _manager.AddToRoleAsync(user, "Student");
            var token = _jwtService.CreateJwt(user, "Student");
            return Ok(token);
        }

        [HttpPost("RegisterTutor")]
        public async Task<ActionResult<UserDTO>> RegisterTutor(RegisterDTO registerDTO)
        {
            if (await _manager.FindByEmailAsync(registerDTO.Email) != null)
            {
                return BadRequest("Email already exists!!!");
            }
            var user = new User
            {
                UserName = registerDTO.Name,
                Email = registerDTO.Email,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = true,
                Gender = "Male",
                Address = "Sai Gon",
                Avatar = "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg",
            };
            var result = await _manager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            bool roleExists = await _roleManager.RoleExistsAsync("Tutor");
            if (!roleExists) await _roleManager.CreateAsync(new Role("Tutor"));
            await _manager.AddToRoleAsync(user, "Tutor");
            var token = _jwtService.CreateJwt(user, "Tutor");
            return Ok(token);
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
