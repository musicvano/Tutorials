namespace Pages;

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
        topPanel = new Panel();
        urlsLabel = new Label();
        urlsTextBox = new TextBox();
        parallelLabel = new Label();
        parallelNumeric = new NumericUpDown();
        timeoutLabel = new Label();
        timeoutNumeric = new NumericUpDown();
        loadButton = new Button();
        resultsGrid = new DataGridView();
        statusLabel = new Label();
        topPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)parallelNumeric).BeginInit();
        ((System.ComponentModel.ISupportInitialize)timeoutNumeric).BeginInit();
        ((System.ComponentModel.ISupportInitialize)resultsGrid).BeginInit();
        SuspendLayout();
        //
        // topPanel
        //
        topPanel.Controls.Add(urlsLabel);
        topPanel.Controls.Add(urlsTextBox);
        topPanel.Controls.Add(parallelLabel);
        topPanel.Controls.Add(parallelNumeric);
        topPanel.Controls.Add(timeoutLabel);
        topPanel.Controls.Add(timeoutNumeric);
        topPanel.Controls.Add(loadButton);
        topPanel.Dock = DockStyle.Top;
        topPanel.Location = new Point(0, 0);
        topPanel.Name = "topPanel";
        topPanel.Size = new Size(640, 170);
        topPanel.TabIndex = 0;
        //
        // urlsLabel
        //
        urlsLabel.AutoSize = true;
        urlsLabel.Location = new Point(12, 9);
        urlsLabel.Name = "urlsLabel";
        urlsLabel.Size = new Size(38, 15);
        urlsLabel.TabIndex = 0;
        urlsLabel.Text = "&URLs:";
        //
        // urlsTextBox
        //
        urlsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        urlsTextBox.Lines = new string[] {
            "https://learn.microsoft.com/dotnet/csharp/asynchronous-programming/",
            "https://learn.microsoft.com/dotnet/standard/threading/cancellation-in-managed-threads",
            "https://dotnet.microsoft.com/",
            "https://www.nuget.org/",
            "https://learn.microsoft.com/dotnet/no-such-page" };
        urlsTextBox.Location = new Point(12, 28);
        urlsTextBox.Multiline = true;
        urlsTextBox.Name = "urlsTextBox";
        urlsTextBox.ScrollBars = ScrollBars.Vertical;
        urlsTextBox.Size = new Size(616, 100);
        urlsTextBox.TabIndex = 1;
        urlsTextBox.WordWrap = false;
        //
        // parallelLabel
        //
        parallelLabel.AutoSize = true;
        parallelLabel.Location = new Point(12, 141);
        parallelLabel.Name = "parallelLabel";
        parallelLabel.Size = new Size(50, 15);
        parallelLabel.TabIndex = 2;
        parallelLabel.Text = "&Parallel:";
        //
        // parallelNumeric
        //
        parallelNumeric.Location = new Point(70, 138);
        parallelNumeric.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
        parallelNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        parallelNumeric.Name = "parallelNumeric";
        parallelNumeric.Size = new Size(60, 23);
        parallelNumeric.TabIndex = 3;
        parallelNumeric.Value = new decimal(new int[] { 3, 0, 0, 0 });
        //
        // timeoutLabel
        //
        timeoutLabel.AutoSize = true;
        timeoutLabel.Location = new Point(150, 141);
        timeoutLabel.Name = "timeoutLabel";
        timeoutLabel.Size = new Size(66, 15);
        timeoutLabel.TabIndex = 4;
        timeoutLabel.Text = "&Timeout, s:";
        //
        // timeoutNumeric
        //
        timeoutNumeric.Location = new Point(222, 138);
        timeoutNumeric.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
        timeoutNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        timeoutNumeric.Name = "timeoutNumeric";
        timeoutNumeric.Size = new Size(60, 23);
        timeoutNumeric.TabIndex = 5;
        timeoutNumeric.Value = new decimal(new int[] { 10, 0, 0, 0 });
        //
        // loadButton
        //
        loadButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        loadButton.Location = new Point(522, 137);
        loadButton.Name = "loadButton";
        loadButton.Size = new Size(106, 25);
        loadButton.TabIndex = 6;
        loadButton.Text = "&Load";
        loadButton.UseVisualStyleBackColor = true;
        loadButton.Click += loadButton_Click;
        //
        // resultsGrid
        //
        resultsGrid.AllowUserToAddRows = false;
        resultsGrid.AllowUserToDeleteRows = false;
        resultsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        resultsGrid.BackgroundColor = SystemColors.Window;
        resultsGrid.Dock = DockStyle.Fill;
        resultsGrid.Location = new Point(0, 170);
        resultsGrid.Name = "resultsGrid";
        resultsGrid.ReadOnly = true;
        resultsGrid.RowHeadersVisible = false;
        resultsGrid.Size = new Size(640, 204);
        resultsGrid.TabIndex = 1;
        //
        // statusLabel
        //
        statusLabel.Dock = DockStyle.Bottom;
        statusLabel.Location = new Point(0, 374);
        statusLabel.Name = "statusLabel";
        statusLabel.Padding = new Padding(8, 0, 0, 0);
        statusLabel.Size = new Size(640, 26);
        statusLabel.TabIndex = 2;
        statusLabel.Text = "Ready";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(640, 400);
        Controls.Add(resultsGrid);
        Controls.Add(statusLabel);
        Controls.Add(topPanel);
        MinimumSize = new Size(480, 320);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Page Loader";
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)parallelNumeric).EndInit();
        ((System.ComponentModel.ISupportInitialize)timeoutNumeric).EndInit();
        ((System.ComponentModel.ISupportInitialize)resultsGrid).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel topPanel;
    private Label urlsLabel;
    private TextBox urlsTextBox;
    private Label parallelLabel;
    private NumericUpDown parallelNumeric;
    private Label timeoutLabel;
    private NumericUpDown timeoutNumeric;
    private Button loadButton;
    private DataGridView resultsGrid;
    private Label statusLabel;
}
