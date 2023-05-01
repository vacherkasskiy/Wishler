using Wishler.Models;

namespace Wishler.ViewModels;

public class SharedBoardViewModel
{
    public int BoardId { get; set; }
    public string BackgroundId { get; set; }
    public IEnumerable<Column> Columns { get; set; }
    public IEnumerable<Row> Rows { get; set; }
}