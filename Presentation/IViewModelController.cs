using NNTP_NewsReader.Presentation.ViewModel;

namespace NNTP_NewsReader.Presentation;

public interface IViewModelController
{
    void RegistryViewModel(ViewModelBase viewModel);
    void UnRegistryViewModel(Type viewModelType);
    void SetCurrentViewModel(Type viewModelType);
    Dictionary<Type, ViewModelBase> GetAllViewModels();
}