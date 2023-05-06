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

    private GroupParticipant[] MixParticipants(GroupParticipant[] participants)
    {
        var rand = new Random();

        var mixedParticipants = participants
            .OrderBy(x => rand.Next())
            .ToArray();

        return mixedParticipants;
    }

    private bool AreTheyUsersWithTheirOwnWish(GroupParticipant[] array1, GroupParticipant[] array2)
    {
        for (var i = 0; i < array1.Length; ++i)
            if (array1[i].UserId == array2[i].UserId)
                return true;

        return false;
    }

    [Route("group/{groupId}")]
    public IActionResult Index(int groupId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!_db
                .GroupParticipants
                .Any(x => x.GroupId == groupId && x.UserId == userId))
            return RedirectToAction("WrongRequest", "ErrorHandler");

        var user = _db.Users.Find(userId)!;
        var groupName = _db.Groups.Find(groupId)!.Name;

        var userFriends = _db
            .Friends
            .Where(x => x.OwnerEmail == user.Email)
            .Select(x => x.FriendEmail)
            .ToArray();

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

        var groupParticipantEmails = usersInGroup
            .Select(x => x.Email)
            .ToArray();

        var possibleMembers = _db.Users
            .Where(x => userFriends.Contains(x.Email) && !groupParticipantEmails.Contains(x.Email))
            .ToArray();

        var model = new GroupViewModel
        {
            IsStarted = _db.Groups.Find(groupId)!.IsStarted,
            GroupId = groupId,
            GroupName = groupName,
            GroupParticipants = groupParticipants,
            UsersInGroup = usersInGroup,
            PossibleMembers = possibleMembers
        };

        return View(model);
    }

    [Route("group/new-group")]
    [HttpPost]
    public IActionResult Create(NewGroupViewModel newGroupViewModel)
    {
        if (newGroupViewModel?.Members == null) return RedirectToAction("Index", "Boards");

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

            return RedirectToAction("Index", new {groupId = newGroup.Entity.Id});
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
            return RedirectToAction("WrongRequest", "ErrorHandler");

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

        if (participants.Length < 3)
        {
            Response.Redirect("/bad-request");
            return;
        }

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
            Response.Redirect("/bad-request");
            return;
        }

        var participantUserId = participant.UserId;
        var groupId = participant.GroupId;
        var group = _db.Groups.Find(groupId)!;

        if (!_db
                .GroupParticipants
                .Any(x => x.GroupId == groupId &&
                          x.UserId == userId &&
                          (x.IsOwner || userId == participantUserId) &&
                          !group.IsStarted))
            Response.Redirect("/bad-request");

        _db.GroupParticipants.Remove(participant);
        _db.SaveChanges();

        if (userId == participantUserId)
            Response.Redirect("/boards");
        else
            Response.Redirect($"/group/{groupId}");
    }

    [Route("/group/addMember")]
    [HttpPost]
    public void AddMember(int userId, int groupId)
    {
        var newParticipant = new GroupParticipant
        {
            GroupId = groupId,
            IsOwner = false,
            UserId = userId
        };

        _db.GroupParticipants.Add(newParticipant);
        _db.SaveChanges();
    }
}