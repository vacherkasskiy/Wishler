using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class GroupParticipant
{
    [Key] public int Id { get; set; }

    public int UserId { get; set; }
    public int GroupId { get; set; }
    public bool IsOwner { get; set; }
    public string Wish { get; set; } = "";
    public string OtherName { get; set; } = "";
    public string OtherWish { get; set; } = "";
}