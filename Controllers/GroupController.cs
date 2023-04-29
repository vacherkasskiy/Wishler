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
    
    GroupParticipant[] MixParticipants(GroupParticipant[] participants)
    {
        var rand = new Random();
        
        var mixedParticipants = participants
            .OrderBy(x => rand.Next())
            .ToArray();

        return mixedParticipants;
    }

    bool AreTheyUsersWithTheirOwnWish (GroupParticipant[] array1, GroupParticipant[] array2)
    {
        for (int i = 0; i < array1.Length; ++i)
        {
            if (array1[i].UserId == array2[i].UserId)
            {
                return true;
            }
        }

        return false;
    }

    public GroupController(ApplicationDbContext db)
    {
        _db = db;
    }

    [Route("group/{groupId}")]
    public IActionResult Index(int groupId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!_db
                .GroupParticipants
                .Any(x => x.GroupId == groupId && x.UserId == userId))
        {
            // return something
        }
        
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
            IsStarted = _db.Groups.Find(groupId)!.IsStarted,
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
        var membersLine = newGroupViewModel.Members.TrimEnd();

        if (ModelState.IsValid && membersLine != null && membersLine.Split().Length >= 2)
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
                _db.GroupParticipants.Add(new GroupParticipant
                {
                    UserId = _db.Users.First(x => x.Email == member).Id,
                    GroupId = newGroup.Entity.Id,
                    IsOwner = false
                });
            _db.SaveChanges();
        }

        return RedirectToAction("Index", "Boards");
    }

    [Route("group/delete/{groupId}")]
    public IActionResult Delete(int groupId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!_db
                .GroupParticipants
                .Any(x => x.GroupId == groupId && x.UserId == userId && x.IsOwner))
        {
            // return something
        }
        
        var group = _db.Groups.Find(groupId)!;

        foreach (var participant in _db.GroupParticipants.Where(x => x.GroupId == groupId))
            _db.GroupParticipants.Remove(participant);

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
        
        var participants = _db
            .GroupParticipants
            .Where(x => x.GroupId == groupId)
            .ToArray();

        GroupParticipant[] mixedParticipants;
        do
        {
            mixedParticipants = MixParticipants(participants);
        } while (AreTheyUsersWithTheirOwnWish(participants, mixedParticipants));

        for (var i = 0; i < participants.Length; ++i)
        {
            participants[i].OtherWish = mixedParticipants[i].Wish;
            participants[i].OtherName = _db
                .Users
                .Find(mixedParticipants[i].UserId)!
                .Name;
            _db.GroupParticipants.Update(participants[i]);
        }

        _db.SaveChanges();

        Response.Redirect($"/group/{groupId}");
    }

    [Route("/group/cancelEvent")]
    [HttpPatch]
    public void CancelEvent(int groupId)
    {
        var group = _db.Groups.Find(groupId)!;
        group.IsStarted = false;
        _db.Groups.Update(group);
        _db.SaveChanges();

        Response.Redirect($"/group/{groupId}");
    }

    [Route("group/kick/{participantId}")]
    public void DeleteParticipant(int participantId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var participant = _db.GroupParticipants.Find(participantId);

        if (participant == null)
        {
            // return something
        }
        
        var participantUserId = participant!.UserId;
        var groupId = participant.GroupId;
        
        if (!_db
                .GroupParticipants
                .Any(x => x.GroupId == groupId && x.UserId == userId && (x.IsOwner || userId == participantUserId)))
        {
            // return something
        }
        
        _db.GroupParticipants.Remove(participant);
        _db.SaveChanges();

        if (userId == participantUserId)
            Response.Redirect("/boards");
        else
            Response.Redirect($"/group/{groupId}");
    }
}