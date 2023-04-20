using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishler.Data;
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
}