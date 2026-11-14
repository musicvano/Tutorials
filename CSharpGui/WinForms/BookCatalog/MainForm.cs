using System.ComponentModel;

namespace BookCatalog;

public partial class MainForm : Form
{
    private readonly BindingList<Book> books = new()
    {
        new() { Title = "Clean Code", Author = "Robert C. Martin",
                Year = 2008, Price = 890m },
        new() { Title = "C# 12 in a Nutshell",
                Author = "Joseph Albahari",
                Year = 2023, Price = 2150m }
    };

    public MainForm()
    {
        InitializeComponent();
        bookBindingSource.DataSource = books;
        bookGrid.DataSource = bookBindingSource;
        bookGrid.Columns["Price"]!.DefaultCellStyle.Format = "N2";
        books.ListChanged += (s, e) => UpdateStatus();
        UpdateStatus();
    }

    private void addButton_Click(object sender, EventArgs e)
    {
        var book = new Book { Year = DateTime.Today.Year };
        using var dialog = new BookForm(book);
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            books.Add(book);
            bookBindingSource.Position = books.Count - 1;
        }
    }

    private void editButton_Click(object sender, EventArgs e)
    {
        if (bookBindingSource.Current is not Book book)
        {
            return;
        }
        using var dialog = new BookForm(book);
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            bookBindingSource.ResetCurrentItem();
            UpdateStatus();
        }
    }

    private void UpdateStatus() =>
        countLabel.Text = $"Books: {books.Count}   "
            + $"Total: {books.Sum(b => b.Price):N2} UAH";
}
