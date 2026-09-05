using library_management.Model;
using Microsoft.EntityFrameworkCore;

namespace library_management.Services.Database ;

public class LibraryContext: DbContext
{
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=library_management.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}