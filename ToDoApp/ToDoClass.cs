namespace ToDoApp;

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class ToDoClass : INotifyPropertyChanged
{
    int _id;
    string _title;
    string _detail;

    public int id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public string title
    {
        get => _title;
        set => SetField(ref _title, value);
    }

    public string detail
    {
        get => _detail;
        set => SetField(ref _detail, value);
    }



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