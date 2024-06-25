using Microsoft.AspNetCore.Mvc;
using PRN231.Models.DTOs;
using PRN231.Models;
using PRN231.Services.Interfaces;
using PRN231.Services;
using PRN231.Constant;
using AutoMapper;
using PRN231.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using PRN231.Models.DTOs.Request;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IGenericService<Post, PostDTO> _postService;
        private readonly IGenericRepository<Post> _postRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericService<User, UserDTO> _userService;
        private readonly ILogger<PostController> _logger;
        public IConfiguration _configuration;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _manager;
        private readonly IGenericService<Transaction, TransactionDTO> _transactionService;


        public PostController(IConfiguration config, ILogger<PostController> logger,
                IGenericService<Post, PostDTO> postService,
                IFileStorageService fileStorageService,
                IGenericService<User, UserDTO> userService,
                IMapper mapper, IGenericRepository<Post> postRepo,
                UserManager<User> manager, IGenericService<Transaction, TransactionDTO> transactionService, IGenericRepository<User> userRepo)
        {
            _logger = logger;
            _configuration = config;
            _postService = postService;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
            _userService = userService;
            _postRepo = postRepo;
            _manager = manager;
            _transactionService = transactionService;
            _userRepo = userRepo;
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

        [HttpGet("GetPostsByUserId")]
        //[Authorize]
        public async Task<IActionResult> GetPostsByUserId(int id)
        {
            var posts = await _postRepo.GetAll(x => x.Where(p => p.UserId == id));
            return Ok(posts);
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
            var adminUsers = await _manager.GetUsersInRoleAsync("Admin");
            var admins = adminUsers.FirstOrDefault();
            if (admins == null) return BadRequest("Admin not found");
            var admin = await _userRepo.Get(admins.Id);
            var transaction = new TransactionDTO
            {
                UserId = user.Id,
                ReceiverId = admin.Id,
                Amount = 10000,
                Message = "Transfer credit to admin",
                Type = TransactionConstant.POST,
                Status = StatusConstant.ACTIVE,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            await _transactionService.Add(transaction);

            user.Credit -= 10000;
            admin.Credit += 10000;
            await _userRepo.Update(user);
            await _userRepo.Update(admin);

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

        [HttpPost("Refund")]
        public async Task<IActionResult> Refund([FromBody]PostDTO dto)
        {
            var user = await _userService.Get(dto.UserId);
            if (user == null)
            {
                return NotFound($"User with ID {dto.UserId} not found.");
            }

            var adminUsers = await _manager.GetUsersInRoleAsync("Admin");
            var admins = adminUsers.FirstOrDefault();
            if (admins == null) return BadRequest("Admin not found");
            var admin = await _userRepo.Get(admins.Id);
            var transaction = new TransactionDTO
            {
                UserId = admin.Id,
                ReceiverId = user.Id,
                Amount = 10000,
                Message = "Transfer credit to user",
                Type = TransactionConstant.REFUND,
                Status = StatusConstant.ACTIVE,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            await _transactionService.Add(transaction);

            user.Credit += 10000;
            admin.Credit -= 10000;
            await _userRepo.Update(user);
            await _userRepo.Update(admin);

            return Ok();
        }

    }
}
