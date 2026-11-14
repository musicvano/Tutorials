using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Habits;

public class Habit
{
    public string Name { get; set; } = "";
    public List<DateOnly> DoneDays { get; set; } = [];

    // Дні поспіль, що закінчуються сьогодні або вчора.
    public int Streak(DateOnly today)
    {
        DateOnly day = DoneDays.Contains(today)
            ? today : today.AddDays(-1);
        int count = 0;
        while (DoneDays.Contains(day))
        {
            count++;
            day = day.AddDays(-1);
        }
        return count;
    }
}

public record HabitRow(Habit Habit, bool IsDoneToday, int Streak)
{
    public string Name => Habit.Name;
    public string StreakText => Streak switch
    {
        0 => "No streak yet",
        1 => "1 day",
        _ => $"{Streak} days in a row"
    };
    public string ButtonText => IsDoneToday ? "Undo" : "Done";
}

public partial class HabitsViewModel(IPreferences preferences)
    : ObservableObject
{
    private const string Key = "habits";
    private List<Habit> habits = [];

    public ObservableCollection<HabitRow> Rows { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddCommand))]
    public partial string NewName { get; set; } = "";

    public void Load()
    {
        string json = preferences.Get(Key, "[]");
        habits = JsonSerializer.Deserialize<List<Habit>>(json) ?? [];
        Refresh();
    }

    private bool CanAdd() => !string.IsNullOrWhiteSpace(NewName);

    [RelayCommand(CanExecute = nameof(CanAdd))]
    private void Add()
    {
        habits.Add(new Habit { Name = NewName.Trim() });
        NewName = "";
        SaveAndRefresh();
    }

    [RelayCommand]
    private void Toggle(HabitRow row)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        if (!row.Habit.DoneDays.Remove(today))
        {
            row.Habit.DoneDays.Add(today);
        }
        SaveAndRefresh();
    }

    [RelayCommand]
    private void Delete(HabitRow row)
    {
        habits.Remove(row.Habit);
        SaveAndRefresh();
    }

    private void SaveAndRefresh()
    {
        preferences.Set(Key, JsonSerializer.Serialize(habits));
        Refresh();
    }

    private void Refresh()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        Rows.Clear();
        foreach (Habit h in habits.OrderByDescending(
                     h => h.Streak(today)))
        {
            Rows.Add(new HabitRow(h, h.DoneDays.Contains(today),
                h.Streak(today)));
        }
    }
}
