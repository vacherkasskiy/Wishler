using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wishler.Controllers;

public class ProfileController : Controller
{
    [HttpGet]
    [Authorize]
    [Route("/user/{userId}/profile")]
    public IActionResult Index(int userId)
    {
        return View();
    }
}