using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wishler.Models;

public class Column : IComparable
{
    [Key] public int Id { get; set; }

    [Required] [MaxLength(25)] public string Name { get; set; } = "";

    [ForeignKey("Board")]
    public int BoardId { get; set; }
    public int Position { get; set; }

    public Board Board { get; set; }
    
    public int CompareTo(object? obj)
    {
        if (obj is Column)
        {
            var other = obj as Column;
            if (other != null && other.Position > Position) return 1;
            if (other!.Position < Position) return -1;
            return 0;
        }

        throw new Exception("Wrong comparison");
    }
}