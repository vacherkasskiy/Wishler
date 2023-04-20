using Microsoft.EntityFrameworkCore;
using Wishler.Models;

namespace Wishler.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // add all tables here !!!
    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<Row> Rows { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupParticipant> GroupParticipants { get; set; }
}