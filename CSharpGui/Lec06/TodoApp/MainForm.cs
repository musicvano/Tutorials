using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TodoApp;

public class MainForm : Form
{
    private readonly ITaskStore store;
    private readonly TodoOptions options;
    private readonly ILogger<MainForm> logger;
    private readonly TextBox input = new() { Dock = DockStyle.Top };
    private readonly ListBox list = new() { Dock = DockStyle.Fill };

    public MainForm(ITaskStore store, IOptions<TodoOptions> options,
        ILogger<MainForm> logger)
    {
        this.store = store;
        this.options = options.Value;
        this.logger = logger;

        Text = $"{this.options.Title} (max {this.options.MaxTasks})";
        ClientSize = new Size(360, 220);
        var add = new Button
        {
            Text = "Add",
            Dock = DockStyle.Bottom
        };
        add.Click += (s, e) => AddTask(input.Text.Trim());
        AcceptButton = add;
        Controls.AddRange([list, input, add]);
    }

    public void AddTask(string task)
    {
        if (task.Length == 0) return;
        if (store.Tasks.Count >= options.MaxTasks)
        {
            logger.LogWarning("Limit {Max} reached",
                options.MaxTasks);
            MessageBox.Show(this, "Task limit reached.", Text);
            return;
        }
        store.Add(task);
        list.Items.Add(task);
        input.Clear();
        logger.LogInformation("Task {Task} added, total {Count}",
            task, store.Tasks.Count);
    }
}
