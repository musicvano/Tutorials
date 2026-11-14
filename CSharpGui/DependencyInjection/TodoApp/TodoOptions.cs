namespace TodoApp;

// Параметри з розділу Todo файла appsettings.json.
public class TodoOptions
{
    public string Title { get; set; } = "Tasks";
    public int MaxTasks { get; set; } = 10;
}
