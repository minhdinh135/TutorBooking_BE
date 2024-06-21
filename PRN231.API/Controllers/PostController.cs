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
        private readonly ILogger<PostController> _logger;
        public IConfiguration _configuration;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public PostController(IConfiguration config, ILogger<PostController> logger,
                IGenericService<Post, PostDTO> postService,
                IFileStorageService fileStorageService,
                IMapper mapper)
        {
            _logger = logger;
            _configuration = config;
            _postService = postService;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
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
            string imageUrl = null;
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                imageUrl = await _fileStorageService.StoreFileAsync(dto.ImageFile);
                if (imageUrl == null)
                {
                    return BadRequest("Failed to store file.");
                }
                imageUrl = $"http://localhost:5176/{imageUrl}";
            }

            dto.ImageUrl = imageUrl;
            dto.Status = StatusConstant.PENDING;
            DateTime currentTime = DateTime.Now;
            dto.CreatedDate = currentTime;

            var post = await _postService.Add(dto);

            var addedDto = new PostDTO
            {
                Id = post.Id,
                Description = dto.Description,
                Status = dto.Status,
                ImageUrl = imageUrl,
                Title = dto.Title,
                UserId = dto.UserId,
                CreatedDate = currentTime
            };
            return Ok(addedDto);
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] PostDTO dto)
        {

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                string newImageUrl = await _fileStorageService.StoreFileAsync(dto.ImageFile);

                if (newImageUrl == null)
                {
                    return BadRequest("Failed to store file.");
                }
                newImageUrl = $"http://localhost:5176/{newImageUrl}";

                if (!string.IsNullOrEmpty(dto.ImageUrl))
                {
                    await _fileStorageService.DeleteFileAsync(dto.ImageUrl);
                }

                dto.ImageUrl = newImageUrl;

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
