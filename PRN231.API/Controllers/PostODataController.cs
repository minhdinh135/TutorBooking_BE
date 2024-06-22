using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Services;
using PRN231.Services.Interfaces;
using System.Linq;

namespace PRN231.API.Controllers.OData
{
    public class PostODataController : ODataController
    {
        private readonly IGenericService<Post, PostDTO> _postService;
        private readonly IFileStorageService _fileStorageService;

        public PostODataController(
            IGenericService<Post, PostDTO> postService,
            IFileStorageService fileStorageService)
        {
            _postService = postService;
            _fileStorageService = fileStorageService;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Post>>> Get()
        {
            var postList = await _postService.GetAll();
            return Ok(postList);
        }

        [EnableQuery]
        public async Task<ActionResult<Post>> Get(int key)
        {
            var post = await _postService.Get(key);
            return post != null ? Ok(post) : NotFound();
        }

        public async Task<ActionResult<Post>> Post([FromForm] PostDTO dto)
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
            dto.Status = StatusConstant.ACTIVE;

            var post = await _postService.Add(dto);
            return Created(post);
        }

        public async Task<IActionResult> Put(int key, [FromForm] PostDTO dto)
        {
            if (key != dto.Id) return BadRequest();

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
            return Updated(updatedPost);
        }

       public async Task<IActionResult> Delete(int key)
        {
            var post = await _postService.Get(key);
            if (post == null) return NotFound();

            await _postService.Delete(key);
            return NoContent();
        }
    }
}
