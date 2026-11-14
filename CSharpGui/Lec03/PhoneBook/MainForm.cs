using System.ComponentModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PhoneBook;

public partial class MainForm : Form
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        // «+» і кирилиця у файлі без послідовностей \uXXXX.
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    private readonly string dataFile = Path.Combine(
        Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData),
        "PhoneBook", "contacts.json");
    private List<Contact> contacts = [];

    public MainForm()
    {
        InitializeComponent();
    }

    [GeneratedRegex(@"^\+?[0-9 ()-]{7,20}$")]
    private static partial Regex PhoneRegex();

    private void MainForm_Load(object sender, EventArgs e)
    {
        try
        {
            if (File.Exists(dataFile))
            {
                string json = File.ReadAllText(dataFile);
                contacts = JsonSerializer
                    .Deserialize<List<Contact>>(json) ?? [];
            }
        }
        catch (Exception ex) when (ex is IOException
            or JsonException)
        {
            MessageBox.Show($"Cannot read contacts: {ex.Message}",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        ShowContacts();
    }

    private void MainForm_FormClosed(object sender,
        FormClosedEventArgs e)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(dataFile)!);
        File.WriteAllText(dataFile,
            JsonSerializer.Serialize(contacts, JsonOptions));
    }

    private void searchTextBox_TextChanged(object sender,
        EventArgs e) => ShowContacts();

    private void ShowContacts()
    {
        string filter = searchTextBox.Text.Trim();
        contactsListBox.BeginUpdate();
        contactsListBox.Items.Clear();
        foreach (Contact c in contacts.OrderBy(c => c.Name))
        {
            if (c.Name.Contains(filter,
                    StringComparison.CurrentCultureIgnoreCase)
                || c.Phone.Contains(filter))
            {
                contactsListBox.Items.Add(c);
            }
        }
        contactsListBox.EndUpdate();
        int shown = contactsListBox.Items.Count;
        countLabel.Text = $"Shown: {shown} of {contacts.Count}";
    }

    private void addButton_Click(object sender, EventArgs e)
    {
        string name = nameTextBox.Text.Trim();
        string phone = phoneTextBox.Text.Trim();
        bool nameOk = Check(nameTextBox, name.Length > 0,
            "Enter a name");
        bool phoneOk = Check(phoneTextBox,
            PhoneRegex().IsMatch(phone),
            "Use digits, spaces, ( ) and -, e.g. +380 67 123 4567");
        if (!nameOk || !phoneOk)
        {
            return;
        }
        contacts.Add(new Contact(name, phone));
        nameTextBox.Clear();
        phoneTextBox.Clear();
        nameTextBox.Focus();
        ShowContacts();
    }

    private bool Check(Control control, bool isValid, string message)
    {
        errorProvider.SetError(control, isValid ? "" : message);
        return isValid;
    }

    private void contactsListBox_MouseDown(object sender,
        MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            // Правий клік виділяє контакт під курсором.
            contactsListBox.SelectedIndex =
                contactsListBox.IndexFromPoint(e.Location);
        }
    }

    private void contactMenu_Opening(object sender,
        CancelEventArgs e) =>
        e.Cancel = contactsListBox.SelectedItem is null;

    private void deleteMenuItem_Click(object sender, EventArgs e)
    {
        if (contactsListBox.SelectedItem is Contact contact)
        {
            contacts.Remove(contact);
            ShowContacts();
        }
    }
}

public record Contact(string Name, string Phone)
{
    // ListBox показує елемент за допомогою ToString().
    public override string ToString() => $"{Name}   {Phone}";
}
