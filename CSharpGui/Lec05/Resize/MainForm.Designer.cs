namespace Resize;

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
        widthLabel = new Label();
        widthNumeric = new NumericUpDown();
        parallelLabel = new Label();
        parallelNumeric = new NumericUpDown();
        startButton = new Button();
        errorsListBox = new ListBox();
        progressBar = new ProgressBar();
        statusLabel = new Label();
        topPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)widthNumeric).BeginInit();
        ((System.ComponentModel.ISupportInitialize)parallelNumeric).BeginInit();
        SuspendLayout();
        //
        // topPanel
        //
        topPanel.Controls.Add(folderLabel);
        topPanel.Controls.Add(folderTextBox);
        topPanel.Controls.Add(widthLabel);
        topPanel.Controls.Add(widthNumeric);
        topPanel.Controls.Add(parallelLabel);
        topPanel.Controls.Add(parallelNumeric);
        topPanel.Controls.Add(startButton);
        topPanel.Dock = DockStyle.Top;
        topPanel.Location = new Point(0, 0);
        topPanel.Name = "topPanel";
        topPanel.Size = new Size(640, 46);
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
        folderTextBox.Location = new Point(62, 12);
        folderTextBox.Name = "folderTextBox";
        folderTextBox.Size = new Size(262, 23);
        folderTextBox.TabIndex = 1;
        folderTextBox.Text = "D:\\Courses\\content\\csharp-advanced\\code\\Lec05\\photos";
        //
        // widthLabel
        //
        widthLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        widthLabel.AutoSize = true;
        widthLabel.Location = new Point(334, 15);
        widthLabel.Name = "widthLabel";
        widthLabel.Size = new Size(42, 15);
        widthLabel.TabIndex = 2;
        widthLabel.Text = "&Width:";
        //
        // widthNumeric
        //
        widthNumeric.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        widthNumeric.Increment = new decimal(new int[] { 100, 0, 0, 0 });
        widthNumeric.Location = new Point(380, 12);
        widthNumeric.Maximum = new decimal(new int[] { 4000, 0, 0, 0 });
        widthNumeric.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
        widthNumeric.Name = "widthNumeric";
        widthNumeric.Size = new Size(62, 23);
        widthNumeric.TabIndex = 3;
        widthNumeric.Value = new decimal(new int[] { 800, 0, 0, 0 });
        //
        // parallelLabel
        //
        parallelLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        parallelLabel.AutoSize = true;
        parallelLabel.Location = new Point(452, 15);
        parallelLabel.Name = "parallelLabel";
        parallelLabel.Size = new Size(50, 15);
        parallelLabel.TabIndex = 4;
        parallelLabel.Text = "&Parallel:";
        //
        // parallelNumeric
        //
        parallelNumeric.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        parallelNumeric.Location = new Point(506, 12);
        parallelNumeric.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
        parallelNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        parallelNumeric.Name = "parallelNumeric";
        parallelNumeric.Size = new Size(46, 23);
        parallelNumeric.TabIndex = 5;
        parallelNumeric.Value = new decimal(new int[] { 4, 0, 0, 0 });
        //
        // startButton
        //
        startButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        startButton.Location = new Point(562, 11);
        startButton.Name = "startButton";
        startButton.Size = new Size(66, 25);
        startButton.TabIndex = 6;
        startButton.Text = "&Start";
        startButton.UseVisualStyleBackColor = true;
        startButton.Click += startButton_Click;
        //
        // errorsListBox
        //
        errorsListBox.Dock = DockStyle.Fill;
        errorsListBox.HorizontalScrollbar = true;
        errorsListBox.IntegralHeight = false;
        errorsListBox.Location = new Point(0, 46);
        errorsListBox.Name = "errorsListBox";
        errorsListBox.Size = new Size(640, 206);
        errorsListBox.TabIndex = 1;
        //
        // progressBar
        //
        progressBar.Dock = DockStyle.Bottom;
        progressBar.Location = new Point(0, 252);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(640, 22);
        progressBar.TabIndex = 2;
        //
        // statusLabel
        //
        statusLabel.Dock = DockStyle.Bottom;
        statusLabel.Location = new Point(0, 274);
        statusLabel.Name = "statusLabel";
        statusLabel.Padding = new Padding(8, 0, 0, 0);
        statusLabel.Size = new Size(640, 26);
        statusLabel.TabIndex = 3;
        statusLabel.Text = "Ready";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // MainForm
        //
        AcceptButton = startButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(640, 300);
        Controls.Add(errorsListBox);
        Controls.Add(progressBar);
        Controls.Add(statusLabel);
        Controls.Add(topPanel);
        MinimumSize = new Size(560, 240);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Photo Resizer";
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)widthNumeric).EndInit();
        ((System.ComponentModel.ISupportInitialize)parallelNumeric).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel topPanel;
    private Label folderLabel;
    private TextBox folderTextBox;
    private Label widthLabel;
    private NumericUpDown widthNumeric;
    private Label parallelLabel;
    private NumericUpDown parallelNumeric;
    private Button startButton;
    private ListBox errorsListBox;
    private ProgressBar progressBar;
    private Label statusLabel;
}
