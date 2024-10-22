using System.Windows;
using NNTP_NewsReader.Infrastructure;
using NNTP_NewsReader.Presentation;
using NNTP_NewsReader.Presentation.ViewModel;

namespace NNTP_NewsReader.Presentation.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
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
        string responseCode;
        // Connect to the NNTP server asynchronously
        NntpConnect connect = NntpConnect.Instance;
        responseCode = await connect.Connect("news.sunsite.dk", 119);

        // Now perform login after connection is established
        if (responseCode == "200")
        {
            await connect.Login("madknu01@easv365.dk", "b5353e");
        }
        else
        {
            Console.WriteLine($"{responseCode} NOT 200");
        }
    }
}