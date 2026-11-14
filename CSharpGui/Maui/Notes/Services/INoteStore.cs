using Notes.Models;

namespace Notes.Services;

public interface INoteStore
{
    Task<List<Note>> LoadAllAsync();
    Task SaveAsync(Note note);
    void Delete(Note note);
}
