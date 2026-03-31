using ToDoApp.Services;
using ToDoApp.Views;

namespace ToDoApp;

public partial class SignInPage : ContentPage
{
    private readonly TodoApiService _apiService;

    public SignInPage()
    {
        InitializeComponent();
        _apiService = new TodoApiService();
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        // 1. Get text from the UI entries
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        // 2. Validate that they aren't empty
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Wait", "Please enter both email and password.", "OK");
            return;
        }

        // Optional: You could disable the button here to prevent double-clicking

        // 3. Send credentials to the API
        var loggedInUser = await _apiService.SignInAsync(email, password);

        // 4. Handle the result
        if (loggedInUser != null)
        {
            // SUCCESS! Save the User ID directly to the device so MainPage can use it
            Preferences.Default.Set("CurrentUserId", loggedInUser.Id);

            // Change the main window of the app to your MainPage
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
        else
        {
            // FAILURE! Show an error
            await DisplayAlert("Login Failed", "Incorrect email or password. Please try again.", "OK");
        }
    }

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }
}