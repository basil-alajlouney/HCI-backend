using Microsoft.EntityFrameworkCore;
namespace Connector;
public class ConnectionPoint : DbContext
{
  public DbSet<User> Users = null!;
  public DbSet<RankCard> RankCard = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlite("Filename=./connection/database.db");
    }
}