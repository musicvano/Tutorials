namespace PhoneBook;

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
        splitContainer = new SplitContainer();
        contactsListBox = new ListBox();
        contactMenu = new ContextMenuStrip(components);
        deleteMenuItem = new ToolStripMenuItem();
        countLabel = new Label();
        searchTextBox = new TextBox();
        nameLabel = new Label();
        nameTextBox = new TextBox();
        phoneLabel = new Label();
        phoneTextBox = new TextBox();
        addButton = new Button();
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        contactMenu.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        //
        // splitContainer
        //
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Location = new Point(0, 0);
        splitContainer.Name = "splitContainer";
        //
        // splitContainer.Panel1
        //
        splitContainer.Panel1.Controls.Add(contactsListBox);
        splitContainer.Panel1.Controls.Add(countLabel);
        splitContainer.Panel1.Controls.Add(searchTextBox);
        //
        // splitContainer.Panel2
        //
        splitContainer.Panel2.Controls.Add(addButton);
        splitContainer.Panel2.Controls.Add(phoneTextBox);
        splitContainer.Panel2.Controls.Add(phoneLabel);
        splitContainer.Panel2.Controls.Add(nameTextBox);
        splitContainer.Panel2.Controls.Add(nameLabel);
        splitContainer.Size = new Size(560, 260);
        splitContainer.SplitterDistance = 290;
        splitContainer.TabIndex = 0;
        //
        // contactsListBox
        //
        contactsListBox.ContextMenuStrip = contactMenu;
        contactsListBox.Dock = DockStyle.Fill;
        contactsListBox.IntegralHeight = false;
        contactsListBox.Location = new Point(0, 23);
        contactsListBox.Name = "contactsListBox";
        contactsListBox.Size = new Size(290, 214);
        contactsListBox.TabIndex = 1;
        contactsListBox.MouseDown += contactsListBox_MouseDown;
        //
        // contactMenu
        //
        contactMenu.Items.AddRange(new ToolStripItem[] { deleteMenuItem });
        contactMenu.Name = "contactMenu";
        contactMenu.Size = new Size(108, 26);
        contactMenu.Opening += contactMenu_Opening;
        //
        // deleteMenuItem
        //
        deleteMenuItem.Name = "deleteMenuItem";
        deleteMenuItem.Size = new Size(107, 22);
        deleteMenuItem.Text = "&Delete";
        deleteMenuItem.Click += deleteMenuItem_Click;
        //
        // countLabel
        //
        countLabel.Dock = DockStyle.Bottom;
        countLabel.Location = new Point(0, 237);
        countLabel.Name = "countLabel";
        countLabel.Size = new Size(290, 23);
        countLabel.TabIndex = 2;
        countLabel.Text = "Shown: 0 of 0";
        countLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // searchTextBox
        //
        searchTextBox.Dock = DockStyle.Top;
        searchTextBox.Location = new Point(0, 0);
        searchTextBox.Name = "searchTextBox";
        searchTextBox.PlaceholderText = "Search by name or phone";
        searchTextBox.Size = new Size(290, 23);
        searchTextBox.TabIndex = 0;
        searchTextBox.TextChanged += searchTextBox_TextChanged;
        //
        // nameLabel
        //
        nameLabel.AutoSize = true;
        nameLabel.Location = new Point(12, 12);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new Size(42, 15);
        nameLabel.TabIndex = 0;
        nameLabel.Text = "&Name:";
        //
        // nameTextBox
        //
        nameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        nameTextBox.Location = new Point(12, 30);
        nameTextBox.Name = "nameTextBox";
        nameTextBox.Size = new Size(212, 23);
        nameTextBox.TabIndex = 1;
        //
        // phoneLabel
        //
        phoneLabel.AutoSize = true;
        phoneLabel.Location = new Point(12, 62);
        phoneLabel.Name = "phoneLabel";
        phoneLabel.Size = new Size(44, 15);
        phoneLabel.TabIndex = 2;
        phoneLabel.Text = "&Phone:";
        //
        // phoneTextBox
        //
        phoneTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        phoneTextBox.Location = new Point(12, 80);
        phoneTextBox.Name = "phoneTextBox";
        phoneTextBox.Size = new Size(212, 23);
        phoneTextBox.TabIndex = 3;
        //
        // addButton
        //
        addButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        addButton.Location = new Point(134, 115);
        addButton.Name = "addButton";
        addButton.Size = new Size(90, 27);
        addButton.TabIndex = 4;
        addButton.Text = "&Add";
        addButton.UseVisualStyleBackColor = true;
        addButton.Click += addButton_Click;
        //
        // errorProvider
        //
        errorProvider.ContainerControl = this;
        //
        // MainForm
        //
        AcceptButton = addButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(560, 260);
        Controls.Add(splitContainer);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Phone Book";
        FormClosed += MainForm_FormClosed;
        Load += MainForm_Load;
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel1.PerformLayout();
        splitContainer.Panel2.ResumeLayout(false);
        splitContainer.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        contactMenu.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer;
    private ListBox contactsListBox;
    private ContextMenuStrip contactMenu;
    private ToolStripMenuItem deleteMenuItem;
    private Label countLabel;
    private TextBox searchTextBox;
    private Label nameLabel;
    private TextBox nameTextBox;
    private Label phoneLabel;
    private TextBox phoneTextBox;
    private Button addButton;
    private ErrorProvider errorProvider;
}
