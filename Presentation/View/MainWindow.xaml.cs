using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NNTP_NewsReader.Infrastructure;
using NNTP_NewsReader.Presentation;
using NNTP_NewsReader.Presentation.ViewModel;

namespace NNTP_NewsReader.Presentation.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly NntpConnect connection = NntpConnect.Instance;
    public MainWindow()
    {
        InitializeComponent();
        DataContext = ((App)App.Current);
        
        new AdminPanelViewModel();
        ViewModelController.Instance.SetCurrentViewModel(typeof(AdminPanelViewModel));
        
        InitializeAsync();
        
    }
    
    private async void InitializeAsync()
    {
        /*
        // Connect to the NNTP server asynchronously
        NntpConnect connect = NntpConnect.Instance;
        
        ENntpResponseCodes code = await connect.Connect("news.sunsite.dk", 119);
        
        //responseCode = connect.Connect("news.sunsite.dk", 119);

        // Now perform login after connection is established
        if (code == ENntpResponseCodes.ConnectionReady)
        {
            await connect.Login("madknu01@easv365.dk", "b5353e");
        }
        else
        {
            await connect.HandleResponse(code);
        }*/
    }

    private void MenuItemConnect_OnClick(object sender, RoutedEventArgs e)
    {
        new ConnectionViewModel();
        ViewModelController.Instance.SetCurrentViewModel(typeof(ConnectionViewModel));
    }

    private async void MenuItemDisconnect_OnClick(object sender, RoutedEventArgs e)
    {
        if (connection.isConnected)
        {
            await connection.Disconnect();
        }
        else
        {
            Console.WriteLine("No Connection to disconnect...");
        }
    }
}