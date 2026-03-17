using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Views;

namespace ToDoApp;

public partial class UserProfile : ContentPage
{
    public UserProfile()
    {
        InitializeComponent();
    }

    private async void SignOut(object? sender, EventArgs eventArgs)
    {


        await Shell.Current.GoToAsync("//LoginPage");


    }

   
}