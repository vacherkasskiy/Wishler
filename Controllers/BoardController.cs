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
    public IActionResult Index(int id)
    {
        var param = new BoardViewModel()
        {
            BoardId = id,
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
        var column = new Column()
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
        var row = new Row()
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
}