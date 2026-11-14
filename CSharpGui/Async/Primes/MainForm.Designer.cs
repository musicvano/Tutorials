namespace Primes;

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
        limitLabel = new Label();
        limitNumeric = new NumericUpDown();
        syncButton = new Button();
        asyncButton = new Button();
        resultLabel = new Label();
        clockLabel = new Label();
        clockTimer = new System.Windows.Forms.Timer(components);
        ((System.ComponentModel.ISupportInitialize)limitNumeric).BeginInit();
        SuspendLayout();
        //
        // limitLabel
        //
        limitLabel.AutoSize = true;
        limitLabel.Location = new Point(12, 16);
        limitLabel.Name = "limitLabel";
        limitLabel.Size = new Size(37, 15);
        limitLabel.TabIndex = 0;
        limitLabel.Text = "&Limit:";
        //
        // limitNumeric
        //
        limitNumeric.Location = new Point(60, 12);
        limitNumeric.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        limitNumeric.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
        limitNumeric.Name = "limitNumeric";
        limitNumeric.Size = new Size(150, 23);
        limitNumeric.TabIndex = 1;
        limitNumeric.TextAlign = HorizontalAlignment.Right;
        limitNumeric.ThousandsSeparator = true;
        limitNumeric.Value = new decimal(new int[] { 10000000, 0, 0, 0 });
        //
        // syncButton
        //
        syncButton.Location = new Point(12, 48);
        syncButton.Name = "syncButton";
        syncButton.Size = new Size(130, 27);
        syncButton.TabIndex = 2;
        syncButton.Text = "Count (&sync)";
        syncButton.UseVisualStyleBackColor = true;
        syncButton.Click += syncButton_Click;
        //
        // asyncButton
        //
        asyncButton.Location = new Point(150, 48);
        asyncButton.Name = "asyncButton";
        asyncButton.Size = new Size(130, 27);
        asyncButton.TabIndex = 3;
        asyncButton.Text = "Count (&async)";
        asyncButton.UseVisualStyleBackColor = true;
        asyncButton.Click += asyncButton_Click;
        //
        // resultLabel
        //
        resultLabel.Location = new Point(12, 90);
        resultLabel.Name = "resultLabel";
        resultLabel.Size = new Size(396, 40);
        resultLabel.TabIndex = 4;
        resultLabel.Text = "Press a button to count primes.";
        //
        // clockLabel
        //
        clockLabel.AutoSize = true;
        clockLabel.Font = new Font("Consolas", 16F);
        clockLabel.Location = new Point(12, 140);
        clockLabel.Name = "clockLabel";
        clockLabel.Size = new Size(130, 26);
        clockLabel.TabIndex = 5;
        clockLabel.Text = "00:00:00.0";
        //
        // clockTimer
        //
        clockTimer.Tick += clockTimer_Tick;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(420, 185);
        Controls.Add(clockLabel);
        Controls.Add(resultLabel);
        Controls.Add(asyncButton);
        Controls.Add(syncButton);
        Controls.Add(limitNumeric);
        Controls.Add(limitLabel);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Primes";
        ((System.ComponentModel.ISupportInitialize)limitNumeric).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label limitLabel;
    private NumericUpDown limitNumeric;
    private Button syncButton;
    private Button asyncButton;
    private Label resultLabel;
    private Label clockLabel;
    private System.Windows.Forms.Timer clockTimer;
}
