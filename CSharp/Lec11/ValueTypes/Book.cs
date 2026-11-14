class BookClass(string title, string author)
{
    public string Title { get; } = title;
    public string Author { get; } = author;
}

record Book(string Title, string Author);
