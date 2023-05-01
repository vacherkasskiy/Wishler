using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishler.Data;
using Wishler.Models;
using Wishler.ViewModels;

namespace Wishler.Controllers;

public class BoardController : Controller
{
    private readonly ApplicationDbContext _db;

    public BoardController(ApplicationDbContext db)
    {
        _db = db;
    }

    [Route("/board/{id}")]
    [Authorize]
    public IActionResult Index(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var board = _db.Boards.Find(id);

        if (board == null || board.UserId != userId) return RedirectToAction("WrongRequest", "ErrorHandler");

        var param = new BoardViewModel
        {
            BoardId = id,
            BackgroundId = board.BackgroundId,
            AccessStatus = board.VisibilityStatus,
            Columns = _db.Columns,
            Rows = _db.Rows,
            Column = new Column(),
            Row = new Row()
        };
        return View(param);
    }

    [Route("/EditOrCreateColumn")]
    [Authorize]
    public IActionResult EditOrCreateColumn()
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("/EditOrCreateColumn")]
    [Authorize]
    public IActionResult EditOrCreateColumn(int id, string name, int boardId, int position)
    {
        var column = new Column
        {
            Id = id,
            Name = name,
            BoardId = boardId,
            Position = position
        };
        _db.Update(column);
        _db.SaveChanges();
        return Json(new {colId = column.Id});
    }

    [Route("/EditOrCreateRow")]
    [Authorize]
    public IActionResult EditOrCreateRow()
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("/EditOrCreateRow")]
    [Authorize]
    public IActionResult EditOrCreateRow(int id, int columnId, int position, string text)
    {
        var row = new Row
        {
            Id = id,
            ColumnId = columnId,
            Position = position,
            Text = text
        };
        _db.Update(row);
        _db.SaveChanges();
        return Json(new {cellId = row.Id});
    }

    [HttpDelete]
    [Route("/board/deleteRow")]
    [Authorize]
    public void DeleteRow(int id)
    {
        var row = _db.Rows.Find(id)!;
        _db.Rows.Remove(row);
        _db.SaveChanges();
    }

    [HttpDelete]
    [Route("/board/deleteColumn")]
    [Authorize]
    public void DeleteColumn(int id)
    {
        var column = _db.Columns.Find(id)!;

        foreach (var row in _db.Rows.Where(x => x.ColumnId == id).ToArray()) _db.Rows.Remove(row);

        _db.Columns.Remove(column);
        _db.SaveChanges();
    }

    [HttpPatch]
    [Route("/board/changeVisibility")]
    [Authorize]
    public void ChangeVisibilityStatus(int boardId, string status)
    {
        var board = _db.Boards.Find(boardId)!;
        board.VisibilityStatus = status;
        _db.Boards.Update(board);

        _db.SaveChanges();
    }

    [HttpGet]
    [Route("/board/shared/{id}")]
    public IActionResult SharedIndex(int id)
    {
        var board = _db.Boards.Find(id);

        if (board == null) return RedirectToAction("WrongRequest", "ErrorHandler");

        var param = new SharedBoardViewModel
        {
            BoardId = board.Id,
            BackgroundId = board.BackgroundId,
            Columns = _db.Columns,
            Rows = _db.Rows
        };

        if (board.VisibilityStatus == "everybody") return View(param);

        if (!User.Identity!.IsAuthenticated) return RedirectToAction("WrongRequest", "ErrorHandler");

        var boardOwner = _db.Users.Find(board.UserId)!;
        var currentUser = _db.Users.Find(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))!;

        if (boardOwner.Id == currentUser.Id) return View();

        if (board.VisibilityStatus == "private") return RedirectToAction("WrongRequest", "ErrorHandler");

        if (_db.Friends.Any(x => x.OwnerEmail == boardOwner.Email && x.FriendEmail == currentUser.Email))
            return View(param);

        return RedirectToAction("WrongRequest", "ErrorHandler");
    }
}