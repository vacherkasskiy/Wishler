using Wishler.Models;

namespace Wishler.ViewModels;

public class BoardsViewModel
{
    public NewGroupViewModel NewGroup { get; set; }
    public Board[] Boards { get; set; }
    public Board Board { get; set; }
    public int UserId { get; set; }
    public Group[] Groups { get; set; }
    public Friend[] Friends { get; set; }
}