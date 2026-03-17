using System.Collections.ObjectModel;

namespace ToDoApp;

public static class TaskStore
{
    // Holds the tasks that are still active (ToDo tab)
    public static ObservableCollection<ToDoClass> ActiveTasks { get; set; } = new ObservableCollection<ToDoClass>();

    // Holds the tasks that are finished (Completed tab)
    public static ObservableCollection<ToDoClass> CompletedTasks { get; set; } = new ObservableCollection<ToDoClass>();
}