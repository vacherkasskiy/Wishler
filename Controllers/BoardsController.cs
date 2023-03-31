using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishler.Data;
using Wishler.Models;
using Wishler.ViewModels;

namespace Wishler.Controllers;

public class BoardsController : Controller
{
    private readonly ApplicationDbContext _db;

    public BoardsController(ApplicationDbContext db)
    {
        _db = db;
    }

    private bool LinkExists(string imageUrlAddress)
    {
        try
        {
            var webRequest = WebRequest.Create(imageUrlAddress);
            var webResponse = webRequest.GetResponse();
        }
        catch //If exception thrown then couldn't get response from address 
        {
            return false;
        }

        return true;
    }

    [Route("/boards")]
    [Authorize]
    public IActionResult Index()
    {
        var param = new BoardsViewModel
        {
            Boards = _db.Boards,
            Board = new Board(),
            UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
        };
        return View(param);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create(Board board)
    {
        if (!LinkExists(board.PictureSource))
            board.PictureSource =
                "https://img.freepik.com/free-vector/geometrical-patterned-blue-scifi-background_53876-99847.jpg?w=1060&t=st=1678562076~exp=1678562676~hmac=af378117f9b8f711ebcc9039413951323044741d874e85a3e135b9a212dbd46c";
        if (board.Name != null)
        {
            _db.Boards.Add(board);
            _db.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}