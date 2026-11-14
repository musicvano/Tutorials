namespace BookCatalog;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolStrip = new ToolStrip();
        addButton = new ToolStripButton();
        editButton = new ToolStripButton();
        bookGrid = new DataGridView();
        bookBindingSource = new BindingSource(components);
        statusStrip = new StatusStrip();
        countLabel = new ToolStripStatusLabel();
        toolStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)bookGrid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)bookBindingSource).BeginInit();
        statusStrip.SuspendLayout();
        SuspendLayout();
        //
        // toolStrip
        //
        toolStrip.Items.AddRange(new ToolStripItem[] { addButton, editButton });
        toolStrip.Location = new Point(0, 0);
        toolStrip.Name = "toolStrip";
        toolStrip.Size = new Size(520, 25);
        toolStrip.TabIndex = 0;
        //
        // addButton
        //
        addButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        addButton.Name = "addButton";
        addButton.Size = new Size(33, 22);
        addButton.Text = "&Add";
        addButton.Click += addButton_Click;
        //
        // editButton
        //
        editButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        editButton.Name = "editButton";
        editButton.Size = new Size(31, 22);
        editButton.Text = "&Edit";
        editButton.Click += editButton_Click;
        //
        // bookGrid
        //
        bookGrid.AllowUserToAddRows = false;
        bookGrid.AllowUserToDeleteRows = false;
        bookGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        bookGrid.BackgroundColor = SystemColors.Window;
        bookGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        bookGrid.Dock = DockStyle.Fill;
        bookGrid.Location = new Point(0, 25);
        bookGrid.MultiSelect = false;
        bookGrid.Name = "bookGrid";
        bookGrid.ReadOnly = true;
        bookGrid.RowHeadersVisible = false;
        bookGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        bookGrid.Size = new Size(520, 173);
        bookGrid.TabIndex = 1;
        bookGrid.CellDoubleClick += editButton_Click;
        //
        // statusStrip
        //
        statusStrip.Items.AddRange(new ToolStripItem[] { countLabel });
        statusStrip.Location = new Point(0, 198);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(520, 22);
        statusStrip.TabIndex = 2;
        //
        // countLabel
        //
        countLabel.Name = "countLabel";
        countLabel.Size = new Size(118, 17);
        countLabel.Text = "Books: 0";
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(520, 220);
        Controls.Add(bookGrid);
        Controls.Add(statusStrip);
        Controls.Add(toolStrip);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Book Catalog";
        toolStrip.ResumeLayout(false);
        toolStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)bookGrid).EndInit();
        ((System.ComponentModel.ISupportInitialize)bookBindingSource).EndInit();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ToolStrip toolStrip;
    private ToolStripButton addButton;
    private ToolStripButton editButton;
    private DataGridView bookGrid;
    private BindingSource bookBindingSource;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel countLabel;
}
