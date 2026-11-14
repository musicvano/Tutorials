using System.Net.Sockets;
using System.Text;

namespace ChatClient;

public class ChatForm : Form
{
    private readonly TextBox hostBox = new() { Text = "127.0.0.1" };
    private readonly TextBox nameBox = new()
    {
        PlaceholderText = "Name"
    };
    private Button connectButton = new()
    {
        Text = "Connect", AutoSize = true
    };
    private ListBox messages = new() { Dock = DockStyle.Fill };
    private TextBox inputBox = new() { Dock = DockStyle.Fill };
    private readonly Button sendButton = new()
    {
        Text = "Send", Dock = DockStyle.Right, Enabled = false
    };
    private TcpClient? client;
    private StreamWriter? writer;
    private CancellationTokenSource? cts;

    public ChatForm()
    {
        Text = "Chat Client";
        AutoScaleDimensions = new SizeF(96, 96); // розміри для 100 %
        AutoScaleMode = AutoScaleMode.Dpi;       // масштаб під DPI
        ClientSize = new Size(460, 360);
        var top = new FlowLayoutPanel
        {
            Dock = DockStyle.Top, AutoSize = true
        };
        top.Controls.AddRange([hostBox, nameBox, connectButton]);
        var bottom = new Panel
        {
            Dock = DockStyle.Bottom, Height = 28
        };
        bottom.Controls.AddRange([inputBox, sendButton]);
        Controls.AddRange([messages, bottom, top]);
        AcceptButton = sendButton;      // Enter надсилає рядок

        connectButton.Click += async (_, _) => await ConnectAsync();
        sendButton.Click += async (_, _) => await SendAsync();
        FormClosing += (_, _) => Disconnect();
    }

    private async Task ConnectAsync()
    {
        if (client != null)
        {
            Disconnect();
            return;
        }
        connectButton.Enabled = false;
        try
        {
            client = new TcpClient();
            await client.ConnectAsync(hostBox.Text, 5055);
            NetworkStream stream = client.GetStream();
            writer = new StreamWriter(stream, new UTF8Encoding(false))
            {
                AutoFlush = true,
                NewLine = "\n"
            };
            await writer.WriteLineAsync(nameBox.Text);
            cts = new CancellationTokenSource();
            SetConnected(true);
            _ = ReceiveAsync(new StreamReader(stream), cts.Token);
        }
        catch (SocketException ex)
        {
            Disconnect();
            messages.Items.Add($"* {ex.Message}");
        }
        connectButton.Enabled = true;
    }

    // await не блокує потік інтерфейсу, а продовження виконується
    // в ньому ж, тому ListBox можна змінювати без Invoke.
    private async Task ReceiveAsync(StreamReader reader,
        CancellationToken token)
    {
        try
        {
            string? line;
            while ((line = await reader.ReadLineAsync(token)) != null)
            {
                messages.Items.Add(line);
                messages.TopIndex = messages.Items.Count - 1;
            }
            messages.Items.Add("* Connection closed by server");
        }
        catch (Exception) when (token.IsCancellationRequested)
        {
            return;                 // користувач сам відключився
        }
        catch (IOException ex)
        {
            messages.Items.Add($"* Connection lost: {ex.Message}");
        }
        Disconnect();
    }

    private async Task SendAsync()
    {
        if (writer is null || inputBox.Text.Length == 0) return;
        try
        {
            await writer.WriteLineAsync(inputBox.Text);
            inputBox.Clear();
        }
        catch (IOException)
        {
            Disconnect();
        }
    }

    private void Disconnect()
    {
        cts?.Cancel();
        client?.Dispose();          // закриває сокет і потік
        client = null;
        writer = null;
        SetConnected(false);
    }

    private void SetConnected(bool connected)
    {
        connectButton.Text = connected ? "Disconnect" : "Connect";
        hostBox.Enabled = nameBox.Enabled = !connected;
        sendButton.Enabled = connected;
    }
}
