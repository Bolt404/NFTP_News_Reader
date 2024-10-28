using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NNTP_NewsReader.Application;
using NNTP_NewsReader.Entitys;
using NNTP_NewsReader.Infrastructure;

namespace NNTP_NewsReader.Presentation.View;

public partial class Connection : UserControl
{
    private static LoadConnectionInfo conInfo = new LoadConnectionInfo("conInfo.json");
    private ConnectionInfo info = conInfo.LoadInfo();
    public Connection()
    {
        InitializeComponent();

        if (info != null)
        {
            Console.WriteLine($"Loading connection info {info}");
            TextBoxServerAddress.Text = info.Host;
            TextBoxPort.Text = info.Port.ToString();
            TextBoxUserName.Text = info.Username;
            TextBoxPassword.Text = info.Password;
        }
    }
    
    private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text

    private void TextBox_NumbersOnly(object sender, TextCompositionEventArgs e)
    {
        //Checking if its a textbox otherwise Exception.
        TextBox textBox = sender as TextBox ?? throw new InvalidOperationException();
        {
            e.Handled = _regex.IsMatch(e.Text);
        }
    }

    private async void ButtonConnect_OnClick(object sender, RoutedEventArgs e)
    {
        SaveInfo();
        NntpConnect connection = NntpConnect.Instance;
        
        if (connection.isConnected)
        {
            Console.WriteLine("Already connected. Disconnecting first...");
            await connection.Disconnect(); // Disconnect if already connected
        }
        
        await connection.Connect(info.Host, info.Port);
        await connection.Login(info.Username, info.Password);
    }

    private void SaveInfo()
    {
        Console.WriteLine("Saving connection info");
        info = new ConnectionInfo
        {
            Host = TextBoxServerAddress.Text,
            Port = int.Parse(TextBoxPort.Text),
            Username = TextBoxUserName.Text,
            Password = TextBoxPassword.Text
        };
        conInfo.SaveUser(info);
    }
    
}