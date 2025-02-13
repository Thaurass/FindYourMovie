﻿using Common.Library.Interfaces;
using FindYourMovie.EntityLayer.EntityClasses;
using FindYourMovie.ViewModelLayer.ViewModelClasses;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace FindYourMovie.Maui.CommandClasses;

public class SearchViewModelCommands : SearchViewModel
{
    #region Constructors
    public SearchViewModelCommands()
    {
    }

    public SearchViewModelCommands(IRepository<Movie> movierepo, IRepository<Actor> actorrepo) : base(movierepo, actorrepo)
    {
        _MovieRepository = movierepo;
        _ActorRepository = actorrepo;
        ActorList = new ObservableCollection<Actor>(_ActorRepository.Get(MauiProgram.databasePath));
    }
    #endregion

    #region Private Variables
    private IRepository<Movie>? _MovieRepository;
    private IRepository<Actor>? _ActorRepository;
    #endregion

    #region Commands
    public ICommand SelectMovieCommand { get; private set; }
    public ICommand SearchMovieCommand { get; private set; }
    #endregion

    #region Init Method
    public override void Init()
    {
        base.Init();
        Routing.RegisterRoute("SelectedMoviePage", typeof(Views.SelectedMoviePage));
        SelectMovieCommand = new Command(async () => await SelectMovie());
        SearchMovieCommand = new Command(() => MovieSearch());
    }
    #endregion

    #region MovieSearch Method
    protected void MovieSearch()
    {
        SearchMovie(MauiProgram.databasePath);

    }
    #endregion

    #region SelectMovieAsync Method
    protected async Task SelectMovie()
    {
        if (MovieObject != null)
        {
            await Shell.Current.GoToAsync($"{"SelectedMoviePage"}?movie={MovieObject.MovieId}");
        }
        
    }
    #endregion

}
