using WPF_MVVM_TEMPLATE.Presentation.ViewModel;

namespace WPF_MVVM_TEMPLATE.Presentation;

public interface IViewModelController
{
    void RegistryViewModel(ViewModelBase viewModel);
    void UnRegistryViewModel(Type viewModelType);
    void SetCurrentViewModel(Type viewModelType);
    Dictionary<Type, ViewModelBase> GetAllViewModels();
}