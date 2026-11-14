namespace BmiCalculator;

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
        heightLabel = new Label();
        heightNumeric = new NumericUpDown();
        weightLabel = new Label();
        weightNumeric = new NumericUpDown();
        resetButton = new Button();
        bmiLabel = new Label();
        categoryLabel = new Label();
        rangeLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)heightNumeric).BeginInit();
        ((System.ComponentModel.ISupportInitialize)weightNumeric).BeginInit();
        SuspendLayout();
        //
        // heightLabel
        //
        heightLabel.AutoSize = true;
        heightLabel.Location = new Point(12, 15);
        heightLabel.Name = "heightLabel";
        heightLabel.Size = new Size(69, 15);
        heightLabel.TabIndex = 0;
        heightLabel.Text = "&Height, cm:";
        //
        // heightNumeric
        //
        heightNumeric.Location = new Point(100, 12);
        heightNumeric.Maximum = new decimal(new int[] { 250, 0, 0, 0 });
        heightNumeric.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
        heightNumeric.Name = "heightNumeric";
        heightNumeric.Size = new Size(90, 23);
        heightNumeric.TabIndex = 1;
        heightNumeric.Value = new decimal(new int[] { 170, 0, 0, 0 });
        heightNumeric.ValueChanged += inputs_ValueChanged;
        //
        // weightLabel
        //
        weightLabel.AutoSize = true;
        weightLabel.Location = new Point(12, 44);
        weightLabel.Name = "weightLabel";
        weightLabel.Size = new Size(67, 15);
        weightLabel.TabIndex = 2;
        weightLabel.Text = "&Weight, kg:";
        //
        // weightNumeric
        //
        weightNumeric.DecimalPlaces = 1;
        weightNumeric.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
        weightNumeric.Location = new Point(100, 41);
        weightNumeric.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
        weightNumeric.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
        weightNumeric.Name = "weightNumeric";
        weightNumeric.Size = new Size(90, 23);
        weightNumeric.TabIndex = 3;
        weightNumeric.Value = new decimal(new int[] { 65, 0, 0, 0 });
        weightNumeric.ValueChanged += inputs_ValueChanged;
        //
        // resetButton
        //
        resetButton.Location = new Point(268, 11);
        resetButton.Name = "resetButton";
        resetButton.Size = new Size(120, 27);
        resetButton.TabIndex = 4;
        resetButton.Text = "&Reset (Esc)";
        resetButton.UseVisualStyleBackColor = true;
        resetButton.Click += resetButton_Click;
        //
        // bmiLabel
        //
        bmiLabel.AutoSize = true;
        bmiLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        bmiLabel.Location = new Point(12, 78);
        bmiLabel.Name = "bmiLabel";
        bmiLabel.Size = new Size(99, 25);
        bmiLabel.TabIndex = 5;
        bmiLabel.Text = "BMI: 22,5";
        //
        // categoryLabel
        //
        categoryLabel.BorderStyle = BorderStyle.FixedSingle;
        categoryLabel.Location = new Point(12, 114);
        categoryLabel.Name = "categoryLabel";
        categoryLabel.Size = new Size(376, 30);
        categoryLabel.TabIndex = 6;
        categoryLabel.Text = "Normal weight";
        categoryLabel.TextAlign = ContentAlignment.MiddleCenter;
        //
        // rangeLabel
        //
        rangeLabel.AutoSize = true;
        rangeLabel.Location = new Point(12, 156);
        rangeLabel.Name = "rangeLabel";
        rangeLabel.Size = new Size(190, 15);
        rangeLabel.TabIndex = 7;
        rangeLabel.Text = "Normal weight: 53,5–72,0 kg";
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = resetButton;
        ClientSize = new Size(400, 185);
        Controls.Add(rangeLabel);
        Controls.Add(categoryLabel);
        Controls.Add(bmiLabel);
        Controls.Add(resetButton);
        Controls.Add(weightNumeric);
        Controls.Add(weightLabel);
        Controls.Add(heightNumeric);
        Controls.Add(heightLabel);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "BMI Calculator";
        ((System.ComponentModel.ISupportInitialize)heightNumeric).EndInit();
        ((System.ComponentModel.ISupportInitialize)weightNumeric).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label heightLabel;
    private NumericUpDown heightNumeric;
    private Label weightLabel;
    private NumericUpDown weightNumeric;
    private Button resetButton;
    private Label bmiLabel;
    private Label categoryLabel;
    private Label rangeLabel;
}
