namespace Checksums;

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
        buttonPanel = new FlowLayoutPanel();
        computeButton = new Button();
        cancelButton = new Button();
        hashListBox = new ListBox();
        progressBar = new ProgressBar();
        statusLabel = new Label();
        openFileDialog = new OpenFileDialog();
        buttonPanel.SuspendLayout();
        SuspendLayout();
        //
        // buttonPanel
        //
        buttonPanel.AutoSize = true;
        buttonPanel.Controls.Add(computeButton);
        buttonPanel.Controls.Add(cancelButton);
        buttonPanel.Dock = DockStyle.Top;
        buttonPanel.Location = new Point(0, 0);
        buttonPanel.Name = "buttonPanel";
        buttonPanel.Padding = new Padding(6);
        buttonPanel.Size = new Size(640, 45);
        buttonPanel.TabIndex = 0;
        //
        // computeButton
        //
        computeButton.Location = new Point(9, 9);
        computeButton.Name = "computeButton";
        computeButton.Size = new Size(106, 27);
        computeButton.TabIndex = 0;
        computeButton.Text = "C&ompute...";
        computeButton.UseVisualStyleBackColor = true;
        computeButton.Click += computeButton_Click;
        //
        // cancelButton
        //
        cancelButton.Location = new Point(121, 9);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(106, 27);
        cancelButton.TabIndex = 1;
        cancelButton.Text = "&Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += cancelButton_Click;
        //
        // hashListBox
        //
        hashListBox.Dock = DockStyle.Fill;
        hashListBox.Font = new Font("Consolas", 10F);
        hashListBox.HorizontalScrollbar = true;
        hashListBox.IntegralHeight = false;
        hashListBox.Location = new Point(0, 45);
        hashListBox.Name = "hashListBox";
        hashListBox.Size = new Size(640, 207);
        hashListBox.TabIndex = 1;
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
        // openFileDialog
        //
        openFileDialog.InitialDirectory = "D:\\Courses\\content\\csharp-advanced\\code\\Lec05\\hash";
        openFileDialog.Multiselect = true;
        openFileDialog.Title = "Select files";
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(640, 300);
        Controls.Add(hashListBox);
        Controls.Add(progressBar);
        Controls.Add(statusLabel);
        Controls.Add(buttonPanel);
        MinimumSize = new Size(480, 240);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Checksums";
        buttonPanel.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private FlowLayoutPanel buttonPanel;
    private Button computeButton;
    private Button cancelButton;
    private ListBox hashListBox;
    private ProgressBar progressBar;
    private Label statusLabel;
    private OpenFileDialog openFileDialog;
}
