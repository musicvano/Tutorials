using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookClient;

public record Book(int Id, string Title, string Author, int Year);

public partial class BookSearchViewModel(HttpClient http)
    : ObservableObject
{
    public ObservableCollection<Book> Books { get; } = [];

    [ObservableProperty]
    public partial string Query { get; set; } = "";

    [ObservableProperty]
    public partial string Status { get; set; } = "Ready";

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task SearchAsync(CancellationToken token)
    {
        Status = "Loading...";
        try
        {
            string url = "api/books?search="
                + Uri.EscapeDataString(Query.Trim());
            List<Book> found = await http
                .GetFromJsonAsync<List<Book>>(url, token) ?? [];
            Books.Clear();
            foreach (Book book in found)
            {
                Books.Add(book);
            }
            Status = $"Found: {found.Count}";
        }
        catch (OperationCanceledException)
        {
            Status = "Cancelled";
        }
        catch (HttpRequestException ex)
        {
            Status = $"Error: {ex.Message}";
        }
    }
}
