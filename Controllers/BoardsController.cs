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
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var groupIds = _db.GroupParticipants.Where(x => x.UserId == userId).Select(x => x.GroupId);
        var groups = _db.Groups.Where(x => groupIds.Contains(x.Id)).ToArray();
        var user = _db.Users.Find(userId);
        var userFriends = _db.Friends.Where(x => x.OwnerEmail == user!.Email).ToArray();

        var param = new BoardsViewModel
        {
            NewGroup = new NewGroupViewModel(),
            Friends = userFriends,
            Boards = _db.Boards.Where(x => x.UserId == userId).ToArray(),
            Board = new Board(),
            UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
            Groups = groups
        };
        return View(param);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Create(Board board)
    {
        //add proper validation
        if (board.Name != null && board.BackgroundId != "0")
        {
            var newBoard = _db.Boards.Add(board);
            _db.SaveChanges();
            
            return RedirectToAction("Index", "Board",new { id = newBoard.Entity.Id});
        }

        return RedirectToAction("Index");
    }

    [Route("/boards/delete/{boardId}")]
    [Authorize]
    public IActionResult Delete(int boardId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var board = _db.Boards.Find(boardId);

        if (board == null || board.UserId != userId) return RedirectToAction("WrongRequest", "ErrorHandler");

        foreach (var column in _db.Columns.Where(x => x.BoardId == boardId).ToArray())
        {
            foreach (var row in _db.Rows.Where(x => x.ColumnId == column.Id).ToArray()) _db.Rows.Remove(row);

            _db.Columns.Remove(column);
        }

        _db.Boards.Remove(board);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}