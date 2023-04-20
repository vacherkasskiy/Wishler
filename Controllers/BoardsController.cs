﻿using System.Security.Claims;
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

        var param = new BoardsViewModel
        {
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
            _db.Boards.Add(board);
            _db.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [Route("/boards/delete/{boardId}")]
    [Authorize]
    public IActionResult Delete(int boardId)
    {
        var board = _db.Boards.Find(boardId);
        _db.Boards.Remove(board!);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}