namespace Wishler.Models;

public class BoardViewCreate
{
    public IEnumerable<Column> Columns { get; set; }
    public IEnumerable<Row> Rows { get; set; }
    public Column Column { get; set; }
    public Row Row { get; set; }
}