using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;

namespace PRN231.ODataAPI.Controllers
{
    public class LevelsController : ODataController
    {
        private readonly IGenericService<Level, LevelDTO> _levelService;

        public LevelsController(IGenericService<Level, LevelDTO> levelService)
        {
            _levelService = levelService;
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var allLevels = await _levelService.GetAll();

            return Ok(allLevels);
        }
    }
}
