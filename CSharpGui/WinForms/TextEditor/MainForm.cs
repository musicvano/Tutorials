namespace TextEditor;

public partial class MainForm : Form
{
    private string? filePath;     // null – файл ще не збережено
    private bool isModified;

    public MainForm()
    {
        InitializeComponent();
        UpdateStatus();
    }

    private void openMenuItem_Click(object sender, EventArgs e)
    {
        if (!ConfirmDiscard()
            || openFileDialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }
        string path = openFileDialog.FileName;
        editorTextBox.Text = File.ReadAllText(path);
        filePath = path;
        isModified = false;
        UpdateStatus();
    }

    private void saveMenuItem_Click(object sender, EventArgs e) =>
        Save();

    private void exitMenuItem_Click(object sender, EventArgs e) =>
        Close();                  // далі спрацює FormClosing

    private void editorTextBox_TextChanged(object sender, EventArgs e)
    {
        isModified = true;
        UpdateStatus();
    }

    private void MainForm_FormClosing(object sender,
        FormClosingEventArgs e)
    {
        e.Cancel = !ConfirmDiscard();
    }

    // true – можна продовжувати: змін немає, їх збережено
    // або користувач відмовився від них.
    private bool ConfirmDiscard()
    {
        if (!isModified)
        {
            return true;
        }
        DialogResult answer = MessageBox.Show(
            "Save changes to the document?", "Text Editor",
            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        return answer == DialogResult.No
            || answer == DialogResult.Yes && Save();
    }

    private bool Save()
    {
        if (filePath is null)
        {
            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return false;
            }
            filePath = saveFileDialog.FileName;
        }
        File.WriteAllText(filePath, editorTextBox.Text);
        isModified = false;
        UpdateStatus();
        return true;
    }

    private void UpdateStatus()
    {
        string name = Path.GetFileName(filePath) ?? "Untitled";
        Text = $"{name}{(isModified ? "*" : "")} – Text Editor";
        statusLabel.Text = $"Lines: {editorTextBox.Lines.Length}"
            + $"   Characters: {editorTextBox.TextLength}";
    }
}
