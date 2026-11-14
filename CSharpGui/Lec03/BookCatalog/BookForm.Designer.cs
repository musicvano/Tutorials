namespace BookCatalog;

partial class BookForm
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
        titleLabel = new Label();
        titleTextBox = new TextBox();
        authorLabel = new Label();
        authorTextBox = new TextBox();
        yearLabel = new Label();
        yearNumeric = new NumericUpDown();
        priceLabel = new Label();
        priceNumeric = new NumericUpDown();
        okButton = new Button();
        cancelButton = new Button();
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)yearNumeric).BeginInit();
        ((System.ComponentModel.ISupportInitialize)priceNumeric).BeginInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        //
        // titleLabel
        //
        titleLabel.AutoSize = true;
        titleLabel.Location = new Point(12, 15);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(32, 15);
        titleLabel.TabIndex = 0;
        titleLabel.Text = "&Title:";
        //
        // titleTextBox
        //
        titleTextBox.Location = new Point(80, 12);
        titleTextBox.Name = "titleTextBox";
        titleTextBox.Size = new Size(260, 23);
        titleTextBox.TabIndex = 1;
        //
        // authorLabel
        //
        authorLabel.AutoSize = true;
        authorLabel.Location = new Point(12, 44);
        authorLabel.Name = "authorLabel";
        authorLabel.Size = new Size(47, 15);
        authorLabel.TabIndex = 2;
        authorLabel.Text = "&Author:";
        //
        // authorTextBox
        //
        authorTextBox.Location = new Point(80, 41);
        authorTextBox.Name = "authorTextBox";
        authorTextBox.Size = new Size(260, 23);
        authorTextBox.TabIndex = 3;
        //
        // yearLabel
        //
        yearLabel.AutoSize = true;
        yearLabel.Location = new Point(12, 73);
        yearLabel.Name = "yearLabel";
        yearLabel.Size = new Size(32, 15);
        yearLabel.TabIndex = 4;
        yearLabel.Text = "&Year:";
        //
        // yearNumeric
        //
        yearNumeric.Location = new Point(80, 70);
        yearNumeric.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
        yearNumeric.Minimum = new decimal(new int[] { 1450, 0, 0, 0 });
        yearNumeric.Name = "yearNumeric";
        yearNumeric.Size = new Size(100, 23);
        yearNumeric.TabIndex = 5;
        yearNumeric.Value = new decimal(new int[] { 2026, 0, 0, 0 });
        //
        // priceLabel
        //
        priceLabel.AutoSize = true;
        priceLabel.Location = new Point(12, 102);
        priceLabel.Name = "priceLabel";
        priceLabel.Size = new Size(36, 15);
        priceLabel.TabIndex = 6;
        priceLabel.Text = "&Price:";
        //
        // priceNumeric
        //
        priceNumeric.DecimalPlaces = 2;
        priceNumeric.Location = new Point(80, 99);
        priceNumeric.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        priceNumeric.Name = "priceNumeric";
        priceNumeric.Size = new Size(100, 23);
        priceNumeric.TabIndex = 7;
        priceNumeric.ThousandsSeparator = true;
        //
        // okButton
        //
        okButton.Location = new Point(184, 138);
        okButton.Name = "okButton";
        okButton.Size = new Size(75, 27);
        okButton.TabIndex = 8;
        okButton.Text = "OK";
        okButton.UseVisualStyleBackColor = true;
        okButton.Click += okButton_Click;
        //
        // cancelButton
        //
        cancelButton.CausesValidation = false;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new Point(265, 138);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(75, 27);
        cancelButton.TabIndex = 9;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        //
        // errorProvider
        //
        errorProvider.ContainerControl = this;
        //
        // BookForm
        //
        AcceptButton = okButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(370, 177);
        Controls.Add(cancelButton);
        Controls.Add(okButton);
        Controls.Add(priceNumeric);
        Controls.Add(priceLabel);
        Controls.Add(yearNumeric);
        Controls.Add(yearLabel);
        Controls.Add(authorTextBox);
        Controls.Add(authorLabel);
        Controls.Add(titleTextBox);
        Controls.Add(titleLabel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "BookForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Book";
        ((System.ComponentModel.ISupportInitialize)yearNumeric).EndInit();
        ((System.ComponentModel.ISupportInitialize)priceNumeric).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label titleLabel;
    private TextBox titleTextBox;
    private Label authorLabel;
    private TextBox authorTextBox;
    private Label yearLabel;
    private NumericUpDown yearNumeric;
    private Label priceLabel;
    private NumericUpDown priceNumeric;
    private Button okButton;
    private Button cancelButton;
    private ErrorProvider errorProvider;
}
