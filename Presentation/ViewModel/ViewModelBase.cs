using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WPF_MVVM_TEMPLATE.Presentation.View;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    
    protected ViewModelBase()
    {
        Registre(this);
    }

    private void Registre(ViewModelBase viewModelBase )
    {
        ViewModelController.Instance.RegistryViewModel(viewModelBase);
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