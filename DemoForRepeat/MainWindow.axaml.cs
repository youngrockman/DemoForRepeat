using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DemoForRepeat.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace DemoForRepeat;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void Authorized_Button(object? sender, RoutedEventArgs e)
    {
        var context = new DemoContext();
        var login = LoginBox.Text;
        var password = PasswordBox.Text;

        var user = context.Users.FirstOrDefault(x=> x.Email == login && x.UserPassword == password);
        
        
        
        
        if (user != null)
        {
            var productWindow = new ProductWindow( user.UserId);
            productWindow.Show();
            this.Close();
        }

        if (login == null || login == "" || password == "" || password == null || user == null)
        {
            var errorBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Неверный логин или пароль", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            errorBox.ShowAsync();

        }
        
    }
    
    private void CloseProgramm_Button(object? sender, RoutedEventArgs e)
    {
        Close(this);
    }
    
    
    
}