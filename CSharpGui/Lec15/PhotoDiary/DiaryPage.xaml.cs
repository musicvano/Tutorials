namespace PhotoDiary;

public partial class DiaryPage : ContentPage
{
    private readonly DiaryViewModel viewModel;

    public DiaryPage(DiaryViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    // The grid is reloaded after returning from the entry page.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadCommand.ExecuteAsync(null);
    }
}
