using System.Windows;
using WPF_MVVM_TEMPLATE.Presentation.ViewModel;

namespace WPF_MVVM_TEMPLATE.Presentation.View;

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
        
    }
    
    
}