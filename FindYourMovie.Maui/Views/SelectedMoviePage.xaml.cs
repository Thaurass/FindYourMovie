using FindYourMovie.EntityLayer.EntityClasses;
using FindYourMovie.Maui.CommandClasses;

namespace FindYourMovie.Maui.Views;

[QueryProperty(nameof(MovieId), "id")]
public partial class SelectedMoviePage : ContentPage
{
	public SelectedMoviePage(SearchViewModelCommands viewModel)
	{
		InitializeComponent();
        ViewModel = viewModel;
        ViewModel.Get(MauiProgram.databasePath, MovieId);
        BindingContext = ViewModel;
	}

    private readonly SearchViewModelCommands ViewModel;
    public int MovieId { get; set; }
}