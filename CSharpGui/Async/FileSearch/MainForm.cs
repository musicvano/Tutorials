namespace FileSearch;

public partial class MainForm : Form
{
    private CancellationTokenSource? cts;

    public MainForm()
    {
        InitializeComponent();
        cancelButton.Enabled = false;
    }

    private async void searchButton_Click(object sender, EventArgs e)
    {
        resultsListBox.Items.Clear();
        progressBar.Value = 0;
        cts = new CancellationTokenSource();
        var progress = new Progress<int>(p => progressBar.Value = p);
        searchButton.Enabled = false;
        cancelButton.Enabled = true;
        statusLabel.Text = "Searching...";
        try
        {
            await foreach (string file in FileSearcher.FindAsync(
                folderTextBox.Text, textTextBox.Text, progress,
                cts.Token))
            {
                resultsListBox.Items.Add(file);
            }
            statusLabel.Text = $"Found: {resultsListBox.Items.Count}";
        }
        catch (OperationCanceledException)
        {
            statusLabel.Text =
                $"Canceled, found: {resultsListBox.Items.Count}";
        }
        catch (Exception ex) when (ex is IOException
            or UnauthorizedAccessException or ArgumentException)
        {
            statusLabel.Text = "Error";
            MessageBox.Show(ex.Message, Text,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            cts.Dispose();
            cts = null;
            searchButton.Enabled = true;
            cancelButton.Enabled = false;
        }
    }

    private void cancelButton_Click(object sender, EventArgs e) =>
        cts?.Cancel();

    private void MainForm_FormClosing(object sender,
        FormClosingEventArgs e) => cts?.Cancel();
}
