namespace BoardClient;

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
        toolStrip = new ToolStrip();
        widthLabel = new ToolStripLabel();
        widthComboBox = new ToolStripComboBox();
        clearButton = new ToolStripButton();
        canvas = new PictureBox();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        toolStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
        statusStrip.SuspendLayout();
        SuspendLayout();
        //
        // toolStrip
        //
        toolStrip.GripStyle = ToolStripGripStyle.Hidden;
        toolStrip.Items.AddRange(new ToolStripItem[] { widthLabel, widthComboBox, clearButton });
        toolStrip.Location = new Point(0, 0);
        toolStrip.Name = "toolStrip";
        toolStrip.Size = new Size(460, 25);
        toolStrip.TabIndex = 0;
        //
        // widthLabel
        //
        widthLabel.Name = "widthLabel";
        widthLabel.Size = new Size(42, 22);
        widthLabel.Text = "Width:";
        //
        // widthComboBox
        //
        widthComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        widthComboBox.Name = "widthComboBox";
        widthComboBox.Size = new Size(60, 25);
        //
        // clearButton
        //
        clearButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        clearButton.Name = "clearButton";
        clearButton.Size = new Size(38, 22);
        clearButton.Text = "Clear";
        clearButton.Click += clearButton_Click;
        //
        // canvas
        //
        canvas.BackColor = Color.White;
        canvas.Cursor = Cursors.Cross;
        canvas.Dock = DockStyle.Fill;
        canvas.Location = new Point(0, 25);
        canvas.Name = "canvas";
        canvas.Size = new Size(460, 253);
        canvas.TabIndex = 1;
        canvas.TabStop = false;
        canvas.Paint += canvas_Paint;
        canvas.MouseDown += canvas_MouseDown;
        canvas.MouseMove += canvas_MouseMove;
        canvas.MouseUp += canvas_MouseUp;
        //
        // statusStrip
        //
        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
        statusStrip.Location = new Point(0, 278);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(460, 22);
        statusStrip.TabIndex = 2;
        //
        // statusLabel
        //
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(82, 17);
        statusLabel.Text = "Connecting...";
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(460, 300);
        Controls.Add(canvas);
        Controls.Add(toolStrip);
        Controls.Add(statusStrip);
        Name = "MainForm";
        StartPosition = FormStartPosition.WindowsDefaultLocation;
        Text = "Shared Whiteboard";
        Load += MainForm_Load;
        toolStrip.ResumeLayout(false);
        toolStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ToolStrip toolStrip;
    private ToolStripLabel widthLabel;
    private ToolStripComboBox widthComboBox;
    private ToolStripButton clearButton;
    private PictureBox canvas;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
}
