using ToDoApp.Views;
using ToDoApp.Services; // Ensure this matches where you put TodoApiService.cs

namespace ToDoApp;

public partial class MainPage : ContentPage
{
    private readonly TodoApiService _apiService;
    private int _currentUserId = 3; // Hardcoded from API docs for now. Replace with actual logged-in user ID later.

    public MainPage()
    {
        InitializeComponent();
        
        _apiService = new TodoApiService();
        _currentUserId = Preferences.Default.Get("CurrentUserId", 0);
        
        // Keep your original binding!
        todoLV.ItemsSource = TaskStore.ActiveTasks;
    }

    // NEW: We need to fetch the active tasks from the database every time the page loads
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTasksFromDatabase();
    }

    private async Task LoadTasksFromDatabase()
    {
        var tasksFromDb = await _apiService.GetTasksAsync(_currentUserId, "active");
        
        TaskStore.ActiveTasks.Clear();
        foreach (var task in tasksFromDb)
        {
            TaskStore.ActiveTasks.Add(task);
        }
    }

    private async void OnAddTapped(object sender, EventArgs e)
    {
        string newTitle = await DisplayPromptAsync("New Task", "Enter the task title:");
        if (string.IsNullOrWhiteSpace(newTitle)) return;

        string newDescription = await DisplayPromptAsync("New Description", "Enter the task description:");
        if (newDescription == null) newDescription = ""; // Handle cancel vs empty

        // 1. Send it to the database FIRST. We no longer use nextId, the database assigns it!
        var newItem = await _apiService.AddTaskAsync(newTitle, newDescription, _currentUserId);

        // 2. If the database successfully created it, add it to your local TaskStore to update the UI
        if (newItem != null)
        {
            TaskStore.ActiveTasks.Add(newItem);
        }
        else
        {
            await DisplayAlert("Error", "Could not save task to the database.", "OK");
        }
    }

    private async void OnDeleteTapped(object sender, EventArgs e)
    {
        var tappedLabel = sender as Label;
        var itemToRemove = tappedLabel?.BindingContext as ToDoClass;

        if (itemToRemove != null)
        {
            // Optional: Ask for confirmation before deleting from database
            bool confirm = await DisplayAlert("Delete Task", $"Are you sure you want to delete '{itemToRemove.title}'?", "Yes", "No");
            if (!confirm) return;

            // 1. Tell the database to delete it
            bool success = await _apiService.DeleteTaskAsync(itemToRemove.id);

            // 2. If the database deleted it, remove it from the screen
            if (success)
            {
                TaskStore.ActiveTasks.Remove(itemToRemove);
            }
            else
            {
                await DisplayAlert("Error", "Could not delete task from the database.", "OK");
            }
        }
    }

    private async void OnEditTapped(object sender, EventArgs e)
    {
        var tappedLabel = sender as Label;
        var itemToEdit = tappedLabel?.BindingContext as ToDoClass;

        if (itemToEdit != null)
        {
            string updatedTitle = await DisplayPromptAsync(
                "Edit Task", 
                "Update the task title:", 
                initialValue: itemToEdit.title);

            if (string.IsNullOrWhiteSpace(updatedTitle)) return;

            string updatedDetails = await DisplayPromptAsync(
                "Edit Details", 
                $"Update details for '{updatedTitle}':", 
                initialValue: itemToEdit.detail);

            if (updatedDetails == null) return;

            // 1. Tell the database to update the item
            bool success = await _apiService.UpdateTaskAsync(itemToEdit.id, updatedTitle, updatedDetails);

            // 2. If the database successfully updated, update the local object so the UI refreshes
            if (success)
            {
                itemToEdit.title = updatedTitle;
                itemToEdit.detail = updatedDetails;
            }
            else
            {
                await DisplayAlert("Error", "Could not update task in the database.", "OK");
            }
        }
    }

    private async void OnCheckTapped(object sender, EventArgs e)
    {
        var tappedLabel = sender as Label;
        var itemToCheck = tappedLabel?.BindingContext as ToDoClass;

        if (itemToCheck != null)
        {
            // 1. Tell the database this item is now "inactive" (completed)
            bool success = await _apiService.ChangeTaskStatusAsync(itemToCheck.id, "inactive");

            // 2. If the database updated successfully, move it locally
            if (success)
            {
                itemToCheck.status = "inactive"; // Update the local status property
                TaskStore.ActiveTasks.Remove(itemToCheck);
                TaskStore.CompletedTasks.Add(itemToCheck);
            }
            else
            {
                await DisplayAlert("Error", "Could not mark task as completed in the database.", "OK");
            }
        }
    }
    private void OnLogOutClicked(object sender, EventArgs e)
    {
        // 1. Remove the saved User ID from the device's memory
        Preferences.Default.Remove("CurrentUserId");

        // Optional: Clear the active tasks so the next person logging in doesn't briefly see them
        TaskStore.ActiveTasks.Clear(); 
        TaskStore.CompletedTasks.Clear();

        // 2. Change the screen back to the Sign In page
        Application.Current.MainPage = new NavigationPage(new SignInPage());
    }
}
