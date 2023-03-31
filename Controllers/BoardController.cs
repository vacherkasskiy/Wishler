using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishler.Data;
using Wishler.Models;
using Wishler.ViewModels;

namespace Wishler.Controllers;

[Authorize]
public class BoardController : Controller
{
    private readonly ApplicationDbContext _db;

    public BoardController(ApplicationDbContext db)
    {
        _db = db;
    }

    [Route("/board/{id}")]
    public IActionResult Index(int id)
    {
        var param = new BoardViewModel
        {
            BoardId = id,
            BackgroundId = _db.Boards.Find(id).PictureSource,
            Columns = _db.Columns,
            Rows = _db.Rows,
            Column = new Column(),
            Row = new Row()
        };
        return View(param);
    }

    [Route("/EditOrCreateColumn")]
    public IActionResult EditOrCreateColumn()
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("/EditOrCreateColumn")]
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
    public IActionResult EditOrCreateRow()
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("/EditOrCreateRow")]
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
    public void DeleteRow(int id)
    {
        var row = _db.Rows.Find(id);
        if (row != null)
        {
            _db.Rows.Remove(row);
            _db.SaveChanges();
        }
    }

    [HttpDelete]
    [Route("/board/deleteColumn")]
    public void DeleteColumn(int id)
    {
        var column = _db.Columns.Find(id);
        if (column != null)
        {
            _db.Columns.Remove(column);
            _db.SaveChanges();
        }
    }
}