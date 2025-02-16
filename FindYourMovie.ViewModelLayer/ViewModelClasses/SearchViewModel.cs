using Common.DataBase;
using Common.Library.BaseClasses;
using Common.Library.Interfaces;
using FindYourMovie.EntityLayer.EntityClasses;
using System.Collections.ObjectModel;

namespace FindYourMovie.ViewModelLayer.ViewModelClasses;
public class SearchViewModel : CommonBase
{
    #region Constructors
    public SearchViewModel() : base()
    {
    }

    public SearchViewModel(IRepository<Movie> movierepo, IRepository<Actor> actorrepo) : base()
    {
        _MovieRepository = movierepo;
    }
    #endregion

    #region Private Variables
    private IRepository<Movie>? _MovieRepository;
    private ObservableCollection<Movie> _MovieList = [];
    private ObservableCollection<Actor> _ActorList = [];
    private ObservableCollection<string> _GenreList = [ "Научная фантастика", "Фэнтези", "Фантастика", "Драма", "Детектив", "Приключения" ];
    private Movie? _MovieObject = new();
    private Actor? _ActorObject = new();
    private string _Genre = string.Empty;
    private string _SearchText = string.Empty;
    #endregion

    #region Public Properties
    public Movie? MovieObject
    {
        get { return _MovieObject; }
        set
        {
            _MovieObject = value;
            RaisePropertyChanged(nameof(MovieObject));
        }
    }

    public ObservableCollection<Movie> MovieList
    {
        get { return _MovieList; }
        set
        {
            _MovieList = value;
            RaisePropertyChanged(nameof(MovieList));
        }
    }

    public Actor? ActorObject
    {
        get { return _ActorObject; }
        set
        {
            _ActorObject = value;
            RaisePropertyChanged(nameof(ActorObject));
        }
    }

    public ObservableCollection<Actor> ActorList
    {
        get { return _ActorList; }
        set
        {
            _ActorList = value;
            RaisePropertyChanged(nameof(ActorList));
        }
    }

    public string Genre
    {
        get { return _Genre; }
        set
        {
            _Genre = value;
            RaisePropertyChanged(nameof(Genre));
        }
    }

    public ObservableCollection<string> GenreList
    {
        get { return _GenreList; }
    }

    public string SearchText
    {
        get { return _SearchText; }
        set
        {
            _SearchText = value;
            RaisePropertyChanged(nameof(SearchText));
        }
    }
    #endregion

    #region Search Method
    public void SearchMovie(string databasePath)
    {
        if (ActorObject != null)
        {
            MovieList = _MovieRepository.Search(databasePath, SearchText, Genre, ActorObject.Name);
        }
        else
        {
            MovieList = _MovieRepository.Search(databasePath, SearchText, Genre, String.Empty);
        }
        

    }
    #endregion

    #region ClearFields Method
    public void ClearFields()
    {
        ActorObject = new(-1, String.Empty);
        Genre = string.Empty;
        SearchText = string.Empty;

    }
    #endregion

    
}
