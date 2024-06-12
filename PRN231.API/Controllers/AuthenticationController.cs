using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using PRN231.Services.Interfaces;
using PRN231.Models;
using PRN231.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using PRN231.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PRN231.Services.Implementations;
using PRN231.Constant;
using Microsoft.AspNetCore.WebUtilities;

namespace PRN231.API.Controllers
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

        private readonly IEmailService _emailSender;
        private readonly IGenericService<Credential, CredentialDTO> _credentialService;
        private readonly OtpService _otpService;

        private readonly IFileStorageService _fileStorageService;

        public IConfiguration _configuration;
        private readonly JWTService _jwtService;

        public AuthenticationController(IConfiguration config, ILogger<AuthenticationController> logger,
                IGenericService<User,UserDTO> userService,
                IGenericService<Role, RoleDTO> roleService,
                UserManager<User> manager,
                RoleManager<Role> roleManager, SignInManager<User> signIn,
                IEmailService emailSender,
                OtpService otpService,
                JWTService jwtService,
                IGenericService<Credential, CredentialDTO> credentialService,
                IFileStorageService fileStorageService)
        {
            _logger = logger;
            _configuration = config;
            _userService = userService;
            _roleService = roleService;
            _jwtService = jwtService;
            _manager = manager;
            _roleManager = roleManager;
            _signIn = signIn;
            _emailSender = emailSender;
            _otpService = otpService;
            _credentialService = credentialService;
            _fileStorageService = fileStorageService;
        }

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail()
        {
            var receiver = "tridmse173029@fpt.edu.vn";
            var subject = "Test";
            var message = "Hello";

            await _emailSender.SendEmailAsync(receiver, subject, message);
            return Ok();
        }

        [HttpPost("SendStatusMail")]
        public async Task<IActionResult> SendStatusMail(SendStatusEmailDTO dto)
        {
            var receiver = dto.Email;
            var subject = "";
            var message = "";
            if(dto.Status == "Active")
            {
                subject = "Account activated";
                message = "Your account has been activated. Please check!";
            }
            else
            {
                subject = "Account rejected";
                message = "Your account has been rejected. Please check!";
            }

            await _emailSender.SendEmailAsync(receiver, subject, message);
            return Ok();
        }

        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] RequestOtpModel model)
        {
            if (await _manager.FindByEmailAsync(model.Email) != null)
            {
                return BadRequest("Email already exists!!!");
            }
            var otp = _otpService.GenerateOtp();
            var hashedOtp = _otpService.HashOtp(otp);

            HttpContext.Session.SetString($"HashedOtp_{model.Email}", hashedOtp);
            //Console.WriteLine(model.Email);
            //Console.WriteLine(HttpContext.Session.GetString($"HashedOtp_{email}"));
            await _emailSender.SendEmailAsync(model.Email, "OTP", otp);

            return Ok(new { Message = "OTP sent to email" });
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpModel model)
        {
            var hashedOtp = HttpContext.Session.GetString($"HashedOtp_{model.Email}");
            var hashedUserOtp = _otpService.HashOtp(model.Otp);

            if (hashedOtp != null && hashedUserOtp == hashedOtp)
            {
                return Ok(new { Message = "OTP verified successfully!" });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid OTP. Please try again." });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var user = await _manager.FindByEmailAsync(login.Email);
            if (user == null) return Unauthorized("Invalid email!!!");
            var result = await _signIn.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid email or password!!!");
            if(user.Status == "Inactive") return Unauthorized("Your account is inactive!!!");
            var roleList = await _manager.GetRolesAsync(user);
            var role = roleList.FirstOrDefault() ?? "";
            var userInfo = new UserDTO();
            var token = _jwtService.CreateJwt(user, role);
            return Ok(token);
        }

        private void ConfigureAuthorizationPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(RoleEnum.ADMIN, policy => policy.RequireRole(RoleEnum.ADMIN));
            options.AddPolicy(RoleEnum.MODERATOR, policy => policy.RequireRole(RoleEnum.MODERATOR));
            options.AddPolicy(RoleEnum.STUDENT, policy => policy.RequireRole(RoleEnum.STUDENT));
            options.AddPolicy(RoleEnum.TUTOR, policy => policy.RequireRole(RoleEnum.TUTOR));
        }

        [HttpPost("RegisterStudent")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> RegisterStudent(RegisterDTO registerDTO)
        {
            if (await _manager.FindByEmailAsync(registerDTO.Email) != null)
            {
                return BadRequest("Email already exists!!!");
            }
            //check Otp
            var hashedOtp = HttpContext.Session.GetString($"HashedOtp_{registerDTO.Email}");
            var hashedUserOtp = _otpService.HashOtp(registerDTO.Otp);

            if (hashedOtp == null || hashedUserOtp != hashedOtp)
            {
                return Unauthorized(new { Message = "Invalid OTP. Please try again." });
            }
            //create user
            var user = new User
            {
                UserName = registerDTO.Name,
                Email = registerDTO.Email,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = "Active",
                PhoneNumber = registerDTO.PhoneNumber,
                Gender = registerDTO.Gender,
                Address = registerDTO.Address,
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
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> RegisterTutor([FromForm] RegisterTutorDTO registerDTO)
        {
            if (await _manager.FindByEmailAsync(registerDTO.Email) != null)
            {
                return BadRequest("Email already exists!!!");
            }
            //check Otp
            var hashedOtp = HttpContext.Session.GetString($"HashedOtp_{registerDTO.Email}");
            var hashedUserOtp = _otpService.HashOtp(registerDTO.Otp);

            if (hashedOtp == null || hashedUserOtp != hashedOtp)
            {
                return Unauthorized(new { Message = "Invalid OTP. Please try again." });
            }
            //create user
            var user = new User
            {
                UserName = registerDTO.Name,
                Email = registerDTO.Email,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = "Inactive",
                PhoneNumber = registerDTO.PhoneNumber,
                Gender = registerDTO.Gender,
                Address = registerDTO.Address,
                Avatar = "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg",
            };
            var result = await _manager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            bool roleExists = await _roleManager.RoleExistsAsync("Tutor");
            if (!roleExists) await _roleManager.CreateAsync(new Role("Tutor"));
            await _manager.AddToRoleAsync(user, "Tutor");
            //add initial credential
            // Store file
            string filePath = await _fileStorageService.StoreFileAsync(registerDTO.CredentialImage);
            if (filePath == null)
            {
                return BadRequest("Failed to store file.");
            }

            // Update credential image
            var image = $"http://localhost:5176/{filePath}";

            var credential = new CredentialDTO{
                TutorId = user.Id,
                Type = registerDTO.CredentialType,
                Image = image,
                Status = "Pending",
                Name = registerDTO.CredentialName,
                SubjectId = registerDTO.SubjectId
            };
            var credentialResult = await _credentialService.Add(credential);
            if (credentialResult == null) return BadRequest("Failed to add credential.");
            //var token = _jwtService.CreateJwt(user, "Tutor");
            return Ok("Your account is being reviewed. You will receive an email when your account is approved.");
        }

        [HttpPost("RegisterAdmin")]
        public async Task<ActionResult<UserDTO>> RegisterAdmin(RegisterDTO registerDTO)
        {
            if (await _manager.FindByEmailAsync(registerDTO.Email) != null)
            {
                return BadRequest("Email already exists!!!");
            }
            //check Otp
            /*var hashedOtp = HttpContext.Session.GetString($"HashedOtp_{registerDTO.Email}");
            var hashedUserOtp = _otpService.HashOtp(registerDTO.Otp);

            if (hashedOtp == null || hashedUserOtp != hashedOtp)
            {
                return Unauthorized(new { Message = "Invalid OTP. Please try again." });
            }*/
            //create user
            var user = new User
            {
                UserName = registerDTO.Name,
                Email = registerDTO.Email,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = "Active",
                Gender = registerDTO.Gender,
                Address = registerDTO.Address,
                Avatar = "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg",
            };
            var result = await _manager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            bool roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists) await _roleManager.CreateAsync(new Role("Admin"));
            await _manager.AddToRoleAsync(user, "Admin");
            var token = _jwtService.CreateJwt(user, "Admin");
            return Ok(token);
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _manager.FindByEmailAsync(model.Email);
            if (user == null || !(await _manager.IsEmailConfirmedAsync(user)))
            {
                return Ok(new { Message = "Password reset email sent if the email exists and is confirmed." });
            }

            var token = await _manager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = Url.Action(
                action: "ResetPassword",
                controller: "Authentication",
                values: new { token, email = user.Email },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                "Reset Password",
                $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            return Ok(new { Message = "Password reset email sent if the email exists and is confirmed." });
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _manager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new { Message = "Password reset successful if email and token were valid." });
            }

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
            var token = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _manager.ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Password reset successful if email and token were valid." });
            }
            else
            {
                return BadRequest(new { Message = "Invalid token or expired token." });
            }
        }

        [HttpPost("RegisterModerator")]
        public async Task<ActionResult<UserDTO>> RegisterModerator(RegisterDTO registerDTO)
        {
            if (await _manager.FindByEmailAsync(registerDTO.Email) != null)
            {
                return BadRequest("Email already exists!!!");
            }
            //check Otp
            /*var hashedOtp = HttpContext.Session.GetString($"HashedOtp_{registerDTO.Email}");
            var hashedUserOtp = _otpService.HashOtp(registerDTO.Otp);

            if (hashedOtp == null || hashedUserOtp != hashedOtp)
            {
                return Unauthorized(new { Message = "Invalid OTP. Please try again." });
            }*/
            //create user
            var user = new User
            {
                UserName = registerDTO.Name,
                Email = registerDTO.Email,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = "Active",
                Gender = registerDTO.Gender,
                Address = registerDTO.Address,
                Avatar = "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg",
            };
            var result = await _manager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            bool roleExists = await _roleManager.RoleExistsAsync("Moderator");
            if (!roleExists) await _roleManager.CreateAsync(new Role("Moderator"));
            await _manager.AddToRoleAsync(user, "Moderator");
            var token = _jwtService.CreateJwt(user, "Moderator");
            return Ok(token);
        }

        [HttpGet("JwtDecode")]
        //[Authorize]
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

    //public class JwtService{
    //    public static JwtDTO CreateJwt(IConfiguration config, User user, string role = RoleEnum.Client){
    //        //create claims details based on the user information
    //            var claims = new[] {
    //                new Claim(JwtRegisteredClaimNames.Sub,
    //                        config["Jwt:Subject"]),
    //                    new Claim(JwtRegisteredClaimNames.Jti
    //                            , Guid.NewGuid().ToString()),
    //                    new Claim(JwtRegisteredClaimNames.Iat
    //                            , DateTime.UtcNow.ToString()),
    //                    new Claim("id", user.Id.ToString()),
    //                    new Claim("email", user.Email),
    //                    new Claim("role", role),
    //            };
    //            var key = new SymmetricSecurityKey(
    //                    Encoding.UTF8.GetBytes(config["Jwt:Key"]));
    //            var signIn = new SigningCredentials(
    //                    key, SecurityAlgorithms.HmacSha256);
    //            var token = new JwtSecurityToken(
    //                    config["Jwt:Issuer"],
    //                    config["Jwt:Audience"],
    //                    claims,
    //                    expires: DateTime.UtcNow.AddMinutes(10),
    //                    signingCredentials: signIn);

    //            string Token = new JwtSecurityTokenHandler().WriteToken(token);
    //            return new JwtDTO{Token = Token};
    //    }
    //}

    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
    public class VerifyOtpModel
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }

    public class RequestOtpModel
    {
        public string Email { get; set; }
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

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        
        public required string Otp {get;set;}
    }


    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
    public class SendStatusEmailDTO
    {
        public string Email { get; set; }
        public string Status { get; set; }
    }

    public class RegisterTutorDTO{
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
        public required string Name { get; set; }
        [MinLength(3, ErrorMessage = "Email must be at least 3 characters")]
        public required string Email { get; set; }
        [MinLength(3, ErrorMessage = "Password must be at least 3 characters")]
        public required string Password {get;set;}

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public string CredentialName { get; set; }

        public int SubjectId { get; set; }

        public string CredentialType { get; set; }

        public IFormFile CredentialImage { get; set; }
        
        public required string Otp {get;set;}
    }

    //public class RoleEnum
    //{
    //    public const string Admin = "Admin";
    //    public const string Client = "Client";
    //}

    public class JwtDTO{
        public string Token {get;set;} =null!;
    }
}
