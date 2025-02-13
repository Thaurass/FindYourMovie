using Common.DataBase;

namespace FindYourMovie.Maui
{
    public partial class App : Application
    {
        
        public App()
        {
            InitializeComponent();
            CoreDB.CreateAndFillDB(MauiProgram.databasePath);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}