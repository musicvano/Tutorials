namespace StopwatchApp;

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
        timeLabel = new Label();
        buttonPanel = new FlowLayoutPanel();
        startStopButton = new Button();
        lapButton = new Button();
        resetButton = new Button();
        saveButton = new Button();
        lapsListBox = new ListBox();
        uiTimer = new System.Windows.Forms.Timer(components);
        saveFileDialog = new SaveFileDialog();
        buttonPanel.SuspendLayout();
        SuspendLayout();
        //
        // timeLabel
        //
        timeLabel.Dock = DockStyle.Top;
        timeLabel.Font = new Font("Consolas", 32F);
        timeLabel.Location = new Point(0, 0);
        timeLabel.Name = "timeLabel";
        timeLabel.Size = new Size(400, 70);
        timeLabel.TabIndex = 0;
        timeLabel.Text = "00:00.00";
        timeLabel.TextAlign = ContentAlignment.MiddleCenter;
        //
        // buttonPanel
        //
        buttonPanel.AutoSize = true;
        buttonPanel.Controls.Add(startStopButton);
        buttonPanel.Controls.Add(lapButton);
        buttonPanel.Controls.Add(resetButton);
        buttonPanel.Controls.Add(saveButton);
        buttonPanel.Dock = DockStyle.Top;
        buttonPanel.Location = new Point(0, 70);
        buttonPanel.Name = "buttonPanel";
        buttonPanel.Padding = new Padding(6, 0, 6, 6);
        buttonPanel.Size = new Size(400, 39);
        buttonPanel.TabIndex = 1;
        //
        // startStopButton
        //
        startStopButton.Location = new Point(9, 3);
        startStopButton.Name = "startStopButton";
        startStopButton.Size = new Size(88, 27);
        startStopButton.TabIndex = 0;
        startStopButton.Text = "&Start";
        startStopButton.UseVisualStyleBackColor = true;
        startStopButton.Click += startStopButton_Click;
        //
        // lapButton
        //
        lapButton.Location = new Point(103, 3);
        lapButton.Name = "lapButton";
        lapButton.Size = new Size(88, 27);
        lapButton.TabIndex = 1;
        lapButton.Text = "&Lap";
        lapButton.UseVisualStyleBackColor = true;
        lapButton.Click += lapButton_Click;
        //
        // resetButton
        //
        resetButton.Location = new Point(197, 3);
        resetButton.Name = "resetButton";
        resetButton.Size = new Size(88, 27);
        resetButton.TabIndex = 2;
        resetButton.Text = "&Reset";
        resetButton.UseVisualStyleBackColor = true;
        resetButton.Click += resetButton_Click;
        //
        // saveButton
        //
        saveButton.Location = new Point(291, 3);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(88, 27);
        saveButton.TabIndex = 3;
        saveButton.Text = "S&ave...";
        saveButton.UseVisualStyleBackColor = true;
        saveButton.Click += saveButton_Click;
        //
        // lapsListBox
        //
        lapsListBox.Dock = DockStyle.Fill;
        lapsListBox.Font = new Font("Consolas", 11F);
        lapsListBox.IntegralHeight = false;
        lapsListBox.Location = new Point(0, 109);
        lapsListBox.Name = "lapsListBox";
        lapsListBox.Size = new Size(400, 141);
        lapsListBox.TabIndex = 2;
        //
        // uiTimer
        //
        uiTimer.Interval = 50;
        uiTimer.Tick += uiTimer_Tick;
        //
        // saveFileDialog
        //
        saveFileDialog.DefaultExt = "txt";
        saveFileDialog.FileName = "laps.txt";
        saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(400, 250);
        Controls.Add(lapsListBox);
        Controls.Add(buttonPanel);
        Controls.Add(timeLabel);
        KeyPreview = true;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Stopwatch";
        KeyDown += MainForm_KeyDown;
        buttonPanel.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label timeLabel;
    private FlowLayoutPanel buttonPanel;
    private Button startStopButton;
    private Button lapButton;
    private Button resetButton;
    private Button saveButton;
    private ListBox lapsListBox;
    private System.Windows.Forms.Timer uiTimer;
    private SaveFileDialog saveFileDialog;
}
