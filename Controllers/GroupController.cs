﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishler.Data;
using Wishler.Models;
using Wishler.ViewModels;

namespace Wishler.Controllers;

[Authorize]
public class GroupController : Controller
{
    private readonly ApplicationDbContext _db;

    public GroupController(ApplicationDbContext db)
    {
        _db = db;
    }

    [Route("/group/{groupId}")]
    public IActionResult Index(int groupId)
    {
        var groupParticipants = _db
            .GroupParticipants
            .Where(x => x.GroupId == groupId)
            .ToArray();

        var groupParticipantsIds = groupParticipants
            .Select(x => x.UserId)
            .ToArray();

        var usersInGroup = _db
            .Users
            .Where(x => groupParticipantsIds.Contains(x.Id))
            .ToArray();

        var model = new GroupViewModel
        {
            GroupId = groupId,
            GroupParticipants = groupParticipants,
            UsersInGroup = usersInGroup
        };

        return View(model);
    }

    [Route("group/new-group")]
    [HttpPost]
    public IActionResult Create(NewGroupViewModel newGroupViewModel)
    {
        var memberEmails = newGroupViewModel.Members.Split().ToArray();
        
        if (ModelState.IsValid && memberEmails.Length >= 3)
        {
            var ownerEmail = User.FindFirstValue(ClaimTypes.Email);
            
            var newGroup = _db.Groups.Add(new Group
            {
                Name = newGroupViewModel.Name,
                OwnerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
            });
            _db.SaveChanges();

            foreach (var member in memberEmails)
            {
                _db.GroupParticipants.Add(new GroupParticipant
                {
                    UserId = _db.Users.First(x => x.Email == member).Id,
                    GroupId = newGroup.Entity.Id,
                    IsOwner = (member == ownerEmail)
                });
            }
            _db.SaveChanges();
        }
        
        return RedirectToAction("Index", "Boards");
    }
}