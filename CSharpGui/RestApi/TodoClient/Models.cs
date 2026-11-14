namespace TodoClient;

public enum Priority { Low, Normal, High }

public record TodoItem(int Id, string Title, Priority Priority,
    DateOnly? DueDate, bool IsDone);

public record TodoCreateDto(string Title,
    Priority Priority = Priority.Normal, DateOnly? DueDate = null);

public record PagedResult<T>(List<T> Items, int Page,
    int PageSize, int TotalCount);

// Тіло відповіді з помилкою у форматі ProblemDetails.
public record ApiProblem(string? Title,
    Dictionary<string, string[]>? Errors);
