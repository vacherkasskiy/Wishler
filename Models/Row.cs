using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class Row : IComparable
{
    [Key] 
    public int Id { get; set; }
    public int ColumnId { get; set; }
    public int Position { get; set; }
    public string Text { get; set; }
    
    public int CompareTo(object? obj)
    {
        if (obj is Row)
        {
            var other = obj as Row;
            if (other != null && other.Position > Position)
            {
                return 1;
            } 
            if (other.Position < Position)
            {
                return -1;
            }
            return 0;
        }

        throw new Exception("Wrong comparison");
    }
}