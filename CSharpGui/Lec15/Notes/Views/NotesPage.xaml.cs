using Notes.ViewModels;

namespace Notes.Views;

public partial class NotesPage : ContentPage
{
    private readonly NotesViewModel viewModel;

    public NotesPage(NotesViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    // Список оновлюється і після повернення зі сторінки нотатки.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadCommand.ExecuteAsync(null);
    }
}
