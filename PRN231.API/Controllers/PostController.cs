using Microsoft.AspNetCore.Mvc;
using PRN231.Models.DTOs;
using PRN231.Models;
using PRN231.Services.Interfaces;
using PRN231.Services;
using PRN231.Constant;
using AutoMapper;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IGenericService<Post, PostDTO> _postService;
        private readonly IGenericService<User, UserDTO> _userService;
        private readonly ILogger<PostController> _logger;
        public IConfiguration _configuration;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public PostController(IConfiguration config, ILogger<PostController> logger,
                IGenericService<Post, PostDTO> postService,
                IFileStorageService fileStorageService,
                IGenericService<User, UserDTO> userService,
                IMapper mapper)
        {
            _logger = logger;
            _configuration = config;
            _postService = postService;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var postList = await _postService.GetAll();
            return Ok(postList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var postList = await _postService.Get(id);
            return Ok(postList);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] PostDTO dto)
        {
            var user = await _userService.Get(dto.UserId);
            if (user == null)
            {
                return NotFound($"User with ID {dto.UserId} not found.");
            }
            if (user.Credit < 10000)
            {
                return BadRequest("Not enough credit");
            }
            user.Credit -= 10000;

            var imageUrls = new List<string>();
            if (dto.ImageFiles != null && dto.ImageFiles.Count > 0)
            {
                foreach (var file in dto.ImageFiles)
                {
                    var imageUrl = await _fileStorageService.StoreFileAsync(file);
                    if (imageUrl == null)
                    {
                        return BadRequest("Failed to store one or more files.");
                    }
                    imageUrl = $"http://localhost:5176/{imageUrl}";
                    imageUrls.Add(imageUrl);
                }
            }

            dto.ImageUrl = imageUrls;
            dto.Status = StatusConstant.PENDING;
            DateTime currentTime = DateTime.Now;
            dto.CreatedDate = currentTime;

            var post = await _postService.Add(dto);

            var addedDto = new PostDTO
            {
                Id = post.Id,
                Description = dto.Description,
                Status = dto.Status,
                ImageUrl = imageUrls,
                Title = dto.Title,
                UserId = dto.UserId,
                CreatedDate = currentTime
            };
            return Ok(addedDto);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] PostDTO dto)
        {
            var newImageurl = new List<string>();
            if (dto.ImageFiles != null && dto.ImageFiles.Count > 0)
            {
                foreach (var file in dto.ImageFiles)
                {
                    var newImageUrl = await _fileStorageService.StoreFileAsync(file);
                    if (newImageUrl == null)
                    {
                        return BadRequest("Failed to store one or more files.");
                    }
                    newImageUrl = $"http://localhost:5176/{newImageUrl}";
                    newImageurl.Add(newImageUrl);
                }

                if (dto.ImageUrl != null && dto.ImageUrl.Count > 0)
                {
                    foreach (var oldImageUrl in dto.ImageUrl)
                    {
                        await _fileStorageService.DeleteFileAsync(oldImageUrl);
                    }
                }

                dto.ImageUrl = newImageurl;
            }

            dto.Status = StatusConstant.ACTIVE;

            var updatedPost = await _postService.Update(dto);

            return Ok(updatedPost);
        }


        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.Delete(id);
            return Ok(post);
        }


    }
}
