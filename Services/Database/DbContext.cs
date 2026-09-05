using library_management.Model;
using Microsoft.EntityFrameworkCore;

namespace library_management.Services.Database ;

public class LibraryContext: DbContext
{
    public DbSet<Book> Books { get; set; }
    
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}