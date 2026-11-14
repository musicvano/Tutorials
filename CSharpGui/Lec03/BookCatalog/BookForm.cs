namespace BookCatalog;

public partial class BookForm : Form
{
    private readonly Book book;

    public BookForm(Book book)
    {
        InitializeComponent();
        this.book = book;
        Text = book.Title.Length == 0 ? "New Book" : "Edit Book";
        titleTextBox.Text = book.Title;
        authorTextBox.Text = book.Author;
        yearNumeric.Value = book.Year;
        priceNumeric.Value = book.Price;
    }

    private void okButton_Click(object sender, EventArgs e)
    {
        string title = titleTextBox.Text.Trim();
        if (title.Length == 0)
        {
            errorProvider.SetError(titleTextBox, "Enter the title");
            titleTextBox.Focus();
            return;           // форма залишається відкритою
        }
        book.Title = title;
        book.Author = authorTextBox.Text.Trim();
        book.Year = (int)yearNumeric.Value;
        book.Price = priceNumeric.Value;
        DialogResult = DialogResult.OK;  // закрити модальну форму
    }
}
