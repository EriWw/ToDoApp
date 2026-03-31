using ToDoApp.Services;

namespace ToDoApp.Views;

public partial class SignUpPage : ContentPage
{
    private readonly TodoApiService _apiService;

    public SignUpPage()
    {
        InitializeComponent();
        _apiService = new TodoApiService();
    }

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        // 1. Get the text from the UI
        string fullName = NameEntry.Text?.Trim();
        string email = EmailEntry.Text?.Trim();
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        // 2. Validate the fields
        if (string.IsNullOrWhiteSpace(fullName) || 
            string.IsNullOrWhiteSpace(email) || 
            string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Wait", "Please fill in all fields.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        // 3. The API needs First and Last name separately. 
        // We will split the Full Name at the first space.
        string fname = fullName;
        string lname = ""; // Default empty
        
        int spaceIndex = fullName.IndexOf(' ');
        if (spaceIndex > 0)
        {
            fname = fullName.Substring(0, spaceIndex);
            lname = fullName.Substring(spaceIndex + 1);
        }

        // 4. Send the data to the API
        bool success = await _apiService.SignUpAsync(fname, lname, email, password);

        // 5. Handle the response
        if (success)
        {
            await DisplayAlert("Success", "Account created successfully! You can now log in.", "OK");
            
            // Send them back to the Sign In page
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Failed to create account. That email might already be taken.", "OK");
        }
    }

    private async void OnLoginTapped(object sender, EventArgs e)
    {
        // If they click "Already have an account? Login", just take them back a page
        await Navigation.PopAsync();
    }
}