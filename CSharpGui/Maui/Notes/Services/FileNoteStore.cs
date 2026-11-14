using Notes.Models;

namespace Notes.Services;

// Кожна нотатка - окремий файл *.note.txt у папці folder.
public class FileNoteStore(string folder) : INoteStore
{
    private const string Pattern = "*.note.txt";

    public async Task<List<Note>> LoadAllAsync()
    {
        var notes = new List<Note>();
        foreach (string path in Directory.EnumerateFiles(folder, Pattern))
        {
            notes.Add(new Note
            {
                FileName = Path.GetFileName(path),
                Text = await File.ReadAllTextAsync(path),
                Date = File.GetLastWriteTime(path)
            });
        }
        return notes.OrderByDescending(n => n.Date).ToList();
    }

    public async Task SaveAsync(Note note)
    {
        if (string.IsNullOrEmpty(note.FileName))
        {
            note.FileName = $"{Guid.NewGuid()}.note.txt";
        }
        string path = Path.Combine(folder, note.FileName);
        await File.WriteAllTextAsync(path, note.Text);
        note.Date = File.GetLastWriteTime(path);
    }

    public void Delete(Note note)
    {
        if (string.IsNullOrEmpty(note.FileName))
        {
            return;                 // нова нотатка ще не збережена
        }
        File.Delete(Path.Combine(folder, note.FileName));
    }
}
