using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace RoomsClient;

public partial class MainForm : Form
{
    private const string HubUrl = "http://localhost:5110/hubs/rooms";
    private HubConnection? connection;
    private string? room;             // кімната, до якої приєдналися

    public MainForm()
    {
        InitializeComponent();
        roomComboBox.Items.AddRange("General", "Games", "Music");
        roomComboBox.SelectedIndex = 0;
        SetOnline(false, "Offline");
    }

    private async void connectButton_Click(object sender, EventArgs e)
    {
        string name = nameTextBox.Text.Trim();
        if (name.Length == 0)
        {
            MessageBox.Show("Enter your name", Text);
            return;
        }
        connection = new HubConnectionBuilder()
            .WithUrl($"{HubUrl}?user={Uri.EscapeDataString(name)}")
            .WithAutomaticReconnect()
            .Build();
        // Обробники On і події підключення викликаються в потоках
        // пулу, тому елементи форми змінюються через InvokeAsync.
        connection.On<string, string>("ReceiveMessage",
            (user, text) => AddLine($"{user}: {text}"));
        connection.On<string>("Notify", text => AddLine($"* {text}"));
        connection.Reconnecting += error =>
            InvokeAsync(() => SetOnline(false, "Reconnecting..."));
        connection.Reconnected += async connectionId =>
        {
            await InvokeAsync(() => SetOnline(true, "Online"));
            await JoinRoomAsync();    // нове підключення – без груп
        };
        connection.Closed += error =>
            InvokeAsync(() => SetOnline(false, "Offline"));
        try
        {
            connectButton.Enabled = nameTextBox.Enabled = false;
            await connection.StartAsync();
            SetOnline(true, "Online");   // після await – потік UI
        }
        catch (HttpRequestException ex)
        {
            connection = null;
            connectButton.Enabled = nameTextBox.Enabled = true;
            MessageBox.Show(ex.Message, Text);
        }
    }

    private async void joinButton_Click(object sender, EventArgs e)
    {
        room = (string)roomComboBox.SelectedItem!;
        await JoinRoomAsync();
        await AddLine($"* You are in {room}");
    }

    private async void sendButton_Click(object sender, EventArgs e)
    {
        if (connection is null || messageTextBox.Text.Length == 0)
        {
            return;
        }
        try
        {
            await connection.InvokeAsync("SendToRoom",
                messageTextBox.Text);
            messageTextBox.Clear();
        }
        catch (HubException ex)   // помилка, передана хабом
        {
            MessageBox.Show(ex.Message, Text);
        }
    }

    private async void MainForm_FormClosing(object sender,
        FormClosingEventArgs e)
    {
        if (connection is null)
        {
            return;
        }
        e.Cancel = true;             // спочатку закрити підключення
        HubConnection current = connection;
        connection = null;
        await current.DisposeAsync();
        Close();
    }

    private async Task JoinRoomAsync()
    {
        if (connection is not null && room is not null)
        {
            await connection.InvokeAsync("JoinRoom", room);
        }
    }

    private Task AddLine(string line) => InvokeAsync(() =>
    {
        messagesListBox.Items.Add(line);
        messagesListBox.TopIndex = messagesListBox.Items.Count - 1;
    });

    private void SetOnline(bool online, string status)
    {
        joinButton.Enabled = sendButton.Enabled = online;
        statusLabel.Text = status;
    }
}
