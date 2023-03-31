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
        //add proper validation
        if (board.Name != null && board.PictureSource != "0")
        {
            _db.Boards.Add(board);
            _db.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}