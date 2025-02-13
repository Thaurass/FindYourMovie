using Common.Library.Interfaces;
using FindYourMovie.DataLayer.DataClasses;
using FindYourMovie.EntityLayer.EntityClasses;
using FindYourMovie.Maui.CommandClasses;
using FindYourMovie.Maui.Views;
using Microsoft.Extensions.Logging;

namespace FindYourMovie.Maui
{
    public static class MauiProgram
    {
        public readonly static string databasePath = Path.Combine(FileSystem.AppDataDirectory, "movie_database.db");
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddScoped<IRepository<Movie>, MovieRepository>();
            builder.Services.AddScoped<IRepository<Actor>, ActorRepository>();

            builder.Services.AddScoped<SearchViewModelCommands>();
            builder.Services.AddScoped<SearchPage>();

            builder.Services.AddScoped<SelectedMoviePage>();
            

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
