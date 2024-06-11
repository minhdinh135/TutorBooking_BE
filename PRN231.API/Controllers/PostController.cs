using Microsoft.AspNetCore.Mvc;

namespace PRN231.API.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
