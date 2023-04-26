using System.Security.Claims;
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

    [Route("group/{groupId}")]
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
        string membersLine = newGroupViewModel.Members.TrimEnd();
        
        if (ModelState.IsValid && membersLine != null && membersLine.Split().Length >= 3)
        {
            var memberEmails = membersLine.Split().ToArray();
            var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var newGroup = _db.Groups.Add(new Group
            {
                Name = newGroupViewModel.Name,
                OwnerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
            });
            _db.SaveChanges();

            
            _db.GroupParticipants.Add(new GroupParticipant
            {
                UserId = ownerId,
                GroupId = newGroup.Entity.Id,
                IsOwner = true
            });
            
            foreach (var member in memberEmails)
            {
                _db.GroupParticipants.Add(new GroupParticipant
                {
                    UserId = _db.Users.First(x => x.Email == member).Id,
                    GroupId = newGroup.Entity.Id,
                    IsOwner = false
                });
            }
            _db.SaveChanges();
        }
        
        return RedirectToAction("Index", "Boards");
    }

    [Route("group/delete/{groupId}")]
    public IActionResult Delete(int groupId)
    {
        var group = _db.Groups.Find(groupId)!;

        foreach (var participant in _db.GroupParticipants.Where(x => x.GroupId == groupId))
        {
            _db.GroupParticipants.Remove(participant);
        }
        
        _db.Groups.Remove(group);
        _db.SaveChanges();
        
        return RedirectToAction("Index", "Boards");
    }
    
    [Route("/group/saveWish")]
    [HttpPost]
    public void SaveWish(int participantId, string wish)
    {
        var participant = _db.GroupParticipants.Find(participantId)!;
        participant.Wish = wish;
        _db.GroupParticipants.Update(participant);
        _db.SaveChanges();
    }

    [Route("/group/startEvent")]
    [HttpPatch]
    public void StartEvent(int groupId)
    {
        var group = _db.Groups.Find(groupId)!;
        group.IsStarted = true;
        _db.Groups.Update(group);
        _db.SaveChanges();
    }
    
    [Route("/group/cancelEvent")]
    [HttpPatch]
    public void CancelEvent(int groupId)
    {
        var group = _db.Groups.Find(groupId)!;
        group.IsStarted = false;
        _db.Groups.Update(group);
        _db.SaveChanges();
    }
    
    [Route("group/kick/{participantId}")]
    public void DeleteParticipant(int participantId)
    {
        var participant = _db.GroupParticipants.Find(participantId)!;
        var participantUserId = participant.UserId;
        var groupId = participant.GroupId;
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        _db.GroupParticipants.Remove(participant);
        _db.SaveChanges();

        if (currentUserId == participantUserId)
        {
            Response.Redirect("/boards");
        }
        else
        {
            Response.Redirect($"/group/{groupId}");
        }
    }
}