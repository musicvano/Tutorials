namespace TextEditor;

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
        menuStrip = new MenuStrip();
        fileMenuItem = new ToolStripMenuItem();
        openMenuItem = new ToolStripMenuItem();
        saveMenuItem = new ToolStripMenuItem();
        fileSeparator = new ToolStripSeparator();
        exitMenuItem = new ToolStripMenuItem();
        editorTextBox = new TextBox();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        openFileDialog = new OpenFileDialog();
        saveFileDialog = new SaveFileDialog();
        menuStrip.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        //
        // menuStrip
        //
        menuStrip.Items.AddRange(new ToolStripItem[] { fileMenuItem });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(480, 24);
        menuStrip.TabIndex = 0;
        //
        // fileMenuItem
        //
        fileMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openMenuItem, saveMenuItem, fileSeparator, exitMenuItem });
        fileMenuItem.Name = "fileMenuItem";
        fileMenuItem.Size = new Size(37, 20);
        fileMenuItem.Text = "&File";
        //
        // openMenuItem
        //
        openMenuItem.Name = "openMenuItem";
        openMenuItem.ShortcutKeys = Keys.Control | Keys.O;
        openMenuItem.Size = new Size(155, 22);
        openMenuItem.Text = "&Open...";
        openMenuItem.Click += openMenuItem_Click;
        //
        // saveMenuItem
        //
        saveMenuItem.Name = "saveMenuItem";
        saveMenuItem.ShortcutKeys = Keys.Control | Keys.S;
        saveMenuItem.Size = new Size(155, 22);
        saveMenuItem.Text = "&Save";
        saveMenuItem.Click += saveMenuItem_Click;
        //
        // fileSeparator
        //
        fileSeparator.Name = "fileSeparator";
        fileSeparator.Size = new Size(152, 6);
        //
        // exitMenuItem
        //
        exitMenuItem.Name = "exitMenuItem";
        exitMenuItem.Size = new Size(155, 22);
        exitMenuItem.Text = "E&xit";
        exitMenuItem.Click += exitMenuItem_Click;
        //
        // editorTextBox
        //
        editorTextBox.Dock = DockStyle.Fill;
        editorTextBox.Location = new Point(0, 24);
        editorTextBox.Multiline = true;
        editorTextBox.Name = "editorTextBox";
        editorTextBox.ScrollBars = ScrollBars.Vertical;
        editorTextBox.Size = new Size(480, 254);
        editorTextBox.TabIndex = 1;
        editorTextBox.TextChanged += editorTextBox_TextChanged;
        //
        // statusStrip
        //
        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
        statusStrip.Location = new Point(0, 278);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(480, 22);
        statusStrip.TabIndex = 2;
        //
        // statusLabel
        //
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(118, 17);
        statusLabel.Text = "Lines: 0   Characters: 0";
        //
        // openFileDialog
        //
        openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        //
        // saveFileDialog
        //
        saveFileDialog.DefaultExt = "txt";
        saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(480, 300);
        Controls.Add(editorTextBox);
        Controls.Add(statusStrip);
        Controls.Add(menuStrip);
        MainMenuStrip = menuStrip;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Untitled – Text Editor";
        FormClosing += MainForm_FormClosing;
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip;
    private ToolStripMenuItem fileMenuItem;
    private ToolStripMenuItem openMenuItem;
    private ToolStripMenuItem saveMenuItem;
    private ToolStripSeparator fileSeparator;
    private ToolStripMenuItem exitMenuItem;
    private TextBox editorTextBox;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
    private OpenFileDialog openFileDialog;
    private SaveFileDialog saveFileDialog;
}
