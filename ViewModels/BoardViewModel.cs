using Wishler.Models;

namespace Wishler.ViewModels;

public class BoardViewModel
{
    public int BoardId { get; set; }
    public string BackgroundId { get; set; }
    public IEnumerable<Column> Columns { get; set; }
    public IEnumerable<Row> Rows { get; set; }
    public Column Column { get; set; }
    public Row Row { get; set; }
}