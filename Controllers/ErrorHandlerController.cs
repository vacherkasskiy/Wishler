using Microsoft.AspNetCore.Mvc;

namespace Wishler.Controllers;

public class ErrorHandlerController : Controller
{
    [Route("/bad-request")]
    public IActionResult WrongRequest()
    {
        return View();
    }
}