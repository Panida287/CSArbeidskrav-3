using CommunityToolkit.Mvvm.ComponentModel;

namespace EventPlatform.Avalonia.ViewModels;

/// <summary>
/// Root ViewModel. Swaps CurrentPage to navigate between views.
/// </summary>
public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject _currentPage = new LoginViewModel();
}
