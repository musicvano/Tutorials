namespace TemperatureConverter;

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
        celsiusLabel = new Label();
        celsiusTextBox = new TextBox();
        convertButton = new Button();
        resultLabel = new Label();
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        //
        // celsiusLabel
        //
        celsiusLabel.AutoSize = true;
        celsiusLabel.Location = new Point(12, 15);
        celsiusLabel.Name = "celsiusLabel";
        celsiusLabel.Size = new Size(99, 15);
        celsiusLabel.TabIndex = 0;
        celsiusLabel.Text = "Temperature, °C:";
        //
        // celsiusTextBox
        //
        celsiusTextBox.Location = new Point(122, 12);
        celsiusTextBox.Name = "celsiusTextBox";
        celsiusTextBox.Size = new Size(105, 23);
        celsiusTextBox.TabIndex = 1;
        celsiusTextBox.Validating += celsiusTextBox_Validating;
        //
        // convertButton
        //
        convertButton.Location = new Point(122, 41);
        convertButton.Name = "convertButton";
        convertButton.Size = new Size(105, 27);
        convertButton.TabIndex = 2;
        convertButton.Text = "&Convert";
        convertButton.UseVisualStyleBackColor = true;
        convertButton.Click += convertButton_Click;
        //
        // resultLabel
        //
        resultLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        resultLabel.Location = new Point(12, 80);
        resultLabel.Name = "resultLabel";
        resultLabel.Size = new Size(376, 25);
        resultLabel.TabIndex = 3;
        resultLabel.Text = "";
        //
        // errorProvider
        //
        errorProvider.ContainerControl = this;
        //
        // MainForm
        //
        AcceptButton = convertButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(400, 118);
        Controls.Add(resultLabel);
        Controls.Add(convertButton);
        Controls.Add(celsiusTextBox);
        Controls.Add(celsiusLabel);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Temperature Converter";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label celsiusLabel;
    private TextBox celsiusTextBox;
    private Button convertButton;
    private Label resultLabel;
    private ErrorProvider errorProvider;
}
