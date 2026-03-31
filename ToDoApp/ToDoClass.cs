namespace ToDoApp;

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization; // <-- Added this for the API mapping

public class ToDoClass : INotifyPropertyChanged
{
    int _id;
    string _title;
    string _detail;
    string _status; // Added for API
    int _userId;    // Added for API

    [JsonPropertyName("item_id")] // Maps API "item_id" to your "id"
    public int id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    [JsonPropertyName("item_name")] // Maps API "item_name" to your "title"
    public string title
    {
        get => _title;
        set => SetField(ref _title, value);
    }

    [JsonPropertyName("item_description")] // Maps API "item_description" to your "detail"
    public string detail
    {
        get => _detail;
        set => SetField(ref _detail, value);
    }

    [JsonPropertyName("status")]
    public string status
    {
        get => _status;
        set => SetField(ref _status, value);
    }

    [JsonPropertyName("user_id")]
    public int userId
    {
        get => _userId;
        set => SetField(ref _userId, value);
    }

    // --- Your existing INotifyPropertyChanged logic remains completely untouched ---

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}