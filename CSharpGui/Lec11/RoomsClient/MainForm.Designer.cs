namespace RoomsClient;

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
        topPanel = new FlowLayoutPanel();
        nameLabel = new Label();
        nameTextBox = new TextBox();
        connectButton = new Button();
        roomComboBox = new ComboBox();
        joinButton = new Button();
        messagesListBox = new ListBox();
        bottomPanel = new TableLayoutPanel();
        messageTextBox = new TextBox();
        sendButton = new Button();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        topPanel.SuspendLayout();
        bottomPanel.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        //
        // topPanel
        //
        topPanel.AutoSize = true;
        topPanel.Controls.Add(nameLabel);
        topPanel.Controls.Add(nameTextBox);
        topPanel.Controls.Add(connectButton);
        topPanel.Controls.Add(roomComboBox);
        topPanel.Controls.Add(joinButton);
        topPanel.Dock = DockStyle.Top;
        topPanel.Location = new Point(0, 0);
        topPanel.Name = "topPanel";
        topPanel.Padding = new Padding(3);
        topPanel.Size = new Size(460, 37);
        topPanel.TabIndex = 0;
        topPanel.WrapContents = false;
        //
        // nameLabel
        //
        nameLabel.Anchor = AnchorStyles.Left;
        nameLabel.AutoSize = true;
        nameLabel.Location = new Point(6, 10);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new Size(42, 15);
        nameLabel.TabIndex = 0;
        nameLabel.Text = "Name:";
        //
        // nameTextBox
        //
        nameTextBox.Location = new Point(54, 6);
        nameTextBox.Name = "nameTextBox";
        nameTextBox.Size = new Size(100, 23);
        nameTextBox.TabIndex = 1;
        //
        // connectButton
        //
        connectButton.Location = new Point(160, 6);
        connectButton.Name = "connectButton";
        connectButton.Size = new Size(75, 23);
        connectButton.TabIndex = 2;
        connectButton.Text = "Connect";
        connectButton.UseVisualStyleBackColor = true;
        connectButton.Click += connectButton_Click;
        //
        // roomComboBox
        //
        roomComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        roomComboBox.Location = new Point(241, 6);
        roomComboBox.Name = "roomComboBox";
        roomComboBox.Size = new Size(110, 23);
        roomComboBox.TabIndex = 3;
        //
        // joinButton
        //
        joinButton.Location = new Point(357, 6);
        joinButton.Name = "joinButton";
        joinButton.Size = new Size(75, 23);
        joinButton.TabIndex = 4;
        joinButton.Text = "Join";
        joinButton.UseVisualStyleBackColor = true;
        joinButton.Click += joinButton_Click;
        //
        // messagesListBox
        //
        messagesListBox.Dock = DockStyle.Fill;
        messagesListBox.IntegralHeight = false;
        messagesListBox.Location = new Point(0, 37);
        messagesListBox.Name = "messagesListBox";
        messagesListBox.Size = new Size(460, 202);
        messagesListBox.TabIndex = 1;
        //
        // bottomPanel
        //
        bottomPanel.AutoSize = true;
        bottomPanel.ColumnCount = 2;
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        bottomPanel.ColumnStyles.Add(new ColumnStyle());
        bottomPanel.Controls.Add(messageTextBox, 0, 0);
        bottomPanel.Controls.Add(sendButton, 1, 0);
        bottomPanel.Dock = DockStyle.Bottom;
        bottomPanel.Location = new Point(0, 239);
        bottomPanel.Name = "bottomPanel";
        bottomPanel.Padding = new Padding(3);
        bottomPanel.RowCount = 1;
        bottomPanel.RowStyles.Add(new RowStyle());
        bottomPanel.Size = new Size(460, 39);
        bottomPanel.TabIndex = 2;
        //
        // messageTextBox
        //
        messageTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        messageTextBox.Location = new Point(6, 8);
        messageTextBox.Name = "messageTextBox";
        messageTextBox.Size = new Size(367, 23);
        messageTextBox.TabIndex = 0;
        //
        // sendButton
        //
        sendButton.Location = new Point(379, 6);
        sendButton.Name = "sendButton";
        sendButton.Size = new Size(75, 27);
        sendButton.TabIndex = 1;
        sendButton.Text = "Send";
        sendButton.UseVisualStyleBackColor = true;
        sendButton.Click += sendButton_Click;
        //
        // statusStrip
        //
        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
        statusStrip.Location = new Point(0, 278);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(460, 22);
        statusStrip.TabIndex = 3;
        //
        // statusLabel
        //
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(39, 17);
        statusLabel.Text = "Offline";
        //
        // MainForm
        //
        AcceptButton = sendButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(460, 300);
        Controls.Add(messagesListBox);
        Controls.Add(bottomPanel);
        Controls.Add(topPanel);
        Controls.Add(statusStrip);
        MinimumSize = new Size(476, 250);
        Name = "MainForm";
        StartPosition = FormStartPosition.WindowsDefaultLocation;
        Text = "SignalR Rooms";
        FormClosing += MainForm_FormClosing;
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        bottomPanel.ResumeLayout(false);
        bottomPanel.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private FlowLayoutPanel topPanel;
    private Label nameLabel;
    private TextBox nameTextBox;
    private Button connectButton;
    private ComboBox roomComboBox;
    private Button joinButton;
    private ListBox messagesListBox;
    private TableLayoutPanel bottomPanel;
    private TextBox messageTextBox;
    private Button sendButton;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
}
