namespace Habits;

public partial class HabitsPage : ContentPage
{
    public HabitsPage(HabitsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.Load();
    }
}
