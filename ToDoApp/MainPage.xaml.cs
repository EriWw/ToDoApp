using ToDoApp.Views;

namespace ToDoApp;

public partial class MainPage : ContentPage
{
    int nextId = 1;

    public MainPage()
    {
        InitializeComponent();
        

        todoLV.ItemsSource = TaskStore.ActiveTasks;
    }

    private async void OnAddTapped(object sender, EventArgs e)
    {
        string newTitle = await DisplayPromptAsync("New Task", "Enter the task title:");
        string newDescription = await DisplayPromptAsync("New Description", "Enter the task description:");
        if (!string.IsNullOrWhiteSpace(newTitle))
        {
            var newItem = new ToDoClass { id = nextId++, title = newTitle, detail = newDescription };
            TaskStore.ActiveTasks.Add(newItem);
        }
    }

    private void OnDeleteTapped(object sender, EventArgs e)
    {
        var tappedLabel = sender as Label;
        var itemToRemove = tappedLabel?.BindingContext as ToDoClass;

        if (itemToRemove != null)
        {
            TaskStore.ActiveTasks.Remove(itemToRemove);
        }
    }
    private async void OnEditTapped(object sender, EventArgs e)
    {
        // 1. Find out which item was tapped
        var tappedLabel = sender as Label;
        var itemToEdit = tappedLabel?.BindingContext as ToDoClass;

        if (itemToEdit != null)
        {
            // 2. Prompt for the Title, pre-filling the text box with the current title
            string updatedTitle = await DisplayPromptAsync(
                "Edit Task", 
                "Update the task title:", 
                initialValue: itemToEdit.title);

            // If they click Cancel, DisplayPromptAsync returns null. 
            // If they delete everything and click OK, it returns empty. Stop in both cases.
            if (string.IsNullOrWhiteSpace(updatedTitle)) return;

            // 3. Prompt for Details, pre-filling with the current details
            string updatedDetails = await DisplayPromptAsync(
                "Edit Details", 
                $"Update details for '{updatedTitle}':", 
                initialValue: itemToEdit.detail);

            // If they cancel the details prompt, abort the edit
            if (updatedDetails == null) return;

            // 4. Update the actual object
            // Because your ToDoClass uses INotifyPropertyChanged, the screen will update instantly!
            itemToEdit.title = updatedTitle;
            itemToEdit.detail = updatedDetails;
        }
    }

    private void OnCheckTapped(object sender, EventArgs e)
    {
        var tappedLabel = sender as Label;
        var itemToCheck = tappedLabel?.BindingContext as ToDoClass;

        if (itemToCheck != null)
        {
            // Moves it from this page to the CompletedPage
            TaskStore.ActiveTasks.Remove(itemToCheck);
            TaskStore.CompletedTasks.Add(itemToCheck);
        }
    }
}