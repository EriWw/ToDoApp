using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Views;

namespace ToDoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new SignInPage());
            
        }

        
    }
}