using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Schedule;

public record Lesson(DayOfWeek Day, TimeOnly Start,
    string Subject, int Room);

public partial class ScheduleViewModel : ObservableValidator
{
    public DayOfWeek[] Days { get; } =
    [
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
        DayOfWeek.Thursday, DayOfWeek.Friday
    ];

    public ObservableCollection<Lesson> Lessons { get; } = [];

    [ObservableProperty]
    public partial DayOfWeek Day { get; set; } = DayOfWeek.Monday;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(AddCommand))]
    [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$",
        ErrorMessage = "Use the HH:mm format, e.g. 08:30")]
    public partial string Start { get; set; } = "08:30";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(AddCommand))]
    [Required(ErrorMessage = "Enter a subject")]
    [MinLength(3, ErrorMessage = "At least 3 characters")]
    public partial string Subject { get; set; } = "";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(AddCommand))]
    [Range(1, 999, ErrorMessage = "Room must be from 1 to 999")]
    public partial int Room { get; set; } = 101;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RemoveCommand))]
    public partial Lesson? SelectedLesson { get; set; }

    [ObservableProperty]
    public partial string Message { get; set; } = "";

    [RelayCommand(CanExecute = nameof(CanAdd))]
    private void Add()
    {
        ValidateAllProperties();   // перевірити всі властивості
        if (HasErrors)
        {
            return;
        }
        var start = TimeOnly.ParseExact(Start, "HH:mm",
            CultureInfo.InvariantCulture);
        if (Lessons.Any(l => l.Day == Day && l.Start == start))
        {
            Message = $"{Day} {Start} is already taken";
            return;
        }
        var lesson = new Lesson(Day, start, Subject.Trim(), Room);
        // Вставити так, щоб розклад лишався впорядкованим.
        int index = Lessons.TakeWhile(l => l.Day < Day
            || l.Day == Day && l.Start < start).Count();
        Lessons.Insert(index, lesson);
        Message = $"Lessons: {Lessons.Count}";
        Subject = "";
        ClearErrors(nameof(Subject));   // порожнє поле без рамки
    }

    private bool CanAdd() =>
        !HasErrors && !string.IsNullOrWhiteSpace(Subject);

    [RelayCommand(CanExecute = nameof(CanRemove))]
    private void Remove()
    {
        Lessons.Remove(SelectedLesson!);
        Message = $"Lessons: {Lessons.Count}";
    }

    private bool CanRemove() => SelectedLesson is not null;
}
