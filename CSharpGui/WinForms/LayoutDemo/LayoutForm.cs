namespace LayoutDemo;

// The lecture listing: the same «label – field» form built in code.
// MainForm is its designer twin (screenshot 04, Edit Rows and Columns).
public class LayoutForm : Form
{
    public LayoutForm()
    {
        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Padding = new Padding(10)
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        AddRow("&Name:", new TextBox { Dock = DockStyle.Fill });
        AddRow("&Email:", new TextBox { Dock = DockStyle.Fill });
        AddRow("&Track:", new ComboBox { Dock = DockStyle.Fill });
        Controls.Add(table);

        void AddRow(string caption, Control control)
        {
            table.Controls.Add(new Label
            {
                Text = caption,
                AutoSize = true,
                Anchor = AnchorStyles.Left   // по центру рядка
            });
            table.Controls.Add(control);
        }
    }
}
