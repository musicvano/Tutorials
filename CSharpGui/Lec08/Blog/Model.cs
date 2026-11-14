using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Post> Posts { get; } = [];
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public DateOnly Published { get; set; }
    public int AuthorId { get; set; }             // зовнішній ключ
    public Author Author { get; set; } = null!;   // навігація
}

public class BlogContext : DbContext
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Post> Posts => Set<Post>();

    // Рядок з’єднання: секрети користувача або змінна середовища
    private static readonly string? ConnectionString =
        new ConfigurationBuilder()
            .AddUserSecrets<BlogContext>()
            .AddEnvironmentVariables()
            .Build()
            .GetConnectionString("Blog");

    protected override void OnConfiguring(
        DbContextOptionsBuilder options) =>
        options.UseNpgsql(ConnectionString);
}
