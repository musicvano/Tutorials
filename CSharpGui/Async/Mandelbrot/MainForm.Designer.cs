namespace Mandelbrot;

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
        pictureBox = new PictureBox();
        progressBar = new ProgressBar();
        statusLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
        SuspendLayout();
        //
        // pictureBox
        //
        pictureBox.BackColor = Color.White;
        pictureBox.Dock = DockStyle.Fill;
        pictureBox.Location = new Point(0, 0);
        pictureBox.Name = "pictureBox";
        pictureBox.Size = new Size(800, 554);
        pictureBox.TabIndex = 0;
        pictureBox.TabStop = false;
        pictureBox.MouseClick += pictureBox_MouseClick;
        //
        // progressBar
        //
        progressBar.Dock = DockStyle.Bottom;
        progressBar.Location = new Point(0, 554);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(800, 23);
        progressBar.TabIndex = 1;
        //
        // statusLabel
        //
        statusLabel.Dock = DockStyle.Bottom;
        statusLabel.Location = new Point(0, 577);
        statusLabel.Name = "statusLabel";
        statusLabel.Padding = new Padding(8, 0, 0, 0);
        statusLabel.Size = new Size(800, 23);
        statusLabel.TabIndex = 2;
        statusLabel.Text = "Rendering...";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 600);
        Controls.Add(pictureBox);
        Controls.Add(progressBar);
        Controls.Add(statusLabel);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Mandelbrot";
        ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private PictureBox pictureBox;
    private ProgressBar progressBar;
    private Label statusLabel;
}
