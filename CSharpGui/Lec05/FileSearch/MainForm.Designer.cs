namespace FileSearch;

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
        folderLabel = new Label();
        folderTextBox = new TextBox();
        searchButton = new Button();
        textLabel = new Label();
        textTextBox = new TextBox();
        cancelButton = new Button();
        resultsListBox = new ListBox();
        progressBar = new ProgressBar();
        statusLabel = new Label();
        topPanel.SuspendLayout();
        SuspendLayout();
        //
        // topPanel
        //
        topPanel.Controls.Add(folderLabel);
        topPanel.Controls.Add(folderTextBox);
        topPanel.Controls.Add(searchButton);
        topPanel.Controls.Add(textLabel);
        topPanel.Controls.Add(textTextBox);
        topPanel.Controls.Add(cancelButton);
        topPanel.Dock = DockStyle.Top;
        topPanel.Location = new Point(0, 0);
        topPanel.Name = "topPanel";
        topPanel.Size = new Size(640, 74);
        topPanel.TabIndex = 0;
        //
        // folderLabel
        //
        folderLabel.AutoSize = true;
        folderLabel.Location = new Point(12, 15);
        folderLabel.Name = "folderLabel";
        folderLabel.Size = new Size(43, 15);
        folderLabel.TabIndex = 0;
        folderLabel.Text = "&Folder:";
        //
        // folderTextBox
        //
        folderTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        folderTextBox.Location = new Point(70, 12);
        folderTextBox.Name = "folderTextBox";
        folderTextBox.Size = new Size(440, 23);
        folderTextBox.TabIndex = 1;
        folderTextBox.Text = "D:\\Courses\\content\\csharp-advanced\\code\\Lec05\\search-data";
        //
        // searchButton
        //
        searchButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        searchButton.Location = new Point(522, 11);
        searchButton.Name = "searchButton";
        searchButton.Size = new Size(106, 25);
        searchButton.TabIndex = 4;
        searchButton.Text = "&Search";
        searchButton.UseVisualStyleBackColor = true;
        searchButton.Click += searchButton_Click;
        //
        // textLabel
        //
        textLabel.AutoSize = true;
        textLabel.Location = new Point(12, 44);
        textLabel.Name = "textLabel";
        textLabel.Size = new Size(31, 15);
        textLabel.TabIndex = 2;
        textLabel.Text = "&Text:";
        //
        // textTextBox
        //
        textTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        textTextBox.Location = new Point(70, 41);
        textTextBox.Name = "textTextBox";
        textTextBox.Size = new Size(440, 23);
        textTextBox.TabIndex = 3;
        textTextBox.Text = "cancellationtoken";
        //
        // cancelButton
        //
        cancelButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cancelButton.Location = new Point(522, 40);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(106, 25);
        cancelButton.TabIndex = 5;
        cancelButton.Text = "&Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += cancelButton_Click;
        //
        // resultsListBox
        //
        resultsListBox.Dock = DockStyle.Fill;
        resultsListBox.HorizontalScrollbar = true;
        resultsListBox.IntegralHeight = false;
        resultsListBox.Location = new Point(0, 74);
        resultsListBox.Name = "resultsListBox";
        resultsListBox.Size = new Size(640, 278);
        resultsListBox.TabIndex = 1;
        //
        // progressBar
        //
        progressBar.Dock = DockStyle.Bottom;
        progressBar.Location = new Point(0, 352);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(640, 22);
        progressBar.TabIndex = 2;
        //
        // statusLabel
        //
        statusLabel.Dock = DockStyle.Bottom;
        statusLabel.Location = new Point(0, 374);
        statusLabel.Name = "statusLabel";
        statusLabel.Padding = new Padding(8, 0, 0, 0);
        statusLabel.Size = new Size(640, 26);
        statusLabel.TabIndex = 3;
        statusLabel.Text = "Ready";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // MainForm
        //
        AcceptButton = searchButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(640, 400);
        Controls.Add(resultsListBox);
        Controls.Add(progressBar);
        Controls.Add(statusLabel);
        Controls.Add(topPanel);
        MinimumSize = new Size(480, 300);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "File Search";
        FormClosing += MainForm_FormClosing;
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel topPanel;
    private Label folderLabel;
    private TextBox folderTextBox;
    private Button searchButton;
    private Label textLabel;
    private TextBox textTextBox;
    private Button cancelButton;
    private ListBox resultsListBox;
    private ProgressBar progressBar;
    private Label statusLabel;
}
