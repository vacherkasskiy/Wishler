using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wishler.Models;

public class Row : IComparable
{
    [Key] public int Id { get; set; }
    [ForeignKey("Column")]
    public int ColumnId { get; set; }
    public int Position { get; set; }
    public string Text { get; set; } = "";
    
    public Column Column { get; set; }

    public int CompareTo(object? obj)
    {
        if (obj is Row)
        {
            var other = obj as Row;
            if (other != null && other.Position > Position) return 1;
            if (other!.Position < Position) return -1;
            return 0;
        }

        throw new Exception("Wrong comparison");
    }
}