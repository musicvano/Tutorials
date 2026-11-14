using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace FileManager;

// Рядок таблиці файлів: значення вже відформатовано.
public record FileRow(string Name, string Size, string Modified);

public partial class MainWindow : Window
{
    private const string Placeholder = "...";  // «ще не завантажено»

    public MainWindow()
    {
        InitializeComponent();
        OpenFolder(Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments));
    }

    public void OpenFolder(string path)
    {
        folderTree.Items.Clear();
        var root = CreateItem(new DirectoryInfo(path));
        folderTree.Items.Add(root);
        root.IsExpanded = true;       // генерує подію Expanded
        root.IsSelected = true;
    }

    private static TreeViewItem CreateItem(DirectoryInfo dir)
    {
        var item = new TreeViewItem { Header = dir.Name, Tag = dir };
        item.Items.Add(Placeholder);   // з’явиться стрілка
        return item;
    }

    // Подія Expanded спливає від вузла TreeViewItem до TreeView.
    private void Folder_Expanded(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is not TreeViewItem
            { Tag: DirectoryInfo dir } item
            || item.Items.Count != 1
            || item.Items[0] as string != Placeholder)
        {
            return;                   // уже завантажено
        }
        item.Items.Clear();
        try
        {
            foreach (DirectoryInfo sub in dir.EnumerateDirectories())
            {
                item.Items.Add(CreateItem(sub));
            }
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException
            or IOException)
        {
            statusItem.Content = ex.Message;
        }
    }

    private void Folder_SelectedItemChanged(object sender,
        RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is not TreeViewItem { Tag: DirectoryInfo dir })
        {
            return;
        }
        try
        {
            FileInfo[] files = dir.GetFiles();
            fileList.ItemsSource = files
                .OrderBy(f => f.Name)
                .Select(f => new FileRow(f.Name,
                    $"{Math.Ceiling(f.Length / 1024.0):N0}",
                    $"{f.LastWriteTime:g}"))
                .ToList();
            long total = files.Sum(f => f.Length);
            statusItem.Content = $"{dir.Name}   Files: {files.Length}"
                + $"   Total: {total / 1024.0:N1} KB";
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException
            or IOException)
        {
            fileList.ItemsSource = null;
            statusItem.Content = ex.Message;
        }
    }

    private void Open_Executed(object sender,
        ExecutedRoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Choose a folder"
        };
        if (dialog.ShowDialog(this) == true)
        {
            OpenFolder(dialog.FolderName);
        }
    }

    private void Exit_Click(object sender, RoutedEventArgs e) =>
        Close();
}
