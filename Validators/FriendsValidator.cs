using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wishler.Data;
using Wishler.Models;

namespace Wishler.Validators;

public class FriendsValidator
{
    private readonly ApplicationDbContext _db;

    public FriendsValidator(ApplicationDbContext db)
    {
        _db = db;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            MailAddress m = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    
    public void ValidateFriendRequest(FriendRequest friendRequest, ModelStateDictionary modelState)
    {
        if (friendRequest.ReceiverEmail == null || !IsValidEmail(friendRequest.ReceiverEmail))
        {
            modelState.AddModelError("FriendRequest.ReceiverEmail", "Invalid email");
        }
        if (_db.Users.FirstOrDefault(x => x.Email == friendRequest.ReceiverEmail) == null)
        {
            modelState.AddModelError("FriendRequest.ReceiverEmail", "There is no such user");
        }
        if (friendRequest.ReceiverEmail == friendRequest.SenderEmail)
        {
            modelState.AddModelError("FriendRequest.ReceiverEmail", "This is you");
        }
        if (_db.FriendRequests.FirstOrDefault(x => x.SenderEmail == friendRequest.SenderEmail
                                                   && x.ReceiverEmail == friendRequest.ReceiverEmail) != null)
        {
            modelState.AddModelError("FriendRequest.ReceiverEmail", "You already sent this request");
        }
        if (_db.FriendRequests.FirstOrDefault(x => x.SenderEmail == friendRequest.ReceiverEmail
                                                   && x.ReceiverEmail == friendRequest.SenderEmail) != null)
        {
            modelState.AddModelError("FriendRequest.ReceiverEmail", "This user already sent you request");
        }
        if (_db.Friends.FirstOrDefault(x => x.OwnerEmail == friendRequest.SenderEmail
                                            && x.FriendEmail == friendRequest.ReceiverEmail) != null)
        {
            modelState.AddModelError("FriendRequest.ReceiverEmail", "This user is already your friend");
        }
    }
}