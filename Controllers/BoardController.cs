using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using Wishler.Models;

namespace Wishler.Controllers;

public class BoardController : Controller
{
    private ApplicationDbContext _db;
    
    public BoardController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    [Route("/board")]
    public IActionResult Index()
    {
        var param = new BoardViewCreate()
        {
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
    public IActionResult CreateColumn(Column column)
    {
        _db.Columns.Add(column);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [Route("/EditOrCreateRow")]
    public IActionResult EditOrCreateRow(int id, string name, int boardId, int position)
    {
        var column = new Column()
        {
            Id = id,
            Name = name,
            BoardId = boardId,
            Position = position
        };
        _db.Columns.Update(column);
        _db.SaveChanges();
        return Json(new {cellId = column.Id});
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