using Blog;
using Microsoft.EntityFrameworkCore;

await using var db = new BlogContext();

// Create: автор і два дописи одним SaveChangesAsync
var author = new Author { Name = "Olena Koval" };
author.Posts.Add(new Post
    { Title = "EF Core basics", Published = new(2026, 9, 1) });
author.Posts.Add(new Post
    { Title = "Migrations", Published = new(2026, 9, 8) });
db.Authors.Add(author);
int saved = await db.SaveChangesAsync();
Console.WriteLine($"Saved rows: {saved}, author Id: {author.Id}");

// Read: дописи автора, відсортовані за датою
var posts = await db.Posts
    .Where(p => p.Author.Name == "Olena Koval")
    .OrderBy(p => p.Published)
    .ToListAsync();
foreach (var p in posts)
    Console.WriteLine($"{p.Id}. {p.Title} ({p.Published:d})");

// Update: змінити властивість і зберегти
var post = await db.Posts.SingleAsync(p => p.Title == "Migrations");
post.Title = "EF Core migrations";
await db.SaveChangesAsync();

// Delete
db.Posts.Remove(post);
await db.SaveChangesAsync();
Console.WriteLine($"Posts left: {await db.Posts.CountAsync()}");
