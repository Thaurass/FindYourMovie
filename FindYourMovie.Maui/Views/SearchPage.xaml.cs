using FindYourMovie.Maui.CommandClasses;

namespace FindYourMovie.Maui.Views;

public partial class SearchPage : ContentPage 
{
	public SearchPage(SearchViewModelCommands viewModel)
	{
        InitializeComponent();

        ViewModel = viewModel;
        BindingContext = ViewModel;
         
    }

    private readonly SearchViewModelCommands ViewModel;
}