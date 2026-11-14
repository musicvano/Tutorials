using System.Data;
using Npgsql;

namespace BookBrowser;

public class MainForm : Form
{
    private readonly NpgsqlDataSource dataSource;
    private readonly TextBox searchBox = new() { Width = 200 };
    private readonly DataGridView grid = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };
    private readonly ToolStripStatusLabel status = new();

    public MainForm(NpgsqlDataSource dataSource)
    {
        SuspendLayout();
        AutoScaleDimensions = new SizeF(96, 96); // розміри для 100 %
        AutoScaleMode = AutoScaleMode.Dpi;       // масштаб під DPI
        this.dataSource = dataSource;
        Text = "Library Books";
        ClientSize = new Size(640, 360);
        var search = new Button { Text = "Search", AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink };
        search.Click += (s, e) => LoadBooks();
        var top = new FlowLayoutPanel
        {
            Dock = DockStyle.Top, Height = 36
        };
        top.Controls.AddRange([searchBox, search]);
        var bar = new StatusStrip();
        bar.Items.Add(status);
        Controls.AddRange([grid, top, bar]);
        AcceptButton = search;
        Load += (s, e) => LoadBooks();
        ResumeLayout();
    }

    private void LoadBooks()
    {
        // Адаптер сам відкриває з’єднання, тому команда має
        // власне (ще закрите) з’єднання, а не CreateCommand.
        using NpgsqlConnection conn = dataSource.CreateConnection();
        using var cmd = new NpgsqlCommand("""
            SELECT id, title, pub_year AS "Year", isbn, copies
            FROM books
            WHERE title ILIKE $1
            ORDER BY title
            """, conn);
        cmd.Parameters.AddWithValue($"%{searchBox.Text.Trim()}%");
        using var adapter = new NpgsqlDataAdapter(cmd);
        var table = new DataTable();
        try
        {
            adapter.Fill(table);   // відкрити, прочитати, закрити
            grid.DataSource = table;
            status.Text = $"Books: {table.Rows.Count}";
        }
        catch (NpgsqlException ex)
        {
            MessageBox.Show(this, ex.Message, "Database error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
