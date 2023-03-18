using Wishler.Models;

namespace Wishler.ViewModels;

public class BoardsViewModel
{
    public IEnumerable<Board> Boards { get; set; }
    public Board Board { get; set; }
    public int UserId { get; set; }
}