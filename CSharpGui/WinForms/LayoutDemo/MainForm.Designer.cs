namespace LayoutDemo;

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
        tableLayoutPanel = new TableLayoutPanel();
        nameLabel = new Label();
        nameTextBox = new TextBox();
        emailLabel = new Label();
        emailTextBox = new TextBox();
        trackLabel = new Label();
        trackComboBox = new ComboBox();
        tableLayoutPanel.SuspendLayout();
        SuspendLayout();
        //
        // tableLayoutPanel
        //
        tableLayoutPanel.ColumnCount = 2;
        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel.Controls.Add(nameLabel, 0, 0);
        tableLayoutPanel.Controls.Add(nameTextBox, 1, 0);
        tableLayoutPanel.Controls.Add(emailLabel, 0, 1);
        tableLayoutPanel.Controls.Add(emailTextBox, 1, 1);
        tableLayoutPanel.Controls.Add(trackLabel, 0, 2);
        tableLayoutPanel.Controls.Add(trackComboBox, 1, 2);
        tableLayoutPanel.Dock = DockStyle.Fill;
        tableLayoutPanel.Location = new Point(0, 0);
        tableLayoutPanel.Name = "tableLayoutPanel";
        tableLayoutPanel.Padding = new Padding(10);
        tableLayoutPanel.RowCount = 4;
        tableLayoutPanel.RowStyles.Add(new RowStyle());
        tableLayoutPanel.RowStyles.Add(new RowStyle());
        tableLayoutPanel.RowStyles.Add(new RowStyle());
        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel.Size = new Size(400, 150);
        tableLayoutPanel.TabIndex = 0;
        //
        // nameLabel
        //
        nameLabel.Anchor = AnchorStyles.Left;
        nameLabel.AutoSize = true;
        nameLabel.Location = new Point(13, 17);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new Size(42, 15);
        nameLabel.TabIndex = 0;
        nameLabel.Text = "&Name:";
        //
        // nameTextBox
        //
        nameTextBox.Dock = DockStyle.Fill;
        nameTextBox.Location = new Point(61, 13);
        nameTextBox.Name = "nameTextBox";
        nameTextBox.Size = new Size(326, 23);
        nameTextBox.TabIndex = 1;
        //
        // emailLabel
        //
        emailLabel.Anchor = AnchorStyles.Left;
        emailLabel.AutoSize = true;
        emailLabel.Location = new Point(13, 46);
        emailLabel.Name = "emailLabel";
        emailLabel.Size = new Size(39, 15);
        emailLabel.TabIndex = 2;
        emailLabel.Text = "&Email:";
        //
        // emailTextBox
        //
        emailTextBox.Dock = DockStyle.Fill;
        emailTextBox.Location = new Point(61, 42);
        emailTextBox.Name = "emailTextBox";
        emailTextBox.Size = new Size(326, 23);
        emailTextBox.TabIndex = 3;
        //
        // trackLabel
        //
        trackLabel.Anchor = AnchorStyles.Left;
        trackLabel.AutoSize = true;
        trackLabel.Location = new Point(13, 75);
        trackLabel.Name = "trackLabel";
        trackLabel.Size = new Size(37, 15);
        trackLabel.TabIndex = 4;
        trackLabel.Text = "&Track:";
        //
        // trackComboBox
        //
        trackComboBox.Dock = DockStyle.Fill;
        trackComboBox.FormattingEnabled = true;
        trackComboBox.Location = new Point(61, 71);
        trackComboBox.Name = "trackComboBox";
        trackComboBox.Size = new Size(326, 23);
        trackComboBox.TabIndex = 5;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(400, 150);
        Controls.Add(tableLayoutPanel);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Layout";
        tableLayoutPanel.ResumeLayout(false);
        tableLayoutPanel.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel;
    private Label nameLabel;
    private TextBox nameTextBox;
    private Label emailLabel;
    private TextBox emailTextBox;
    private Label trackLabel;
    private ComboBox trackComboBox;
}
