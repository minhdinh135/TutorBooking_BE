using EXE101.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models;
using PRN231.Services.Interfaces;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IGenericService<User, UserDTO> _userService;

    }
}
