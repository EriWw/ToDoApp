namespace ToDoApp.Views;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
    }

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        // Basic validation
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || 
            string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        // Add your registration/backend logic here
        await DisplayAlert("Success", "Account created successfully!", "OK");
        
        // Return to the Login Page
        await Navigation.PopAsync();
    }

    private async void OnLoginTapped(object sender, TappedEventArgs e)
    {
        // Go back to the previous page in the navigation stack (LoginPage)
        await Navigation.PopAsync();
    }
}