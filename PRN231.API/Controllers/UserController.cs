using EXE101.Models.DTOs;
using EXE101.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IGenericService<User, UserDTO> _userService;

    }
}
