using FindYourMovie.EntityLayer.EntityClasses;
using FindYourMovie.Maui.CommandClasses;

namespace FindYourMovie.Maui.Views;

public partial class SelectedMoviePage : ContentPage
{
	public SelectedMoviePage(SearchViewModelCommands viewModel)
	{
		InitializeComponent();
        ViewModel = viewModel;
        BindingContext = ViewModel;
	}

    private readonly SearchViewModelCommands ViewModel;
}