using System.ComponentModel.DataAnnotations;

namespace TodoApi;

public enum Priority { Low, Normal, High }

// Сутність, яку зберігає сховище.
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public Priority Priority { get; set; }
    public DateOnly? DueDate { get; set; }
    public bool IsDone { get; set; }
}

// DTO: що клієнт надсилає під час створення справи.
public record TodoCreateDto(
    [Required, StringLength(100, MinimumLength = 3,
        ErrorMessage = "Назва має містити від 3 до 100 символів.")]
    string Title,
    Priority Priority = Priority.Normal,
    DateOnly? DueDate = null);

// DTO для повної заміни справи (PUT).
public record TodoUpdateDto(
    [Required, StringLength(100, MinimumLength = 3,
        ErrorMessage = "Назва має містити від 3 до 100 символів.")]
    string Title,
    Priority Priority,
    DateOnly? DueDate,
    bool IsDone);

public record PagedResult<T>(IReadOnlyList<T> Items,
    int Page, int PageSize, int TotalCount);
