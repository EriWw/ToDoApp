namespace ToDoApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        // Add your authentication logic here
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please enter both email and password.", "OK");
            return;
        }

      
        Application.Current.MainPage = new AppShell(); 
    }

    private async void OnSignUpTapped(object sender, TappedEventArgs e)
    {
        // Navigate to the Sign Up page
        await Navigation.PushAsync(new SignUpPage());
    }
}