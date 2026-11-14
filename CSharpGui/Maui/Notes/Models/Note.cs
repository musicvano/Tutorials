namespace Notes.Models;

public class Note
{
    // Ім'я файлу *.note.txt; порожнє для нової нотатки.
    public string FileName { get; set; } = "";
    public string Text { get; set; } = "";
    public DateTime Date { get; set; }
}
