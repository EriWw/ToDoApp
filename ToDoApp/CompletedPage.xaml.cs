namespace ToDoApp;

public partial class CompletedPage : ContentPage
{
    public CompletedPage()
    {
        InitializeComponent();
        // Bind the ListView directly to our shared store
        completedLV.ItemsSource = TaskStore.CompletedTasks;
    }

    private void OnDeleteTapped(object sender, EventArgs e)
    {
        var tappedLabel = sender as Label;
        var itemToRemove = tappedLabel?.BindingContext as ToDoClass;

        if (itemToRemove != null)
        {
            TaskStore.CompletedTasks.Remove(itemToRemove);
        }
    }
}