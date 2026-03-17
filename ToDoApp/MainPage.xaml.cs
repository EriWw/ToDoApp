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