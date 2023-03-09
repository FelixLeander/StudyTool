using Microsoft.EntityFrameworkCore;
using StudyTool.Models;

namespace StudyTool.Data;

/// <summary>
/// Provides interaction with the Database by inheriting from <see cref="DbContext"/>.
/// This class should only contain data strictly bound to the database, to let the project interact with it use the <see cref="DataAccessor"/> class.
/// </summary>
public class DatabaseContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"FileName=.\Database\Database.db");
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<User> Users => Set<User>();
}
